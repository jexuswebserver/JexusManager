﻿// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.XPath;
using RollbarDotNet;
using Exception = System.Exception;

namespace Microsoft.Web.Administration
{
    public sealed class IisExpressServerManager : ServerManager
    {
        public Version Version { get; }

        public override bool SupportsSni => Version >=  Version.Parse("8.0") && Environment.OSVersion.Version >= Version.Parse("6.2");

        public IisExpressServerManager(string applicationHostConfigurationPath)
            : this(false, applicationHostConfigurationPath)
        {
        }

        public IisExpressServerManager(bool readOnly, string applicationHostConfigurationPath)
            : base(readOnly, applicationHostConfigurationPath)
        {
            Mode = WorkingMode.IisExpress;
            Version = Helper.GetIisExpressVersion();
        }

        internal override bool GetSiteState(Site site)
        {
            try
            {
                using (var process = new Process())
                {
                    var start = process.StartInfo;
                    start.Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated
                        ? "runas"
                        : null;
                    start.FileName = "cmd";
                    start.Arguments =
                        $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /config:\"{site.FileContext.FileName}\" /siteId:{site.Id}\"";
                    start.CreateNoWindow = true;
                    start.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                    process.WaitForExit();

                    return process.ExitCode == 1;
                }
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                {
                    Rollbar.Report(ex, ErrorLevel.Error, new Dictionary<string, object> {{"native", ex.NativeErrorCode}});
                    // throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.HResult != NativeMethods.NoProcessAssociated)
                {
                    Rollbar.Report(ex, ErrorLevel.Error, new Dictionary<string, object> { { "hresult", ex.HResult } });
                }
            }
            catch (Exception ex)
            {
                Rollbar.Report(ex, ErrorLevel.Error);
            }

            return true;
        }

        internal override bool GetPoolState(ApplicationPool pool)
        {
            return true;
        }

        internal override void Start(Site site)
        {
            StartInner(site, false);
        }

        private void StartInner(Site site, bool restart)
        {
            var name = site.Applications[0].ApplicationPoolName;
            var pool = ApplicationPools.FirstOrDefault(item => item.Name == name);
            var fileName =
                Path.Combine(
                    Environment.GetFolderPath(
                        pool != null && pool.Enable32BitAppOnWin64
                            ? Environment.SpecialFolder.ProgramFilesX86
                            : Environment.SpecialFolder.ProgramFiles),
                    "IIS Express",
                    "iisexpress.exe");
            if (!File.Exists(fileName))
            {
                fileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "IIS Express",
                    "iisexpress.exe");
            }

            var temp = Path.GetTempFileName();
            using (var process = new Process())
            {
                var start = process.StartInfo;
                start.Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated
                    ? "runas"
                    : null;
                start.FileName = "cmd";
                var extra = restart ? "/r" : string.Empty;
                start.Arguments =
                    $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /launcher:\"{fileName}\" /config:\"{site.FileContext.FileName}\" /siteId:{site.Id} /resultFile:\"{temp}\"\" {extra}";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                InjectEnvironmentVariables(site, start);

                try
                {
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode == 0)
                    {
                        site.State = ObjectState.Started;
                    }
                    else if (process.ExitCode == 1)
                    {
                        throw new InvalidOperationException("The process has terminated");
                    }
                }
                catch (Win32Exception ex)
                {
                    // elevation is cancelled.
                    if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                    {
                        throw new COMException(
                            $"cannot start site: {ex.Message}, {File.ReadAllText(temp)}");
                    }
                    
                    throw new COMException(
                        $"site start cancelled: {ex.Message}, {File.ReadAllText(temp)}");
                }
                catch (Exception ex)
                {
                    throw new COMException(
                        $"cannot start site: {ex.Message}, {File.ReadAllText(temp)}");
                }
                finally
                {
                    site.State = process.ExitCode == 0 ? ObjectState.Started : ObjectState.Stopped;
                }
            }
        }

        private static void InjectEnvironmentVariables(Site site, ProcessStartInfo startInfo)
        {
            // TODO: make this site extension method.
            var root = site.PhysicalPath.ExpandIisExpressEnvironmentVariables();
            string[] projects;
            try
            {
                projects = Directory.GetFiles(root, "*.csproj");
                if (projects.Length != 1)
                {
                    return;
                }
            }
            catch (IOException)
            {
                return;
            }

            var project = projects[0];
            var xml = XDocument.Load(project);
            if (xml.Root.Attribute("Sdk")?.Value != "Microsoft.NET.Sdk.Web")
            {
                // Not web project
                return;
            }

            var file = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe");
            var args = "-latest -products * -requires Microsoft.Component.MSBuild -property installationPath";
            if (!File.Exists(file))
            {
                // Not VS 15.2 and above
                return;
            }

            var vswhere = Process.Start(new ProcessStartInfo
            {
                FileName = file,
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false
            });
            var folder = vswhere.StandardOutput.ReadToEnd().TrimEnd();
            var dotnet = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\dotnet\dotnet.exe");
            var restore = Process.Start(new ProcessStartInfo
            {
                FileName = dotnet,
                Arguments = "restore",
                WorkingDirectory = root
            });
            restore.WaitForExit();
            var build = Process.Start(new ProcessStartInfo
            {
                FileName = dotnet,
                Arguments = "build",
                WorkingDirectory = root
            });
            build.WaitForExit();
            var files = Directory.GetFiles(Path.Combine(root, "bin", "Debug", xml.Root.XPathSelectElement("/Project/PropertyGroup/TargetFramework").Value), "*.dll");
            if (files.Length == 1)
            {
                var rootAssembly = files[0].Replace(@"\", @"\\");
                startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", $@"{folder}\Common7\IDE\Extensions\Microsoft\Web Tools\ProjectSystem\VSIISExeLauncher.exe");
                startInfo.EnvironmentVariables.Add("LAUNCHER_ARGS", $"-p \"{dotnet.Replace(@"\", @"\\")}\" -a \"exec \\\"{rootAssembly}\\\"\" -pidFile \"{Path.GetTempFileName().Replace(@"\", @"\\")}\" -wd \"{root.Replace(@"\", @"\\")}\"");
            }
        }

        internal override void Stop(Site site)
        {
            try
            {
                using (var process = new Process())
                {
                    var start = process.StartInfo;
                    start.Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated
                        ? "runas"
                        : null;
                    start.FileName = "cmd";
                    start.Arguments =
                        $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /k /config:\"{site.FileContext.FileName}\" /siteId:{site.Id}\"";
                    start.CreateNoWindow = true;
                    start.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        site.State = ObjectState.Stopped;
                    }
                }
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                {
                    Rollbar.Report(ex, ErrorLevel.Error, new Dictionary<string, object> {{ "native", ex.NativeErrorCode } });
                    // throw;
                }
            }
            catch (Exception ex)
            {
                Rollbar.Report(ex, ErrorLevel.Error);
            }
        }

        internal override void Restart(Site site)
        {
            StartInner(site, true);
        }

        internal override IEnumerable<string> GetSchemaFiles()
        {
            var directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                    "IIS Express",
                    "config",
                    "schema");
            if (Directory.Exists(directory))
            {
                return Directory.GetFiles(directory);
            }

            // IMPORTANT: for x86 IIS 7 Express
            var x86 = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                "IIS Express",
                "config",
                "schema");
            return Directory.Exists(x86) ? Directory.GetFiles(x86) : base.GetSchemaFiles();
        }
    }
}

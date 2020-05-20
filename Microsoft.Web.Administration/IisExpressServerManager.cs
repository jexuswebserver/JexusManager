﻿// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Xml.XPath;
using Rollbar;
using Exception = System.Exception;

namespace Microsoft.Web.Administration
{
    public sealed class IisExpressServerManager : ServerManager
    {
        public Version Version { get; }

        public string PrimaryExecutable { get; }

        public override bool SupportsSni => Version >=  Version.Parse("8.0") && Environment.OSVersion.Version >= Version.Parse("6.2");

        public override bool SupportsWildcard => Version >=  Version.Parse("10.0") && Environment.OSVersion.Version >= Version.Parse("10.0");
        
        public static bool ServerInstalled
        {
            get
            {
                return GetPrimaryExecutable() != null;
            }
        }

        public IisExpressServerManager(string applicationHostConfigurationPath)
            : this(false, applicationHostConfigurationPath)
        {
        }

        public IisExpressServerManager(bool readOnly, string applicationHostConfigurationPath)
            : base(readOnly, applicationHostConfigurationPath)
        {
            Mode = WorkingMode.IisExpress;
            PrimaryExecutable = GetPrimaryExecutable();
            Version = GetIisExpressVersion();
        }

        private static string GetPrimaryExecutable()
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "IIS Express");
            if (!Directory.Exists(directory))
            {
                // IMPORTANT: for x86 IIS 7 Express
                directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "IIS Express");
                if (!Directory.Exists(directory))
                {
                    return null;
                }
            }

            var fileName = Path.Combine(directory, "iisexpress.exe");
            return File.Exists(fileName) ? fileName : null;
        }

        internal override void SetPassword(VirtualDirectory virtualDirectory, string password)
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                "IIS Express");
            if (!Directory.Exists(directory))
            {
                // IMPORTANT: for x86 IIS 7 Express
                directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "IIS Express");
                if (!Directory.Exists(directory))
                {
                    // IMPORTANT: fallback to default password setting. Should throw encryption exception.
                    virtualDirectory.Password = password;
                    return;
                }
            }

            // IMPORTANT: save vdir to config file.
            CommitChanges();
            var appcmd = Path.Combine(directory, "appcmd.exe");
            if (!File.Exists(appcmd))
            {
                // IMPORTANT: fallback to default password setting. Should throw encryption exception.
                virtualDirectory.Password = password;
                return;
            }

            {
                var command = $"set vdir /vdir.name:\"{virtualDirectory.LocationPath()}\" /-password /apphostconfig:\"{FileName}\"";
                var resultFile = Path.GetTempFileName();
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /verb:appcmd /launcher:\"{appcmd}\" /resultFile:\"{resultFile}\" /input:\"{command}\"\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                try
                {
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        var message = File.ReadAllText(resultFile);
                        File.Delete(resultFile);
                        throw new Exception($"{process.ExitCode} {message}");                        
                    }
                }
                catch (Win32Exception ex)
                {
                    // elevation is cancelled.
                    if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                    {
                        RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                        // throw;
                    }
                }
            }

            {
                if (string.IsNullOrEmpty(password))
                {
                    return;
                }

                var command = $"set vdir /vdir.name:\"{virtualDirectory.LocationPath()}\" /password:{password} /apphostconfig:\"{FileName}\"";
                var resultFile = Path.GetTempFileName();
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /verb:appcmd /launcher:\"{appcmd}\" /resultFile:{resultFile} /input:\"{command}\"\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        Verb = "runas",
                        UseShellExecute = true
                    }
                };
                try
                {
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        var message = File.ReadAllText(resultFile);
                        File.Delete(resultFile);
                        throw new Exception($"{process.ExitCode} {message}");
                    }

                    var message1 = File.ReadAllText(resultFile);
                }
                catch (Win32Exception ex)
                {
                    // elevation is cancelled.
                    if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                    {
                        RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                        // throw;
                    }
                }
            }
        }

        private Version GetIisExpressVersion()
        {
            if (PrimaryExecutable != null && File.Exists(PrimaryExecutable))
            {
                if (Version.TryParse(FileVersionInfo.GetVersionInfo(PrimaryExecutable).ProductVersion, out Version result))
                {
                    return result;
                }
            }

            return Version.Parse("0.0.0.0");
        }

        internal override bool GetSiteState(Site site)
        {
            try
            {
                using var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated
                    ? "runas"
                    : null,
                        UseShellExecute = true,
                        FileName = "cmd",
                        Arguments =
                    $"/c \"\"{CertificateInstallerLocator.FileName}\" /config:\"{site.FileContext.FileName}\" /siteId:{site.Id}\"",
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                process.Start();
                process.WaitForExit();

                return process.ExitCode == 1;
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                {
                    RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> {{"native", ex.NativeErrorCode}});
                    // throw;
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.HResult != NativeMethods.NoProcessAssociated)
                {
                    RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "hresult", ex.HResult } });
                }
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
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
            if (site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated)
            {
                throw new InvalidOperationException("This site requires elevation. Please run Jexus Manager as administrator");
            }

            var actualExecutable = site.Applications[0].GetActualExecutable();
            var temp = Path.GetTempFileName();
            using var process = new Process();
            var start = process.StartInfo;
            start.FileName = "cmd";
            var extra = restart ? "/r" : string.Empty;
            start.Arguments =
                $"/c \"\"{CertificateInstallerLocator.FileName}\" /launcher:\"{actualExecutable}\" /config:\"{site.FileContext.FileName}\" /siteId:{site.Id} /resultFile:\"{temp}\"\" {extra}";
            start.CreateNoWindow = true;
            start.WindowStyle = ProcessWindowStyle.Hidden;
            InjectEnvironmentVariables(site, start, actualExecutable);

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

        private static void InjectEnvironmentVariables(Site site, ProcessStartInfo startInfo, string actualExecutable)
        {
            // TODO: make this site extension method.
            var root = site.PhysicalPath.ExpandIisExpressEnvironmentVariables(actualExecutable);
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
            Debug.Assert(xml.Root != null, "xml.Root != null");
            if (xml.Root.Attribute("Sdk")?.Value != "Microsoft.NET.Sdk.Web")
            {
                // Not web project
                return;
            }

            var dotnet = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\dotnet\dotnet.exe");
            var restore = Process.Start(new ProcessStartInfo
            {
                FileName = dotnet,
                Arguments = "restore",
                WorkingDirectory = root
            });
            Debug.Assert(restore != null, nameof(restore) + " != null");
            restore.WaitForExit();
            var build = Process.Start(new ProcessStartInfo
            {
                FileName = dotnet,
                Arguments = "build",
                WorkingDirectory = root
            });
            Debug.Assert(build != null, nameof(build) + " != null");
            build.WaitForExit();
            XElement framework = xml.Root.XPathSelectElement("/Project/PropertyGroup/TargetFramework");
            Debug.Assert(framework != null, nameof(framework) + " != null");
            var primary = Path.Combine(Path.Combine(root, "bin", "Debug", framework.Value), $"{Path.GetFileNameWithoutExtension(project)}.exe");
            if (File.Exists(primary))
            {
                // Shortcut for 3.1 apps. What about 2.2?
                startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", primary);
                return;
            }

            primary = Path.Combine(Path.Combine(root, "bin", "Debug", framework.Value), $"{Path.GetFileNameWithoutExtension(project)}.dll");
            if (!File.Exists(primary))
            {
                var files = Directory.GetFiles(Path.Combine(root, "bin", "Debug", framework.Value), "*.dll");
                if (files.Length == 1)
                {
                    primary = files[0];
                }
                else
                {
                    primary = null;
                }
            }

            if (primary == null)
            {
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
            Debug.Assert(vswhere != null, nameof(vswhere) + " != null");
            var folder = vswhere.StandardOutput.ReadToEnd().TrimEnd();

            var launcher = $@"{folder}\Common7\IDE\Extensions\Microsoft\Web Tools\ProjectSystem\VSIISExeLauncher.exe";
            if (!File.Exists(launcher))
            {
                return;
            }

            var rootAssembly = primary.Replace(@"\", @"\\");
            startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", $@"{folder}\Common7\IDE\Extensions\Microsoft\Web Tools\ProjectSystem\VSIISExeLauncher.exe");
            startInfo.EnvironmentVariables.Add("LAUNCHER_ARGS", $"-p \"{dotnet.Replace(@"\", @"\\")}\" -a \"exec \\\"{rootAssembly}\\\"\" -pidFile \"{Path.GetTempFileName().Replace(@"\", @"\\")}\" -wd \"{root.Replace(@"\", @"\\")}\"");
        }

        internal override void Stop(Site site)
        {
            try
            {
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated
                    ? "runas"
                    : null;
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments =
                    $"/c \"\"{CertificateInstallerLocator.FileName}\" /k /config:\"{site.FileContext.FileName}\" /siteId:{site.Id}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    site.State = ObjectState.Stopped;
                }
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != NativeMethods.ErrorCancelled)
                {
                    RollbarLocator.RollbarInstance.Error(ex,  new Dictionary<string, object> {{ "native", ex.NativeErrorCode } });
                    // throw;
                }
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
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

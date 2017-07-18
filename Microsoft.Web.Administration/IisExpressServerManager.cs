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
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Microsoft.Web.Administration
{
    public sealed class IisExpressServerManager : ServerManager
    {
        public IisExpressServerManager(bool readOnly, string applicationHostConfigurationPath)
            : base(readOnly, applicationHostConfigurationPath)
        {
            Mode = WorkingMode.IisExpress;
        }

        public IisExpressServerManager(string applicationHostConfigurationPath)
            : this(false, applicationHostConfigurationPath)
        {
        }

        internal override async Task<bool> GetSiteStateAsync(Site site)
        {
            using (var process = new Process())
            {
                var start = process.StartInfo;
                start.Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated ? "runas" : null;
                start.FileName = "cmd";
                start.Arguments =
                    $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /q:\"{site.CommandLine.Replace("\"", null)}\"\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                return process.ExitCode == 1;
            }
        }

        internal override async Task<bool> GetPoolStateAsync(ApplicationPool pool)
        {
            return true;
        }

        internal override async Task StartAsync(Site site)
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
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c \"\"{fileName}\" {site.CommandLine} > {temp}\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                Verb = site.Bindings.ElevationRequired && !PublicNativeMethods.IsProcessElevated ? "runas" : null
            };
            InjectEnvironmentVariables(site, startInfo);
            var process = new Process
            {
                StartInfo = startInfo
            };
            try
            {
                process.Start();
                process.WaitForExit(5000);
                if (process.HasExited)
                {
                    throw new InvalidOperationException("process terminated");
                }

                site.State = ObjectState.Started;
            }
            catch (Exception ex)
            {
                throw new COMException(
                    $"cannot start site: {ex.Message}, {File.ReadAllText(temp)}");
            }
            finally
            {
                site.State = process.HasExited ? ObjectState.Stopped : ObjectState.Started;
            }
        }

        private static void InjectEnvironmentVariables(Site site, ProcessStartInfo startInfo)
        {
            // TODO: make this site extension method.
            var root = site.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables();
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

        internal override async Task StopAsync(Site site)
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
                        $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /k:\"{site.CommandLine.Replace("\"", null)}\"\"";
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
            catch (Win32Exception)
            {}
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

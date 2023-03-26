// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
using Rollbar;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Microsoft.Web.Administration
{
    internal static class AspNetCoreHelper
    {
        public static void FixConfigFile(ServerManager serverManager)
        {
            foreach (Site site in serverManager.Sites)
            {
                FixConfigFile(site);
            }
        }

        private static void FixConfigFile(Site site)
        {
            // TODO: make this site extension method.
            var root = site.PhysicalPath.ExpandIisExpressEnvironmentVariables(null);
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

            var config = site.Server.GetApplicationHostConfiguration();
            var section = config.GetSection("system.webServer/aspNetCore", site.Name);
            if (section.RawAttributes.Count == 0)
            {
                section["processPath"] = "%LAUNCHER_PATH%";
                section["stdoutLogEnabled"] = false;
                section["hostingModel"] = "InProcess";
                section["startupTimeLimit"] = 3600;
                section["requestTimeout"] = TimeSpan.Parse("23:00:00");

                // TODO: found a bug in Remove.
                //var modules = config.GetSection("system.webServer/modules", site.Name);
                //var collectionModules = modules.GetCollection();
                //var found = collectionModules.FirstOrDefault(item => item["name"].ObjectToString() == "WebMatrixSupportModule");
                //if (found != null)
                //{
                //    collectionModules.Remove(found);
                //}

                var handlers = config.GetSection("system.webServer/handlers", site.Name);
                var collection = handlers.GetCollection();
                var add = collection.CreateElement("add");
                add["name"] = "aspNetCore";
                add["path"] = "*";
                add["verb"] = "*";
                add["modules"] = "AspNetCoreModuleV2";
                add["resourceType"] = "Unspecified";
                collection.Add(add);

                // TODO: cannot add this to the location tag?
                // var httpCompression = config.GetSection("system.webServer/httpCompression");
                // var types = httpCompression.GetCollection("dynamicTypes");
                // var type = types.CreateElement("add");
                // type["mimeType"] = "text/event-stream";
                // type["enabled"] = false;
                // types.Add(type);

                site.Server.CommitChanges();
            }
        }

        public static void InjectEnvironmentVariables(Site site, ProcessStartInfo startInfo, string actualExecutable)
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
            string input = framework.Value;
            int index = input.IndexOf("net");
            if (index == -1)
            {
                RollbarLocator.RollbarInstance.Error($"Unknown framework {input}");
                return;
            }

            string versionString = input.Substring(index + 3);
            var latestFramework = Version.TryParse(versionString, out Version version);
            var primary = Path.Combine(Path.Combine(root, "bin", "Debug", input), $"{Path.GetFileNameWithoutExtension(project)}.exe");
            var baseVersion = Version.Parse("5.0");
            if (File.Exists(primary)) // found default executables.
            {
                if (latestFramework)
                {
                    if (version >= baseVersion)
                    {
                        startInfo.EnvironmentVariables.Add("ANCM_LAUNCHER_PATH", primary); // New environment variable in ANCM since 5.0 preview.
                    }
                    else
                    {
                        RollbarLocator.RollbarInstance.Error($"impossible ASP.NET Core version {version}");
                    }
                }
                else
                {
                    // Shortcut for .NET Core 3.0/3.1 apps.
                    startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", primary); // To replace %LAUNCHER_PATH% in ".vs\xxx\applicationHost.config"
                }

                return;
            }

            // .NET Core 2.2 and below, special treatment.
            primary = Path.Combine(Path.Combine(root, "bin", "Debug", input), $"{Path.GetFileNameWithoutExtension(project)}.dll");
            if (!File.Exists(primary))
            {
                var files = Directory.GetFiles(Path.Combine(root, "bin", "Debug", input), "*.dll");
                if (files.Length == 1)
                {
                    RollbarLocator.RollbarInstance.Error($"Didn't find the expected assembly. Choose another instead.");
                    primary = files[0];
                }
                else
                {
                    primary = null;
                }
            }

            if (primary == null)
            {
                RollbarLocator.RollbarInstance.Error($"Didn't find compiled assembly for {input}");
                return;
            }

            var file = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe");
            var args = "-latest -products * -requires Microsoft.Component.MSBuild -property installationPath";
            if (!File.Exists(file))
            {
                // Not VS 15.2 and above
                RollbarLocator.RollbarInstance.Error($"Didn't detect VS 15.2 or above");
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
                RollbarLocator.RollbarInstance.Error($"Didn't detect VSIISExeLauncher");
                return;
            }

            var rootAssembly = primary.Replace(@"\", @"\\");
            var launcherArgs = $"-p \"{dotnet.Replace(@"\", @"\\")}\" -a \"exec \\\"{rootAssembly}\\\"\" -pidFile \"{Path.GetTempFileName().Replace(@"\", @"\\")}\" -wd \"{root.Replace(@"\", @"\\")}\"";
            startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", launcher);
            startInfo.EnvironmentVariables.Add("LAUNCHER_ARGS", launcherArgs);
        }
    }
}

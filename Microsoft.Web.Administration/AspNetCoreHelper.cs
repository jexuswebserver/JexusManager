// Copyright (c) Lex Li. All rights reserved. 
//  
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using JexusManager;
using Microsoft.Extensions.Logging;

namespace Microsoft.Web.Administration
{
    internal static class AspNetCoreHelper
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("AspNetCoreHelper");

        internal static void FixConfigFile(string fileName)
        {
            var doc = XDocument.Load(fileName);
            var virturalDirectories = doc.Descendants("virtualDirectory");
            var fixSection = false;
            foreach (var vDir in virturalDirectories)
            {
                string[] projects;
                try
                {
                    projects = Directory.GetFiles(vDir.Attribute("physicalPath").Value, "*.csproj");
                    if (projects.Length != 1)
                    {
                        continue;
                    }
                }
                catch (IOException)
                {
                    continue;
                }

                var project = projects[0];
                var xml = XDocument.Load(project);
                Debug.Assert(xml.Root != null, "xml.Root != null");
                var sdk = xml.Root.Attribute("Sdk")?.Value;
                if (sdk != "Microsoft.NET.Sdk.Web")
                {
                    // Not web project
                    continue;
                }

                fixSection = true;
                var siteName = vDir.Ancestors("site").Select(s => s.Attribute("name").Value).FirstOrDefault();
                FixSite(doc, siteName);
            }

            if (fixSection)
            {
                FixSection(doc);
                doc.Save(fileName);
            }
        }

        private static void FixSite(XDocument xml, string siteName)
        {
            // Find the <location> element with a "path" attribute equal to site name
            XElement locationElement = xml.Descendants("location")
                .FirstOrDefault(e => e.Attribute("path")?.Value == siteName);

            // Create a new <location> element for site name
            XElement newLocationElement = new XElement("location",
                new XAttribute("path", siteName),
                new XAttribute("inheritInChildApplications", "false"),
                new XElement("system.webServer",
                    new XElement("modules",
                        new XElement("remove", new XAttribute("name", "WebMatrixSupportModule"))),
                    new XElement("handlers",
                        new XElement("add",
                            new XAttribute("name", "aspNetCore"),
                            new XAttribute("path", "*"),
                            new XAttribute("verb", "*"),
                            new XAttribute("modules", "AspNetCoreModuleV2"),
                            new XAttribute("resourceType", "Unspecified"))),
                    new XElement("aspNetCore",
                        new XAttribute("processPath", "%LAUNCHER_PATH%"),
                        new XAttribute("stdoutLogEnabled", "false"),
                        new XAttribute("hostingModel", "InProcess"),
                        new XAttribute("startupTimeLimit", "3600"),
                        new XAttribute("requestTimeout", "23:00:00")),
                    new XElement("httpCompression",
                        new XElement("dynamicTypes",
                            new XElement("add",
                                new XAttribute("mimeType", "text/event-stream"),
                                new XAttribute("enabled", "false"))))));

            if (locationElement != null)
            {
                // Replace the existing <location> element with the new one
                locationElement.ReplaceWith(newLocationElement);
            }
            else
            {
                // Add the new <location> element to the <configuration> element
                xml.Element("configuration").Add(newLocationElement);
            }
        }

        private static void FixSection(XDocument xml)
        {
            // Find all descendant elements named "section"
            IEnumerable<XElement> sections = xml.Descendants("section");

            // Check if any of the "section" elements have a "name" attribute equal to "aspNetCore"
            bool aspNetCoreExists = sections.Any(s => s.Attribute("name")?.Value == "aspNetCore");

            if (!aspNetCoreExists)
            {
                // Create a new section element for "aspNetCore"
                XElement aspNetCoreSection = new XElement("section",
                    new XAttribute("name", "aspNetCore"),
                    new XAttribute("overrideModeDefault", "Allow"));

                // Find the sectionGroup element with the name "system.webServer"
                XElement sectionGroup = xml.Descendants("sectionGroup")
                    .FirstOrDefault(sg => sg.Attribute("name")?.Value == "system.webServer");

                if (sectionGroup != null)
                {
                    // Add the "aspNetCore" section element to the sectionGroup
                    sectionGroup.Add(aspNetCoreSection);
                }
            }

            // Find the <globalModules> element in <system.webServer>
            XElement globalModulesElement = xml.Descendants("system.webServer")
                .Elements("globalModules").FirstOrDefault();

            // Check if the <globalModules> element already contains an <add> element with name="AspNetCoreModuleV2"
            bool aspNetCoreModuleExists = globalModulesElement?.Descendants("add")
                .Any(e => e.Attribute("name")?.Value == "AspNetCoreModuleV2") ?? false;

            if (!aspNetCoreModuleExists)
            {
                // Create a new <add> element for AspNetCoreModuleV2
                XElement aspNetCoreModuleElement = new XElement("add",
                    new XAttribute("name", "AspNetCoreModuleV2"),
                    new XAttribute("image", "%IIS_BIN%\\Asp.Net Core Module\\V2\\aspnetcorev2.dll"));

                // Add the new <add> element to the <globalModules> element
                globalModulesElement.Add(aspNetCoreModuleElement);
            }

            // Find the <modules> element under <system.webServer> inside <location>
            XElement modulesElement = xml.Descendants("location")
                .Where(e => e.Attribute("path")?.Value == "")
                .Elements("system.webServer")
                .Elements("modules")
                .FirstOrDefault();

            // Check if the <modules> element already contains an <add> element with name="AspNetCoreModuleV2"
            bool aspNetCoreModuleExistsInLocation = modulesElement?.Descendants("add")
                .Any(e => e.Attribute("name")?.Value == "AspNetCoreModuleV2") ?? false;

            if (!aspNetCoreModuleExistsInLocation)
            {
                // Create a new <add> element for AspNetCoreModuleV2
                XElement aspNetCoreModuleElement = new XElement("add",
                    new XAttribute("name", "AspNetCoreModuleV2"));

                modulesElement.Add(aspNetCoreModuleElement);
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
            var sdk = xml.Root.Attribute("Sdk")?.Value;
            if (sdk == "Microsoft.NET.Sdk.BlazorWebAssembly")
            {
                // Blazor WASM
                string dotnet = RestoreAndBuild(root);
                string devServerVersion = xml.Descendants("PackageReference")
                    .Where(e => (string)e.Attribute("Include") == "Microsoft.AspNetCore.Components.WebAssembly.DevServer")
                    .Select(e => (string)e.Attribute("Version"))
                    .FirstOrDefault();
                if (devServerVersion == null)
                {
                    _logger.LogWarning("Blazor dev server missing in project {Project}", project);
                    return;
                }

                XElement framework = xml.Root.XPathSelectElement("/Project/PropertyGroup/TargetFramework");
                Debug.Assert(framework != null, nameof(framework) + " != null");
                string input = framework.Value;
                int index = input.IndexOf("net");
                if (index == -1)
                {
                    _logger.LogWarning("Unknown framework {Framework} in project {Project}", input, project);
                    return;
                }

                string versionString = input.Substring(index + 3);
                var latestFramework = Version.TryParse(versionString, out Version version);
                var primary = Path.Combine(Path.Combine(root, "bin", "Debug", input), $"{Path.GetFileNameWithoutExtension(project)}.dll");

                var devServerLibrary = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", "microsoft.aspnetcore.components.webassembly.devserver", devServerVersion, "tools", "blazor-devserver.dll");
                if (!File.Exists(devServerLibrary))
                {
                    _logger.LogWarning("Cannot find Blazor dev server library version {Version}", devServerVersion);
                    return;
                }

                var launcherArgs = $"\"{devServerLibrary}\" --applicationpath \"{primary}\"";
                startInfo.EnvironmentVariables.Add("ANCM_LAUNCHER_PATH", dotnet);
                startInfo.EnvironmentVariables.Add("ANCM_LAUNCHER_ARGS", launcherArgs);
            }
            else if (sdk == "Microsoft.NET.Sdk.Web")
            {
                string dotnet = RestoreAndBuild(root);
                XElement framework = xml.Root.XPathSelectElement("/Project/PropertyGroup/TargetFramework");
                Debug.Assert(framework != null, nameof(framework) + " != null");
                string input = framework.Value;
                int index = input.IndexOf("net");
                if (index == -1)
                {
                    _logger.LogWarning("Unknown framework {Framework} in project {Project}", input, project);
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
                            startInfo.EnvironmentVariables.Add("ASPNETCORE_ENVIRONMENT", "Development");
                        }
                        else
                        {
                            _logger.LogInformation("ASP.NET Core {Version} is older than baseline {Baseline}", version, baseVersion);
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
                        _logger.LogDebug("Expected assembly not found, using alternative: {Assembly}", files[0]);
                        primary = files[0];
                    }
                    else
                    {
                        primary = null;
                    }
                }

                if (primary == null)
                {
                    _logger.LogWarning("Could not find compiled assembly for framework {Framework} in {Path}", input, root);
                    return;
                }

                var file = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe");
                var args = "-latest -products * -requires Microsoft.Component.MSBuild -property installationPath";
                if (!File.Exists(file))
                {
                    // Not VS 15.2 and above
                    _logger.LogWarning("Visual Studio 15.2 or above not detected. VSWhere not found at {Path}", file);
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
                    _logger.LogWarning("VSIISExeLauncher not found at {Path}", launcher);
                    return;
                }

                var rootAssembly = primary.Replace(@"\", @"\\");
                var launcherArgs = $"-p \"{dotnet.Replace(@"\", @"\\")}\" -a \"exec \\\"{rootAssembly}\\\"\" -pidFile \"{Path.GetTempFileName().Replace(@"\", @"\\")}\" -wd \"{root.Replace(@"\", @"\\")}\"";
                startInfo.EnvironmentVariables.Add("LAUNCHER_PATH", launcher);
                startInfo.EnvironmentVariables.Add("LAUNCHER_ARGS", launcherArgs);
            }
        }

        private static string RestoreAndBuild(string root)
        {
            var dotnet = Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\dotnet\dotnet.exe");
            _logger.LogInformation("Running dotnet restore in {Path}", root);
            var restore = Process.Start(new ProcessStartInfo
            {
                FileName = dotnet,
                Arguments = "restore",
                WorkingDirectory = root
            });
            Debug.Assert(restore != null, nameof(restore) + " != null");
            restore.WaitForExit();

            _logger.LogInformation("Running dotnet build in {Path}", root);
            var build = Process.Start(new ProcessStartInfo
            {
                FileName = dotnet,
                Arguments = "build",
                WorkingDirectory = root
            });
            Debug.Assert(build != null, nameof(build) + " != null");
            build.WaitForExit();
            return dotnet;
        }
    }
}

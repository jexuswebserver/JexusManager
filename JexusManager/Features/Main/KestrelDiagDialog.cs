﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client.Win32;
using System;

namespace JexusManager.Features.Main
{
    using System.Drawing;
    using System.IO;

    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using System.Diagnostics;
    using JexusManager.Features.Modules;
    using Microsoft.Web.Management.Client;
    using JexusManager.Features.Handlers;
    using System.Collections.Generic;
    using EnumsNET;
    using System.Runtime.InteropServices;
    using JexusManager.Main.Properties;
    using Newtonsoft.Json;
    using System.Net;
    using NuGet.Versioning;
    using Microsoft.Extensions.Logging;

    public partial class KestrelDiagDialog : DialogForm
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("KestrelDiagDialog");
        private static IDictionary<SemanticVersion, Tuple<Version, bool>> mappings = new Dictionary<SemanticVersion, Tuple<Version, bool>>();
        private static IDictionary<string, string> fileCaches = new Dictionary<string, string>();

        public KestrelDiagDialog(IServiceProvider provider, Application application)
            : base(provider)
        {
            InitializeComponent();

            using (var client = new WebClient())
            {
                string latest = null;
                var hasException = false;
                try
                {
                    var entry = "https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/releases-index.json";
                    if (fileCaches.ContainsKey(entry))
                    {
                        latest = fileCaches[entry];
                    }
                    else
                    {
                        latest = client.DownloadString(entry);
                        fileCaches.Add(entry, latest);
                    }
                }
                catch (Exception)
                {
                    hasException = true;
                }

                JObject content;
                if (string.IsNullOrWhiteSpace(latest) || hasException)
                {
                    // fallback to resources.
                    using var bytes = new MemoryStream(Resources.releases_index);
                    using var stream = new StreamReader(bytes);
                    using var json = new JsonTextReader(stream);
                    content = (JObject)JToken.ReadFrom(json);
                }
                else
                {
                    content = JObject.Parse(latest);
                }

                var releases = content["releases-index"];
                foreach (var release in releases)
                {
                    var link = release["releases.json"].Value<string>();
                    string info = null;
                    hasException = false;
                    try
                    {
                        if (fileCaches.ContainsKey(link))
                        {
                            info = fileCaches[link];
                        }
                        else
                        {
                            info = client.DownloadString(link);
                            fileCaches.Add(link, info);
                        }
                    }
                    catch (Exception)
                    {
                        hasException = true;
                    }

                    JObject details;
                    if (string.IsNullOrWhiteSpace(info) || hasException)
                    {
                        // fallback to resources.
                        var number = new Version(release.Value<string>("channel-version"));
                        var name = $"{number.Major}.{number.Minor}-release";
                        var stored = Resources.ResourceManager.GetObject(name);
                        if (stored == null)
                        {
                            // IMPORTANT: didn't have this version in resource.
                            continue;
                        }

                        using var bytes = new MemoryStream((byte[])stored);
                        using var stream = new StreamReader(bytes);
                        using var json = new JsonTextReader(stream);
                        details = (JObject)JToken.ReadFrom(json);
                    }
                    else
                    {
                        details = JObject.Parse(info);
                    }

                    foreach (var actual in details["releases"])
                    {
                        var runtimeObject = actual["runtime"];
                        if (runtimeObject == null)
                        {
                            // skip no runtime release.
                            continue;
                        }

                        try
                        {
                            if (!runtimeObject.HasValues)
                            {
                                continue;
                            }

                            var longVersion = runtimeObject.Value<string>("version");
                            if (longVersion == null)
                            {
                                continue;
                            }

                            var aspNetRuntime = actual["aspnetcore-runtime"];
                            if (aspNetRuntime == null || !aspNetRuntime.HasValues)
                            {
                                continue;
                            }

                            var aspNetCoreModuleObject = aspNetRuntime["version-aspnetcoremodule"];
                            if (aspNetCoreModuleObject == null || !aspNetCoreModuleObject.HasValues)
                            {
                                // skip no ASP.NET Core module release.
                                continue;
                            }

                            var aspNetCoreModule = aspNetCoreModuleObject.Values<string>().First();
                            var phase = release.Value<string>("support-phase");
                            var expired = phase == "eol";
                            var runtime = SemanticVersion.Parse(longVersion);
                            if (mappings.ContainsKey(runtime))
                            {
                                Console.WriteLine($"{runtime}: new {aspNetCoreModule}: old {mappings[runtime].Item1}");
                            }
                            else
                            {
                                mappings.Add(runtime,
                                    new Tuple<Version, bool>(Version.Parse(aspNetCoreModule), expired));
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"JSON parsing failed. {ex}");
                        }
                    }
                }
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnGenerate, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtResult.Clear();
                    try
                    {
                        Warn("IMPORTANT: This report might contain confidential information. Mask such before sharing with others.");
                        Warn("-----");
                        Debug($"System Time: {DateTime.Now}");
                        Debug($"Processor Architecture: {Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}");
                        Debug($"OS: {Environment.OSVersion}");
                        Debug($"Server Type: {application.Server.Mode.AsString(EnumFormat.Description)}");
                        Debug(string.Empty);

                        var root = application.VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables(application.GetActualExecutable());
                        if (string.IsNullOrWhiteSpace(root))
                        {
                            Error("Invalid site root directory is detected.");
                            return;
                        }

                        // check ANCM.
                        var appHost = application.Server.GetApplicationHostConfiguration();
                        var definitions = new List<SectionDefinition>();
                        appHost.RootSectionGroup.GetAllDefinitions(definitions);
                        if (!definitions.Any(item => item.Path == "system.webServer/aspNetCore"))
                        {
                            Error($"ASP.NET Core module is not installed as part of IIS. Please refer to https://docs.microsoft.com/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle for more details.");
                            return;
                        }

                        var modules = new ModulesFeature((Module)provider);
                        modules.Load();
                        Debug($"Scan {modules.Items.Count} installed module(s).");
                        var hasV1 = modules.Items.FirstOrDefault(item => item.Name == "AspNetCoreModule");
                        var hasV2 = modules.Items.FirstOrDefault(item => item.Name == "AspNetCoreModuleV2");

                        if (hasV1 == null && hasV2 == null)
                        {
                            Error($"ASP.NET Core module is not installed as part of IIS. Please refer to https://docs.microsoft.com/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle for more details.");
                            return;
                        }

                        Version ancmVersion = null;
                        if (hasV2 != null)
                        {
                            var file = hasV2.GlobalModule.Image.ExpandIisExpressEnvironmentVariables(application.GetActualExecutable());
                            if (File.Exists(file))
                            {
                                var info = FileVersionInfo.GetVersionInfo(file);
                                ancmVersion = new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
                                Info($"ASP.NET Core module version 2 is installed for .NET Core 3.1 and above: {file} ({info.FileVersion}).");
                            }
                            else
                            {
                                Error("ASP.NET Core module version 2 is not installed properly.");
                            }
                        }
                        else
                        {
                            var file = hasV1.GlobalModule.Image.ExpandIisExpressEnvironmentVariables(application.GetActualExecutable());
                            if (File.Exists(hasV1.GlobalModule.Image.ExpandIisExpressEnvironmentVariables(application.GetActualExecutable())))
                            {
                                var info = FileVersionInfo.GetVersionInfo(file);
                                ancmVersion = new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
                                Info($"ASP.NET Core module version 1 is installed for .NET Core 1.0-2.1: {file} ({info.FileVersion})");
                            }
                            else
                            {
                                Error("ASP.NET Core module version 2 is not installed properly.");
                            }
                        }

                        // check handler.
                        Debug(string.Empty);
                        var handlers = new HandlersFeature((Module)provider);
                        handlers.Load();
                        var foundHandlers = new List<HandlersItem>();
                        Debug($"Scan {handlers.Items.Count} registered handler(s).");
                        foreach (var item in handlers.Items)
                        {
                            if (item.Modules == "AspNetCoreModule")
                            {
                                if (hasV1 != null)
                                {
                                    Info($"* Found a valid ASP.NET Core handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }}.");
                                    foundHandlers.Add(item);
                                }
                                else
                                {
                                    Error($"* Found an invalid ASP.NET Core handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }} because ASP.NET Core module version 1 is missing.");
                                }
                            }
                            else if (item.Modules == "AspNetCoreModuleV2")
                            {
                                if (hasV2 != null)
                                {
                                    Info($"* Found a valid ASP.NET Core handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }}.");
                                    foundHandlers.Add(item);
                                }
                                else
                                {
                                    Error($"* Found an invalid ASP.NET Core handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }} because ASP.NET Core module version 2 is missing.");
                                }
                            }
                        }

                        if (foundHandlers.Count == 0)
                        {
                            Error($"No valid ASP.NET Core handler is registered for this web site.");
                            Error($"To run ASP.NET Core on IIS, please refer to https://docs.microsoft.com/aspnet/core/host-and-deploy/iis/index for more details.");
                            return;
                        }
                        
                        var pool = application.GetPool();
                        if (pool == null)
                        {
                            Error($"The application pool '{application.ApplicationPoolName}' cannot be found.");
                            return;
                        }

                        var x86 = pool.Enable32BitAppOnWin64;

                        // check VC++ 2015.
                        var cppFile = Path.Combine(
                            Environment.GetFolderPath(x86 ? Environment.SpecialFolder.SystemX86 : Environment.SpecialFolder.System),
                            $"msvcp140.dll");
                        if (File.Exists(cppFile))
                        {
                            var cpp = FileVersionInfo.GetVersionInfo(cppFile);
                            if (cpp.FileMinorPart >= 0)
                            {
                                Info($"  Visual C++ runtime is detected (expected: 14.0, detected: {cpp.FileVersion}): {cppFile}.");
                            }
                            else
                            {
                                Error($"  Visual C++ runtime 14.0 is not detected. Please install it following the tips on https://docs.microsoft.com/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle.");
                            }
                        }
                        else
                        {
                            Error($"  Visual C++ 14.0 runtime is not detected. Please install it following the tips on https://docs.microsoft.com/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle.");
                        }

                        Info($"The application pool '{pool.Name}' is used.");
                        // check ASP.NET version.
                        if (pool.ManagedRuntimeVersion != ApplicationPool.ManagedRuntimeVersionNone)
                        {
                            Error($"The application pool '{pool.Name}' is using .NET CLR {pool.ManagedRuntimeVersion}. Please set it to 'No Managed Code'.");
                        }

                        Info($"Pool identity is {pool.ProcessModel}");
                        var user = application.Server.Mode == WorkingMode.IisExpress ? $"{Environment.UserDomainName}\\{Environment.UserName}" : pool.ProcessModel.ToString();
                        Warn($"Please ensure pool identity has read access to the content folder {application.PhysicalPath}.");

                        var poolBitness = x86 ? "32" : "64";
                        Info($"Pool bitness is {poolBitness} bit");

                        var config = application.GetWebConfiguration();
                        var section = config.GetSection("system.webServer/aspNetCore");
                        var processPath = (string)section["processPath"];
                        var arguments = (string)section["arguments"];
                        var hostingModel = (string)section["hostingModel"];

                        Debug($"Scan aspNetCore section.");
                        Debug($"    \"processPath\": {processPath}.");
                        Debug($"    \"arguments\": {arguments}.");
                        Debug($"    \"hostingModel\": {hostingModel}.");

                        if (string.Equals("InProcess", hostingModel, StringComparison.OrdinalIgnoreCase))
                        {
                            Warn("In-process hosting model is detected. To avoid 500.xx errors, make sure that the bitness of published artifacts matches the application pool bitness");

                            if (string.Equals("dotnet", processPath, StringComparison.OrdinalIgnoreCase))
                            {
                                Info("Framework dependent deployment is detected. Skip bitness check.");
                            }
                            else
                            {
                                Info("Self-contained deployment is detected. Check bitness.");
                                var path = processPath;
                                if (!File.Exists(path))
                                {
                                    path = Path.Combine(application.PhysicalPath, path);
                                }

                                if (!File.Exists(path))
                                {
                                    Error($"Cannot locate executable: {path}");
                                }
                                else
                                {
                                    var bit32 = DialogHelper.GetImageArchitecture(path);
                                    if (bit32 == x86)
                                    {
                                        Info($"Published artifacts bitness matches.");
                                    }
                                    else
                                    {
                                        var artifactBitness = bit32 ? "32" : "64";
                                        Error($"Published artifacts bitness is {artifactBitness} bit. Mismatch detected.");
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(processPath) && string.IsNullOrWhiteSpace(arguments))
                        {
                            Warn("There is no ASP.NET Core web app to analyze.");
                        }
                        else if (string.Equals(processPath, "%LAUNCHER_PATH%", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(arguments, "%LAUNCHER_ARGS%", StringComparison.OrdinalIgnoreCase))
                        {
                            Warn("Value of processPath or arguments is placeholder used by Visual Studio. This site can only be run from within Visual Studio.");
                        }
                        else
                        {
                            try
                            {
                                var fileName = Path.GetFileName(processPath);
                                string executable;
                                if (string.Equals(fileName, "dotnet.exe", StringComparison.OrdinalIgnoreCase) || string.Equals(fileName, "dotnet", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (arguments.StartsWith("exec ", StringComparison.OrdinalIgnoreCase))
                                    {
                                        arguments = arguments.Substring("exec ".Length).Replace("\"", null);
                                    }

                                    executable = Path.GetFileNameWithoutExtension(arguments);
                                }
                                else
                                {
                                    executable = Path.GetFileNameWithoutExtension(processPath);
                                }

                                var runtime = Path.Combine(root, executable + ".runtimeconfig.json");
                                if (File.Exists(runtime))
                                {
                                    Debug($"Found runtime config file {runtime}.");
                                    var reader = JObject.Parse(File.ReadAllText(runtime));
                                    string actual = null;
                                    var framework = reader["runtimeOptions"]["framework"];
                                    if (framework != null)
                                    {
                                        actual = framework["version"].Value<string>();
                                        Info($"Runtime is {actual}");
                                    }
                                    else
                                    {
                                        var frameworks = reader["runtimeOptions"]["includedFrameworks"];
                                        if (frameworks == null)
                                        {
                                            frameworks = reader["runtimeOptions"]["frameworks"]; // .NET 6
                                        }

                                        if (frameworks != null)
                                        {
                                            foreach (var item in frameworks.Children())
                                            {
                                                if (item["name"].Value<string>() == "Microsoft.AspNetCore.App")
                                                {
                                                    actual = item["version"].Value<string>();
                                                    Info($"Runtime is {actual}.");
                                                }
                                            }
                                        }
                                    }
                                    if (actual != null && SemanticVersion.TryParse(actual, out SemanticVersion aspNetCoreVersion) && aspNetCoreVersion >= SemanticVersion.Parse("3.1.0"))
                                    {
                                        if (aspNetCoreVersion.IsPrerelease)
                                        {
                                            Warn("This is a prerelease runtime version.");
                                        }

                                        if (mappings.ContainsKey(aspNetCoreVersion))
                                        {
                                            var expired = mappings[aspNetCoreVersion].Item2;
                                            if (expired)
                                            {
                                                Error($".NET Core version {aspNetCoreVersion} is end-of-life. Please upgrade to a supported version.");
                                            }

                                            var minimal = mappings[aspNetCoreVersion].Item1;
                                            if (ancmVersion == null || ancmVersion < minimal)
                                            {
                                                Error($"Runtime {aspNetCoreVersion} does not work with ASP.NET Core module version {ancmVersion}. Minimal version is {minimal}.");
                                            }

                                            if (ancmVersion > minimal && (ancmVersion.Major != minimal.Major || ancmVersion.Minor != minimal.Minor))
                                            {
                                                Warn($"Runtime {aspNetCoreVersion} requires ASP.NET Core module version {minimal}. Installed version {ancmVersion} might not be compatible.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Warn($"Couldn't detect runtime version {actual}. Please refer to pages such as https://dotnet.microsoft.com/download/dotnet-core/3.1 to verify that ASP.NET Core version {ancmVersion} matches the runtime of the web app.");
                                    }
                                }
                                else
                                {
                                    Error($"Cannot locate runtime config file {runtime}.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Error("Cannot analyze ASP.NET Core web app successfully.");
                                _logger.LogError(ex, "Error loading web.config of source web app");
                            }
                        }
                    }
                    catch (COMException ex)
                    {
                        Error("A generic exception occurred.");
                        Error($"To run ASP.NET Core on IIS, please refer to https://docs.microsoft.com/aspnet/core/host-and-deploy/iis/index for more details.");
                        Debug(ex.ToString());
                        _logger.LogDebug(ex.ToString());
                    }
                    catch (Exception ex)
                    {
                        Debug(ex.ToString());
                        _logger.LogDebug(ex.ToString());
                    }
                }));

           container.Add(
                Observable.FromEventPattern<EventArgs>(btnSave, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var fileName = DialogHelper.ShowSaveFileDialog(null, "Text Files|*.txt|All Files|*.*", application.GetActualExecutable());
                    if (string.IsNullOrEmpty(fileName))
                    {
                        return;
                    }
                    
                    File.WriteAllText(fileName, txtResult.Text);
                }));

           container.Add(
                Observable.FromEventPattern<EventArgs>(btnVerify, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtResult.Clear();
                }));
        }

        private void Debug(string text)
        {
            txtResult.AppendText(text);
            txtResult.AppendText(Environment.NewLine);
        }

        private void Error(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Red;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private void Warn(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Green;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private void Info(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Blue;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            DialogHelper.ProcessStart("https://docs.lextudio.com/jexusmanager/tutorials/ancm-diagnostics.html");
        }

        private void SslDiagDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnHelpClick(null, EventArgs.Empty);
        }
    }
}

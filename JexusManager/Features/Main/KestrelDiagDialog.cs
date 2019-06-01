// Copyright (c) Lex Li. All rights reserved.
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

    public partial class KestrelDiagDialog : DialogForm
    {
        private static IDictionary<Version, Version> mappings = new Dictionary<Version, Version> {
            { Version.Parse("2.2.5"), Version.Parse("12.2.19109.5") },
            { Version.Parse("2.2.4"), Version.Parse("12.2.19048.0") },
            { Version.Parse("2.2.3"), Version.Parse("12.2.19024.2") },
            { Version.Parse("2.2.2"), Version.Parse("12.2.18346.0") },
            { Version.Parse("2.2.1"), Version.Parse("12.2.18316.0") },
            { Version.Parse("2.2.0"), Version.Parse("12.2.18316.0") },
            { Version.Parse("2.1.11"), Version.Parse("12.1.19108.11") },
            { Version.Parse("2.1.10"), Version.Parse("12.1.18263.2") },
            { Version.Parse("2.1.9"), Version.Parse("12.1.19046.9") },
            { Version.Parse("2.1.8"), Version.Parse("12.1.18263.2") },
            { Version.Parse("2.1.7"), Version.Parse("12.1.18263.2") },
            { Version.Parse("2.1.6"), Version.Parse("12.1.18263.2") },
            { Version.Parse("2.1.5"), Version.Parse("8.2.1991.0") },
            { Version.Parse("2.1.4"), Version.Parse("8.2.1991.0") },
            { Version.Parse("2.1.3"), Version.Parse("8.2.1991.0") },
            { Version.Parse("2.1.2"), Version.Parse("8.2.1991.0") },
            { Version.Parse("2.1.1"), Version.Parse("8.2.1991.0") },
            { Version.Parse("2.1.0"), Version.Parse("8.2.1991.0") }
        };

        public KestrelDiagDialog(IServiceProvider provider, Application application)
            : base(provider)
        {
            InitializeComponent();

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
                        Warn("IMPORTANT: This report might contain confidential information. Mask such before sharing to others.");
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
                        if (!definitions.Any(item => item.Name == "system.webServer/aspNetCore"))
                        {
                            Error($"ASP.NET Core module is not installed as part of IIS. Please refer to https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle for more details.");
                            return;
                        }

                        var modules = new ModulesFeature((Module)provider);
                        modules.Load();
                        Debug($"Scan {modules.Items.Count} installed module(s).");
                        var hasV1 = modules.Items.FirstOrDefault(item => item.Name == "AspNetCoreModule");
                        var hasV2 = modules.Items.FirstOrDefault(item => item.Name == "AspNetCoreModuleV2");

                        if (hasV1 == null && hasV2 == null)
                        {
                            Error($"ASP.NET Core module is not installed as part of IIS. Please refer to https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle for more details.");
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
                                Info($"ASP.NET Core module version 2 is installed for .NET Core 2.2 and above: {file} ({info.FileVersion}).");
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
                            Error($"To run ASP.NET Core on IIS, please refer to https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/index for more details.");
                            return;
                        }
                        
                        var name = application.ApplicationPoolName;
                        var pool = application.Server.ApplicationPools.FirstOrDefault(item => item.Name == name);
                        if (pool == null)
                        {
                            Error($"The application pool '{name}' cannot be found.");
                        }
                        else
                        {
                            // check VC++ 2015.
                            var x86 = pool.Enable32BitAppOnWin64;
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
                                    Error($"  Visual C++ runtime 14.0 is not detected. Please install it following the tips on https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle.");
                                }
                            }
                            else
                            {
                                Error($"  Visual C++ 14.0 runtime is not detected. Please install it following the tips on https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/index#install-the-net-core-hosting-bundle.");
                            }

                            // check ASP.NET version.
                            if (pool.ManagedRuntimeVersion != ApplicationPool.ManagedRuntimeVersionNone)
                            {
                                Error($"The application pool '{name}' is using .NET CLR {pool.ManagedRuntimeVersion}. Please set it to 'No Managed Code'.");
                            }
                        }

                        var config = application.GetWebConfiguration();
                        var section = config.GetSection("system.webServer/aspNetCore");
                        var processPath = (string)section["processPath"];
                        var arguments = (string)section["arguments"];
                        var hostingModel = (string)section["hostingModel"];

                        Debug($"Scan aspNetCore section.");
                        Debug($"    \"processPath\": {processPath}.");
                        Debug($"    \"arguments\": {arguments}.");
                        Debug($"    \"hostingModel\": {hostingModel}.");

                        if (string.IsNullOrWhiteSpace(processPath) && string.IsNullOrWhiteSpace(arguments))
                        {
                            Warn("There is no ASP.NET Core web app to analyze.");
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

                                var runtime = Path.Combine(root, executable + ".deps.json");
                                if (File.Exists(runtime))
                                {
                                    var reader = JObject.Parse(File.ReadAllText(runtime));
                                    var targetName = (string)reader["runtimeTarget"]["name"];
                                    Debug($"\"runtimeTarget\": {targetName}.");
                                    var slash = targetName.IndexOf('/');
                                    if (slash > -1)
                                    {
                                        targetName = targetName.Substring(0, slash);
                                    }

                                    var actual = reader["targets"][targetName];
                                    Version aspNetCoreVersion = null;
                                    foreach (var item in actual.Children())
                                    {
                                        if (item is JProperty prop)
                                        {
                                            if (prop.Name.Contains("Microsoft.AspNetCore.All/"))
                                            {
                                                Info($"Runtime is {prop.Name}.");
                                                Version.TryParse(prop.Name.Substring(prop.Name.IndexOf('/') + 1), out aspNetCoreVersion);
                                            }
                                            else if (prop.Name.Contains("Microsoft.AspNetCore.App/"))
                                            {
                                                Info($"Runtime is {prop.Name}.");
                                                Version.TryParse(prop.Name.Substring(prop.Name.IndexOf('/') + 1), out aspNetCoreVersion);
                                            }
                                        }
                                    }

                                    if (aspNetCoreVersion != null && aspNetCoreVersion > Version.Parse("2.1.0"))
                                    {
                                        if (mappings.ContainsKey(aspNetCoreVersion))
                                        {
                                            var minimal = mappings[aspNetCoreVersion];
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
                                }
                                else
                                {
                                    Error($"Cannot locate runtime config file {runtime}.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Error("Cannot analyze ASP.NET Core web app successfully.");
                                Rollbar.RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "source", "web app" } });
                            }
                        }

                        Warn($"Please refer to pages such as https://dotnet.microsoft.com/download/dotnet-core/2.2 to verify that ASP.NET Core version {ancmVersion} matches the runtime of the web app.");
                    }
                    catch (COMException ex)
                    {
                        Error("A generic exception occurred.");
                        Error($"To run ASP.NET Core on IIS, please refer to https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/index for more details.");
                        Debug(ex.ToString());
                        Rollbar.RollbarLocator.RollbarInstance.Error(ex);
                    }
                    catch (Exception ex)
                    {
                        Debug(ex.ToString());
                        Rollbar.RollbarLocator.RollbarInstance.Error(ex);
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
            DialogHelper.ProcessStart("http://www.jexusmanager.com/en/latest/tutorials/vs-diagnostics.html");
        }

        private void SslDiagDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnHelpClick(null, EventArgs.Empty);
        }
    }
}

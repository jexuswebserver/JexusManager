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
    using Microsoft.Win32;

    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using System.Diagnostics;
    using JexusManager.Features.Modules;
    using Microsoft.Web.Management.Client;
    using JexusManager.Features.Handlers;
    using System.Collections.Generic;

    public partial class KestrelDiagDialog : DialogForm
    {
        public KestrelDiagDialog(IServiceProvider provider, Site site)
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

                        var root = site.PhysicalPath.ExpandIisExpressEnvironmentVariables(site.Applications[0].GetActualExecutable());
                        if (string.IsNullOrWhiteSpace(root))
                        {
                            Error("Invalid site root directory is detected.");
                            return;
                        }

                        // check ANCM.
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
                        else if (hasV2 != null)
                        {
                            var file = hasV2.GlobalModule.Image.ExpandIisExpressEnvironmentVariables(site.Applications[0].GetActualExecutable());
                            if (File.Exists(file))
                            {
                                var info = FileVersionInfo.GetVersionInfo(file);
                                Debug($"ASP.NET Core module version 2 is installed for .NET Core 2.2 and above: {file} ({info.FileVersion}).");
                                Warn($"Please refer to pages such as https://dotnet.microsoft.com/download/dotnet-core/2.2 to verify that the version number {info.FileVersion} matches the runtime of the web app.");
                            }
                            else
                            {
                                Error("ASP.NET Core module version 2 is not installed properly.");
                            }
                        }
                        else
                        {
                            var file = hasV1.GlobalModule.Image.ExpandIisExpressEnvironmentVariables(site.Applications[0].GetActualExecutable());
                            if (File.Exists(hasV1.GlobalModule.Image.ExpandIisExpressEnvironmentVariables(site.Applications[0].GetActualExecutable())))
                            {
                                var info = FileVersionInfo.GetVersionInfo(file);
                                Debug($"ASP.NET Core module version 1 is installed for .NET Core 1.0-2.1: {file} ({info.FileVersion})");
                                Warn($"Please refer to pages such as https://dotnet.microsoft.com/download/dotnet-core/2.2 to verify that the version number {info.FileVersion} matches the runtime of the web app.");
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
                                    Debug($"* Found a valid ASP.NET Core handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }}.");
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
                                    Debug($"* Found a valid ASP.NET Core handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }}.");
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
                            Error($"To run ASP.NET Core on IIS, please refer to https://docs.microsoft.com/en-us/iis/application-frameworks/scenario-build-a-php-website-on-iis/configuring-step-1-install-iis-and-php#13-download-and-install-php-manually for more details.");
                            return;
                        }
                        
                        var name = site.Applications[0].ApplicationPoolName;
                        var pool = site.Server.ApplicationPools.FirstOrDefault(item => item.Name == name);
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
                                    Debug($"  Visual C++ runtime is detected (expected: 14.0, detected: {cpp.FileVersion}): {cppFile}.");
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
                    var fileName = DialogHelper.ShowSaveFileDialog(null, "Text Files|*.txt|All Files|*.*", site.Applications[0].GetActualExecutable());
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

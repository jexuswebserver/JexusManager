// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client.Win32;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace JexusManager.Features.Main
{
    using System.Drawing;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Win32;

    using Org.BouncyCastle.Utilities.Encoders;

    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Collections.Generic;
    using Microsoft.Web.Management.Client;
    using JexusManager.Features.FastCgi;
    using JexusManager.Features.Handlers;
    using System.Diagnostics;

    public partial class PhpDiagDialog : DialogForm
    {
        public PhpDiagDialog(IServiceProvider provider, ServerManager server)
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
                        Debug($"System Time: {DateTime.Now}");
                        Debug($"Processor Architecture: {Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}");
                        Debug($"OS: {Environment.OSVersion}");
                        Debug($"{server.Type}");
                        Debug(Environment.NewLine);

                        var handlers = new HandlersFeature((Module)provider);
                        handlers.Load();
                        var foundPhpHandler = false;
                        Debug($"Scan {handlers.Items.Count} registered handler(s).");
                        foreach (var item in handlers.Items)
                        {
                            if (string.Equals(item.Path, "*.php", StringComparison.OrdinalIgnoreCase))
                            {
                                Debug($"* Found PHP handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Handler: {item.TypeString}, Entry Type: {item.Flag} }}.");
                                foundPhpHandler = true;
                            }
                        }

                        if (!foundPhpHandler)
                        {
                            Error($"No PHP handler is registered for this web site. Please refer to https://docs.microsoft.com/en-us/iis/application-frameworks/scenario-build-a-php-website-on-iis/configuring-step-1-install-iis-and-php#13-download-and-install-php-manually for more details.");
                            return;
                        }

                        Debug(Environment.NewLine);
                        var fastCgiFeature = new FastCgiFeature((Module)provider);
                        fastCgiFeature.Load();
                        Debug($"Scan {fastCgiFeature.Items.Count} registered FastCGI application(s).");
                        var foundPhp = new List<string>();
                        foreach (var item in fastCgiFeature.Items)
                        {
                            if (item.Path.TrimEnd('"').EndsWith("php-cgi.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                Debug($"* Found PHP FastCGI application registered as {{ Full path: {item.Path}, Arguments: {item.Arguments} }}.");
                                foundPhp.Add(item.Path);
                            }
                        }

                        if (foundPhp.Count == 0)
                        {
                            Error($"No PHP FastCGI appilcation is registered on this server. Please refer to https://docs.microsoft.com/en-us/iis/application-frameworks/scenario-build-a-php-website-on-iis/configuring-step-1-install-iis-and-php#13-download-and-install-php-manually for more details.");
                            return;
                        }

                        Debug(Environment.NewLine);
                        Debug($"Verify PHP installation versions.");
                        foreach (var path in foundPhp)
                        {
                            var info = FileVersionInfo.GetVersionInfo(path);
                            if (info.FileMajorPart < 5)
                            {
                                Error($"* PHP {info.FileVersion} ({path}) is obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                            }
                            else if (info.FileMajorPart == 5)
                            {
                                if (info.FileMinorPart < 6)
                                {
                                    Error($"* PHP {info.FileVersion} ({path}) is obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                                }
                                else if (info.FileMinorPart == 6)
                                {
                                    Warn($"* PHP {info.FileVersion} ({path}) will soon be obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                                }
                                else
                                {
                                    Error($"* PHP {info.FileVersion} ({path}) is unknown. Please refer to http://php.net/supported-versions.php for more details.");
                                }
                            }
                            else if (info.FileMajorPart == 7)
                            {
                                if (info.FileMinorPart == 0)
                                {
                                    Warn($"* PHP {info.FileVersion} ({path}) will soon be obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                                }
                                else
                                {
                                    Debug($"* PHP {info.FileVersion} ({path}) is supported. Please refer to http://php.net/supported-versions.php for more details.");
                                }
                            }
                            else
                            {
                                Error($"* PHP {info.FileVersion} ({path}) is unknown. Please refer to http://php.net/supported-versions.php for more details.");
                            }
                        }

                        Debug(Environment.NewLine);
                        var systemPath = Environment.GetEnvironmentVariable("PATH");
                        Debug($"Scan Windows PATH: {systemPath}.");
                        string[] paths = systemPath.Split(new char[1] { Path.PathSeparator });
                        foreach (var path in foundPhp)
                        {
                            var folder = Path.GetDirectoryName(path);
                            var matched = false;
                            foreach (var system in paths)
                            {
                                if (string.Equals(folder, system, StringComparison.OrdinalIgnoreCase))
                                {
                                    matched = true;
                                    break;
                                }
                            }

                            if (matched)
                            {
                                Debug($"* PHP installation {folder} has been added to Windows PATH.");
                            }
                            else
                            {
                                Error($"* PHP installation {folder} is not yet added to Windows PATH.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug(ex.ToString());
                        RollbarDotNet.Rollbar.Report(ex);
                    }
                }));

           container.Add(
                Observable.FromEventPattern<EventArgs>(btnSave, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var fileName = DialogHelper.ShowSaveFileDialog(null, "Text Files|*.txt|All Files|*.*");
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
            DialogHelper.ProcessStart("http://www.jexusmanager.com/en/latest/tutorials/php-diagnostics.html");
        }

        private void SslDiagDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnHelpClick(null, EventArgs.Empty);
        }
    }
}

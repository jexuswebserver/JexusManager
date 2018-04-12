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
    using IniParser.Parser;

    public partial class PhpDiagDialog : DialogForm
    {
        private struct PhpVersion
        {
            public PhpVersion(string version, bool recommended, Version cppVersion)
            {
                Version = version;
                Recommended = recommended;
                CppVersion = cppVersion;
            }

            public string Version { get; set; }
            public bool Recommended { get; set; }
            public Version CppVersion { get; set; }
        }

        public PhpDiagDialog(IServiceProvider provider, ServerManager server)
            : base(provider)
        {
            InitializeComponent();

            var knownPhpVersions = new Dictionary<string, PhpVersion>
            {
                { "5.6", new PhpVersion("5.6", false, new Version(11, 0)) },
                { "7.0", new PhpVersion("7.0", false, new Version(14, 0)) },
                { "7.1", new PhpVersion("7.1", true, new Version(14, 0)) },
                { "7.2", new PhpVersion("7.2", true, new Version(14, 11)) }
            };

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
                        Debug(string.Empty);

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

                        Debug(string.Empty);
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
                            var version = $"{info.FileMajorPart}.{info.FileMinorPart}";
                            if (knownPhpVersions.ContainsKey(version))
                            {
                                var php = knownPhpVersions[version];
                                if (php.Recommended)
                                {
                                    Debug($"* PHP {version} ({path}) is supported.");
                                }
                                else
                                {
                                    Warn($"* PHP {version} ({path}) will soon be obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                                }

                                var cppFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), $"msvcp{php.CppVersion.Major}0.dll");
                                if (File.Exists(cppFile))
                                {
                                    var cpp = FileVersionInfo.GetVersionInfo(cppFile);
                                    if (cpp.FileMinorPart >= php.CppVersion.Minor)
                                    {
                                        Debug($"  Visual C++ runtime is detected (expected: {php.CppVersion}, detected: {cpp.FileVersion}).");
                                    }
                                    else
                                    {
                                        Error($"  Visual C++ runtime {php.CppVersion} is not detected. Please install it following the tips on https://windows.php.net/download/.");
                                    }
                                }
                                else
                                {
                                    Error($"  Visual C++ {php.CppVersion} runtime is not detected. Please install it following the tips on https://windows.php.net/download/.");
                                }
                            }
                            else
                            {
                                Error($"* PHP {info.FileVersion} ({path}) is unknown or obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                            }
                        }

                        Debug(string.Empty);
                        var systemPath = Environment.GetEnvironmentVariable("Path");
                        Debug($"Windows Path environment variable: {systemPath}.");
                        Debug(string.Empty);
                        string[] paths = systemPath.Split(new char[1] { Path.PathSeparator });
                        foreach (var path in foundPhp)
                        {
                            var rootFolder = Path.GetDirectoryName(path);
                            Debug($"[{rootFolder}]");
                            var config = Path.Combine(rootFolder, "php.ini");
                            if (File.Exists(config))
                            {
                                Info($"Found PHP config file {config}.");
                                var parser = new ConcatenateDuplicatedKeysIniDataParser();
                                parser.Configuration.ConcatenateSeparator = " ";
                                var data = parser.Parse(File.ReadAllText(config));
                                var extensionFolder = data["PHP"]["extension_dir"];
                                if (extensionFolder == null)
                                {
                                    extensionFolder = "ext";
                                }

                                var fullPath = Path.Combine(rootFolder, extensionFolder);
                                Info($"PHP loadable extension folder: {fullPath}");
                                var extesionNames = data["PHP"]["extension"];
                                if (extesionNames == null)
                                {
                                    Info("No extension to verify.");
                                }
                                else
                                {
                                    var extensions = extesionNames.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    Info($"Found {extensions.Length} extension(s) to verify.");
                                    var noError = true;
                                    foreach (var name in extensions)
                                    {
                                        var fileName = Path.Combine(fullPath, $"php_{name}.dll");
                                        if (!File.Exists(fileName))
                                        {
                                            Error($"* Extension {name} is listed, but on disk the file cannot be found {fileName}");
                                            noError = false;
                                        }
                                    }

                                    if (noError)
                                    {
                                        Info("All extension(s) listed can be found on disk.");
                                    }
                                }
                            }
                            else
                            {
                                Warn($"Cannot find PHP config file {config}. Default settings are used.");
                            }

                            var matched = false;
                            foreach (var system in paths)
                            {
                                if (string.Equals(rootFolder, system, StringComparison.OrdinalIgnoreCase))
                                {
                                    matched = true;
                                    break;
                                }
                            }

                            if (matched)
                            {
                                Debug($"PHP installation has been added to Windows Path environment variable.");
                            }
                            else
                            {
                                Error($"PHP installation is not yet added to Windows Path environment variable. Please refer to https://docs.microsoft.com/en-us/iis/application-frameworks/scenario-build-a-php-website-on-iis/configuring-step-1-install-iis-and-php#13-download-and-install-php-manually for more details.");
                                Warn($"Restart Jexus Manager and rerun PHP Diagnostics after changing Windows Path environment variable.");
                            }

                            Debug(string.Empty);
                        }

                        // TODO: verify other configurations in php.info.
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

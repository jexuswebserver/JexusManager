// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client.Win32;
using System;
using System.Linq;

namespace JexusManager.Features.Main
{
    using System.Drawing;
    using System.IO;

    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Collections.Generic;
    using Microsoft.Web.Management.Client;
    using JexusManager.Features.FastCgi;
    using JexusManager.Features.Handlers;
    using System.Diagnostics;
    using IniParser.Parser;
    using EnumsNET;
    using JexusManager.Features.Modules;

    public partial class PhpDiagDialog : DialogForm
    {
        private struct PhpVersion
        {
            public PhpVersion(string version, DateTime expiringDate, Version cppVersion)
            {
                Version = version;
                ExpiringDate = expiringDate;
                CppVersion = cppVersion;
            }

            public string Version { get; set; }
            public DateTime ExpiringDate { get; set; }
            public Version CppVersion { get; set; }
        }

        public PhpDiagDialog(IServiceProvider provider, ServerManager server)
            : base(provider)
        {
            InitializeComponent();

            var knownPhpVersions = new Dictionary<string, PhpVersion>
            {
                { "5.6", new PhpVersion("5.6", new DateTime(2018, 12, 31), new Version(11, 0)) },
                { "7.0", new PhpVersion("7.0", new DateTime(2018, 12, 3), new Version(14, 0)) },
                { "7.1", new PhpVersion("7.1", new DateTime(2019, 12, 1), new Version(14, 0)) },
                { "7.2", new PhpVersion("7.2", new DateTime(2020, 11, 30), new Version(14, 11)) },
                { "7.3", new PhpVersion("7.3", new DateTime(2021, 12, 6), new Version(14, 11)) },
                { "7.4", new PhpVersion("7.4", new DateTime(2022, 11, 28), new Version(14, 0)) },
                { "8.0", new PhpVersion("8.0", new DateTime(2023, 11, 26), new Version(14, 0)) }
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
                        Warn("IMPORTANT: This report might contain confidential information. Mask such before sharing with others.");
                        Warn("-----");
                        Debug($"System Time: {DateTime.Now}");
                        Debug($"Processor Architecture: {Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}");
                        Debug($"OS: {Environment.OSVersion}");
                        Debug($"Server Type: {server.Mode.AsString(EnumFormat.Description)}");
                        Debug(string.Empty);

                        var modules = new ModulesFeature((Module)provider);
                        modules.Load();
                        Debug($"Scan {modules.Items.Count} installed module(s).");
                        if (modules.Items.All(item => item.Name != "FastCgiModule"))
                        {
                            Error($"FastCGI module is not installed as part of IIS. Please refer to https://docs.microsoft.com/en-us/iis/application-frameworks/scenario-build-a-php-website-on-iis/configuring-step-1-install-iis-and-php#13-download-and-install-php-manually for more details.");
                            return;
                        }
                        else
                        {
                            Debug("FastCGI module is installed.");
                        }

                        Debug(string.Empty);
                        var handlers = new HandlersFeature((Module)provider);
                        handlers.Load();
                        var foundPhpHandler = new List<HandlersItem>();
                        Debug($"Scan {handlers.Items.Count} registered handler(s).");
                        foreach (var item in handlers.Items)
                        {
                            if (item.Modules == "FastCgiModule")
                            {
                                if (File.Exists(item.ScriptProcessor))
                                {
                                    Debug($"* Found a valid FastCGI handler as {{ Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }}. File path: {item.ScriptProcessor}.");
                                    foundPhpHandler.Add(item);
                                }
                                else
                                {
                                    Error($"* Found an invalid FastCGI handler as {{Name: {item.Name}, Path: {item.Path}, State: {item.GetState(handlers.AccessPolicy)}, Module: {item.TypeString}, Entry Type: {item.Flag} }} because file path does not exist: {item.ScriptProcessor}.");
                                }
                            }
                        }

                        if (foundPhpHandler.Count == 0)
                        {
                            Error($"No valid FastCGI handler is registered for this web site.");
                            Error($"To run PHP on IIS, please refer to https://docs.microsoft.com/en-us/iis/application-frameworks/scenario-build-a-php-website-on-iis/configuring-step-1-install-iis-and-php#13-download-and-install-php-manually for more details.");
                            return;
                        }

                        Debug(string.Empty);
                        var fastCgiFeature = new FastCgiFeature((Module)provider);
                        fastCgiFeature.Load();
                        Debug($"Scan {fastCgiFeature.Items.Count} registered FastCGI application(s).");
                        var foundPhp = new List<FastCgiItem>();
                        foreach (var item in fastCgiFeature.Items)
                        {
                            var combination = string.IsNullOrWhiteSpace(item.Arguments) ? item.Path : item.Path + '|' + item.Arguments;
                            foreach (var handler in foundPhpHandler)
                            {
                                if (string.Equals(combination, handler.ScriptProcessor, StringComparison.OrdinalIgnoreCase))
                                {
                                    Debug($"* Found FastCGI application registered as {{ Full path: {item.Path}, Arguments: {item.Arguments} }}.");
                                    foundPhp.Add(item);
                                    break;
                                }
                            }
                        }

                        if (foundPhp.Count == 0)
                        {
                            Error($"No suitable FastCGI appilcation is registered on this server.");
                            Error($"To run PHP on IIS, please refer to https://docs.microsoft.com/en-us/iis/application-frameworks/scenario-build-a-php-website-on-iis/configuring-step-1-install-iis-and-php#13-download-and-install-php-manually for more details.");
                           return;
                        }

                        Debug(Environment.NewLine);
                        Debug($"Verify web stack installation versions.");
                        foreach (var item in foundPhp)
                        {
                            var path = item.Path;
                            if (path.TrimEnd('"').EndsWith("php-cgi.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                // PHP
                                var info = FileVersionInfo.GetVersionInfo(path);
                                var version = $"{info.FileMajorPart}.{info.FileMinorPart}";
                                if (knownPhpVersions.ContainsKey(version))
                                {
                                    var matched = knownPhpVersions[version];
                                    if (matched.ExpiringDate <= DateTime.Now)
                                    {
                                        Error($"* PHP {info.FileVersion} ({path}) is unknown or obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                                    }
                                    else if (matched.ExpiringDate > DateTime.Now && (matched.ExpiringDate - DateTime.Now).TotalDays < 180)
                                    {
                                        Warn($"* PHP {version} ({path}) will soon be obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                                    }
                                    else
                                    {
                                        Debug($"* PHP {version} ({path}) is supported.");
                                    }

                                    var x86 = DialogHelper.GetImageArchitecture(path);
                                    var cppFile = Path.Combine(
                                        Environment.GetFolderPath(x86 ? Environment.SpecialFolder.SystemX86 : Environment.SpecialFolder.System),
                                        $"msvcp{matched.CppVersion.Major}0.dll");
                                    if (File.Exists(cppFile))
                                    {
                                        var cpp = FileVersionInfo.GetVersionInfo(cppFile);
                                        if (cpp.FileMinorPart >= matched.CppVersion.Minor)
                                        {
                                            Debug($"  Visual C++ runtime is detected (expected: {matched.CppVersion}, detected: {cpp.FileVersion}): {cppFile}.");
                                        }
                                        else
                                        {
                                            Error($"  Visual C++ runtime {matched.CppVersion} is not detected. Please install it following the tips on https://windows.php.net/download/.");
                                        }
                                    }
                                    else
                                    {
                                        Error($"  Visual C++ {matched.CppVersion} runtime is not detected. Please install it following the tips on https://windows.php.net/download/.");
                                    }
                                }
                                else
                                {
                                    Error($"* PHP {info.FileVersion} ({path}) is unknown or obsolete. Please refer to http://php.net/supported-versions.php for more details.");
                                }
                            }
                        }

                        Debug(string.Empty);
                        var systemPath = Environment.GetEnvironmentVariable("Path");
                        Debug($"Windows Path environment variable: {systemPath}.");
                        Debug(string.Empty);
                        string[] paths = systemPath.Split(new char[1] { Path.PathSeparator });
                        foreach (var item in foundPhp)
                        {
                            var path = item.Path;
                            if (path.TrimEnd('"').EndsWith("php-cgi.exe", StringComparison.OrdinalIgnoreCase))
                            {
                                var rootFolder = Path.GetDirectoryName(path);
                                Debug($"[{rootFolder}]");
                                if (!Directory.Exists(rootFolder))
                                {
                                    Error("Invalid root folder is found. Skip.");
                                    continue;
                                }

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
                                        extensionFolder = "\"ext\"";
                                    }

                                    extensionFolder = extensionFolder.Trim('"');

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

                                // TODO: verify other configuration in php.info.
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
                    var fileName = DialogHelper.ShowSaveFileDialog(null, "Text Files|*.txt|All Files|*.*", null);
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
            DialogHelper.ProcessStart("https://docs.jexusmanager.com/tutorials/php-diagnostics.html");
        }

        private void PhpDiagDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnHelpClick(null, EventArgs.Empty);
        }
    }
}

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

    public partial class VsDiagDialog : DialogForm
    {
        public VsDiagDialog(IServiceProvider provider, Site site)
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

                        Debug($"Scan the folder {root} for project files.");

                        string[] projects = null;
                        try
                        {
                            projects = Directory.GetFiles(root, "*.csproj");
                            Info($"{projects.Length} project(s) are detected.");
                        }
                        catch (IOException ex)
                        {
                            Error($"Query project files failed: {ex.Message}");
                            return;
                        }

                        foreach (var proj in projects)
                        {
                            Info($"* {Path.GetFileName(proj)}");
                        }

                        if (projects.Length != 1)
                        {
                            Warn("Zero or multiple C# project files are detected. Currently VSDiag only supports a single C# project.");
                            return;
                        }

                        Debug(Environment.NewLine);
                        var project = projects[0];

                        Info($"Project file: {project}.");
                        Info($"IIS Express configuration file: {site.Server.FileName}.");
                        // TODO: free the resources.
                        var xmlReader = XmlReader.Create(new StringReader(File.ReadAllText(project))); // Or whatever your source is, of course.
                        var xml = XDocument.Load(xmlReader);
                        var namespaceManager = new XmlNamespaceManager(xmlReader.NameTable); // We now have a namespace manager that knows of the namespaces used in your document.
                        var name = xml.Root.GetDefaultNamespace();
                        namespaceManager.AddNamespace("x", name.NamespaceName); // We add an explicit prefix mapping for our query.

                        if (IsWebProject(xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:ProjectTypeGuids", namespaceManager)?.Value))
                        {
                            AnalyzeWebProject(xml, namespaceManager, name, site);
                        }
                        else if (IsAspNetCoreProject(xml))
                        {
                            AnalyzeAspNetCoreProject(project, site);
                        }
                        else
                        {
                            Error("The .csproj file does not seem to be a web project.");
                            return;
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

        private void AnalyzeAspNetCoreProject(string project, Site site)
        {
            Debug($"Analyze ASP.NET Core project.");
            var folder = Path.GetDirectoryName(project);
            var settingsFile = Path.Combine(folder, "Properties", "launchSettings.json");
            if (!File.Exists(settingsFile))
            {
                Error($"Cannot find {settingsFile}.");
                return;
            }

            Info($"Visual Studio launchSettings.json: {settingsFile}.");
            Debug($"Extract debugging profiles.");
            JObject o1 = JObject.Parse(File.ReadAllText(settingsFile));
            var profiles = o1["profiles"].Children<JProperty>().ToList();
            if (profiles.Count == 0)
            {
                Error($"Cannot find mandate profiles.");
                Error($"Please fix launchSettings.json.");
                return;
            }

            Info($"Found {profiles.Count} profile(s).");
            foreach (var profile in profiles)
            {
                Info($"* {profile.Name}");
            }

            Debug(Environment.NewLine);
            Debug("Extract IIS settings.");
            var iisSettings = o1["iisSettings"]?.Children<JProperty>().ToList();
            if (iisSettings == null)
            {
                Error($"Cannot find 'iisSettings' section.");
                Error($"Please fix launchSettings.json.");
                return;
            }

            var hasExpress = false;
            foreach (var item in iisSettings)
            {
                if (item.Name == "iisExpress")
                {
                    hasExpress = true;
                    var sslPort = o1["iisSettings"]["iisExpress"]["sslPort"]?.Value<int>();
                    if (sslPort == null)
                    {
                        sslPort = 0; // the default value is 0.
                    }

                    Info($"sslPort is {sslPort}.");
                    var rawUrl = o1["iisSettings"]["iisExpress"]["applicationUrl"]?.Value<string>();
                    if (rawUrl == null)
                    {
                        Error($"Cannot find applicationUrl.");
                        Error($"Please fix launchSettings.json.");
                        return;
                    }

                    Info($"applicationUrl is {rawUrl}.");
                    var iisUrl = sslPort == 0 ? rawUrl : $"https://localhost:{sslPort}/";
                    var matched = false;
                    foreach (Binding binding in site.Bindings)
                    {
                        Info($"Binding {binding.ToShortString()}.");
                        if (string.Equals(binding.ToIisUrl(), iisUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            Info($"A matching binding is found for {iisUrl}.");
                            matched = true;
                        }
                    }

                    if (!matched)
                    {
                        Error($"No matching binding is found for {iisUrl}");
                        Error($"Please edit launchSettings.json and IIS Express configuration file to match each other.");
                    }
                }
            }

            if (!hasExpress)
            {
                Error("Cannot find 'iisSettings/iisExpress' section.");
                Error($"Please fix launchSettings.json.");
            }
        }

        private static bool IsAspNetCoreProject(XDocument xml)
        {
            if (xml.Root.Name == "Project" && xml.Root.Attribute("Sdk").Value == "Microsoft.NET.Sdk.Web")
            {
                // TODO: use a more accurate way to detect SDK.
                return true;
            }

            return false;
        }

        private void AnalyzeWebProject(XDocument xml, XmlNamespaceManager namespaceManager, XNamespace name, Site site)
        {
            Debug($"Analyze ASP.NET project.");
            Debug($"Extract web project settings.");
            var webSettings = xml.Root.XPathSelectElement("/x:Project/x:ProjectExtensions/x:VisualStudio/x:FlavorProperties/x:WebProjectProperties", namespaceManager);
            var useIIS = webSettings.Element(name + "UseIIS")?.Value;
            Info($"UseIIS: {useIIS}");
            var autoAssignPort = webSettings.Element(name + "AutoAssignPort")?.Value;
            Info($"AutoAssignPort: {autoAssignPort}");
            var developmentServerPort = webSettings.Element(name + "DevelopmentServerPort")?.Value;
            Info($"DevelopmentServerPort: {developmentServerPort}");
            var developmentServerVPath = webSettings.Element(name + "DevelopmentServerVPath")?.Value;
            Info($"DevelopmentServerVPath: {developmentServerVPath}");
            var iisUrl = webSettings.Element(name + "IISUrl")?.Value;
            Info($"IISUrl: {iisUrl}");
            var ntlmAuthentication = webSettings.Element(name + "NTLMAuthentication")?.Value;
            Info($"NTLMAuthentication: {ntlmAuthentication}");
            var useCustomServer = webSettings.Element(name + "UseCustomServer")?.Value;
            Info($"UseCustomServer: {useCustomServer}");
            var customServerUrl = webSettings.Element(name + "CustomServerUrl")?.Value;
            Info($"CustomServerUrl: {customServerUrl}");
            var saveServerSettingsInUserFile = webSettings.Element(name + "SaveServerSettingsInUserFile")?.Value;
            Info($"SaveServerSettingsInUserFile: {saveServerSettingsInUserFile}");

            var useIISExpress = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:UseIISExpress", namespaceManager)?.Value;
            Info($"UseIISExpress: {useIISExpress}");
            var iisExpressSSLPort = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressSSLPort", namespaceManager)?.Value;
            Info($"IISExpressSSLPort: {iisExpressSSLPort}");
            var iisExpressAnonymousAuthentication = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressAnonymousAuthentication", namespaceManager)?.Value;
            Info($"IISExpressAnonymousAuthentication: {iisExpressAnonymousAuthentication}");
            var iisExpressWindowsAuthentication = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressWindowsAuthentication", namespaceManager)?.Value;
            Info($"IISExpressWindowsAuthentication: {iisExpressWindowsAuthentication}");
            var iisExpressUseClassicPipelineMode = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressUseClassicPipelineMode", namespaceManager)?.Value;
            Info($"IISExpressUseClassicPipelineMode: {iisExpressUseClassicPipelineMode}");
            var useGlobalApplicationHostFile = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:UseGlobalApplicationHostFile", namespaceManager)?.Value;
            Info($"UseGlobalApplicationHostFile: {useGlobalApplicationHostFile}");

            Info($"Scan all bindings.");
            bool matched = false;
            if (string.Equals(useIIS, "true", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(useIISExpress, "true", StringComparison.OrdinalIgnoreCase))
                {
                    // IIS Express
                    Info($"IIS Express is used for this project.");
                    foreach (Binding binding in site.Bindings)
                    {
                        Info($"Binding {binding.ToShortString()}.");
                        if (string.Equals(binding.ToIisUrl(), iisUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            Info($"A matching binding is found for {iisUrl}.");
                            matched = true;
                        }
                    }
                }
                else if (string.Equals(useIISExpress, "false", StringComparison.OrdinalIgnoreCase))
                {
                    // IIS
                    Info($"Full IIS is used for this project.");
                    foreach (Binding binding in site.Bindings)
                    {
                        Info($"Binding {binding.ToShortString()}.");
                        if (string.Equals(binding.ToIisUrl(), iisUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            Info($"A matching binding is found for {iisUrl}.");
                            matched = true;
                        }
                    }
                }
                else
                {
                    //TODO:  What?
                    Warn($"An unexpected condition hit.");
                    Rollbar.RollbarLocator.RollbarInstance.Error("unexpected condition hit.");
                    return;
                }
            }
            else
            {
                // External server
                Warn($"External server is used.");
                return;
            }

            if (!matched)
            {
                Error($"No matching binding is found for {iisUrl}.");
                Error($"Please edit project file and IIS Express configuration file to match each other.");
            }
        }

        //*
        private static readonly string[] mvc = {"{603C0E0B-DB56-11DC-BE95-000D561079B0}", // ASP.NET MVC 1	
        "{F85E285D-A4E0-4152-9332-AB1D724D3325}", // ASP.NET MVC 2	
        "{E53F8FEA-EAE0-44A6-8774-FFD645390401}", // ASP.NET MVC 3	
        "{E3E379DF-F4C6-4180-9B81-6769533ABE47}", // ASP.NET MVC 4	
        "{349C5851-65DF-11DA-9384-00065B846F21}" // ASP.NET MVC 5
        };
        // */

        private static bool IsWebProject(string value)
        {
            if (value == null)
                return false;

            foreach (var name in mvc)
            {
                if (value.IndexOf(name, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    return true;
                }
            }

            return false;
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

        private int GetEventLogging()
        {
            if (Helper.IsRunningOnMono())
            {
                return -1;
            }

            // https://support.microsoft.com/en-us/kb/260729
            var key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\SecurityProviders\SCHANNEL");
            if (key == null)
            {
                return 1;
            }

            var value = (int)key.GetValue("EventLogging", 1);
            return value;
        }

        private static bool GetProtocol(string protocol)
        {
            if (Helper.IsRunningOnMono())
            {
                return false;
            }

            // https://support.microsoft.com/en-us/kb/187498
            var key =
                Registry.LocalMachine.OpenSubKey(
                    $@"SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\{protocol}\Server");
            if (key == null)
            {
                return true;
            }

            var value = (int)key.GetValue("Enabled", 1);
            var enabled = value == 1;
            return enabled;
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

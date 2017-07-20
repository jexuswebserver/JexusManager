// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Threading.Tasks;

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    using JexusManager.Dialogs;
    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Binding = Microsoft.Web.Administration.Binding;
    using Module = Microsoft.Web.Management.Client.Module;
    using System.IO;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class SiteFeature
    {
        private sealed class FeatureTaskList : TaskList
        {
            private readonly SiteFeature _owner;

            public FeatureTaskList(SiteFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Explore", "Explore", string.Empty, string.Empty, Resources.explore_16).SetUsage());
                result.Add(new MethodTaskItem("Permissions", "Edit Permissions...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                result.Add(new TextTaskItem("Edit Site", string.Empty, true));
                result.Add(new MethodTaskItem("Bindings", "Bindings...", string.Empty).SetUsage());
                result.Add(
                    new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                        Resources.basic_settings_16).SetUsage());
                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("Applications", "View Applications", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("VirtualDirectories", "View Virtual Directories", string.Empty).SetUsage());

                if (_owner.SiteBindings.Any(item => item.CanBrowse))
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    var manageGroup = new GroupTaskItem(string.Empty, "Manage Website", string.Empty, true);
                    result.Add(manageGroup);
                    manageGroup.Items.Add(
                        new MethodTaskItem("Restart", "Restart", string.Empty, string.Empty, Resources.restart_16).SetUsage(!_owner.IsBusy));
                    manageGroup.Items.Add(
                        new MethodTaskItem("Start", "Start", string.Empty, string.Empty, Resources.start_16).SetUsage(
                            !_owner.IsBusy && !_owner.IsStarted));
                    manageGroup.Items.Add(new MethodTaskItem("Stop", "Stop", string.Empty, string.Empty, Resources.stop_16)
                        .SetUsage(
                            !_owner.IsBusy && _owner.IsStarted));
                    manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    manageGroup.Items.Add(new TextTaskItem("Browse Website", string.Empty, true));
                    foreach (Binding binding in _owner.SiteBindings)
                    {
                        if (binding.CanBrowse)
                        {
                            manageGroup.Items.Add(
                                new MethodTaskItem("Browse", string.Format("Browse {0}", binding.ToShortString()),
                                    string.Empty, string.Empty,
                                    Resources.browse_16, binding.ToUri()).SetUsage());
                        }
                    }

                    manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("Advanced", "Advanced Settings...", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    manageGroup.Items.Add(new TextTaskItem("Configure", string.Empty, true));
                    manageGroup.Items.Add(new MethodTaskItem("Tracing", "Failed Request Tracing...", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("Limits", "Limits...", string.Empty).SetUsage());

                    if (_owner.HasProject)
                    {
                        manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                        manageGroup.Items.Add(new TextTaskItem("Troubleshooting", string.Empty, true));
                        manageGroup.Items.Add(new MethodTaskItem("FixProject", "Project Diagnostics", string.Empty).SetUsage());
                    }
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Explore()
            {
                _owner.Explore();
            }

            [Obfuscation(Exclude = true)]
            public void Permissions()
            {
                _owner.Permissions();
            }

            [Obfuscation(Exclude = true)]
            public void Bindings()
            {
                _owner.Bindings();
            }

            [Obfuscation(Exclude = true)]
            public void Basic()
            {
                _owner.Basic();
            }

            [Obfuscation(Exclude = true)]
            public void Applications()
            {
                _owner.Applications();
            }

            [Obfuscation(Exclude = true)]
            public void VirtualDirectories()
            {
                _owner.VirtualDirectories();
            }

            [Obfuscation(Exclude = true)]
            public void Restart()
            {
                _owner.Restart();
            }

            [Obfuscation(Exclude = true)]
            public void Start()
            {
                _owner.Start();
            }

            [Obfuscation(Exclude = true)]
            public void Stop()
            {
                _owner.Stop();
            }

            [Obfuscation(Exclude = true)]
            public void Browse(object uri)
            {
                _owner.Browse(uri);
            }

            [Obfuscation(Exclude = true)]
            public void Advanced()
            {
                _owner.Advanced();
            }

            [Obfuscation(Exclude = true)]
            public void Tracing()
            {
                _owner.Tracing();
            }

            [Obfuscation(Exclude = true)]
            public void Limits()
            {
                _owner.Limits();
            }

            [Obfuscation (Exclude = true)]
            public void FixProject()
            {
                _owner.FixProject();
            }
        }

        public SiteFeature(Module module)
        {
            Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (Module as IServiceProvider).GetService(type);
        }

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            IsStarted = AsyncHelper.RunSync(() => site.GetStateAsync());
            HasProject = SiteHasProject(site);
            OnSiteSettingsSaved();
        }

        protected void OnSiteSettingsSaved()
        {
            SiteSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210463");
            return false;
        }

        private void Limits()
        {
        }

        private void Tracing()
        {
        }

        private void Advanced()
        {
        }

        private void Browse(object uri)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));

            // IMPORTANT: help users launch IIS Express instance.
            var site = service.Site;
            if (site.Server.Mode == WorkingMode.IisExpress && site.State != ObjectState.Started)
            {
                var message = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result = message.ShowMessage(
                    "This website is not yet running. Do you want to start it now?",
                    "Question",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                Start();
            }

            Process.Start(uri.ToString());
        }

        private void Stop()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            IsBusy = true;
            OnSiteSettingsSaved();
            site.Stop();
            IsStarted = false;
            IsBusy = false;
            OnSiteSettingsSaved();
        }

        private void Start()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var site = service.Site;
            IsBusy = true;
            OnSiteSettingsSaved();
            try
            {
                site.Start();
            }
            catch (Exception ex)
            {
                dialog.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsStarted = site.State == ObjectState.Started;
            IsBusy = false;
            OnSiteSettingsSaved();
        }

        private void Restart()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var site = service.Site;
            IsBusy = true;
            OnSiteSettingsSaved();
            try
            {
                AsyncHelper.RunSync(() => site.RestartAsync());
            }
            catch (Exception ex)
            {
                dialog.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsStarted = site.State == ObjectState.Started;
            IsBusy = false;
            OnSiteSettingsSaved();
        }

        private void VirtualDirectories()
        {
        }

        private void Applications()
        {
        }

        private void Basic()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            var dialog = new EditSiteDialog(Module, site.Applications[0]);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            site.Applications[0].Save();
            site.Server.CommitChanges();
        }

        private void Permissions()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            NativeMethods.ShowFileProperties(
                site.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables());
        }

        private void Explore()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            DialogHelper.Explore(site.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables());
        }

        private void Bindings()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            var dialog = new BindingsDialog(Module, site);
            dialog.ShowDialog();
            OnSiteSettingsSaved();
        }

        private void FixProject()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            var root = site.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables();
            var projects = Directory.GetFiles(root, "*.csproj");

            var project = projects[0];
            // TODO: free the resources.
            var xmlReader = XmlReader.Create(new StringReader(File.ReadAllText(project))); // Or whatever your source is, of course.
            var xml = XDocument.Load(xmlReader);
            var namespaceManager = new XmlNamespaceManager(xmlReader.NameTable); // We now have a namespace manager that knows of the namespaces used in your document.
            var name = xml.Root.GetDefaultNamespace();
            namespaceManager.AddNamespace("x", name.NamespaceName); // We add an explicit prefix mapping for our query.

            var webSettings = xml.Root.XPathSelectElement("/x:Project/x:ProjectExtensions/x:VisualStudio/x:FlavorProperties/x:WebProjectProperties", namespaceManager);
            var useIIS = webSettings.Element(name + "UseIIS")?.Value;
            var autoAssignPort = webSettings.Element(name + "AutoAssignPort")?.Value;
            var developmentServerPort = webSettings.Element(name + "DevelopmentServerPort")?.Value;
            var developmentServerVPath = webSettings.Element(name + "DevelopmentServerVPath")?.Value;
            var iisUrl = webSettings.Element(name + "IISUrl")?.Value;
            var ntlmAuthentication = webSettings.Element(name + "NTLMAuthentication")?.Value;
            var useCustomServer = webSettings.Element(name + "UseCustomServer")?.Value;
            var customServerUrl = webSettings.Element(name + "CustomServerUrl")?.Value;
            var saveServerSettingsInUserFile = webSettings.Element(name + "SaveServerSettingsInUserFile")?.Value;

            var useIISExpress = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:UseIISExpress", namespaceManager)?.Value;
            var iisExpressSSLPort = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressSSLPort", namespaceManager)?.Value;
            var iisExpressAnonymousAuthentication = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressAnonymousAuthentication", namespaceManager)?.Value;
            var iisExpressWindowsAuthentication = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressWindowsAuthentication", namespaceManager)?.Value;
            var iisExpressUseClassicPipelineMode = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressUseClassicPipelineMode", namespaceManager)?.Value;
            var useGlobalApplicationHostFile = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:UseGlobalApplicationHostFile", namespaceManager)?.Value;

            string url = null;
            foreach (Binding binding in site.Bindings)
            {
                if (string.IsNullOrEmpty(iisExpressSSLPort))
                {
                    // http
                    if (binding.Protocol == "http")
                    {
                        url = binding.ToIisUrl();
                        break;
                    }
                }
                else
                {
                    // https
                    if (binding.Protocol == "https")
                    {
                        url = binding.ToIisUrl();
                        break;
                    }
                }
            }

            if (url != null)
            {
                webSettings.Element(name + "IISUrl").SetValue(url);
                MessageBox.Show("Changed IISUrl to " + url);
                xml.Save(project);
                return;
            }

            MessageBox.Show("No binding is valid to generate IISUrl");
        }

        private static bool SiteHasProject(Site site)
        {
            var root = site.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables();
            string[] projects;
            try
            {
                projects = Directory.GetFiles(root, "*.csproj");
                if (projects.Length != 1)
                {
                    return false;
                }
            }
            catch (IOException)
            {
                return false;
            }

            var project = projects[0];
            // TODO: free the resources.
            var xmlReader = XmlReader.Create(new StringReader(File.ReadAllText(project))); // Or whatever your source is, of course.
            var xml = XDocument.Load(xmlReader);
            var namespaceManager = new XmlNamespaceManager(xmlReader.NameTable); // We now have a namespace manager that knows of the namespaces used in your document.
            var name = xml.Root.GetDefaultNamespace();
            namespaceManager.AddNamespace("x", name.NamespaceName); // We add an explicit prefix mapping for our query.

            if (!IsWebProject(xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:ProjectTypeGuids", namespaceManager)?.Value))
            {
                // Not web project
                return false;
            }

            var webSettings = xml.Root.XPathSelectElement("/x:Project/x:ProjectExtensions/x:VisualStudio/x:FlavorProperties/x:WebProjectProperties", namespaceManager);
            if (webSettings == null)
            {
                return false;
            }

            var useIIS = webSettings.Element(name + "UseIIS")?.Value;
            var autoAssignPort = webSettings.Element(name + "AutoAssignPort")?.Value;
            var developmentServerPort = webSettings.Element(name + "DevelopmentServerPort")?.Value;
            var developmentServerVPath = webSettings.Element(name + "DevelopmentServerVPath")?.Value;
            var iisUrl = webSettings.Element(name + "IISUrl")?.Value;
            var ntlmAuthentication = webSettings.Element(name + "NTLMAuthentication")?.Value;
            var useCustomServer = webSettings.Element(name + "UseCustomServer")?.Value;
            var customServerUrl = webSettings.Element(name + "CustomServerUrl")?.Value;
            var saveServerSettingsInUserFile = webSettings.Element(name + "SaveServerSettingsInUserFile")?.Value;

            var useIISExpress = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:UseIISExpress", namespaceManager)?.Value;
            var iisExpressSSLPort = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressSSLPort", namespaceManager)?.Value;
            var iisExpressAnonymousAuthentication = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressAnonymousAuthentication", namespaceManager)?.Value;
            var iisExpressWindowsAuthentication = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressWindowsAuthentication", namespaceManager)?.Value;
            var iisExpressUseClassicPipelineMode = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:IISExpressUseClassicPipelineMode", namespaceManager)?.Value;
            var useGlobalApplicationHostFile = xml.Root.XPathSelectElement("/x:Project/x:PropertyGroup/x:UseGlobalApplicationHostFile", namespaceManager)?.Value;

            if (string.Equals(useIIS, "true", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(useIISExpress, "true", StringComparison.OrdinalIgnoreCase))
                {
                    // IIS Express
                    foreach (Binding binding in site.Bindings)
                    {
                        var url = binding.ToIisUrl();
                        if (string.Equals(url, iisUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                }
                else if (string.Equals(useIISExpress, "false", StringComparison.OrdinalIgnoreCase))
                {
                    // IIS
                    foreach (Binding binding in site.Bindings)
                    {
                        if (string.Equals(binding.ToIisUrl(), iisUrl, StringComparison.OrdinalIgnoreCase))
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //TODO:  What?
                    return false;
                }
            }
            else
            {
                // External server
                return false;
            }

            return true;
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

        public IEnumerable<Binding> SiteBindings
        {
            get
            {
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                var site = service.Site;
                return site.Bindings;
            }
        }

        public bool HasProject { get; set; }

        public bool IsStarted { get; set; }

        public bool IsBusy { get; set; }

        public SiteSettingsSavedEventHandler SiteSettingsUpdated { get; set; }

        public string Description { get; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }
        public string Name { get; }
    }

    public delegate void SiteSettingsSavedEventHandler();
}

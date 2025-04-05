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

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
    using System.Linq;
    using JexusManager.Features.TraceFailedRequests;
    using Application = Microsoft.Web.Administration.Application;

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
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new TextTaskItem("Edit Site", string.Empty, true));
                result.Add(new MethodTaskItem("Bindings", "Bindings...", string.Empty).SetUsage());
                result.Add(
                    new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                        Resources.basic_settings_16).SetUsage());
                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new MethodTaskItem("Applications", "View Applications", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("VirtualDirectories", "View Virtual Directories", string.Empty).SetUsage());

                if (_owner.SiteBindings.Any(item => item.CanBrowse))
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
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
                    manageGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
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

                    manageGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("Advanced", "Advanced Settings...", string.Empty).SetUsage());
                    manageGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    manageGroup.Items.Add(new TextTaskItem("Configure", string.Empty, true));
                    manageGroup.Items.Add(new MethodTaskItem("Tracing", "Failed Request Tracing...", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("Limits", "Limits...", string.Empty).SetUsage());
                    manageGroup.Items.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    manageGroup.Items.Add(new TextTaskItem("Troubleshooting", string.Empty, true));
                    manageGroup.Items.Add(new MethodTaskItem("FixBinding", "Binding Diagnostics", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("FixKestrel", "ASP.NET Core Diagnostics", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("FixProject", "Project Diagnostics", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("FixPhp", "PHP Diagnostics", string.Empty).SetUsage());
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

            [Obfuscation(Exclude = true)]
            public void FixBinding()
            {
                _owner.FixBinding();
            }

            [Obfuscation(Exclude = true)]
            public void FixPhp()
            {
                _owner.FixPhp();
            }

            [Obfuscation(Exclude = true)]
            public void FixKestrel()
            {
                _owner.FixKestrel();
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
            IsStarted = site.GetState();
            OnSiteSettingsSaved();
        }

        protected void OnSiteSettingsSaved()
        {
            SiteSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210463");
            return false;
        }

        private void Limits()
        {
        }

        private void Tracing()
        {
            var feature = new TraceFailedRequestsFeature(Module);
            feature.Set();
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

            DialogHelper.ProcessStart(uri.ToString());
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
                DialogHelper.SiteStart(site);
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
                site.Restart();
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
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var application = service.Site.Applications[0];

            IModulePage page = new VirtualDirectoriesPage();
            page.Initialize(Module, null, application);
            ((MainForm)service.Form).LoadPage(page);
        }

        private void Applications()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;

            IModulePage page = new ApplicationsPage();
            page.Initialize(Module, null, new Tuple<List<Application>, Site>(null, site));
            ((MainForm)service.Form).LoadPage(page);
        }

        private void Basic()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            using (var dialog = new EditSiteDialog(Module, site.Applications[0]))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            site.Applications[0].Save();
            site.Server.CommitChanges();
        }

        private void Permissions()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            var path = site.PhysicalPath.ExpandIisExpressEnvironmentVariables(site.Applications[0].GetActualExecutable());
            if (!string.IsNullOrWhiteSpace(path))
            {
                NativeMethods.ShowFileProperties(path);
            }
        }

        private void Explore()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            var path = site.PhysicalPath.ExpandIisExpressEnvironmentVariables(site.Applications[0].GetActualExecutable());
            if (!string.IsNullOrWhiteSpace(path))
            {
                DialogHelper.Explore(path);
            }
        }

        private void Bindings()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            using (var dialog = new BindingsDialog(Module, site))
            {
                dialog.ShowDialog();
            }
            OnSiteSettingsSaved();
        }

        private void FixProject()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            using var dialog = new VsDiagDialog(Module, service.Site);
            dialog.ShowDialog();
        }

        private void FixBinding()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            using var dialog = new BindingDiagDialog(Module, service.Site);
            dialog.ShowDialog();
        }

        private void FixPhp()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            using var dialog = new PhpDiagDialog(Module, service.Site.Server);
            dialog.ShowDialog();
        }

        private void FixKestrel()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            using var dialog = new KestrelDiagDialog(Module, service.Site.Applications[0]);
            dialog.ShowDialog();
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

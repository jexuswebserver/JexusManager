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
                    manageGroup.Items.Add(
                        new MethodTaskItem("Browse", string.Format("Browse {0}", binding.ToShortString()), string.Empty, string.Empty,
                            Resources.browse_16, binding.ToUri()).SetUsage());
                }

                manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                manageGroup.Items.Add(new MethodTaskItem("Advanced", "Advanced Settings...", string.Empty).SetUsage());
                manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                manageGroup.Items.Add(new TextTaskItem("Configure", string.Empty, true));
                manageGroup.Items.Add(new MethodTaskItem("Tracing", "Failed Request Tracing...", string.Empty).SetUsage());
                manageGroup.Items.Add(new MethodTaskItem("Limits", "Limits...", string.Empty).SetUsage());
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

        public async void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            IsStarted = await site.GetStateAsync();
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

        private async void Stop()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            if (site.Bindings.RequireElevation() && !JexusManager.NativeMethods.IsProcessElevated)
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                dialog.ShowMessage("This site cannot be stopped. Please run Jexus Manager as administrator.", Name);
                return;
            }

            IsBusy = true;
            OnSiteSettingsSaved();
            await site.StopAsync();
            IsStarted = false;
            IsBusy = false;
            OnSiteSettingsSaved();
        }

        private async void Start()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var site = service.Site;
            if (site.Bindings.RequireElevation() && !JexusManager.NativeMethods.IsProcessElevated)
            {
                dialog.ShowMessage("This site cannot be started. Please run Jexus Manager as administrator.", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsBusy = true;
            OnSiteSettingsSaved();
            try
            {
                await site.StartAsync();
            }
            catch (Exception ex)
            {
                dialog.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsStarted = site.State == ObjectState.Started;
            IsBusy = false;
            OnSiteSettingsSaved();
        }

        private async void Restart()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var site = service.Site;
            if (site.Bindings.RequireElevation() && !JexusManager.NativeMethods.IsProcessElevated)
            {
                dialog.ShowMessage("This site cannot be restarted. Please run Jexus Manager as administrator.", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsBusy = true;
            OnSiteSettingsSaved();
            try
            {
                await site.RestartAsync();
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
            Process.Start(site.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables());
        }

        private void Bindings()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var site = service.Site;
            var dialog = new BindingsDialog(Module, site);
            dialog.ShowDialog();
            OnSiteSettingsSaved();
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

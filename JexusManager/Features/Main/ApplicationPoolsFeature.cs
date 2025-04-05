// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;
    using System.Collections.Generic;

    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;
    using Application = Microsoft.Web.Administration.Application;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class ApplicationPoolsFeature : FeatureBase<ApplicationPool>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly ApplicationPoolsFeature _owner;

            public FeatureTaskList(ApplicationPoolsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(
                    new MethodTaskItem("Add", "Add Application Pool...", string.Empty, string.Empty, Resources.application_pool_new_16)
                        .SetUsage());
                result.Add(new MethodTaskItem("Set", "Set Application Pool Defaults...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new TextTaskItem("Application Pool Tasks", string.Empty, true));
                    result.Add(
                        new MethodTaskItem("Start", "Start", string.Empty, string.Empty, Resources.start_16).SetUsage(
                            !_owner.IsBusy && _owner.SelectedItem.State != ObjectState.Started));
                    result.Add(
                        new MethodTaskItem("Stop", "Stop", string.Empty, string.Empty, Resources.stop_16).SetUsage(
                            !_owner.IsBusy && _owner.SelectedItem.State == ObjectState.Started));
                    result.Add(
                        new MethodTaskItem("Recycle", "Recycle...", string.Empty, string.Empty, Resources.restart_16)
                            .SetUsage(!_owner.IsBusy && _owner.SelectedItem.State == ObjectState.Started));
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new TextTaskItem("Edit Application Pool", string.Empty, true));
                    result.Add(
                        new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                            Resources.basic_settings_16).SetUsage());
                    result.Add(new MethodTaskItem("Recycling", "Recycling...", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Advanced", "Advanced Settings", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(RemoveTaskItem);
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(
                        new MethodTaskItem("Applications", "View Applications", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void Set()
            {
                _owner.Set();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Rename()
            {
                _owner.Rename();
            }

            [Obfuscation(Exclude = true)]
            public void Basic()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void Applications()
            {
                _owner.Applications();
            }

            [Obfuscation(Exclude = true)]
            public void Recycle()
            {
                _owner.Recycle();
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
            public void Recycling()
            {
                _owner.Recycling();
            }

            [Obfuscation(Exclude = true)]
            public void Advanced()
            {
                _owner.Advanced();
            }
        }

        public ApplicationPoolsFeature(Module module)
            : base(module)
        {
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;
        private ServerManager _serverManager;

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            Items = service.Server.ApplicationPools.ToList();
            _serverManager = service.Server;
            OnApplicationPoolsSettingsSaved();
        }

        protected void OnApplicationPoolsSettingsSaved()
        {
            ApplicationPoolsSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210456");
            return false;
        }

        private void Add()
        {
            using (var dialog = new ApplicationPoolBasicSettingsDialog(Module, null, _serverManager.ApplicationPoolDefaults, _serverManager.ApplicationPools))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _serverManager.CommitChanges();
                }

                SelectedItem = dialog.Pool;
                Items = _serverManager.ApplicationPools.ToList();
            }
            OnApplicationPoolsSettingsSaved();
        }

        private void Set()
        {
            using var dialog = new ApplicationPoolDefaultsSettingsDialog(Module, _serverManager.ApplicationPoolDefaults);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _serverManager.CommitChanges();
            }
        }

        private void Recycling()
        { }

        internal void Remove()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result = service.ShowMessage("Are you sure that you want to remove the selected application pool?", "Confirm Remove",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var index = _serverManager.ApplicationPools.IndexOf(SelectedItem);
            _serverManager.ApplicationPools.RemoveAt(index);
            if (_serverManager.ApplicationPools.Count == 0)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = index > _serverManager.ApplicationPools.Count - 1 ? _serverManager.ApplicationPools[_serverManager.ApplicationPools.Count - 1] : _serverManager.ApplicationPools[index];
            }

            _serverManager.CommitChanges();
            Items = _serverManager.ApplicationPools.ToList();
            OnApplicationPoolsSettingsSaved();
        }

        private void Rename()
        {
            RenameInline(SelectedItem);
        }

        private void Advanced()
        {
            using var dialog = new ApplicationPoolAdvancedSettingsDialog(Module, SelectedItem);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _serverManager.CommitChanges();
            }
        }

        private void Stop()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;
            OnApplicationPoolsSettingsSaved();
            SelectedItem.Stop();
            IsBusy = false;
            OnApplicationPoolsSettingsSaved();
        }

        private void Start()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;
            OnApplicationPoolsSettingsSaved();
            SelectedItem.Start();
            IsBusy = false;
            OnApplicationPoolsSettingsSaved();
        }

        private void Recycle()
        {
            if (SelectedItem == null)
            {
                return;
            }

            IsBusy = true;
            OnApplicationPoolsSettingsSaved();
            SelectedItem.Recycle();
            IsBusy = false;
            OnApplicationPoolsSettingsSaved();
        }

        private void Applications()
        {
            if (SelectedItem == null)
            {
                return;
            }
            
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var mainForm = (MainForm)service.Form;
            
            // Find all applications using this pool
            var applications = new List<Application>();
            foreach (Site container in _serverManager.Sites)
            {
                foreach (Application app in container.Applications)
                {
                    if (app.ApplicationPoolName == SelectedItem.Name)
                    {
                        applications.Add(app);
                    }
                }
            }
            
            if (applications.Count == 0)
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                dialog.ShowMessage("No applications are using this application pool.", "Applications", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }            
           
            var page = new ApplicationsPage();
            ((IModulePage)page).Initialize(Module, null, new Tuple<List<Application>, Site>(applications, null));
            
            mainForm.LoadPageAndSelectNode(page, service.Server);
        }

        internal void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(ApplicationPool item)
        {
            using (var dialog = new ApplicationPoolBasicSettingsDialog(Module, item, null, _serverManager.ApplicationPools))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _serverManager.CommitChanges();
                }
            }

            OnApplicationPoolsSettingsSaved();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return null;
        }

        protected override void OnSettingsSaved()
        {
            OnApplicationPoolsSettingsSaved();
        }

        public bool IsBusy { get; set; }

        public ApplicationPoolsSettingsSavedEventHandler ApplicationPoolsSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name { get; }
    }
}

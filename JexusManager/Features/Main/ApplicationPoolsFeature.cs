// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class ApplicationPoolsFeature
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
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
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
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(new TextTaskItem("Edit Application Pool", string.Empty, true));
                    result.Add(
                        new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                            Resources.basic_settings_16).SetUsage());
                    result.Add(new MethodTaskItem("Recycling", "Recycling...", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Advanced", "Advanced Settings", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
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
                _owner.Basic();
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
            Items = service.Server.ApplicationPools;
            OnApplicationPoolsSettingsSaved();
        }

        public ApplicationPoolCollection Items { get; set; }
        public ApplicationPool SelectedItem { get; set; }

        protected void OnApplicationPoolsSettingsSaved()
        {
            ApplicationPoolsSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210456");
            return false;
        }

        private async void Add()
        {
            var dialog = new ApplicationPoolBasicSettingsDialog(Module, null, Items.Parent.ApplicationPoolDefaults, Items);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await Items.Parent.CommitChangesAsync();
            }

            SelectedItem = dialog.Pool;
            OnApplicationPoolsSettingsSaved();
        }

        private async void Set()
        {
            var dialog = new ApplicationPoolDefaultsSettingsDialog(Module, Items.Parent.ApplicationPoolDefaults);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await Items.Parent.CommitChangesAsync();
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

            var index = Items.IndexOf(SelectedItem);
            Items.RemoveAt(index);
            if (Items.Count == 0)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = index > Items.Count - 1 ? Items[Items.Count - 1] : Items[index];
            }

            Items.Parent.CommitChanges();
            OnApplicationPoolsSettingsSaved();
        }

        private void Rename()
        {
        }

        private async void Advanced()
        {
            var dialog = new ApplicationPoolAdvancedSettingsDialog(Module, SelectedItem);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await SelectedItem.Parent.Parent.CommitChangesAsync();
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
        }

        internal async void Basic()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var dialog = new ApplicationPoolBasicSettingsDialog(Module, SelectedItem, null, Items);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await SelectedItem.Parent.Parent.CommitChangesAsync();
            }

            OnApplicationPoolsSettingsSaved();
        }

        public bool IsBusy { get; set; }

        public ApplicationPoolsSettingsSavedEventHandler ApplicationPoolsSettingsUpdated { get; set; }
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
}

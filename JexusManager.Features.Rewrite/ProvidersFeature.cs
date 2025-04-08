// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Features;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    internal class ProvidersFeature : FeatureBase<ProviderItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly ProvidersFeature _owner;

            public FeatureTaskList(ProvidersFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Add", "Add Provider...", string.Empty).SetUsage());

                if (_owner.SelectedItem != null)
                {
                    result.Add(new MethodTaskItem("ViewSettings", "View Settings", string.Empty).SetUsage());
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                if (_owner.CanRevert)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(RevertTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void ViewSettings()
            {
                _owner.ViewSettings();
            }

            [Obfuscation(Exclude = true)]
            public void Edit()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void Rename()
            {
                _owner.Rename();
            }

            [Obfuscation(Exclude = true)]
            public void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Revert()
            {
                _owner.Revert();
            }

            [Obfuscation(Exclude = true)]
            public void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        public ProvidersFeature(Module module)
            : base(module)
        {
        }

        private FeatureTaskList _taskList;

        public bool CanRevert { get; private set; }

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            LoadItems();
        }

        internal protected void OnRewriteSettingsSaved()
        {
            RewriteSettingsUpdated?.Invoke();
        }

        public bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=183852");
            return false;
        }

        public void Add()
        {
            using var dialog = new AddProviderDialog(Module, this, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void ViewSettings()
        {
            var navigationService = (INavigationService)GetService(typeof(INavigationService));
            navigationService.Navigate(null, null, typeof(SettingsPage), SelectedItem);
        }

        public void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(ProviderItem item)
        {
            using var dialog = new AddProviderDialog(Module, this, item);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        public void Rename()
        {
            RenameInline(SelectedItem);
        }

        public void Remove()
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (service.ShowMessage(
                    "Are you sure you want to remove this provider?",
                    "Confirm Remove",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public void Revert()
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result =
                service.ShowMessage(
                    "Reverting to the parent configuration will result in the loss of all settings in the local configuration file for this feature. Are you sure you want to continue?",
                    Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            this.RevertItems();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            var section = service.GetSection("system.webServer/rewrite/providers");
            return section.GetCollection();
        }

        protected override void OnSettingsSaved()
        {
            OnRewriteSettingsSaved();
        }

        public RewriteSettingsSavedEventHandler RewriteSettingsUpdated { get; set; }

        public string Name
        {
            get { return "URL Rewrite Providers"; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        private static readonly Version FxVersionNotRequired = new Version();
    }
}

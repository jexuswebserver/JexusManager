﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using JexusManager.Services;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Module = Microsoft.Web.Management.Client.Module;

    internal class SettingsFeature : FeatureBase<SettingItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly SettingsFeature _owner;

            public FeatureTaskList(SettingsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Add", "Add Setting...", string.Empty).SetUsage());

                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void Edit()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }
        }

        private FeatureTaskList _taskList;
        private readonly ProviderItem _provider;

        public SettingsFeature(Module module, ProviderItem provider)
            : base(module)
        {
            _provider = provider;
        }

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            LoadItems();
        }

        public void Add()
        {
            using var dialog = new AddProviderSettingDialog(Module, this, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(SettingItem item)
        {
            using var dialog = new AddProviderSettingDialog(Module, this, item);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        public void Remove()
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (service.ShowMessage(
                    "Are you sure you want to remove this setting?",
                    "Confirm Remove",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            return _provider.Element.GetCollection("settings");
        }

        protected override void OnSettingsSaved()
        {
            OnSettingsUpdated();
        }

        public bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=183852");
            return false;
        }

        private void OnSettingsUpdated()
        {
            SettingsUpdated?.Invoke();
        }

        public SettingsUpdatedEventHandler SettingsUpdated { get; set; }

        public string Description => "Configure provider settings";

        public virtual Version MinimumFrameworkVersion => new Version();

        public string Name => "Provider Settings";

        public override void InitializeGrouping(ToolStripComboBox cbGroup)
        {
            cbGroup.Items.AddRange(new object[] {
                "No Grouping",
                "Encrypted"
            });
        }

        public override string GetGroupKey(ListViewItem item, string selectedGroup)
        {
            if (selectedGroup == "Encrypted")
            {
                return item.SubItems[2].Text == "Yes" ? "Encrypted" : "Unencrypted";
            }

            return "Unknown";
        }
    }

    public delegate void SettingsUpdatedEventHandler();
}

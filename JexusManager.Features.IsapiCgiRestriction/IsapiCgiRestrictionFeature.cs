﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace JexusManager.Features.IsapiCgiRestriction
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class IsapiCgiRestrictionFeature : FeatureBase<IsapiCgiRestrictionItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly IsapiCgiRestrictionFeature _owner;

            public FeatureTaskList(IsapiCgiRestrictionFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Add", "Add...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    if (_owner.SelectedItem.Allowed)
                    {
                        result.Add(new MethodTaskItem("Deny", "Deny...", string.Empty).SetUsage());
                    }
                    else
                    {
                        result.Add(new MethodTaskItem("Allow", "Allow...", string.Empty).SetUsage());
                    }

                    result.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                result.Add(new MethodTaskItem("Set", "Edit Feature Settings...", string.Empty).SetUsage());

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Edit()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void Set()
            {
                _owner.Set();
            }

            [Obfuscation(Exclude = true)]
            public void Deny()
            {
                _owner.Deny();
            }

            [Obfuscation(Exclude = true)]
            public void Allow()
            {
                _owner.Allow();
            }
        }

        public IsapiCgiRestrictionFeature(Module module)
            : base(module)
        {
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            LoadItems();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            ConfigurationSection section = service.GetSection("system.webServer/security/isapiCgiRestriction", null, false);
            return section.GetCollection();
        }

        public void Add()
        {
            using var dialog = new NewRestrictionDialog(Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void Deny()
        {
            SetAllowed(false);
        }

        public void Allow()
        {
            SetAllowed(true);
        }

        private void SetAllowed(bool allowed)
        {
            SelectedItem.Allowed = allowed;
            SelectedItem.Apply();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public void Set()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/security/isapiCgiRestriction");
            using (var dialog = new SettingsDialog(Module, section, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected restriction?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(IsapiCgiRestrictionItem item)
        {
            using var dialog = new NewRestrictionDialog(Module, item, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        public override void InitializeGrouping(ToolStripComboBox cbGroup)
        {
            cbGroup.Items.AddRange(["No Grouping", "Restriction"]);
        }

        public override string GetGroupKey(ListViewItem item, string selectedGroup)
        {
            switch (selectedGroup)
            {
                case "Restriction":
                    return item.SubItems[1].Text;
                default:
                    return "Unknown";
            }
        }

        protected override void OnSettingsSaved()
        {
            IsapiCgiRestrictionSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210515");
            return false;
        }

        public IsapiCgiRestrictionSettingsSavedEventHandler IsapiCgiRestrictionSettingsUpdated { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name
        {
            get { return "ISAPI and CGI Restrictions"; }
        }
    }
}

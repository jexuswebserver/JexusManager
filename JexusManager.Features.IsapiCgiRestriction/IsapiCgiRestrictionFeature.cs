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
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
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

                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
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
            var dialog = new NewRestrictionDialog(this.Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.AddItem(dialog.Item);
        }

        public void Deny()
        {
            SetAllowed(false);
        }

        public void Allow()
        {
            this.SetAllowed(true);
        }

        private void SetAllowed(bool allowed)
        {
            this.SelectedItem.Allowed = allowed;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public void Set()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/security/isapiCgiRestriction");
            var dialog = new SettingsDialog(this.Module, section);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            service.ServerManager.CommitChanges();
            this.OnSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
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
            var dialog = new NewRestrictionDialog(this.Module, this.SelectedItem, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.EditItem(dialog.Item);
        }

        protected override void OnSettingsSaved()
        {
            this.IsapiCgiRestrictionSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210515");
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

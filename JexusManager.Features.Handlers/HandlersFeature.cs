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

namespace JexusManager.Features.Handlers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class HandlersFeature : FeatureBase<HandlersItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly HandlersFeature _owner;

            public FeatureTaskList(HandlersFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                if (_owner.IsInOrder)
                {
                    result.Add(GetMoveUpTaskItem(_owner.CanMoveUp));
                    result.Add(GetMoveDownTaskItem(_owner.CanMoveDown));
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Unorder", "View Unordered List...", string.Empty).SetUsage());
                }
                else
                {
                    result.Add(new MethodTaskItem("AddManaged", "Add Managed Handler...", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("AddScript", "Add Script Map...", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("AddWildcard", "Add Wildcard Script Map...", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Add", "Add Module Mapping...", string.Empty).SetUsage());
                    if (_owner.SelectedItem != null)
                    {
                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                        result.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
                        result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                        result.Add(RemoveTaskItem);
                    }

                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Set", "Edit Feature Permissions...", string.Empty).SetUsage());
                    if (_owner.CanRevert)
                    {
                        result.Add(RevertTaskItem);
                    }

                    result.Add(new MethodTaskItem("InOrder", "View Ordered List...", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void AddManaged()
            {
                _owner.AddManaged();
            }

            [Obfuscation(Exclude = true)]
            public void AddScript()
            {
                _owner.AddScript();
            }

            [Obfuscation(Exclude = true)]
            public void AddWildcard()
            {
                _owner.AddWildcard();
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
            public void Rename()
            {
                _owner.Rename();
            }

            [Obfuscation(Exclude = true)]
            public override void MoveUp()
            {
                _owner.MoveUp();
            }

            [Obfuscation(Exclude = true)]
            public override void MoveDown()
            {
                _owner.MoveDown();
            }

            [Obfuscation(Exclude = true)]
            public void InOrder()
            {
                _owner.InOrder();
            }

            [Obfuscation(Exclude = true)]
            public void Unorder()
            {
                _owner.Unorder();
            }

            [Obfuscation(Exclude = true)]
            public void Revert()
            {
                _owner.Revert();
            }
        }

        public HandlersFeature(Module module)
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
            Items = new List<HandlersItem>();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));

            // server level modules are in "" location.
            ConfigurationSection section = service.Scope == ManagementScope.Server ? service.GetSection("system.webServer/handlers", string.Empty) : service.GetSection("system.webServer/handlers", null, false);
            AccessPolicy = (long)section["accessPolicy"];
            CanRevert = service.Scope != ManagementScope.Server;
            IsInOrder = false;
            LoadItems();
        }

        public long AccessPolicy { get; set; }

        public void Add()
        {
            using var dialog = new NewMappingDialog(Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void AddManaged()
        {
            using var dialog = new NewHandlerDialog(Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void AddScript()
        {
            using var dialog = new NewScriptMapDialog(Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void AddWildcard()
        {
            using var dialog = new NewWildcardDialog(Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void Set()
        {
            using (var dialog = new PermissionsDialog(Module, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            service.ServerManager.CommitChanges();
            OnSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected authorization rule?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public void Edit()
        {
            HandlersItem newItem;

            if (!string.IsNullOrWhiteSpace(SelectedItem.Type))
            {
                using var dialog = new NewHandlerDialog(Module, SelectedItem, this);
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                newItem = dialog.Item;
            }
            else if (SelectedItem.Modules == "IsapiModule" && !string.IsNullOrWhiteSpace(SelectedItem.ScriptProcessor))
            {
                using var dialog = new NewScriptMapDialog(Module, SelectedItem, this);
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                newItem = dialog.Item;
            }
            else
            {
                using var dialog = new NewMappingDialog(Module, SelectedItem, this);
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                newItem = dialog.Item;
            }

            EditItem(newItem);
        }

        public void Rename()
        {
            RenameInline(SelectedItem);
        }

        public void MoveUp()
        {
            if (Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            MoveUpItem();
        }

        public void MoveDown()
        {
            if (Items.Any(item => item.Flag != "Local"))
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result =
                    dialog.ShowMessage(
                        "The list order will be changed for this feature. If you continue, changes made to this feature at a parent level will no longer be inherited at this level. Do you want to continue?",
                        Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }

            MoveDownItem();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            // server level modules are in "" location.
            ConfigurationSection section = service.Scope == ManagementScope.Server ? service.GetSection("system.webServer/handlers", string.Empty) : service.GetSection("system.webServer/handlers", null, false);
            return section.GetCollection();
        }

        public void InOrder()
        {
            IsInOrder = true;
            OnSettingsSaved();
        }

        public void Unorder()
        {
            IsInOrder = false;
            OnSettingsSaved();
        }

        public void Revert()
        {
            if (!CanRevert)
            {
                throw new InvalidOperationException("Revert operation cannot be done at server level");
            }

            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result =
                dialog.ShowMessage(
                    "Reverting to the parent configuration will result in the loss of all settings in the local configuration file for this feature. Are you sure you want to continue?",
                    Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            RevertItems();
        }

        protected override void OnSettingsSaved()
        {
            HandlersSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210505");
            return false;
        }

        public bool IsInOrder { get; private set; }

        public bool CanRevert { get; private set; }

        public HandlersSettingsSavedEventHandler HandlersSettingsUpdated { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }


        public string Name
        {
            get { return "Handler Mappings"; }
        }
    }
}

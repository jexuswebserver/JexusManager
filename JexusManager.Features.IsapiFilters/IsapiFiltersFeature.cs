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

namespace JexusManager.Features.IsapiFilters
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class IsapiFiltersFeature : FeatureBase<IsapiFiltersItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly IsapiFiltersFeature _owner;

            public FeatureTaskList(IsapiFiltersFeature owner)
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
                    result.Add(new MethodTaskItem("Add", "Add...", string.Empty).SetUsage());
                    if (_owner.SelectedItem != null)
                    {
                        result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                        result.Add(new MethodTaskItem("Edit", "Edit...", string.Empty).SetUsage());
                        result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                        result.Add(RemoveTaskItem);
                    }

                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    if (_owner.CanRevert)
                    {
                        result.Add(new MethodTaskItem("Revert", "Revert to Parent", string.Empty).SetUsage());
                    }

                    result.Add(new MethodTaskItem("InOrder", "View Ordered List...", string.Empty).SetUsage());
                }

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

        public IsapiFiltersFeature(Module module)
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
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            CanRevert = service.Scope != ManagementScope.Server;
            IsInOrder = false;
            LoadItems();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            ConfigurationSection section = service.GetSection("system.webServer/isapiFilters", null, false);
            return section.GetCollection();
        }

        public void Add()
        {
            using var dialog = new NewFilterDialog(Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
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
            using var dialog = new NewFilterDialog(Module, SelectedItem, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        public void Rename()
        {
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
            IsapiFiltersSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("https://go.microsoft.com/fwlink/?LinkId=210516");
            return false;
        }

        public bool IsInOrder { get; private set; }

        public bool CanRevert { get; private set; }

        public IsapiFiltersSettingsSavedEventHandler IsapiFiltersSettingsUpdated { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name
        {
            get { return "ISAPI Filters"; }
        }
    }
}

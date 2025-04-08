// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="RewriteFeature.cs">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Resources;
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
    internal class CustomTagsFeature : FeatureBase<CustomTagsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly CustomTagsFeature _owner;

            public FeatureTaskList(CustomTagsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("AddGroup", "Add Group...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("Add", "Add Custom Tag...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
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
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void AddGroup()
            {
                _owner.AddGroup();
            }

            [Obfuscation(Exclude = true)]
            public void Rename()
            {
                _owner.Edit();
            }
        }

        public CustomTagsFeature(Module module)
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
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            CanRevert = section.CanRevert();
            LoadItems();
        }

        public void AddGroup()
        {
            using (var dialog = new AddCustomTagsDialog(Module))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                AddItem(dialog.Item);
            }
        }

        public void Add()
        {
            using (var dialog = new AddCustomTagDialog(Module))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                SelectedItem.Add(newItem);
                service.ServerManager.CommitChanges();
            }
            OnRewriteSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected entry?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public void Edit()
        {
            RenameInline(SelectedItem);
        }

        internal protected void OnRewriteSettingsSaved()
        {
            RewriteSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130403&amp;clcid=0x409");
            return false;
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            return section.GetCollection("customTags");
        }

        protected override void OnSettingsSaved()
        {
            OnRewriteSettingsSaved();
        }

        public bool CanRevert { get; private set; }

        public RewriteSettingsSavedEventHandler RewriteSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name { get; }
    }
}

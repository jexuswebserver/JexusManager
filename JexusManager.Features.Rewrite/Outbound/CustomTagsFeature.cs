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
    internal class CustomTagsFeature
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
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
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
                _owner.Rename();
            }
        }

        public CustomTagsFeature(Module module)
        {
            this.Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), string.Empty, false);
        }

        protected object GetService(Type type)
        {
            return (this.Module as IServiceProvider).GetService(type);
        }

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            this.Items = new List<CustomTagsItem>();
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            ConfigurationElementCollection rulesCollection = section.GetCollection("customTags");
            foreach (ConfigurationElement ruleElement in rulesCollection)
            {
                var node = new CustomTagsItem(ruleElement);
                this.Items.Add(node);
            }

            this.CanRevert = section.CanRevert();
            this.OnRewriteSettingsSaved();
        }

        public List<CustomTagsItem> Items { get; set; }

        public void AddGroup()
        {
            var dialog = new AddCustomTagsDialog(this.Module);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var rulesSection = service.GetSection("system.webServer/rewrite/outboundRules");
            ConfigurationElementCollection rulesCollection = rulesSection.GetCollection("customTags");

            if (this.SelectedItem != newItem)
            {
                this.Items.Add(newItem);
                this.SelectedItem = newItem;
            }
            else if (newItem.Flag != "Local")
            {
                rulesCollection.Remove(newItem.Element);
                newItem.Flag = "Local";
            }

            newItem.AppendTo(rulesCollection);
            service.ServerManager.CommitChanges();
            this.OnRewriteSettingsSaved();
        }

        public void Add()
        {
            var dialog = new AddCustomTagDialog(this.Module);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            this.SelectedItem.Add(newItem);
            service.ServerManager.CommitChanges();
            this.OnRewriteSettingsSaved();
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected entry?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            this.Items.Remove(this.SelectedItem);
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            ConfigurationElementCollection collection = section.GetCollection("customTags");
            collection.Remove(this.SelectedItem.Element);
            service.ServerManager.CommitChanges();

            this.SelectedItem = null;
            this.OnRewriteSettingsSaved();
        }

        public void Rename()
        {
        }

        internal protected void OnRewriteSettingsSaved()
        {
            this.RewriteSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130403&amp;clcid=0x409");
            return false;
        }

        public CustomTagsItem SelectedItem { get; internal set; }
        public bool CanRevert { get; private set; }

        public RewriteSettingsSavedEventHandler RewriteSettingsUpdated { get; set; }
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

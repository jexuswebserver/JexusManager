// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="RewriteFeature.cs">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
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
    internal class AllowedVariablesFeature
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly AllowedVariablesFeature _owner;

            public FeatureTaskList(AllowedVariablesFeature owner)
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
                    result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                if (_owner.CanRevert)
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Revert", "Revert to Parent", string.Empty).SetUsage());
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
            public void Rename()
            {
                _owner.Rename();
            }

            [Obfuscation(Exclude = true)]
            public void Revert()
            {
                _owner.Revert();
            }
        }

        public AllowedVariablesFeature(Module module)
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
            this.Items = new List<AllowedVariableItem>();
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/allowedServerVariables");
            ConfigurationElementCollection rulesCollection = section.GetCollection();
            foreach (ConfigurationElement ruleElement in rulesCollection)
            {
                var node = new AllowedVariableItem(ruleElement, this);
                this.Items.Add(node);
            }

            this.CanRevert = section.CanRevert();
            this.OnRewriteSettingsSaved();
        }

        public List<AllowedVariableItem> Items { get; set; }

        public void Add()
        {
            var dialog = new AddAllowedVariableDialog(this.Module, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var rulesSection = service.GetSection("system.webServer/rewrite/allowedServerVariables");
            ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();

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
            var section = service.GetSection("system.webServer/rewrite/allowedServerVariables");
            ConfigurationElementCollection collection = section.GetCollection();
            collection.Remove(this.SelectedItem.Element);
            service.ServerManager.CommitChanges();

            this.SelectedItem = null;
            this.OnRewriteSettingsSaved();
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

        public void Revert()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            var result =
                dialog.ShowMessage(
                    "Reverting to the parent configuration will result in the loss of all settings in the local configuration file for this feature. Are you sure you want to continue?",
                    this.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/allowedServerVariables");
            ConfigurationElementCollection collection = section.GetCollection();
            collection.Clear();
            collection.Delete();
            collection = section.GetCollection();

            this.SelectedItem = null;
            this.Items.Clear();
            foreach (ConfigurationElement ruleElement in collection)
            {
                var node = new AllowedVariableItem(ruleElement, this);
                this.Items.Add(node);
            }

            service.ServerManager.CommitChanges();
            this.OnRewriteSettingsSaved();
        }

        private void Rename()
        {
        }

        public AllowedVariableItem SelectedItem { get; internal set; }
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

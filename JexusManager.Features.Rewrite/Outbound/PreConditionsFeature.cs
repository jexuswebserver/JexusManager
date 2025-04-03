﻿// Copyright (c) Lex Li. All rights reserved.
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
    internal class PreConditionsFeature
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly PreConditionsFeature _owner;

            public FeatureTaskList(PreConditionsFeature owner)
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
            public void Edit()
            {
                _owner.Edit();
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

        public PreConditionsFeature(Module module)
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
            service.ShowError(ex, resourceManager.GetString("General"), string.Empty, false);
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
            Items = new List<PreConditionItem>();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            ConfigurationElementCollection rulesCollection = section.GetCollection("preConditions");
            foreach (ConfigurationElement ruleElement in rulesCollection)
            {
                var node = new PreConditionItem(ruleElement);
                Items.Add(node);
            }

            CanRevert = section.CanRevert();
            OnRewriteSettingsSaved();
        }

        public List<PreConditionItem> Items { get; set; }

        public void Add()
        {
            using (var dialog = new AddPreConditionDialog(Module, null))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                var rulesSection = service.GetSection("system.webServer/rewrite/outboundRules");
                ConfigurationElementCollection rulesCollection = rulesSection.GetCollection("preConditions");

                if (SelectedItem != newItem)
                {
                    Items.Add(newItem);
                    SelectedItem = newItem;
                }
                else if (newItem.Flag != "Local")
                {
                    rulesCollection.Remove(newItem.Element);
                    newItem.Flag = "Local";
                }

                newItem.AppendTo(rulesCollection);
                service.ServerManager.CommitChanges();
            }
            OnRewriteSettingsSaved();
        }

        public void Edit()
        {
            // TODO: how to edit.
            using (var dialog = new AddPreConditionDialog(Module, SelectedItem))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var newItem = dialog.Item;
                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                var rulesSection = service.GetSection("system.webServer/rewrite/outboundRules");
                ConfigurationElementCollection rulesCollection = rulesSection.GetCollection("preConditions");

                if (SelectedItem != newItem)
                {
                    Items.Add(newItem);
                    SelectedItem = newItem;
                }
                else if (newItem.Flag != "Local")
                {
                    rulesCollection.Remove(newItem.Element);
                    newItem.Flag = "Local";
                }

                newItem.AppendTo(rulesCollection);

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

            Items.Remove(SelectedItem);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            ConfigurationElementCollection collection = section.GetCollection("preConditions");
            collection.Remove(SelectedItem.Element);
            service.ServerManager.CommitChanges();

            SelectedItem = null;
            OnRewriteSettingsSaved();
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

        public void Revert()
        {
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

            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/rewrite/outboundRules");
            ConfigurationElementCollection collection = section.GetCollection("preConditions");
            collection.Clear();
            collection.Delete();
            collection = section.GetCollection();

            SelectedItem = null;
            Items.Clear();
            foreach (ConfigurationElement ruleElement in collection)
            {
                var node = new PreConditionItem(ruleElement);
                Items.Add(node);
            }

            service.ServerManager.CommitChanges();
            OnRewriteSettingsSaved();
        }

        private void Rename()
        {
        }

        public PreConditionItem SelectedItem { get; internal set; }
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

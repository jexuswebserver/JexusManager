// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="RewriteFeature.cs">
//   
// </copyright>
// 
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
    internal class MapsFeature
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly MapsFeature _owner;

            public FeatureTaskList(MapsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Add", "Add Rewrite Map...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Edit", "Edit Rewrite Map", string.Empty).SetUsage());
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
            public void Edit()
            {
                _owner.Edit();
            }
        }

        public MapsFeature(Module module)
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
            this.Items = new List<MapItem>();
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var rulesSection = service.GetSection("system.webServer/rewrite/rewriteMaps");
            ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();
            foreach (ConfigurationElement ruleElement in rulesCollection)
            {
                var node = new MapItem(ruleElement, this);
                this.Items.Add(node);
            }

            this.OnRewriteSettingsSaved();
        }

        public List<MapItem> Items { get; set; }

        public void Add()
        {
            var dialog = new AddMapsDialog(this.Module, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var rulesSection = service.GetSection("system.webServer/rewrite/rewriteMaps");
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

            this.Edit();
        }

        public void AddRule()
        {
            var dialog = new AddMapDialog(this.Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            ConfigurationElementCollection rulesCollection = this.SelectedItem.Element.GetCollection();

            if (this.SelectedItem.SelectedItem != newItem)
            {
                this.SelectedItem.Items.Add(newItem);
                this.SelectedItem.SelectedItem = newItem;
            }
            else if (newItem.Flag != "Local")
            {
                rulesCollection.Remove(newItem.Element);
                newItem.Flag = "Local";
            }

            newItem.AppendTo(rulesCollection);
            service.ServerManager.CommitChanges();
            this.OnRewriteSettingsSaved();
            this.SelectedItem.OnRewriteSettingsSaved();
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
            var section = service.GetSection("system.webServer/rewrite/rewriteMaps");
            ConfigurationElementCollection collection = section.GetCollection();
            collection.Remove(this.SelectedItem.Element);
            service.ServerManager.CommitChanges();

            this.SelectedItem = null;
            this.OnRewriteSettingsSaved();
        }

        public void RemoveRule()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected entry?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            this.SelectedItem.Items.Remove(this.SelectedItem.SelectedItem);
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            ConfigurationElementCollection collection = this.SelectedItem.Element.GetCollection();
            collection.Remove(this.SelectedItem.SelectedItem.Element);
            service.ServerManager.CommitChanges();

            this.SelectedItem.SelectedItem = null;
            this.OnRewriteSettingsSaved();
            this.SelectedItem.OnRewriteSettingsSaved();
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

        internal void Edit()
        {
            var service = (INavigationService)this.GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(MapPage), new Tuple<MapsFeature, MapItem>(this, this.SelectedItem));
            this.OnRewriteSettingsSaved();
        }

        public void EditRule()
        {
            var dialog = new AddMapDialog(this.Module, this.SelectedItem.SelectedItem, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var newItem = dialog.Item;
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            ConfigurationElementCollection rulesCollection = this.SelectedItem.Element.GetCollection();

            if (this.SelectedItem.SelectedItem != newItem)
            {
                this.SelectedItem.Items.Add(newItem);
                this.SelectedItem.SelectedItem = newItem;
            }
            else if (newItem.Flag != "Local")
            {
                rulesCollection.Remove(newItem.Element);
                newItem.Flag = "Local";
            }

            newItem.AppendTo(rulesCollection);
            service.ServerManager.CommitChanges();
            this.OnRewriteSettingsSaved();
            this.SelectedItem.OnRewriteSettingsSaved();
        }

        public MapItem SelectedItem { get; internal set; }
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

        public void Set()
        {
            var dialog = new MapSettingsDialog(this.Module, this.SelectedItem);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.OnRewriteSettingsSaved();
        }
    }
}

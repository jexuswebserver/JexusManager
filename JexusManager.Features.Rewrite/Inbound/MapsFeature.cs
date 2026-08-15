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
    internal class MapsFeature : FeatureBase<MapItem>
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
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
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
            return _taskList ??= new FeatureTaskList(this);
        }

        internal RewriteModuleProxy Proxy => ((RewriteModule)Module).Proxy;

        public void Load()
        {
            Items.Clear();
            foreach (var map in ((RewriteModule)Module).Proxy.GetMaps())
            {
                Items.Add(map);
            }

            CanRevert = GetService(typeof(IConfigurationService)) is IConfigurationService service && service.GetSection("system.webServer/rewrite/rewriteMaps").CanRevert();
            OnSettingsSaved();
        }

        public void Add()
        {
            using var dialog = new AddMapsDialog(Module, this);
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
                dialog.ShowMessage("Are you sure that you want to remove the selected entry?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
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

        internal void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(MapItem item)
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service.Navigate(null, null, typeof(MapPage), SelectedItem);
            OnRewriteSettingsSaved();
        }

        public void Revert()
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result =
                service.ShowMessage(
                    "Reverting to the parent configuration will result in the loss of all settings in the local configuration file for this feature. Are you sure you want to continue?",
                    Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            RevertItems();
        }

        public override void AddItem(MapItem item)
        {
            ((RewriteModule)Module).Proxy.AddMap(item);
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public override void RemoveItem()
        {
            var item = SelectedItem ?? throw new InvalidOperationException("No rewrite map is selected.");
            ((RewriteModule)Module).Proxy.RemoveMap(item);
            SelectedItem = null;
            Load();
        }

        public override void RevertItems()
        {
            ((RewriteModule)Module).Proxy.RevertMaps();
            SelectedItem = null;
            Load();
        }

        public bool CanRevert { get; private set; } = true;

        public RewriteSettingsSavedEventHandler RewriteSettingsUpdated { get; set; }
        public string Description { get; }


        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name { get; }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            throw new NotSupportedException("Rewrite maps are accessed through the module service.");
        }

        protected override void OnSettingsSaved()
        {
            OnRewriteSettingsSaved();
        }
    }
}

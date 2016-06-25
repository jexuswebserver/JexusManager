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

namespace JexusManager.Features.Caching
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
    internal class CachingFeature : FeatureBase<CachingItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly CachingFeature _owner;

            public FeatureTaskList(CachingFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Add", "Add...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
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
        }

        public CachingFeature(Module module)
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

        public void Add()
        {
            var dialog = new NewCachingDialog(this.Module, null, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.AddItem(dialog.Item);
        }

        public void Set()
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/caching");
            var dialog = new CachingSettingsDialog(this.Module, section);
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
            var dialog = new NewCachingDialog(this.Module, this.SelectedItem, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.EditItem(dialog.Item);
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            var section = service.GetSection("system.webServer/caching");
            return section.GetCollection("profiles");
        }

        protected override void OnSettingsSaved()
        {
            this.CachingSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210522");
            return false;
        }

        public CachingSettingsSavedEventHandler CachingSettingsUpdated { get; set; }

        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name
        {
            get { return "Output Caching"; }
        }
    }
}

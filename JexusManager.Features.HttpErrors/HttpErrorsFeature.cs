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

namespace JexusManager.Features.HttpErrors
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
    internal class HttpErrorsFeature : FeatureBase<HttpErrorsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly HttpErrorsFeature _owner;

            public FeatureTaskList(HttpErrorsFeature owner)
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
                    if (_owner.SelectedItem.Flag == "Local")
                    {
                        result.Add(new MethodTaskItem("Change", "Change Status Code", string.Empty).SetUsage());
                    }

                    result.Add(RemoveTaskItem);
                }

                result.Add(MethodTaskItem.CreateSeparator().SetUsage());
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

        public HttpErrorsFeature(Module module)
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
            Items.Clear();
            Items.AddRange(((HttpErrorsModule)Module).Proxy.GetItems());
            OnSettingsSaved();
        }

        public void Add()
        {
            using var dialog = new NewErrorDialog(Module, null, this);
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
                dialog.ShowMessage("Are you sure that you want to remove the selected custom error page?", "Confirm Remove",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(HttpErrorsItem item)
        {
            using var dialog = new NewErrorDialog(Module, item, this);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        public override void InitializeGrouping(ToolStripComboBox cbGroup)
        {
            cbGroup.Items.AddRange(["No Grouping", "Entry Type", "Type"]);
        }

        public override string GetGroupKey(ListViewItem item, string selectedGroup)
        {
            switch (selectedGroup)
            {
                case "Entry Type":
                    return item.SubItems[3].Text;
                case "Type":
                    return item.SubItems[2].Text;
                default:
                    return string.Empty;
            }
        }

        public void Set()
        {
            var settings = ((HttpErrorsModule)Module).Proxy.GetSettings();
            using (var dialog = new EditDialog(Module, settings, this))
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            ((HttpErrorsModule)Module).Proxy.ApplySettings(settings);
            OnSettingsSaved();
        }



        protected override void OnSettingsSaved()
        {
            HttpErrorsSettingsUpdated?.Invoke();
        }

        public void AddItem(HttpErrorsItem item)
        {
            ((HttpErrorsModule)Module).Proxy.Add(item);
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public void EditItem(HttpErrorsItem item)
        {
            var original = SelectedItem ?? throw new InvalidOperationException("No custom error page is selected.");
            ((HttpErrorsModule)Module).Proxy.Update(original, item);
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public void RemoveItem()
        {
            var item = SelectedItem ?? throw new InvalidOperationException("No custom error page is selected.");
            ((HttpErrorsModule)Module).Proxy.Remove(item);
            SelectedItem = null;
            Load();
        }

        public void MoveUpItem()
        {
            ((HttpErrorsModule)Module).Proxy.MoveUp(SelectedItem);
            Load();
        }

        public void MoveDownItem()
        {
            ((HttpErrorsModule)Module).Proxy.MoveDown(SelectedItem);
            Load();
        }

        public void RevertItems()
        {
            ((HttpErrorsModule)Module).Proxy.Revert();
            SelectedItem = null;
            Load();
        }

        public virtual bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210481");
            return false;
        }

        public HttpErrorsSettingsSavedEventHandler HttpErrorsSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public string Name
        {
            get
            {
                return "Error Pages";
            }
        }
    }
}

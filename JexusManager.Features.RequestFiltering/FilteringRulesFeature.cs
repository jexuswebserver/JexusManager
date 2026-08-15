// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

    using System;
    using System;
namespace JexusManager.Features.RequestFiltering
{
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

    internal class FilteringRulesFeature : RequestFilteringFeature<FilteringRulesItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly FilteringRulesFeature _owner;

            public FeatureTaskList(FilteringRulesFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();

                result.Add(new MethodTaskItem("AddRule", "Add Filtering Rule...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(new MethodTaskItem("EditRule", "Edit Filtering Rule...", string.Empty).SetUsage());
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void AddRule()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void EditRule()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }
        }

        public FilteringRulesFeature(Module module)
            : base(module)
        {
        }

        private TaskList _taskList;

        public override TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Add()
        {
            using var dialog = new NewRuleDialog(this.Module, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void Edit()
        {
            DoubleClick(SelectedItem);
        }

        protected override void DoubleClick(FilteringRulesItem item)
        {
            using var dialog = new NewRuleDialog(Module, item);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            EditItem(dialog.Item);
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected rule?", this.Name,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        public override void Load()
        {
            Items.Clear();
            Items.AddRange(((RequestFilteringModule)Module).Proxy.GetFilteringRules());
            OnSettingsSaved();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            throw new NotSupportedException("Filtering rules are accessed through the module service.");
        }

        public override void AddItem(FilteringRulesItem item)
        {
            ((RequestFilteringModule)Module).Proxy.AddFilteringRule(item);
            LoadAndSelect(item);
        }

        public override void EditItem(FilteringRulesItem item)
        {
            var original = SelectedItem ?? throw new InvalidOperationException("No filtering rule is selected.");
            ((RequestFilteringModule)Module).Proxy.UpdateFilteringRule(original, item);
            LoadAndSelect(item);
        }

        public override void RemoveItem()
        {
            var item = SelectedItem ?? throw new InvalidOperationException("No filtering rule is selected.");
            ((RequestFilteringModule)Module).Proxy.RemoveFilteringRule(item);
            SelectedItem = null;
            Load();
        }

        private void LoadAndSelect(FilteringRulesItem item)
        {
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public override bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210526#Rules");
            return true;
        }

        public override string Name
        {
            get
            {
                return "Rules";
            }
        }
    }
}

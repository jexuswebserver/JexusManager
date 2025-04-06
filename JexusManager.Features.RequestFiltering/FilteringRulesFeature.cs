// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            LoadItems();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            ConfigurationSection requestFilteringSection =
                service.GetSection("system.webServer/security/requestFiltering");
            return requestFilteringSection.GetCollection("filteringRules");
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

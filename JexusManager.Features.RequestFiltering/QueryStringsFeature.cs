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

    internal class QueryStringsFeature : RequestFilteringFeature<QueryStringsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly QueryStringsFeature _owner;

            public FeatureTaskList(QueryStringsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("AddQuery", "Allow Query String...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("AddDenyQuery", "Deny Query String...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void AddQuery()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void AddDenyQuery()
            {
                _owner.AddDeny();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }
        }

        public QueryStringsFeature(Module module)
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
            using var dialog = new NewQueryDialog(Module, true);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void AddDeny()
        {
            using var dialog = new NewQueryDialog(Module, false);
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
                dialog.ShowMessage("Are you sure that you want to remove the selected query string?", Name,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            throw new NotSupportedException("Query string restrictions are accessed through the module service.");
        }

        public override void Load()
        {
            Items.Clear();
            Items.AddRange(((RequestFilteringModule)Module).Proxy.GetQueryStrings());
            OnSettingsSaved();
        }

        public override void AddItem(QueryStringsItem item)
        {
            ((RequestFilteringModule)Module).Proxy.AddQueryString(item);
            LoadAndSelect(item);
        }

        public override void RemoveItem()
        {
            var item = SelectedItem ?? throw new InvalidOperationException("No query string entry is selected.");
            ((RequestFilteringModule)Module).Proxy.RemoveQueryString(item);
            SelectedItem = null;
            Load();
        }

        private void LoadAndSelect(QueryStringsItem item)
        {
            Load();
            SelectedItem = Items.Find(candidate => candidate.Equals(item));
            OnSettingsSaved();
        }

        public override bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210526#Query_Strings");
            return true;
        }

        public override string Name
        {
            get
            {
                return "Query Strings";
            }
        }
    }
}

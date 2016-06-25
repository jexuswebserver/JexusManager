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
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
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
            var dialog = new NewQueryDialog(this.Module, true);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.AddItem(dialog.Item);
        }

        public void AddDeny()
        {
            var dialog = new NewQueryDialog(this.Module, false);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddItem(dialog.Item);
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected query string?", this.Name,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            var section = service.GetSection("system.webServer/security/requestFiltering");
            ConfigurationElement hiddenSegmentsElement = section.ChildElements["alwaysAllowedQueryStrings"];
            return hiddenSegmentsElement.GetCollection();
        }

        protected override ConfigurationElementCollection GetSecondaryCollection(IConfigurationService service)
        {
            var section = service.GetSection("system.webServer/security/requestFiltering");
            ConfigurationElement hiddenSegmentsElement = section.ChildElements["denyQueryStringSequences"];
            return hiddenSegmentsElement.GetCollection();
        }

        public override void Load()
        {
            LoadItems();
        }

        public override bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210526#Query_Strings");
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

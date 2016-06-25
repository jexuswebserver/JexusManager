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

    internal class UrlsFeature : RequestFilteringFeature<UrlsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly UrlsFeature _owner;

            public FeatureTaskList(UrlsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();

                result.Add(new MethodTaskItem("AddUrl", "Allow URL...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("AddDenyUrl", "Deny Sequence...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void AddUrl()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void AddDenyUrl()
            {
                _owner.AddDeny();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }
        }

        public UrlsFeature(Module module)
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
            var dialog = new NewUrlDialog(this.Module, true);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.AddItem(dialog.Item);
        }

        public void AddDeny()
        {
            var dialog = new NewUrlDialog(this.Module, false);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            this.AddItem(dialog.Item);
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)this.GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage("Are you sure that you want to remove the selected URL?", this.Name,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) !=
                DialogResult.Yes)
            {
                return;
            }

            RemoveItem();
        }

        protected override ConfigurationElementCollection GetCollection(IConfigurationService service)
        {
            ConfigurationSection requestFilteringSection =
                service.GetSection("system.webServer/security/requestFiltering");
            return requestFilteringSection.GetCollection("alwaysAllowedUrls");
        }

        protected override ConfigurationElementCollection GetSecondaryCollection(IConfigurationService service)
        {
            ConfigurationSection requestFilteringSection =
                service.GetSection("system.webServer/security/requestFiltering");
            return requestFilteringSection.GetCollection("denyUrlSequences");
        }

        public override void Load()
        {
            LoadItems();
        }

        public override bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210526#URL_Page");
            return true;
        }

        public override string Name
        {
            get
            {
                return "URL";
            }
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Module = Microsoft.Web.Management.Client.Module;

    internal class FileExtensionsFeature : RequestFilteringFeature<FileExtensionsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly FileExtensionsFeature _owner;

            public FeatureTaskList(FileExtensionsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();

                result.Add(new MethodTaskItem("AddExtension", "Allow File Name Extension...", string.Empty).SetUsage());
                result.Add(
                    new MethodTaskItem("AddDenyExtension", "Deny File Name Extension...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void AddExtension()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void AddDenyExtension()
            {
                _owner.AddDeny();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }
        }

        public FileExtensionsFeature(Module module)
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
            CreateExtension(true);
        }

        public void AddDeny()
        {
            CreateExtension(false);
        }

        private void CreateExtension(bool allowed)
        {
            var dialog = new NewExtensionDialog(Module, allowed);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (Items.Any(item => item.Match(dialog.Item)))
            {
                var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                service.ShowMessage(
                    "The file extension specified already exists.",
                    dialog.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            AddItem(dialog.Item);
        }

        public void Remove()
        {
            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (
                dialog.ShowMessage(
                    "Are you sure that you want to remove the selected file extension?",
                    Name,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) != DialogResult.Yes)
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
            ConfigurationSection requestFilteringSection = service.GetSection("system.webServer/security/requestFiltering");
            return requestFilteringSection.GetCollection("fileExtensions");
        }

        public override bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210526");
            return true;
        }

        public override string Name
        {
            get
            {
                return "File Name Extensions";
            }
        }
    }
}

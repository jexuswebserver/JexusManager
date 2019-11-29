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

    internal class HiddenSegmentsFeature : RequestFilteringFeature<HiddenSegmentsItem>
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly HiddenSegmentsFeature _owner;

            public FeatureTaskList(HiddenSegmentsFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();

                result.Add(new MethodTaskItem("AddSegement", "Add Hidden Segment...", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(MethodTaskItem.CreateSeparator().SetUsage());
                    result.Add(RemoveTaskItem);
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void AddSegment()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }
        }

        public HiddenSegmentsFeature(Module module)
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
            using var dialog = new NewHiddenSegmentDialog(this.Module);
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
                dialog.ShowMessage("Are you sure that you want to remove the selected hidden segment?", this.Name,
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
            return requestFilteringSection.GetCollection("hiddenSegments");
        }

        public override bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210526#Hidden_Segments");
            return true;
        }

        public override string Name
        {
            get
            {
                return " Hidden Segments";
            }
        }
    }
}

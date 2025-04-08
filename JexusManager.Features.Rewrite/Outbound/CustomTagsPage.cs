using System.ComponentModel;
// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InboundRulePage.cs" company="LeXtudio">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class CustomTagsPage : ModuleListPage, IModuleChildPage
    {
        private sealed class PageTaskList : DefaultTaskList
        {
            private readonly CustomTagsPage _owner;

            public PageTaskList(CustomTagsPage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[]
                           {
                               GetBackTaskItem("Back", "Back to Rules"),
                               MethodTaskItem.CreateSeparator().SetUsage(),
                               HelpTaskItem
                           };
            }

            [Obfuscation(Exclude = true)]
            public void ShowHelp()
            {
                _owner.ShowHelp();
            }

            [Obfuscation(Exclude = true)]
            public void Back()
            {
                _owner.Back();
            }
        }

        private sealed class CustomTagsListViewItem : ListViewItem, IFeatureListViewItem<CustomTagsItem>
        {
            public CustomTagsItem Item { get; }
            private readonly CustomTagsPage _page;

            public CustomTagsListViewItem(CustomTagsItem item, CustomTagsPage page)
                : base(item.Name)
            {
                this.Item = item;
                _page = page;
                this.SubItems.Add(new ListViewSubItem(this, item.TagString));
                this.SubItems.Add(new ListViewSubItem(this, item.Flag));
            }

            public void Update()
            {
                // TODO: how to update.
            }
        }

        private TaskList _taskList;
        private CustomTagsFeature _feature;

        public CustomTagsPage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new CustomTagsFeature(Module);
            _feature.RewriteSettingsUpdated = this.InitializeListPage;
            _feature.Load();

            _feature.InitializeMouseClick(listView1, (item, text) =>
            {
                item.Name = text;
                item.Apply();
            },
            text =>
            {
                if (_feature.FindDuplicate(item => item.Name, text))
                {
                    var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                    service.ShowMessage("A custom tag with this name already exists.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                return true;
            });
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new CustomTagsListViewItem(file, this));
            }

            _feature.InitializeColumnClick(listView1);

            if (_feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (CustomTagsListViewItem item in listView1.Items)
            {
                if (item.Item == _feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.HandleMouseDoubleClick(listView1);
        }
        
        private void ListView1KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Remove();
            }
        }

        private void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.HandleSelectedIndexChanged(listView1);
            Refresh();
        }

        protected override void Refresh()
        {
            this.Tasks.Fill(tsActionPanel, cmsActionPanel);
            base.Refresh();
        }

        protected override bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
            return true;
        }

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        private void Back()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service?.NavigateBack(1);
        }

        protected override TaskListCollection Tasks
        {
            get
            {
                if (_taskList == null)
                {
                    _taskList = new PageTaskList(this);
                }

                base.Tasks.Add(_feature.GetTaskList());
                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModulePage ParentPage { get; set; }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    public partial class ApplicationPoolsPage : ModuleListPage
    {
        private sealed class PageTaskList : TaskList
        {
            private readonly ApplicationPoolsPage _owner;

            public PageTaskList(ApplicationPoolsPage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[]
                {
                    new MethodTaskItem("ShowHelp", "Help", string.Empty, string.Empty, Resources.help_16).SetUsage()
                };
            }

            [Obfuscation(Exclude = true)]
            public void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class ApplicationPoolsListViewItem : ListViewItem, IFeatureListViewItem<ApplicationPool>
        {
            public ApplicationPool Item { get; }
            private readonly ApplicationPoolsPage _page;

            public ApplicationPoolsListViewItem(ApplicationPool item, ApplicationPoolsPage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, CommonHelper.ToString(Item.State)));
                SubItems.Add(new ListViewSubItem(this, Item.ManagedRuntimeVersion.RuntimeVersionToDisplay2()));
                SubItems.Add(new ListViewSubItem(this, CommonHelper.ToString(Item.ManagedPipelineMode)));
                SubItems.Add(new ListViewSubItem(this, Item.ProcessModel.UserName));
                SubItems.Add(new ListViewSubItem(this, item.ApplicationCount.ToString()));
                ImageIndex = item.State == ObjectState.Started ? 0 : 1;
            }
        }

        private ApplicationPoolsFeature _feature;
        private TaskList _taskList;

        public ApplicationPoolsPage()
        {
            InitializeComponent();
            btnGo.Image = DefaultTaskList.GoImage;
            btnShowAll.Image = DefaultTaskList.ShowAllImage;

            imageList1.Images.Add(Resources.application_pools_16);
            imageList1.Images.Add(Resources.application_pools_stopped_16);
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new ApplicationPoolsFeature(Module);
            _feature.ApplicationPoolsSettingsUpdated = InitializeListPage;
            _feature.Load();

            _feature.HandleMouseClick(listView1, (item, text) =>
            {
                item.Name = text;
                item.Apply();
            });
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (ApplicationPool file in _feature.Items)
            {
                listView1.Items.Add(new ApplicationPoolsListViewItem(file, this));
            }

            if (_feature.SelectedItem != null)
            {
                foreach (ApplicationPoolsListViewItem item in listView1.Items)
                {
                    if (item.Item.Name == _feature.SelectedItem.Name)
                    {
                        item.Selected = true;
                    }
                }
            }

            Refresh();
        }

        protected override void Refresh()
        {
            Tasks.Fill(tsActionPanel, cmsActionPanel);
            base.Refresh();
        }

        protected override bool ShowHelp()
        {
            return _feature.ShowHelp();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
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

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Remove();
            }
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.HandleMouseDoubleClick(listView1);
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.HandleSelectedIndexChanged(listView1);
            Refresh();
        }        
    }
}

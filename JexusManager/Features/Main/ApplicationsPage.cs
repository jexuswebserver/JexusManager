// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Application = Microsoft.Web.Administration.Application;

    public partial class ApplicationsPage : ModuleListPage
    {
        private sealed class PageTaskList : TaskList
        {
            private readonly ApplicationsPage _owner;

            public PageTaskList(ApplicationsPage owner)
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

        private sealed class ApplicationsListViewItem : ListViewItem, IFeatureListViewItem<Application>
        {
            public Application Item { get; }
            private readonly ApplicationsPage _page;

            public ApplicationsListViewItem(Application item, ApplicationsPage page)
                : base(page._site == null && item.Path == "/" ? "Root Application" : item.Path)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, Item.PhysicalPath));
                SubItems.Add(new ListViewSubItem(this, Item.Site.Name));
                SubItems.Add(new ListViewSubItem(this, item.GetPoolName()));
                ImageIndex = 0;
            }
        }

        private ApplicationsFeature _feature;
        private PageTaskList _taskList;
        private List<Application> _applications;
        private Site _site;

        public ApplicationsPage()
        {
            InitializeComponent();
            btnGo.Image = DefaultTaskList.GoImage;
            btnShowAll.Image = DefaultTaskList.ShowAllImage;

            imageList1.Images.Add(Resources.application_16);
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            var data = navigationData as Tuple<List<Application>, Site>;
            _applications = data.Item1;
            _site = data.Item2;
            if (_applications == null && _site == null)
            {
                throw new InvalidOperationException("Site object required");
            }

            _feature = new ApplicationsFeature(Module);
            _feature.ApplicationsSettingsUpdated = InitializeListPage;
            _feature.Load(_applications, _site);
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (Application app in _feature.Items)
            {
                listView1.Items.Add(new ApplicationsListViewItem(app, this));
            }

            if (_feature.SelectedItem != null)
            {
                foreach (ApplicationsListViewItem item in listView1.Items)
                {
                    if (item.Item.Path == _feature.SelectedItem.Path)
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

        private void cbFilter_TextChanged(object sender, EventArgs e)
        {
            btnGo.Enabled = !string.IsNullOrWhiteSpace(cbFilter.Text);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            cbFilter.Text = string.Empty;
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

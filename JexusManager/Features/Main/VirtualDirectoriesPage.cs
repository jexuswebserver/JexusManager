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
    using Application = Microsoft.Web.Administration.Application;

    public partial class VirtualDirectoriesPage : ModuleListPage
    {
        private sealed class PageTaskList : TaskList
        {
            private readonly VirtualDirectoriesPage _owner;

            public PageTaskList(VirtualDirectoriesPage owner)
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

        private sealed class VirtualDirectoriesListViewItem : ListViewItem, IFeatureListViewItem<VirtualDirectory>
        {
            public VirtualDirectory Item { get; }
            private readonly VirtualDirectoriesPage _page;

            public VirtualDirectoriesListViewItem(VirtualDirectory item, VirtualDirectoriesPage page)
                : base(item.Application.IsRoot() ? "Root Application" : item.Application.Path) // TODO: miss the icon in this column.
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, Item.Path));
                SubItems.Add(new ListViewSubItem(this, Item.PhysicalPath));
                SubItems.Add(new ListViewSubItem(this, Item.UserName ?? string.Empty));
                ImageIndex = 0;
            }
        }

        private VirtualDirectoriesFeature _feature;
        private PageTaskList _taskList;
        private Application _application;

        public VirtualDirectoriesPage()
        {
            InitializeComponent();
            imageList1.Images.Add(Resources.virtual_directory_16);
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _application = navigationData as Application;
            if (_application == null)
            {
                throw new InvalidOperationException("Application object required");
            }

            _feature = new VirtualDirectoriesFeature(Module);
            _feature.VirtualDirectoriesSettingsUpdated = InitializeListPage;
            _feature.Load(_application);
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (VirtualDirectory vdir in _feature.Items)
            {
                listView1.Items.Add(new VirtualDirectoriesListViewItem(vdir, this));
            }

            _feature.InitializeColumnClick(listView1);

            if (_feature.SelectedItem != null)
            {
                foreach (VirtualDirectoriesListViewItem item in listView1.Items)
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

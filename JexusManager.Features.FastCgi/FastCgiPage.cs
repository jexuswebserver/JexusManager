// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class FastCgiPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly FastCgiPage _owner;

            public PageTaskList(FastCgiPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class FastCgiListViewItem : ListViewItem
        {
            public FastCgiItem Item { get; }

            private readonly FastCgiPage _page;

            public FastCgiListViewItem(FastCgiItem item, FastCgiPage page)
                : base(item.Path)
            {
                this.Item = item;
                _page = page;
                this.SubItems.Add(new ListViewSubItem(this, item.Arguments));
                this.SubItems.Add(new ListViewSubItem(this, item.MaxInstances.ToString()));
                this.SubItems.Add(new ListViewSubItem(this, item.InstanceMaxRequests.ToString()));
            }
        }

        private FastCgiFeature _feature;
        private PageTaskList _taskList;

        public FastCgiPage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new FastCgiFeature(this.Module);
            _feature.FastCgiSettingsUpdated = this.InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new FastCgiListViewItem(file, this));
            }

            if (_feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (FastCgiListViewItem item in listView1.Items)
            {
                if (item.Item == _feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        protected override void Refresh()
        {
            this.Tasks.Fill(tsActionPanel, cmsActionPanel);
            base.Refresh();
        }

        private void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.SelectedItem = listView1.SelectedItems.Count > 0
                ? ((FastCgiListViewItem)listView1.SelectedItems[0]).Item
                : null;
            // TODO: optimize refresh when null to not null (vice versa)
            this.Refresh();
        }

        protected override bool ShowHelp()
        {
            _feature.ShowHelp();
            return true;
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

        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.Edit();
        }
    }
}

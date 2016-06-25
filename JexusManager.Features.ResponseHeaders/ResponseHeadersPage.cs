// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.ResponseHeaders
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class ResponseHeadersPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly ResponseHeadersPage _owner;

            public PageTaskList(ResponseHeadersPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class ResponseHeadersListViewItem : ListViewItem
        {
            public ResponseHeadersItem Item { get; }
            private readonly ResponseHeadersPage _page;

            public ResponseHeadersListViewItem(ResponseHeadersItem item, ResponseHeadersPage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Value));
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private ResponseHeadersFeature _feature;
        private PageTaskList _taskList;

        public ResponseHeadersPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new ResponseHeadersFeature(Module);
            _feature.ResponseHeadersSettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new ResponseHeadersListViewItem(file, this));
            }

            if (_feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (ResponseHeadersListViewItem item in listView1.Items)
            {
                if (item.Item == _feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        protected override void Refresh()
        {
            Tasks.Fill(tsActionPanel, cmsActionPanel);
            base.Refresh();
        }

        private void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.SelectedItem = listView1.SelectedItems.Count > 0
                ? ((ResponseHeadersListViewItem)listView1.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.Edit();
        }

        protected override bool ShowHelp()
        {
            _feature.ShowHelp();
            return true;
        }

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
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
    }
}

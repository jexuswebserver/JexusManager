// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class TraceFailedRequestsPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly TraceFailedRequestsPage _owner;

            public PageTaskList(TraceFailedRequestsPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class TraceFailedRequestsListViewItem : ListViewItem, IFeatureListViewItem<TraceFailedRequestsItem>
        {
            public TraceFailedRequestsItem Item { get; }
            private readonly TraceFailedRequestsPage _page;
            public TraceFailedRequestsListViewItem(TraceFailedRequestsItem item, TraceFailedRequestsPage page)
                : base(item.Path)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.GetProviders()));
                SubItems.Add(new ListViewSubItem(this, item.Codes));
                SubItems.Add(new ListViewSubItem(this, item.TimeTaken.ToString()));
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private TraceFailedRequestsFeature _feature;
        private PageTaskList _taskList;

        public TraceFailedRequestsPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new TraceFailedRequestsFeature(Module);
            _feature.TraceFailedRequestsSettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            toolStrip2.Visible = !_feature.IsInOrder;
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new TraceFailedRequestsListViewItem(file, this));
            }

            if (_feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (TraceFailedRequestsListViewItem item in listView1.Items)
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
            _feature.HandleSelectedIndexChanged(listView1);
            Refresh();
        }

        private void ListView1KeyDown(object sender, KeyEventArgs e)
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
    }
}

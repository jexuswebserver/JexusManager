// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class HttpErrorsPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly HttpErrorsPage _owner;

            public PageTaskList(HttpErrorsPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class CustomErrorsListViewItem : ListViewItem, IFeatureListViewItem<HttpErrorsItem>
        {
            public HttpErrorsItem Item { get; }
            private readonly HttpErrorsPage _page;

            public CustomErrorsListViewItem(HttpErrorsItem item, HttpErrorsPage page)
                : base(item.Code)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.FullPath));
                SubItems.Add(new ListViewSubItem(this, item.Response));
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private HttpErrorsFeature _feature;
        private PageTaskList _taskList;

        public HttpErrorsPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new HttpErrorsFeature(Module);
            _feature.HttpErrorsSettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new CustomErrorsListViewItem(file, this));
            }

            _feature.InitializeColumnClick(listView1);
            _feature.InitializeGrouping(cbGroup);

            if (_feature.SelectedItem != null)
            {
                foreach (CustomErrorsListViewItem item in listView1.Items)
                {
                    if (item.Item == _feature.SelectedItem)
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

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Remove();
            }
        }

        private void ListView1_MouseDoubleClick(object sender, EventArgs e)
        {
            _feature.HandleMouseDoubleClick(listView1);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.HandleSelectedIndexChanged(listView1);
            Refresh();
        }

        private void CbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogHelper.HandleGrouping(listView1, cbGroup.SelectedItem.ToString(), _feature.GetGroupKey);
        }

        protected override bool ShowHelp()
        {
            _feature.ShowHelp();
            return true;
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
    }
}

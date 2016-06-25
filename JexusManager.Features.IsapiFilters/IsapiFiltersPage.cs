// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiFilters
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class IsapiFiltersPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly IsapiFiltersPage _owner;

            public PageTaskList(IsapiFiltersPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class IsapiFiltersListViewItem : ListViewItem
        {
            public IsapiFiltersItem Item { get; }

            private readonly IsapiFiltersPage _page;

            public IsapiFiltersListViewItem(IsapiFiltersItem item, IsapiFiltersPage page)
                : base(item.Name)
            {
                this.Item = item;
                _page = page;
                this.SubItems.Add(new ListViewSubItem(this, item.Path));
                this.SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private IsapiFiltersFeature _feature;
        private PageTaskList _taskList;

        public IsapiFiltersPage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new IsapiFiltersFeature(this.Module);
            _feature.IsapiFiltersSettingsUpdated = this.InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            toolStrip2.Visible = !_feature.IsInOrder;
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new IsapiFiltersListViewItem(file, this));
            }

            if (_feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (IsapiFiltersListViewItem item in listView1.Items)
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
                ? ((IsapiFiltersListViewItem)listView1.SelectedItems[0]).Item
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
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Caching
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class CachingPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly CachingPage _owner;

            public PageTaskList(CachingPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class CachingListViewItem : ListViewItem
        {
            public CachingItem Item { get; }

            private readonly CachingPage _page;

            public CachingListViewItem(CachingItem item, CachingPage page)
                : base(item.Extension)
            {
                this.Item = item;
                _page = page;
                this.SubItems.Add(new ListViewSubItem(this, ToString(item.Policy)));
                this.SubItems.Add(new ListViewSubItem(this, ToString(item.KernelCachePolicy)));
                this.SubItems.Add(new ListViewSubItem(this, item.Flag));
            }

            private static string ToString(long policy)
            {
                switch (policy)
                {
                    case 0L:
                        return "Do not cache";
                    case 1L:
                        return "Cache until change";
                    case 2:
                        return "Cache for time period";
                    case 3:
                        return "Prevent all caching";
                }

                return string.Empty;
            }
        }

        private CachingFeature _feature;
        private PageTaskList _taskList;

        public CachingPage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new CachingFeature(this.Module);
            _feature.CachingSettingsUpdated = this.InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new CachingListViewItem(file, this));
            }

            if (_feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (CachingListViewItem item in listView1.Items)
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
                ? ((CachingListViewItem)listView1.SelectedItems[0]).Item
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

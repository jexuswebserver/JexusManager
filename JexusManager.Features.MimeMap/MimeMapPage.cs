// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.MimeMap
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class MimeMapPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly MimeMapPage _owner;

            public PageTaskList(MimeMapPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class MimeMapListViewItem : ListViewItem
        {
            public MimeMapItem Item { get; }

            private readonly MimeMapPage _page;

            public MimeMapListViewItem(MimeMapItem item, MimeMapPage page)
                : base(item.FileExtension)
            {
                this.Item = item;
                _page = page;
                this.SubItems.Add(new ListViewSubItem(this, item.MimeType));
                this.SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private MimeMapFeature _feature;
        private PageTaskList _taskList;

        public MimeMapPage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new MimeMapFeature(this.Module);
            _feature.MimeMapSettingsUpdated = this.InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new MimeMapListViewItem(file, this));
            }

            if (_feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (MimeMapListViewItem item in listView1.Items)
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.SelectedItem = listView1.SelectedItems.Count > 0
                ? ((MimeMapListViewItem)listView1.SelectedItems[0]).Item
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

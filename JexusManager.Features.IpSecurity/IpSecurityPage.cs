// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class IpSecurityPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly IpSecurityPage _owner;

            public PageTaskList(IpSecurityPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class IpSecurityListViewItem : ListViewItem, IFeatureListViewItem<IpSecurityItem>
        {
            public IpSecurityItem Item { get; }
            private readonly IpSecurityPage _page;

            public IpSecurityListViewItem(IpSecurityItem item, IpSecurityPage page)
                : base(item.Allowed ? "Allow" : "Deny")
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Mask == string.Empty ? item.Address : string.Format("{0}({1})", item.Address, item.Mask)));
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private IpSecurityFeature _feature;
        private PageTaskList _taskList;

        public IpSecurityPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new IpSecurityFeature(Module);
            _feature.IpSecuritySettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new IpSecurityListViewItem(file, this));
            }

            _feature.InitializeColumnClick(listView1);
            _feature.InitializeGrouping(cbGroup);

            if (_feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (IpSecurityListViewItem item in listView1.Items)
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

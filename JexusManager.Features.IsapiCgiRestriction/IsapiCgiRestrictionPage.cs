﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiCgiRestriction
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class IsapiCgiRestrictionPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly IsapiCgiRestrictionPage _owner;

            public PageTaskList(IsapiCgiRestrictionPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class IsapiCgiRestrictionListViewItem : ListViewItem, IFeatureListViewItem<IsapiCgiRestrictionItem>
        {
            public IsapiCgiRestrictionItem Item { get; }

            private readonly IsapiCgiRestrictionPage _page;

            public IsapiCgiRestrictionListViewItem(IsapiCgiRestrictionItem item, IsapiCgiRestrictionPage page)
                : base(item.Description)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Allowed ? "Allowed" : "Not Allowed"));
                SubItems.Add(new ListViewSubItem(this, item.Path));
            }
        }

        private IsapiCgiRestrictionFeature _feature;
        private PageTaskList _taskList;

        public IsapiCgiRestrictionPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new IsapiCgiRestrictionFeature(Module);
            _feature.IsapiCgiRestrictionSettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new IsapiCgiRestrictionListViewItem(file, this));
            }

            _feature.InitializeColumnClick(listView1);
            _feature.InitializeGrouping(cbGroup);

            if (_feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (IsapiCgiRestrictionListViewItem item in listView1.Items)
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

        private void ListView1KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Remove();
            }
        }

        private void ListView1MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.HandleMouseDoubleClick(listView1);
        }

        private void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.HandleSelectedIndexChanged(listView1);
            Refresh();
        }

        private void CbGroupSelectedIndexChanged(object sender, EventArgs e)
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

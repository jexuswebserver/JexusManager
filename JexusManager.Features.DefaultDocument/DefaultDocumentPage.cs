// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.DefaultDocument
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class DefaultDocumentPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly DefaultDocumentPage _owner;

            public PageTaskList(DefaultDocumentPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class DefaultDocumentListViewItem : ListViewItem
        {
            public DocumentItem Item { get; }
            private readonly DefaultDocumentPage _page;

            public DefaultDocumentListViewItem(DocumentItem item, DefaultDocumentPage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private DefaultDocumentFeature _feature;
        private PageTaskList _taskList;

        public DefaultDocumentPage()
        {
            InitializeComponent();
            btnView.Image = DefaultTaskList.ViewImage;
            btnGo.Image = DefaultTaskList.GoImage;
            btnShowAll.Image = DefaultTaskList.ShowAllImage;
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new DefaultDocumentFeature(Module);
            _feature.DefaultDocumentSettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new DefaultDocumentListViewItem(file, this));
            }

            if (_feature.SelectedItem != null)
            {
                foreach (DefaultDocumentListViewItem item in listView1.Items)
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: use WatchDog to suppress extra refresh (one selection triggers this event twice)
            _feature.SelectedItem = listView1.SelectedItems.Count > 0
                ? ((DefaultDocumentListViewItem)listView1.SelectedItems[0]).Item
                : null;
            Refresh();
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
    }
}

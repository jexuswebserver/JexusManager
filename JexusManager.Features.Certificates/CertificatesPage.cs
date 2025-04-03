// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    public partial class CertificatesPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly CertificatesPage _owner;

            public PageTaskList(CertificatesPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class CertificatesListViewItem : ListViewItem, IFeatureListViewItem<CertificatesItem>
        {
            public CertificatesItem Item { get; }
            private readonly CertificatesPage _page;

            public CertificatesListViewItem(CertificatesItem item, CertificatesPage page)
                : base(item.Item.FriendlyName)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Item.GetNameInfo(X509NameType.SimpleName, false)));
                SubItems.Add(new ListViewSubItem(this, item.Item.GetNameInfo(X509NameType.SimpleName, true)));
                SubItems.Add(new ListViewSubItem(this, item.Item.GetExpirationDateString()));
                SubItems.Add(new ListViewSubItem(this, item.Item.GetCertHashString()));
                SubItems.Add(new ListViewSubItem(this, item.Store));
            }
        }

        private CertificatesFeature _feature;
        private PageTaskList _taskList;

        public CertificatesPage()
        {
            InitializeComponent();
            btnGo.Image = DefaultTaskList.GoImage;
            btnShowAll.Image = DefaultTaskList.ShowAllImage;
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new CertificatesFeature(Module);
            _feature.CertificatesSettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in _feature.Items)
            {
                listView1.Items.Add(new CertificatesListViewItem(file, this));
            }

            if (_feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (CertificatesListViewItem item in listView1.Items)
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
        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.HandleMouseDoubleClick(listView1);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.HandleSelectedIndexChanged(listView1);
            Refresh();
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

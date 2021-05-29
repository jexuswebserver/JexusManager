// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpApi
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    using Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Cryptography;

    internal partial class HttpApiPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly HttpApiPage _owner;

            public PageTaskList(HttpApiPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class SniMappingListViewItem : ListViewItem
        {
            public SniMappingItem Item { get; }
            private readonly HttpApiPage _page;

            public SniMappingListViewItem(SniMappingItem item, HttpApiPage page)
                : base(item.Host)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Port));
                SubItems.Add(new ListViewSubItem(this, BindingUtility.GetAppName(item.AppId)));
                SubItems.Add(new ListViewSubItem(this, item.AppId));
                SubItems.Add(new ListViewSubItem(this, item.Hash));
                SubItems.Add(new ListViewSubItem(this, item.Store));

                string flag = "Broken";
                using X509Store personal = new X509Store(item.Store, StoreLocation.LocalMachine);
                try
                {
                    personal.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var found = personal.Certificates.Find(X509FindType.FindByThumbprint, item.Hash, false);
                    if (found.Count > 0)
                    {
                        flag = "Healthy";
                    }

                    personal.Close();
                }
                catch (CryptographicException)
                {
                    flag = "Unknown";
                }

                SubItems.Add(new ListViewSubItem(this, flag));
            }
        }

        private sealed class IpMappingListViewItem : ListViewItem
        {
            public IpMappingItem Item { get; }
            private readonly HttpApiPage _page;

            public IpMappingListViewItem(IpMappingItem item, HttpApiPage page)
                : base(item.Address)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Port));
                SubItems.Add(new ListViewSubItem(this, BindingUtility.GetAppName(item.AppId)));
                SubItems.Add(new ListViewSubItem(this, item.AppId));
                SubItems.Add(new ListViewSubItem(this, item.Hash));
                SubItems.Add(new ListViewSubItem(this, item.Store));

                string flag = "Broken";
                using X509Store personal = new X509Store(item.Store, StoreLocation.LocalMachine);
                try
                {
                    personal.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    var found = personal.Certificates.Find(X509FindType.FindByThumbprint, item.Hash, false);
                    if (found.Count > 0)
                    {
                        flag = "Healthy";
                    }

                    personal.Close();
                }
                catch (CryptographicException)
                {
                    flag = "Unknown";
                }

                SubItems.Add(new ListViewSubItem(this, flag));
            }
        }

        private sealed class ReservedUrlListViewItem : ListViewItem
        {
            public ReservedUrlsItem Item { get; }
            private readonly HttpApiPage _page;

            public ReservedUrlListViewItem(ReservedUrlsItem item, HttpApiPage page)
                : base(item.UrlPrefix)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.SecurityDescriptor));
            }
        }

        private PageTaskList _taskList;

        public HttpApiPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            var ip = new IpMappingFeature(Module);
            ip.HttpApiSettingsUpdate = RefreshIp;
            ip.Load();
            tpIP.Tag = ip;
            RefreshIp();

            var sni = new SniMappingFeature(Module);
            sni.HttpApiSettingsUpdate = RefreshSni;
            sni.Load();
            tpSNI.Tag = sni;

            var url = new ReservedUrlsFeature(Module);
            url.HttpApiSettingsUpdate = RefreshUrl;
            url.Load();
            tpURL.Tag = url;
        }

        private void RefreshSni()
        {
            var feature = (SniMappingFeature)tpSNI.Tag;
            if (feature == null)
            {
                return;
            }

            lvSni.Items.Clear();
            foreach (var file in feature.Items)
            {
                lvSni.Items.Add(new SniMappingListViewItem(file, this));
            }

            if (feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (SniMappingListViewItem item in lvSni.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshIp()
        {
            var feature = (IpMappingFeature)tpIP.Tag;
            if (feature == null)
            {
                return;
            }

            lvIP.Items.Clear();
            foreach (var file in feature.Items)
            {
                lvIP.Items.Add(new IpMappingListViewItem(file, this));
            }

            if (feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (IpMappingListViewItem item in lvIP.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshUrl()
        {
            var feature = (ReservedUrlsFeature)tpURL.Tag;
            if (feature == null)
            {
                return;
            }

            lvURL.Items.Clear();
            foreach (var file in feature.Items)
            {
                lvURL.Items.Add(new ReservedUrlListViewItem(file, this));
            }

            if (feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (ReservedUrlListViewItem item in lvURL.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        protected override void InitializeListPage()
        {
        }

        protected override void Refresh()
        {
            var feature = (IHttpApiFeature)tabControl1.SelectedTab.Tag;
            var extra = feature?.GetTaskList();
            Tasks.Fill(tsActionPanel, cmsActionPanel, extra);
            base.Refresh();
        }

        private void LvSniSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (SniMappingFeature)tpSNI.Tag;
            feature.SelectedItem = lvSni.SelectedItems.Count > 0
                ? ((SniMappingListViewItem)lvSni.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void LvIPSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (IpMappingFeature)tpIP.Tag;
            feature.SelectedItem = lvIP.SelectedItems.Count > 0
                ? ((IpMappingListViewItem)lvIP.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void LvURLSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (ReservedUrlsFeature)tpURL.Tag;
            feature.SelectedItem = lvURL.SelectedItems.Count > 0
                ? ((ReservedUrlListViewItem)lvURL.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        protected override bool ShowHelp()
        {
            var feature = (IHttpApiFeature)tabControl1.SelectedTab.Tag;
            feature.ShowHelp();
            return true;
        }

        private void TabControl1SelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (IHttpApiFeature)tabControl1.SelectedTab.Tag;
            feature.HttpApiSettingsUpdate.Invoke();
            Refresh();
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

                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }
    }
}

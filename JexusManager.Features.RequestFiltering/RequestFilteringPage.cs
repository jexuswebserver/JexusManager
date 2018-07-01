// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class RequestFilteringPage : ModuleListPage
    {
        private sealed class PageTaskList : DefaultTaskList
        {
            private readonly RequestFilteringPage _owner;

            public PageTaskList(RequestFilteringPage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Set", "Edit Feature Settings...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                result.Add(HelpTaskItem);
                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Set()
            {
                _owner.Set();
            }

            [Obfuscation(Exclude = true)]
            public void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class ExtensionListViewItem : ListViewItem
        {
            public FileExtensionsItem Item { get; }
            private readonly RequestFilteringPage _page;

            public ExtensionListViewItem(FileExtensionsItem item, RequestFilteringPage page)
                : base(item.Extension)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Allowed ? "True" : "False"));
            }
        }

        private sealed class RuleListViewItem : ListViewItem
        {
            public FilteringRulesItem Item { get; }
            private readonly RequestFilteringPage _page;

            public RuleListViewItem(FilteringRulesItem item, RequestFilteringPage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.ScanString));
                SubItems.Add(new ListViewSubItem(this, item.AppliesToString));
                SubItems.Add(new ListViewSubItem(this, item.DenyStringsString));
            }
        }

        private sealed class SegmentListViewItem : ListViewItem
        {
            public HiddenSegmentsItem Item { get; }
            private readonly RequestFilteringPage _page;

            public SegmentListViewItem(HiddenSegmentsItem item, RequestFilteringPage page)
                : base(item.Segment)
            {
                Item = item;
                _page = page;
            }
        }

        private sealed class UrlListViewItem : ListViewItem
        {
            public UrlsItem Item { get; }
            private readonly RequestFilteringPage _page;

            public UrlListViewItem(UrlsItem item, RequestFilteringPage page)
                : base(item.Url)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Allowed ? "always allow" : "Deny"));
            }
        }

        private sealed class VerbListViewItem : ListViewItem
        {
            public VerbsItem Item { get; }
            private readonly RequestFilteringPage _page;

            public VerbListViewItem(VerbsItem item, RequestFilteringPage page)
                : base(item.Verb)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Allowed ? "True" : "False"));
            }
        }

        private sealed class HeaderListViewItem : ListViewItem
        {
            public HeadersItem Item { get; }
            private readonly RequestFilteringPage _page;

            public HeaderListViewItem(HeadersItem item, RequestFilteringPage page)
                : base(item.Header)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.SizeLimit.ToString()));
            }
        }

        private sealed class QueryListViewItem : ListViewItem
        {
            public QueryStringsItem Item { get; }
            private readonly RequestFilteringPage _page;

            public QueryListViewItem(QueryStringsItem item, RequestFilteringPage page)
                : base(item.QueryString)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Allowed ? "Always allow" : "Deny"));
            }
        }

        private PageTaskList _taskList;

        public RequestFilteringPage()
        {
            InitializeComponent();
            imageList1.Images.Add(Resources.extensions_16);
            imageList1.Images.Add(Resources.map_16);
            imageList1.Images.Add(Resources.hidden_16);
            imageList1.Images.Add(Resources.url_16);
            imageList1.Images.Add(Resources.verbs_16);
            imageList1.Images.Add(Resources.headers_16);
            imageList1.Images.Add(Resources.query_16);
            tpExtensions.ImageIndex = 0;
            tpRules.ImageIndex = 1;
            tpSegments.ImageIndex = 2;
            tpUrl.ImageIndex = 3;
            tpVerbs.ImageIndex = 4;
            tpHeaders.ImageIndex = 5;
            tpQuery.ImageIndex = 6;
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            var extensions = new FileExtensionsFeature(Module);
            extensions.RequestFilteringSettingsUpdate = RefreshExtensions;
            extensions.Load();
            tpExtensions.Tag = extensions;
            RefreshExtensions();

            var rules = new FilteringRulesFeature(Module);
            rules.RequestFilteringSettingsUpdate = RefreshRules;
            rules.Load();
            tpRules.Tag = rules;

            var segments = new HiddenSegmentsFeature(Module);
            segments.RequestFilteringSettingsUpdate = RefreshSegments;
            segments.Load();
            tpSegments.Tag = segments;

            var urls = new UrlsFeature(Module);
            urls.RequestFilteringSettingsUpdate = RefreshUrls;
            urls.Load();
            tpUrl.Tag = urls;

            var verbs = new VerbsFeature(Module);
            verbs.RequestFilteringSettingsUpdate = RefreshVerbs;
            verbs.Load();
            tpVerbs.Tag = verbs;

            var headers = new HeadersFeature(Module);
            headers.RequestFilteringSettingsUpdate = RefreshHeaders;
            headers.Load();
            tpHeaders.Tag = headers;

            var queries = new QueryStringsFeature(Module);
            queries.RequestFilteringSettingsUpdate = RefreshQueries;
            queries.Load();
            tpQuery.Tag = queries;
        }

        protected override void InitializeListPage()
        {
        }

        private void RefreshExtensions()
        {
            var feature = (FileExtensionsFeature)tpExtensions.Tag;
            if (feature == null)
            {
                return;
            }

            lvExtensions.Items.Clear();
            foreach (var file in feature.Items)
            {
                lvExtensions.Items.Add(new ExtensionListViewItem(file, this));
            }

            if (feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (ExtensionListViewItem item in lvExtensions.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshRules()
        {
            var feature = (FilteringRulesFeature)tpRules.Tag;
            if (feature == null)
            {
                return;
            }

            lvRules.Items.Clear();
            foreach (var file in feature.Items)
            {
                lvRules.Items.Add(new RuleListViewItem(file, this));
            }

            if (feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (RuleListViewItem item in lvRules.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshSegments()
        {
            var feature = (HiddenSegmentsFeature)tpSegments.Tag;
            if (feature == null)
            {
                return;
            }

            lvSegments.Items.Clear();
            foreach (var file in feature.Items)
            {
                lvSegments.Items.Add(new SegmentListViewItem(file, this));
            }

            if (feature.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (SegmentListViewItem item in lvSegments.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshUrls()
        {
            var feature = (UrlsFeature)tpUrl.Tag;
            if (feature == null)
            {
                return;
            }

            lvUrls.Items.Clear();
            foreach (var url in feature.Items)
            {
                lvUrls.Items.Add(new UrlListViewItem(url, this));
            }

            if (feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (UrlListViewItem item in lvUrls.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshVerbs()
        {
            var feature = (VerbsFeature)tpVerbs.Tag;
            if (feature == null)
            {
                return;
            }

            lvVerbs.Items.Clear();
            foreach (var url in feature.Items)
            {
                lvVerbs.Items.Add(new VerbListViewItem(url, this));
            }

            if (feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (VerbListViewItem item in lvVerbs.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshHeaders()
        {
            var feature = (HeadersFeature)tpHeaders.Tag;
            if (feature == null)
            {
                return;
            }

            lvHeaders.Items.Clear();
            foreach (var url in feature.Items)
            {
                lvHeaders.Items.Add(new HeaderListViewItem(url, this));
            }

            if (feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (HeaderListViewItem item in lvHeaders.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void RefreshQueries()
        {
            var feature = (QueryStringsFeature)tpQuery.Tag;
            if (feature == null)
            {
                return;
            }

            lvQueries.Items.Clear();
            foreach (var url in feature.Items)
            {
                lvQueries.Items.Add(new QueryListViewItem(url, this));
            }

            if (feature.SelectedItem == null)
            {
                Refresh();
                return;
            }

            foreach (QueryListViewItem item in lvQueries.Items)
            {
                if (item.Item == feature.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        protected override void Refresh()
        {
            var feature = (IRequestFilteringFeature)tabControl1.SelectedTab.Tag;
            TaskList extra = feature.GetTaskList();
            Tasks.Fill(tsActionPanel, cmsActionPanel, extra);
            base.Refresh();
        }

        private void LvExtensionsSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (FileExtensionsFeature)tpExtensions.Tag;
            feature.SelectedItem = lvExtensions.SelectedItems.Count > 0
                ? ((ExtensionListViewItem)lvExtensions.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void LvRuleSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (FilteringRulesFeature)tpRules.Tag;
            feature.SelectedItem = lvRules.SelectedItems.Count > 0
                ? ((RuleListViewItem)lvRules.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void LvSegmentsSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (HiddenSegmentsFeature)tpSegments.Tag;
            feature.SelectedItem = lvSegments.SelectedItems.Count > 0
                ? ((SegmentListViewItem)lvSegments.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void LvUrlsSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (UrlsFeature)tpUrl.Tag;
            feature.SelectedItem = lvUrls.SelectedItems.Count > 0
                                       ? ((UrlListViewItem)lvUrls.SelectedItems[0]).Item
                                       : null;
            Refresh();
        }

        private void LvVerbsSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (VerbsFeature)tpVerbs.Tag;
            feature.SelectedItem = lvVerbs.SelectedItems.Count > 0
                ? ((VerbListViewItem)lvVerbs.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void LvHeadersSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (HeadersFeature)tpHeaders.Tag;
            feature.SelectedItem = lvHeaders.SelectedItems.Count > 0
                ? ((HeaderListViewItem)lvHeaders.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        private void LvQueriesSelectedIndexChanged(object sender, EventArgs e)
        {
            var feature = (QueryStringsFeature)tpQuery.Tag;
            feature.SelectedItem = lvQueries.SelectedItems.Count > 0
                ? ((QueryListViewItem)lvQueries.SelectedItems[0]).Item
                : null;
            Refresh();
        }

        protected override bool ShowHelp()
        {
            var feature = (IRequestFilteringFeature)tabControl1.SelectedTab.Tag;
            feature.ShowHelp();
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

                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        private void TabControl1SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = tabControl1.SelectedTab;
            if (item == null)
            {
                return;
            }

            var feature = (IRequestFilteringFeature)item.Tag;
            feature?.RequestFilteringSettingsUpdate?.Invoke();
            Refresh();
        }

        public void Set()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var section = service.GetSection("system.webServer/security/requestFiltering");
            var dialog = new SegmentSettingsDialog(Module, section);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            service.ServerManager.CommitChanges();
        }
    }
}

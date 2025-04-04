// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Features.Rewrite.Inbound;
    using JexusManager.Features.Rewrite.Outbound;
    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class RewritePage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly RewritePage _owner;

            public PageTaskList(RewritePage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class InboundRuleListViewItem : ListViewItem, IFeatureListViewItem<InboundRule>
        {
            public InboundRule Item { get; }
            private readonly RewritePage _page;

            public InboundRuleListViewItem(InboundRule item, RewritePage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Input));
                SubItems.Add(new ListViewSubItem(this, item.Negate ? "Does not matches" : "Matches"));
                SubItems.Add(new ListViewSubItem(this, item.PatternUrl));
                SubItems.Add(new ListViewSubItem(this, ToString(item.Type)));
                SubItems.Add(new ListViewSubItem(this, item.ActionUrl));
                SubItems.Add(new ListViewSubItem(this, item.StopProcessing ? "True" : "False"));
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }

            private static string ToString(long action)
            {
                switch (action)
                {
                    case 0:
                        return "None";
                    case 1:
                        return "Rewrite";
                    case 2:
                        return "Redirect";
                    case 3:
                        return "Custom Response";
                }

                return "Abort Request";
            }
        }

        private sealed class OutboundRuleListViewItem : ListViewItem, IFeatureListViewItem<OutboundRule>
        {
            public OutboundRule Item { get; private set; }

            private readonly RewritePage _page;

            public OutboundRuleListViewItem(OutboundRule item, RewritePage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Input));
                SubItems.Add(new ListViewSubItem(this, item.Negate ? "Does not match" : "Matches"));
                SubItems.Add(new ListViewSubItem(this, item.Pattern));
                SubItems.Add(new ListViewSubItem(this, ToString(item.Action)));
                SubItems.Add(new ListViewSubItem(this, item.Value));
                SubItems.Add(new ListViewSubItem(this, item.Stopping ? "True" : "False"));
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }

            private static string ToString(long action)
            {
                switch (action)
                {
                    case 0:
                        return "None";
                    case 1:
                        return "Rewrite";
                    case 2:
                        return "Redirect";
                    case 3:
                        return "Custom Response";
                }

                return "Abort Request";
            }
        }

        private RewriteFeature _feature;
        private PageTaskList _taskList;

        public RewritePage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)ServiceProvider.GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new RewriteFeature(Module);
            _feature.Inbound.RewriteSettingsUpdated = InitializeInbound;
            _feature.Outbound.RewriteSettingsUpdated = InitializeOutbound;
            _feature.Load();

            _feature.Inbound.HandleMouseClick(lvIn, (item, text) =>
            {
                item.Name = text;
                item.Apply();
            });

            _feature.Outbound.HandleMouseClick(lvOut, (item, text) =>
            {
                item.Name = text;
                item.Apply();
            });
        }

        protected override void InitializeListPage()
        {
            InitializeInbound();
            InitializeOutbound();
        }

        private void InitializeInbound()
        {
            lvIn.Items.Clear();
            foreach (var file in _feature.Inbound.Items)
            {
                lvIn.Items.Add(new InboundRuleListViewItem(file, this));
            }

            if (_feature.Inbound.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (InboundRuleListViewItem item in lvIn.Items)
            {
                if (item.Item == _feature.Inbound.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void InitializeOutbound()
        {
            lvOut.Items.Clear();
            foreach (var file in _feature.Outbound.Items)
            {
                lvOut.Items.Add(new OutboundRuleListViewItem(file, this));
            }

            if (_feature.Outbound.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (OutboundRuleListViewItem item in lvOut.Items)
            {
                if (item.Item == _feature.Outbound.SelectedItem)
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

        private void LvInMouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.Inbound.HandleMouseDoubleClick(lvIn);
        }

        private void LvInKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Inbound.Remove();
            }
        }

        private void LvInSelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.Inbound.HandleSelectedIndexChanged(lvIn);
            Refresh();
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

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        private void btnRenameIn_Click(object sender, EventArgs e)
        {
            lvIn.SelectedItems[0].BeginEdit();
        }

        private void LvInAfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            var element = (InboundRuleListViewItem)lvIn.Items[e.Item];
            element.Name = e.Label;
        }

        private void LvOutMouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.Outbound.HandleMouseDoubleClick(lvOut);
        }

        private void LvOutKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Outbound.Remove();
            }
        }

        private void LvOutSelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.Outbound.HandleSelectedIndexChanged(lvOut);
            Refresh();
        }
    }
}

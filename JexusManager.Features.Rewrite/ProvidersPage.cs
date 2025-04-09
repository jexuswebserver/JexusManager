// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class ProvidersPage : ModuleListPage, IModuleChildPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly ProvidersPage _owner;

            public PageTaskList(ProvidersPage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[]
                           {
                               GetBackTaskItem("Back", "Back to Rules"),
                               MethodTaskItem.CreateSeparator().SetUsage(),
                               HelpTaskItem
                           };
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }

            [Obfuscation(Exclude = true)]
            public void Back()
            {
                _owner.Back();
            }
        }

        private sealed class ProviderListViewItem : ListViewItem, IFeatureListViewItem<ProviderItem>
        {
            public ProviderItem Item { get; }
            private readonly ProvidersPage _page;

            public ProviderListViewItem(ProviderItem item, ProvidersPage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Type));
                SubItems.Add(new ListViewSubItem(this, item.Flag));
            }
        }

        private ProvidersFeature _feature;
        private PageTaskList _taskList;

        public ProvidersPage()
        {
            InitializeComponent();

            // Set the labels from resources if needed
            label3.Text = "Rewrite Providers";
            label2.Text = "Providers implement custom rewrite logic and can be invoked from inbound and outbound rewrite rules.";
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new ProvidersFeature(Module);
            _feature.RewriteSettingsUpdated = InitializeListPage;
            _feature.Load();

            _feature.InitializeMouseClick(listView1, (item, text) =>
            {
                item.Name = text;
                item.Apply();
            },
            text =>
            {
                if (_feature.FindDuplicate(item => item.Name, text))
                {
                    var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                    service.ShowMessage("A rewrite provider with this name already exists.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                return true;
            });
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var provider in _feature.Items)
            {
                listView1.Items.Add(new ProviderListViewItem(provider, this));
            }

            _feature.InitializeColumnClick(listView1);

            if (_feature.SelectedItem != null)
            {
                foreach (ProviderListViewItem item in listView1.Items)
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

        private void ListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Remove();
            }
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _feature.HandleMouseDoubleClick(listView1);
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.HandleSelectedIndexChanged(listView1);
            Refresh();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        protected override bool ShowHelp()
        {
            return _feature.ShowHelp();
        }

        private void Back()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service?.NavigateBack(1);
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

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModulePage ParentPage { get; set; }
    }
}

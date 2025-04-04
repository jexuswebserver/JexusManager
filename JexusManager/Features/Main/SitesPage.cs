﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Binding = Microsoft.Web.Administration.Binding;

    internal partial class SitesPage : ModuleListPage
    {
        private sealed class PageTaskList : TaskList
        {
            private readonly SitesPage _owner;

            public PageTaskList(SitesPage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[]
                {
                    new MethodTaskItem("ShowHelp", "Help", string.Empty, string.Empty, Resources.help_16).SetUsage()
                };
            }

            [Obfuscation(Exclude = true)]
            public void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class SitesListViewItem : ListViewItem, IFeatureListViewItem<Site>
        {
            public Site Item { get; }
            private readonly SitesPage _page;

            public SitesListViewItem(Site item, SitesPage page)
                : base(item.Name)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, Item.Id.ToString(CultureInfo.InvariantCulture)));
                SubItems.Add(new ListViewSubItem(this, CommonHelper.ToString(Item.State)));
                SubItems.Add(new ListViewSubItem(this, ToString(Item.Bindings)));
                SubItems.Add(new ListViewSubItem(this, Item.PhysicalPath));
                ImageIndex = item.State == ObjectState.Started ? 0 : 1;
                Tag = item;
            }

            private static string ToString(BindingCollection bindings)
            {
                return bindings.Select(binding => binding.ToShortString()).Combine(",");
            }
        }

        private readonly MainForm _form;
        private SitesFeature _feature;
        private PageTaskList _taskList;

        public SitesPage(MainForm form)
        {
            InitializeComponent();
            btnGo.Image = DefaultTaskList.GoImage;
            btnShowAll.Image = DefaultTaskList.ShowAllImage;

            imageList1.Images.Add(Resources.site_16);
            imageList1.Images.Add(Resources.site_stopped_16);
            _form = form;
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new SitesFeature(Module);
            _feature.SitesSettingsUpdated = InitializeListPage;
            _feature.Load();

            _feature.HandleMouseClick(listView1, (item, text) =>
            {
                item.Name = text;
                item.Apply();
                _form.UpdateSiteNode(item);
                item.Server.CommitChanges();
            },
            text =>
            {
                var service = (IManagementUIService)GetService(typeof(IManagementUIService));
                Debug.Assert(service != null, "service != null");
                if (_feature.FindDuplicate(item => item.Name, text))
                {
                    service.ShowMessage(
                        "A site with this name already exists.",
                        Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return false;
                }

                var forbidden = SiteCollection.InvalidSiteNameCharacters();
                foreach (var ch in forbidden)
                {
                    if (text.Contains(ch))
                    {
                        service.ShowMessage(
                            $"The site name cannot contain the following characters: '{string.Join(", ", forbidden)}'.",
                            "Sites", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }

                if (_feature.SelectedItem.Server.Mode == WorkingMode.Jexus)
                {
                    foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
                    {
                        if (text.Contains(ch) || text.StartsWith("~"))
                        {
                            service.ShowMessage("The site name cannot contain the following characters: '~,  '.", Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                    }
                }

                return true;
            });
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (Site file in _feature.Items)
            {
                listView1.Items.Add(new SitesListViewItem(file, this));
            }

            if (_feature.SelectedItem != null)
            {
                foreach (SitesListViewItem item in listView1.Items)
                {
                    if (item.Item.Id == _feature.SelectedItem.Id)
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

        private void cbFilter_TextChanged(object sender, EventArgs e)
        {
            btnGo.Enabled = string.IsNullOrWhiteSpace(cbFilter.Text);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            cbFilter.Text = string.Empty;
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
    }
}

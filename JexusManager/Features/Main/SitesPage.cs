// Copyright (c) Lex Li. All rights reserved.
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
    using System.IO;
    using System.Collections.Generic;
    using JexusManager.Main.Features;

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

        private sealed class SitesListViewItem : ListViewItem
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
                SubItems.Add(new ListViewSubItem(this, Item.Applications[0].VirtualDirectories[0].PhysicalPath));
                SubItems.Add(new ListViewSubItem(this, Directory.Exists(Item.Applications[0].VirtualDirectories[0].PhysicalPath).ToString()));
                ImageIndex = item.State == ObjectState.Started ? 0 : 1;
            }

            private static string ToString(BindingCollection bindings)
            {
                var result = new StringBuilder();
                foreach (Binding binding in bindings)
                {
                    result.Append(binding.ToShortString()).Append(',');
                }

                if (result.Length > 0)
                {
                    result.Length--;
                }

                return result.ToString();
            }
        }

        private readonly MainForm _form;
        private SitesFeature _feature;
        private PageTaskList _taskList;
        private ListViewItemSorter sorter;
        public SitesPage(MainForm form)
        {
            InitializeComponent();
            btnGo.Image = DefaultTaskList.GoImage;
            btnShowAll.Image = DefaultTaskList.ShowAllImage;

            imageList1.Images.Add(Resources.site_16);
            imageList1.Images.Add(Resources.site_stopped_16);
            imageList1.Images.Add("Ascending", Resources.Ascending);
            imageList1.Images.Add("Descending", Resources.Descending);
            imageList1.Images.Add("None", Resources.None);
            foreach (ColumnHeader listView1Column in listView1.Columns)
            {
                listView1Column.ImageKey= "None";
            }
            _form = form;
            sorter = new ListViewItemSorter();
            listView1.ListViewItemSorter = sorter;
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new SitesFeature(Module);
            _feature.SitesSettingsUpdated = InitializeListPage;
            _feature.Load();
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

        private void actRename_Execute(object sender, EventArgs e)
        {
            listView1.SelectedItems[0].BeginEdit();
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            Debug.Assert(service != null, "service != null");
            foreach (var ch in SiteCollection.InvalidSiteNameCharacters())
            {
                if (e.Label.Contains(ch))
                {
                    service.ShowMessage(
                        "The site name cannot contain the following characters: '\\, /, ?, ;, :, @, &, =, +, $, ,, |, \", <, >'.",
                        "Sites", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                    return;
                }
            }

            foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
            {
                if (e.Label.Contains(ch) || e.Label.StartsWith("~"))
                {
                    service.ShowMessage("The site name cannot contain the following characters: '~,  '.", "Sites",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                    return;
                }
            }

            var site = (Site)listView1.Items[e.Item].Tag;
            site.Name = e.Label;
            _form.UpdateSiteNode(site);
        }

        private void cbFilter_TextChanged(object sender, EventArgs e)
        {
            btnGo.Enabled = string.IsNullOrWhiteSpace(cbFilter.Text);
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            cbFilter.Text = string.Empty;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
          

            _feature.SelectedItem = listView1.SelectedItems.Count > 0
                ? ((SitesListViewItem)listView1.SelectedItems[0]).Item
                : null;

            //Refresh();
            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }


        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            _feature.SelectedItems = listView1.SelectedItems.OfType<SitesListViewItem>().Select(i => i.Item).ToList();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                _form.ShowSite((Site)listView1.SelectedItems[0].Tag);
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Remove();
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // todo : 需要实现正序倒序排列方法，
            var sorting = sorter.PushSort(e.Column, listView1.Columns.Count);
            var header = listView1.Columns[e.Column];

            header.ImageKey = sorting.ToString();
            switch (sorting)
            {
                case SortOrder.None:
                    //header.Text = header.Text.Replace("", "");
                    break;
                case SortOrder.Ascending:

                    break;

                case SortOrder.Descending:

                    break;
            }


            listView1.Sort();
        }

    }
}

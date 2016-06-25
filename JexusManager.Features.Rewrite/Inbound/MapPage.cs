// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InboundRulePage.cs" company="LeXtudio">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class MapPage : ModuleListPage, IModuleChildPage
    {
        private sealed class PageTaskList : DefaultTaskList
        {
            private readonly MapPage _owner;

            public PageTaskList(MapPage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(new MethodTaskItem("Add", "Add Mapping Entry...", string.Empty).SetUsage());
                result.Add(new MethodTaskItem("Set", "Edit Map Settings...", string.Empty).SetUsage());
                if (_owner.Map.SelectedItem != null)
                {
                    //result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Edit", "Edit Mapping Entry...", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                }

                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                result.Add(GetBackTaskItem("BackMore", "Back to Rewrite Maps"));
                result.Add(GetBackTaskItem("Back", "Back to Rules"));
                result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                result.Add(HelpTaskItem);
                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void ShowHelp()
            {
                _owner.ShowHelp();
            }

            [Obfuscation(Exclude = true)]
            public void Back()
            {
                _owner.Back();
            }

            [Obfuscation(Exclude = true)]
            public void BackMore()
            {
                _owner.BackMore();
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Edit()
            {
                _owner.Edit();
            }

            [Obfuscation(Exclude = true)]
            public void Set()
            {
                _owner.Set();
            }
        }

        private sealed class MapListViewItem : ListViewItem
        {
            public MapRule Item { get; }
            private readonly MapPage _page;

            public MapListViewItem(MapRule item, MapPage page)
                : base(item.Original)
            {
                this.Item = item;
                _page = page;
                this.SubItems.Add(new ListViewSubItem(this, item.New));
            }
        }

        private TaskList _taskList;
        private MapsFeature _feature;

        public MapPage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var info = (Tuple<MapsFeature, MapItem>)navigationData;

            // TODO: pictureBox1.Image = service.Scope.GetImage();

            _feature = info.Item1;
            this.Map = info.Item2;
            txtName.ReadOnly = this.Map != null;
            if (this.Map != null)
            {
                this.Map.MapSettingsUpdated = this.InitializeListPage;
                txtName.Text = this.Map.Name;
            }

            this.Map?.OnRewriteSettingsSaved();
        }

        public MapItem Map { get; set; }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var file in this.Map.Items)
            {
                listView1.Items.Add(new MapListViewItem(file, this));
            }

            if (this.Map.SelectedItem == null)
            {
                this.Refresh();
                return;
            }

            foreach (MapListViewItem item in listView1.Items)
            {
                if (item.Item == this.Map.SelectedItem)
                {
                    item.Selected = true;
                }
            }
        }

        private void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Map.SelectedItem = listView1.SelectedItems.Count > 0
                ? ((MapListViewItem)listView1.SelectedItems[0]).Item
                : null;
            this.Refresh();
        }

        private void ListView1KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _feature.Remove();
            }
        }

        protected override void Refresh()
        {
            this.Tasks.Fill(tsActionPanel, cmsActionPanel);
            base.Refresh();
        }

        protected override bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
            return true;
        }

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        private void Back()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service?.NavigateBack(2);
            _feature.SelectedItem = this.Map;
            _feature.OnRewriteSettingsSaved();
        }

        private void BackMore()
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service?.NavigateBack(1);
        }

        public void Add()
        {
            _feature.AddRule();
        }

        public void Remove()
        {
            _feature.RemoveRule();
        }

        private void Edit()
        {
            _feature.EditRule();
        }

        private void Set()
        {
            _feature.Set();
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

        public IModulePage ParentPage { get; set; }
    }
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using JexusManager.Services;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;

namespace JexusManager.Features.Rewrite
{
    internal partial class SettingsPage : ModuleListPage, IModuleChildPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly SettingsPage _owner;

            public PageTaskList(SettingsPage owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                return new TaskItem[]
                {
                    GetBackTaskItem("BackToProviders", "Back to Providers"),
                    GetBackTaskItem("BackToRules", "Back to Rules"),
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

            [Obfuscation(Exclude = true)]
            public void BackToProviders()
            {
                _owner.BackToProviders();
            }

            [Obfuscation(Exclude = true)]
            public void BackToRules()
            {
                _owner.BackToRules();
            }
        }

        private sealed class SettingListViewItem : ListViewItem, IFeatureListViewItem<SettingItem>
        {
            public SettingItem Item { get; }
            private readonly SettingsPage _page;

            public SettingListViewItem(SettingItem item, SettingsPage page)
                : base(item.Key)
            {
                Item = item;
                _page = page;
                SubItems.Add(new ListViewSubItem(this, item.Value));
                SubItems.Add(new ListViewSubItem(this, item.Encrypted ? "Yes" : "No"));
            }
        }

        private SettingsFeature _feature;
        private PageTaskList _taskList;
        private ProviderItem _provider;

        public SettingsPage()
        {
            InitializeComponent();

            label3.Text = "Provider Settings";
            label2.Text = "Configure settings for the selected provider.";
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _provider = navigationData as ProviderItem;
            if (_provider == null)
            {
                throw new InvalidOperationException("Provider must be specified for settings page");
            }

            _feature = new SettingsFeature(Module, _provider);
            _feature.SettingsUpdated = InitializeListPage;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            listView1.Items.Clear();
            foreach (var setting in _feature.Items)
            {
                listView1.Items.Add(new SettingListViewItem(setting, this));
            }

            _feature.InitializeColumnClick(listView1);

            // Initialize grouping support
            _feature?.InitializeGrouping(cbGroup);

            if (_feature.SelectedItem != null)
            {
                foreach (SettingListViewItem item in listView1.Items)
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
            if (listView1.SelectedItems.Count <= 0)
            {
                return;
            }

            var item = listView1.SelectedItems[0] as SettingListViewItem;
            _feature.SelectedItem = item?.Item;
            _feature.HandleMouseDoubleClick(listView1);
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                _feature.SelectedItem = null;
            }
            else
            {
                var item = listView1.SelectedItems[0] as SettingListViewItem;
                _feature.SelectedItem = item?.Item;
            }

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
            Back(1);
        }

        [Obfuscation(Exclude = true)]
        public void BackToProviders()
        {
            Back(1);
        }

        [Obfuscation(Exclude = true)]
        public void BackToRules()
        {
            Back(2);
        }

        private void Back(int levels)
        {
            var service = (INavigationService)GetService(typeof(INavigationService));
            service?.NavigateBack(levels);
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

        private void CbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogHelper.HandleGrouping(listView1, cbGroup.SelectedItem.ToString(), _feature.GetGroupKey);
        }
    }
}

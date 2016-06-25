// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System.Collections;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Application = Microsoft.Web.Administration.Application;

    internal partial class ApplicationPage : ModuleListPage
    {
        private sealed class PageTaskList : TaskList
        {
            private readonly ApplicationPage _owner;

            public PageTaskList(ApplicationPage owner)
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

        private PageTaskList _taskList;
        private Application _application;
        private MainForm _main;
        private ApplicationFeature _feature;

        public ApplicationPage(Application application, MainForm main)
        {
            InitializeComponent();
            btnView.Image = DefaultTaskList.ViewImage;
            btnGo.Image = DefaultTaskList.GoImage;
            btnShowAll.Image = DefaultTaskList.ShowAllImage;

            _application = application;
            _main = main;
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            txtTitle.Text = string.Format("{0} Home", _application.Path);
            InitializeListPage();

            _feature = new ApplicationFeature(Module);
            _feature.ApplicationSettingsUpdated = Refresh;
            _feature.Load();
        }

        protected override void InitializeListPage()
        {
            var iis = new ListViewGroup("IIS");
            listView1.Groups.Add(iis);
            var service = (IControlPanel)GetService(typeof(IControlPanel));
            for (int index = 0; index < service.Pages.Count; index++)
            {
                var pageInfo = service.Pages[index];
                imageList1.Images.Add((Image)pageInfo.LargeImage);
                listView1.Items.Add(new ModulePageInfoListViewItem(pageInfo) { ImageIndex = index, Group = iis });
            }
        }

        protected override void Refresh()
        {
            Tasks.Fill(tsActionPanel, cmsActionPanel);
            base.Refresh();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }

            var item = (ModulePageInfoListViewItem)listView1.SelectedItems[0];
            _main.LoadPage(item.Page);
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

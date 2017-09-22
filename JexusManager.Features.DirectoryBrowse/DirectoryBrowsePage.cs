// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.DirectoryBrowse
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class DirectoryBrowsePage : ModuleDialogPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly DirectoryBrowsePage _owner;

            public PageTaskList(DirectoryBrowsePage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private DirectoryBrowseFeature _feature;
        private bool _hasChanges;
        private bool _initialized;
        private TaskList _taskList;

        public DirectoryBrowsePage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new DirectoryBrowseFeature(Module);
            _feature.DirectoryBrowseSettingsUpdated = Refresh;
            _feature.Load();
            base.Initialize(navigationData);
        }

        protected override bool ShowHelp()
        {
            return _feature.ShowHelp();
        }

        private void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        protected override bool ApplyChanges()
        {
            _feature.TimeEnabled = cbTime.Checked;
            _feature.SizeEnabled = cbSize.Checked;
            _feature.ExtensionEnabled = cbExtension.Checked;
            _feature.DateEnabled = cbDate.Checked;
            _feature.LongDateEnabled = cbLongDate.Checked;
            if (!_feature.ApplyChanges())
            {
                return false;
            }

            ClearChanges();
            return true;
        }

        protected override void CancelChanges()
        {
            _initialized = false;
            _hasChanges = false;
            _feature.CancelChanges();
            ClearChanges();
        }

        protected override bool HasChanges
        {
            get { return _hasChanges; }
        }

        protected override bool CanApplyChanges
        {
            get { return true; }
        }

        private void InformChanges()
        {
            if (!_initialized)
            {
                return;
            }

            _hasChanges = true;
            Refresh();
        }

        private void ClearChanges()
        {
            _hasChanges = false;
            Refresh();
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

        protected override void OnRefresh()
        {
            cbTime.Enabled = cbSize.Enabled = cbExtension.Enabled = cbDate.Enabled = _feature.IsEnabled;
            if (!_hasChanges)
            {
                cbTime.Checked = _feature.TimeEnabled;
                cbSize.Checked = _feature.SizeEnabled;
                cbExtension.Checked = _feature.ExtensionEnabled;
                cbDate.Checked = _feature.DateEnabled;
                cbLongDate.Checked = _feature.LongDateEnabled;
                _initialized = true;
            }

            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }

        private void CbDateCheckedChanged(object sender, EventArgs e)
        {
            InformChanges();
            cbLongDate.Enabled = cbDate.Checked && cbDate.Enabled;
        }

        private void CbSslCheckedChanged(object sender, EventArgs e)
        {
            InformChanges();
        }
    }
}

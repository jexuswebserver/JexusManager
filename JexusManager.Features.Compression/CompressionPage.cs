// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Compression
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    internal partial class CompressionPage : ModuleDialogPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly CompressionPage _owner;

            public PageTaskList(CompressionPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private CompressionFeature _feature;
        private bool _hasChanges;
        private bool _initialized;
        private TaskList _taskList;

        public CompressionPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new CompressionFeature(Module);
            _feature.CompressionSettingsUpdated = Refresh;
            _feature.Load();
            base.Initialize(navigationData);
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

        private void cbStatic_CheckedChanged(object sender, EventArgs e)
        {
            _feature.StaticEnabled = cbStatic.Checked;
            InformChanges();
        }

        private void cbDynamic_CheckedChanged(object sender, EventArgs e)
        {
            _feature.DynamicEnabled = cbDynamic.Checked;
            InformChanges();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogHelper.ShowBrowseDialog(txtPath, null);
            _feature.Directory = txtPath.Text;
            InformChanges();
        }

        private void cbFileSize_CheckedChanged(object sender, EventArgs e)
        {
            _feature.DoFileSize = cbFileSize.Checked;
            InformChanges();
        }

        private void cbDiskspaceLimit_CheckedChanged(object sender, EventArgs e)
        {
            _feature.DoDiskSpaceLimiting = cbDiskspaceLimit.Checked;
            InformChanges();
        }

        private void txtFileSize_TextChanged(object sender, EventArgs e)
        {
            _feature.MinFileSizeForComp = txtFileSize.Text;
            InformChanges();
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            _feature.Directory = txtPath.Text;
            InformChanges();
        }

        private void txtDiskspaceLimit_TextChanged(object sender, EventArgs e)
        {
            _feature.MaxDiskSpaceUsage = txtDiskspaceLimit.Text;
            InformChanges();
        }

        protected override bool ApplyChanges()
        {
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
            get
            {
                return !gbStatic.Visible || (!string.IsNullOrWhiteSpace(txtFileSize.Text)
                    && !string.IsNullOrWhiteSpace(txtPath.Text)
                    && !string.IsNullOrWhiteSpace(txtDiskspaceLimit.Text));
            }
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

                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }

        protected override void OnRefresh()
        {
            if (!_hasChanges)
            {
                cbDynamic.Checked = _feature.DynamicEnabled;
                cbStatic.Checked = _feature.StaticEnabled;

                var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                gbStatic.Visible = service.Scope == ManagementScope.Server;
                if (service.Scope == ManagementScope.Server)
                {
                    txtDiskspaceLimit.Enabled = cbDiskspaceLimit.Checked = _feature.DoDiskSpaceLimiting;
                    txtDiskspaceLimit.Text = _feature.MaxDiskSpaceUsage.ToString();
                    txtPath.Text = _feature.Directory;
                    txtFileSize.Text = _feature.FileSize;
                    txtFileSize.Enabled = cbFileSize.Checked = _feature.DoFileSize;
                }

                _initialized = true;
            }

            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }
    }
}

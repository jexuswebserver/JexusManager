// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Logging
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class LoggingPage : ModuleDialogPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly LoggingPage _owner;

            public PageTaskList(LoggingPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private PageTaskList _taskList;
        private LoggingFeature _feature;
        private bool _hasChanges;
        private bool _initialized;

        public LoggingPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new LoggingFeature(Module);
            _feature.LoggingSettingsUpdated = Refresh;
            _feature.Load();
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogHelper.ShowBrowseDialog(txtPath);
            _feature.Directory = txtPath.Text;
            InformChanges();
        }

        private void cbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = cbFormat.SelectedIndex == cbFormat.Items.Count - 1;
            _feature.LogFormat = cbFormat.SelectedIndex;
            InformChanges();
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

        protected override void OnRefresh()
        {
            if (!_hasChanges)
            {
                cbLogType.SelectedIndex = (int)_feature.Mode;
                cbEncoding.SelectedIndex = _feature.Encoding;
                cbFormat.SelectedIndex = (int)_feature.LogFormat;
                txtPath.Text = _feature.Directory;
                txtPath.Enabled = _feature.CanBrowse && _feature.IsEnabled;
                btnSelect.Enabled = _feature.IsEnabled;
                btnBrowse.Enabled = _feature.CanBrowse && _feature.IsEnabled;
                cbFormat.Enabled = _feature.CanBrowse && _feature.IsEnabled;
                cbEncoding.Enabled = _feature.CanEncoding;

                rbFile.Enabled = rbEvent.Enabled = rbBoth.Enabled = _feature.LogTargetW3C == -1;
                if (_feature.LogTargetW3C != -1)
                {
                    switch (_feature.LogTargetW3C)
                    {
                        case 1:
                            rbFile.Checked = true;
                            break;
                        case 2:
                            rbEvent.Checked = true;
                            break;
                        default:
                            rbBoth.Checked = true;
                            break;
                    }
                }

                if (_feature.Period == 0)
                {
                    rbSize.Checked = true;
                    cbSchedule.Enabled = false;
                    txtSize.Text = _feature.TruncateSize.ToString();
                }
                else
                {
                    rbSchedule.Checked = true;
                    txtSize.Enabled = false;
                    switch (_feature.Period)
                    {
                        case 2:
                            cbSchedule.SelectedIndex = 2;
                            break;
                        case 1:
                            cbSchedule.SelectedIndex = 1;
                            break;
                        case 4:
                            cbSchedule.SelectedIndex = 0;
                            break;
                        case 3:
                            cbSchedule.SelectedIndex = 3;
                            break;
                    }
                }

                cbLocalTime.Checked = _feature.LocalTimeRollover;

                _initialized = true;
            }

            Tasks.Fill(tsActionPanel, cmsActionPanel);
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

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            _feature.Directory = txtPath.Text;
            InformChanges();
        }

        protected override bool HasChanges
        {
            get { return _hasChanges; }
        }

        protected override bool CanApplyChanges
        {
            get { return true; }
        }

        private void cbEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            _feature.Encoding = cbEncoding.SelectedIndex;
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
            _feature.CancelChanges();
            ClearChanges();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            _feature.SelectFields();
        }

        private void rbFile_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFile.Checked)
            {
                _feature.LogTargetW3C = 1;
            }
            else if (rbEvent.Checked)
            {
                _feature.LogTargetW3C = 2;
            }
            else
            {
                _feature.LogTargetW3C = 3;
            }
        }

        private void rbSchedule_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSchedule.Checked)
            {
                switch (cbSchedule.SelectedIndex)
                {
                    case 0:
                        _feature.Period = 4;
                        break;
                    case 1:
                        _feature.Period = 1;
                        break;
                    case 2:
                        _feature.Period = 2;
                        break;
                    case 3:
                        _feature.Period = 3;
                        break;
                }
            }
            else if (rbSize.Checked)
            {
                _feature.Period = 0;
                _feature.TruncateSize = long.Parse(txtSize.Text);
            }
        }

        private void cbLocalTime_CheckedChanged(object sender, EventArgs e)
        {
            _feature.LocalTimeRollover = cbLocalTime.Checked;
        }
    }
}

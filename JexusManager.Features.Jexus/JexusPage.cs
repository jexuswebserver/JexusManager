// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Jexus
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class JexusPage : ModuleDialogPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly JexusPage _owner;

            public PageTaskList(JexusPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private JexusFeature _feature;
        private bool _hasChanges;
        private bool _initialized;
        private TaskList _taskList;

        public JexusPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new JexusFeature(Module);
            _feature.JexusSettingsUpdated = Refresh;
            _feature.Load();
            txtSettings.Enabled = _feature.IsFeatureEnabled;
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

        private void txtSettings_TextChanged(object sender, EventArgs e)
        {
            _feature.Contents = txtSettings.Text;
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

                base.Tasks.Add(_taskList);
                return base.Tasks;
            }
        }

        protected override void OnRefresh()
        {
            if (!_hasChanges)
            {
                txtSettings.Text = _feature.Contents;
                _initialized = true;
            }

            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }
    }
}

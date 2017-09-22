// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Access
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Binding = Microsoft.Web.Administration.Binding;

    internal partial class AccessPage : ModuleDialogPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly AccessPage _owner;

            public PageTaskList(AccessPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private AccessFeature _feature;
        private bool _hasChanges;
        private bool _initialized;
        private TaskList _taskList;

        public AccessPage()
        {
            this.InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new AccessFeature(this.Module, null, null);
            _feature.AccessSettingsUpdated = this.Refresh;
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
            long result = 0;
            if (cbSSL.Checked)
            {
                result |= 8;
            }

            if (rbAccept.Checked)
            {
                result |= 32;
            }

            if (rbRequire.Checked)
            {
                result |= 64;
            }

            _feature.SslFlags = result;
            if (!_feature.ApplyChanges())
            {
                return false;
            }

            this.ClearChanges();
            return true;
        }

        protected override void CancelChanges()
        {
            _initialized = false;
            _hasChanges = false;
            _feature.CancelChanges();
            this.ClearChanges();
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
            this.Refresh();
        }

        private void ClearChanges()
        {
            _hasChanges = false;
            this.Refresh();
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
                cbSSL.Checked = (_feature.SslFlags & 8) == 8;
                rbAccept.Checked = (_feature.SslFlags & 32) == 32;
                rbRequire.Checked = (_feature.SslFlags & 64) == 64;
                rbIgnore.Checked = !rbAccept.Checked && !rbRequire.Checked;

                var service = (IConfigurationService)this.GetService(typeof(IConfigurationService));
                if (service.Scope == ManagementScope.Site)
                {
                    var result = false;
                    foreach (Binding binding in service.Site.Bindings)
                    {
                        if (binding.Protocol == "https")
                        {
                            result = true;
                            break;
                        }
                    }

                    cbSSL.Enabled = rbAccept.Enabled = rbIgnore.Enabled = rbRequire.Enabled = result;
                }
                else if (service.Scope == ManagementScope.Application)
                {
                    var result = false;
                    foreach (Binding binding in service.Application.GetSite().Bindings)
                    {
                        if (binding.Protocol == "https")
                        {
                            result = true;
                            break;
                        }
                    }

                    cbSSL.Enabled = rbAccept.Enabled = rbIgnore.Enabled = rbRequire.Enabled = result;
                }

                _initialized = true;
            }

            this.Tasks.Fill(tsActionPanel, cmsActionPanel);
        }

        private void CbSslCheckedChanged(object sender, EventArgs e)
        {
            InformChanges();
        }
    }
}

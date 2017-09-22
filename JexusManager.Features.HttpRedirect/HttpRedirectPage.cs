// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpRedirect
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class HttpRedirectPage : ModuleDialogPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly HttpRedirectPage _owner;

            public PageTaskList(HttpRedirectPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private class CodeItem
        {
            public long Code { get; set; }
            public string Description { get; set; }

            public CodeItem(long code, string description)
            {
                Code = code;
                Description = description;
            }

            public override string ToString()
            {
                return Description;
            }
        }

        private HttpRedirectFeature _feature;
        private bool _hasChanges;
        private bool _initialized;
        private TaskList _taskList;

        public HttpRedirectPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            _feature = new HttpRedirectFeature(Module, null, null);
            _feature.HttpRedirectSettingsUpdated = Refresh;
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
            _feature.Enabled = cbRedirect.Checked;
            _feature.Link = txtLink.Text;
            _feature.Exact = cbExact.Checked;
            _feature.OnlyRoot = cbThis.Checked;
            _feature.Mode = ((CodeItem)(cbCode.SelectedItem)).Code;
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
                if (!_initialized)
                {
                    var lines = new List<CodeItem>
                                    {
                                        new CodeItem(302, "Found(302)"),
                                        new CodeItem(301, "Permanent(301)"),
                                        new CodeItem(307, "Temporary(307)"),
                                        new CodeItem(308, "Permanent Redirect (308)")
                                    };
                    foreach (var line in lines)
                    {
                        if (_feature.SupportedModes.Contains(line.Code))
                        {
                            cbCode.Items.Add(line);
                        }
                    }
                }

                cbRedirect.Checked = _feature.Enabled;
                txtLink.Text = _feature.Link;
                cbExact.Checked = _feature.Exact;
                cbThis.Checked = _feature.OnlyRoot;
                foreach (CodeItem item in cbCode.Items)
                {
                    if (item.Code == _feature.Mode)
                    {
                        cbCode.SelectedItem = item;
                    }
                }

                _initialized = true;
            }

            Tasks.Fill(tsActionPanel, cmsActionPanel);
        }

        private void CbSslCheckedChanged(object sender, EventArgs e)
        {
            InformChanges();
            txtLink.Enabled = cbCode.Enabled = cbExact.Enabled = cbThis.Enabled = cbRedirect.Checked;
        }

        private void CbExactCheckedChanged(object sender, EventArgs e)
        {
            InformChanges();
        }

        private void CbCodeSelectedIndexChanged(object sender, EventArgs e)
        {
            InformChanges();
        }

        private void TxtLinkTextChanged(object sender, EventArgs e)
        {
            InformChanges();
        }
    }
}

﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Authentication
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;
    using Services;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Extensions;
    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class AuthenticationPage : ModuleListPage
    {
        private sealed class PageTaskList : ShowHelpTaskList
        {
            private readonly AuthenticationPage _owner;

            public PageTaskList(AuthenticationPage owner)
            {
                _owner = owner;
            }

            [Obfuscation(Exclude = true)]
            public override void ShowHelp()
            {
                _owner.ShowHelp();
            }
        }

        private sealed class AuthenticationListViewItem : ListViewItem
        {
            private readonly AuthenticationPage _page;
            private readonly ListViewSubItem _status;

            public AuthenticationListViewItem(AuthenticationFeature feature, AuthenticationPage page)
                : base(feature.Name)
            {
                _page = page;
                Feature = feature;
                _status = new ListViewSubItem(this, feature.IsEnabled ? "Enabled" : "Disabled");
                SubItems.Add(_status);
                SubItems.Add(new ListViewSubItem(this, GetString(feature.AuthenticationType)));
                feature.AuthenticationSettingsUpdated = AuthenticationSettingsUpdated;
            }

            private void AuthenticationSettingsUpdated()
            {
                _status.Text = Feature.IsEnabled ? "Enabled" : "Disabled";
                _page.ListView1SelectedIndexChanged(ListView, EventArgs.Empty);
            }

            private string GetString(AuthenticationType authenticationType)
            {
                switch (authenticationType)
                {
                    case AuthenticationType.ChallengeBase:
                    case AuthenticationType.ClientCertificateBased:
                        return "HTTP 401 Challenge";
                    case AuthenticationType.LoginRedirectBased:
                        return "HTTP 302 Login/Redirect";
                    case AuthenticationType.Other:
                        return string.Empty;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(authenticationType));
                }
            }

            public AuthenticationFeature Feature { get; }
        }

        private TaskList _taskList;

        public AuthenticationPage()
        {
            InitializeComponent();
        }

        protected override void Initialize(object navigationData)
        {
            base.Initialize(navigationData);
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            pictureBox1.Image = service.Scope.GetImage();

            var certificateFeature = new ClientCertificateAuthenticationFeature(Module);
            if (certificateFeature.IsFeatureEnabled)
            {
                listView1.Items.Add(new AuthenticationListViewItem(certificateFeature, this));
                certificateFeature.Load();
            }

            var anonymousFeature = new AnonymousAuthenticationFeature(Module);
            if (anonymousFeature.IsFeatureEnabled)
            {
                listView1.Items.Add(new AuthenticationListViewItem(anonymousFeature, this));
                anonymousFeature.Load();
            }

            // TODO: Elevation is needed to modify root web.config.
            var impersonationFeature = new ImpersonationFeature(Module);
            if (impersonationFeature.IsFeatureEnabled)
            {
                listView1.Items.Add(new AuthenticationListViewItem(impersonationFeature, this));
                impersonationFeature.Load();
            }

            var basicFeature = new BasicAuthenticationFeature(Module);
            if (basicFeature.IsFeatureEnabled)
            {
                listView1.Items.Add(new AuthenticationListViewItem(basicFeature, this));
                basicFeature.Load();
            }

            var digestFeature = new DigestAuthenticationFeature(Module);
            if (digestFeature.IsFeatureEnabled)
            {
                listView1.Items.Add(new AuthenticationListViewItem(digestFeature, this));
                digestFeature.Load();
            }

            var formsFeature = new FormsAuthenticationFeature(Module);
            if (formsFeature.IsFeatureEnabled)
            {
                listView1.Items.Add(new AuthenticationListViewItem(formsFeature, this));
                formsFeature.Load();
            }

            var windowsFeature = new WindowsAuthenticationFeature(Module);
            if (windowsFeature.IsFeatureEnabled)
            {
                listView1.Items.Add(new AuthenticationListViewItem(windowsFeature, this));
                windowsFeature.Load();
            }

            InitializeListPage();
        }

        protected override void InitializeListPage()
        {
            TaskList extra = null;
            if (listView1.SelectedItems.Count != 0)
            {
                var item = (AuthenticationListViewItem)listView1.SelectedItems[0];
                extra = item.Feature.GetTaskList();
            }

            new ColumnClickHook().HandleColumnClick(listView1);

            Tasks.Fill(tsActionPanel, cmsActionPanel, extra);
        }

        private void ListView1SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeListPage();
        }

        protected override bool ShowHelp()
        {
            if (listView1.SelectedItems.Count == 0)
            {
                DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210461");
                return true;
            }

            return ((AuthenticationListViewItem)listView1.SelectedItems[0]).Feature.ShowHelp();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        private void CbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogHelper.HandleGrouping(listView1, cbGroup.SelectedItem.ToString(), (item, selectedGroup) =>
            {
                // Determine the group key based on the selected grouping option
                switch (selectedGroup)
                {
                    case "Response Type":
                        return item.SubItems[2].Text;
                    case "Status":
                        return item.SubItems[1].Text;
                    default:
                        return "Unknown";
                }
            });
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
    }
}

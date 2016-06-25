// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Forms;

    using JexusManager.Properties;
    using JexusManager.Services;

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
                var service = _page.GetService(typeof(IConfigurationService)) as IConfigurationService;
                service?.ServerManager.CommitChanges();

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
            listView1.Items.Add(new AuthenticationListViewItem(certificateFeature, this));

            var anonymousFeature = new AnonymousAuthenticationFeature(Module);
            listView1.Items.Add(new AuthenticationListViewItem(anonymousFeature, this));

            var impersonationFeature = new ImpersonationFeature(Module);
            listView1.Items.Add(new AuthenticationListViewItem(impersonationFeature, this));

            var basicFeature = new BasicAuthenticationFeature(Module);
            listView1.Items.Add(new AuthenticationListViewItem(basicFeature, this));

            var digestFeature = new DigestAuthenticationFeature(Module);
            listView1.Items.Add(new AuthenticationListViewItem(digestFeature, this));

            var formsFeature = new FormsAuthenticationFeature(Module);
            listView1.Items.Add(new AuthenticationListViewItem(formsFeature, this));

            var windowsFeature = new WindowsAuthenticationFeature(Module);
            listView1.Items.Add(new AuthenticationListViewItem(windowsFeature, this));

            certificateFeature.Load();
            anonymousFeature.Load();
            impersonationFeature.Load();
            basicFeature.Load();
            digestFeature.Load();
            formsFeature.Load();
            windowsFeature.Load();

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
                Process.Start("http://go.microsoft.com/fwlink/?LinkId=210461");
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

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System;

    using JexusManager.Main.Properties;

    using Microsoft.Web.Management.Client.Win32;

    public partial class ConnectionWizard : WizardForm
    {
        private readonly string[] _names;

        private ConnectionWizardData _wizardData;

        public ConnectionWizard(IServiceProvider serviceProvider, string[] names)
            : base(serviceProvider)
        {
            _names = names;
            InitializeComponent();
            TaskGlyph = Resources.title_48;
        }

        protected internal override object WizardData
        {
            get { return _wizardData ?? (_wizardData = new ConnectionWizardData(_names)); }
        }

        protected override void CompleteWizard()
        {
            Close();
        }

        protected override WizardPage[] GetWizardPages()
        {
            var solutionType = new SolutionTypePage();
            var server = new ServerPage();
            var credentials = new CredentialsPage();
            var finish = new FinishPage();
            var browse = new BrowsePage(solutionType, finish);
            var type = new TypePage(server, browse);
            type.SetWizard(this);
            solutionType.SetWizard(this);
            solutionType.SetPreviousPage(browse);
            solutionType.SetNextPage(finish);
            browse.SetPreviousPage(type);
            browse.SetWizard(this);
            server.SetPreviousPage(type);
            server.SetNextPage(credentials);
            server.SetWizard(this);
            credentials.SetPreviousPage(server);
            credentials.SetNextPage(finish);
            credentials.SetWizard(this);
            finish.SetPreviousPage(credentials);
            finish.SetWizard(this);
            return new WizardPage[] { type, browse, solutionType, server, credentials, finish };
        }

        protected override void ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210463#ServerConnectionDetails");
        }
    }
}

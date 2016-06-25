// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System;

    using Microsoft.Web.Management.Client.Win32;

    public partial class ServerPage : WizardPage
    {
        public ServerPage()
        {
            InitializeComponent();
            Caption = "Specify Server Connection Details";
        }

        protected internal override bool CanNavigateNext
        {
            get { return base.CanNavigateNext && !string.IsNullOrWhiteSpace(cbHostName.Text); }
        }

        private void cbHostName_TextChanged(object sender, EventArgs e)
        {
            var wizardData = ((ConnectionWizardData)WizardData);
            wizardData.HostName = cbHostName.Text;
            UpdateWizard();
        }

        protected internal override void Activate()
        {
            base.Activate();
            cbHostName.Focus();
        }
    }
}

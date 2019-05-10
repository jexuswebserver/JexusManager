// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Wizards.ConnectionWizard
{
    using System;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class FinishPage : WizardPage
    {
        private bool _initialized;

        public FinishPage()
        {
            InitializeComponent();
            Caption = "Specify a Connection Name";
        }

        protected internal override bool CanNavigateNext
        {
            get
            {
                return !string.IsNullOrWhiteSpace(txtName.Text)
                       && ((ConnectionWizardData)WizardData).VerifyName(txtName.Text);
            }
        }

        private void TxtNameTextChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            ((ConnectionWizardData)WizardData).Name = txtName.Text;
            this.UpdateWizard();
        }

        protected internal override void Activate()
        {
            base.Activate();
            _initialized = false;
            var connectionWizardData = (ConnectionWizardData)WizardData;
            txtName.Text = connectionWizardData.HostName.ExtractName();
            connectionWizardData.Name = txtName.Text;
            _initialized = true;
            txtName.Focus();
            txtName.SelectAll();
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRenewWizard
{
    using System;

    public partial class OptionsPage : DefaultWizardPage
    {
        public OptionsPage()
        {
            InitializeComponent();
            Caption = "Renew an existing certificate";
        }

        private void Option_Click(object sender, EventArgs e)
        {
            if (rbRequestRenewal.Checked)
            {
                var finish = (DefaultWizardPage)Pages[1];
                SetNextPage(finish);
                finish.SetPreviousPage(this);
            }

            UpdateWizard();
        }
    }
}

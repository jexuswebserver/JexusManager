// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRequestWizard
{
    using System;

    public partial class KeysPage : DefaultWizardPage
    {
        public KeysPage()
        {
            InitializeComponent();
            Caption = "Cryptographic Service Provider Properties";
        }

        protected override void Activate()
        {
            base.Activate();
            var wizardData = (CertificateRequestWizardData)WizardData;
            cbProvider.SelectedIndex = wizardData.Provider;
        }

        private void CbProviderSelectedIndexChanged(object sender, EventArgs e)
        {
            cbLength.Items.Clear();
            if (cbProvider.SelectedIndex == 0)
            {
                cbLength.Items.Add("512");
                cbLength.Items.Add("1024");
                cbLength.SelectedIndex = 0;
            }
            else
            {
                cbLength.Items.Add("384");
                cbLength.Items.Add("512");
                cbLength.Items.Add("1024");
                cbLength.Items.Add("2048");
                cbLength.Items.Add("4096");
                cbLength.Items.Add("8192");
                cbLength.Items.Add("16384");
                cbLength.SelectedIndex = 2;
            }
        }

        private void CbLengthSelectedIndexChanged(object sender, EventArgs e)
        {
            var wizardData = (CertificateRequestWizardData)WizardData;
            wizardData.Provider = cbProvider.SelectedIndex;
            wizardData.Length = int.Parse(cbLength.Text);
        }
    }
}

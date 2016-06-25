// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRenewWizard
{
    using System;
    using System.Windows.Forms;

    public partial class FinishPage : DefaultWizardPage
    {
        private bool _initialized;

        public FinishPage()
        {
            InitializeComponent();
            Caption = "Specify save as file name";
        }


        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            ((CertificateRenewWizardData)WizardData).FileName = txtPath.Text;
            ((DefaultWizardForm)Wizard).UpdateWizard();
        }

        protected override void Activate()
        {
            base.Activate();
            _initialized = false;
            txtPath.Text = ((CertificateRenewWizardData)WizardData).FileName;
            _initialized = true;
            txtPath.Focus();
            txtPath.SelectAll();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                FileName = txtPath.Text
            };
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            txtPath.Text = dialog.FileName;
        }

        private void FileStyle_Changed(object sender, EventArgs e)
        {
            ((CertificateRenewWizardData)WizardData).UseIisStyle = rbIis.Checked;
        }
    }
}

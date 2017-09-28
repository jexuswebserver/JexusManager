// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRequestWizard
{
    using System;
    using System.Windows.Forms;

    public partial class FinishPage : DefaultWizardPage
    {
        private bool _initialized;

        public FinishPage()
        {
            InitializeComponent();
            Caption = "File Name";
        }


        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (!_initialized)
            {
                return;
            }

            ((CertificateRequestWizardData)WizardData).FileName = txtPath.Text;
            ((DefaultWizardForm)Wizard).UpdateWizard();
        }

        protected override void Activate()
        {
            base.Activate();
            _initialized = false;
            txtPath.Text = ((CertificateRequestWizardData)WizardData).FileName;
            _initialized = true;
            txtPath.Focus();
            txtPath.SelectAll();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogHelper.ShowSaveFileDialog(txtPath, "*.txt|*.txt|*.*|*.*");
        }

        private void FileStyle_Changed(object sender, EventArgs e)
        {
            ((CertificateRequestWizardData)WizardData).UseIisStyle = rbIis.Checked;
        }
    }
}

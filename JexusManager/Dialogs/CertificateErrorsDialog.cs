// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;

    public partial class CertificateErrorsDialog : Form
    {
        private readonly X509Certificate _certificate;

        public CertificateErrorsDialog(X509Certificate certificate)
        {
            InitializeComponent();
            _certificate = certificate;
        }

        private void BtnViewClick(object sender, EventArgs e)
        {
            DialogHelper.DisplayCertificate((X509Certificate2)_certificate, Handle);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210463#CertificateNameMismatch");
        }

        private void CertificateErrorsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210463#CertificateVerificationHelp");
        }
    }
}

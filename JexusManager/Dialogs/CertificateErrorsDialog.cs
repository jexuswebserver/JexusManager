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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public partial class CertificateErrorsDialog : Form
    {
        public CertificateErrorsDialog(X509Certificate certificate)
        {
            InitializeComponent();
            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnView, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.DisplayCertificate((X509Certificate2)certificate, Handle);
                }));
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

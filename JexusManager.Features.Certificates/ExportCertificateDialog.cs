// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    public partial class ExportCertificateDialog : DialogForm
    {
        public ExportCertificateDialog(X509Certificate certificate2, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPassword, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtConfirm, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtPath.Text)
                                    && !string.IsNullOrWhiteSpace(txtPassword.Text)
                                    && !string.IsNullOrWhiteSpace(txtConfirm.Text)
                                    && txtPassword.Text == txtConfirm.Text;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
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
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    File.WriteAllBytes(txtPath.Text, certificate2.Export(X509ContentType.Pfx, txtPassword.Text));
                    DialogResult = DialogResult.OK;
                }));
        }

        private void ExportCertificateDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210528");
        }
    }
}

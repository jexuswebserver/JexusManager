// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class ExportCertificateDialog : DialogForm
    {
        public ExportCertificateDialog(X509Certificate certificate2, IServiceProvider serviceProvider, CertificatesFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPassword, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtConfirm, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
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
                    DialogHelper.ShowSaveFileDialog(txtPath, "*.pfx|*.pfx|*.*|*.*", null);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    File.WriteAllBytes(txtPath.Text, certificate2.Export(X509ContentType.Pfx, txtPassword.Text));
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }
    }
}

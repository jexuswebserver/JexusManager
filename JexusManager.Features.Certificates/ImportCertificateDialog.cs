// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace JexusManager.Features.Certificates
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Microsoft.Extensions.Logging;
    using JexusManager;

    using Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.OpenSsl;
    using Org.BouncyCastle.Security;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
    using System.Security.Cryptography.X509Certificates;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Reactive.Disposables;
    using System.Text;
    using System.IO;

    internal partial class ImportCertificateDialog : DialogForm
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("ImportCertificateDialog");

        public ImportCertificateDialog(IServiceProvider serviceProvider, CertificatesFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbStore.SelectedIndex = 0;
            if (Environment.OSVersion.Version < Version.Parse("6.2"))
            {
                // IMPORTANT: WebHosting store is available since Windows 8.
                cbStore.Enabled = false;
            }

            if (!Helper.IsRunningOnMono())
            {
                JexusManager.NativeMethods.TryAddShieldToButton(btnOK);
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowOpenFileDialog(txtFile, ".pfx|*.pfx|*.*|*.*", null);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtFile, "TextChanged")
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtFile.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    try
                    {
                        // Load your certificate from file
                        Item = X509CertificateLoader.LoadPkcs12FromFile(txtFile.Text, txtPassword.Text, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                        Item.FriendlyName = txtName.Text;
                        Store = cbStore.SelectedIndex == 0 ? "Personal" : "WebHosting";
                        if (((CertificatesModule)ServiceProvider).Proxy.InstallFromFile(txtFile.Text, txtPassword.Text, txtName.Text, Store))
                        {
                            DialogResult = DialogResult.OK;
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex, Text, false);
                    }
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Store { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public X509Certificate2 Item { get; set; }
    }
}

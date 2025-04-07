// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Web.Administration;

namespace JexusManager.Features.Certificates
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Microsoft.Extensions.Logging;
    using JexusManager;

    using Microsoft.Web.Management.Client.Win32;
    using Mono.Security.Authenticode;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
    using System.Security.Cryptography.X509Certificates;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.IO;
    using System.Diagnostics;

    public partial class CompleteRequestDialog : DialogForm
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("CompleteRequestDialog");

        public CompleteRequestDialog(IServiceProvider serviceProvider, CertificatesFeature feature)
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
                NativeMethods.TryAddShieldToButton(btnOK);
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                        && !string.IsNullOrWhiteSpace(txtPath.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (!File.Exists(txtPath.Text))
                    {
                        ShowMessage(
                            string.Format(
                                "There was an error while performing this operation.{0}{0}Details:{0}{0}Could not find file '{1}'.",
                                Environment.NewLine,
                                txtPath.Text),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    var p12File = DialogHelper.GetTempFileName();
                    var p12pwd = "test";
                    try
                    {
                        // TODO: check administrator permission.
                        var x509 = new X509Certificate2(txtPath.Text);
                        var filename = DialogHelper.GetPrivateKeyFile(x509.Subject);
                        if (!File.Exists(filename))
                        {
                            ShowMessage(
                                string.Format(
                                    "There was an error while performing this operation.{0}{0}Details:{0}{0}Could not find private key for '{1}'.",
                                    Environment.NewLine,
                                    txtPath.Text),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        x509.CopyWithPrivateKey(PrivateKey.CreateFromFile(filename).RSA);
                        x509.FriendlyName = txtName.Text;
                        var raw = x509.Export(X509ContentType.Pfx, p12pwd);
                        File.WriteAllBytes(p12File, raw);
                        Item = x509;
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex, Text, false);
                        return;
                    }

                    Store = cbStore.SelectedIndex == 0 ? "Personal" : "WebHosting";

                    Install(p12File, p12pwd);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowOpenFileDialog(txtPath, "*.cer|*.cer|*.*|*.*", null);
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

        private void Install(string p12File, string p12pwd)
        {
            try
            {
                // add certificate
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /f:\"{p12File}\" /p:{p12pwd} /n:\"{txtName.Text}\" /s:{(cbStore.SelectedIndex == 0 ? "MY" : "WebHosting")}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                File.Delete(p12File);
                if (process.ExitCode == 0)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(process.ExitCode.ToString());
                }
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    _logger.LogError(ex, "Win32 error during certificate installation. Native error code: {Code}", ex.NativeErrorCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during certificate installation");
            }
        }
    }
}

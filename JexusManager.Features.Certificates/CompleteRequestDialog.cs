// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Web.Administration;
using RollbarDotNet;

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
    using Mono.Security.Authenticode;

    internal partial class CompleteRequestDialog : DialogForm
    {
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

                        x509.PrivateKey = PrivateKey.CreateFromFile(filename).RSA;
                        x509.FriendlyName = txtName.Text;
                        var raw = x509.Export(X509ContentType.Pfx, p12pwd);
                        File.WriteAllBytes(p12File, raw);
                        Item = x509;
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex, string.Empty, false);
                        return;
                    }

                    Store = cbStore.SelectedIndex == 0 ? "Personal" : "WebHosting";

                    try
                    {
                        // add certificate
                        using (var process = new Process())
                        {
                            var start = process.StartInfo;
                            start.Verb = "runas";
                            start.FileName = "cmd";
                            start.Arguments = string.Format("/c \"\"{4}\" /f:\"{0}\" /p:{1} /n:\"{2}\" /s:{3}\"",
                                p12File,
                                p12pwd,
                                txtName.Text,
                                cbStore.SelectedIndex == 0 ? "MY" : "WebHosting",
                                Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe"));
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
                    }
                    catch (Win32Exception ex)
                    {
                        // elevation is cancelled.
                        if (ex.NativeErrorCode != Microsoft.Web.Administration.NativeMethods.ErrorCancelled)
                        {
                            Rollbar.Report(ex, ErrorLevel.Error, new Dictionary<string, object> {{ "native", ex.NativeErrorCode } });
                            // throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        Rollbar.Report(ex, ErrorLevel.Error);
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowOpenFileDialog(txtPath, "*.cer|*.cer|*.*|*.*");
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        public string Store { get; set; }

        public X509Certificate2 Item { get; set; }
    }
}

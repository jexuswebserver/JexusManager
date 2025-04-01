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
                        Item = new X509Certificate2(txtFile.Text, txtPassword.Text, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet)
                        {
                            FriendlyName = txtName.Text
                        };
                        Store = cbStore.SelectedIndex == 0 ? "Personal" : "WebHosting";
                        var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                        if (service.ServerManager.Mode == WorkingMode.Jexus)
                        {
                            var server = (JexusServerManager)service.Server;
                            // Public Key;
                            StringBuilder publicBuilder = new StringBuilder();
                            publicBuilder.AppendLine("-----BEGIN CERTIFICATE-----");
                            publicBuilder.AppendLine(Convert.ToBase64String(Item.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
                            publicBuilder.AppendLine("-----END CERTIFICATE-----");
                            var file = AsyncHelper.RunSync(() => server.SaveCertificateAsync(publicBuilder.ToString()));
                            server.SetCertificate(file);
                            // Private Key
                            var rsa = Item.GetRSAPrivateKey();
                            MemoryStream memoryStream = new MemoryStream();
                            TextWriter streamWriter = new StreamWriter(memoryStream);
                            PemWriter pemWriter = new PemWriter(streamWriter);
                            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
                            pemWriter.WriteObject(keyPair.Private);
                            streamWriter.Flush();
                            string output = Encoding.ASCII.GetString(memoryStream.GetBuffer()).Trim();
                            int indexOfFooter = output.IndexOf("-----END RSA PRIVATE KEY-----", StringComparison.Ordinal);
                            memoryStream.Close();
                            streamWriter.Close();
                            string key = output.Substring(0, indexOfFooter + 29);
                            var keyFile = AsyncHelper.RunSync(() => server.SaveKeyAsync(key));
                            server.SetKeyFile(keyFile);
                            service.ServerManager.CommitChanges();
                        }
                        else
                        {
                            Install(txtFile.Text, txtPassword.Text);
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

        public string Store { get; set; }

        public X509Certificate2 Item { get; set; }

        private void Install(string fileName, string password)
        {
            try
            {
                // install certificate
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /f:\"{fileName}\" /p:\"{password}\" /n:\"{txtName.Text}\" /s:{(cbStore.SelectedIndex == 0 ? "MY" : "WebHosting")}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    DialogResult = DialogResult.OK;
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

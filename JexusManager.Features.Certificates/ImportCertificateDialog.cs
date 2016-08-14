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
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.OpenSsl;
    using Org.BouncyCastle.Security;

    public partial class ImportCertificateDialog : DialogForm
    {
        public ImportCertificateDialog(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbStore.SelectedIndex = 0;
            if (Environment.OSVersion.Version.Major < 8)
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
                .Subscribe(evt =>
                {
                    var dialog = new OpenFileDialog { Filter = ".pfx|*.pfx|*.*|*.*", FilterIndex = 0, Multiselect = false };
                    if (dialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }

                    txtFile.Text = dialog.FileName;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtFile, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtFile.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(async evt =>
                {
                    try
                    {
                        // Load your certificate from file
                        Item = new X509Certificate2(txtFile.Text, txtPassword.Text, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                        Store = cbStore.SelectedIndex == 0 ? "Personal" : "WebHosting";
                        var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                        if (service.ServerManager.Mode == WorkingMode.Jexus)
                        {
                            // Public Key;
                            StringBuilder publicBuilder = new StringBuilder();
                            publicBuilder.AppendLine("-----BEGIN CERTIFICATE-----");
                            publicBuilder.AppendLine(Convert.ToBase64String(Item.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
                            publicBuilder.AppendLine("-----END CERTIFICATE-----");
                            var file = await service.ServerManager.SaveCertificateAsync(publicBuilder.ToString());
                            service.ServerManager.SetCertificate(file);
                            // Private Key
                            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)Item.PrivateKey;
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
                            var keyFile = await service.ServerManager.SaveKeyAsync(key);
                            service.ServerManager.SetKeyFile(keyFile);
                            await service.ServerManager.CommitChangesAsync();
                        }
                        else
                        {
                            try
                            {
                                using (var process = new Process())
                                {
                                    // add certificate
                                    var start = process.StartInfo;
                                    start.Verb = "runas";
                                    start.FileName = "cmd";
                                    start.Arguments = string.Format(
                                        "/c \"\"{4}\" /f:\"{0}\" /p:{1} /n:\"{2}\" /s:{3}\"",
                                        txtFile.Text,
                                        txtPassword.Text,
                                        Item.FriendlyName,
                                        cbStore.SelectedIndex == 0 ? "MY" : "WebHosting",
                                        Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe"));
                                    start.CreateNoWindow = true;
                                    start.WindowStyle = ProcessWindowStyle.Hidden;
                                    process.Start();
                                    process.WaitForExit();
                                    if (process.ExitCode == 0)
                                    {
                                        this.DialogResult = DialogResult.OK;
                                    }
                                    else
                                    {
                                        MessageBox.Show(process.ExitCode.ToString());
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                // elevation is cancelled.
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var message = new StringBuilder()
                            .AppendLine("There was an error while performing this operation.")
                            .AppendLine()
                            .AppendLine("Details:")
                            .AppendLine()
                            .AppendLine(ex.Message);
                        ShowMessage(
                            message.ToString(),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                    }
                }));
        }

        private void ImportCertificateDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210528");
        }

        public string Store { get; set; }

        public X509Certificate2 Item { get; set; }
    }
}

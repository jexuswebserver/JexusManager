// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client.Win32;
using System;

namespace JexusManager.Features.Main
{
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Windows.Forms;

    using Microsoft.Win32;

    using Org.BouncyCastle.Utilities.Encoders;

    using Binding = Microsoft.Web.Administration.Binding;

    public partial class SslDiagDialog : DialogForm
    {
        private readonly ServerManager _server;

        public SslDiagDialog(IServiceProvider provider, ServerManager server)
            : base(provider)
        {
            InitializeComponent();
            _server = server;
        }

        private void BtnGenerateClick(object sender, System.EventArgs e)
        {
            txtResult.Clear();
            Debug(string.Format("System Time: {0}", DateTime.Now));
            Debug(string.Format("Processor Architecture: {0}", Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")));
            Debug(string.Format("OS: {0}", Environment.OSVersion));
            Debug(string.Format("{0}", _server.Type));
            Debug(Environment.NewLine);
            Debug(string.Format("SERVER SSL PROTOCOLS{0}", Environment.NewLine));
            Debug(string.Format("PCT 1.0: {0}", GetProtocol("PCT 1.0")));
            Debug(string.Format("SSL 2.0: {0}", GetProtocol("SSL 2.0")));
            Debug(string.Format("SSL 3.0: {0}", GetProtocol("SSL 3.0")));
            Debug(string.Format("TLS 1.0: {0}", GetProtocol("TLS 1.0")));
            Debug(string.Format("TLS 1.1: {0}", GetProtocol("TLS 1.1")));
            Debug(string.Format("TLS 1.2: {0}", GetProtocol("TLS 1.2")));

            Debug(string.Format("SChannel EventLogging: {0} (hex)", GetEventLogging()));
            Debug("-----");

            foreach (Site site in _server.Sites)
            {
                Debug(string.Format("[W3SVC/{0}]", site.Id));
                Debug(string.Format("ServerComment  : {0}", site.Name));
                Debug(string.Format("ServerAutoStart: {0}", site.ServerAutoStart));
                Debug(string.Format("ServerState    : {0}", site.State));
                Debug(string.Empty);
                foreach (Binding binding in site.Bindings)
                {
                    Info(string.Format("BINDING: {0} {1}", binding.Protocol, binding));
                    if (binding.Protocol == "https")
                    {
                        var hashString = Hex.ToHexString(binding.CertificateHash);
                        Debug(string.Format("SSLCertHash: {0}", hashString));
                        Debug(string.Format("SSL Flags: {0}", binding.SslFlags));
                        Debug("Testing EndPoint: 127.0.0.1");

                        var personal = new X509Store(binding.CertificateStoreName, StoreLocation.LocalMachine);
                        personal.Open(OpenFlags.MaxAllowed);
                        var selectedItem = personal.Certificates.Find(X509FindType.FindByThumbprint, hashString, false);
                        var cert = selectedItem[0];
                        Debug(string.Format("#CertName: {0}", cert.FriendlyName));
                        Debug(string.Format("#Version: {0}", cert.Version));
                        if (cert.HasPrivateKey)
                        {
                            Debug("#You have a private key that corresponds to this certificate.");
                        }
                        else
                        {
                            Error("#You don't have a private key that corresponds to this certificate.");
                        }

                        var key = cert.PublicKey.Key;
                        Debug(string.Format("#Signature Algorithm: {0}", cert.SignatureAlgorithm.FriendlyName));
                        Debug(string.Format("#Key Exchange Algorithm: {0} Key Size: {1}", key.KeyExchangeAlgorithm, key.KeySize));
                        Debug(string.Format("#Subject: {0}", cert.Subject));
                        Debug(string.Format("#Issuer: {0}", cert.Issuer));
                        Debug(string.Format("#Validity: From {0} To {1}", cert.NotBefore.ToString("U"), cert.NotAfter.ToString("U")));
                        Debug(string.Format("#Serial Number: {0}", cert.SerialNumber));
                        Debug(string.Format("DS Mapper Usage: {0}", binding.UseDsMapper ? "Enabled" : "Disabled"));
                        Debug(string.Format("Archived: {0}", cert.Archived));

                        foreach (var extension in cert.Extensions)
                        {
                            if (extension.Oid.FriendlyName == "Key Usage")
                            {
                                Debug(string.Format("#Key Usage: {0}", ((X509KeyUsageExtension)extension).KeyUsages));
                                continue;
                            }

                            if (extension.Oid.FriendlyName == "Enhanced Key Usage")
                            {
                                var enhancedKeyUsage = new StringBuilder();
                                var usages = ((X509EnhancedKeyUsageExtension)extension).EnhancedKeyUsages;
                                foreach (var usage in usages)
                                {
                                    enhancedKeyUsage.AppendFormat("{0} ({1}), ", usage.FriendlyName, usage.Value);
                                }

                                if (enhancedKeyUsage.Length > 0)
                                {
                                    enhancedKeyUsage.Length -= 2;
                                }

                                Debug(string.Format("#Enhanced Key Usage: {0}", enhancedKeyUsage));
                                continue;
                            }

                            if (extension.Oid.FriendlyName == "Basic Constraints")
                            {
                                var ext = (X509BasicConstraintsExtension)extension;
                                Debug(
                                    string.Format(
                                        "#Basic Constraints: Subject Type={0}, Path Length Constraint={1}",
                                        ext.CertificateAuthority ? "CA" : "End Entity",
                                        ext.HasPathLengthConstraint ? ext.PathLengthConstraint.ToString() : "None"));
                                continue;
                            }
                        }

                        X509Chain chain = X509Chain.Create();
                        X509ChainPolicy policy = new X509ChainPolicy();
                        policy.RevocationMode = X509RevocationMode.NoCheck;
                        chain.ChainPolicy = policy;
                        bool valid = chain.Build(cert);
                        if (valid)
                        {
                            Debug("Certificate verified.");
                        }
                        else
                        {
                            Error("Certificate valication failed.");
                        }

                        foreach (var item in chain.ChainStatus)
                        {
                            Warn(item.StatusInformation);
                        }

                        personal.Close();
                    }

                    Debug(string.Empty);
                }
            }
        }

        private void Debug(string text)
        {
            txtResult.AppendText(text);
            txtResult.AppendText(Environment.NewLine);
        }

        private void Error(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Red;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private void Warn(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Green;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private void Info(string text)
        {
            var defaultColor = txtResult.SelectionColor;
            txtResult.SelectionColor = Color.Blue;
            txtResult.AppendText(text);
            txtResult.SelectionColor = defaultColor;
            txtResult.AppendText(Environment.NewLine);
        }

        private int GetEventLogging()
        {
            if (Helper.IsRunningOnMono())
            {
                return -1;
            }

            // https://support.microsoft.com/en-us/kb/260729
            var key = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\SecurityProviders\SCHANNEL");
            if (key == null)
            {
                return 1;
            }

            var value = (int)key.GetValue("EventLogging", 1);
            return value;
        }

        private static bool GetProtocol(string protocol)
        {
            if (Helper.IsRunningOnMono())
            {
                return false;
            }

            // https://support.microsoft.com/en-us/kb/187498
            var key =
                Registry.LocalMachine.OpenSubKey(
                    string.Format(
                        @"SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\{0}\Server",
                        protocol));
            if (key == null)
            {
                return true;
            }

            var value = (int)key.GetValue("Enabled", 1);
            var enabled = value == 1;
            return enabled;
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Text Files|*.txt|All Files|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            File.WriteAllText(dialog.FileName, txtResult.Text);
        }

        private void BtnHelpClick(object sender, EventArgs e)
        {
            Process.Start("https://blog.lextudio.com/2015/11/jexus-manager-built-in-ssl-diagnostics/");
        }

        private void SslDiagDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BtnHelpClick(null, EventArgs.Empty);
        }

        private void BtnVerifyClick(object sender, EventArgs e)
        {
            txtResult.Clear();
        }
    }
}

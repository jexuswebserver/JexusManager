// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client.Win32;
using System;
using System.Linq;
using System.Security.Cryptography;

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
            try
            {
                Debug($"System Time: {DateTime.Now}");
                Debug($"Processor Architecture: {Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}");
                Debug($"OS: {Environment.OSVersion}");
                Debug($"{_server.Type}");
                Debug(Environment.NewLine);
                Debug($"SERVER SSL PROTOCOLS{Environment.NewLine}");
                Debug($"PCT 1.0: {GetProtocol("PCT 1.0")}");
                Debug($"SSL 2.0: {GetProtocol("SSL 2.0")}");
                Debug($"SSL 3.0: {GetProtocol("SSL 3.0")}");
                Debug($"TLS 1.0: {GetProtocol("TLS 1.0")}");
                Debug($"TLS 1.1: {GetProtocol("TLS 1.1")}");
                Debug($"TLS 1.2: {GetProtocol("TLS 1.2")}");

                Debug($"SChannel EventLogging: {GetEventLogging()} (hex)");
                Debug("-----");

                foreach (Site site in _server.Sites)
                {
                    Debug($"[W3SVC/{site.Id}]");
                    Debug($"ServerComment  : {site.Name}");
                    Debug($"ServerAutoStart: {site.ServerAutoStart}");
                    Debug($"ServerState    : {site.State}");
                    Debug(string.Empty);
                    foreach (Binding binding in site.Bindings)
                    {
                        Info($"BINDING: {binding.Protocol} {binding}");
                        if (binding.Protocol == "https")
                        {
                            var hashString = Hex.ToHexString(binding.CertificateHash);
                            Debug($"SSLCertHash: {hashString}");
                            if (site.Server.SupportsSni)
                            {
                                Debug($"SSL Flags: {binding.SslFlags}");
                            }

                            Debug("Testing EndPoint: 127.0.0.1");

                            var personal = new X509Store(binding.CertificateStoreName, StoreLocation.LocalMachine);
                            personal.Open(OpenFlags.MaxAllowed);
                            var selectedItem = personal.Certificates.Find(X509FindType.FindByThumbprint, hashString, false);
                            var cert = selectedItem[0];
                            Debug($"#CertName: {cert.FriendlyName}");
                            Debug($"#Version: {cert.Version}");
                            if (cert.HasPrivateKey)
                            {
                                Debug("#You have a private key that corresponds to this certificate.");
                            }
                            else
                            {
                                Error("#You don't have a private key that corresponds to this certificate.");
                            }

                            var key = cert.PublicKey.Key;
                            Debug($"#Signature Algorithm: {cert.SignatureAlgorithm.FriendlyName}");
                            Debug($"#Key Exchange Algorithm: {key.KeyExchangeAlgorithm} Key Size: {key.KeySize}");
                            Debug($"#Subject: {cert.Subject}");
                            Debug($"#Issuer: {cert.Issuer}");
                            Debug($"#Validity: From {cert.NotBefore:U} To {cert.NotAfter:U}");
                            Debug($"#Serial Number: {cert.SerialNumber}");
                            Debug($"DS Mapper Usage: {(binding.UseDsMapper ? "Enabled" : "Disabled")}");
                            Debug($"Archived: {cert.Archived}");

                            foreach (var extension in cert.Extensions)
                            {
                                if (extension.Oid.FriendlyName == "Key Usage")
                                {
                                    Debug($"#Key Usage: {((X509KeyUsageExtension) extension).KeyUsages}");
                                    continue;
                                }

                                if (extension.Oid.FriendlyName == "Enhanced Key Usage")
                                {
                                    var usages = ((X509EnhancedKeyUsageExtension)extension).EnhancedKeyUsages;
                                    var enhancedKeyUsage = usages.Cast<Oid>().Select(usage => $"{usage.FriendlyName} ({usage.Value})")
                                        .Combine(",");

                                    Debug($"#Enhanced Key Usage: {enhancedKeyUsage}");
                                    continue;
                                }

                                if (extension.Oid.FriendlyName == "Basic Constraints")
                                {
                                    var ext = (X509BasicConstraintsExtension)extension;
                                    Debug(
                                        $"#Basic Constraints: Subject Type={(ext.CertificateAuthority ? "CA" : "End Entity")}, Path Length Constraint={(ext.HasPathLengthConstraint ? ext.PathLengthConstraint.ToString() : "None")}");
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
            catch (Exception ex)
            {
                Debug(ex.ToString());
                RollbarDotNet.Rollbar.Report(ex);
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
                    $@"SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\{protocol}\Server");
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

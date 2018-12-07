// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Web.Administration;
using Rollbar;

namespace JexusManager.Features.Certificates
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;
    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Digests;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Crypto.Operators;
    using Org.BouncyCastle.Crypto.Prng;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Pkcs;
    using Org.BouncyCastle.Security;
    using Org.BouncyCastle.Utilities;
    using Org.BouncyCastle.X509;

    internal partial class SelfCertificateDialog : DialogForm
    {
        public SelfCertificateDialog(IServiceProvider serviceProvider, CertificatesFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbStore.SelectedIndex = 0;
            cbLength.SelectedIndex = 3;
            cbHashing.SelectedIndex = 1;
            txtCommonName.Text = Environment.MachineName;
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = dtpFrom.Value.AddYears(1);

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
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var names = txtCommonName.Text;
                    if (string.IsNullOrWhiteSpace(names))
                    {
                        ShowMessage("DNS names cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    var dnsNames = names.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).ToArray();
                    if (dnsNames.Length == 0)
                    {
                        ShowMessage("DNS names cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    // Generate certificate
                    string defaultIssuer = string.Format("CN={0}", dnsNames[0]);
                    string defaultSubject = defaultIssuer;

                    string subject = defaultSubject;
                    string issuer = defaultIssuer;

                    if (subject == null)
                        throw new Exception("Missing Subject Name");

                    DateTime notBefore = dtpFrom.Value;
                    DateTime notAfter = dtpTo.Value;

                    var random = new SecureRandom(new CryptoApiRandomGenerator());
                    var kpgen = new RsaKeyPairGenerator();
                    kpgen.Init(new KeyGenerationParameters(random, int.Parse(cbLength.Text)));
                    var cerKp = kpgen.GenerateKeyPair();

                    X509V3CertificateGenerator certGen = new X509V3CertificateGenerator();

                    var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(long.MaxValue), random);
                    certGen.SetSerialNumber(serialNumber);
                    certGen.SetIssuerDN(new X509Name(issuer));
                    certGen.SetNotBefore(notBefore);
                    certGen.SetNotAfter(notAfter);
                    if (dnsNames.Length == 1)
                    {
                        certGen.SetSubjectDN(new X509Name(subject));
                    }

                    certGen.SetPublicKey(cerKp.Public);
                    certGen.AddExtension(X509Extensions.BasicConstraints, true, new BasicConstraints(true));

                    var keyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(cerKp.Public);
                    certGen.AddExtension(X509Extensions.SubjectKeyIdentifier, true, new SubjectKeyIdentifier(keyInfo));
                    certGen.AddExtension(X509Extensions.AuthorityKeyIdentifier, true, new AuthorityKeyIdentifier(keyInfo));
                    certGen.AddExtension(X509Extensions.ExtendedKeyUsage, true, new ExtendedKeyUsage(KeyPurposeID.IdKPServerAuth));

                    if (cbGenerate.Checked)
                    {
                        var subjectAlternativeNames = new List<Asn1Encodable>();
                        foreach (var item in dnsNames)
                        {
                            subjectAlternativeNames.Add(new GeneralName(GeneralName.DnsName, item));
                        }
                        var subjectAlternativeNamesExtension = new DerSequence(subjectAlternativeNames.ToArray());
                        certGen.AddExtension(X509Extensions.SubjectAlternativeName, false, subjectAlternativeNamesExtension);
                    }

                    string hashName = cbHashing.SelectedIndex == 0 ? "SHA1WithRSA" : "SHA256WithRSA";
                    var factory = new Asn1SignatureFactory(hashName, cerKp.Private, random);
                                       
                    string p12File = Path.GetTempFileName();
                    string p12pwd = "test";

                    try
                    {
                        Org.BouncyCastle.X509.X509Certificate x509 = certGen.Generate(factory);
                        var store = new Pkcs12Store();
                        var certificateEntry = new X509CertificateEntry(x509);
                        var friendlyName = txtName.Text;
                        store.SetCertificateEntry(friendlyName, certificateEntry);
                        store.SetKeyEntry(friendlyName, new AsymmetricKeyEntry(cerKp.Private), new[] { certificateEntry });
                        var stream = new MemoryStream();
                        store.Save(stream, p12pwd.ToCharArray(), random);
                        File.WriteAllBytes(p12File, stream.ToArray());

                        Item = new X509Certificate2(p12File, p12pwd) { FriendlyName = friendlyName };
                        Store = cbStore.SelectedIndex == 0 ? "Personal" : "WebHosting";

                        try
                        {
                            using (var process = new Process())
                            {
                                // add certificate
                                var start = process.StartInfo;
                                start.Verb = "runas";
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
                                    ShowMessage(process.ExitCode.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                                }
                            }
                        }
                        catch (Win32Exception ex)
                        {
                            // elevation is cancelled.
                            if (ex.NativeErrorCode != Microsoft.Web.Administration.NativeMethods.ErrorCancelled)
                            {
                                RollbarLocator.RollbarInstance.Error(ex, new Dictionary<string, object> { { "native", ex.NativeErrorCode } });
                                // throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            RollbarLocator.RollbarInstance.Error(ex);
                        }
                    }
                    catch (Exception ex)
                    {
                        RollbarLocator.RollbarInstance.Error(ex);
                        ShowError(ex, "Certificate generation error", false);
                        return;
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
    }
}

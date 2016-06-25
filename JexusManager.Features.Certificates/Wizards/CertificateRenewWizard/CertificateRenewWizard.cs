// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates.Wizards.CertificateRenewWizard
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;

    using Microsoft.Web.Management.Client.Win32;

    using Mono.Security.Authenticode;

    using Org.BouncyCastle.Asn1;
    using Org.BouncyCastle.Asn1.X509;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Pkcs;
    using Org.BouncyCastle.Security;
    using Properties;
    using Org.BouncyCastle.Crypto.Operators;

    public partial class CertificateRenewWizard : WizardForm
    {
        private readonly X509Certificate2 _existing;
        private CertificateRenewWizardData _wizardData;

        public CertificateRenewWizard(X509Certificate2 existing, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _existing = existing;
            InitializeComponent();
            TaskGlyph = Resources.certificates_48;
        }

        protected override object WizardData
        {
            get { return _wizardData ?? (_wizardData = new CertificateRenewWizardData()); }
        }

        protected override void CompleteWizard()
        {
            X509Name subjectName = new X509Name(_existing.Subject);

            // Generate the private/public keypair 
            var keyPair = DotNetUtilities.GetKeyPair(_existing.PrivateKey);

            // Generate the CSR 
            Asn1Set attributes = new DerSet(
                new DerSequence(
                    new DerObjectIdentifier("1.3.6.1.4.1.311.13.2.3"),
                    new DerSet(new DerIA5String(Environment.OSVersion.Version.ToString()))),
                new DerSequence(
                    new DerObjectIdentifier("1.3.6.1.4.1.311.21.20"),
                    new DerSet(
                        new DerSequence(
                            new DerInteger(5),
                            new DerUtf8String(Environment.MachineName),
                            new DerUtf8String(Environment.UserName),
                            new DerUtf8String("JexusManager.exe")))),
                new DerSequence(
                    new DerObjectIdentifier("1.3.6.1.4.1.311.13.2.2"),
                    new DerSet(
                        new DerSequence(
                            new DerInteger(1),
                            new DerBmpString("Microsoft RSA SChannel Cryptographic Provider"),
                            new DerBitString(new byte[0])))),
                new DerSequence(
                    new DerObjectIdentifier("1.2.840.113549.1.9.14"),
                    new DerSet(
                        new DerSequence(
                            new DerSequence(
                                new DerObjectIdentifier("2.5.29.15"),
                                new DerBoolean(new byte[] { 0x01 }),
                                new DerOctetString(new byte[] { 0x03, 0x02, 0x04, 0xF0 })),
                            new DerSequence(
                                new DerObjectIdentifier("2.5.29.37"),
                                new DerOctetString(new byte[]
                                {
                                    0x30, 0x0a, 0x06, 0x08,
                                    0x2b, 0x06, 0x01, 0x05,
                                    0x05, 0x07, 0x03, 0x01 })),
                            new DerSequence(
                                new DerObjectIdentifier("1.2.840.113549.1.9.15"),
                                new DerOctetString(new byte[]
                                {
                                    0x30, 0x69, 0x30, 0x0e,
                                    0x06, 0x08, 0x2a, 0x86,
                                    0x48, 0x86, 0xf7, 0x0d,
                                    0x03, 0x02, 0x02, 0x02,
                                    0x00, 0x80, 0x30, 0x0e,
                                    0x06, 0x08, 0x2a, 0x86,
                                    0x48, 0x86, 0xf7, 0x0d,
                                    0x03, 0x04, 0x02, 0x02,
                                    0x00, 0x80, 0x30, 0x0b,
                                    0x06, 0x09, 0x60, 0x86,
                                    0x48, 0x01, 0x65, 0x03,
                                    0x04, 0x01, 0x2a, 0x30,
                                    0x0b, 0x06, 0x09, 0x60,
                                    0x86, 0x48, 0x01, 0x65,
                                    0x03, 0x04, 0x01, 0x2d,
                                    0x30, 0x0b, 0x06, 0x09,
                                    0x60, 0x86, 0x48, 0x01,
                                    0x65, 0x03, 0x04, 0x01,
                                    0x02, 0x30, 0x0b, 0x06,
                                    0x09, 0x60, 0x86, 0x48,
                                    0x01, 0x65, 0x03, 0x04,
                                    0x01, 0x05, 0x30, 0x07,
                                    0x06, 0x05, 0x2b, 0x0e,
                                    0x03, 0x02, 0x07, 0x30,
                                    0x0a, 0x06, 0x08, 0x2a,
                                    0x86, 0x48, 0x86, 0xf7,
                                    0x0d, 0x03, 0x07
                                })),
                            new DerSequence(
                                new DerObjectIdentifier("2.5.29.14"),
                                new DerOctetString(new byte[]
                                {
                                    0x04, 0x14, 0xaa, 0x25,
                                    0xd9, 0xa2, 0x39, 0x7e,
                                    0x49, 0xd2, 0x94, 0x85,
                                    0x7e, 0x82, 0xa8, 0x8f,
                                    0x3b, 0x20, 0xf1, 0x4e, 0x65, 0xe5
                                }))))));

            var signing = new Asn1SignatureFactory("SHA256withRSA", keyPair.Private);
            Pkcs10CertificationRequest kpGen = new Pkcs10CertificationRequest(signing, subjectName, keyPair.Public, attributes, keyPair.Private);
            using (var stream = new StreamWriter(_wizardData.FileName))
            {
                stream.WriteLine(_wizardData.UseIisStyle ? "-----BEGIN NEW CERTIFICATE REQUEST-----" : "-----BEGIN CERTIFICATE REQUEST-----");
                stream.WriteLine(Convert.ToBase64String(kpGen.GetDerEncoded(), Base64FormattingOptions.InsertLineBreaks));
                stream.WriteLine(_wizardData.UseIisStyle ? "-----END NEW CERTIFICATE REQUEST-----" : "-----END CERTIFICATE REQUEST-----");
            }

            var key = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair.Private);
            PrivateKey pvk = new PrivateKey();
            pvk.RSA = new RSACryptoServiceProvider();
            pvk.RSA.ImportParameters(key);
            pvk.Save(DialogHelper.GetPrivateKeyFile(_existing.Subject));
        }

        protected override bool CanComplete
        {
            get
            {
                var data = (CertificateRenewWizardData)WizardData;
                return data?.IsComplete ?? false;
            }
        }

        protected override WizardPage[] GetWizardPages()
        {
            var optionsPage = new OptionsPage();
            optionsPage.SetWizard(this);
            var finish = new FinishPage();
            finish.SetWizard(this);
            return new WizardPage[] { optionsPage, finish };
        }

        protected override void ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210528");
        }
    }
}

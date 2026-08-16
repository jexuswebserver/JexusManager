// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace JexusManager.Features.Certificates
{
    internal sealed class CertificatesService : ModuleService
    {
        [ModuleServiceMethod]
        public CertificatesItem[] GetCertificates()
        {
            var result = new List<CertificatesItem>();
            if (ManagementUnit.ServerManager.Mode == WorkingMode.Jexus)
            {
                var server = (JexusServerManager)ManagementUnit.ServerManager;
                var certificate = AsyncHelper.RunSync(() => server.GetCertificateAsync());
                if (certificate != null)
                {
                    result.Add(new CertificatesItem(certificate, "Jexus", null));
                }

                return result.ToArray();
            }

            X509Store personal = new X509Store("MY", StoreLocation.LocalMachine);
            personal.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            foreach (var certificate in personal.Certificates)
            {
                result.Add(new CertificatesItem(certificate, "Personal", null));
            }

            personal.Close();

            if (Environment.OSVersion.Version >= Version.Parse("6.2"))
            {
                // IMPORTANT: WebHosting store is available since Windows 8.
                X509Store hosting = new X509Store("WebHosting", StoreLocation.LocalMachine);
                try
                {
                    hosting.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    foreach (var certificate in hosting.Certificates)
                    {
                        result.Add(new CertificatesItem(certificate, "WebHosting", null));
                    }

                    hosting.Close();
                }
                catch (CryptographicException)
                {
                    // store does not exist.
                }
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public bool InstallFromFile(string fileName, string password, string friendlyName, string store)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("A certificate file is required.", nameof(fileName));
            }

            if (ManagementUnit.ServerManager.Mode == WorkingMode.Jexus)
            {
                var server = (JexusServerManager)ManagementUnit.ServerManager;
                var item = X509CertificateLoader.LoadPkcs12FromFile(
                    fileName,
                    password,
                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                item.FriendlyName = friendlyName;

                // Public Key;
                StringBuilder publicBuilder = new StringBuilder();
                publicBuilder.AppendLine("-----BEGIN CERTIFICATE-----");
                publicBuilder.AppendLine(Convert.ToBase64String(item.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
                publicBuilder.AppendLine("-----END CERTIFICATE-----");
                var file = AsyncHelper.RunSync(() => server.SaveCertificateAsync(publicBuilder.ToString()));
                server.SetCertificate(file);
                // Private Key
                var rsa = item.GetRSAPrivateKey();
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
                ManagementUnit.Update();
                return true;
            }

            return RunElevated($"/f:\"{fileName}\" /p:\"{password}\" /n:\"{friendlyName}\" /s:{(store == "Personal" ? "MY" : "WebHosting")}");
        }

        [ModuleServiceMethod]
        public bool Delete(string thumbprint, string store)
        {
            // remove certificate and mapping
            return RunElevated($"/h:\"{thumbprint}\" /s:{(store == "Personal" ? "MY" : "WebHosting")}");
        }

        [ModuleServiceMethod]
        public void Trust(string thumbprint, string store)
        {
            var cert = FindCertificate(thumbprint, store);
            if (cert == null)
            {
                throw new InvalidOperationException("The certificate was not found in the store.");
            }

            var root = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            root.Open(OpenFlags.ReadWrite);
            try
            {
                if (root.Certificates.Find(X509FindType.FindByThumbprint, cert.Thumbprint, false).Count == 0)
                {
                    root.Add(cert);
                }
            }
            finally
            {
                root.Close();
            }
        }

        private static X509Certificate2 FindCertificate(string thumbprint, string store)
        {
            var name = store == "Personal" ? "MY" : store == "WebHosting" ? "WebHosting" : "MY";
            using X509Store personal = new X509Store(name, StoreLocation.LocalMachine);
            try
            {
                personal.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                var found = personal.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
                return found.Count > 0 ? found[0] : null;
            }
            catch (CryptographicException)
            {
                return null;
            }
        }

        private static bool RunElevated(string arguments)
        {
            try
            {
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" {arguments}";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                return process.ExitCode == 0;
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                if (ex.NativeErrorCode != (int)Windows.Win32.Foundation.WIN32_ERROR.ERROR_CANCELLED)
                {
                    throw;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

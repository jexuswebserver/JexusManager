// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;

    using JexusManager;

    using Org.BouncyCastle.Utilities.Encoders;

    public static class BindingUtility
    {
        private const string AppIdIisExpress = "214124cd-d05b-4309-9af9-9caa44b2b74a";

        private const string AppIdIis = "4dc3e181-e14b-4a21-b022-59fc669b0914";

        internal static void Reinitialize(this Binding original, Binding binding)
        {
            if (original.Parent.Parent.Server.SupportsSni)
            {
                if (original.GetIsSni() && (!binding.GetIsSni() || original.Host != binding.Host))
                {
                    original.CleanUpSni();
                }

                original.SslFlags = binding.SslFlags;
            }

            original.BindingInformation = binding.BindingInformation;
            original.Protocol = binding.Protocol;
            original.CertificateHash = binding.CertificateHash;
            original.CertificateStoreName = binding.CertificateStoreName;
            binding.Delete();
        }

        internal static string FixCertificateMapping(this Binding binding, X509Certificate2 certificate2)
        {
            if (binding.Protocol == "http")
            {
                return string.Empty;
            }

            if (binding.Parent.Parent.Server.SupportsSni)
            {
                if (binding.GetIsSni())
                {
                    if (certificate2.GetNameInfo(X509NameType.DnsName, false) != binding.Host)
                    {
                        return "SNI mode requires host name matches common name of the certificate";
                    }

                    // handle SNI
                    var sni = NativeMethods.QuerySslSniInfo(new Tuple<string, int>(binding.Host,
                        binding.EndPoint.Port));
                    if (sni == null)
                    {
                        try
                        {
                            // register mapping
                            using (var process = new Process())
                            {
                                var start = process.StartInfo;
                                start.Verb = "runas";
                                start.FileName = "cmd";
                                start.Arguments =
                                    $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /h:\"{Hex.ToHexString(binding.CertificateHash)}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /a:{binding.EndPoint.Address} /o:{binding.EndPoint.Port} /x:{binding.Host}";
                                start.CreateNoWindow = true;
                                start.WindowStyle = ProcessWindowStyle.Hidden;
                                process.Start();
                                process.WaitForExit();

                                if (process.ExitCode != 0)
                                {
                                    return "Register new certificate failed: access is denied";
                                }

                                return string.Empty;
                            }
                        }
                        catch (Exception)
                        {
                            // elevation is cancelled.
                            return "Register new certificate failed: operation is cancelled";
                        }
                    }

                    if (!sni.Hash.SequenceEqual(binding.CertificateHash))
                    {
                        // TODO: fix the error message.
                        var result =
                            MessageBox.Show(
                                "At least one other site is using the same HTTPS binding and the binding is configured with a different certificate. Are you sure that you want to reuse this HTTPS binding and reassign the other site or sites to use the new certificate?",
                                "TODO",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button1);
                        if (result != DialogResult.Yes)
                        {
                            return
                                "Certificate hash does not match. Please use the certificate that matches HTTPS binding";
                        }

                        try
                        {
                            // register mapping
                            using (var process = new Process())
                            {
                                var start = process.StartInfo;
                                start.Verb = "runas";
                                start.FileName = "cmd";
                                start.Arguments =
                                    $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /h:\"{Hex.ToHexString(binding.CertificateHash)}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /a:{binding.EndPoint.Address} /o:{binding.EndPoint.Port} /x:{binding.Host}";
                                start.CreateNoWindow = true;
                                start.WindowStyle = ProcessWindowStyle.Hidden;
                                process.Start();
                                process.WaitForExit();

                                if (process.ExitCode != 0)
                                {
                                    return "Register new certificate failed: access is denied";
                                }

                                return string.Empty;
                            }
                        }
                        catch (Exception)
                        {
                            // elevation is cancelled.
                            return "Register new certificate failed: operation is cancelled";
                        }
                    }

                    if (!string.Equals(sni.StoreName, binding.CertificateStoreName, StringComparison.OrdinalIgnoreCase))
                    {
                        // TODO: can this happen?
                        return
                            "Certificate store name does not match. Please use the certificate that matches HTTPS binding";
                    }

                    return string.Empty;
                }
            }

            // handle IP based
            var certificate = NativeMethods.QuerySslCertificateInfo(binding.EndPoint);
            if (certificate == null)
            {
                try
                {
                    // register mapping
                    using (var process = new Process())
                    {
                        var start = process.StartInfo;
                        start.Verb = "runas";
                        start.FileName = "cmd";
                        start.Arguments =
                            $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /h:\"{Hex.ToHexString(binding.CertificateHash)}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /a:{binding.EndPoint.Address} /o:{binding.EndPoint.Port}";
                        start.CreateNoWindow = true;
                        start.WindowStyle = ProcessWindowStyle.Hidden;
                        process.Start();
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            return "Register new certificate failed: access is denied";
                        }

                        return string.Empty;
                    }
                }
                catch (Exception)
                {
                    // elevation is cancelled.
                    return "Register new certificate failed: operation is cancelled";
                }
            }

            if (!certificate.Hash.SequenceEqual(binding.CertificateHash))
            {
                var result =
                    MessageBox.Show(
                        "At least one other site is using the same HTTPS binding and the binding is configured with a different certificate. Are you sure that you want to reuse this HTTPS binding and reassign the other site or sites to use the new certificate?",
                        "TODO",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return "Certificate hash does not match. Please use the certificate that matches HTTPS binding";
                }

                try
                {
                    // register mapping
                    using (var process = new Process())
                    {
                        var start = process.StartInfo;
                        start.Verb = "runas";
                        start.FileName = "cmd";
                        start.Arguments =
                            $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /h:\"{Hex.ToHexString(binding.CertificateHash)}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /a:{binding.EndPoint.Address} /o:{binding.EndPoint.Port}";
                        start.CreateNoWindow = true;
                        start.WindowStyle = ProcessWindowStyle.Hidden;
                        process.Start();
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            return "Register new certificate failed: access is denied";
                        }

                        return string.Empty;
                    }
                }
                catch (Exception)
                {
                    // elevation is cancelled.
                    return "Register new certificate failed: operation is cancelled";
                }
            }

            if (!string.Equals(certificate.StoreName, binding.CertificateStoreName, StringComparison.OrdinalIgnoreCase))
            {
                // TODO: can this happen?
                return
                    "Certificate store name does not match. Please use the certificate that matches HTTPS binding";
            }

            return string.Empty;
        }

        internal static bool? Verify(string protocol, string address, string port, CertificateInfo certificate)
        {
            IPAddress ip = IPAddress.Any;
            if (address != StringExtensions.AllUnassigned && !IPAddress.TryParse(address, out ip))
            {
                return null;
            }

            int portNumber;
            if (!int.TryParse(port, out portNumber))
            {
                return null;
            }

            if (protocol != "https")
            {
                return false;
            }

            if (certificate == null)
            {
                // no certificate
                return null;
            }

            var found = NativeMethods.QuerySslCertificateInfo(new IPEndPoint(ip, portNumber));
            if (found == null)
            {
                // need to create mapping
                return true;
            }

            if (found.Hash.SequenceEqual(certificate.Certificate.GetCertHash()))
            {
                // hash match, no need to change mapping
                return false;
            }

            // replace mapping
            return true;
        }

        public static string GetAppName(string appId)
        {
            if (appId == AppIdIis)
            {
                return "IIS";
            }

            if (appId == AppIdIisExpress)
            {
                return "IIS Express";
            }

            return string.Empty;
        }

        public static string CleanUpSni(this Binding binding)
        {
#if !IIS
            if (!binding.GetIsSni())
            {
                return string.Empty;
            }
#endif
            try
            {
                // remove sni mapping
                using (var process = new Process())
                {
                    var start = process.StartInfo;
                    start.Verb = "runas";
                    start.FileName = "cmd";
                    start.Arguments =
                        $"/c \"\"{Path.Combine(Environment.CurrentDirectory, "certificateinstaller.exe")}\" /h:\"{Hex.ToHexString(binding.CertificateHash)}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /o:{binding.EndPoint.Port} /x:{binding.Host}";
                    start.CreateNoWindow = true;
                    start.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        return "Remove SNI certificate failed: access is denied";
                    }

                    return string.Empty;
                }
            }
            catch (Exception)
            {
                // elevation is cancelled.
                return "Remove SNI certificate failed: operation is cancelled";
            }
        }
    }
}

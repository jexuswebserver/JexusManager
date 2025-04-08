// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exception = System.Exception;

namespace Microsoft.Web.Administration
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    using System.Windows.Forms;
    using Microsoft.Extensions.Logging;
    using JexusManager;
    using Org.BouncyCastle.Utilities.Encoders;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

    public enum CertificateMappingState
    {
        HostNameNotMatched,
        RegistrationFailed,
        RegistrationSucceeded,
        UacCancelled,
        Win32ErrorOccurred,
        GenericErrorOccurred,
        CertificateHashNotMatched,
        CertificateStoreNotMatched,
        IpEndPointInvalid,
    }

    public static class BindingUtility
    {
        private static readonly ILogger _logger = LogHelper.GetLogger("BindingUtility");

        private const string AppIdIisExpress = "214124cd-d05b-4309-9af9-9caa44b2b74a";

        private const string AppIdIis = "4dc3e181-e14b-4a21-b022-59fc669b0914";
        private const string SubjectAlternativeNameOid = "2.5.29.17";
        private const string DnsNamePrefix = "DNS Name=";

        internal static void Reinitialize(this Binding original, Binding binding)
        {
            if (original.Parent.Parent.Server.SupportsSni)
            {
                if (original.GetIsSni() && (!binding.GetIsSni() || original.Host != binding.Host))
                {
                    original.CleanUpMapping();
                }

                original.SslFlags = binding.SslFlags;
            }

            original.BindingInformation = binding.BindingInformation;
            original.Protocol = binding.Protocol;
            original.CertificateHash = binding.CertificateHash;
            original.CertificateStoreName = binding.CertificateStoreName;
            binding.Delete();
        }

        internal static (CertificateMappingState state, string message) FixCertificateMapping(this Binding binding, X509Certificate2 certificate2, bool suppressHostNameMatching = false)
        {
            if (binding.Protocol == "http")
            {
                return (CertificateMappingState.RegistrationSucceeded, null);
            }

            if (binding.Parent.Parent.Server.SupportsSni)
            {
                if (binding.GetIsSni())
                {
                    if (!suppressHostNameMatching && !certificate2.MatchHostName(binding.Host))
                    {
                        return (CertificateMappingState.HostNameNotMatched, "SNI mode requires host name matches common name of the certificate");
                    }

                    // handle SNI
                    var sni = NativeMethods.QuerySslSniInfo(new Tuple<string, int>(binding.Host,
                        binding.EndPoint.Port));
                    return AddMapping(binding, sni, true);
                }
            }

            // handle IP based
            if (binding.EndPoint == null)
            {
                return (CertificateMappingState.IpEndPointInvalid, "This binding does not have valid IP endpoint");
            }

            var certificate = NativeMethods.QuerySslCertificateInfo(binding.EndPoint);
            return AddMapping(binding, certificate, false);
        }

        private static (CertificateMappingState state, string message) AddMapping(this Binding binding, NativeMethods.ISslCertificateInfo certificate, bool sni)
        {
            var arguments = binding.ToAddMappingArguments(sni);
            if (certificate == null)
            {
                return ManipulateCertificateMapping(arguments);
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
                    return (CertificateMappingState.CertificateHashNotMatched, "Certificate hash does not match. Please use the certificate that matches HTTPS binding");
                }

                return ManipulateCertificateMapping(arguments);
            }

            if (string.Equals(certificate.StoreName, binding.CertificateStoreName, StringComparison.OrdinalIgnoreCase))
            {
                return (CertificateMappingState.RegistrationSucceeded, null);
            }

            // TODO: can this happen?
            return (CertificateMappingState.CertificateStoreNotMatched, "Certificate store name does not match. Please use the certificate that matches HTTPS binding");
        }

        private static (CertificateMappingState state, string message) ManipulateCertificateMapping(string arguments)
        {
            try
            {
                // register mapping
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments = arguments;
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    return (CertificateMappingState.RegistrationFailed, "Register new certificate failed: access is denied");
                }

                return (CertificateMappingState.RegistrationSucceeded, null);
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                var message = NativeMethods.KnownCases(ex.NativeErrorCode);
                if (string.IsNullOrEmpty(message))
                {
                    _logger.LogWarning(ex, "Win32 error during certificate mapping. Native error code: {Code}", ex.NativeErrorCode);
                    return (CertificateMappingState.Win32ErrorOccurred, $"Register new certificate failed: unknown (native {ex.NativeErrorCode})");
                }

                return (CertificateMappingState.UacCancelled, $"Register new certificate failed: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error during certificate mapping");
                return (CertificateMappingState.GenericErrorOccurred, $"Register new certificate failed: unknown ({ex.Message})");
            }
        }

        internal static bool? Verify(string protocol, string address, string port, CertificateInfo certificate)
        {
            IPAddress ip = IPAddress.Any;
            if (address != StringExtensions.AllUnassigned && !IPAddress.TryParse(address, out ip))
            {
                return null;
            }

            if (!int.TryParse(port, out int portNumber))
            {
                return null;
            }

            if (portNumber < 1 || portNumber > 65535)
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

        public static string CleanUpMapping(this Binding binding)
        {
            if (binding.Protocol == "http")
            {
                return string.Empty;
            }

            var sni = true;
#if !IIS
            sni = binding.GetIsSni();
#endif            
            return binding.RemoveMapping(sni);
        }

        private static string RemoveMapping(this Binding binding, bool sni)
        {
            var arguments = binding.ToRemoveMappingArguments(sni);
            try
            {
                // remove sni mapping
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments = arguments;
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
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                var message = NativeMethods.KnownCases(ex.NativeErrorCode);
                if (string.IsNullOrEmpty(message))
                {
                    _logger.LogWarning(ex, "Win32 error during mapping removal. Native error code: {Code}", ex.NativeErrorCode);
                    return $"Remove SNI certificate failed: unknown (native {ex.NativeErrorCode})";
                }

                return $"Remove SNI certificate failed: {message}";
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Null reference during mapping removal. Binding: {Binding}, EndPoint null: {IsNull}",
                    binding.ToString(), binding.EndPoint == null);
                return $"Remove SNI certificate failed: unknown ({ex.Message})";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error during mapping removal");
                return $"Remove SNI certificate failed: unknown ({ex.Message})";
            }
        }

        private static string ToRemoveMappingArguments(this Binding binding, bool sni)
        {
            var hash = binding.CertificateHash == null ? string.Empty : Hex.ToHexString(binding.CertificateHash);
            // TODO: should app ID and port number to verified?
            return sni
                ? $"/c \"\"{CertificateInstallerLocator.FileName}\" /h:\"{hash}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /o:{binding.EndPoint.Port} /x:{binding.Host}"
                : $"/c \"\"{CertificateInstallerLocator.FileName}\" /h:\"{hash}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /o:{binding.EndPoint.Port}";
        }

        public static string AddReservedUrl(string url)
        {
            try
            {
                // add reserved URL
                using var process = new Process();
                var start = process.StartInfo;
                start.Verb = "runas";
                start.UseShellExecute = true;
                start.FileName = "cmd";
                start.Arguments = $"/c \"\"{CertificateInstallerLocator.FileName}\" /u:\"{url}\"";
                start.CreateNoWindow = true;
                start.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    return $"Process exited unexpectedly: {process.ExitCode}";
                }

                return string.Empty;
            }
            catch (Win32Exception ex)
            {
                // elevation is cancelled.
                var message = NativeMethods.KnownCases(ex.NativeErrorCode);
                if (string.IsNullOrEmpty(message))
                {
                    _logger.LogWarning(ex, "Win32 error adding reserved URL. Native error code: {Code}", ex.NativeErrorCode);
                    return $"failed to add reserved URL: unknown ({message})";
                }
                else
                {
                    return $"failed to add reserved URL: {message}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding reserved URL: {Url}", url);
                return $"failed to add reserved URL: unknown ({ex.Message})";
            }
        }

        public static string ToUrlPrefix(this Binding binding)
        {
            var name = binding.Host == "*" || binding.Host == string.Empty ? "*" : binding.Host;
            return $"{binding.Protocol}://{name}:{binding.EndPoint.Port}/";
        }

        public static bool MatchHostName(this X509Certificate2 certificate, string host)
        {
            var extension = certificate.Extensions[SubjectAlternativeNameOid];
            if (extension != null)
            {
                var names = extension.Format(true);
                var lines = new StringReader(names);
                string line;
                while ((line = lines.ReadLine()) != null)
                {
                    if (line.StartsWith(DnsNamePrefix) && line.Substring(DnsNamePrefix.Length).MatchHostName(host))
                    {
                        return true;
                    }
                }
            }

            return certificate.GetNameInfo(X509NameType.SimpleName, false).MatchHostName(host);
        }

        private static string ToAddMappingArguments(this Binding binding, bool sni)
        {
            return sni
                ? $"/c \"\"{CertificateInstallerLocator.FileName}\" /h:\"{Hex.ToHexString(binding.CertificateHash)}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /a:{binding.EndPoint.Address} /o:{binding.EndPoint.Port} /x:{binding.Host}"
                : $"/c \"\"{CertificateInstallerLocator.FileName}\" /h:\"{Hex.ToHexString(binding.CertificateHash)}\" /s:{binding.CertificateStoreName}\" /i:{AppIdIisExpress} /a:{binding.EndPoint.Address} /o:{binding.EndPoint.Port}";
        }
    }
}

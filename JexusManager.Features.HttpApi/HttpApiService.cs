// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;
using Org.BouncyCastle.Utilities.Encoders;

namespace JexusManager.Features.HttpApi
{
    internal sealed class HttpApiService : ModuleService
    {
        [ModuleServiceMethod]
        public ReservedUrlsItem[] GetReservedUrls()
        {
            var result = new List<ReservedUrlsItem>();
            var httpNamespaceAcls = Microsoft.Web.Administration.NativeMethods.QueryHttpNamespaceAcls();
            foreach (var mapping in httpNamespaceAcls)
            {
                result.Add(new ReservedUrlsItem { UrlPrefix = mapping.UrlPrefix, SecurityDescriptor = mapping.SecurityDescriptor });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public string AddReservedUrl(string urlPrefix)
        {
            return BindingUtility.AddReservedUrl(urlPrefix);
        }

        [ModuleServiceMethod]
        public bool DeleteReservedUrl(string urlPrefix, string securityDescriptor)
        {
            return RunElevated($"/u:\"{urlPrefix}\" /d:\"{securityDescriptor}\"");
        }

        [ModuleServiceMethod]
        public IpMappingItem[] GetIpMappings()
        {
            var result = new List<IpMappingItem>();
            var ipMappings = Microsoft.Web.Administration.NativeMethods.QuerySslCertificateInfo();
            foreach (var mapping in ipMappings)
            {
                result.Add(new IpMappingItem
                {
                    Address = mapping.IpPort.Address.ToString(),
                    Port = mapping.IpPort.Port.ToString(),
                    AppId = mapping.AppId.ToString(),
                    Hash = Hex.ToHexString(mapping.Hash),
                    Store = mapping.StoreName
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public bool DeleteIpMapping(string address, string port)
        {
            return RunElevated($"/a:\"{address}\" /o:{port}");
        }

        [ModuleServiceMethod]
        public SniMappingItem[] GetSniMappings()
        {
            var result = new List<SniMappingItem>();
            var sniMappings = Microsoft.Web.Administration.NativeMethods.QuerySslSniInfo();
            foreach (var mapping in sniMappings)
            {
                result.Add(new SniMappingItem
                {
                    Host = mapping.Host,
                    Port = mapping.Port.ToString(),
                    AppId = mapping.AppId.ToString(),
                    Hash = Hex.ToHexString(mapping.Hash),
                    Store = mapping.StoreName
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public bool DeleteSniMapping(string host, string port)
        {
            return RunElevated($"/h:\"{host}\" /o:{port}");
        }

        [ModuleServiceMethod]
        public X509Certificate2 GetCertificate(string thumbprint, string store)
        {
            using X509Store personal = new X509Store(store, StoreLocation.LocalMachine);
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

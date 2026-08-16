// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;
using Microsoft.Web.Management.Client;

namespace JexusManager.Features.HttpApi
{
    internal sealed class HttpApiModuleProxy : ModuleServiceProxy
    {
        internal ReservedUrlsItem[] GetReservedUrls()
        {
            return (ReservedUrlsItem[])Invoke(nameof(GetReservedUrls));
        }

        internal string AddReservedUrl(string urlPrefix)
        {
            return (string)Invoke(nameof(AddReservedUrl), urlPrefix);
        }

        internal bool DeleteReservedUrl(string urlPrefix, string securityDescriptor)
        {
            return (bool)Invoke(nameof(DeleteReservedUrl), urlPrefix, securityDescriptor);
        }

        internal IpMappingItem[] GetIpMappings()
        {
            return (IpMappingItem[])Invoke(nameof(GetIpMappings));
        }

        internal bool DeleteIpMapping(string address, string port)
        {
            return (bool)Invoke(nameof(DeleteIpMapping), address, port);
        }

        internal SniMappingItem[] GetSniMappings()
        {
            return (SniMappingItem[])Invoke(nameof(GetSniMappings));
        }

        internal bool DeleteSniMapping(string host, string port)
        {
            return (bool)Invoke(nameof(DeleteSniMapping), host, port);
        }

        internal X509Certificate2 GetCertificate(string thumbprint, string store)
        {
            return (X509Certificate2)Invoke(nameof(GetCertificate), thumbprint, store);
        }
    }
}

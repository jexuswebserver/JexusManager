// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security.Cryptography.X509Certificates;

namespace JexusManager
{
    internal sealed class CertificateInfo
    {
        public X509Certificate2 Certificate { get; }
        public string Store { get; }

        public CertificateInfo(X509Certificate2 certificate, string store)
        {
            Certificate = certificate;
            Store = store;
        }

        public override string ToString()
        {
            var friendlyName = Certificate.FriendlyName;
            var dnsName = Certificate.GetNameInfo(X509NameType.SimpleName, false);
            if (!string.IsNullOrWhiteSpace(friendlyName))
            {
                return friendlyName;
            }

            return string.IsNullOrWhiteSpace(dnsName) ? "<Unknown>" : dnsName;
        }
    }
}

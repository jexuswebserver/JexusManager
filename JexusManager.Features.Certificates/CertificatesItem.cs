// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    internal class CertificatesItem : IEquatable<CertificatesItem>
    {
        public CertificatesItem(X509Certificate2 certificate, string store, CertificatesFeature feature)
        {
            Certificate = certificate;
            Store = store;
            Feature = feature;
        }

        public X509Certificate2 Certificate { get; set; }
        public string Store { get; set; }
        public CertificatesFeature Feature { get; private set; }

        public bool Equals(CertificatesItem other)
        {
            return other != null && other.Certificate.GetCertHashString() == Certificate.GetCertHashString();
        }
    }
}
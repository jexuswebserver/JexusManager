// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.Web.Administration;

    [Serializable]
    public class CertificatesItem : IItem<CertificatesItem>
    {
        public CertificatesItem()
        {
        }

        public CertificatesItem(X509Certificate2 certificate, string store, CertificatesFeature feature)
        {
            Item = certificate;
            Store = store;
            Feature = feature;
        }

        public X509Certificate2 Item { get; set; }
        public string Store { get; set; }
        public CertificatesFeature Feature { get; private set; }
        public string Flag { get; set; }

        public string Thumbprint
        {
            get { return Item?.GetCertHashString(); }
        }

        public bool Equals(CertificatesItem other)
        {
            return other != null && Equals(other.Item);
        }

        public bool Equals(X509Certificate2 other)
        {
            return other != null && other.GetCertHashString() == Item.GetCertHashString();
        }

        public bool Match(CertificatesItem other)
        {
            return Equals(other);
        }
    }
}

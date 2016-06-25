// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpApi
{
    using Microsoft.Web.Administration;

    internal class SniMappingItem : IItem<SniMappingItem>
    {
        public SniMappingItem(string host, string port, string appId, string hash, string store, SniMappingFeature feature)
        {
            this.Host = host;
            this.Port = port;
            this.AppId = appId;
            this.Hash = hash;
            this.Store = store;
            this.Feature = feature;
        }

        public string Hash { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string AppId { get; set; }
        public string Store { get; set; }
        public SniMappingFeature Feature { get; private set; }

        public string Flag { get; set; }
        public ConfigurationElement Element { get; set; }

        public void Apply()
        { }

        public bool Equals(SniMappingItem other)
        {
            return Match(other);
        }

        public bool Match(SniMappingItem other)
        {
            return other != null && other.Hash == this.Hash && other.Store == this.Store && other.Host == this.Host
                   && other.Port == this.Port && other.AppId == this.AppId;
        }
    }
}

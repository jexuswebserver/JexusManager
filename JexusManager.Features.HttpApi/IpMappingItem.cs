// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.HttpApi
{
    using Microsoft.Web.Administration;

    [Serializable]
    internal class IpMappingItem : IItem<IpMappingItem>
    {
        public IpMappingItem()
        {
        }

        public string Hash { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
        public string AppId { get; set; }
        public string Store { get; set; }

        public string Flag { get; set; }

        public void Apply()
        { }

        public bool Equals(IpMappingItem other)
        {
            return Match(other);
        }

        public bool Match(IpMappingItem other)
        {
            return other != null && other.Hash == Hash && other.Store == Store && other.Address == Address
                   && other.Port == Port && other.AppId == AppId;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class IpSecurityItem : IItem<IpSecurityItem>
    {
        public IpSecurityItem()
        {
            Mask = "255.255.255.255";
            Flag = "Local";
        }

        public ConfigurationElement Element { get; set; }
        public string Address { get; set; }
        public string Mask { get; set; }
        public bool Allowed { get; set; }
        public string Flag { get; set; }

        public void Apply()
        {
        }

        public bool Equals(IpSecurityItem other)
        {
            return other != null && other.Address == Address && other.Mask == Mask && other.Allowed == Allowed;
        }

        public bool Match(IpSecurityItem other)
        {
            return other != null && other.Address == Address && other.Mask == Mask;
        }
    }
}

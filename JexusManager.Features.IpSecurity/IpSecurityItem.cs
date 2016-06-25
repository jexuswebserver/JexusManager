// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using Microsoft.Web.Administration;

    internal class IpSecurityItem : IItem<IpSecurityItem>
    {
        public IpSecurityItem(ConfigurationElement element)
        {
            Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                Mask = "255.255.255.255";
                return;
            }

            Address = (string)element["ipAddress"];
            Mask = (string)element["subnetMask"];
            this.Allowed = (bool)element["allowed"];
        }

        public ConfigurationElement Element { get; set; }
        public string Address { get; set; }
        public string Mask { get; set; }
        public bool Allowed { get; set; }
        public string Flag { get; set; }

        public void Apply()
        {
            Element["ipAddress"] = Address;
            Element["subnetMask"] = Mask;
            Element["allowed"] = this.Allowed;
        }

        public bool Equals(IpSecurityItem other)
        {
            return other != null && other.Address == Address && other.Mask == Mask && other.Allowed == this.Allowed;
        }

        public bool Match(IpSecurityItem other)
        {
            return other != null && other.Address == Address && other.Mask == Mask;
        }
    }
}

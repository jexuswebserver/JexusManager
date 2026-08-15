// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.IpSecurity
{
    internal sealed class IpSecurityService : ModuleService
    {
        private const string SectionPath = "system.webServer/security/ipSecurity";
        private const string DynamicSectionPath = "system.webServer/security/dynamicIpSecurity";

        [ModuleServiceMethod]
        public IpSecurityItem[] GetItems()
        {
            var result = new List<IpSecurityItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection())
            {
                result.Add(new IpSecurityItem
                {
                    Address = (string)element["ipAddress"],
                    Mask = (string)element["subnetMask"],
                    Allowed = (bool)element["allowed"],
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(IpSecurityItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var element = collection.CreateElement();
            element["ipAddress"] = item.Address;
            element["subnetMask"] = item.Mask;
            element["allowed"] = item.Allowed;
            collection.Add(element);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(IpSecurityItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("IP restriction was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Revert()
        {
            GetSection().GetCollection().Revert();
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public IpSecuritySettings GetSettings()
        {
            var section = GetSection();
            return new IpSecuritySettings
            {
                EnableReverseDns = (bool)section["enableReverseDns"],
                AllowUnlisted = (bool)section["allowUnlisted"],
                EnableProxyMode = section.Schema.AttributeSchemas["enabled"] != null ? (bool?)section["enableProxyMode"] : null,
                DenyAction = section.Schema.AttributeSchemas["denyAction"] != null ? (long?)section["denyAction"] : null
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(IpSecuritySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection();
            section["enableReverseDns"] = settings.EnableReverseDns;
            section["allowUnlisted"] = settings.AllowUnlisted;
            if (section.Schema.AttributeSchemas["enabled"] != null)
            {
                section["enableProxyMode"] = settings.EnableProxyMode ?? false;
            }

            if (section.Schema.AttributeSchemas["denyAction"] != null)
            {
                section["denyAction"] = settings.DenyAction ?? 0L;
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public bool IsDynamicIpSecurityAvailable()
        {
            return GetDynamicSection() != null;
        }

        [ModuleServiceMethod]
        public DynamicIpSecuritySettings GetDynamicSettings()
        {
            var section = GetDynamicSection();
            if (section == null)
            {
                return null;
            }

            var concurrent = section.ChildElements["denyByConcurrentRequests"];
            var rate = section.ChildElements["denyByRequestRate"];
            return new DynamicIpSecuritySettings
            {
                EnableConcurrentDenial = (bool)concurrent["enabled"],
                MaxConcurrentRequests = (uint)concurrent["maxConcurrentRequests"],
                EnableRateDenial = (bool)rate["enabled"],
                MaxRequests = (uint)rate["maxRequests"],
                RequestIntervalInMilliseconds = (uint)rate["requestIntervalInMilliseconds"],
                EnableLoggingOnlyMode = (bool)section["enableLoggingOnlyMode"]
            };
        }

        [ModuleServiceMethod]
        public void ApplyDynamicSettings(DynamicIpSecuritySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetDynamicSection();
            if (section == null)
            {
                throw new InvalidOperationException("Dynamic IP restriction section is not available.");
            }

            var concurrent = section.ChildElements["denyByConcurrentRequests"];
            concurrent["enabled"] = settings.EnableConcurrentDenial;
            concurrent["maxConcurrentRequests"] = settings.MaxConcurrentRequests;
            var rate = section.ChildElements["denyByRequestRate"];
            rate["enabled"] = settings.EnableRateDenial;
            rate["maxRequests"] = settings.MaxRequests;
            rate["requestIntervalInMilliseconds"] = settings.RequestIntervalInMilliseconds;
            section["enableLoggingOnlyMode"] = settings.EnableLoggingOnlyMode;
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }

        private ConfigurationSection GetDynamicSection()
        {
            return ManagementUnit.Configuration.GetSection(DynamicSectionPath);
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, IpSecurityItem item)
        {
            foreach (ConfigurationElement element in collection)
            {
                if ((string)element["ipAddress"] == item.Address && (string)element["subnetMask"] == item.Mask)
                {
                    return element;
                }
            }

            return null;
        }
    }
}

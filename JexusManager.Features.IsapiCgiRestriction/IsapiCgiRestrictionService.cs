// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.IsapiCgiRestriction
{
    internal sealed class IsapiCgiRestrictionService : ModuleService
    {
        private const string SectionPath = "system.webServer/security/isapiCgiRestriction";

        [ModuleServiceMethod]
        public IsapiCgiRestrictionSettings GetSettings()
        {
            var s = Section();
            return new IsapiCgiRestrictionSettings
            {
                NotListedCgisAllowed = (bool)s["notListedCgisAllowed"],
                NotListedIsapisAllowed = (bool)s["notListedIsapisAllowed"]
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(IsapiCgiRestrictionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var s = Section();
            s["notListedCgisAllowed"] = settings.NotListedCgisAllowed;
            s["notListedIsapisAllowed"] = settings.NotListedIsapisAllowed;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public IsapiCgiRestrictionItem[] GetItems()
        {
            var list = new List<IsapiCgiRestrictionItem>();
            foreach (ConfigurationElement e in Collection())
            {
                list.Add(new IsapiCgiRestrictionItem
                {
                    OriginalKey = (string)e["path"],
                    Path = (string)e["path"],
                    Description = (string)e["description"],
                    Allowed = (bool)e["allowed"],
                    Flag = e.IsLocallyStored ? "Local" : "Inhertied"
                });
            }

            return list.ToArray();
        }

        [ModuleServiceMethod]
        public void SetAllowed(IsapiCgiRestrictionItem item, bool allowed)
        {
            var e = Find(item) ?? throw new InvalidOperationException("Restriction was not found.");
            e["allowed"] = allowed;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Add(IsapiCgiRestrictionItem item)
        {
            AddItem(item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(IsapiCgiRestrictionItem oldItem, IsapiCgiRestrictionItem item)
        {
            var e = Find(oldItem) ?? throw new InvalidOperationException("Restriction was not found.");
            if (e.IsLocallyStored)
            {
                ApplyItem(e, item);
            }
            else
            {
                Collection().Remove(e);
                AddItem(item);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(IsapiCgiRestrictionItem item)
        {
            var e = Find(item) ?? throw new InvalidOperationException("Restriction was not found.");
            Collection().Remove(e);
            ManagementUnit.Update();
        }

        private ConfigurationSection Section()
        {
            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                return ManagementUnit.Configuration.GetSection(SectionPath);
            }

            var section = ManagementUnit.Configuration.GetSection(SectionPath);
            if (!section.IsLocallyStored)
            {
                var locationPath = ManagementUnit.ConfigurationPath.GetEffectiveConfigurationPath(ManagementScope.Site);
                section = ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection(SectionPath, locationPath);
            }

            return section;
        }

        private ConfigurationElementCollection Collection()
        {
            return Section().GetCollection();
        }

        private ConfigurationElement Find(IsapiCgiRestrictionItem item)
        {
            var key = string.IsNullOrEmpty(item.OriginalKey) ? item.Path : item.OriginalKey;
            foreach (ConfigurationElement e in Collection())
            {
                if ((string)e["path"] == key)
                {
                    return e;
                }
            }

            return null;
        }

        private void AddItem(IsapiCgiRestrictionItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Path))
            {
                throw new ArgumentException("A restriction path is required.");
            }

            var e = Collection().CreateElement();
            ApplyItem(e, item);
            Collection().Add(e);
        }

        private static void ApplyItem(ConfigurationElement e, IsapiCgiRestrictionItem item)
        {
            e["path"] = item.Path;
            e["description"] = item.Description;
            e["allowed"] = item.Allowed;
        }
    }
}

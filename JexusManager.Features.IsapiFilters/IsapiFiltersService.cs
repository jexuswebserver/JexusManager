// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.IsapiFilters
{
    internal sealed class IsapiFiltersService : ModuleService
    {
        private const string SectionPath = "system.webServer/isapiFilters";

        [ModuleServiceMethod]
        public IsapiFiltersItem[] GetItems()
        {
            var result = new List<IsapiFiltersItem>();
            foreach (ConfigurationElement e in GetSection().GetCollection())
            {
                result.Add(new IsapiFiltersItem
                {
                    OriginalKey = (string)e["name"],
                    Name = (string)e["name"],
                    Path = (string)e["path"],
                    PreConditions = CreatePreConditions((string)e["preCondition"]),
                    EnableCache = (bool)e["enableCache"],
                    Flag = e.IsLocallyStored ? "Local" : "Inherited"
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(IsapiFiltersItem item)
        {
            var c = GetSection().GetCollection();
            var e = c.CreateElement();
            ApplyFilter(e, item);
            c.Add(e);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(IsapiFiltersItem oldItem, IsapiFiltersItem item)
        {
            var c = GetSection().GetCollection();
            var e = Find(c, oldItem) ?? throw new InvalidOperationException("ISAPI filter was not found.");
            if (e.IsLocallyStored)
            {
                ApplyFilter(e, item);
            }
            else
            {
                c.Remove(e);
                var created = c.CreateElement();
                ApplyFilter(created, item);
                c.Add(created);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(IsapiFiltersItem item)
        {
            var e = Find(GetSection().GetCollection(), item) ?? throw new InvalidOperationException("ISAPI filter was not found.");
            GetSection().GetCollection().Remove(e);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void MoveUp(IsapiFiltersItem item)
        {
            Move(item, -1);
        }

        [ModuleServiceMethod]
        public void MoveDown(IsapiFiltersItem item)
        {
            Move(item, 1);
        }

        [ModuleServiceMethod]
        public void Revert()
        {
            GetSection().GetCollection().Revert();
            ManagementUnit.Update();
        }

        private void Move(IsapiFiltersItem item, int delta)
        {
            var c = GetSection().GetCollection();
            var e = Find(c, item) ?? throw new InvalidOperationException("ISAPI filter was not found.");
            var index = c.IndexOf(e);
            var target = index + delta;
            if (target < 0 || target >= c.Count)
            {
                return;
            }

            c.RemoveAt(index);
            c.AddAt(target, e);
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
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

        private static List<string> CreatePreConditions(string content)
        {
            return string.IsNullOrEmpty(content) ? new List<string>() : new List<string>(content.Split(','));
        }

        private static ConfigurationElement Find(ConfigurationElementCollection c, IsapiFiltersItem item)
        {
            var key = string.IsNullOrEmpty(item.OriginalKey) ? item.Name : item.OriginalKey;
            foreach (ConfigurationElement e in c)
            {
                if ((string)e["name"] == key)
                {
                    return e;
                }
            }

            return null;
        }

        private static void ApplyFilter(ConfigurationElement e, IsapiFiltersItem item)
        {
            e["name"] = item.Name;
            e["path"] = item.Path;
            e["preCondition"] = string.Join(",", item.PreConditions);
            e["enableCache"] = item.EnableCache;
        }
    }
}

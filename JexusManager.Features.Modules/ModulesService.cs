// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Modules
{
    internal sealed class ModulesService : ModuleService
    {
        private const string SectionPath = "system.webServer/modules";
        private const string GlobalSectionPath = "system.webServer/globalModules";

        [ModuleServiceMethod]
        public GlobalModule[] GetGlobalModules()
        {
            var result = new List<GlobalModule>();
            foreach (ConfigurationElement element in GetGlobalSection().GetCollection())
            {
                result.Add(new GlobalModule
                {
                    Name = (string)element["name"],
                    Image = (string)element["image"],
                    PreConditions = CreatePreConditions((string)element["preCondition"])
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public ModulesItem[] GetItems()
        {
            var result = new List<ModulesItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection())
            {
                result.Add(CreateItem(element));
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(ModulesItem item)
        {
            AddModule(GetSection().GetCollection(), item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(ModulesItem original, ModulesItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, original);
            if (existing == null)
            {
                throw new InvalidOperationException("Module was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyModule(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyModule(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(ModulesItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Module was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void AddGlobal(GlobalModule item)
        {
            AddGlobalModule(GetGlobalSection().GetCollection(), item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void UpdateGlobal(GlobalModule original, GlobalModule item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetGlobalSection().GetCollection();
            var existing = FindGlobal(collection, original);
            if (existing == null)
            {
                throw new InvalidOperationException("Native module was not found.");
            }

            collection.Remove(existing);
            AddGlobalModule(collection, item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void RemoveGlobal(GlobalModule item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetGlobalSection().GetCollection();
            var existing = FindGlobal(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Native module was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void MoveUp(ModulesItem item)
        {
            Move(item, -1);
        }

        [ModuleServiceMethod]
        public void MoveDown(ModulesItem item)
        {
            Move(item, 1);
        }

        [ModuleServiceMethod]
        public void Revert()
        {
            GetSection().GetCollection().Revert();
            ManagementUnit.Update();
        }

        private void Move(ModulesItem item, int delta)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Module was not found.");
            }

            var index = collection.IndexOf(existing);
            var target = index + delta;
            if (target < 0 || target >= collection.Count)
            {
                return;
            }

            collection.RemoveAt(index);
            collection.AddAt(target, existing);
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
        {
            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                return ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection(SectionPath, string.Empty);
            }

            var section = ManagementUnit.Configuration.GetSection(SectionPath);
            if (!section.IsLocallyStored)
            {
                var locationPath = ManagementUnit.ConfigurationPath.GetEffectiveConfigurationPath(ManagementScope.Site);
                section = ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection(SectionPath, locationPath);
            }

            return section;
        }

        private ConfigurationSection GetGlobalSection()
        {
            return ManagementUnit.Configuration.GetSection(GlobalSectionPath);
        }

        private static List<string> CreatePreConditions(string content)
        {
            return string.IsNullOrEmpty(content) ? new List<string>() : new List<string>(content.Split(','));
        }

        private static ModulesItem CreateItem(ConfigurationElement element)
        {
            var type = (string)element["type"];
            var item = new ModulesItem
            {
                OriginalKey = (string)element["name"],
                Name = (string)element["name"],
                Type = type,
                PreConditions = CreatePreConditions((string)element["preCondition"]),
                IsLocked = element.GetIsLocked(),
                Flag = element.IsLocallyStored ? "Local" : "Inhertied"
            };
            if (!string.IsNullOrWhiteSpace(type))
            {
                item.IsManaged = true;
            }

            return item;
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, ModulesItem item)
        {
            var key = string.IsNullOrEmpty(item.OriginalKey) ? item.Name : item.OriginalKey;
            foreach (ConfigurationElement element in collection)
            {
                if ((string)element["name"] == key)
                {
                    return element;
                }
            }

            return null;
        }

        private static ConfigurationElement FindGlobal(ConfigurationElementCollection collection, GlobalModule item)
        {
            foreach (ConfigurationElement element in collection)
            {
                if ((string)element["name"] == item.Name)
                {
                    return element;
                }
            }

            return null;
        }

        private static void AddModule(ConfigurationElementCollection collection, ModulesItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("A module name is required.");
            }

            var element = collection.CreateElement();
            ApplyModule(element, item);
            collection.Add(element);
        }

        private static void ApplyModule(ConfigurationElement element, ModulesItem item)
        {
            element["name"] = item.Name;
            if (item.IsManaged)
            {
                element["type"] = item.Type;
            }

            element["preCondition"] = item.PreConditions.Combine(",");
        }

        private static void AddGlobalModule(ConfigurationElementCollection collection, GlobalModule item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("A native module name is required.");
            }

            var element = collection.CreateElement();
            element["name"] = item.Name;
            element["image"] = item.Image;
            element["preCondition"] = item.PreConditions.Combine(",");
            collection.Add(element);
        }
    }
}

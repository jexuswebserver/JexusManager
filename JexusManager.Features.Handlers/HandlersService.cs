// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.Handlers
{
    internal sealed class HandlersService : ModuleService
    {
        private const string SectionPath = "system.webServer/handlers";

        [ModuleServiceMethod]
        public HandlersItem[] GetItems()
        {
            var result = new List<HandlersItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection())
            {
                result.Add(new HandlersItem
                {
                    OriginalKey = (string)element["name"],
                    Name = (string)element["name"],
                    Path = (string)element["path"],
                    ResourceType = (long)element["resourceType"],
                    Verb = (string)element["verb"],
                    RequireAccess = (long)element["requireAccess"],
                    Modules = (string)element["modules"],
                    ScriptProcessor = (string)element["scriptProcessor"],
                    Type = (string)element["type"],
                    PreConditions = CreatePreConditions((string)element["preCondition"]),
                    ResponseBufferLimit = (uint)element["responseBufferLimit"],
                    AllowPathInfo = (bool)element["allowPathInfo"],
                    Flag = element.IsLocallyStored ? "Local" : "Inhertied"
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(HandlersItem item)
        {
            AddHandler(GetSection().GetCollection(), item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(HandlersItem original, HandlersItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, original);
            if (existing == null)
            {
                throw new InvalidOperationException("Handler was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyHandler(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyHandler(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(HandlersItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Handler was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Rename(HandlersItem original, string name)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A handler name is required.", nameof(name));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, original);
            if (existing == null)
            {
                throw new InvalidOperationException("Handler was not found.");
            }

            existing["name"] = name;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void MoveUp(HandlersItem item)
        {
            Move(item, -1);
        }

        [ModuleServiceMethod]
        public void MoveDown(HandlersItem item)
        {
            Move(item, 1);
        }

        [ModuleServiceMethod]
        public void Revert()
        {
            GetSection().GetCollection().Revert();
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public HandlersSettings GetSettings()
        {
            return new HandlersSettings
            {
                AccessPolicy = (long)GetSection()["accessPolicy"]
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(HandlersSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            GetSection()["accessPolicy"] = settings.AccessPolicy;
            ManagementUnit.Update();
        }

        private void Move(HandlersItem item, int delta)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Handler was not found.");
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

        private static List<string> CreatePreConditions(string content)
        {
            return string.IsNullOrEmpty(content) ? new List<string>() : new List<string>(content.Split(','));
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, HandlersItem item)
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

        private static void AddHandler(ConfigurationElementCollection collection, HandlersItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("A handler name is required.");
            }

            var element = collection.CreateElement();
            ApplyHandler(element, item);
            collection.Add(element);
        }

        private static void ApplyHandler(ConfigurationElement element, HandlersItem item)
        {
            element["name"] = item.Name;
            element["path"] = item.Path;
            element["resourceType"] = item.ResourceType;
            element["verb"] = item.Verb;
            element["requireAccess"] = item.RequireAccess;
            element["modules"] = item.Modules;
            element["scriptProcessor"] = item.ScriptProcessor;
            element["type"] = item.Type;
            element["preCondition"] = item.PreConditions.Combine(",");
            element["responseBufferLimit"] = item.ResponseBufferLimit;
            element["allowPathInfo"] = item.AllowPathInfo;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.DefaultDocument
{
    internal sealed class DefaultDocumentService : ModuleService
    {
        private const string SectionPath = "system.webServer/defaultDocument";

        [ModuleServiceMethod]
        public DefaultDocumentSnapshot GetSettings()
        {
            var section = GetSection();
            var entries = new List<DefaultDocumentEntry>();
            foreach (ConfigurationElement element in GetCollection(section))
            {
                entries.Add(new DefaultDocumentEntry
                {
                    Name = (string)element["value"],
                    IsLocal = element.IsLocallyStored,
                    IsLocked = element.LockAttributes.Count != 0
                });
            }

            return new DefaultDocumentSnapshot
            {
                Enabled = (bool)section["enabled"],
                CanRevert = section.CanRevert(),
                Entries = entries.ToArray()
            };
        }

        [ModuleServiceMethod]
        public void SetEnabled(bool enabled)
        {
            GetSection()["enabled"] = enabled;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Insert(string name, int index)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("A default document name is required.", nameof(name));
            }

            var collection = GetCollection(GetSection());
            var element = collection.CreateElement();
            element["value"] = name;
            collection.AddAt(Math.Max(0, Math.Min(index, collection.Count)), element);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(string name)
        {
            var collection = GetCollection(GetSection());
            var element = Find(collection, name);
            if (element == null)
            {
                throw new InvalidOperationException($"Default document '{name}' was not found.");
            }

            collection.Remove(element);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Move(string name, int offset)
        {
            if (offset != -1 && offset != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            var collection = GetCollection(GetSection());
            var element = Find(collection, name);
            if (element == null)
            {
                throw new InvalidOperationException($"Default document '{name}' was not found.");
            }

            var index = collection.IndexOf(element);
            var target = index + offset;
            if (target < 0 || target >= collection.Count)
            {
                throw new InvalidOperationException("The default document cannot be moved in that direction.");
            }

            collection.Remove(element);
            collection.AddAt(target, element);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Revert()
        {
            var collection = GetCollection(GetSection());
            collection.Revert();
            ManagementUnit.Update();
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }

        private static ConfigurationElementCollection GetCollection(ConfigurationSection section)
        {
            return section.GetCollection("files");
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, string name)
        {
            foreach (ConfigurationElement element in collection)
            {
                if (string.Equals((string)element["value"], name, StringComparison.OrdinalIgnoreCase))
                {
                    return element;
                }
            }

            return null;
        }
    }
}

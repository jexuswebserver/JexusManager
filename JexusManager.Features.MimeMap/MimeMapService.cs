// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.MimeMap
{
    internal sealed class MimeMapService : ModuleService
    {
        private const string SectionPath = "system.webServer/staticContent";

        [ModuleServiceMethod]
        public MimeMapItem[] GetItems()
        {
            var result = new List<MimeMapItem>();
            foreach (ConfigurationElement e in Collection())
            {
                result.Add(new MimeMapItem
                {
                    OriginalKey = (string)e["fileExtension"],
                    FileExtension = (string)e["fileExtension"],
                    MimeType = (string)e["mimeType"],
                    Flag = e.IsLocallyStored ? "Local" : "Inhertied"
                });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(MimeMapItem item)
        {
            AddItem(Collection(), item);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(MimeMapItem oldItem, MimeMapItem item)
        {
            var c = Collection();
            var e = Find(c, oldItem) ?? throw new InvalidOperationException("MIME map was not found.");
            if (e.IsLocallyStored)
            {
                ApplyItem(e, item);
            }
            else
            {
                c.Remove(e);
                AddItem(c, item);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(MimeMapItem item)
        {
            var e = Find(Collection(), item) ?? throw new InvalidOperationException("MIME map was not found.");
            Collection().Remove(e);
            ManagementUnit.Update();
        }

        private ConfigurationElementCollection Collection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath).GetCollection();
        }

        private static ConfigurationElement Find(ConfigurationElementCollection c, MimeMapItem i)
        {
            var key = string.IsNullOrEmpty(i.OriginalKey) ? i.FileExtension : i.OriginalKey;
            foreach (ConfigurationElement e in c)
            {
                if ((string)e["fileExtension"] == key)
                {
                    return e;
                }
            }

            return null;
        }

        private static void AddItem(ConfigurationElementCollection c, MimeMapItem i)
        {
            if (i == null || string.IsNullOrWhiteSpace(i.FileExtension))
            {
                throw new ArgumentException("A file extension is required.");
            }

            var e = c.CreateElement();
            ApplyItem(e, i);
            c.Add(e);
        }

        private static void ApplyItem(ConfigurationElement e, MimeMapItem i)
        {
            e["fileExtension"] = i.FileExtension;
            e["mimeType"] = i.MimeType;
        }
    }
}

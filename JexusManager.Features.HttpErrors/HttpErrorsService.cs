// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.HttpErrors
{
    internal sealed class HttpErrorsService : ModuleService
    {
        private const string SectionPath = "system.webServer/httpErrors";

        [ModuleServiceMethod]
        public HttpErrorsSettings GetSettings()
        {
            var section = GetSection();
            return new HttpErrorsSettings
            {
                ErrorMode = (long)section["errorMode"],
                DefaultResponseMode = (long)section["defaultResponseMode"],
                DefaultPath = (string)section["defaultPath"]
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(HttpErrorsSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection();
            section["errorMode"] = settings.ErrorMode;
            section["defaultResponseMode"] = settings.DefaultResponseMode;
            section["defaultPath"] = settings.DefaultPath;
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public HttpErrorsItem[] GetItems()
        {
            var result = new List<HttpErrorsItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection())
            {
                var item = new HttpErrorsItem
                {
                    OriginalKey = CreateKey(element),
                    Status = (uint)element["statusCode"],
                    Substatus = (int)element["subStatusCode"],
                    Path = (string)element["path"],
                    Prefix = (string)element["prefixLanguageFilePath"],
                    Response = element.Schema.AttributeSchemas["responseMode"].Format(element["responseMode"]),
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(HttpErrorsItem item)
        {
            var collection = GetSection().GetCollection();
            var element = collection.CreateElement();
            ApplyItem(element, item);
            collection.Add(element);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(HttpErrorsItem original, HttpErrorsItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, original);
            if (existing == null)
            {
                throw new InvalidOperationException("Custom error page was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyItem(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyItem(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(HttpErrorsItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Custom error page was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void MoveUp(HttpErrorsItem item)
        {
            Move(item, -1);
        }

        [ModuleServiceMethod]
        public void MoveDown(HttpErrorsItem item)
        {
            Move(item, 1);
        }

        [ModuleServiceMethod]
        public void Revert()
        {
            GetSection().GetCollection().Revert();
            ManagementUnit.Update();
        }

        private void Move(HttpErrorsItem item, int delta)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Custom error page was not found.");
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
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }

        private static string CreateKey(ConfigurationElement element)
        {
            return $"{element["statusCode"]}.{element["subStatusCode"]}";
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, HttpErrorsItem item)
        {
            var key = string.IsNullOrEmpty(item.OriginalKey) ? $"{item.Status}.{item.Substatus}" : item.OriginalKey;
            foreach (ConfigurationElement element in collection)
            {
                if ($"{element["statusCode"]}.{element["subStatusCode"]}" == key)
                {
                    return element;
                }
            }

            return null;
        }

        private static void ApplyItem(ConfigurationElement element, HttpErrorsItem item)
        {
            element["statusCode"] = item.Status;
            element["subStatusCode"] = item.Substatus;
            element["prefixLanguageFilePath"] = item.Prefix;
            element["path"] = item.Path;
            element["responseMode"] = item.Response;
        }
    }
}

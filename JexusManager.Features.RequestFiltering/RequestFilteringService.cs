// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.RequestFiltering
{
    internal sealed class RequestFilteringService : ModuleService
    {
        private const string SectionPath = "system.webServer/security/requestFiltering";

        [ModuleServiceMethod]
        public VerbsItem[] GetVerbs()
        {
            return GetItems(GetSection().GetCollection("verbs"), element => new VerbsItem
            {
                Verb = (string)element["verb"],
                Allowed = (bool)element["allowed"],
                Flag = element.IsLocallyStored ? "Local" : "Inherited"
            });
        }

        [ModuleServiceMethod]
        public void AddVerb(VerbsItem item)
        {
            AddItem(GetSection().GetCollection("verbs"), item.Verb, element =>
            {
                element["verb"] = item.Verb;
                element["allowed"] = item.Allowed;
            });
        }

        [ModuleServiceMethod]
        public void RemoveVerb(VerbsItem item)
        {
            RemoveItem(GetSection().GetCollection("verbs"), item.Verb, "verb");
        }

        [ModuleServiceMethod]
        public UrlsItem[] GetUrls()
        {
            var result = new List<UrlsItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection("alwaysAllowedUrls"))
            {
                result.Add(new UrlsItem(true) { Url = (string)element["url"], Flag = element.IsLocallyStored ? "Local" : "Inherited" });
            }

            foreach (ConfigurationElement element in GetSection().GetCollection("denyUrlSequences"))
            {
                result.Add(new UrlsItem(false) { Url = (string)element["sequence"], Flag = element.IsLocallyStored ? "Local" : "Inherited" });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddUrl(UrlsItem item)
        {
            AddItem(GetUrlCollection(item), item.Url, element =>
            {
                if (item.Allowed)
                {
                    element["url"] = item.Url;
                }
                else
                {
                    element["sequence"] = item.Url;
                }
            });
        }

        [ModuleServiceMethod]
        public void RemoveUrl(UrlsItem item)
        {
            RemoveItem(GetUrlCollection(item), item.Url, item.Allowed ? "url" : "sequence");
        }

        [ModuleServiceMethod]
        public QueryStringsItem[] GetQueryStrings()
        {
            var result = new List<QueryStringsItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection("alwaysAllowedQueryStrings"))
            {
                result.Add(new QueryStringsItem(true) { QueryString = (string)element["queryString"], Flag = element.IsLocallyStored ? "Local" : "Inherited" });
            }

            foreach (ConfigurationElement element in GetSection().GetCollection("denyQueryStringSequences"))
            {
                result.Add(new QueryStringsItem(false) { QueryString = (string)element["sequence"], Flag = element.IsLocallyStored ? "Local" : "Inherited" });
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddQueryString(QueryStringsItem item)
        {
            AddItem(GetQueryStringCollection(item), item.QueryString, element =>
            {
                if (item.Allowed)
                {
                    element["queryString"] = item.QueryString;
                }
                else
                {
                    element["sequence"] = item.QueryString;
                }
            });
        }

        [ModuleServiceMethod]
        public void RemoveQueryString(QueryStringsItem item)
        {
            RemoveItem(GetQueryStringCollection(item), item.QueryString, item.Allowed ? "queryString" : "sequence");
        }

        [ModuleServiceMethod]
        public HiddenSegmentsItem[] GetHiddenSegments()
        {
            return GetItems(GetSection().GetCollection("hiddenSegments"), element => new HiddenSegmentsItem
            {
                Segment = (string)element["segment"],
                Flag = element.IsLocallyStored ? "Local" : "Inherited"
            });
        }

        [ModuleServiceMethod]
        public void AddHiddenSegment(HiddenSegmentsItem item)
        {
            AddItem(GetSection().GetCollection("hiddenSegments"), item.Segment, element => element["segment"] = item.Segment);
        }

        [ModuleServiceMethod]
        public void RemoveHiddenSegment(HiddenSegmentsItem item)
        {
            RemoveItem(GetSection().GetCollection("hiddenSegments"), item.Segment, "segment");
        }

        [ModuleServiceMethod]
        public HeadersItem[] GetHeaders()
        {
            return GetItems(GetSection().GetCollection("headerLimits"), element => new HeadersItem
            {
                Header = (string)element["header"],
                SizeLimit = (uint)element["sizeLimit"],
                Flag = element.IsLocallyStored ? "Local" : "Inherited"
            });
        }

        [ModuleServiceMethod]
        public void AddHeader(HeadersItem item)
        {
            AddItem(GetSection().GetCollection("headerLimits"), item.Header, element =>
            {
                element["header"] = item.Header;
                element["sizeLimit"] = item.SizeLimit;
            });
        }

        [ModuleServiceMethod]
        public void RemoveHeader(HeadersItem item)
        {
            RemoveItem(GetSection().GetCollection("headerLimits"), item.Header, "header");
        }

        [ModuleServiceMethod]
        public FileExtensionsItem[] GetFileExtensions()
        {
            return GetItems(GetSection().GetCollection("fileExtensions"), element => new FileExtensionsItem
            {
                Extension = (string)element["fileExtension"],
                Allowed = (bool)element["allowed"],
                Flag = element.IsLocallyStored ? "Local" : "Inherited"
            });
        }

        [ModuleServiceMethod]
        public void AddFileExtension(FileExtensionsItem item)
        {
            AddItem(GetSection().GetCollection("fileExtensions"), item.Extension, element =>
            {
                element["fileExtension"] = item.Extension;
                element["allowed"] = item.Allowed;
            });
        }

        [ModuleServiceMethod]
        public void RemoveFileExtension(FileExtensionsItem item)
        {
            RemoveItem(GetSection().GetCollection("fileExtensions"), item.Extension, "fileExtension");
        }

        [ModuleServiceMethod]
        public FilteringRulesItem[] GetFilteringRules()
        {
            var result = new List<FilteringRulesItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection("filteringRules"))
            {
                var item = new FilteringRulesItem
                {
                    Name = (string)element["name"],
                    ScanQueryString = (bool)element["scanQueryString"],
                    ScanUrl = (bool)element["scanUrl"],
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                };
                foreach (ConfigurationElement child in element.GetCollection("scanHeaders"))
                {
                    item.Headers.Add(new ScanHeadersItem { RequestHeader = (string)child["requestHeader"] });
                }

                foreach (ConfigurationElement child in element.GetCollection("appliesTo"))
                {
                    item.Extensions.Add(new AppliesToItem { FileExtension = (string)child["fileExtension"] });
                }

                foreach (ConfigurationElement child in element.GetCollection("denyStrings"))
                {
                    item.DenyStrings.Add(new DenyStringsItem { DenyString = (string)child["string"] });
                }

                result.Add(item);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void AddFilteringRule(FilteringRulesItem item)
        {
            AddFilteringRuleInner(GetSection().GetCollection("filteringRules"), item);
        }

        [ModuleServiceMethod]
        public void UpdateFilteringRule(FilteringRulesItem original, FilteringRulesItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetSection().GetCollection("filteringRules");
            var existing = Find(collection, "name", original.Name);
            if (existing == null)
            {
                throw new InvalidOperationException("Filtering rule was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyFilteringRule(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyFilteringRule(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void RemoveFilteringRule(FilteringRulesItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection("filteringRules");
            var existing = Find(collection, "name", item.Name);
            if (existing == null)
            {
                throw new InvalidOperationException("Filtering rule was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public RequestFilteringSettings GetSettings()
        {
            var section = GetSection();
            var limits = section.ChildElements["requestLimits"];
            return new RequestFilteringSettings
            {
                FileExtensionsAllowUnlisted = (bool)section.ChildElements["fileExtensions"]["allowUnlisted"],
                VerbsAllowUnlisted = (bool)section.ChildElements["verbs"]["allowUnlisted"],
                AllowHighBitCharacters = (bool)section["allowHighBitCharacters"],
                AllowDoubleEscaping = (bool)section["allowDoubleEscaping"],
                MaxAllowedContentLength = (uint)limits["maxAllowedContentLength"],
                MaxUrl = (uint)limits["maxUrl"],
                MaxQueryString = (uint)limits["maxQueryString"]
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(RequestFilteringSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var section = GetSection();
            var limits = section.ChildElements["requestLimits"];
            section.ChildElements["fileExtensions"]["allowUnlisted"] = settings.FileExtensionsAllowUnlisted;
            section.ChildElements["verbs"]["allowUnlisted"] = settings.VerbsAllowUnlisted;
            section["allowHighBitCharacters"] = settings.AllowHighBitCharacters;
            section["allowDoubleEscaping"] = settings.AllowDoubleEscaping;
            limits["maxAllowedContentLength"] = settings.MaxAllowedContentLength;
            limits["maxUrl"] = settings.MaxUrl;
            limits["maxQueryString"] = settings.MaxQueryString;
            ManagementUnit.Update();
        }

        private ConfigurationElementCollection GetUrlCollection(UrlsItem item)
        {
            return GetSection().GetCollection(item.Allowed ? "alwaysAllowedUrls" : "denyUrlSequences");
        }

        private ConfigurationElementCollection GetQueryStringCollection(QueryStringsItem item)
        {
            return GetSection().GetCollection(item.Allowed ? "alwaysAllowedQueryStrings" : "denyQueryStringSequences");
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }

        private static T[] GetItems<T>(ConfigurationElementCollection collection, Func<ConfigurationElement, T> factory)
        {
            var result = new List<T>();
            foreach (ConfigurationElement element in collection)
            {
                result.Add(factory(element));
            }

            return result.ToArray();
        }

        private void AddItem(ConfigurationElementCollection collection, string key, Action<ConfigurationElement> apply)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("A value is required.");
            }

            var element = collection.CreateElement();
            apply(element);
            collection.Add(element);
            ManagementUnit.Update();
        }

        private void RemoveItem(ConfigurationElementCollection collection, string key, string attributeName)
        {
            var existing = Find(collection, attributeName, key);
            if (existing == null)
            {
                throw new InvalidOperationException("The entry was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, string attributeName, string value)
        {
            foreach (ConfigurationElement element in collection)
            {
                if ((string)element[attributeName] == value)
                {
                    return element;
                }
            }

            return null;
        }

        private void AddFilteringRuleInner(ConfigurationElementCollection collection, FilteringRulesItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Name))
            {
                throw new ArgumentException("A rule name is required.");
            }

            var element = collection.CreateElement();
            ApplyFilteringRule(element, item);
            collection.Add(element);
            ManagementUnit.Update();
        }

        private static void ApplyFilteringRule(ConfigurationElement element, FilteringRulesItem item)
        {
            element["name"] = item.Name;
            element["scanQueryString"] = item.ScanQueryString;
            element["scanUrl"] = item.ScanUrl;

            var scanHeaders = element.GetCollection("scanHeaders");
            scanHeaders.Clear();
            foreach (var header in item.Headers)
            {
                var child = scanHeaders.CreateElement();
                child["requestHeader"] = header.RequestHeader;
                scanHeaders.Add(child);
            }

            var appliesTo = element.GetCollection("appliesTo");
            appliesTo.Clear();
            foreach (var extension in item.Extensions)
            {
                var child = appliesTo.CreateElement();
                child["fileExtension"] = extension.FileExtension;
                appliesTo.Add(child);
            }

            var denyStrings = element.GetCollection("denyStrings");
            denyStrings.Clear();
            foreach (var denyString in item.DenyStrings)
            {
                var child = denyStrings.CreateElement();
                child["string"] = denyString.DenyString;
                denyStrings.Add(child);
            }
        }
    }
}

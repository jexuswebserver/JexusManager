// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Server;

namespace JexusManager.Features.TraceFailedRequests
{
    internal sealed class TraceFailedRequestsService : ModuleService
    {
        private const string SectionPath = "system.webServer/tracing/traceFailedRequests";
        private const string DefinitionsSectionPath = "system.webServer/tracing/traceProviderDefinitions";

        [ModuleServiceMethod]
        public TraceFailedRequestsItem[] GetItems()
        {
            var result = new List<TraceFailedRequestsItem>();
            foreach (ConfigurationElement element in GetSection().GetCollection())
            {
                var failureDefinitions = element.GetChildElement("failureDefinitions");
                var item = new TraceFailedRequestsItem
                {
                    OriginalKey = (string)element["path"],
                    Path = (string)element["path"],
                    Verbosity = (long)failureDefinitions["verbosity"],
                    Codes = failureDefinitions["statusCodes"].ToString(),
                    TimeTaken = (TimeSpan)failureDefinitions["timeTaken"],
                    Flag = element.IsLocallyStored ? "Local" : "Inherited"
                };
                foreach (ConfigurationElement area in element.GetCollection("traceAreas"))
                {
                    item.Providers.Add(new Provider
                    {
                        Name = area["provider"].ToString(),
                        Verbosity = (int)(long)area["verbosity"],
                        SelectedAreas = Split((string)area["areas"]),
                        Selected = true
                    });
                }

                result.Add(item);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public Provider[] GetProviderDefinitions()
        {
            var result = new List<Provider>();
            var section = GetDefinitionsSection();
            if (section == null)
            {
                return result.ToArray();
            }

            foreach (ConfigurationElement item in section.GetCollection())
            {
                var name = item.GetAttribute("name").Value.ToString();
                var areas = item.ChildElements["areas"].GetCollection();
                var provider = new Provider { Name = name, Areas = new List<string>(areas.Count) };
                foreach (ConfigurationElement area in areas)
                {
                    provider.Areas.Add(area["name"].ToString());
                }

                result.Add(provider);
            }

            return result.ToArray();
        }

        [ModuleServiceMethod]
        public void Add(TraceFailedRequestsItem item)
        {
            var collection = GetSection().GetCollection();
            var element = collection.CreateElement();
            ApplyRule(element, item);
            collection.Add(element);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Update(TraceFailedRequestsItem original, TraceFailedRequestsItem item)
        {
            if (original == null || item == null)
            {
                throw new ArgumentNullException(original == null ? nameof(original) : nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, original);
            if (existing == null)
            {
                throw new InvalidOperationException("Failed request tracing rule was not found.");
            }

            if (existing.IsLocallyStored)
            {
                ApplyRule(existing, item);
            }
            else
            {
                collection.Remove(existing);
                var element = collection.CreateElement();
                ApplyRule(element, item);
                collection.Add(element);
            }

            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void Remove(TraceFailedRequestsItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Failed request tracing rule was not found.");
            }

            collection.Remove(existing);
            ManagementUnit.Update();
        }

        [ModuleServiceMethod]
        public void MoveUp(TraceFailedRequestsItem item)
        {
            Move(item, -1);
        }

        [ModuleServiceMethod]
        public void MoveDown(TraceFailedRequestsItem item)
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
        public TraceFailedRequestsSettings GetSettings()
        {
            var element = GetSiteTraceLogging();
            return new TraceFailedRequestsSettings
            {
                Enabled = element.Enabled,
                Directory = element.Directory,
                MaxLogFiles = element.MaxLogFiles
            };
        }

        [ModuleServiceMethod]
        public void ApplySettings(TraceFailedRequestsSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var element = GetSiteTraceLogging();
            element.Enabled = settings.Enabled;
            element.Directory = settings.Directory;
            element.MaxLogFiles = settings.MaxLogFiles;
            ManagementUnit.Update();
        }

        private void Move(TraceFailedRequestsItem item, int delta)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var collection = GetSection().GetCollection();
            var existing = Find(collection, item);
            if (existing == null)
            {
                throw new InvalidOperationException("Failed request tracing rule was not found.");
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

        private SiteTraceFailedRequestsLogging GetSiteTraceLogging()
        {
            var siteName = ManagementUnit.ConfigurationPath.SiteName;
            if (string.IsNullOrWhiteSpace(siteName))
            {
                throw new InvalidOperationException("Site tracing settings require a site scope.");
            }

            return ManagementUnit.ServerManager.Sites[siteName].TraceFailedRequestsLogging;
        }

        private ConfigurationSection GetSection()
        {
            return ManagementUnit.Configuration.GetSection(SectionPath);
        }

        private ConfigurationSection GetDefinitionsSection()
        {
            if (ManagementUnit.Scope == ManagementScope.Server)
            {
                return ManagementUnit.Configuration.GetSection(DefinitionsSectionPath);
            }

            var locationPath = ManagementUnit.ConfigurationPath.GetEffectiveConfigurationPath(ManagementScope.Site);
            return ManagementUnit.ServerManager.GetApplicationHostConfiguration().GetSection(DefinitionsSectionPath, locationPath);
        }

        private static List<string> Split(string content)
        {
            return string.IsNullOrEmpty(content) ? new List<string>() : new List<string>(content.Split(','));
        }

        private static ConfigurationElement Find(ConfigurationElementCollection collection, TraceFailedRequestsItem item)
        {
            var key = string.IsNullOrEmpty(item.OriginalKey) ? item.Path : item.OriginalKey;
            foreach (ConfigurationElement element in collection)
            {
                if ((string)element["path"] == key)
                {
                    return element;
                }
            }

            return null;
        }

        private static void ApplyRule(ConfigurationElement element, TraceFailedRequestsItem item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Path))
            {
                throw new ArgumentException("A content path is required.");
            }

            element["path"] = item.Path;

            var providers = item.Providers;
            if (providers == null || providers.Count == 0)
            {
                providers = new List<Provider>
                {
                    new Provider { Name = "ASP" },
                    new Provider { Name = "ASPNET", SelectedAreas = new List<string> { "Infrastructure", "Module", "Page", "AppServices" } },
                    new Provider { Name = "ISAPI Extension" },
                    new Provider { Name = "WWW Server", SelectedAreas = new List<string> { "Authentication", "Security", "Filter", "StaticFile", "CGI", "Compression", "Cache", "RequestNotifications", "Module", "Rewrite", "WebSocket" } }
                };
            }

            var areas = element.GetCollection("traceAreas");
            areas.Clear();
            foreach (Provider provider in providers)
            {
                var add = areas.CreateElement("add");
                add["provider"] = provider.Name;
                add["verbosity"] = provider.Verbosity;
                add["areas"] = provider.SelectedAreas.Combine(",");
                areas.Add(add);
            }

            var failureDefinitions = element.GetChildElement("failureDefinitions");
            failureDefinitions["verbosity"] = item.Verbosity;
            failureDefinitions["statusCodes"] = item.Codes;
            failureDefinitions["timeTaken"] = item.TimeTaken;
        }
    }
}

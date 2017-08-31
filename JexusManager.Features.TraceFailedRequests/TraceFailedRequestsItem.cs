// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests
{
    using JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard;
    using Microsoft.Web.Administration;
    using System;
    using System.Collections.Generic;

    internal class TraceFailedRequestsItem : IItem<TraceFailedRequestsItem>
    {
        private IList<Provider> _providers;

        public TraceFailedRequestsItem(ConfigurationElement element)
        {
            Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                Path = string.Empty;
                return;
            }

            Path = (string)element["path"];
            var failureDefinitions = element.GetChildElement("failureDefinitions");
            Verbosity = (long)failureDefinitions["verbosity"];
            Codes = failureDefinitions["statusCodes"].ToString();
            TimeTaken = (TimeSpan)failureDefinitions["timeTaken"];
        }

        public string Path { get; set; }

        public long Verbosity { get; set; }

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

        public bool Equals(TraceFailedRequestsItem other)
        {
            // all properties
            return Match(other);
        }

        public void Apply()
        {
            Element["path"] = Path;
            var collection = Element.GetCollection("traceAreas");
            collection.Clear();
            foreach (Provider provider in _providers)
            {
                var add = collection.CreateElement("add");
                add["provider"] = provider.Name;
                add["verbosity"] = provider.Verbosity;
                add["areas"] = provider.SelectedAreas.Combine(",");
                collection.Add(add);
            }
        }

        public bool Match(TraceFailedRequestsItem other)
        {
            // match combined keys.
            return other != null && other.Path == Path;
        }

        internal string GetProviders()
        {
            var collection = Element.ChildElements["traceAreas"].GetCollection();
            var providers = new List<string>(collection.Count);
            foreach (ConfigurationElement item in collection)
            {
                providers.Add(item.GetAttribute("provider").Value.ToString());
            }

            return providers.Combine(",");
        }

        internal void SetProviders(IList<Provider> providers)
        {
            _providers = providers;
        }

        public string Codes { get; set; }

        public TimeSpan TimeTaken { get; set; }
    }
}

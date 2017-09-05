// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Web.Administration;

    internal class FilteringRulesItem : IItem<FilteringRulesItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(FilteringRulesItem other)
        {
            return other != null && other.Name == Name;
        }

        public string Name { get; set; }

        public string ScanString
        {
            get
            {
                var result = new StringBuilder("Header");
                if (ScanUrl)
                {
                    result.Insert(0, "Url, ");
                }

                if (ScanQueryString)
                {
                    result.Insert(0, "Query string, ");
                }

                return result.ToString();
            }
        }

        public string AppliesToString
        {
            get
            {
                return Extensions.Select(item => item.FileExtension).Combine(", ");
            }
        }

        public string DenyStringsString
        {
            get
            {
                return DenyStrings.Select(item => item.DenyString).Combine(", ");
            }
        }

        public bool ScanUrl { get; set; }
        public bool ScanQueryString { get; set; }
        public List<ScanHeadersItem> Headers { get; set; }
        public List<AppliesToItem> Extensions { get; set; }
        public List<DenyStringsItem> DenyStrings { get; set; }
        public string Flag { get; set; }

        public FilteringRulesItem(ConfigurationElement element)
        {
            Element = element;
            Headers = new List<ScanHeadersItem>();
            Extensions = new List<AppliesToItem>();
            DenyStrings = new List<DenyStringsItem>();
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                return;
            }

            Name = (string)element["name"];
            ScanQueryString = (bool)element["scanQueryString"];
            ScanUrl = (bool)element["scanUrl"];
            foreach (ConfigurationElement child in element.GetCollection("scanHeaders"))
            {
                Headers.Add(new ScanHeadersItem(child));
            }

            foreach (ConfigurationElement child in element.GetCollection("appliesTo"))
            {
                Extensions.Add(new AppliesToItem(child));
            }

            foreach (ConfigurationElement child in element.GetCollection("denyStrings"))
            {
                DenyStrings.Add(new DenyStringsItem(child));
            }
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["scanQueryString"] = ScanQueryString;
            Element["scanUrl"] = ScanUrl;
            var collection = Element.GetCollection("scanHeaders");
            collection.Clear();
            collection.Delete();
            foreach (var header in Headers)
            {
                header.AppendTo(collection);
            }

            var appliesToCollection = Element.GetCollection("appliesTo");
            appliesToCollection.Clear();
            appliesToCollection.Delete();
            foreach (var header in Extensions)
            {
                header.AppendTo(appliesToCollection);
            }

            var denyStringsCollection = Element.GetCollection("denyStrings");
            denyStringsCollection.Clear();
            denyStringsCollection.Delete();
            foreach (var header in DenyStrings)
            {
                header.AppendTo(denyStringsCollection);
            }
        }

        public bool Equals(FilteringRulesItem other)
        {
            return Match(other);
        }
    }
}

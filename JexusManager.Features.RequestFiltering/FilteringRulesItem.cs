// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Web.Administration;

    [Serializable]
    internal class FilteringRulesItem : IItem<FilteringRulesItem>
    {
        public FilteringRulesItem()
        {
            Headers = new List<ScanHeadersItem>();
            Extensions = new List<AppliesToItem>();
            DenyStrings = new List<DenyStringsItem>();
            Flag = "Local";
        }

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

        public void Apply()
        {
        }

        public bool Equals(FilteringRulesItem other)
        {
            return Match(other);
        }
    }
}

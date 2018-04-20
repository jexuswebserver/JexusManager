// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiFilters
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Web.Administration;

    internal class IsapiFiltersItem : IItem<IsapiFiltersItem>
    {
        public IsapiFiltersItem(ConfigurationElement element)
        {
            Element = element;
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                Path = string.Empty;
                PreConditions = new List<string>(0);
                return;
            }

            Name = (string)element["name"];
            Path = (string)element["path"];
            var content = (string)element["preCondition"];
            PreConditions = content.Split(',').ToList();
            EnableCache = (bool)element["enableCache"];
        }

        public List<string> PreConditions { get; set; }

        public bool EnableCache { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

        public bool Equals(IsapiFiltersItem other)
        {
            // all properties
            return Match(other) && other.Path == Path;
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["path"] = Path;
            Element["preCondition"] = PreConditions.Combine(",");
            Element["enableCache"] = EnableCache;
        }

        public bool Match(IsapiFiltersItem other)
        {
            // match combined keys.
            return other != null && other.Name == Name;
        }
    }
}

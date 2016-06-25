// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiFilters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.Web.Administration;

    internal class IsapiFiltersItem : IItem<IsapiFiltersItem>
    {
        public IsapiFiltersItem(ConfigurationElement element)
        {
            this.Element = element;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                Path = string.Empty;
                PreConditions = new List<string>(0);
                return;
            }

            this.Name = (string)element["name"];
            this.Path = (string)element["path"];
            var content = (string)element["preCondition"];
            this.PreConditions = content.Split(',').ToList();
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
            return this.Match(other) && other.Path == this.Path;
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["path"] = Path;
            Element["preCondition"] = Combine(PreConditions);
            Element["enableCache"] = EnableCache;
        }

        private static string Combine(List<string> preConditions)
        {
            if (preConditions.Count == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder(preConditions[0]);
            for (int index = 1; index < preConditions.Count; index++)
            {
                result.AppendFormat(",{0}", preConditions[index]);
            }

            return result.ToString();
        }

        public bool Match(IsapiFiltersItem other)
        {
            // match combined keys.
            return other != null && other.Name == this.Name;
        }
    }
}

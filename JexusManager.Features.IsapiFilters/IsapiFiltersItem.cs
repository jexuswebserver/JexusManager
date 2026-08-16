// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiFilters
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Web.Administration;

    [System.Serializable]
    internal class IsapiFiltersItem : IItem<IsapiFiltersItem>
    {
        public IsapiFiltersItem() { Name = Path = string.Empty; PreConditions = new List<string>(); Flag = "Local"; }


        public List<string> PreConditions { get; set; }

        public bool EnableCache { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        internal string OriginalKey { get; set; }

        public string Flag { get; set; }

        public bool Equals(IsapiFiltersItem other)
        {
            // all properties
            return Match(other) && other.Path == Path;
        }

        public bool Match(IsapiFiltersItem other)
        {
            // match combined keys.
            return other != null && other.Name == Name;
        }
    }
}

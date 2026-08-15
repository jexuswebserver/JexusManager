// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    public class ConditionItem : IItem<ConditionItem>
    {
        public ConditionItem()
        {
            Input = "{QUERY_STRING}";
            MatchType = 4;
            IgnoreCase = true;
        }

        public ConfigurationElement Element { get; set; }

        public bool IgnoreCase { get; set; }

        public string Pattern { get; set; }

        public string Input { get; set; }

        public int MatchType { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
        }

        public bool Match(ConditionItem other)
        {
            return other != null && Input == other.Input && MatchType == other.MatchType && Pattern == other.Pattern && IgnoreCase == other.IgnoreCase;
        }

        public bool Equals(ConditionItem other)
        {
            return Match(other);
        }
    }
}

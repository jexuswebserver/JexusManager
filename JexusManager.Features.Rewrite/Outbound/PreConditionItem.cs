// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    [Serializable]
    public class PreConditionItem : IItem<PreConditionItem>
    {
        public PreConditionItem()
        {
            Conditions = new List<ConditionItem>();
            Flag = "Local";
        }

        public ConfigurationElement Element { get; set; }

        public List<ConditionItem> Conditions { get; set; }

        public long PatternSyntax { get; set; }

        public long LogicalGrouping { get; set; }

        public string Name { get; set; }

        internal string OriginalKey { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
        }

        public bool Match(PreConditionItem other)
        {
            return other != null && Name == other.Name;
        }

        public bool Equals(PreConditionItem other)
        {
            return Match(other) && LogicalGrouping == other.LogicalGrouping && PatternSyntax == other.PatternSyntax; // TODO: compare children.
        }
    }
}

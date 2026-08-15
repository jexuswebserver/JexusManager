// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Outbound
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    [Serializable]
    public class OutboundRule : IItem<OutboundRule>
    {
        public OutboundRule()
        {
            Flag = "Local";
            Conditions = new List<ConditionItem>();
            Input = "URL path after '/'";
        }

        public string Name { get; set; }

        public string Input { get; set; }

        public bool Scope { get; set; }

        public string Pattern { get; set; }

        public long Action { get; set; }

        public bool Stopping { get; set; }

        public string Flag { get; set; }

        public ConfigurationElement Element { get; set; }

        public bool Enabled { get; set; }

        public RuleSettingsUpdatedEventHandler RuleSettingsUpdated { get; set; }

        public bool IgnoreCase { get; set; }

        public long Syntax { get; set; }

        public List<ConditionItem> Conditions { get; }

        public bool Equals(OutboundRule other)
        {
            return Match(other);
        }

        public bool Match(OutboundRule other)
        {
            return other != null && other.Name == Name;
        }

        public bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
            return true;
        }

        public void CancelChanges()
        {
        }

        public bool Negate { get; set; }

        public bool Replace { get; set; }

        public string Value { get; set; }

        public string ServerVariable { get; set; }

        public long Filter { get; set; }

        public string PreCondition { get; set; }

        public long LogicalGrouping { get; set; }

        public bool TrackAllCaptures { get; set; }

        public string CustomTags { get; set; }

        public bool ApplyChanges()
        {
            return true;
        }

        public void Apply()
        {
        }
    }
}

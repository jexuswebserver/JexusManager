// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    [Serializable]
    public class InboundRule : IItem<InboundRule>
    {
        public InboundRule()
        {
            Flag = "Local";
            ServerVariables = new List<ServerVariableItem>();
            Conditions = new List<ConditionItem>();
            Type = 1;
            IgnoreCase = true;
            AppendQueryString = true;
            Enabled = true;
        }

        public string Name { get; set; }

        internal string OriginalKey { get; set; }

        public string Input { get; set; }

        public bool Negate { get; set; }

        public string PatternUrl { get; set; }

        public long Type { get; set; }

        public string ActionUrl { get; set; }

        public bool StopProcessing { get; set; }

        public string Flag { get; set; }

        public bool Enabled { get; set; }

        public RuleSettingsUpdatedEventHandler RuleSettingsUpdated { get; set; }

        public bool IgnoreCase { get; set; }

        public long PatternSyntax { get; set; }

        public bool AppendQueryString { get; set; }

        public bool LogRewrittenUrl { get; set; }

        public int RedirectType { get; set; }

        public uint StatusCode { get; set; }

        public uint SubStatusCode { get; set; }

        public string StatusReason { get; set; }

        public string StatusDescription { get; set; }

        public List<ServerVariableItem> ServerVariables { get; }

        public List<ConditionItem> Conditions { get; }

        public bool Equals(InboundRule other)
        {
            return Match(other);
        }

        public bool Match(InboundRule other)
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

        public long LogicalGrouping { get; set; }

        public bool TrackAllCaptures { get; set; }

        public bool ApplyChanges()
        {
            return true;
        }
    }
}

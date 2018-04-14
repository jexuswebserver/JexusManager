// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Outbound
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Web.Administration;

    public class OutboundRule : IItem<OutboundRule>
    {
        public OutboundRule(ConfigurationElement element)
        {
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            Element = element;
            Conditions = new List<ConditionItem>();
            if (element == null)
            {
                Input = "URL path after '/'";
                return;
            }

            CancelChanges();
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
            Name = (string)Element["name"];
            PreCondition = (string)Element["preCondition"];
            Enabled = (bool)Element["enabled"];
            Syntax = (long)Element["patternSyntax"];
            Stopping = (bool)Element["stopProcessing"];
            ConfigurationElement matchElement = Element.ChildElements["match"];
            Filter = (long)matchElement["filterByTags"];
            CustomTags = (string)matchElement["customTags"];
            ServerVariable = (string)matchElement["serverVariable"];
            Pattern = (string)matchElement["pattern"];
            IgnoreCase = (bool)matchElement["ignoreCase"];
            Negate = (bool)matchElement["negate"];

            var conditions = Element.ChildElements["conditions"];
            TrackAllCaptures = (bool)conditions["trackAllCaptures"];
            LogicalGrouping = (long)conditions["logicalGrouping"];
            foreach (ConfigurationElement condition in conditions.GetCollection())
            {
                var item = new ConditionItem(condition);
                Conditions.Add(item);
            }

            ConfigurationElement actionElement = Element.ChildElements["action"];
            Action = (long)actionElement["type"];
            Value = (string)actionElement["value"];
            Replace = (bool)actionElement["replace"];
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
            Apply();
            return true;
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["preCondition"] = PreCondition;
            Element["enabled"] = Enabled;
            Element["patternSyntax"] = Syntax;
            Element["stopProcessing"] = Stopping;
            ConfigurationElement matchElement = Element.ChildElements["match"];
            matchElement["filterByTags"] = Filter;
            matchElement["customTags"] = CustomTags;
            matchElement["serverVariable"] = ServerVariable;
            matchElement["ignoreCase"] = IgnoreCase;
            matchElement["pattern"] = Pattern;
            matchElement["negate"] = Negate;
            ConfigurationElement actionElement = Element.ChildElements["action"];
            actionElement["type"] = Action;
            actionElement["value"] = Value;
            actionElement["replace"] = Replace;

            var conditions = Element.ChildElements["conditions"];
            conditions["trackAllCaptures"] = TrackAllCaptures;
            conditions["logicalGrouping"] = LogicalGrouping;
            var conditionsCollection = conditions.GetCollection();
            conditionsCollection.Clear();
            foreach (var condition in Conditions)
            {
                condition.AppendTo(conditionsCollection);
            }
        }
    }
}

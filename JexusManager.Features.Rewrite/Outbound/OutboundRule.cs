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
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            this.Element = element;
            this.Conditions = new List<ConditionItem>();
            if (element == null)
            {
                this.Input = "URL path after '/'";
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
            return other != null && other.Name == this.Name;
        }

        public bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
            return true;
        }

        public void CancelChanges()
        {
            this.Name = (string)this.Element["name"];
            this.PreCondition = (string)this.Element["preCondition"];
            this.Enabled = (bool)this.Element["enabled"];
            this.Syntax = (long)this.Element["patternSyntax"];
            this.Stopping = (bool)this.Element["stopProcessing"];
            ConfigurationElement matchElement = this.Element.ChildElements["match"];
            this.Filter = (long)matchElement["filterByTags"];
            CustomTags = (string)matchElement["customTags"];
            this.ServerVariable = (string)matchElement["serverVariable"];
            this.Pattern = (string)matchElement["pattern"];
            this.IgnoreCase = (bool)matchElement["ignoreCase"];
            this.Negate = (bool)matchElement["negate"];

            var conditions = this.Element.ChildElements["conditions"];
            this.TrackAllCaptures = (bool)conditions["trackAllCaptures"];
            this.LogicalGrouping = (long)conditions["logicalGrouping"];
            foreach (ConfigurationElement condition in conditions.GetCollection())
            {
                var item = new ConditionItem(condition);
                this.Conditions.Add(item);
            }

            ConfigurationElement actionElement = this.Element.ChildElements["action"];
            this.Action = (long)actionElement["type"];
            this.Value = (string)actionElement["value"];
            this.Replace = (bool)actionElement["replace"];
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
            this.Apply();
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

        public void AppendTo(ConfigurationElementCollection rulesCollection)
        {
            Element = rulesCollection.CreateElement();
            Apply();
            rulesCollection.Add(Element);
        }
    }
}

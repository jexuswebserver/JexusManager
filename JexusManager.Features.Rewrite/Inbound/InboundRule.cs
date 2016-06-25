// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Web.Administration;

    public class InboundRule : IItem<InboundRule>
    {
        public InboundRule(ConfigurationElement element)
        {
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            this.Element = element;
            this.ServerVariables = new List<ServerVariableItem>();
            this.Conditions = new List<ConditionItem>();
            if (element == null)
            {
                this.Type = 1;
                IgnoreCase = true;
                this.AppendQueryString = true;
                Enabled = true;
                return;
            }

            CancelChanges();
        }

        public string Name { get; set; }

        public string Input { get; set; }

        public bool Negate { get; set; }

        public string PatternUrl { get; set; }

        public long Type { get; set; }

        public string ActionUrl { get; set; }

        public bool StopProcessing { get; set; }

        public string Flag { get; set; }

        public ConfigurationElement Element { get; set; }

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
            return other != null && other.Name == this.Name;
        }

        public bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
            return true;
        }

        public void CancelChanges()
        {
            if (Element == null)
            {
                return;
            }

            this.Name = (string)this.Element["name"];
            this.Enabled = (bool)this.Element["enabled"];
            this.PatternSyntax = (long)this.Element["patternSyntax"];
            this.StopProcessing = (bool)this.Element["stopProcessing"];
            ConfigurationElement matchElement = this.Element.ChildElements["match"];
            this.PatternUrl = (string)matchElement["url"];
            this.Negate = (bool)matchElement["negate"];
            ConfigurationElement actionElement = this.Element.ChildElements["action"];
            this.Type = (long)actionElement["type"];
            this.ActionUrl = (string)actionElement["url"];
            this.AppendQueryString = (bool)actionElement["appendQueryString"];
            this.LogRewrittenUrl = (bool)actionElement["logRewrittenUrl"];
            var redirect = (long)actionElement["redirectType"];
            switch (redirect)
            {
                case 301:
                    this.RedirectType = 0;
                    break;
                case 302:
                    this.RedirectType = 1;
                    break;
                case 303:
                    this.RedirectType = 2;
                    break;
                case 307:
                    this.RedirectType = 3;
                    break;
            }

            this.StatusCode = (uint)actionElement["statusCode"];
            this.SubStatusCode = (uint)actionElement["subStatusCode"];
            this.StatusReason = (string)actionElement["statusReason"];
            this.StatusDescription = (string)actionElement["statusDescription"];

            var conditions = this.Element.ChildElements["conditions"];
            this.TrackAllCaptures = (bool)conditions["trackAllCaptures"];
            this.LogicalGrouping = (long)conditions["logicalGrouping"];

            foreach (ConfigurationElement condition in conditions.GetCollection())
            {
                var item = new ConditionItem(condition);
                this.Conditions.Add(item);
            }

            var variables = this.Element.ChildElements["serverVariables"];
            foreach (ConfigurationElement variable in variables.GetCollection())
            {
                var item = new ServerVariableItem(variable);
                this.ServerVariables.Add(item);
            }
        }

        public long LogicalGrouping { get; set; }

        public bool TrackAllCaptures { get; set; }

        public bool ApplyChanges()
        {
            this.Apply();
            return true;
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["enabled"] = Enabled;
            Element["patternSyntax"] = this.PatternSyntax;
            Element["stopProcessing"] = this.StopProcessing;
            ConfigurationElement matchElement = Element.ChildElements["match"];
            matchElement["url"] = this.PatternUrl;
            matchElement["negate"] = this.Negate;
            ConfigurationElement actionElement = Element.ChildElements["action"];
            actionElement["type"] = this.Type;
            actionElement["url"] = this.ActionUrl;
            actionElement["appendQueryString"] = this.AppendQueryString;
            actionElement["logRewrittenUrl"] = this.LogRewrittenUrl;

            switch (RedirectType)
            {
                case 0:
                    actionElement["redirectType"] = 301L;
                    break;
                case 1:
                    actionElement["redirectType"] = 302L;
                    break;
                case 2:
                    actionElement["redirectType"] = 303L;
                    break;
                case 3:
                    actionElement["redirectType"] = 307L;
                    break;
            }

            actionElement["statusCode"] = this.StatusCode;
            actionElement["subStatusCode"] = this.SubStatusCode;
            actionElement["statusReason"] = this.StatusReason;
            actionElement["statusDescription"] = this.StatusDescription;

            var conditions = Element.ChildElements["conditions"];
            conditions["trackAllCaptures"] = TrackAllCaptures;
            conditions["logicalGrouping"] = LogicalGrouping;
            var conditionsCollection = conditions.GetCollection();
            conditionsCollection.Clear();
            foreach (var condition in Conditions)
            {
                condition.AppendTo(conditionsCollection);
            }

            var variableCollection = Element.ChildElements["serverVariables"].GetCollection();
            variableCollection.Clear();
            foreach (var variable in ServerVariables)
            {
                variable.AppendTo(variableCollection);
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

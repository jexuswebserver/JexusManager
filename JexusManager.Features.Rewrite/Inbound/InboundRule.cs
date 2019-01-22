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

    using Microsoft.Web.Administration;

    public class InboundRule : IItem<InboundRule>
    {
        public InboundRule(ConfigurationElement element)
        {
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            Element = element;
            ServerVariables = new List<ServerVariableItem>();
            Conditions = new List<ConditionItem>();
            if (element == null)
            {
                Type = 1;
                IgnoreCase = true;
                AppendQueryString = true;
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
            return other != null && other.Name == Name;
        }

        public bool ShowHelp()
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130425&amp;clcid=0x409");
            return true;
        }

        public void CancelChanges()
        {
            if (Element == null)
            {
                return;
            }

            Name = (string)Element["name"];
            Enabled = (bool)Element["enabled"];
            PatternSyntax = (long)Element["patternSyntax"];
            StopProcessing = (bool)Element["stopProcessing"];
            ConfigurationElement matchElement = Element.ChildElements["match"];
            PatternUrl = (string)matchElement["url"];
            Negate = (bool)matchElement["negate"];
            IgnoreCase = (bool)matchElement["ignoreCase"];
            ConfigurationElement actionElement = Element.ChildElements["action"];
            Type = (long)actionElement["type"];
            ActionUrl = (string)actionElement["url"];
            AppendQueryString = (bool)actionElement["appendQueryString"];
            LogRewrittenUrl = (bool)actionElement["logRewrittenUrl"];
            var redirect = (long)actionElement["redirectType"];
            switch (redirect)
            {
                case 301:
                    RedirectType = 0;
                    break;
                case 302:
                    RedirectType = 1;
                    break;
                case 303:
                    RedirectType = 2;
                    break;
                case 307:
                    RedirectType = 3;
                    break;
            }

            StatusCode = (uint)actionElement["statusCode"];
            SubStatusCode = (uint)actionElement["subStatusCode"];
            StatusReason = (string)actionElement["statusReason"];
            StatusDescription = (string)actionElement["statusDescription"];

            var conditions = Element.ChildElements["conditions"];
            TrackAllCaptures = (bool)conditions["trackAllCaptures"];
            LogicalGrouping = (long)conditions["logicalGrouping"];

            Conditions.Clear();
            foreach (ConfigurationElement condition in conditions.GetCollection())
            {
                var item = new ConditionItem(condition);
                Conditions.Add(item);
            }

            ServerVariables.Clear();
            var variables = Element.ChildElements["serverVariables"];
            foreach (ConfigurationElement variable in variables.GetCollection())
            {
                var item = new ServerVariableItem(variable);
                ServerVariables.Add(item);
            }
        }

        public long LogicalGrouping { get; set; }

        public bool TrackAllCaptures { get; set; }

        public bool ApplyChanges()
        {
            Apply();
            return true;
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["enabled"] = Enabled;
            Element["patternSyntax"] = PatternSyntax;
            Element["stopProcessing"] = StopProcessing;
            ConfigurationElement matchElement = Element.ChildElements["match"];
            matchElement["url"] = PatternUrl;
            matchElement["negate"] = Negate;
            matchElement["ignoreCase"] = IgnoreCase;
            ConfigurationElement actionElement = Element.ChildElements["action"];
            actionElement["type"] = Type;
            actionElement["url"] = ActionUrl;
            actionElement["appendQueryString"] = AppendQueryString;
            actionElement["logRewrittenUrl"] = LogRewrittenUrl;

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

            actionElement["statusCode"] = StatusCode;
            actionElement["subStatusCode"] = SubStatusCode;
            actionElement["statusReason"] = StatusReason;
            actionElement["statusDescription"] = StatusDescription;

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
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    public class PreConditionItem
    {
        public ConfigurationElement Element { get; set; }

        public PreConditionItem(ConfigurationElement element)
        {
            this.Element = element;
            this.Conditions = new List<ConditionItem>();
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                return;
            }

            this.Name = (string)element["name"];
            this.LogicalGrouping = (long)element["logicalGrouping"];
            this.PatternSyntax = (long)element["patternSyntax"];
            var items = element.GetCollection();
            foreach (ConfigurationElement item in items)
            {
                var subElement = new ConditionItem(item);
                this.Conditions.Add(subElement);
            }
        }

        public List<ConditionItem> Conditions { get; set; }

        public long PatternSyntax { get; set; }

        public long LogicalGrouping { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
            Element["name"] = Name;
            Element["logicalGrouping"] = LogicalGrouping;
            Element["patternSyntax"] = PatternSyntax;

            var conditions = Element.GetCollection();
            conditions.Clear();
            foreach (var item in Conditions)
            {
                item.AppendTo(conditions);
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
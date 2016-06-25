// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using Microsoft.Web.Administration;

    public class ConditionItem
    {
        public ConfigurationElement Element { get; set; }

        public ConditionItem(ConfigurationElement element)
        {
            this.Element = element;
            if (element == null)
            {
                this.Input = "{QUERY_STRING}";
                this.MatchType = 4;
                this.IgnoreCase = true;
                return;
            }

            this.Input = (string)element["input"];
            this.Pattern = (string)element["pattern"];
            this.IgnoreCase = (bool)element["ignoreCase"];

            var root = (long)element["matchType"];
            var negate = (bool)element["negate"];
            this.MatchType = (int)root * 2 + (negate ? 1 : 0);
        }

        public bool IgnoreCase { get; set; }

        public string Pattern { get; set; }

        public string Input { get; set; }

        public int MatchType { get; set; }

        public void Apply()
        {
            Element["input"] = Input;
            Element["pattern"] = Pattern;
            Element["ignoreCase"] = IgnoreCase;
            Element["negate"] = MatchType % 2 == 1;
            Element["matchType"] = MatchType / 2;
        }

        public void AppendTo(ConfigurationElementCollection conditionsCollection)
        {
            Element = conditionsCollection.CreateElement();
            Apply();
            conditionsCollection.Add(Element);
        }
    }
}
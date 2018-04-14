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
            Element = element;
            if (element == null)
            {
                Input = "{QUERY_STRING}";
                MatchType = 4;
                IgnoreCase = true;
                return;
            }

            Input = (string)element["input"];
            Pattern = (string)element["pattern"];
            IgnoreCase = (bool)element["ignoreCase"];

            var root = (long)element["matchType"];
            var negate = (bool)element["negate"];
            var value = root == 0 ? 2 : root - 1;
            MatchType = (int)value * 2 + (negate ? 1 : 0);
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
            var value = MatchType / 2;
            Element["matchType"] = value == 2 ? 0 : value + 1;
        }

        public void AppendTo(ConfigurationElementCollection conditionsCollection)
        {
            Element = conditionsCollection.CreateElement();
            Apply();
            conditionsCollection.Add(Element);
        }
    }
}

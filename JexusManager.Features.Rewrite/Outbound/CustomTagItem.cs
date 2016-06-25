// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Outbound
{
    using Microsoft.Web.Administration;

    public class CustomTagItem : IItem<CustomTagItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(CustomTagItem other)
        {
            return other != null && other.Name == Name;
        }

        public CustomTagItem(ConfigurationElement element)
        {
            this.Element = element;
            if (element == null)
            {
                return;
            }

            this.Name = (string)element["name"];
            this.Attribute = (string)element["attribute"];
        }

        public string Attribute { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
            Element["name"] = Name;
            Element["attribute"] = Attribute;
        }

        public bool Equals(CustomTagItem other)
        {
            return Match(other) && other.Attribute == Attribute;
        }
    }
}

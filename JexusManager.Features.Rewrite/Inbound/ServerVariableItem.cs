// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using Microsoft.Web.Administration;

    public class ServerVariableItem : IItem<ServerVariableItem>
    {
        public ConfigurationElement Element { get; set; }

        public ServerVariableItem(ConfigurationElement element)
        {
            Element = element;
            if (element == null)
            {
                Replace = true;
                return;
            }

            Name = (string)element["name"];
            Value = (string)element["value"];
            Replace = (bool)element["replace"];
        }

        public bool Replace { get; set; }

        public string Value { get; set; }

        public string Name { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
            Element["name"] = Name;
            Element["value"] = Value;
            Element["replace"] = Replace;
        }

        public bool Match(ServerVariableItem other)
        {
            return other != null && Name == other.Name;
        }

        public bool Equals(ServerVariableItem other)
        {
            return Match(other) && Value == other.Value && Replace == other.Replace;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using Microsoft.Web.Administration;

    public class ServerVariableItem
    {
        public ConfigurationElement Element { get; set; }

        public ServerVariableItem(ConfigurationElement element)
        {
            this.Element = element;
            if (element == null)
            {
                this.Replace = true;
                return;
            }

            this.Name = (string)element["name"];
            this.Value = (string)element["value"];
            this.Replace = (bool)element["replace"];
        }

        public bool Replace { get; set; }

        public string Value { get; set; }

        public string Name { get; set; }

        public void Apply()
        {
            Element["name"] = Name;
            Element["value"] = Value;
            Element["replace"] = Replace;
        }

        public void AppendTo(ConfigurationElementCollection variableCollection)
        {
            Element = variableCollection.CreateElement();
            Apply();
            variableCollection.Add(Element);
        }
    }
}
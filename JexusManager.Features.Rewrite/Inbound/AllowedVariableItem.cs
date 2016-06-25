// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using Microsoft.Web.Administration;

    internal class AllowedVariableItem : IItem<AllowedVariableItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(AllowedVariableItem other)
        {
            return other != null && other.Name == Name;
        }

        private readonly AllowedVariablesFeature _feature;

        public string Name { get; internal set; }

        public string Flag { get; set; }

        public AllowedVariableItem(ConfigurationElement element, AllowedVariablesFeature feature)
        {
            this.Element = element;
            _feature = feature;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            if (element == null)
            {
                return;
            }

            this.Name = (string)element["name"];
        }

        public void Apply()
        {
            Element["name"] = Name;
        }

        public bool Equals(AllowedVariableItem other)
        {
            return Match(other);
        }
    }
}

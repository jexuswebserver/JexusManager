// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using Microsoft.Web.Administration;

    internal class MapRule : IItem<MapRule>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(MapRule other)
        {
            return other != null && other.Original == Original;
        }

        private MapsFeature _feature;

        public MapRule(ConfigurationElement element, MapsFeature feature)
        {
            this.Element = element;
            _feature = feature;
            if (element != null)
            {
                this.Original = (string)element["key"];
                this.New = (string)element["value"];
            }

            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
        }

        public string Original { get; set; }

        public string New { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
            Element["key"] = Original;
            Element["value"] = New;
        }

        public bool Equals(MapRule other)
        {
            return Match(other) && other.New == New;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite.Inbound
{
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    internal class MapItem : IItem<MapItem>
    {
        public ConfigurationElement Element { get; set; }

        public bool Match(MapItem other)
        {
            return other != null && other.Name == Name;
        }

        private readonly MapsFeature _feature;

        public string Name { get; internal set; }

        public string Flag { get; set; }

        public string DefaultValue { get; set; }

        public MapSettingsUpdatedEventHandler MapSettingsUpdated { get; set; }

        public List<MapRule> Items { get; private set; }

        public MapRule SelectedItem { get; set; }

        public MapItem(ConfigurationElement element, MapsFeature feature)
        {
            this.Element = element;
            _feature = feature;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            this.Name = element == null ? string.Empty : (string)element["name"];
            this.Items = new List<MapRule>();
            if (element != null)
            {
                var collection = element.GetCollection();
                foreach (ConfigurationElement rule in collection)
                {
                    this.Items.Add(new MapRule(rule, _feature));
                }
            }
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["defaultValue"] = DefaultValue;
            Element["ignoreCase"] = IgnoreCase;
        }

        internal protected void OnRewriteSettingsSaved()
        {
            this.MapSettingsUpdated?.Invoke();
        }

        public bool IgnoreCase { get; set; }

        public bool Equals(MapItem other)
        {
            return Match(other) && other.DefaultValue == DefaultValue && other.IgnoreCase == IgnoreCase;
        }
    }
}

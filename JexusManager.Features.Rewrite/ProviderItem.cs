// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Web.Administration;

    /// <summary>
    /// Represents a URL rewrite provider item in system.webServer/rewrite/providers.
    /// </summary>
    public class ProviderItem : IItem<ProviderItem>
    {

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string Name
        {
            get { return (string)Element["name"]; }
            set { Element["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the type of the provider.
        /// </summary>
        public string Type
        {
            get { return (string)Element["type"]; }
            set { Element["type"] = value; }
        }

        /// <summary>
        /// Gets the settings collection for this provider.
        /// </summary>
        public IList<SettingItem> Settings { get; } = new List<SettingItem>();

        /// <summary>
        /// Gets a flag indicating whether this item is locally stored or inherited.
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// Gets the configuration element representing this provider.
        /// </summary>
        public ConfigurationElement Element { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderItem"/> class.
        /// </summary>
        /// <param name="element">The provider configuration element.</param>
        public ProviderItem(ConfigurationElement element)
        {
            Element = element ?? throw new ArgumentNullException(nameof(element));
            Flag = element.IsLocallyStored ? "Local" : "Inherited";

            // Load settings from the provider's settings collection
            var settingsCollection = Element.GetChildElement("settings")?.GetCollection();
            if (settingsCollection != null)
            {
                foreach (ConfigurationElement setting in settingsCollection)
                {
                    Settings.Add(new SettingItem(setting));
                }
            }
        }

        /// <summary>
        /// Updates an existing provider item with the current values.
        /// </summary>
        public void Apply()
        {
            Element["name"] = Name;
            Element["type"] = Type;

            var conditions = Element.GetCollection();
            conditions.Clear();
            foreach (var item in Settings)
            {
                item.AppendTo(conditions);
            }
        }

        /// <summary>
        /// Determines whether this provider item equals another provider item.
        /// </summary>
        /// <param name="other">The provider item to compare with.</param>
        /// <returns>True if the items are equal, false otherwise.</returns>
        public bool Equals(ProviderItem other)
        {
            return Match(other) && Type == other.Type;
        }

        /// <summary>
        /// Determines whether this provider item has the same key values as another provider item.
        /// </summary>
        /// <param name="other">The provider item to compare with.</param>
        /// <returns>True if the items match, false otherwise.</returns>
        public bool Match(ProviderItem other)
        {
            if (other is null)
                return false;

            return Name == other.Name;
        }
    }
}

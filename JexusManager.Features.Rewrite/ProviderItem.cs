// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Web.Administration;

    /// <summary>
    /// Represents a URL rewrite provider item in system.webServer/rewrite/providers.
    /// </summary>
    [Serializable]
    public class ProviderItem : IItem<ProviderItem>
    {
        public ProviderItem()
        {
            Name = string.Empty;
            Type = string.Empty;
            Flag = "Local";
        }

        public string Name { get; set; }

        internal string OriginalKey { get; set; }

        public string Type { get; set; }

        public IList<SettingItem> Settings { get; } = new List<SettingItem>();

        public string Flag { get; set; }

        public ConfigurationElement Element { get; set; }

        public void Apply()
        {
        }

        public bool Equals(ProviderItem other)
        {
            return Match(other) && Type == other.Type;
        }

        public bool Match(ProviderItem other)
        {
            if (other is null)
                return false;

            return Name == other.Name;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;

    using Microsoft.Web.Administration;

    [Serializable]
    public class SettingItem : IItem<SettingItem>
    {
        public SettingItem()
        {
            Flag = "Local";
            Key = string.Empty;
            Value = string.Empty;
        }

        public ConfigurationElement Element { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public bool Encrypted { get; set; }

        public void Apply()
        {
        }

        public bool Match(SettingItem other)
        {
            return other != null && other.Key == Key;
        }

        public bool Equals(SettingItem other)
        {
            return Match(other);
        }

        public string Flag { get; set; }
    }
}

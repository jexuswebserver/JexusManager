// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using Microsoft.Web.Administration;

    public class SettingItem : IItem<SettingItem>
    {
        public ConfigurationElement Element { get; set; }

        public SettingItem(ConfigurationElement element)
        {
            Element = element;
            if (element == null)
            {
                Key = string.Empty;
                Value = string.Empty;
                return;
            }

            Key = (string)element["key"];
            Value = (string)element["value"];
            // TODO: encryptedValue
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
            Element["key"] = Key;
            Element["value"] = Value;
        }

        public bool Match(SettingItem other)
        {
            return other != null && Key == other.Key;
        }

        public bool Equals(SettingItem other)
        {
            return Match(other) && Value == other.Value;
        }
    }
}

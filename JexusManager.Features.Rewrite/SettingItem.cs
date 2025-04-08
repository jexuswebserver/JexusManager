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
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            Element = element;
            if (element == null)
            {
                Key = string.Empty;
                Value = string.Empty;
                return;
            }

            Key = (string)element["key"];
            Value = (string)element["value"];
            // TODO:
            //var temp = (string)element["encryptedValue"];
            //if (temp != null)
            //{
            //    Encrypted = true;
            //    Value = temp;
            //}
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public bool Encrypted { get; set; }

        public string Flag { get; set; }

        public void Apply()
        {
            Element["key"] = Key;
            if (Encrypted)
            {
                Element["encryptedValue"] = Value;
                Element["value"] = null;
            }
            else
            {
                //Element["encryptedValue"] = null;
                Element["value"] = Value;
            }
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

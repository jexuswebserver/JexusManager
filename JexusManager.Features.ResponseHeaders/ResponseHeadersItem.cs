// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.ResponseHeaders
{
    using Microsoft.Web.Administration;

    internal class ResponseHeadersItem : IItem<ResponseHeadersItem>
    {
        public string Name { get; internal set; }
        public string Value { get; internal set; }
        public string Flag { get; set; }
        public ConfigurationElement Element { get; set; }

        public ResponseHeadersItem(ConfigurationElement element)
        {
            Flag = element == null || element.IsLocallyStored ? "Local" : "Inherited";
            Element = element;
            if (element == null)
            {
                Value = string.Empty;
                return;
            }

            Name = (string)element["name"];
            Value = (string)element["value"];
        }

        public bool Equals(ResponseHeadersItem other)
        {
            return Match(other);
        }

        public bool Match(ResponseHeadersItem other)
        {
            return other != null && other.Name == Name;
        }

        public void Apply()
        {
            Element["name"] = Name;
            Element["value"] = Value;
        }
    }
}

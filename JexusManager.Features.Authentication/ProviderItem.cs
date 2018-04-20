// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using Microsoft.Web.Administration;

    public class ProviderItem
    {
        public ConfigurationElement Element { get; set; }

        public ProviderItem(ConfigurationElement element)
        {
            Element = element;
            if (element == null)
            {
                return;
            }

            Value = (string)element["value"];
        }

        public string Value { get; set; }

        public void Apply()
        {
            Element["value"] = Value;
        }
    }
}

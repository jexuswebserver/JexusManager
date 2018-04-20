// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using Microsoft.Web.Administration;

    public class DigestItem
    {
        public ConfigurationElement Element { get; set; }

        public DigestItem(ConfigurationElement element)
        {
            Element = element;
            Realm = (string)element["realm"];
        }

        public string Realm { get; set; }

        public void Apply()
        {
            Element["realm"] = Realm;
        }
    }
}
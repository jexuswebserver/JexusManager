// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using Microsoft.Web.Administration;

    public class BasicItem
    {
        public ConfigurationElement Element { get; set; }

        public BasicItem(ConfigurationElement element)
        {
            Element = element;
            Domain = (string)element["defaultLogonDomain"];
            Realm = (string)element["realm"];
        }

        public string Domain { get; set; }
        public string Realm { get; set; }

        public void Apply()
        {
            Element["defaultLogonDomain"] = Domain;
            Element["realm"] = Realm;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using Microsoft.Web.Administration;

    public class AnonymousItem
    {
        public ConfigurationElement Element { get; set; }

        public AnonymousItem(ConfigurationElement element)
        {
            Element = element;
            Name = (string)element["userName"];
            Password = (string)element["password"];
        }

        public string Name { get; set; }
        public string Password { get; set; }

        public void Apply()
        {
            Element["userName"] = Name;
            Element["password"] = Password;
        }
    }
}

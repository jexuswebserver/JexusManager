// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;

    using Microsoft.Web.Administration;


    public class FormsItem
    {
        public ConfigurationElement Element { get; set; }

        public FormsItem(ConfigurationElement element)
        {
            Element = element;
            LoginUrl = (string)element["loginUrl"];
            Timeout = (TimeSpan)element["timeout"];
            Mode = (long)element["cookieless"];
            Name = (string)element["name"];
            ProtectedMode = (long)element["protection"];
            RequireSsl = (bool)element["requireSSL"];
            SlidinngExpiration = (bool)element["slidingExpiration"];
        }

        public long Mode { get; set; }

        public long ProtectedMode { get; set; }

        public bool RequireSsl { get; set; }

        public bool SlidinngExpiration { get; set; }

        public string Name { get; set; }

        public TimeSpan Timeout { get; set; }

        public string LoginUrl { get; set; }

        public void Apply()
        {
            Element["loginUrl"] = LoginUrl;
            Element["timeout"] = Timeout;
            Element["cookieless"] = Mode;
            Element["name"] = Name;
            Element["protection"] = ProtectedMode;
            Element["requireSSL"] = RequireSsl;
            Element["slidingExpiration"] = SlidinngExpiration;
        }
    }
}

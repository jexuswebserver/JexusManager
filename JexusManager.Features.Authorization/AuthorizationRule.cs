// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authorization
{
    using Microsoft.Web.Administration;

    internal class AuthorizationRule : IItem<AuthorizationRule>
    {
        public AuthorizationRule(ConfigurationElement element)
        {
            this.Element = element;
            this.Flag = element == null || element.IsLocallyStored ? "Local" : "Inhertied";
            if (element == null)
            {
                Users = Roles = Verbs = string.Empty;
                return;
            }

            this.AccessType = (long)element["accessType"];
            this.Users = (string)element["users"];
            this.Roles = (string)element["roles"];
            this.Verbs = (string)element["verbs"];
        }

        public string Verbs { get; set; }

        public string Roles { get; set; }

        public string Users { get; set; }

        public long AccessType { get; set; }

        public ConfigurationElement Element { get; set; }

        public string Flag { get; set; }

        public string UsersString
        {
            get
            {
                return Users == "*" ? "All Users" : Users == "?" ? "Anonymous Users" : Users;
            }
        }

        public bool Equals(AuthorizationRule other)
        {
            // all properties
            return Match(other) && other.AccessType == AccessType;
        }

        public void Apply()
        {
            Element["accessType"] = AccessType;
            Element["users"] = Users;
            Element["roles"] = Roles;
            Element["verbs"] = Verbs;
        }

        public bool Match(AuthorizationRule other)
        {
            // match combined keys.
            return other != null && other.Verbs == this.Verbs && other.Roles == this.Roles && other.Users == this.Users;
        }
    }
}

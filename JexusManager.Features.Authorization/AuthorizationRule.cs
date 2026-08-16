// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authorization
{
    using System;
    using Microsoft.Web.Administration;

    [Serializable]
    internal class AuthorizationRule : IItem<AuthorizationRule>
    {
        public AuthorizationRule(ConfigurationElement element = null) { Users = Roles = Verbs = string.Empty; Flag = "Local"; }

        public string Verbs { get; set; }

        public string Roles { get; set; }

        public string Users { get; set; }

        internal string OriginalKey { get; set; }

        public long AccessType { get; set; }

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

        public bool Match(AuthorizationRule other)
        {
            // match combined keys.
            return other != null && other.Verbs == this.Verbs && other.Roles == this.Roles && other.Users == this.Users;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public class VirtualDirectoryDefaults : ConfigurationElement
    {
        internal VirtualDirectoryDefaults(ConfigurationElement element, ConfigurationElement parent)
            : base(element, null, null, parent, null, null)
        {
        }

        public AuthenticationLogonMethod LogonMethod
        {
            get { return (AuthenticationLogonMethod)Enum.ToObject(typeof(AuthenticationLogonMethod), this["logonMethod"]); }
            set { this["logonMethod"] = (long)value; }
        }

        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        public string UserName
        {
            get { return (string)this["userName"]; }
            set { this["userName"] = value; }
        }
    }
}

﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public sealed class VirtualDirectory : ConfigurationElement, IItem<VirtualDirectory>
    {
        public override string ToString()
        {
            return Path;
        }

        public void Apply()
        {
        }

        public bool Match(VirtualDirectory other)
        {
            return Equals(other);
        }

        public bool Equals(VirtualDirectory other)
        {
            return other != null && string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);
        }

        public AuthenticationLogonMethod LogonMethod
        {
            get
            {
                return (AuthenticationLogonMethod)Enum.ToObject(typeof(AuthenticationLogonMethod), this["logonMethod"]);
            }

            set { this["logpnMethod"] = (long)value; }
        }

        public string Password
        {
            get { return this["password"].ObjectToString(); }
            set { this["password"] = value; }
        }

        public string Path
        {
            get { return this["path"].ToString(); }
            set { this["path"] = value; }
        }

        public string PhysicalPath
        {
            get { return this["physicalPath"].ToString(); }
            set { this["physicalPath"] = value; }
        }

        public string UserName
        {
            get { return this["userName"].ObjectToString(); }
            set { this["userName"] = value; }
        }

        internal static readonly string RootPath = "/";

        internal VirtualDirectory(ConfigurationElement element, VirtualDirectoryCollection parent)
            : base(element, "virtualDirectory", null, parent, null, null)
        {
            Parent = parent;
        }

        internal VirtualDirectoryCollection Parent { get; private set; }

        internal Application Application
        {
            get { return Parent.Parent; }
        }

        public string Flag { get; set; } = "Local";
        public ConfigurationElement Element
        {
            get { return this; }
            set { }
        }
    }
}

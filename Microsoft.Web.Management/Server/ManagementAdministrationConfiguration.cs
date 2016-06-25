// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementAdministrationConfiguration
    {
        public ManagementAdministrationConfiguration GetDelegatedScope(
            string path
            )
        { throw new NotImplementedException(); }

        public bool AllowUntrustedProviders { get; }
        public IDictionary<string, AdministrationModuleProvider> ModuleProviders { get; }
        public AdministrationModuleCollection Modules { get; }
        public string Path { get; }
        public ICollection TrustedProviders { get; }
    }
}
// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Server
{
    public sealed class ModuleInfo
    {
        public ModuleInfo(string name, string clientModuleTypeName)
            : this(name, clientModuleTypeName, null)
        {
        }

        public ModuleInfo(string name, string clientModuleTypeName, IDictionary arguments)
        {
            Name = name;
            ClientModuleTypeName = clientModuleTypeName;
            Arguments = arguments ?? new Dictionary<string, object>();
        }

        public IDictionary Arguments { get; }
        public string ClientModuleTypeName { get; }
        public string Name { get; }
    }
}

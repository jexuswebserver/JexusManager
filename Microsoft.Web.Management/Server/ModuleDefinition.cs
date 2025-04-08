﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;

namespace Microsoft.Web.Management.Server
{
    public sealed class ModuleDefinition
    {
        public ModuleDefinition(string name, string clientModuleTypeName)
        {
            Name = name;
            ClientModuleTypeName = clientModuleTypeName;
        }

        public IDictionary Arguments { get; }
        public string ClientModuleTypeName { get; }
        public string Name { get; }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public sealed class AdministrationModule
    {
        public AdministrationModule(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}

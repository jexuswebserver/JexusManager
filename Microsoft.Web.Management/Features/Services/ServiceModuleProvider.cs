// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Features.Services
{
    public sealed class ServiceModuleProvider : ModuleProvider

    {
        public override ModuleDefinition GetModuleDefinition(
    IManagementContext context
)
        { throw new NotImplementedException(); }

        public override bool SupportsScope(
    ManagementScope scope
)
        { return scope == ManagementScope.Server; }

        public override Type ServiceType { get { throw new NotImplementedException(); } }
        public override bool SupportsDelegation { get { throw new NotImplementedException(); } }
    }
}

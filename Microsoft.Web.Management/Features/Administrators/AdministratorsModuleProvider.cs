// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Features.Administrators
{
    public sealed class AdministratorsModuleProvider : ModuleProvider
    {
        public override ModuleDefinition GetModuleDefinition(
            IManagementContext context
            )
        {
            return null;
        }

        public override bool SupportsScope(
            ManagementScope scope
            )
        {
            return false;
        }

        public override string FriendlyName { get; }
        public override Type ServiceType { get; }
        public override bool SupportsDelegation { get; }
    }
}

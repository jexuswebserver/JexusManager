// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Certificates
{
    using System;

    using Microsoft.Web.Management.Server;

    public class CertificatesModuleProvider : ModuleProvider
    {
        public override Type ServiceType
        {
            get { return null; }
        }

        public override ModuleDefinition GetModuleDefinition(IManagementContext context)
        {
            return new ModuleDefinition(Name, typeof(CertificatesModule).AssemblyQualifiedName);
        }

        public override bool SupportsScope(ManagementScope scope)
        {
            return scope == ManagementScope.Server;
        }
    }
}

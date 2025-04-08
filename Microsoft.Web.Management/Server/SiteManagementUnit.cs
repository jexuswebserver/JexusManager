// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public sealed class SiteManagementUnit : ApplicationManagementUnit
    {
        public ApplicationManagementUnit GetApplication(
            string applicationPath
            )
        {
            return null;
        }

        public override ManagementScope Scope { get; }
    }
}

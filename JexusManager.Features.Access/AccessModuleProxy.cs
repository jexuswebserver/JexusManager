// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Access
{
    internal sealed class AccessModuleProxy : ModuleServiceProxy
    {
        internal AccessSnapshot GetSettings()
        {
            return (AccessSnapshot)Invoke(nameof(GetSettings));
        }

        internal void Apply(AccessSnapshot settings)
        {
            Invoke(nameof(Apply), settings);
        }
    }
}
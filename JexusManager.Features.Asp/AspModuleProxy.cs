// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Asp
{
    internal sealed class AspModuleProxy : ModuleServiceProxy
    {
        internal AspItem GetSettings()
        {
            return (AspItem)Invoke(nameof(GetSettings));
        }

        internal void Apply(AspItem settings)
        {
            Invoke(nameof(Apply), settings);
        }
    }
}
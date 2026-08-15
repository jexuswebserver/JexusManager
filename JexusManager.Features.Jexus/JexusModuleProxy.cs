// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Jexus
{
    internal sealed class JexusModuleProxy : ModuleServiceProxy
    {
        internal JexusSettings GetSettings()
        {
            return (JexusSettings)Invoke(nameof(GetSettings));
        }

        internal void Apply(string contents)
        {
            Invoke(nameof(Apply), contents);
        }
    }
}
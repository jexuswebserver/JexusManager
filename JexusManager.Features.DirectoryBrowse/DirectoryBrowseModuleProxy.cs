// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.DirectoryBrowse
{
    internal sealed class DirectoryBrowseModuleProxy : ModuleServiceProxy
    {
        internal DirectoryBrowseSnapshot GetSettings()
        {
            return (DirectoryBrowseSnapshot)Invoke(nameof(GetSettings));
        }

        internal void SetEnabled(bool enabled)
        {
            Invoke(nameof(SetEnabled), enabled);
        }

        internal void Apply(DirectoryBrowseSnapshot settings)
        {
            Invoke(nameof(Apply), settings);
        }
    }
}

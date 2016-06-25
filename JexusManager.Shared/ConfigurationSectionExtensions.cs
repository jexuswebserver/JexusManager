// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;

namespace JexusManager
{
    public static class ConfigurationSectionExtensions
    {
        public static bool CanRevert(this ConfigurationSection section)
        {
            return !string.IsNullOrEmpty(section.Location);
        }
    }
}

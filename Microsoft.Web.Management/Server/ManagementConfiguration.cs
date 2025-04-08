// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementConfiguration
    {
        public ConfigurationSection GetSection(
            string sectionPath
            )
        {
            return null;
        }

        public ConfigurationSection GetSection(
            string sectionPath,
            Type sectionType
            )
        {
            return null;
        }

        public ConfigurationSection GetSection(
            string sectionPath,
            ManagementConfigurationPath path,
            bool respectDelegation
            )
        {
            return null;
        }

        public ConfigurationSection GetSection(
            string sectionPath,
            Type sectionType,
            ManagementConfigurationPath path,
            bool respectDelegation
            )
        {
            return null;
        }
    }
}

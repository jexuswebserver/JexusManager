// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public sealed class ManagementConfiguration
    {
        private readonly Configuration _configuration;

        internal ManagementConfiguration(Configuration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public ConfigurationSection GetSection(
            string sectionPath
            )
        {
            return _configuration.GetSection(sectionPath);
        }

        public ConfigurationSection GetSection(
            string sectionPath,
            Type sectionType
            )
        {
            return _configuration.GetSection(sectionPath, sectionType);
        }

        public ConfigurationSection GetSection(
            string sectionPath,
            ManagementConfigurationPath path,
            bool respectDelegation
            )
        {
            return _configuration.GetSection(sectionPath);
        }

        public ConfigurationSection GetSection(
            string sectionPath,
            Type sectionType,
            ManagementConfigurationPath path,
            bool respectDelegation
            )
        {
            return _configuration.GetSection(sectionPath, sectionType);
        }
    }
}

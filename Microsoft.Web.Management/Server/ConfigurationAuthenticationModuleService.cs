// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public abstract class ConfigurationAuthenticationModuleService : ModuleService,
        IAuthenticationModuleService
    {
        protected ConfigurationAttribute GetEnabledProperty(
            ConfigurationElement element
            )
        {
            return null;
        }

        protected ConfigurationSection GetSection(
            Type type
            )
        {
            return null;
        }

        protected abstract string EnabledPropertyName { get; }
        protected abstract string SectionName { get; }

        bool IAuthenticationModuleService.IsEnabled()
        {
            throw new NotImplementedException();
        }
    }
}

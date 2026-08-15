// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public sealed class InProcessManagementUnit : ManagementUnit
    {
        public InProcessManagementUnit(
            IManagementContext context,
            ServerManager serverManager,
            Configuration configuration,
            ManagementScope scope,
            ManagementConfigurationPath configurationPath,
            IEnumerable<ModuleProvider> moduleProviders)
            : base(context, serverManager, configuration, configurationPath, moduleProviders)
        {
            Scope = scope;
        }

        protected override ManagementFrameworkVersion GetAssociatedFrameworkVersion()
        {
            return null;
        }

        public override object[] GetTypeInformation(string baseTypeName, bool includeNonpublicTypes, Type generatorType)
        {
            return Array.Empty<object>();
        }

        public override ManagementScope Scope { get; }
    }
}

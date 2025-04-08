// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.ApplicationHost;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public abstract class ManagementUnit
    {
        protected virtual WebConfigurationMap CreateConfigurationMap(
            bool addFrameworkConfiguration
            )
        { throw new NotImplementedException(); }

        protected abstract ManagementFrameworkVersion GetAssociatedFrameworkVersion();
        protected static ReadOnlyCollection<ManagementFrameworkVersion> GetFrameworkVersions(
            IManagementContext context
            )
        { throw new NotImplementedException(); }

        public ICollection<ModuleProvider> GetModuleProviders(
            Type moduleProviderType
            )
        { throw new NotImplementedException(); }

        public ModuleService GetModuleService(
            string moduleName
            )
        { throw new NotImplementedException(); }

        public Object[] GetTypeInformation(
            string baseTypeName
            )
        { throw new NotImplementedException(); }
        public Object[] GetTypeInformation(
            string baseTypeName,
            bool includeNonpublicTypes
            )
        { throw new NotImplementedException(); }

        public abstract Object[] GetTypeInformation(
            string baseTypeName,
            bool includeNonpublicTypes,
            Type generatorType
            );

        public void Update()
        { throw new NotImplementedException(); }

        public ManagementAdministrationConfiguration Administration { get; }
        public static AppHostFileProvider AppHostProvider { get; }
        public ManagementConfiguration Configuration { get; }
        public WebConfigurationMap ConfigurationMap { get; }
        public ManagementConfigurationPath ConfigurationPath { get; }
        public IManagementContext Context { get; }
        public static string CustomAppHostConfigPath { get; set; }
        public static bool DynamicRegistrationEnabled { get; }
        public ManagementFrameworkVersion FrameworkVersion { get; }
        public bool IsUserServerAdministrator { get; }
        public static List<IApplicationPool> ReadOnlyAppPools { get; }
        public static ServerManager ReadOnlyServerManager { get; }
        public static List<SiteInfo> ReadOnlySites { get; }
        public abstract ManagementScope Scope { get; }
        public ServerManager ServerManager { get; }
    }
}

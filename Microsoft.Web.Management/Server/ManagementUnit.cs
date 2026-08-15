// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.ApplicationHost;
using Microsoft.Web.Administration;

namespace Microsoft.Web.Management.Server
{
    public abstract class ManagementUnit
    {
        private readonly List<ModuleProvider> _moduleProviders = new List<ModuleProvider>();

        // Retained for the legacy IIS Manager-compatible derived types. Concrete
        // runtime units should use the fully scoped constructor below.
        protected ManagementUnit()
        {
        }

        protected ManagementUnit(
            IManagementContext context,
            ServerManager serverManager,
            Configuration configuration,
            ManagementConfigurationPath configurationPath,
            IEnumerable<ModuleProvider> moduleProviders)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            ServerManager = serverManager ?? throw new ArgumentNullException(nameof(serverManager));
            Configuration = new ManagementConfiguration(configuration ?? throw new ArgumentNullException(nameof(configuration)));
            ConfigurationPath = configurationPath ?? throw new ArgumentNullException(nameof(configurationPath));

            foreach (var provider in moduleProviders ?? Array.Empty<ModuleProvider>())
            {
                if (string.IsNullOrWhiteSpace(provider.Name))
                {
                    var name = provider.GetType().Name;
                    provider.Initialize(name.EndsWith("ModuleProvider", StringComparison.Ordinal)
                        ? name.Substring(0, name.Length - "ModuleProvider".Length)
                        : name);
                }

                provider.SetManagementUnit(this);
                _moduleProviders.Add(provider);
            }
        }

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
        {
            if (moduleProviderType == null)
            {
                throw new ArgumentNullException(nameof(moduleProviderType));
            }

            return _moduleProviders.Where(moduleProviderType.IsInstanceOfType).ToList();
        }

        public ModuleService GetModuleService(
            string moduleName
            )
        {
            var provider = _moduleProviders.FirstOrDefault(item =>
                string.Equals(item.Name, moduleName, StringComparison.OrdinalIgnoreCase));
            if (provider == null || !provider.SupportsScope(Scope))
            {
                throw new ModuleServiceException($"Module '{moduleName}' is not available at {Scope} scope.", "ModuleNotAvailable");
            }

            var serviceType = provider.ServiceType;
            if (serviceType == null || !typeof(ModuleService).IsAssignableFrom(serviceType) || serviceType.IsAbstract)
            {
                throw new ModuleServiceException($"Module '{moduleName}' does not expose a valid service.", "ServiceNotAvailable");
            }

            var service = (ModuleService)Activator.CreateInstance(serviceType, nonPublic: true);
            service.Initialize(this, provider.Name);
            return service;
        }

        public ICollection<ModuleProvider> ModuleProviders => _moduleProviders.AsReadOnly();

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
        {
            ServerManager.CommitChanges();
        }

        public ManagementAdministrationConfiguration Administration { get; }
        public static AppHostFileProvider AppHostProvider { get; }
        public ManagementConfiguration Configuration { get; }
        public WebConfigurationMap ConfigurationMap { get; }
        public ManagementConfigurationPath ConfigurationPath { get; }
        public IManagementContext Context { get; }
        public static string CustomAppHostConfigPath { get; set; }
        public static bool DynamicRegistrationEnabled { get; }
        public ManagementFrameworkVersion FrameworkVersion { get; }
        public bool IsUserServerAdministrator => Context.User?.Identity?.IsAuthenticated == true || Context.IsLocalConnection;
        public static List<IApplicationPool> ReadOnlyAppPools { get; }
        public ServerManager ReadOnlyServerManager => ServerManager;
        public static List<SiteInfo> ReadOnlySites { get; }
        public abstract ManagementScope Scope { get; }
        public ServerManager ServerManager { get; }
    }
}

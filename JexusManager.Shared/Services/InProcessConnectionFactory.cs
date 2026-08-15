// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Server;

namespace JexusManager.Services
{
    public static class InProcessConnectionFactory
    {
        public static IReadOnlyList<Module> InitializeModules(
            ServiceContainer services,
            IConfigurationService configurationService,
            IEnumerable<ModuleProvider> moduleProviders)
        {
            var providers = new List<ModuleProvider>(moduleProviders ?? Array.Empty<ModuleProvider>());
            var connection = Configure(services, configurationService, providers);
            var modules = new List<Module>();
            foreach (var provider in providers)
            {
                if (!provider.SupportsScope(configurationService.Scope))
                {
                    continue;
                }

                var definition = provider.GetModuleDefinition(null);
                var type = Type.GetType(definition.ClientModuleTypeName);
                if (type == null || !typeof(Module).IsAssignableFrom(type))
                {
                    continue;
                }

                var module = (Module)Activator.CreateInstance(type);
                var moduleInfo = (ModuleInfo)connection.Modules[definition.Name];
                module.Initialize(services, moduleInfo);
                modules.Add(module);
            }

            return modules;
        }

        public static Connection Configure(
            ServiceContainer services,
            IConfigurationService configurationService,
            IEnumerable<ModuleProvider> moduleProviders)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configurationService == null)
            {
                throw new ArgumentNullException(nameof(configurationService));
            }

            var serverManager = configurationService.ServerManager;
            var scope = configurationService.Scope;
            var application = configurationService.Application
                ?? configurationService.VirtualDirectory?.Application
                ?? configurationService.PhysicalDirectory?.Application;
            var siteName = configurationService.Site?.Name ?? application?.Site?.Name;
            var applicationPath = application?.Path;

            var configurationPath = scope switch
            {
                ManagementScope.Server => ManagementConfigurationPath.CreateServerConfigurationPath(),
                ManagementScope.Site => ManagementConfigurationPath.CreateSiteConfigurationPath(siteName),
                _ => ManagementConfigurationPath.CreateApplicationConfigurationPath(siteName, applicationPath)
            };

            var scopePath = scope switch
            {
                ManagementScope.Server => ManagementScopePath.CreateServerPath("localhost", 0),
                ManagementScope.Site => ManagementScopePath.CreateSitePath("localhost", 0, siteName),
                _ => ManagementScopePath.CreateApplicationPath("localhost", 0, siteName, applicationPath)
            };

            var connectionInfo = new ConnectionInfo(
                serverManager.Title,
                new Uri("inproc://localhost"),
                true,
                scope,
                scopePath,
                ConnectionCredential.CurrentUserAccount,
                null);
            var context = new ManagementContext(isLocalConnection: true);
            var providers = CreateProviderInstances(moduleProviders);
            var managementUnit = new InProcessManagementUnit(
                context,
                serverManager,
                configurationService.GetConfiguration(),
                scope,
                configurationPath,
                providers);
            var connection = Connection.CreateInProcess(connectionInfo, managementUnit);

            var previousConnection = services.GetService(typeof(Connection)) as Connection;
            services.RemoveService(typeof(Connection));
            (previousConnection as IDisposable)?.Dispose();
            services.AddService(typeof(Connection), connection);
            return connection;
        }

        private static IReadOnlyList<ModuleProvider> CreateProviderInstances(IEnumerable<ModuleProvider> moduleProviders)
        {
            var result = new List<ModuleProvider>();
            foreach (var provider in moduleProviders ?? Array.Empty<ModuleProvider>())
            {
                if (string.IsNullOrWhiteSpace(provider.Name))
                {
                    var name = provider.GetType().Name;
                    provider.Initialize(name.EndsWith("ModuleProvider", StringComparison.Ordinal)
                        ? name.Substring(0, name.Length - "ModuleProvider".Length)
                        : name);
                }

                var instance = (ModuleProvider)Activator.CreateInstance(provider.GetType(), nonPublic: true);
                instance.Initialize(provider.Name);
                result.Add(instance);
            }

            return result;
        }
    }
}

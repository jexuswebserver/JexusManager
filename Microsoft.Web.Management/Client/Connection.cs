// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Collections.Generic;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public sealed class Connection : IServiceContainer, IDisposable
    {
        private readonly ServiceContainer _services;
        private readonly ManagementChannel _channel;
        private readonly Dictionary<string, ModuleInfo> _modules;

        public Connection(ConnectionInfo connectionInfo, ManagementChannel channel)
        {
            ConnectionInfo = connectionInfo ?? throw new ArgumentNullException(nameof(connectionInfo));
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _services = new ServiceContainer();
            _services.AddService(typeof(Connection), this);
            _modules = new Dictionary<string, ModuleInfo>(StringComparer.OrdinalIgnoreCase);
        }

        public static Connection CreateInProcess(ConnectionInfo connectionInfo, InProcessManagementUnit managementUnit)
        {
            if (managementUnit == null)
            {
                throw new ArgumentNullException(nameof(managementUnit));
            }

            var connection = new Connection(connectionInfo, new InProcessManagementChannel(connectionInfo, managementUnit));
            foreach (var provider in managementUnit.ModuleProviders)
            {
                if (!provider.SupportsScope(managementUnit.Scope))
                {
                    continue;
                }

                var definition = provider.GetModuleDefinition(managementUnit.Context);
                connection.RegisterModule(new ModuleInfo(definition.Name, definition.ClientModuleTypeName, definition.Arguments));
            }

            return connection;
        }

        internal ConnectionInfo ConnectionInfo { get; }

        public void RegisterModule(ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                throw new ArgumentNullException(nameof(moduleInfo));
            }

            _modules[moduleInfo.Name] = moduleInfo;
        }

        public ModuleServiceProxy CreateProxy(Module module, Type proxyType)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            if (proxyType == null)
            {
                throw new ArgumentNullException(nameof(proxyType));
            }

            var serviceName = module.ModuleInfo?.Name;
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new InvalidOperationException("The module has not been initialized with module information.");
            }

            return CreateProxy(serviceName, proxyType);
        }

        public ModuleServiceProxy CreateProxy(string serviceName, Type proxyType)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("A module service name is required.", nameof(serviceName));
            }

            if (proxyType == null)
            {
                throw new ArgumentNullException(nameof(proxyType));
            }

            if (!typeof(ModuleServiceProxy).IsAssignableFrom(proxyType) || proxyType.IsAbstract)
            {
                throw new ArgumentException("The proxy type must be a concrete ModuleServiceProxy.", nameof(proxyType));
            }

            var proxy = (ModuleServiceProxy)Activator.CreateInstance(proxyType, nonPublic: true);
            proxy.Initialize(this, serviceName);
            return proxy;
        }

        internal object Invoke(string serviceName, string methodName, params object[] parameters)
        {
            return _channel.InvokeService(serviceName, methodName, parameters);
        }

        public bool EndConfigurationManagement()
        {
            return false;
        }

        public bool EndConfigurationManagement(Type pageType)
        {
            return false;
        }

        public bool Refresh()
        {
            return false;
        }

        public void SetConfigurationPath(ManagementConfigurationPath configurationPath)
        {
            ConfigurationPath = configurationPath;
        }

        public bool StartConfigurationManagement(ManagementConfigurationPath configurationPath)
        {
            return false;
        }

        public bool StartConfigurationManagement(ManagementConfigurationPath configurationPath, Type pageType)
        {
            return false;
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance)
        {
            _services.AddService(serviceType, serviceInstance);
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
        {
            _services.AddService(serviceType, serviceInstance, promote);
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            _services.AddService(serviceType, callback);
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            _services.AddService(serviceType, callback, promote);
        }

        void IServiceContainer.RemoveService(Type serviceType)
        {
            _services.RemoveService(serviceType);
        }

        void IServiceContainer.RemoveService(Type serviceType, bool promote)
        {
            _services.RemoveService(serviceType, promote);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return serviceType == typeof(Connection) ? this : _services.GetService(serviceType);
        }

        void IDisposable.Dispose()
        {
            _services.Dispose();
        }

        public ManagementConfigurationPath ConfigurationPath { get; set; }
        public ManagementFrameworkVersion CurrentFrameworkVersion { get; }
        public bool Deactivated { get; }
        public ReadOnlyCollection<ManagementFrameworkVersion> FrameworkVersions { get; }
        public bool IsLocalConnection => ConnectionInfo.IsLocal;
        public bool IsUserServerAdministrator { get; }
        public IDictionary Modules => _modules;
        public string Name => ConnectionInfo.Name;
        public ManagementScope Scope => ConnectionInfo.Scope;
        public ManagementScopePath ScopePath => ConnectionInfo.ScopePath;
        public Uri Url => ConnectionInfo.Url;
        public string UserName => ConnectionInfo.Credentials?.UserName;

        public event EventHandler Initialized;
    }
}

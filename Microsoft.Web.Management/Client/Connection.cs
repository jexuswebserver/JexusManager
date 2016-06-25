// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public sealed class Connection : IServiceContainer, IDisposable
    {
        public ModuleServiceProxy CreateProxy(Module module, Type proxyType)
        {
            return null;
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
            throw new NotImplementedException();
        }

        void IServiceContainer.AddService(Type serviceType, object serviceInstance, bool promote)
        {
            throw new NotImplementedException();
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback)
        {
            throw new NotImplementedException();
        }

        void IServiceContainer.AddService(Type serviceType, ServiceCreatorCallback callback, bool promote)
        {
            throw new NotImplementedException();
        }

        void IServiceContainer.RemoveService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        void IServiceContainer.RemoveService(Type serviceType, bool promote)
        {
            throw new NotImplementedException();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
        }

        public ManagementConfigurationPath ConfigurationPath { get; set; }
        public ManagementFrameworkVersion CurrentFrameworkVersion { get; }
        public bool Deactivated { get; }
        public ReadOnlyCollection<ManagementFrameworkVersion> FrameworkVersions { get; }
        public bool IsLocalConnection { get; }
        public bool IsUserServerAdministrator { get; }
        public IDictionary Modules { get; }
        public string Name { get; }
        public ManagementScope Scope { get; }
        public ManagementScopePath ScopePath { get; }
        public Uri Url { get; }
        public string UserName { get; }

        public event EventHandler Initialized;
    }
}
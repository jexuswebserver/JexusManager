// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public abstract class Module : IServiceProvider, IDisposable
    {
        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private IServiceProvider _serviceProvider;

        protected virtual void Dispose()
        {
            _serviceProvider = null;
            ModuleInfo = null;
        }

        protected virtual object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }

        internal protected virtual void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            _serviceProvider = serviceProvider;
            ModuleInfo = moduleInfo;
        }

        internal void TestInitialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            Initialize(serviceProvider, moduleInfo);
        }

        protected virtual bool IsPageEnabled(ModulePageInfo pageInfo)
        {
            return pageInfo.IsEnabled;
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            return GetService(serviceType);
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public ModuleInfo ModuleInfo { get; private set; }

        public virtual TaskListCollection Tasks
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}

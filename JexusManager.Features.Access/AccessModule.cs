// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Access
{
    using System;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class AccessModule : Module
    {
        private AccessModuleProxy _proxy;

        internal AccessModuleProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    var connection = (Connection)GetService(typeof(Connection));
                    if (connection == null)
                    {
                        throw new InvalidOperationException("SSL Settings requires an active management connection.");
                    }

                    _proxy = (AccessModuleProxy)connection.CreateProxy(this, typeof(AccessModuleProxy));
                }

                return _proxy;
            }
        }

        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)this.GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(AccessPage), "SSL Settings",
                "Specify requirements for SSL and client certificates.",
                Resources.access_36, Resources.access_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

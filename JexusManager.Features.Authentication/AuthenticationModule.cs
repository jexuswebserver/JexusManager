// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class AuthenticationModule : Module
    {
        private AuthenticationModuleProxy _proxy;

        internal AuthenticationModuleProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    var connection = (Connection)GetService(typeof(Connection));
                    if (connection == null)
                    {
                        throw new InvalidOperationException("Authentication requires an active management connection.");
                    }

                    _proxy = (AuthenticationModuleProxy)connection.CreateProxy(this, typeof(AuthenticationModuleProxy));
                }

                return _proxy;
            }
        }

        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(AuthenticationPage), "Authentication", "Configure authentication settings for sites and applications", Resources.authentication_36, Resources.authentication_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

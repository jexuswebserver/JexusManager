// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Asp
{
    using System;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class AspModule : Module
    {
        private AspModuleProxy _proxy;

        internal AspModuleProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    var connection = (Connection)GetService(typeof(Connection));
                    if (connection == null)
                    {
                        throw new InvalidOperationException("ASP requires an active management connection.");
                    }

                    _proxy = (AspModuleProxy)connection.CreateProxy(this, typeof(AspModuleProxy));
                }

                return _proxy;
            }
        }

        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)this.GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(
                this,
                typeof(AspPage),
                "ASP",
                "Configure properties for ASP applications",
                Resources.asp_36,
                Resources.asp_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

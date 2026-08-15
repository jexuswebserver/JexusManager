// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class FastCgiModule : Module
    {
        private FastCgiModuleProxy _proxy;
        internal FastCgiModuleProxy Proxy => _proxy ??= (FastCgiModuleProxy)((Connection)GetService(typeof(Connection))).CreateProxy(this, typeof(FastCgiModuleProxy));
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(FastCgiPage), "FastCGI Settings",
                "Configure FastCGI process application pools", Resources.response_header_36,
                Resources.response_header_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

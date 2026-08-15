// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Caching
{
    using System;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class CachingModule : Module
    {
        private CachingModuleProxy _proxy;
        internal CachingModuleProxy Proxy => _proxy ??= (CachingModuleProxy)((Connection)GetService(typeof(Connection))).CreateProxy(this, typeof(CachingModuleProxy));
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(
                this,
                typeof(CachingPage),
                "Output Caching",
                "Specify rules for caching served content in the output cache",
                Resources.caching_36,
                Resources.caching_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

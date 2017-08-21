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
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)this.GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(
                this,
                typeof(AspPage),
                "ASP",
                "Configure properties for ASP applications",
                Resources.cgi_36,
                Resources.cgi_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

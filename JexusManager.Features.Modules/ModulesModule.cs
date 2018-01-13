// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class ModulesModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(ModulesPage), "Modules",
                "Configure native and managed code modules that process requests on the Web server", Resources.modules_36,
                Resources.modules_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

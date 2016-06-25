// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class HttpErrorsModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)this.GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(HttpErrorsPage), "Error Pages",
                "Configure pages to return when errors occur", Resources.custom_error_36,
                Resources.custom_error_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

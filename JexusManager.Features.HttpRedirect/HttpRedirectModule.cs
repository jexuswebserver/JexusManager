// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpRedirect
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class HttpRedirectModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)this.GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(
                this,
                typeof(HttpRedirectPage),
                "HTTP Redirect",
                "Specify rules for redirecting incoming requests to another file or URL",
                Resources.http_redirect_36,
                Resources.http_redirect_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

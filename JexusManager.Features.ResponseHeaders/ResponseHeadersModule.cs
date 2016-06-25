// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.ResponseHeaders
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class ResponseHeadersModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(ResponseHeadersPage), "HTTP Response Headers",
                "Configure HTTP headers that are added to responses from the Web server", Resources.response_header_36,
                Resources.response_header_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

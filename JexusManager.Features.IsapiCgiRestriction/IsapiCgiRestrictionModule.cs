// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiCgiRestriction
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class IsapiCgiRestrictionModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(IsapiCgiRestrictionPage), "ISAPI and CGI Restrictions",
                "Restrict or enable ISAPI and CGI extensions on the Web server", Resources.isapi_cgi_restriction_36,
                Resources.isapi_cgi_restriction_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

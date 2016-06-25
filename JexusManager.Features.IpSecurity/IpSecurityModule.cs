// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class IpSecurityModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(IpSecurityPage), "IP Address and Domain Restrictions",
                "Restrict or grant access to Web content based on IP addresses or domain names", Resources.ip_restriction_36,
                Resources.ip_restriction_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

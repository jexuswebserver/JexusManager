// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authorization
{
    using System;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class AuthorizationModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)this.GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(AuthorizationPage), "Authorization Rules",
                "Configure rules for authorizing users to access websites and applications", Resources.authorization_36,
                Resources.authorization_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

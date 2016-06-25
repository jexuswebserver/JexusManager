// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class RewriteModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(RewritePage), "URL Rewrite",
                "Provide URL and content rewriting capabilities based on rules", Resources.url_rewrite_36,
                Resources.url_rewrite_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

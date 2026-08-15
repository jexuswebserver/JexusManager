// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class RequestFilteringModule : Module
    {
        private RequestFilteringModuleProxy _proxy;

        internal RequestFilteringModuleProxy Proxy
        {
            get
            {
                return _proxy ??= (RequestFilteringModuleProxy)((Connection)GetService(typeof(Connection))).CreateProxy(this, typeof(RequestFilteringModuleProxy));
            }
        }

        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(RequestFilteringPage), "Request Filtering",
                "Use this feature to configure filtering rules", Resources.request_filtering_36,
                Resources.request_filtering_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

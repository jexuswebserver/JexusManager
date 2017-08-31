// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.TraceFailedRequests
{
    using System;

    using Properties;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal class TraceFailedRequestsModule : Module
    {
        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(this, typeof(TraceFailedRequestsPage), "Failed Request Tracing Rules",
                "Configure logging of failed request traces", Resources.trace_failed_requests_36,
                Resources.trace_failed_requests_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

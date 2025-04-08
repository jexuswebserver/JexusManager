// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Microsoft.Web.Management.Client.Extensions
{
    public abstract class ProtocolProvider : IServiceProvider
    {
        protected ProtocolProvider(
            IServiceProvider serviceProvider
            )
        { }

        public abstract TaskList GetSitesTaskList();

        public abstract TaskList GetSiteTaskList(
            string siteName,
            ICollection<string> bindingProtocols
            );

        public void UpdateSite(
            string siteName,
            SiteAction action
            )
        { }

        object IServiceProvider.GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsIPPortProtocol { get; }
        public SiteUpdatedEventHandler SiteUpdated { get; set; }
        public virtual string SupportedProtocol { get; }

        public delegate void SiteUpdatedEventHandler(
            Object sender,
            SiteUpdatedEventArgs e
            );
    }
}

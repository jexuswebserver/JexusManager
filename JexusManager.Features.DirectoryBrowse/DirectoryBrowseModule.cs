// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.DirectoryBrowse
{
    using System;

    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;
    using Properties;

    internal class DirectoryBrowseModule : Module
    {
        private DirectoryBrowseModuleProxy _proxy;

        internal DirectoryBrowseModuleProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    var connection = (Connection)GetService(typeof(Connection));
                    if (connection == null)
                    {
                        throw new InvalidOperationException("Directory Browsing requires an active management connection.");
                    }

                    _proxy = (DirectoryBrowseModuleProxy)connection.CreateProxy(this, typeof(DirectoryBrowseModuleProxy));
                }

                return _proxy;
            }
        }

        protected override void Initialize(IServiceProvider serviceProvider, ModuleInfo moduleInfo)
        {
            base.Initialize(serviceProvider, moduleInfo);
            var controlPanel = (IControlPanel)this.GetService(typeof(IControlPanel));
            var modulePage = new ModulePageInfo(
                this,
                typeof(DirectoryBrowsePage),
                "Directory Browsing",
                "Configure information to display in a directory listing",
                Resources.directory_browsing_36,
                Resources.directory_browsing_36);
            controlPanel.RegisterPage(modulePage);
        }
    }
}

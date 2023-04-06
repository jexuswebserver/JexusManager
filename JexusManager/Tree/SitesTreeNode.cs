// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Tree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using JexusManager.Features.Main;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal sealed class SitesTreeNode : ManagerTreeNode
    {
        public SitesTreeNode(IServiceProvider serviceProvider, SiteCollection sites, ServerTreeNode server)
            : base("Sites", serviceProvider)
        {
            ImageIndex = 3;
            SelectedImageIndex = 3;
            Tag = sites;
            ServerManager = sites.Parent;
            ServerNode = server;
        }

        public override string PathToSite
        {
            get { return string.Empty; }
        }

        public override string Folder
        {
            get { return string.Empty; }
        }

        public override string Uri
        {
            get { return string.Empty; }
        }

        public override ServerManager ServerManager { get; set; }

        public override ServerTreeNode ServerNode { get; }

        public override void LoadPanels(MainForm mainForm, ServiceContainer serviceContainer, List<ModuleProvider> moduleProviders)
        {
            serviceContainer.RemoveService(typeof(IConfigurationService));
            serviceContainer.RemoveService(typeof(IControlPanel));
            var sites = (SiteCollection)Tag;
            var scope = ManagementScope.Server;
            serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(mainForm, null, scope, sites.Parent, null, null, null, null, null));
            IModulePage page = new SitesPage(mainForm);
            var mainModule = new MainModule();
            mainModule.Initialize(serviceContainer, null);
            page.Initialize(mainModule, null, null);
            mainForm.LoadPage(page);
        }

        public override void HandleDoubleClick()
        {
        }

        public override void Expand(MainForm mainForm)
        { }

        public override void AddApplication(ContextMenuStrip appMenu)
        {
        }

        public override void AddVirtualDirectory(ContextMenuStrip vDirMenu)
        {
        }
    }
}

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

    using JexusManager.Dialogs;
    using JexusManager.Features.Main;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal sealed class VirtualDirectoryTreeNode : ManagerTreeNode
    {
        private bool _loaded;

        public VirtualDirectoryTreeNode(IServiceProvider serviceProvider, VirtualDirectory virtualDirectory)
            : base(virtualDirectory.Path.PathToName(), serviceProvider)
        {
            ImageIndex = 7;
            SelectedImageIndex = 7;
            Tag = virtualDirectory;
            VirtualDirectory = virtualDirectory;
            Nodes.Add("temp");
            ServerManager = virtualDirectory.Application.Server;
        }

        public VirtualDirectory VirtualDirectory { get; }

        public override string PathToSite
        {
            get { return VirtualDirectory.PathToSite(); }
        }

        public override string Uri
        {
            get
            {
                return VirtualDirectory.Application.Site.Bindings[0].ToUri() + PathToSite;
            }
        }

        public override ServerManager ServerManager { get; set; }

        public override string Folder
        {
            get
            {
                return VirtualDirectory.PhysicalPath.ExpandIisExpressEnvironmentVariables();
            }
        }

        public override void LoadPanels(MainForm mainForm, ServiceContainer serviceContainer, List<ModuleProvider> moduleProviders)
        {
            serviceContainer.RemoveService(typeof(IConfigurationService));
            serviceContainer.RemoveService(typeof(IControlPanel));
            var panel = new VirtualDirectoryPage(VirtualDirectory, mainForm);
            var scope = ManagementScope.Application;
            serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(mainForm, VirtualDirectory.Application.GetWebConfiguration(), scope, null,
                    null, null, VirtualDirectory, null, VirtualDirectory.Application.Site.Name + PathToSite));
            foreach (var provider in moduleProviders)
            {
                if (!provider.SupportsScope(scope))
                {
                    continue;
                }

                var definition = provider.GetModuleDefinition(null);
                var type = Type.GetType(definition.ClientModuleTypeName);
                if (type == null)
                {
                    continue;
                }

                if (!typeof(Module).IsAssignableFrom(type))
                {
                    continue;
                }

                var module = (Module)Activator.CreateInstance(type);
                module.Initialize(serviceContainer, null);
            }

            IModulePage page = panel;
            var mainModule = new MainModule();
            mainModule.Initialize(serviceContainer, null);
            page.Initialize(mainModule, null, null);
            mainForm.LoadPage(page);
        }

        public async override Task Expand(MainForm mainForm)
        {
            if (_loaded)
            {
                return;
            }

            Nodes.Clear();
            var rootFolder = VirtualDirectory.PhysicalPath.ExpandIisExpressEnvironmentVariables();
            var rootLevel = GetLevel(PathToSite);
            LoadChildren(VirtualDirectory.Application, rootLevel, rootFolder, PathToSite, mainForm.PhysicalDirectoryMenu, mainForm.VirtualDirectoryMenu, mainForm.ApplicationMenu);
            _loaded = true;
        }

        public override async Task AddApplication(ContextMenuStrip appMenu)
        {
            var dialog = new NewApplicationDialog(ServiceProvider, VirtualDirectory.Application.Site, PathToSite, VirtualDirectory.Application.ApplicationPoolName, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dialog.Application.Save();
            AddToParent(this, new ApplicationTreeNode(ServiceProvider, dialog.Application) { ContextMenuStrip = appMenu });
        }

        public override void AddVirtualDirectory(ContextMenuStrip vDirMenu)
        {
            var dialog = new NewVirtualDirectoryDialog(ServiceProvider, null, PathToSite, VirtualDirectory.Application);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //await dialog.VirtualDirectory.SaveAsync();
            AddToParent(this, new VirtualDirectoryTreeNode(ServiceProvider, dialog.VirtualDirectory) { ContextMenuStrip = vDirMenu });
        }

        public async override Task HandleDoubleClick(MainForm mainForm)
        { }
    }
}

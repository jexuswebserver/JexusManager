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

    internal sealed class PhysicalDirectoryTreeNode : ManagerTreeNode
    {
        private bool _loaded;

        public PhysicalDirectoryTreeNode(IServiceProvider serviceProvider, PhysicalDirectory physicalDirectory, ServerTreeNode server)
            : base(physicalDirectory.Name, serviceProvider)
        {
            ImageIndex = 6;
            SelectedImageIndex = 6;
            PhysicalDirectory = physicalDirectory;
            Nodes.Add("temp");
            ServerManager = physicalDirectory.Application.Server;
            ServerNode = server;
        }

        public PhysicalDirectory PhysicalDirectory { get; }

        public override string PathToSite
        {
            get { return PhysicalDirectory.PathToSite; }
        }

        public override string Folder
        {
            get { return PhysicalDirectory.FullName; }
        }

        public override string Uri
        {
            get
            {
                foreach (Microsoft.Web.Administration.Binding binding in PhysicalDirectory.Application.Site.Bindings)
                {
                    if (binding.CanBrowse)
                    {
                        return binding.ToUri() + PathToSite;
                    }
                }

                return string.Empty;
            }
        }

        public override ServerManager ServerManager { get; set; }

        public override ServerTreeNode ServerNode { get; }

        public override void LoadPanels(MainForm mainForm, ServiceContainer serviceContainer, List<ModuleProvider> moduleProviders)
        {
            serviceContainer.RemoveService(typeof(IConfigurationService));
            serviceContainer.RemoveService(typeof(IControlPanel));
            var panel = new PhysicalDirectoryPage(PhysicalDirectory, mainForm);
            var scope = ManagementScope.Application;
            serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(mainForm, PhysicalDirectory.Application.GetWebConfiguration(), scope, null,
                    null, null, null, PhysicalDirectory, PhysicalDirectory.Application.Site.Name + PhysicalDirectory.PathToSite));
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
            var rootFolder = PhysicalDirectory.FullName;
            var rootLevel = GetLevel(PathToSite);
            LoadChildren(PhysicalDirectory.Application, rootLevel, rootFolder, PathToSite, mainForm.PhysicalDirectoryMenu, mainForm.VirtualDirectoryMenu, mainForm.ApplicationMenu);
            _loaded = true;
        }

        public override async Task AddApplication(ContextMenuStrip appMenu)
        {
            var dialog = new NewApplicationDialog(ServiceProvider, PhysicalDirectory.Application.Site, PathToSite, PhysicalDirectory.Application.ApplicationPoolName, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dialog.Application.Save();
            AddToParent(this, new ApplicationTreeNode(ServiceProvider, dialog.Application, this.ServerNode) { ContextMenuStrip = appMenu });
        }

        public override void AddVirtualDirectory(ContextMenuStrip vDirMenu)
        {
            var dialog = new NewVirtualDirectoryDialog(ServiceProvider, null, PathToSite, PhysicalDirectory.Application);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //await dialog.VirtualDirectory.SaveAsync();
            AddToParent(this, new VirtualDirectoryTreeNode(ServiceProvider, dialog.VirtualDirectory, this.ServerNode) { ContextMenuStrip = vDirMenu });
        }

        public async override Task HandleDoubleClick(MainForm mainForm)
        { }
    }
}

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

    using Application = Microsoft.Web.Administration.Application;

    internal sealed class ApplicationTreeNode : ManagerTreeNode
    {
        private bool _loaded;

        public ApplicationTreeNode(IServiceProvider serviceProvider, Application application, ServerTreeNode server)
            : base(application.Path.Substring(application.Path.LastIndexOf('/') + 1), serviceProvider)
        {
            ImageIndex = 5;
            SelectedImageIndex = 5;
            Tag = application;
            Application = application;
            Nodes.Add("temp");
            ServerManager = application.Server;
            ServerNode = server;
        }

        public Application Application { get; }

        public override string PathToSite
        {
            get { return Application.Path; }
        }

        public override string Folder
        {
            get
            {
                return Application.PhysicalPath.ExpandIisExpressEnvironmentVariables();
            }
        }

        public override string Uri
        {
            get
            {
                foreach (Microsoft.Web.Administration.Binding binding in Application.Site.Bindings)
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
            var panel = new ApplicationPage(Application, mainForm);
            var scope = ManagementScope.Application;
            serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(mainForm, Application.GetWebConfiguration(), scope, null, Application.Site,
                    Application, null, null, this.Application.Location));
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

        public override void Expand(MainForm mainForm)
        {
            if (_loaded)
            {
                return;
            }

            _loaded = true;
            Nodes.Clear();
            var rootFolder = Application.PhysicalPath.ExpandIisExpressEnvironmentVariables();
            var rootLevel = GetLevel(Application.Path);
            LoadChildren(Application, rootLevel, rootFolder, Application.Path, mainForm.PhysicalDirectoryMenu, mainForm.VirtualDirectoryMenu, mainForm.ApplicationMenu);
        }

        public override void AddApplication(ContextMenuStrip appMenu)
        {
            var dialog = new NewApplicationDialog(ServiceProvider, Application.Site, PathToSite, Application.ApplicationPoolName, null);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            dialog.Application.Save();
            AddToParent(this, new ApplicationTreeNode(ServiceProvider, dialog.Application, ServerNode) { ContextMenuStrip = appMenu });
        }

        public override void AddVirtualDirectory(ContextMenuStrip vDirMenu)
        {
            var dialog = new NewVirtualDirectoryDialog(ServiceProvider, null, PathToSite, Application);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //await dialog.VirtualDirectory.SaveAsync();
            AddToParent(this, new VirtualDirectoryTreeNode(ServiceProvider, dialog.VirtualDirectory, ServerNode) { ContextMenuStrip = vDirMenu });
        }

        public override void HandleDoubleClick(MainForm mainForm)
        { }
    }
}

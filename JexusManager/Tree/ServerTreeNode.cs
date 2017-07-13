// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Tree
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Net;
    using System.Net.Security;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using JexusManager.Dialogs;
    using JexusManager.Features.Main;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Server;

    internal sealed class ServerTreeNode : ManagerTreeNode
    {
        private enum NodeStatus
        {
            Default = 0,
            Loading = 1,
            Loaded = 2
        }

        public string DisplayName;
        public string HostName;
        public string CertificateHash;
        public string Credentials;
        public bool Ignore;

        public TreeNode PoolsNode => Nodes[0];

        public TreeNode SitesNode => Nodes[1];

        public bool IsLocalhost;
        public WorkingMode Mode;
        private NodeStatus _status;

        public ServerTreeNode(IServiceProvider serviceProvider, string name, string hostName, string credentials, string hash, ServerManager server, bool isLocalhost, WorkingMode mode, bool ignore)
            : base(GetNodeName(name, credentials, isLocalhost), serviceProvider)
        {
            ImageIndex = 1;
            SelectedImageIndex = 1;
            Tag = server;
            ServerManager = server;
            DisplayName = name;
            HostName = hostName;
            Credentials = credentials;
            Mode = mode;
            IsLocalhost = isLocalhost;
            CertificateHash = hash;
            Ignore = ignore;

            Handler = (sender1, certificate, chain, sslPolicyErrors) =>
                {
                    var remoteHash = certificate.GetCertHashString();
                    if (remoteHash == this.CertificateHash)
                    {
                        return true;
                    }

                    var dialog = new CertificateErrorsDialog(certificate);
                    var result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        this.MainForm.SaveMenuItem.Enabled = true;
                    }

                    if (result != DialogResult.OK)
                    {
                        return false;
                    }

                    this.CertificateHash = remoteHash;
                    return true;
                };
        }

        public bool IsBusy => _status == NodeStatus.Loading;

        public override async Task HandleDoubleClick(MainForm mainForm)
        {
            if (ServerManager != null)
            {
                return;
            }

            if (_status == NodeStatus.Loading)
            {
                return;
            }

            MainForm = mainForm;
            mainForm.DisconnectButton.Enabled = false;
            _status = NodeStatus.Loading;
            mainForm.ShowInfo($"Connecting to {HostName}...");
            try
            {
                if (Mode == WorkingMode.IisExpress)
                {
                    ServerManager = new IisExpressServerManager(HostName);
                }
                else if (Mode == WorkingMode.Iis)
                {
                    ServerManager = new IisServerManager(false, this.HostName);
                }
                else
                {
                    ServerManager = new JexusServerManager(HostName, Credentials);
                }

                ServerManager.Name = this.DisplayName;

                if (this.Mode == WorkingMode.Jexus)
                {
                    this.SetHandler();
                    var server = (JexusServerManager)ServerManager;
                    var version = await server.GetVersionAsync();
                    if (version == null)
                    {
                        MessageBox.Show("Authentication failed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ServerManager = null;
                        _status = NodeStatus.Default;
                        mainForm.DisconnectButton.Enabled = true;
                        return;
                    }

                    if (version < JexusServerManager.MinimumServerVersion)
                    {
                        var toContinue =
                            MessageBox.Show(
                                $"The server version is {version}, while minimum compatible version is {JexusServerManager.MinimumServerVersion}. Making changes might corrupt server configuration. Do you want to continue?",
                                Text,
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question);
                        if (toContinue != DialogResult.Yes)
                        {
                            ServerManager = null;
                            _status = NodeStatus.Default;
                            mainForm.DisconnectButton.Enabled = true;
                            return;
                        }
                    }

                    var conflict = await server.HelloAsync();
                    if (Environment.MachineName != conflict)
                    {
                        MessageBox.Show(
                            $"The server is also connected to {conflict}. Making changes on multiple clients might corrupt server configuration.",
                            Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }

                ServerManager.IsLocalhost = IsLocalhost;
                await LoadServerAsync(mainForm.ApplicationPoolsMenu, mainForm.SitesMenu, mainForm.SiteMenu);
                Tag = ServerManager;
                mainForm.EnableServerMenuItems(true);
                _status = NodeStatus.Loaded;
                mainForm.DisconnectButton.Enabled = true;
            }
            catch (Exception ex)
            {
                File.WriteAllText(DialogHelper.DebugLog, ex.ToString());
                var last = ex;
                while (last is AggregateException)
                {
                    last = last.InnerException;
                }

                var message = new StringBuilder();
                message.AppendLine("Could not connect to the specified computer.")
                    .AppendLine()
                    .AppendFormat("Details: {0}", last?.Message);
                MessageBox.Show(message.ToString(), mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ServerManager = null;
                _status = NodeStatus.Default;
                mainForm.DisconnectButton.Enabled = true;
            }
            finally
            {
                mainForm.HideInfo();
            }
        }

        public MainForm MainForm { get; set; }

        private static RemoteCertificateValidationCallback Handler { get; set; }

        public override string PathToSite => string.Empty;

        public override string Folder => string.Empty;

        public override string Uri => throw new NotImplementedException();

        public override ServerManager ServerManager { get; set; }

        public override void LoadPanels(MainForm mainForm, ServiceContainer serviceContainer, List<ModuleProvider> moduleProviders)
        {
            if (ServerManager == null && _status != NodeStatus.Loaded)
            {
                return;
            }

            serviceContainer.RemoveService(typeof(IConfigurationService));
            serviceContainer.RemoveService(typeof(IControlPanel));
            var server = (ServerManager)Tag;
            var panel = new ServerPage(server);
            var scope = ManagementScope.Server;
            serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(mainForm, server.GetApplicationHostConfiguration(), scope, server, null, null,
                    null, null, null));

            var moduleDefinitions = new List<ModuleDefinition>();
            var modules = new List<Module>();
            foreach (var provider in moduleProviders)
            {
                if (!provider.SupportsScope(scope))
                {
                    continue;
                }

                var definition = provider.GetModuleDefinition(null);
                moduleDefinitions.Add(definition);
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
                modules.Add(module);
            }

            IModulePage page = panel;
            var mainModule = new MainModule();
            mainModule.Initialize(serviceContainer, null);
            page.Initialize(mainModule, null, null);
            mainForm.LoadPage(page);
        }

        internal static string GetNodeName(string name, string credentials, bool isLocalhost)
        {
            return isLocalhost
                ? $"{name} ({Environment.UserDomainName}\\{Environment.UserName})"
                : $"{name} ({credentials.ExtractUser()})";
        }

        public async override Task Expand(MainForm mainForm)
        { }

        public async override Task AddApplication(ContextMenuStrip appMenu)
        {
        }

        public override void AddVirtualDirectory(ContextMenuStrip vDirMenu)
        {
        }

        public async Task<bool> LoadServerAsync(ContextMenuStrip poolsMenu, ContextMenuStrip sitesMenu, ContextMenuStrip siteMenu)
        {
            Nodes.Add(new ApplicationPoolsTreeNode(ServiceProvider, ServerManager.ApplicationPools)
            {
                ContextMenuStrip = poolsMenu
            });
            Nodes.Add(new SitesTreeNode(ServiceProvider, ServerManager.Sites)
            {
                ContextMenuStrip = sitesMenu
            });
            TreeView.SelectedNode = this;

            foreach (Site site in ServerManager.Sites)
            {
                var siteNode = new SiteTreeNode(ServiceProvider, site) { ContextMenuStrip = siteMenu };
                SitesNode.Nodes.Add(siteNode);
            }

            // TODO: re-enable web farm support.
            //data.FarmNode = data.ServerNode.Nodes.Add("Server Farms");
            //data.FarmNode.ContextMenuStrip = cmsFarm;
            //data.FarmNode.ImageIndex = 7;
            //data.FarmNode.SelectedImageIndex = 7;
            //data.FarmNode.Tag = data.Server;

            return true;
        }

        public void SetHandler()
        {
            ServicePointManager.ServerCertificateValidationCallback = Handler;
        }
    }
}

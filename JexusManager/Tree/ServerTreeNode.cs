// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;

using JexusManager.Dialogs;
using JexusManager.Features.Main;
using JexusManager.Services;

using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Server;
using Rollbar;

namespace JexusManager.Tree
{
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
        public bool IgnoreInCache;

        public TreeNode PoolsNode { get; private set; }

        public TreeNode SitesNode { get; private set; }

        public override ServerTreeNode ServerNode => this;

        public bool IsLocalhost;
        public WorkingMode Mode;
        private NodeStatus _status;
        private bool readOnly;

        private static string[] RestrictedPaths = new[]
                {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) + Path.DirectorySeparatorChar,
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)) + Path.DirectorySeparatorChar,
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) + Path.DirectorySeparatorChar
                };

        private ServerTreeNode(IServiceProvider serviceProvider, string name, string hostName, string credentials, string hash, ServerManager server, bool isLocalhost, WorkingMode mode, bool ignoreInCache)
            : base(GetNodeName(name, credentials, isLocalhost), serviceProvider)
        {
            Tag = server;
            ServerManager = server;
            DisplayName = name;
            HostName = hostName;
            Credentials = credentials;
            Mode = mode;
            var elevated = PublicNativeMethods.IsProcessElevated;
            if (Mode == WorkingMode.Iis)
            {
                if (elevated)
                {
                    ImageIndex = 8;
                    SelectedImageIndex = 8;
                }
                else
                {
                    ImageIndex = 12;
                    SelectedImageIndex = 12;
                    readOnly = true;
                }
            }
            else
            {
                var restricted = false;
                foreach (var restrictedPath in RestrictedPaths)
                {
                    if (hostName.StartsWith(restrictedPath, StringComparison.OrdinalIgnoreCase))
                    {
                        restricted = true;
                        break;
                    }
                }

                if (restricted && !elevated)
                {
                    ImageIndex = 11;
                    SelectedImageIndex = 11;
                    readOnly = true;
                }
                else
                {
                    ImageIndex = 1;
                    SelectedImageIndex = 1;
                }
            }

            IsLocalhost = isLocalhost;
            CertificateHash = hash;
            IgnoreInCache = ignoreInCache;

            if (mode == WorkingMode.Jexus)
            {
                ((JexusServerManager)server).ServerCertificateValidationCallback = (sender1, certificate, chain, sslPolicyErrors) =>
                    {
                        var remoteHash = certificate.GetCertHashString();
                        if (remoteHash == CertificateHash)
                        {
                            return true;
                        }

                        using (var dialog = new CertificateErrorsDialog(certificate))
                        {
                            var result = dialog.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                MainForm.SaveMenuItem.Enabled = true;
                            }

                            if (result != DialogResult.OK)
                            {
                                return false;
                            }
                        }

                        CertificateHash = remoteHash;
                        return true;
                    };
            }
        }

        public bool IsBusy => _status == NodeStatus.Loading;

        public override void HandleDoubleClick(MainForm mainForm)
        {
            if (readOnly)
            {
                MessageBox.Show("Elevation is required. Please run Jexus Manager as administrator.");
                return;
            }

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
                    ServerManager = new IisServerManager(false, HostName);
                }
                else
                {
                    ServerManager = new JexusServerManager(HostName, Credentials);
                }

                ServerManager.Name = DisplayName;

                if (Mode == WorkingMode.Jexus)
                {
                    var server = (JexusServerManager)ServerManager;
                    var version = AsyncHelper.RunSync(() => server.GetVersionAsync());
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

                    var conflict = AsyncHelper.RunSync(() => server.HelloAsync());
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
                var succeeded = LoadServer(mainForm.ApplicationPoolsMenu, mainForm.SitesMenu, mainForm.SiteMenu);
                if (succeeded)
                {
                    Tag = ServerManager;
                    mainForm.EnableServerMenuItems(true);
                    _status = NodeStatus.Loaded;
                    mainForm.DisconnectButton.Enabled = true;
                }
                else
                {
                    HandleServerConnectionFailed();
                }
            }
            catch (Exception ex)
            {
                RollbarLocator.RollbarInstance.Error(ex);
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
                HandleServerConnectionFailed();
            }
            finally
            {
                mainForm.HideInfo();
            }
        }

        private void HandleServerConnectionFailed()
        {
            ServerManager = null;
            _status = NodeStatus.Default;
            MainForm.DisconnectButton.Enabled = true;
        }

        public MainForm MainForm { get; set; }

        public override string PathToSite => string.Empty;

        public override string Folder => string.Empty;

        public override string Uri => string.Empty;

        public override ServerManager ServerManager { get; set; }

        public override void LoadPanels(MainForm mainForm, ServiceContainer serviceContainer, List<ModuleProvider> moduleProviders)
        {
            if (readOnly)
            {
                MessageBox.Show("Elevation is required. Please run Jexus Manager as administrator.");
                return;
            }

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

        public override void Expand(MainForm mainForm)
        { }

        public override void AddApplication(ContextMenuStrip appMenu)
        {
        }

        public override void AddVirtualDirectory(ContextMenuStrip vDirMenu)
        {
        }

        public bool LoadServer(ContextMenuStrip poolsMenu, ContextMenuStrip sitesMenu, ContextMenuStrip siteMenu)
        {
            if (readOnly)
            {
                MessageBox.Show("Elevation is required. Please run Jexus Manager as administrator.");
                return false;
            }

            if (ServerManager == null)
            {
                return false;
            }

            try
            {
                PoolsNode = new ApplicationPoolsTreeNode(ServiceProvider, ServerManager.ApplicationPools, this)
                {
                    ContextMenuStrip = poolsMenu
                };
                Nodes.Add(PoolsNode);
            }
            catch (Exception)
            {
                return false;
            }

            SitesNode = new SitesTreeNode(ServiceProvider, ServerManager.Sites, this)
            {
                ContextMenuStrip = sitesMenu
            };
            Nodes.Add(SitesNode);
            TreeView.SelectedNode = this;

            foreach (Site site in ServerManager.Sites)
            {
                var siteNode = new SiteTreeNode(ServiceProvider, site, this) { ContextMenuStrip = siteMenu };
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

        public static ServerTreeNode CreateJexusNode(IServiceProvider serviceProvider, string name, string hostName, string credentials, string hash, ServerManager server, bool isLocalhost)
        {
            return new ServerTreeNode(serviceProvider, name, hostName, credentials, hash, server, isLocalhost, WorkingMode.Jexus, false);
        }

        public static ServerTreeNode CreateIisExpressNode(IServiceProvider serviceProvider, string name, string fileName, ServerManager server, bool ignoreInCache)
        {
            return new ServerTreeNode(serviceProvider, name, fileName, string.Empty, string.Empty, server, true, WorkingMode.IisExpress, ignoreInCache);
        }

        public static ServerTreeNode CreateIisNode(IServiceProvider serviceProvider, string name, string fileName)
        {
            return new ServerTreeNode(serviceProvider, name, fileName, string.Empty, string.Empty, null, true, WorkingMode.Iis, true);
        }
    }
}

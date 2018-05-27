// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;
using Microsoft.Win32;

namespace JexusManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using JexusManager.Dialogs;
    using JexusManager.Features.Access;
    using JexusManager.Features.Authentication;
    using JexusManager.Features.Authorization;
    using JexusManager.Features.Caching;
    using JexusManager.Features.Certificates;
    using JexusManager.Features.Cgi;
    using JexusManager.Features.Compression;
    using JexusManager.Features.DefaultDocument;
    using JexusManager.Features.DirectoryBrowse;
    using JexusManager.Features.FastCgi;
    using JexusManager.Features.Handlers;
    using JexusManager.Features.HttpApi;
    using JexusManager.Features.HttpErrors;
    using JexusManager.Features.HttpRedirect;
    using JexusManager.Features.IpSecurity;
    using JexusManager.Features.IsapiCgiRestriction;
    using JexusManager.Features.IsapiFilters;
    using JexusManager.Features.Jexus;
    using JexusManager.Features.Logging;
    using JexusManager.Features.Main;
    using JexusManager.Features.MimeMap;
    using JexusManager.Features.Modules;
    using JexusManager.Features.RequestFiltering;
    using JexusManager.Features.ResponseHeaders;
    using JexusManager.Features.Rewrite;
    using JexusManager.Main.Properties;
    using JexusManager.Services;
    using JexusManager.Tree;
    using JexusManager.Wizards.ConnectionWizard;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Vista.Controls;

    using Action = Crad.Windows.Forms.Actions.Action;
    using Application = Microsoft.Web.Administration.Application;
    using Features;
    using JexusManager.Features.Asp;

    public sealed partial class MainForm : Form
    {
        private const string expressGlobalInstanceName = "IIS Express";
        private readonly List<ModuleProvider> _providers;
        private readonly ServiceContainer _serviceContainer;
        private readonly NavigationService _navigationService;

        public ManagementUIService UIService { get; }

        public MainForm(List<string> files)
        {
            InitializeComponent();

            removeToolStripMenuItem.Image = DefaultTaskList.RemoveImage;
            removeToolStripMenuItem1.Image = DefaultTaskList.RemoveImage;
            btnRemoveFarmServer.Image = DefaultTaskList.RemoveImage;
            toolStripMenuItem44.Image = DefaultTaskList.RemoveImage;

            Icon = Resources.iis;
            imageList1.Images.Add(Resources.iis_16); // 0
            imageList1.Images.Add(Resources.server_16); // 1
            imageList1.Images.Add(Resources.application_pools_16); // 2
            imageList1.Images.Add(Resources.sites_16); // 3
            imageList1.Images.Add(Resources.site_16); // 4
            imageList1.Images.Add(Resources.application_16); // 5
            imageList1.Images.Add(Resources.physical_directory_16); // 6
            imageList1.Images.Add(Resources.virtual_directory_16); // 7
            imageList1.Images.Add(Resources.farm_16); // 8
            imageList1.Images.Add(Resources.farm_server_16); // 9
            imageList1.Images.Add(Resources.servers_16); // 10
            imageList1.Images.Add(Resources.server_disabled_16); // 11
            imageList1.Images.Add(Resources.farm_disabled_16); // 12
            btnAbout.Text = string.Format("About Jexus Manager {0}", Assembly.GetExecutingAssembly().GetName().Version);
            treeView1.Nodes.Add(new HomePageTreeNode { ContextMenuStrip = cmsIis });

            _providers = new List<ModuleProvider>
            {
                new AspModuleProvider(),
                new AuthenticationModuleProvider(),
                new AuthorizationModuleProvider(),
                new CgiModuleProvider(),
                new CompressionModuleProvider(),
                new DefaultDocumentModuleProvider(),
                new DirectoryBrowseModuleProvider(),
                new FastCgiModuleProvider(),
                new HttpErrorsModuleProvider(),
                new HandlersModuleProvider(),
                new HttpRedirectModuleProvider(),
                new ResponseHeadersModuleProvider(),
                new IpSecurityModuleProvider(),
                new IsapiCgiRestrictionModuleProvider(),
                new IsapiFiltersModuleProvider(),
                new LoggingModuleProvider(),
                new MimeMapModuleProvider(),
                new ModulesModuleProvider(),
                new CachingModuleProvider(),
                new RequestFilteringModuleProvider(),
                new AccessModuleProvider(),
                new CertificatesModuleProvider(),
                new RewriteModuleProvider(),
                new HttpApiModuleProvider(),
                new JexusModuleProvider()
            };

            _navigationService = new NavigationService(this);
            _navigationService.NavigationPerformed += (sender, args) =>
                {
                    var item = new ExplorerNavigationHistoryItem("");
                    item.Tag = args.NewItem;
                    eanLocation.Navigation.AddHistory(item);
                };
            UIService = new ManagementUIService(this);
            _serviceContainer = new ServiceContainer();
            _serviceContainer.AddService(typeof(INavigationService), _navigationService);
            _serviceContainer.AddService(typeof(IManagementUIService), UIService);

            if (files.Count == 0)
            {
                LoadIisExpress();
                LoadIis();
                LoadJexus();
            }
            else
            {
                LoadIisExpressQuick(files);
            }

            Text = PublicNativeMethods.IsProcessElevated ? string.Format("{0} (Administrator)", Text) : Text;
        }

        internal ToolStripButton DisconnectButton
        {
            get { return btnDisconnect; }
        }

        internal ContextMenuStrip ApplicationPoolsMenu
        {
            get { return cmsApplicationPools; }
        }

        internal ContextMenuStrip SitesMenu
        {
            get { return cmsSites; }
        }

        internal ContextMenuStrip SiteMenu
        {
            get { return cmsSite; }
        }

        internal ContextMenuStrip VirtualDirectoryMenu
        {
            get { return cmsVirtualDirectory; }
        }

        internal ContextMenuStrip PhysicalDirectoryMenu
        {
            get { return cmsPhysicalDirectory; }
        }

        internal ContextMenuStrip ApplicationMenu
        {
            get { return cmsApplication; }
        }

        internal Action SaveMenuItem
        {
            get
            {
                return actSave;
            }
        }

        private void LoadIisExpress()
        {
            if (!File.Exists(DialogHelper.ListIisExpress))
            {
                return;
            }

            // TODO: load if only on Windows.
            var globalFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "IISExpress",
                "config",
                "applicationhost.config");
            if (File.Exists(globalFile))
            {
                var global = ServerTreeNode.CreateIisExpressNode(
                    _serviceContainer,
                    expressGlobalInstanceName,
                    globalFile,
                    server: null,
                    ignoreInCache: true);
                RegisterServer(global);
            }

            var lines = File.ReadAllLines(DialogHelper.ListIisExpress);
            foreach (var item in lines)
            {
                var parts = item.Split('|');
                if (parts.Length != 2)
                {
                    continue;
                }

                var data = ServerTreeNode.CreateIisExpressNode(
                    _serviceContainer,
                    name: parts[0],
                    fileName: parts[1], 
                    server: null,
                    ignoreInCache: true);
                RegisterServer(data);
            }
        }

        private void LoadIisExpressQuick(List<string> files)
        {
            if (!File.Exists(DialogHelper.ListIisExpress))
            {
                return;
            }

            var number = 1;
            foreach (var file in files)
            {
                var data = ServerTreeNode.CreateIisExpressNode(
                    _serviceContainer,
                    name: $"IIS Express {number++}",
                    fileName: file,
                    server: null,
                    ignoreInCache: true);
                RegisterServer(data);
            }
        }

        private void LoadIis()
        {
            // TODO: load if only on Windows.
            var config = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "inetsrv",
                "config",
                "applicationhost.config");
            if (File.Exists(config))
            {
                var data = ServerTreeNode.CreateIisNode(
                    _serviceContainer,
                    Environment.MachineName,
                    config);
                RegisterServer(data);
            }
        }

        private void RegisterServer(ServerTreeNode data)
        {
            data.ContextMenuStrip = cmsServer;
            treeView1.Nodes.Add(data);
        }

        private void LoadJexus()
        {
            if (!File.Exists(DialogHelper.ListJexus))
            {
                return;
            }

            var lines = File.ReadAllLines(DialogHelper.ListJexus);
            foreach (var item in lines)
            {
                var parts = item.Split(',');
                var data =  ServerTreeNode.CreateJexusNode(
                    _serviceContainer, 
                    name: parts[0],
                    hostName: parts[1],
                    credentials: parts[4],
                    hash: parts[2],
                    server: null,
                    isLocalhost: bool.Parse(parts[3]));
                RegisterServer(data);
            }
        }

        private ServerTreeNode GetCurrentData(TreeNode node)
        {
            if (node == null)
                throw new InvalidOperationException("no selected node");

            var managerNode = node as ManagerTreeNode;
            if (managerNode?.ServerNode == null)
                throw new InvalidOperationException($"no server node {node.GetType().FullName}");

            return managerNode.ServerNode;
        }

        private void actCreateSite_Execute(object sender, EventArgs e)
        {
            var selected = treeView1.SelectedNode;
            if (selected == null)
            {
                return;
            }

            var data = GetCurrentData(selected);
            if (data.IsBusy)
            {
                return;
            }

            if (data.ServerManager == null)
            {
                RollbarDotNet.Rollbar.Report($"null server: {data.DisplayName} : {data.Mode} : {selected.Text} : {selected.GetType().FullName}");
                return;
            }

            if (data.ServerManager.Sites == null)
            {
                RollbarDotNet.Rollbar.Report($"null sites collection: {data.DisplayName} : {data.Mode} : {selected.Text} : {selected.GetType().FullName} : {data.ServerManager.FileName}");
                return;
            }

            var dialog = new NewSiteDialog(_serviceContainer, data.ServerManager.Sites);
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            data.ServerManager.Sites.Add(dialog.NewSite);
            dialog.NewSite.Applications[0].Save();
            data.ServerManager.CommitChanges();
            AddSiteNode(dialog.NewSite);
        }

        public void LoadPage(IModulePage page)
        {
            var item = new NavigationItem(null, null, page.GetType(), null);
            item.Page = page;
            var info = page.PageInfo;
            _navigationService.NavigateToItem(item, false);
        }

        public void LoadInner(IModulePage page)
        {
            var panel = page as ContainerControl;
            if (panel == null)
            {
                return;
            }

            panel.Dock = DockStyle.Fill;
            scMain.Panel2.Controls.Clear();
            scMain.Panel2.Controls.Add(panel);
        }

        private void scMain_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (scMain.Panel1.Width > 500)
            {
                scMain.SplitterDistance = 500;
            }
        }

        private void editBindingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode.Tag as Site;
            if (node == null)
            {
                return;
            }

            var module = new MainModule();
            module.Initialize(_serviceContainer, null);
            var dialog = new BindingsDialog(module, node);
            dialog.ShowDialog(this);
        }

        private void btnRemoveSite_Click(object sender, EventArgs e)
        {
            var result = UIService.ShowMessage(
                "Are you sure that you want to remove the selected site?",
                "Confirm Remove",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var node = (Site)treeView1.SelectedNode.Tag;
            node.Parent.Remove(node);
            node.Server.CommitChanges();
            treeView1.SelectedNode.Remove();
        }

        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            if (actSave.Enabled)
            {
                var result = UIService.ShowMessage("The connection list has changed. Do you want to save changes?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    actSave.DoExecute();
                }
            }

            foreach (var item in treeView1.Nodes)
            {
                var serverNode = item as ServerTreeNode;
                if (serverNode?.ServerManager?.Mode != WorkingMode.Jexus)
                {
                    continue;
                }

                try
                {
                    serverNode.ServerManager.CommitChanges();
                    var conflict = AsyncHelper.RunSync(() => ((JexusServerManager)serverNode.ServerManager).ByeAsync());
                    if (Environment.MachineName != conflict)
                    {
                        MessageBox.Show(string.Format("The server is also connected to {0}. Making changes on multiple clients might corrupt server configuration.", conflict), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    var last = ex;
                    Exception previous = null;
                    while (last.InnerException != null)
                    {
                        previous = last;
                        last = last.InnerException;
                    }

                    var message = new StringBuilder();
                    message.AppendLine("Could not connect to the specified computer.")
                        .AppendLine()
                        .AppendFormat("Details: {0}", previous?.Message ?? last.Message);
                    MessageBox.Show(message.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void renameToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.BeginEdit();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var site = (Site)e.Node.Tag;
            if (string.IsNullOrEmpty(e.Label))
            {
                e.CancelEdit = true;
                return;
            }

            foreach (var ch in SiteCollection.InvalidSiteNameCharacters())
            {
                if (e.Label.Contains(ch))
                {
                    UIService.ShowMessage("The site name cannot contain the following characters: '\\, /, ?, ;, :, @, &, =, +, $, ,, |, \", <, >'.", "Sites", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                    return;
                }
            }

            foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
            {
                if (e.Label.Contains(ch) || e.Label.StartsWith("~"))
                {
                    UIService.ShowMessage("The site name cannot contain the following characters: '~,  '.", "Sites", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                    return;
                }
            }

            // TODO: is this required by Jexus?
            // await site.RemoveApplicationsAsync();
            site.Name = e.Label;
            site.Server.CommitChanges();

            treeView1.SelectedNode = null;
            treeView1.SelectedNode = e.Node;
        }

        private void treeView1_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (!(e.Node is SiteTreeNode))
            {
                e.CancelEdit = true;
            }
        }

        internal void RemoveSiteNode(Site site)
        {
            var selected = treeView1.SelectedNode;
            if (selected == null)
            {
                return;
            }
                
            var data = GetCurrentData(selected);
            if (data.IsBusy)
            {
                return;
            }

            foreach (TreeNode node in data.SitesNode.Nodes)
            {
                if (node.Tag == site)
                {
                    node.Remove();
                    break;
                }
            }
        }

        internal void RemoveFarmNode(string name)
        {
            //var data = GetCurrentData();
            //foreach (TreeNode node in data.FarmNode.Nodes)
            //{
            //    if (node.Text == name)
            //    {
            //        node.Remove();
            //    }
            //}
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            if (e.Node == null)
            {
                return;
            }

            var node = e.Node as ManagerTreeNode;
            if (node == null)
            {
                RollbarDotNet.Rollbar.Report($"wrong node {e.Node.GetType().FullName} {e.Node.Text}");
                return;
            }

            var serverNode = node as ServerTreeNode;
            actUp.Enabled = !(serverNode != null || node is HomePageTreeNode);
            if (serverNode != null)
            {
                if (serverNode.IsBusy)
                {
                    return;
                }

                actDisconnect.Enabled = !serverNode.IsBusy && !serverNode.IgnoreInCache;
                EnableServerMenuItems(serverNode.ServerManager != null);
                node.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            var siteNode = node as SiteTreeNode;
            if (siteNode != null)
            {
                var canBrowse = siteNode.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripMenuItem12.Visible = canBrowse;
                manageWebsiteToolStripMenuItem.Visible = canBrowse;
                node.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            var appNode = node as ApplicationTreeNode;
            if (appNode != null)
            {
                var canBrowse = appNode.Application.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripMenuItem15.Visible = canBrowse;
                manageApplicationToolStripMenuItem.Visible = canBrowse;
                node.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            var vdirNode = node as VirtualDirectoryTreeNode;
            if (vdirNode != null)
            {
                var canBrowse = vdirNode.VirtualDirectory.Application.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripSeparator4.Visible = canBrowse;
                manageVirtualDirectoryToolStripMenuItem.Visible = canBrowse;
                node.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            var physNode = node as PhysicalDirectoryTreeNode;
            if (physNode != null)
            {
                var canBrowse = physNode.PhysicalDirectory.Application.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripSeparator9.Visible = canBrowse;
                manageFolderToolStripMenuItem.Visible = canBrowse;
                node.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            node.LoadPanels(this, _serviceContainer, _providers);
            ShowInfo("Ready");
        }

        internal void EnableServerMenuItems(bool enabled)
        {
            toolStripMenuItem2.Visible = enabled;
            toolStripMenuItem3.Visible = enabled;
            toolStripMenuItem4.Visible = enabled;
            btnRenameServer.Visible = enabled;
            btnStartServer.Visible = enabled;
            btnStopServer.Visible = enabled;
            btnOpenConfig.Enabled = enabled;
            //actConnectServer.Visible = enabled;
        }

        internal void ShowInfo(string text)
        {
            txtInfo.Text = text;
            txtInfo.Visible = true;
        }

        internal void HideInfo()
        {
            txtInfo.Visible = false;
        }

        internal void UpdateSiteNode(Site site)
        {
            var selected = treeView1.SelectedNode;
            if (selected == null)
            {
                return;
            }
            
            var data = GetCurrentData(selected);
            if (data.IsBusy)
            {
                return;
            }

            foreach (TreeNode node in data.SitesNode.Nodes)
            {
                if (node.Tag == site)
                {
                    node.Text = site.Name;
                    break;
                }
            }
        }

        internal void ShowSite(Site site)
        {
            var selected = treeView1.SelectedNode;
            if (selected == null)
            {
                return;
            }
            
            var data = GetCurrentData(selected);
            if (data.IsBusy)
            {
                return;
            }

            foreach (TreeNode node in data.SitesNode.Nodes)
            {
                if (node.Tag == site)
                {
                    treeView1.SelectedNode = node;
                    break;
                }
            }
        }

        internal void AddSiteNode(Site site)
        {
            var selected = treeView1.SelectedNode;
            if (selected == null)
            {
                return;
            }
            
            var server = GetCurrentData(selected);
            if (server.IsBusy)
            {
                return;
            }

            ManagerTreeNode.AddToParent(server.SitesNode, new SiteTreeNode(_serviceContainer, site, server) { ContextMenuStrip = cmsSite });
        }

        internal void AddFarmNode(string farmName, List<FarmServerAdvancedSettings> servers)
        {
            //var data = GetCurrentData();
            //var farmNode = new TreeNode(farmName)
            //{
            //    ContextMenuStrip = cmsFarmServer,
            //    ImageIndex = 8,
            //    SelectedImageIndex = 8
            //};
            //var serversNode = farmNode.Nodes.Add("Servers");
            //serversNode.ContextMenuStrip = cmsServers;
            //serversNode.ImageIndex = 9;
            //serversNode.SelectedImageIndex = 9;
            //serversNode.Tag = servers;
            //data.FarmNode.Nodes.Add(farmNode);
            //treeView1.SelectedNode = farmNode;
        }

        internal void LoadSites()
        {
            var selected = treeView1.SelectedNode;
            if (selected == null)
            {
                return;
            }

            var server = GetCurrentData(selected);
            if (server.IsBusy)
            {
                return;
            }

            treeView1.SelectedNode = server.SitesNode;
        }

        internal void LoadPools()
        {
            var selected = treeView1.SelectedNode;
            if (selected == null)
            {
                return;
            }

            var server = GetCurrentData(selected);
            if (server.IsBusy)
            {
                return;
            }

            treeView1.SelectedNode = server.PoolsNode;
        }

        private void actUp_Execute(object sender, EventArgs e)
        {
            treeView1.SelectedNode = treeView1.SelectedNode.Parent;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void iISOnMSDNOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=213860");
        }

        private void iISNETOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=213859");
        }

        private void iISKBsOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210477");
        }

        private void actConnectServer_Execute(object sender, EventArgs e)
        {
            var names = new List<string>();
            names.Add("Start Page");
            foreach (var item in treeView1.Nodes)
            {
                var serverNode = item as ServerTreeNode;
                if (serverNode != null)
                {
                    names.Add(serverNode.DisplayName);
                }
            }

            var dialog = new ConnectionWizard(_serviceContainer, names.ToArray());
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            var data = (ConnectionWizardData)dialog.WizardData;
            ServerTreeNode node;
            if (data.Mode == WorkingMode.Jexus)
            {
                node = ServerTreeNode.CreateJexusNode(
                    _serviceContainer,
                    data.Name,
                    data.HostName,
                    data.UserName + "|" + data.Password,
                    data.CertificateHash,
                    data.Server,
                    isLocalhost: true);
                node.SetHandler();
                var path = Path.GetTempFileName();
                var random = Guid.NewGuid().ToString();
                File.WriteAllText(path, random);
                node.IsLocalhost = AsyncHelper.RunSync(() => ((JexusServerManager)node.ServerManager).LocalhostTestAsync(path, random));
                data.Server.IsLocalhost = node.IsLocalhost;
            }
            else
            {
                node = ServerTreeNode.CreateIisExpressNode(
                    _serviceContainer,
                    data.Name,
                    data.FileName,
                    data.Server,
                    ignoreInCache: false);
            }

            try
            {
                RegisterServer(node);
                // TODO: trigger the load in connection wizard to throw exception earlier.
                node.LoadServer(cmsApplicationPools, cmsSites, cmsSite);
                actSave.Enabled = true;
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
                MessageBox.Show(message.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void actSave_Execute(object sender, EventArgs e)
        {
            var jexusServers = new List<string>();
            var iisExpressFiles = new List<string>();
            foreach (var item in treeView1.Nodes)
            {
                var serverNode = item as ServerTreeNode;
                if (serverNode == null || serverNode.IgnoreInCache)
                {
                    continue;
                }

                if (serverNode.Mode == WorkingMode.Jexus)
                {
                    jexusServers.Add(
                        string.Format(
                            "{0},{1},{2},{3},{4}",
                            serverNode.DisplayName,
                            serverNode.HostName,
                            serverNode.CertificateHash,
                            serverNode.IsLocalhost,
                            serverNode.Credentials));
                    continue;
                }

                if (serverNode.Mode == WorkingMode.IisExpress)
                {
                    if (!string.IsNullOrWhiteSpace(serverNode.HostName))
                    {
                        iisExpressFiles.Add(
                            string.Format(
                                "{0}|{1}",
                                serverNode.DisplayName,
                                serverNode.HostName));
                    }
                }
            }

            File.WriteAllLines(DialogHelper.ListIisExpress, iisExpressFiles);
            File.WriteAllLines(DialogHelper.ListJexus, jexusServers);
            actSave.Enabled = false;
        }

        private void actDisconnect_Execute(object sender, EventArgs e)
        {
            var result = UIService.ShowMessage("Are you sure that you want to remove this connection?", "Confirm Remove", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            treeView1.SelectedNode.Remove();
            actDisconnect.Enabled = false;
            actSave.Enabled = true;
        }

        private void actCreateApplication_Execute(object sender, EventArgs e)
        {
            var treeNode = ((ManagerTreeNode)treeView1.SelectedNode);
            treeNode.AddApplication(cmsApplication);
            treeNode.ServerManager.CommitChanges();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            DialogHelper.ProcessStart("https://jexusmanager.com");
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = null;
            treeView1.SelectedNode = e.Node;

            if (e.Node is HomePageTreeNode)
            {
                return;
            }

            var server = GetCurrentData(e.Node);
            if (server.IsBusy)
            {
                return;
            }

            if (server.ServerManager == null && e.Button == MouseButtons.Right)
            {
                treeView1_NodeMouseDoubleClick(sender, e);
                treeView1_AfterSelect(sender, new TreeViewEventArgs(e.Node));
            }
        }

        private void btnRemoveApplication_Click(object sender, EventArgs e)
        {
            var node = (Application)treeView1.SelectedNode.Tag;
            node.Site.Applications = node.Remove();
            node.Server.CommitChanges();
            treeView1.SelectedNode.Remove();
        }

        private void actConnectSite_Execute(object sender, EventArgs e)
        {
        }

        private void btnRestartSite_Click(object sender, EventArgs e)
        {
            btnRestartSite.Enabled = false;
            var node = (Site)treeView1.SelectedNode.Tag;
            try
            {
                node.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnRestartSite.Enabled = true;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((ManagerTreeNode)e.Node).HandleDoubleClick(this);
            treeView1_AfterSelect(sender, new TreeViewEventArgs(e.Node));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var dialog = new UpdateDialog();
            dialog.ShowDialog(this);
        }

        private void btnRemoveFarmServer_Click(object sender, EventArgs e)
        {
            treeView1.SelectedNode.Remove();
        }

        private void btnAddFarmServer_Click(object sender, EventArgs e)
        {
            //var dialog = new NewFarmServerDialog();
            //if (dialog.ShowDialog() == DialogResult.Cancel)
            //{
            //    return;
            //}

            //var node = treeView1.SelectedNode;
            //if (node.ImageIndex == 8)
            //{
            //    node = node.Nodes[0];
            //}

            //if (node.ImageIndex == 9)
            //{
            //    var servers = (List<FarmServerAdvancedSettings>)node.Tag;
            //    servers.AddRange(dialog.Servers);
            //}
        }

        private void btnCreateFarm_Click(object sender, EventArgs e)
        {
            //var dialog = new FarmWizard();
            //if (dialog.ShowDialog() == DialogResult.Cancel)
            //{
            //    return;
            //}

            //var config = _current.Server.GetApplicationHostConfiguration();
            //ConfigurationSection webFarmsSection = config.GetSection("webFarms");
            //ConfigurationElementCollection webFarmsCollection = webFarmsSection.GetCollection();
            //ConfigurationElement webFarmElement = webFarmsCollection.CreateElement("webFarm");
            //webFarmElement["name"] = dialog.FarmName;
            //ConfigurationElementCollection webFarmCollection = webFarmElement.GetCollection();

            //foreach (var server in dialog.Servers)
            //{
            //    ConfigurationElement serverElement = webFarmCollection.CreateElement("server");
            //    serverElement["address"] = server.Name;
            //    serverElement["enabled"] = true;

            //    ConfigurationElement applicationRequestRoutingElement = serverElement.GetChildElement("applicationRequestRouting");
            //    applicationRequestRoutingElement["weight"] = server.Weight;
            //    applicationRequestRoutingElement["httpPort"] = server.HttpPort;
            //    applicationRequestRoutingElement["httpsPort"] = server.HttpsPort;
            //    webFarmCollection.Add(serverElement);
            //}

            //webFarmsCollection.Add(webFarmElement);
            //AddFarmNode(dialog.FarmName, dialog.Servers);
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            ((ManagerTreeNode)e.Node).Expand(this);
        }

        private void actBrowse_Execute(object sender, EventArgs e)
        {
            ((ManagerTreeNode)treeView1.SelectedNode).Browse();
        }

        private void actEditPermissions_Execute(object sender, EventArgs e)
        {
            ((ManagerTreeNode)treeView1.SelectedNode).EditPermissions();
        }

        private void actExplore_Execute(object sender, EventArgs e)
        {
            ((ManagerTreeNode)treeView1.SelectedNode).Explore();
        }

        private void actCreateVirtualDirectory_Execute(object sender, EventArgs e)
        {
            var treeNode = (ManagerTreeNode)treeView1.SelectedNode;
            treeNode.AddVirtualDirectory(cmsVirtualDirectory);
            treeNode.ServerManager.CommitChanges();
        }

        private void btnRemoveVirtualDirectory_Click(object sender, EventArgs e)
        {
            var treeNode = (VirtualDirectoryTreeNode)treeView1.SelectedNode;
            treeNode.VirtualDirectory.Application.VirtualDirectories.Remove(treeNode.VirtualDirectory);
            treeNode.ServerManager.CommitChanges();
            treeNode.Parent.Nodes.Remove(treeNode);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var key = Registry.CurrentUser.CreateSubKey(@"Software\LeXtudio\JexusManager");
            if (key == null)
            {
                return;
            }

            var last = (string)key.GetValue("LastUpdateCheck", string.Empty);
            DateTime lastDate;
            var valid = DateTime.TryParseExact(last, "D", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out lastDate);
            if (!valid)
            {
                lastDate = DateTime.UtcNow.Date.Subtract(TimeSpan.FromDays(1));
            }

            var span = DateTime.UtcNow.Date.Subtract(lastDate);
            if (span.TotalHours < 24)
            {
                return;
            }

            key.SetValue("LastUpdateCheck", DateTime.UtcNow.Date.ToString("D", CultureInfo.InvariantCulture));
            btnUpdate.PerformClick();
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            var server = GetCurrentData(treeView1.SelectedNode);
            if (server.IsBusy)
            {
                return;
            }

            if (server.Mode == WorkingMode.Iis || server.Mode == WorkingMode.IisExpress)
            {
                DialogHelper.BrowseFile(server.ServerManager.FileName);
                return;
            }
        }
    }
}

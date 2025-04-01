﻿// Copyright (c) Lex Li. All rights reserved.
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

    using Action = Crad.Windows.Forms.Actions.Action;
    using Application = Microsoft.Web.Administration.Application;
    using Features;
    using JexusManager.Features.Asp;
    using JexusManager.Features.TraceFailedRequests;
    using System.Diagnostics;
    using Microsoft.Extensions.Logging;
    using System.Drawing;

    public sealed partial class MainForm : Form
    {
        private readonly Panel _logPanel;
        private readonly RichTextBox _logTextBox;
        private const string expressGlobalInstanceName = "Global";
        private readonly List<ModuleProvider> _providers;
        private readonly ServiceContainer _serviceContainer;
        private readonly NavigationService _navigationService;
        private TreeNode IisExpressRoot { get; }
        private TreeNode IisRoot { get; }
        private TreeNode JexusRoot { get; }
        private TreeNode StartPage { get; set; }

        public ManagementUIService UIService { get; }

        public MainForm(List<string> files, RichTextBox texBox)
        {
            InitializeComponent();

            // Create logging panel
            _logTextBox = texBox;

            _logPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 200,
                Visible = false
            };
            _logPanel.Controls.Add(_logTextBox);
            Controls.Add(_logPanel);

            LogHelper.GetLogger<MainForm>().LogInformation("Jexus Manager starting up. Version: {Version}", 
                Assembly.GetExecutingAssembly().GetName().Version);

            // Add toggle logging menu item
            helpToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            var toggleLogItem = new ToolStripMenuItem("Show &Logs", null, (s, e) =>
            {
                _logPanel.Visible = !_logPanel.Visible;
                ((ToolStripMenuItem)s).Text = _logPanel.Visible ? "Hide &Logs" : "Show &Logs";
            });
            helpToolStripMenuItem.DropDownItems.Add(toggleLogItem);


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

            actRunAsAdmin.Image = NativeMethods.GetShieldIcon();
            btnAbout.Text = string.Format("About Jexus Manager {0}", Assembly.GetExecutingAssembly().GetName().Version);
            StartPage = new PlaceholderTreeNode("Start Page", 0) { ContextMenuStrip = cmsIis };
            treeView1.Nodes.Add(StartPage);
            if (!Helper.IsRunningOnMono())
            {
                IisExpressRoot = new PlaceholderTreeNode("IIS Express", 10) { ContextMenuStrip = cmsIis };
                treeView1.Nodes.Add(IisExpressRoot);
                IisRoot = new PlaceholderTreeNode("IIS", 10) { ContextMenuStrip = cmsIis };
                treeView1.Nodes.Add(IisRoot);
            }

            JexusRoot = new PlaceholderTreeNode("Jexus", 10) { ContextMenuStrip = cmsIis };
            treeView1.Nodes.Add(JexusRoot);

            _providers = new List<ModuleProvider>
            {
                new AspModuleProvider(),
                new AuthenticationModuleProvider(),
                new AuthorizationModuleProvider(),
                new CgiModuleProvider(),
                new CompressionModuleProvider(),
                new DefaultDocumentModuleProvider(),
                new DirectoryBrowseModuleProvider(),
                new HttpErrorsModuleProvider(),
                new TraceFailedRequestsModuleProvider(),
                new FastCgiModuleProvider(),
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

            if (PublicNativeMethods.IsProcessElevated)
            {
                Text = string.Format("{0} (Administrator)", Text);
                actRunAsAdmin.Visible = false;
            }
            else
            {
                IisRoot.ToolTipText = "This program must run administrator to manage IIS";
                IisRoot.Text = "IIS (Disabled)";
            }
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
            LogHelper.GetLogger<MainForm>().LogInformation("Loading IIS Express servers");
            if (!IisExpressServerManager.ServerInstalled)
            {
                LogHelper.GetLogger<MainForm>().LogWarning("IIS Express is not installed on this machine");
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
                    this,
                    _serviceContainer,
                    expressGlobalInstanceName,
                    globalFile,
                    server: null,
                    ignoreInCache: true);
                RegisterServer(global);
            }

            if (!File.Exists(DialogHelper.ListIisExpress))
            {
                return;
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
                    this,
                    _serviceContainer,
                    name: parts[0],
                    fileName: parts[1], 
                    server: null,
                    ignoreInCache: false);
                RegisterServer(data);
            }
        }

        private void LoadIisExpressQuick(List<string> files)
        {
            if (!IisExpressServerManager.ServerInstalled)
            {
                var result = UIService.ShowMessage(
                    "Didn't find IIS Express on this machine. Do you want to go and download it now?",
                    "IIS Express not found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    DialogHelper.ProcessStart("https://www.microsoft.com/download/details.aspx?id=48264");
                }

                IisExpressRoot.Text = "IIS Express (Disabled)";
                return;
            }

            if (files.Count == 0 && !File.Exists(DialogHelper.ListIisExpress))
            {
                return;
            }

            var number = 1;
            foreach (var file in files)
            {
                AspNetCoreHelper.FixConfigFile(file);
                var data = ServerTreeNode.CreateIisExpressNode(
                    this,
                    _serviceContainer,
                    name: $"IIS Express {number++}",
                    fileName: file,
                    server: null,
                    ignoreInCache: true);
                RegisterServer(data);
                IisExpressRoot.Expand();
                data.HandleDoubleClick();
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
                    this,
                    _serviceContainer,
                    Environment.MachineName,
                    config);
                RegisterServer(data);
            }
        }

        private void RegisterServer(ServerTreeNode data)
        {
            LogHelper.GetLogger<MainForm>().LogInformation("Registering server {Name} ({Mode})", data.DisplayName, data.Mode);
            data.ContextMenuStrip = cmsServer;
            if (data.Mode == WorkingMode.IisExpress)
            {
                IisExpressRoot.Nodes.Add(data);
            }
            else if (data.Mode == WorkingMode.Iis)
            {
                IisRoot.Nodes.Add(data);
            }
            else if (data.Mode == WorkingMode.Jexus)
            {
                JexusRoot.Nodes.Add(data);
            }
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
                    this,
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
                LogHelper.GetLogger<MainForm>().LogWarning("Cannot create site: no node selected");
                return;
            }

            var data = GetCurrentData(selected);
            if (data.IsBusy)
            {
                LogHelper.GetLogger<MainForm>().LogWarning("Cannot create site: server {Name} is busy", data.DisplayName);
                return;
            }

            if (data.ServerManager == null)
            {
                LogHelper.GetLogger<MainForm>().LogWarning("Null server: {DisplayName} : {Mode} : {Text} : {Type}", 
                    data.DisplayName, data.Mode, selected.Text, selected.GetType().FullName);
                return;
            }

            if (data.ServerManager.Sites == null)
            {
                LogHelper.GetLogger<MainForm>().LogWarning("Null sites collection: {DisplayName} : {Mode} : {Text} : {Type} : {FileName}", 
                    data.DisplayName, data.Mode, selected.Text, selected.GetType().FullName, data.ServerManager.FileName);
                return;
            }

            var module = new MainModule();
            module.Initialize(_serviceContainer, null);
            var dialog = new NewSiteDialog(module, data.ServerManager.Sites);
            if (dialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            LogHelper.GetLogger<MainForm>().LogInformation("Creating new site on server {Server}", data.DisplayName);
            data.ServerManager.Sites.Add(dialog.NewSite);
            dialog.NewSite.Applications[0].Save();
            data.ServerManager.CommitChanges();
            LogHelper.GetLogger<MainForm>().LogInformation("Site {SiteName} created successfully", dialog.NewSite.Name);
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
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode == null)
            {
                return;
            }

            if (!(selectedNode.Tag is Site node))
            {
                return;
            }

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

            node.Parent.Remove(node);
            node.Server.CommitChanges();
            selectedNode.Remove();
        }

        private void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            LogHelper.GetLogger<MainForm>().LogInformation("Application shutting down");
            if (actSave.Enabled)
            {
                var result = UIService.ShowMessage("The connection list has changed. Do you want to save changes?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    actSave.DoExecute();
                }
            }

            foreach (var item in JexusRoot.Nodes)
            {
                var serverNode = item as ServerTreeNode;
                try
                {
                    serverNode.ServerManager.CommitChanges();
                    var conflict = AsyncHelper.RunSync(() => ((JexusServerManager)serverNode.ServerManager).ByeAsync());
                    if (Environment.MachineName != conflict)
                    {
                        UIService.ShowMessage(string.Format("The server is also connected to {0}. Making changes on multiple clients might corrupt server configuration.", conflict), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.GetLogger<MainForm>().LogError(ex, "Error during server shutdown");
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
                    UIService.ShowMessage(message.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            foreach (var item in IisExpressRoot.Nodes)
            {
                var serverNode = item as ServerTreeNode;
                var serverManager = serverNode?.ServerManager;
                if (serverManager == null)
                {
                    continue;
                }

                if (serverManager.Sites.Count == 0)
                {
                    continue;
                }

                foreach (Site site in serverManager.Sites)
                {
                    if (site.State == ObjectState.Started)
                    {
                        var result = UIService.ShowMessage($"Site {site.Name} is still running. Do you want to stop it?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            site.Stop();
                        }
                    }
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
                if (e.Node.Text != ManagerTreeNode.TempNodeName)
                {
                    LogHelper.GetLogger<MainForm>().LogWarning("Wrong node type: {Type} Name: {Text}", e.Node.GetType().FullName, e.Node.Text);
                }

                return;
            }

            var serverNode = node as ServerTreeNode;
            actUp.Enabled = !(serverNode != null || node is PlaceholderTreeNode);
            if (serverNode != null)
            {
                if (serverNode.IsBusy)
                {
                    return;
                }

                actDisconnect.Enabled = !serverNode.IsBusy && !serverNode.IgnoreInCache;
                EnableServerMenuItems(serverNode.ServerManager != null);
                serverNode.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            if (node is SiteTreeNode siteNode)
            {
                var canBrowse = siteNode.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripMenuItem12.Visible = canBrowse;
                manageWebsiteToolStripMenuItem.Visible = canBrowse;
                siteNode.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            if (node is ApplicationTreeNode appNode)
            {
                var canBrowse = appNode.Application.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripMenuItem15.Visible = canBrowse;
                manageApplicationToolStripMenuItem.Visible = canBrowse;
                appNode.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            if (node is VirtualDirectoryTreeNode vdirNode)
            {
                var canBrowse = vdirNode.VirtualDirectory.Application.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripSeparator4.Visible = canBrowse;
                manageVirtualDirectoryToolStripMenuItem.Visible = canBrowse;
                vdirNode.LoadPanels(this, _serviceContainer, _providers);
                ShowInfo("Ready");
                return;
            }

            if (node is PhysicalDirectoryTreeNode physNode)
            {
                var canBrowse = physNode.PhysicalDirectory.Application.Site.Bindings.Any(binding => binding.CanBrowse);
                toolStripSeparator9.Visible = canBrowse;
                manageFolderToolStripMenuItem.Visible = canBrowse;
                physNode.LoadPanels(this, _serviceContainer, _providers);
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

            if (server.SitesNode == null)
            {
                return;
            }

            ManagerTreeNode.AddToParent(server.SitesNode, new SiteTreeNode(_serviceContainer, site, server) { ContextMenuStrip = cmsSite });
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
            ConnectToServer();
        }

        internal void ConnectToServer()
        {
            var names = new List<string>();
            GetAllServers(names, treeView1.Nodes);
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
                    this,
                    _serviceContainer,
                    data.Name,
                    data.HostName,
                    data.UserName + "|" + data.Password,
                    data.CertificateHash,
                    data.Server,
                    isLocalhost: true);
                var path = Path.GetTempFileName();
                var random = Guid.NewGuid().ToString();
                File.WriteAllText(path, random);
                node.IsLocalhost = AsyncHelper.RunSync(() => ((JexusServerManager)node.ServerManager).LocalhostTestAsync(path, random));
                data.Server.IsLocalhost = node.IsLocalhost;
            }
            else
            {
                node = ServerTreeNode.CreateIisExpressNode(
                    this,
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
                var succeeded = node.LoadServer(cmsApplicationPools, cmsSites, cmsSite);
                if (succeeded)
                {
                    actSave.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.GetLogger<MainForm>().LogError(ex, "Error during server shutdown");
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
                UIService.ShowMessage(message.ToString(), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetAllServers(List<string> names, TreeNodeCollection nodes)
        {
            foreach (TreeNode item in nodes)
            {
                if (item is ServerTreeNode serverNode)
                {
                    names.Add(serverNode.DisplayName);
                    continue;
                }

                GetAllServers(names, item.Nodes);
            }
        }

        private void actSave_Execute(object sender, EventArgs e)
        {
            var jexusServers = new List<string>();
            var iisExpressFiles = new List<string>();
            foreach (TreeNode node in treeView1.Nodes)
            {
                foreach (var item in node.Nodes)
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

            if (e.Node is PlaceholderTreeNode)
            {
                return;
            }

            var server = GetCurrentData(e.Node);
            if (server.IsBusy)
            {
                return;
            }

            // TODO: disable this so that config file errors can be investigated.
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
                UIService.ShowError(ex, string.Empty, Name, false);
            }

            btnRestartSite.Enabled = true;
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            ((ManagerTreeNode)e.Node).HandleDoubleClick();
            treeView1_AfterSelect(sender, new TreeViewEventArgs(e.Node));
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            // Navigate to the Start Page which will display update information
            treeView1.SelectedNode = StartPage;
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

        private void actRunAsAdmin_Execute(object sender, EventArgs e)
        {
            var info = new ProcessStartInfo
            {
                FileName = Path.ChangeExtension(Environment.CommandLine.Trim('"'), ".exe"),
                Verb = "runas",
                UseShellExecute = true
            };
            using var process = new Process
            {
                StartInfo = info
            };
            try
            {
                process.Start();
                if (process.Id != 0)
                {
                    Close();
                }
            }
            catch (Exception)
            {

            }
        }
    }
}

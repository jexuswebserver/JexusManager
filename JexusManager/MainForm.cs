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
    using System.Diagnostics;
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

    public sealed partial class MainForm : Form
    {
        private const string expressGlobalInstanceName = "IIS Express";
        private readonly List<ModuleProvider> _providers;
        private readonly ServiceContainer _serviceContainer;
        private readonly NavigationService _navigationService;

        public ManagementUIService UIService { get; }

        public MainForm()
        {
            InitializeComponent();

            removeToolStripMenuItem.Image = DefaultTaskList.RemoveImage;
            removeToolStripMenuItem1.Image = DefaultTaskList.RemoveImage;
            btnRemoveFarmServer.Image = DefaultTaskList.RemoveImage;
            toolStripMenuItem44.Image = DefaultTaskList.RemoveImage;

            Icon = Resources.iis;
            imageList1.Images.Add(Resources.iis_16);
            imageList1.Images.Add(Resources.server_16);
            imageList1.Images.Add(Resources.application_pools_16);
            imageList1.Images.Add(Resources.sites_16);
            imageList1.Images.Add(Resources.site_16);
            imageList1.Images.Add(Resources.application_16);
            imageList1.Images.Add(Resources.physical_directory_16);
            imageList1.Images.Add(Resources.virtual_directory_16);
            imageList1.Images.Add(Resources.farm_16);
            imageList1.Images.Add(Resources.farm_server_16);
            imageList1.Images.Add(Resources.servers_16);
            btnAbout.Text = string.Format("About Jexus Manager {0}", Assembly.GetExecutingAssembly().GetName().Version);
            treeView1.Nodes.Add(new HomePageTreeNode { ContextMenuStrip = cmsIis });

            _providers = new List<ModuleProvider>
            {
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
            this.UIService = new ManagementUIService(this);
            _serviceContainer = new ServiceContainer();
            _serviceContainer.AddService(typeof(INavigationService), _navigationService);
            _serviceContainer.AddService(typeof(IManagementUIService), this.UIService);

            LoadIisExpress();
            LoadIis();
            LoadJexus();

            Text = NativeMethods.IsProcessElevated ? string.Format("{0} (Administrator)", this.Text) : Text;
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
            // TODO: load if only on Windows.
            var globalFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "IISExpress",
                "config",
                "applicationhost.config");
            if (File.Exists(globalFile))
            {
                var global = new ServerTreeNode(
                    _serviceContainer,
                    expressGlobalInstanceName,
                    globalFile,
                    "",
                    "",
                    null,
                    true,
                    WorkingMode.IisExpress,
                    true);
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

                var data = new ServerTreeNode(_serviceContainer, parts[0], parts[1], "", "", null, true, WorkingMode.IisExpress, true);
                RegisterServer(data);
            }
        }

        private void LoadIis()
        {
            if (!NativeMethods.IsProcessElevated)
            {
                // IMPORTANT: only elevated can manipulate IIS.
                return;
            }

            // TODO: load if only on Windows.
            var config = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.System),
                "inetsrv",
                "config",
                "applicationhost.config");
            if (File.Exists(config))
            {
                var data = new ServerTreeNode(
                    _serviceContainer,
                    Environment.MachineName,
                    config,
                    "",
                    "",
                    null,
                    true,
                    WorkingMode.Iis,
                    true);
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
                var data = new ServerTreeNode(_serviceContainer, parts[0], parts[1], parts[4], parts[2], null, bool.Parse(parts[3]), WorkingMode.Jexus, false);
                RegisterServer(data);
            }
        }

        private ServerTreeNode GetCurrentData()
        {
            var top = treeView1.SelectedNode;
            while (top.Parent != null)
            {
                top = top.Parent;
            }

            return (top as ServerTreeNode);
        }

        private async void actCreateSite_Execute(object sender, EventArgs e)
        {
            var data = GetCurrentData();
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
            var node = (Site)treeView1.SelectedNode.Tag;
            var dialog = new BindingsDialog(_serviceContainer, node);
            dialog.ShowDialog(this);
        }

        private async void btnRemoveSite_Click(object sender, EventArgs e)
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

        private async void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            if (actSave.Enabled)
            {
                var result = this.UIService.ShowMessage("The connection list has changed. Do you want to save changes?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
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
                    await serverNode.ServerManager.CommitChangesAsync();
                    var conflict = await ((JexusServerManager)serverNode.ServerManager).ByeAsync();
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

        private async void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
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
                    this.UIService.ShowMessage("The site name cannot contain the following characters: '\\, /, ?, ;, :, @, &, =, +, $, ,, |, \", <, >'.", "Sites", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                    return;
                }
            }

            foreach (var ch in SiteCollection.InvalidSiteNameCharactersJexus())
            {
                if (e.Label.Contains(ch) || e.Label.StartsWith("~"))
                {
                    this.UIService.ShowMessage("The site name cannot contain the following characters: '~,  '.", "Sites", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    e.CancelEdit = true;
                    return;
                }
            }

            await site.RemoveApplicationsAsync();
            site.Name = e.Label;
            site.Save();
            // TODO: how to handle rename.
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
            var data = GetCurrentData();
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
            var node = ((ManagerTreeNode)e.Node);
            var serverNode = node as ServerTreeNode;
            if (serverNode != null && serverNode.IsBusy)
            {
                return;
            }

            actDisconnect.Enabled = serverNode != null && !serverNode.IsBusy && !serverNode.Ignore;
            EnableServerMenuItems(serverNode?.ServerManager != null);
            actUp.Enabled = !(serverNode != null || node is HomePageTreeNode);
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
            var data = GetCurrentData();
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
            var data = GetCurrentData();
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
            ManagerTreeNode.AddToParent(GetCurrentData().SitesNode, new SiteTreeNode(_serviceContainer, site) { ContextMenuStrip = cmsSite });
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
            treeView1.SelectedNode = GetCurrentData().SitesNode;
        }

        internal void LoadPools()
        {
            treeView1.SelectedNode = GetCurrentData().PoolsNode;
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
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=213860");
        }

        private void iISNETOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=213859");
        }

        private void iISKBsOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210477");
        }

        private async void actConnectServer_Execute(object sender, EventArgs e)
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
                node = new ServerTreeNode(
                    _serviceContainer,
                    data.Name,
                    data.HostName,
                    data.UserName + "|" + data.Password,
                    data.CertificateHash,
                    data.Server,
                    true,
                    WorkingMode.Jexus,
                    false);
                node.SetHandler();
                var path = Path.GetTempFileName();
                var random = Guid.NewGuid().ToString();
                File.WriteAllText(path, random);
                node.IsLocalhost = await ((JexusServerManager)node.ServerManager).LocalhostTestAsync(path, random);
                data.Server.IsLocalhost = node.IsLocalhost;
            }
            else
            {
                node = new ServerTreeNode(
                    _serviceContainer,
                    data.Name,
                    data.FileName,
                    string.Empty,
                    null,
                    data.Server,
                    true,
                    WorkingMode.IisExpress,
                    false);
            }

            try
            {
                RegisterServer(node);
                // TODO: trigger the load in connection wizard to throw exception earlier.
                await node.LoadServerAsync(cmsApplicationPools, cmsSites, cmsSite);
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
                if (serverNode == null || serverNode.Ignore)
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
            var result = this.UIService.ShowMessage("Are you sure that you want to remove this connection?", "Confirm Remove", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            treeView1.SelectedNode.Remove();
            actDisconnect.Enabled = false;
            actSave.Enabled = true;
        }

        private async void actCreateApplication_Execute(object sender, EventArgs e)
        {
            var treeNode = ((ManagerTreeNode)treeView1.SelectedNode);
            await treeNode.AddApplication(cmsApplication);
            treeNode.ServerManager.CommitChanges();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            Process.Start("https://jexus.codeplex.com");
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = null;
            treeView1.SelectedNode = e.Node;
        }

        private async void btnRemoveApplication_Click(object sender, EventArgs e)
        {
            var node = (Application)treeView1.SelectedNode.Tag;
            node.Site.Applications = await node.RemoveAsync();
            node.Server.CommitChanges();
            treeView1.SelectedNode.Remove();
        }

        private void actConnectSite_Execute(object sender, EventArgs e)
        {
        }

        private async void btnRestartSite_Click(object sender, EventArgs e)
        {
            btnRestartSite.Enabled = false;
            var node = (Site)treeView1.SelectedNode.Tag;
            try
            {
                await node.RestartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnRestartSite.Enabled = true;
        }

        private async void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            await ((ManagerTreeNode)e.Node).HandleDoubleClick(this);
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
    }
}

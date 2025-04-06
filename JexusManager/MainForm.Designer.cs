namespace JexusManager
{
    using System.ComponentModel;
    using System.Windows.Forms;

    using Crad.Windows.Forms.Actions;

    public sealed partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripSplitButton();
            this.btnServer = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDisconnect = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmsServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.addWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStartServer = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStopServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRenameServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOpenConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem40 = new System.Windows.Forms.ToolStripSeparator();
            this.switchToContentViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsApplicationPools = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addApplicationPoolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSites = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addWebsiteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.switchToContentViewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSite = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exploreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editPermissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.btnApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.addVirtualDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.editBindingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.manageWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRestartSite = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBrowseSite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
            this.advancedSettingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
            this.switchToContentViewToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txtInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAnApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
            this.saveConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
            this.runAsAdministratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upOneLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
            this.stopToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
            this.groupByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortByToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iISHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iISOnMSDNOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iISNETOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iISKBsOnlineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.actionList1 = new Crad.Windows.Forms.Actions.ActionList();
            this.actConnectServer = new Crad.Windows.Forms.Actions.Action();
            this.actBack = new Crad.Windows.Forms.Actions.Action();
            this.actForward = new Crad.Windows.Forms.Actions.Action();
            this.actSave = new Crad.Windows.Forms.Actions.Action();
            this.actUp = new Crad.Windows.Forms.Actions.Action();
            this.actDisconnect = new Crad.Windows.Forms.Actions.Action();
            this.actCreateSite = new Crad.Windows.Forms.Actions.Action();
            this.actExplore = new Crad.Windows.Forms.Actions.Action();
            this.actEditPermissions = new Crad.Windows.Forms.Actions.Action();
            this.actCreateApplication = new Crad.Windows.Forms.Actions.Action();
            this.actCreateVirtualDirectory = new Crad.Windows.Forms.Actions.Action();
            this.actBrowse = new Crad.Windows.Forms.Actions.Action();
            this.actConnectSite = new Crad.Windows.Forms.Actions.Action();
            this.actConnectionApplication = new Crad.Windows.Forms.Actions.Action();
            this.actRunAsAdmin = new Crad.Windows.Forms.Actions.Action();
            this.connectToAServerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAWebsiteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToAnApplicationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem38 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem49 = new System.Windows.Forms.ToolStripMenuItem();
            this.exploreToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editPermissionsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.addVirtualDirectoryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBrowseApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem36 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem37 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem39 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem41 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem46 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem47 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem50 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem52 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsIis = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsApplication = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.manageApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
            this.advancedSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
            this.switchToContentViewToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFarm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem29 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCreateFarm = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem30 = new System.Windows.Forms.ToolStripSeparator();
            this.switchToContentViewToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFarmServer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRemoveFarmServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem31 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddFarmServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRenameFarmServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem33 = new System.Windows.Forms.ToolStripSeparator();
            this.switchToContentViewToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsServers = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem34 = new System.Windows.Forms.ToolStripSeparator();
            this.addServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem35 = new System.Windows.Forms.ToolStripSeparator();
            this.switchToContentViewToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsVirtualDirectory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.convertToApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.manageVirtualDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem42 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem43 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem44 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem45 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsPhysicalDirectory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem48 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.manageFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem54 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem56 = new System.Windows.Forms.ToolStripMenuItem();
            this._logSplitter = new System.Windows.Forms.SplitContainer();
            this._logPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.cmsServer.SuspendLayout();
            this.cmsApplicationPools.SuspendLayout();
            this.cmsSites.SuspendLayout();
            this.cmsSite.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionList1)).BeginInit();
            this.cmsIis.SuspendLayout();
            this.cmsApplication.SuspendLayout();
            this.cmsFarm.SuspendLayout();
            this.cmsFarmServer.SuspendLayout();
            this.cmsServers.SuspendLayout();
            this.cmsVirtualDirectory.SuspendLayout();
            this.cmsPhysicalDirectory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._logSplitter)).BeginInit();
            this._logSplitter.Panel1.SuspendLayout();
            this._logSplitter.Panel2.SuspendLayout();
            this._logSplitter.SuspendLayout();
            this._logPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.scMain.Panel1MinSize = 150;
            this.scMain.Size = new System.Drawing.Size(915, 451);
            this.scMain.SplitterDistance = 175;
            this.scMain.SplitterWidth = 5;
            this.scMain.TabIndex = 0;
            this.scMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.scMain_SplitterMoved);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(175, 451);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.treeView1);
            this.panel2.Controls.Add(this.toolStrip2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(169, 418);
            this.panel2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.LabelEdit = true;
            this.treeView1.Location = new System.Drawing.Point(0, 39);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(169, 379);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_BeforeLabelEdit);
            this.treeView1.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView1_AfterLabelEdit);
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip2
            // 
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.btnSave,
            this.toolStripSeparator2,
            this.btnUp,
            this.toolStripSeparator1,
            this.btnDisconnect});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip2.Size = new System.Drawing.Size(169, 39);
            this.toolStrip2.TabIndex = 6;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnServer,
            this.connectToWebsiteToolStripMenuItem,
            this.connectToApplicationToolStripMenuItem});
            this.toolStripButton3.Image = global::JexusManager.Main.Properties.Resources.connect_16;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(48, 36);
            this.toolStripButton3.Text = "Create New Connection";
            // 
            // btnServer
            // 
            this.actionList1.SetAction(this.btnServer, this.actConnectServer);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(222, 22);
            this.btnServer.Text = "Connect to a Server...";
            this.btnServer.ToolTipText = "Connect to a Server...";
            // 
            // connectToWebsiteToolStripMenuItem
            // 
            this.connectToWebsiteToolStripMenuItem.Enabled = false;
            this.connectToWebsiteToolStripMenuItem.Image = global::JexusManager.Main.Properties.Resources.site_16;
            this.connectToWebsiteToolStripMenuItem.Name = "connectToWebsiteToolStripMenuItem";
            this.connectToWebsiteToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.connectToWebsiteToolStripMenuItem.Text = "Connect to a Website...";
            // 
            // connectToApplicationToolStripMenuItem
            // 
            this.connectToApplicationToolStripMenuItem.Enabled = false;
            this.connectToApplicationToolStripMenuItem.Image = global::JexusManager.Main.Properties.Resources.application_16;
            this.connectToApplicationToolStripMenuItem.Name = "connectToApplicationToolStripMenuItem";
            this.connectToApplicationToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.connectToApplicationToolStripMenuItem.Text = "Connect to an Application...";
            // 
            // btnSave
            // 
            this.actionList1.SetAction(this.btnSave, this.actSave);
            this.btnSave.AutoToolTip = false;
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Enabled = false;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(36, 36);
            this.btnSave.Text = "Save Connections";
            this.btnSave.ToolTipText = "Save Connections";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // btnUp
            // 
            this.actionList1.SetAction(this.btnUp, this.actUp);
            this.btnUp.AutoToolTip = false;
            this.btnUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUp.Enabled = false;
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(36, 36);
            this.btnUp.Text = "Up One Level";
            this.btnUp.ToolTipText = "Up One Level";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // btnDisconnect
            // 
            this.actionList1.SetAction(this.btnDisconnect, this.actDisconnect);
            this.btnDisconnect.AutoToolTip = false;
            this.btnDisconnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(36, 36);
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.ToolTipText = "Disconnect";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(175, 27);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connections";
            // 
            // cmsServer
            // 
            this.cmsServer.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.toolStripMenuItem1,
            this.removeConnectionToolStripMenuItem,
            this.toolStripMenuItem4,
            this.addWebsiteToolStripMenuItem,
            this.toolStripMenuItem2,
            this.btnStartServer,
            this.btnStopServer,
            this.toolStripMenuItem3,
            this.btnRenameServer,
            this.toolStripMenuItem5,
            this.btnOpenConfig,
            this.toolStripMenuItem40,
            this.switchToContentViewToolStripMenuItem});
            this.cmsServer.Name = "cmsServer";
            this.cmsServer.Size = new System.Drawing.Size(218, 344);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Enabled = false;
            this.refreshToolStripMenuItem.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(217, 38);
            this.refreshToolStripMenuItem.Text = "Refresh";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(214, 6);
            // 
            // removeConnectionToolStripMenuItem
            // 
            this.actionList1.SetAction(this.removeConnectionToolStripMenuItem, this.actDisconnect);
            this.removeConnectionToolStripMenuItem.Enabled = false;
            this.removeConnectionToolStripMenuItem.Name = "removeConnectionToolStripMenuItem";
            this.removeConnectionToolStripMenuItem.Size = new System.Drawing.Size(217, 38);
            this.removeConnectionToolStripMenuItem.Text = "Disconnect";
            this.removeConnectionToolStripMenuItem.ToolTipText = "Disconnect";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(214, 6);
            // 
            // addWebsiteToolStripMenuItem
            // 
            this.actionList1.SetAction(this.addWebsiteToolStripMenuItem, this.actCreateSite);
            this.addWebsiteToolStripMenuItem.AutoToolTip = true;
            this.addWebsiteToolStripMenuItem.Name = "addWebsiteToolStripMenuItem";
            this.addWebsiteToolStripMenuItem.Size = new System.Drawing.Size(217, 38);
            this.addWebsiteToolStripMenuItem.Text = "Add Website...";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(214, 6);
            // 
            // btnStartServer
            // 
            this.btnStartServer.Enabled = false;
            this.btnStartServer.Image = global::JexusManager.Main.Properties.Resources.start_16;
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(217, 38);
            this.btnStartServer.Text = "Start";
            // 
            // btnStopServer
            // 
            this.btnStopServer.Enabled = false;
            this.btnStopServer.Image = global::JexusManager.Main.Properties.Resources.stop_16;
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(217, 38);
            this.btnStopServer.Text = "Stop";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(214, 6);
            // 
            // btnRenameServer
            // 
            this.btnRenameServer.Enabled = false;
            this.btnRenameServer.Name = "btnRenameServer";
            this.btnRenameServer.Size = new System.Drawing.Size(217, 38);
            this.btnRenameServer.Text = "Rename";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(214, 6);
            // 
            // btnOpenConfig
            // 
            this.btnOpenConfig.Name = "btnOpenConfig";
            this.btnOpenConfig.Size = new System.Drawing.Size(217, 38);
            this.btnOpenConfig.Text = "Open Configuration File";
            this.btnOpenConfig.Click += new System.EventHandler(this.btnOpenConfig_Click);
            // 
            // toolStripMenuItem40
            // 
            this.toolStripMenuItem40.Name = "toolStripMenuItem40";
            this.toolStripMenuItem40.Size = new System.Drawing.Size(214, 6);
            // 
            // switchToContentViewToolStripMenuItem
            // 
            this.switchToContentViewToolStripMenuItem.Enabled = false;
            this.switchToContentViewToolStripMenuItem.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.switchToContentViewToolStripMenuItem.Name = "switchToContentViewToolStripMenuItem";
            this.switchToContentViewToolStripMenuItem.Size = new System.Drawing.Size(217, 38);
            this.switchToContentViewToolStripMenuItem.Text = "Switch to Content View";
            // 
            // cmsApplicationPools
            // 
            this.cmsApplicationPools.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsApplicationPools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addApplicationPoolToolStripMenuItem,
            this.toolStripMenuItem6,
            this.refreshToolStripMenuItem1});
            this.cmsApplicationPools.Name = "cmsApplicationPools";
            this.cmsApplicationPools.Size = new System.Drawing.Size(213, 86);
            // 
            // addApplicationPoolToolStripMenuItem
            // 
            this.addApplicationPoolToolStripMenuItem.Enabled = false;
            this.addApplicationPoolToolStripMenuItem.Name = "addApplicationPoolToolStripMenuItem";
            this.addApplicationPoolToolStripMenuItem.Size = new System.Drawing.Size(212, 38);
            this.addApplicationPoolToolStripMenuItem.Text = "Add Application Pool...";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(209, 6);
            // 
            // refreshToolStripMenuItem1
            // 
            this.refreshToolStripMenuItem1.Enabled = false;
            this.refreshToolStripMenuItem1.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem1.Name = "refreshToolStripMenuItem1";
            this.refreshToolStripMenuItem1.Size = new System.Drawing.Size(212, 38);
            this.refreshToolStripMenuItem1.Text = "Refresh";
            // 
            // cmsSites
            // 
            this.cmsSites.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsSites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addWebsiteToolStripMenuItem1,
            this.toolStripMenuItem7,
            this.refreshToolStripMenuItem2,
            this.toolStripMenuItem8,
            this.switchToContentViewToolStripMenuItem1});
            this.cmsSites.Name = "cmsSites";
            this.cmsSites.Size = new System.Drawing.Size(214, 130);
            // 
            // addWebsiteToolStripMenuItem1
            // 
            this.actionList1.SetAction(this.addWebsiteToolStripMenuItem1, this.actCreateSite);
            this.addWebsiteToolStripMenuItem1.AutoToolTip = true;
            this.addWebsiteToolStripMenuItem1.Name = "addWebsiteToolStripMenuItem1";
            this.addWebsiteToolStripMenuItem1.Size = new System.Drawing.Size(213, 38);
            this.addWebsiteToolStripMenuItem1.Text = "Add Website...";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(210, 6);
            // 
            // refreshToolStripMenuItem2
            // 
            this.refreshToolStripMenuItem2.Enabled = false;
            this.refreshToolStripMenuItem2.Name = "refreshToolStripMenuItem2";
            this.refreshToolStripMenuItem2.Size = new System.Drawing.Size(213, 38);
            this.refreshToolStripMenuItem2.Text = "Refresh";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(210, 6);
            // 
            // switchToContentViewToolStripMenuItem1
            // 
            this.switchToContentViewToolStripMenuItem1.Enabled = false;
            this.switchToContentViewToolStripMenuItem1.Name = "switchToContentViewToolStripMenuItem1";
            this.switchToContentViewToolStripMenuItem1.Size = new System.Drawing.Size(213, 38);
            this.switchToContentViewToolStripMenuItem1.Text = "Switch to Content View";
            // 
            // cmsSite
            // 
            this.cmsSite.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsSite.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exploreToolStripMenuItem,
            this.editPermissionsToolStripMenuItem,
            this.toolStripMenuItem10,
            this.btnApplication,
            this.addVirtualDirectoryToolStripMenuItem,
            this.toolStripMenuItem11,
            this.editBindingsToolStripMenuItem,
            this.toolStripMenuItem12,
            this.manageWebsiteToolStripMenuItem,
            this.toolStripMenuItem13,
            this.refreshToolStripMenuItem3,
            this.removeToolStripMenuItem,
            this.toolStripMenuItem14,
            this.renameToolStripMenuItem1,
            this.toolStripMenuItem17,
            this.switchToContentViewToolStripMenuItem2});
            this.cmsSite.Name = "cmsSite";
            this.cmsSite.Size = new System.Drawing.Size(214, 420);
            // 
            // exploreToolStripMenuItem
            // 
            this.actionList1.SetAction(this.exploreToolStripMenuItem, this.actExplore);
            this.exploreToolStripMenuItem.AutoToolTip = true;
            this.exploreToolStripMenuItem.Name = "exploreToolStripMenuItem";
            this.exploreToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.exploreToolStripMenuItem.Text = "Explore";
            // 
            // editPermissionsToolStripMenuItem
            // 
            this.actionList1.SetAction(this.editPermissionsToolStripMenuItem, this.actEditPermissions);
            this.editPermissionsToolStripMenuItem.AutoToolTip = true;
            this.editPermissionsToolStripMenuItem.Name = "editPermissionsToolStripMenuItem";
            this.editPermissionsToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.editPermissionsToolStripMenuItem.Text = "Edit Permissions...";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(210, 6);
            // 
            // btnApplication
            // 
            this.actionList1.SetAction(this.btnApplication, this.actCreateApplication);
            this.btnApplication.AutoToolTip = true;
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(213, 38);
            this.btnApplication.Text = "Add Application...";
            // 
            // addVirtualDirectoryToolStripMenuItem
            // 
            this.actionList1.SetAction(this.addVirtualDirectoryToolStripMenuItem, this.actCreateVirtualDirectory);
            this.addVirtualDirectoryToolStripMenuItem.AutoToolTip = true;
            this.addVirtualDirectoryToolStripMenuItem.Name = "addVirtualDirectoryToolStripMenuItem";
            this.addVirtualDirectoryToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.addVirtualDirectoryToolStripMenuItem.Text = "Add Virtual Directory...";
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(210, 6);
            // 
            // editBindingsToolStripMenuItem
            // 
            this.editBindingsToolStripMenuItem.Name = "editBindingsToolStripMenuItem";
            this.editBindingsToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.editBindingsToolStripMenuItem.Text = "Edit Bindings...";
            this.editBindingsToolStripMenuItem.Click += new System.EventHandler(this.editBindingsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(210, 6);
            // 
            // manageWebsiteToolStripMenuItem
            // 
            this.manageWebsiteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRestartSite,
            this.startToolStripMenuItem1,
            this.stopToolStripMenuItem2,
            this.toolStripMenuItem26,
            this.btnBrowseSite,
            this.toolStripMenuItem27,
            this.advancedSettingsToolStripMenuItem1});
            this.manageWebsiteToolStripMenuItem.Name = "manageWebsiteToolStripMenuItem";
            this.manageWebsiteToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.manageWebsiteToolStripMenuItem.Text = "Manage Website";
            // 
            // btnRestartSite
            // 
            this.btnRestartSite.Image = global::JexusManager.Main.Properties.Resources.restart_16;
            this.btnRestartSite.Name = "btnRestartSite";
            this.btnRestartSite.Size = new System.Drawing.Size(181, 22);
            this.btnRestartSite.Text = "Restart";
            this.btnRestartSite.Click += new System.EventHandler(this.btnRestartSite_Click);
            // 
            // startToolStripMenuItem1
            // 
            this.startToolStripMenuItem1.Enabled = false;
            this.startToolStripMenuItem1.Image = global::JexusManager.Main.Properties.Resources.start_16;
            this.startToolStripMenuItem1.Name = "startToolStripMenuItem1";
            this.startToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.startToolStripMenuItem1.Text = "Start";
            // 
            // stopToolStripMenuItem2
            // 
            this.stopToolStripMenuItem2.Enabled = false;
            this.stopToolStripMenuItem2.Image = global::JexusManager.Main.Properties.Resources.stop_16;
            this.stopToolStripMenuItem2.Name = "stopToolStripMenuItem2";
            this.stopToolStripMenuItem2.Size = new System.Drawing.Size(181, 22);
            this.stopToolStripMenuItem2.Text = "Stop";
            // 
            // toolStripMenuItem26
            // 
            this.toolStripMenuItem26.Name = "toolStripMenuItem26";
            this.toolStripMenuItem26.Size = new System.Drawing.Size(178, 6);
            // 
            // btnBrowseSite
            // 
            this.actionList1.SetAction(this.btnBrowseSite, this.actBrowse);
            this.btnBrowseSite.AutoToolTip = true;
            this.btnBrowseSite.Name = "btnBrowseSite";
            this.btnBrowseSite.Size = new System.Drawing.Size(181, 22);
            this.btnBrowseSite.Text = "Browse";
            // 
            // toolStripMenuItem27
            // 
            this.toolStripMenuItem27.Name = "toolStripMenuItem27";
            this.toolStripMenuItem27.Size = new System.Drawing.Size(178, 6);
            // 
            // advancedSettingsToolStripMenuItem1
            // 
            this.advancedSettingsToolStripMenuItem1.Enabled = false;
            this.advancedSettingsToolStripMenuItem1.Name = "advancedSettingsToolStripMenuItem1";
            this.advancedSettingsToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.advancedSettingsToolStripMenuItem1.Text = "Advanced Settings...";
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(210, 6);
            // 
            // refreshToolStripMenuItem3
            // 
            this.refreshToolStripMenuItem3.Enabled = false;
            this.refreshToolStripMenuItem3.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem3.Name = "refreshToolStripMenuItem3";
            this.refreshToolStripMenuItem3.Size = new System.Drawing.Size(213, 38);
            this.refreshToolStripMenuItem3.Text = "Refresh";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.btnRemoveSite_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(210, 6);
            // 
            // renameToolStripMenuItem1
            // 
            this.renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            this.renameToolStripMenuItem1.Size = new System.Drawing.Size(213, 38);
            this.renameToolStripMenuItem1.Text = "Rename";
            this.renameToolStripMenuItem1.Click += new System.EventHandler(this.renameToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size(210, 6);
            // 
            // switchToContentViewToolStripMenuItem2
            // 
            this.switchToContentViewToolStripMenuItem2.Enabled = false;
            this.switchToContentViewToolStripMenuItem2.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.switchToContentViewToolStripMenuItem2.Name = "switchToContentViewToolStripMenuItem2";
            this.switchToContentViewToolStripMenuItem2.Size = new System.Drawing.Size(213, 38);
            this.switchToContentViewToolStripMenuItem2.Text = "Switch to Content View";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtInfo});
            this.statusStrip1.Location = new System.Drawing.Point(0, 475);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(915, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txtInfo
            // 
            this.txtInfo.Image = global::JexusManager.Main.Properties.Resources.info_16;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(150, 32);
            this.txtInfo.Text = "toolStripStatusLabel1";
            this.txtInfo.Visible = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(915, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToAServerToolStripMenuItem,
            this.connectToAWebsiteToolStripMenuItem,
            this.connectToAnApplicationToolStripMenuItem,
            this.toolStripMenuItem22,
            this.saveConnectionsToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.toolStripMenuItem21,
            this.runAsAdministratorToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // connectToAServerToolStripMenuItem
            // 
            this.actionList1.SetAction(this.connectToAServerToolStripMenuItem, this.actConnectServer);
            this.connectToAServerToolStripMenuItem.Name = "connectToAServerToolStripMenuItem";
            this.connectToAServerToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.connectToAServerToolStripMenuItem.Text = "Connect to a Server...";
            this.connectToAServerToolStripMenuItem.ToolTipText = "Connect to a Server...";
            // 
            // connectToAWebsiteToolStripMenuItem
            // 
            this.actionList1.SetAction(this.connectToAWebsiteToolStripMenuItem, this.actConnectSite);
            this.connectToAWebsiteToolStripMenuItem.Enabled = false;
            this.connectToAWebsiteToolStripMenuItem.Name = "connectToAWebsiteToolStripMenuItem";
            this.connectToAWebsiteToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.connectToAWebsiteToolStripMenuItem.Text = "Connect to a Website...";
            this.connectToAWebsiteToolStripMenuItem.ToolTipText = "Connect to a Website...";
            // 
            // connectToAnApplicationToolStripMenuItem
            // 
            this.actionList1.SetAction(this.connectToAnApplicationToolStripMenuItem, this.actConnectionApplication);
            this.connectToAnApplicationToolStripMenuItem.Enabled = false;
            this.connectToAnApplicationToolStripMenuItem.Name = "connectToAnApplicationToolStripMenuItem";
            this.connectToAnApplicationToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.connectToAnApplicationToolStripMenuItem.Text = "Connect to an Application...";
            this.connectToAnApplicationToolStripMenuItem.ToolTipText = "Connect to an Application...";
            // 
            // toolStripMenuItem22
            // 
            this.toolStripMenuItem22.Name = "toolStripMenuItem22";
            this.toolStripMenuItem22.Size = new System.Drawing.Size(235, 6);
            // 
            // saveConnectionsToolStripMenuItem
            // 
            this.actionList1.SetAction(this.saveConnectionsToolStripMenuItem, this.actSave);
            this.saveConnectionsToolStripMenuItem.Enabled = false;
            this.saveConnectionsToolStripMenuItem.Name = "saveConnectionsToolStripMenuItem";
            this.saveConnectionsToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.saveConnectionsToolStripMenuItem.Text = "Save Connections";
            this.saveConnectionsToolStripMenuItem.ToolTipText = "Save Connections";
            // 
            // disconnectToolStripMenuItem
            // 
            this.actionList1.SetAction(this.disconnectToolStripMenuItem, this.actDisconnect);
            this.disconnectToolStripMenuItem.Enabled = false;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.ToolTipText = "Disconnect";
            // 
            // toolStripMenuItem21
            // 
            this.toolStripMenuItem21.Name = "toolStripMenuItem21";
            this.toolStripMenuItem21.Size = new System.Drawing.Size(235, 6);
            // 
            // runAsAdministratorToolStripMenuItem
            // 
            this.actionList1.SetAction(this.runAsAdministratorToolStripMenuItem, this.actRunAsAdmin);
            this.runAsAdministratorToolStripMenuItem.AutoToolTip = true;
            this.runAsAdministratorToolStripMenuItem.Name = "runAsAdministratorToolStripMenuItem";
            this.runAsAdministratorToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.runAsAdministratorToolStripMenuItem.Text = "Run as Administrator";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(238, 38);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backToolStripMenuItem,
            this.forwardToolStripMenuItem,
            this.upOneLevelToolStripMenuItem,
            this.homeToolStripMenuItem,
            this.toolStripMenuItem19,
            this.stopToolStripMenuItem1,
            this.refreshToolStripMenuItem4,
            this.toolStripMenuItem20,
            this.groupByToolStripMenuItem,
            this.sortByToolStripMenuItem,
            this.viewToolStripMenuItem1});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // backToolStripMenuItem
            // 
            this.actionList1.SetAction(this.backToolStripMenuItem, this.actBack);
            this.backToolStripMenuItem.Enabled = false;
            this.backToolStripMenuItem.Name = "backToolStripMenuItem";
            this.backToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.backToolStripMenuItem.Text = "Back";
            // 
            // forwardToolStripMenuItem
            // 
            this.actionList1.SetAction(this.forwardToolStripMenuItem, this.actForward);
            this.forwardToolStripMenuItem.Enabled = false;
            this.forwardToolStripMenuItem.Name = "forwardToolStripMenuItem";
            this.forwardToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.forwardToolStripMenuItem.Text = "Forward";
            // 
            // upOneLevelToolStripMenuItem
            // 
            this.actionList1.SetAction(this.upOneLevelToolStripMenuItem, this.actUp);
            this.upOneLevelToolStripMenuItem.Enabled = false;
            this.upOneLevelToolStripMenuItem.Name = "upOneLevelToolStripMenuItem";
            this.upOneLevelToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.upOneLevelToolStripMenuItem.Text = "Up One Level";
            this.upOneLevelToolStripMenuItem.ToolTipText = "Up One Level";
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Enabled = false;
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.homeToolStripMenuItem.Text = "Home";
            this.homeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Home)));
            // 
            // toolStripMenuItem19
            // 
            this.toolStripMenuItem19.Name = "toolStripMenuItem19";
            this.toolStripMenuItem19.Size = new System.Drawing.Size(141, 6);
            // 
            // stopToolStripMenuItem1
            // 
            this.stopToolStripMenuItem1.Enabled = false;
            this.stopToolStripMenuItem1.Name = "stopToolStripMenuItem1";
            this.stopToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.stopToolStripMenuItem1.Text = "Stop";
            // 
            // refreshToolStripMenuItem4
            // 
            this.refreshToolStripMenuItem4.Enabled = false;
            this.refreshToolStripMenuItem4.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem4.Name = "refreshToolStripMenuItem4";
            this.refreshToolStripMenuItem4.Size = new System.Drawing.Size(144, 22);
            this.refreshToolStripMenuItem4.Text = "Refresh";
            // 
            // toolStripMenuItem20
            // 
            this.toolStripMenuItem20.Name = "toolStripMenuItem20";
            this.toolStripMenuItem20.Size = new System.Drawing.Size(141, 6);
            // 
            // groupByToolStripMenuItem
            // 
            this.groupByToolStripMenuItem.Enabled = false;
            this.groupByToolStripMenuItem.Name = "groupByToolStripMenuItem";
            this.groupByToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.groupByToolStripMenuItem.Text = "Group By";
            // 
            // sortByToolStripMenuItem
            // 
            this.sortByToolStripMenuItem.Enabled = false;
            this.sortByToolStripMenuItem.Name = "sortByToolStripMenuItem";
            this.sortByToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.sortByToolStripMenuItem.Text = "Sort By";
            // 
            // viewToolStripMenuItem1
            // 
            this.viewToolStripMenuItem1.Enabled = false;
            this.viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            this.viewToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.viewToolStripMenuItem1.Text = "View";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iISHelpToolStripMenuItem,
            this.iISOnMSDNOnlineToolStripMenuItem,
            this.iISNETOnlineToolStripMenuItem,
            this.iISKBsOnlineToolStripMenuItem,
            this.toolStripMenuItem28,
            this.btnUpdate,
            this.toolStripMenuItem18,
            this.btnAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // iISHelpToolStripMenuItem
            // 
            this.iISHelpToolStripMenuItem.Image = global::JexusManager.Main.Properties.Resources.help_16;
            this.iISHelpToolStripMenuItem.Name = "iISHelpToolStripMenuItem";
            this.iISHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.iISHelpToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.iISHelpToolStripMenuItem.Text = "IIS Help";
            // 
            // iISOnMSDNOnlineToolStripMenuItem
            // 
            this.iISOnMSDNOnlineToolStripMenuItem.Name = "iISOnMSDNOnlineToolStripMenuItem";
            this.iISOnMSDNOnlineToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.iISOnMSDNOnlineToolStripMenuItem.Text = "IIS on MSDN Online";
            this.iISOnMSDNOnlineToolStripMenuItem.Click += new System.EventHandler(this.iISOnMSDNOnlineToolStripMenuItem_Click);
            // 
            // iISNETOnlineToolStripMenuItem
            // 
            this.iISNETOnlineToolStripMenuItem.Name = "iISNETOnlineToolStripMenuItem";
            this.iISNETOnlineToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.iISNETOnlineToolStripMenuItem.Text = "IIS.NET Online";
            this.iISNETOnlineToolStripMenuItem.Click += new System.EventHandler(this.iISNETOnlineToolStripMenuItem_Click);
            // 
            // iISKBsOnlineToolStripMenuItem
            // 
            this.iISKBsOnlineToolStripMenuItem.Name = "iISKBsOnlineToolStripMenuItem";
            this.iISKBsOnlineToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.iISKBsOnlineToolStripMenuItem.Text = "IIS KBs Online";
            this.iISKBsOnlineToolStripMenuItem.Click += new System.EventHandler(this.iISKBsOnlineToolStripMenuItem_Click);
            // 
            // toolStripMenuItem28
            // 
            this.toolStripMenuItem28.Name = "toolStripMenuItem28";
            this.toolStripMenuItem28.Size = new System.Drawing.Size(185, 6);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::JexusManager.Main.Properties.Resources.update_16;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(188, 22);
            this.btnUpdate.Text = "Check Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripMenuItem18
            // 
            this.toolStripMenuItem18.Name = "toolStripMenuItem18";
            this.toolStripMenuItem18.Size = new System.Drawing.Size(185, 6);
            // 
            // btnAbout
            // 
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(188, 22);
            this.btnAbout.Text = "About Jexus Manager";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // actionList1
            // 
            this.actionList1.Actions.Add(this.actDisconnect);
            this.actionList1.Actions.Add(this.actUp);
            this.actionList1.Actions.Add(this.actConnectServer);
            this.actionList1.Actions.Add(this.actConnectSite);
            this.actionList1.Actions.Add(this.actConnectionApplication);
            this.actionList1.Actions.Add(this.actSave);
            this.actionList1.Actions.Add(this.actCreateSite);
            this.actionList1.Actions.Add(this.actCreateApplication);
            this.actionList1.Actions.Add(this.actCreateVirtualDirectory);
            this.actionList1.Actions.Add(this.actExplore);
            this.actionList1.Actions.Add(this.actEditPermissions);
            this.actionList1.Actions.Add(this.actBrowse);
            this.actionList1.Actions.Add(this.actRunAsAdmin);
            this.actionList1.Actions.Add(this.actBack);
            this.actionList1.Actions.Add(this.actForward);
            this.actionList1.ContainerControl = this;
            // 
            // actConnectServer
            // 
            this.actConnectServer.Image = global::JexusManager.Main.Properties.Resources.server_16;
            this.actConnectServer.Text = "Connect to a Server...";
            this.actConnectServer.ToolTipText = "Connect to a Server...";
            this.actConnectServer.Execute += new System.EventHandler(this.actConnectServer_Execute);
            // 
            // actSave
            // 
            this.actSave.Enabled = false;
            this.actSave.Image = global::JexusManager.Main.Properties.Resources.save_16;
            this.actSave.Text = "Save Connections";
            this.actSave.ToolTipText = "Save Connections";
            this.actSave.Execute += new System.EventHandler(this.actSave_Execute);
            // 
            // actUp
            // 
            this.actUp.Enabled = false;
            this.actUp.Image = global::JexusManager.Main.Properties.Resources.up_16;
            this.actUp.Text = "Up One Level";
            this.actUp.ToolTipText = "Up One Level";
            this.actUp.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Up)));
            this.actUp.Execute += new System.EventHandler(this.actUp_Execute);
            // 
            // actDisconnect
            // 
            this.actDisconnect.Enabled = false;
            this.actDisconnect.Image = global::JexusManager.Main.Properties.Resources.disconnected_16;
            this.actDisconnect.Text = "Disconnect";
            this.actDisconnect.ToolTipText = "Disconnect";
            this.actDisconnect.Execute += new System.EventHandler(this.actDisconnect_Execute);
            // 
            // actCreateSite
            // 
            this.actCreateSite.Image = global::JexusManager.Main.Properties.Resources.site_new_16;
            this.actCreateSite.Text = "Add Website...";
            this.actCreateSite.Execute += new System.EventHandler(this.actCreateSite_Execute);
            // 
            // actExplore
            // 
            this.actExplore.Image = global::JexusManager.Main.Properties.Resources.explore_16;
            this.actExplore.Text = "Explore";
            this.actExplore.Execute += new System.EventHandler(this.actExplore_Execute);
            // 
            // actEditPermissions
            // 
            this.actEditPermissions.Text = "Edit Permissions...";
            this.actEditPermissions.Execute += new System.EventHandler(this.actEditPermissions_Execute);
            // 
            // actCreateApplication
            // 
            this.actCreateApplication.Image = global::JexusManager.Main.Properties.Resources.application_new_16;
            this.actCreateApplication.Text = "Add Application...";
            this.actCreateApplication.Execute += new System.EventHandler(this.actCreateApplication_Execute);
            // 
            // actCreateVirtualDirectory
            // 
            this.actCreateVirtualDirectory.Image = global::JexusManager.Main.Properties.Resources.virtual_directory_new_16;
            this.actCreateVirtualDirectory.Text = "Add Virtual Directory...";
            this.actCreateVirtualDirectory.Execute += new System.EventHandler(this.actCreateVirtualDirectory_Execute);
            // 
            // actBrowse
            // 
            this.actBrowse.Image = global::JexusManager.Main.Properties.Resources.browse_16;
            this.actBrowse.Text = "Browse";
            this.actBrowse.Execute += new System.EventHandler(this.actBrowse_Execute);
            // 
            // actConnectSite
            // 
            this.actConnectSite.Enabled = false;
            this.actConnectSite.Image = global::JexusManager.Main.Properties.Resources.site_16;
            this.actConnectSite.Text = "Connect to a Website...";
            this.actConnectSite.ToolTipText = "Connect to a Website...";
            this.actConnectSite.Execute += new System.EventHandler(this.actConnectSite_Execute);
            // 
            // actConnectionApplication
            // 
            this.actConnectionApplication.Enabled = false;
            this.actConnectionApplication.Image = global::JexusManager.Main.Properties.Resources.application_16;
            this.actConnectionApplication.Text = "Connect to an Application...";
            this.actConnectionApplication.ToolTipText = "Connect to an Application...";
            // 
            // actRunAsAdmin
            // 
            this.actRunAsAdmin.Text = "Run as Administrator";
            this.actRunAsAdmin.Execute += new System.EventHandler(this.actRunAsAdmin_Execute);
            // 
            // actBack
            // 
            this.actBack.Text = "Back";
            this.actBack.Image = global::JexusManager.Main.Properties.Resources.back_16;
            this.actBack.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
            this.actBack.Execute += new System.EventHandler(this.actBack_Execute);
            // 
            // actForward
            // 
            this.actForward.Text = "Forward";
            this.actForward.Image = global::JexusManager.Main.Properties.Resources.forward_16;
            this.actForward.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
            this.actForward.Execute += new System.EventHandler(this.actForward_Execute);
            // 
            // connectToAServerToolStripMenuItem1
            // 
            this.actionList1.SetAction(this.connectToAServerToolStripMenuItem1, this.actConnectServer);
            this.connectToAServerToolStripMenuItem1.Name = "connectToAServerToolStripMenuItem1";
            this.connectToAServerToolStripMenuItem1.Size = new System.Drawing.Size(238, 38);
            this.connectToAServerToolStripMenuItem1.Text = "Connect to a Server...";
            this.connectToAServerToolStripMenuItem1.ToolTipText = "Connect to a Server...";
            // 
            // connectToAWebsiteToolStripMenuItem1
            // 
            this.actionList1.SetAction(this.connectToAWebsiteToolStripMenuItem1, this.actConnectSite);
            this.connectToAWebsiteToolStripMenuItem1.Enabled = false;
            this.connectToAWebsiteToolStripMenuItem1.Name = "connectToAWebsiteToolStripMenuItem1";
            this.connectToAWebsiteToolStripMenuItem1.Size = new System.Drawing.Size(238, 38);
            this.connectToAWebsiteToolStripMenuItem1.Text = "Connect to a Website...";
            this.connectToAWebsiteToolStripMenuItem1.ToolTipText = "Connect to a Website...";
            // 
            // connectToAnApplicationToolStripMenuItem1
            // 
            this.actionList1.SetAction(this.connectToAnApplicationToolStripMenuItem1, this.actConnectionApplication);
            this.connectToAnApplicationToolStripMenuItem1.Enabled = false;
            this.connectToAnApplicationToolStripMenuItem1.Name = "connectToAnApplicationToolStripMenuItem1";
            this.connectToAnApplicationToolStripMenuItem1.Size = new System.Drawing.Size(238, 38);
            this.connectToAnApplicationToolStripMenuItem1.Text = "Connect to an Application...";
            this.connectToAnApplicationToolStripMenuItem1.ToolTipText = "Connect to an Application...";
            // 
            // addApplicationToolStripMenuItem
            // 
            this.actionList1.SetAction(this.addApplicationToolStripMenuItem, this.actCreateApplication);
            this.addApplicationToolStripMenuItem.AutoToolTip = true;
            this.addApplicationToolStripMenuItem.Name = "addApplicationToolStripMenuItem";
            this.addApplicationToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.addApplicationToolStripMenuItem.Text = "Add Application...";
            // 
            // toolStripMenuItem38
            // 
            this.actionList1.SetAction(this.toolStripMenuItem38, this.actCreateApplication);
            this.toolStripMenuItem38.Name = "toolStripMenuItem38";
            this.toolStripMenuItem38.Size = new System.Drawing.Size(221, 38);
            this.toolStripMenuItem38.Text = "Add Application...";
            // 
            // toolStripMenuItem49
            // 
            this.actionList1.SetAction(this.toolStripMenuItem49, this.actCreateApplication);
            this.toolStripMenuItem49.AutoToolTip = true;
            this.toolStripMenuItem49.Name = "toolStripMenuItem49";
            this.toolStripMenuItem49.Size = new System.Drawing.Size(213, 38);
            this.toolStripMenuItem49.Text = "Add Application...";
            // 
            // exploreToolStripMenuItem1
            // 
            this.actionList1.SetAction(this.exploreToolStripMenuItem1, this.actExplore);
            this.exploreToolStripMenuItem1.AutoToolTip = true;
            this.exploreToolStripMenuItem1.Name = "exploreToolStripMenuItem1";
            this.exploreToolStripMenuItem1.Size = new System.Drawing.Size(213, 38);
            this.exploreToolStripMenuItem1.Text = "Explore";
            // 
            // editPermissionsToolStripMenuItem1
            // 
            this.actionList1.SetAction(this.editPermissionsToolStripMenuItem1, this.actEditPermissions);
            this.editPermissionsToolStripMenuItem1.AutoToolTip = true;
            this.editPermissionsToolStripMenuItem1.Name = "editPermissionsToolStripMenuItem1";
            this.editPermissionsToolStripMenuItem1.Size = new System.Drawing.Size(213, 38);
            this.editPermissionsToolStripMenuItem1.Text = "Edit Permissions...";
            // 
            // addVirtualDirectoryToolStripMenuItem1
            // 
            this.actionList1.SetAction(this.addVirtualDirectoryToolStripMenuItem1, this.actCreateVirtualDirectory);
            this.addVirtualDirectoryToolStripMenuItem1.AutoToolTip = true;
            this.addVirtualDirectoryToolStripMenuItem1.Name = "addVirtualDirectoryToolStripMenuItem1";
            this.addVirtualDirectoryToolStripMenuItem1.Size = new System.Drawing.Size(213, 38);
            this.addVirtualDirectoryToolStripMenuItem1.Text = "Add Virtual Directory...";
            // 
            // btnBrowseApplication
            // 
            this.actionList1.SetAction(this.btnBrowseApplication, this.actBrowse);
            this.btnBrowseApplication.AutoToolTip = true;
            this.btnBrowseApplication.Name = "btnBrowseApplication";
            this.btnBrowseApplication.Size = new System.Drawing.Size(181, 22);
            this.btnBrowseApplication.Text = "Browse";
            // 
            // toolStripMenuItem36
            // 
            this.actionList1.SetAction(this.toolStripMenuItem36, this.actExplore);
            this.toolStripMenuItem36.AutoToolTip = true;
            this.toolStripMenuItem36.Name = "toolStripMenuItem36";
            this.toolStripMenuItem36.Size = new System.Drawing.Size(221, 38);
            this.toolStripMenuItem36.Text = "Explore";
            // 
            // toolStripMenuItem37
            // 
            this.actionList1.SetAction(this.toolStripMenuItem37, this.actEditPermissions);
            this.toolStripMenuItem37.AutoToolTip = true;
            this.toolStripMenuItem37.Name = "toolStripMenuItem37";
            this.toolStripMenuItem37.Size = new System.Drawing.Size(221, 38);
            this.toolStripMenuItem37.Text = "Edit Permissions...";
            // 
            // toolStripMenuItem39
            // 
            this.actionList1.SetAction(this.toolStripMenuItem39, this.actCreateVirtualDirectory);
            this.toolStripMenuItem39.AutoToolTip = true;
            this.toolStripMenuItem39.Name = "toolStripMenuItem39";
            this.toolStripMenuItem39.Size = new System.Drawing.Size(221, 38);
            this.toolStripMenuItem39.Text = "Add Virtual Directory...";
            // 
            // toolStripMenuItem41
            // 
            this.actionList1.SetAction(this.toolStripMenuItem41, this.actBrowse);
            this.toolStripMenuItem41.AutoToolTip = true;
            this.toolStripMenuItem41.Name = "toolStripMenuItem41";
            this.toolStripMenuItem41.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem41.Text = "Browse";
            // 
            // toolStripMenuItem46
            // 
            this.actionList1.SetAction(this.toolStripMenuItem46, this.actExplore);
            this.toolStripMenuItem46.AutoToolTip = true;
            this.toolStripMenuItem46.Name = "toolStripMenuItem46";
            this.toolStripMenuItem46.Size = new System.Drawing.Size(213, 38);
            this.toolStripMenuItem46.Text = "Explore";
            // 
            // toolStripMenuItem47
            // 
            this.actionList1.SetAction(this.toolStripMenuItem47, this.actEditPermissions);
            this.toolStripMenuItem47.AutoToolTip = true;
            this.toolStripMenuItem47.Name = "toolStripMenuItem47";
            this.toolStripMenuItem47.Size = new System.Drawing.Size(213, 38);
            this.toolStripMenuItem47.Text = "Edit Permissions...";
            // 
            // toolStripMenuItem50
            // 
            this.actionList1.SetAction(this.toolStripMenuItem50, this.actCreateVirtualDirectory);
            this.toolStripMenuItem50.AutoToolTip = true;
            this.toolStripMenuItem50.Name = "toolStripMenuItem50";
            this.toolStripMenuItem50.Size = new System.Drawing.Size(213, 38);
            this.toolStripMenuItem50.Text = "Add Virtual Directory...";
            // 
            // toolStripMenuItem52
            // 
            this.actionList1.SetAction(this.toolStripMenuItem52, this.actBrowse);
            this.toolStripMenuItem52.AutoToolTip = true;
            this.toolStripMenuItem52.Name = "toolStripMenuItem52";
            this.toolStripMenuItem52.Size = new System.Drawing.Size(112, 22);
            this.toolStripMenuItem52.Text = "Browse";
            // 
            // cmsIis
            // 
            this.cmsIis.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsIis.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem5,
            this.toolStripMenuItem23,
            this.connectToAServerToolStripMenuItem1,
            this.connectToAWebsiteToolStripMenuItem1,
            this.connectToAnApplicationToolStripMenuItem1});
            this.cmsIis.Name = "cmsIis";
            this.cmsIis.Size = new System.Drawing.Size(239, 162);
            // 
            // refreshToolStripMenuItem5
            // 
            this.refreshToolStripMenuItem5.Enabled = false;
            this.refreshToolStripMenuItem5.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem5.Name = "refreshToolStripMenuItem5";
            this.refreshToolStripMenuItem5.Size = new System.Drawing.Size(238, 38);
            this.refreshToolStripMenuItem5.Text = "Refresh";
            // 
            // toolStripMenuItem23
            // 
            this.toolStripMenuItem23.Name = "toolStripMenuItem23";
            this.toolStripMenuItem23.Size = new System.Drawing.Size(235, 6);
            // 
            // cmsApplication
            // 
            this.cmsApplication.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsApplication.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exploreToolStripMenuItem1,
            this.editPermissionsToolStripMenuItem1,
            this.toolStripMenuItem9,
            this.addApplicationToolStripMenuItem,
            this.addVirtualDirectoryToolStripMenuItem1,
            this.toolStripMenuItem15,
            this.manageApplicationToolStripMenuItem,
            this.toolStripMenuItem16,
            this.refreshToolStripMenuItem6,
            this.removeToolStripMenuItem1,
            this.toolStripMenuItem24,
            this.switchToContentViewToolStripMenuItem3});
            this.cmsApplication.Name = "cmsApplication";
            this.cmsApplication.Size = new System.Drawing.Size(214, 332);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(210, 6);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(210, 6);
            // 
            // manageApplicationToolStripMenuItem
            // 
            this.manageApplicationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBrowseApplication,
            this.toolStripMenuItem25,
            this.advancedSettingsToolStripMenuItem});
            this.manageApplicationToolStripMenuItem.Name = "manageApplicationToolStripMenuItem";
            this.manageApplicationToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.manageApplicationToolStripMenuItem.Text = "Manage Application";
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(178, 6);
            // 
            // advancedSettingsToolStripMenuItem
            // 
            this.advancedSettingsToolStripMenuItem.Enabled = false;
            this.advancedSettingsToolStripMenuItem.Name = "advancedSettingsToolStripMenuItem";
            this.advancedSettingsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.advancedSettingsToolStripMenuItem.Text = "Advanced Settings...";
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(210, 6);
            // 
            // refreshToolStripMenuItem6
            // 
            this.refreshToolStripMenuItem6.Enabled = false;
            this.refreshToolStripMenuItem6.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem6.Name = "refreshToolStripMenuItem6";
            this.refreshToolStripMenuItem6.Size = new System.Drawing.Size(213, 38);
            this.refreshToolStripMenuItem6.Text = "Refresh";
            // 
            // removeToolStripMenuItem1
            // 
            this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            this.removeToolStripMenuItem1.Size = new System.Drawing.Size(213, 38);
            this.removeToolStripMenuItem1.Text = "Remove";
            this.removeToolStripMenuItem1.Click += new System.EventHandler(this.btnRemoveApplication_Click);
            // 
            // toolStripMenuItem24
            // 
            this.toolStripMenuItem24.Name = "toolStripMenuItem24";
            this.toolStripMenuItem24.Size = new System.Drawing.Size(210, 6);
            // 
            // switchToContentViewToolStripMenuItem3
            // 
            this.switchToContentViewToolStripMenuItem3.Enabled = false;
            this.switchToContentViewToolStripMenuItem3.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.switchToContentViewToolStripMenuItem3.Name = "switchToContentViewToolStripMenuItem3";
            this.switchToContentViewToolStripMenuItem3.Size = new System.Drawing.Size(213, 38);
            this.switchToContentViewToolStripMenuItem3.Text = "Switch to Content View";
            // 
            // cmsFarm
            // 
            this.cmsFarm.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsFarm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem7,
            this.toolStripMenuItem29,
            this.btnCreateFarm,
            this.toolStripMenuItem30,
            this.switchToContentViewToolStripMenuItem4});
            this.cmsFarm.Name = "cmsFarm";
            this.cmsFarm.Size = new System.Drawing.Size(214, 130);
            // 
            // refreshToolStripMenuItem7
            // 
            this.refreshToolStripMenuItem7.Enabled = false;
            this.refreshToolStripMenuItem7.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem7.Name = "refreshToolStripMenuItem7";
            this.refreshToolStripMenuItem7.Size = new System.Drawing.Size(213, 38);
            this.refreshToolStripMenuItem7.Text = "Refresh";
            // 
            // toolStripMenuItem29
            // 
            this.toolStripMenuItem29.Name = "toolStripMenuItem29";
            this.toolStripMenuItem29.Size = new System.Drawing.Size(210, 6);
            // 
            // btnCreateFarm
            // 
            this.btnCreateFarm.Name = "btnCreateFarm";
            this.btnCreateFarm.Size = new System.Drawing.Size(213, 38);
            this.btnCreateFarm.Text = "Create Server Farm...";
            this.btnCreateFarm.Click += new System.EventHandler(this.btnCreateFarm_Click);
            // 
            // toolStripMenuItem30
            // 
            this.toolStripMenuItem30.Name = "toolStripMenuItem30";
            this.toolStripMenuItem30.Size = new System.Drawing.Size(210, 6);
            // 
            // switchToContentViewToolStripMenuItem4
            // 
            this.switchToContentViewToolStripMenuItem4.Enabled = false;
            this.switchToContentViewToolStripMenuItem4.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.switchToContentViewToolStripMenuItem4.Name = "switchToContentViewToolStripMenuItem4";
            this.switchToContentViewToolStripMenuItem4.Size = new System.Drawing.Size(213, 38);
            this.switchToContentViewToolStripMenuItem4.Text = "Switch to Content View";
            // 
            // cmsFarmServer
            // 
            this.cmsFarmServer.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsFarmServer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem8,
            this.btnRemoveFarmServer,
            this.toolStripMenuItem31,
            this.btnAddFarmServer,
            this.toolStripMenuItem32,
            this.btnRenameFarmServer,
            this.toolStripMenuItem33,
            this.switchToContentViewToolStripMenuItem5});
            this.cmsFarmServer.Name = "cmsFarmServer";
            this.cmsFarmServer.Size = new System.Drawing.Size(214, 212);
            // 
            // refreshToolStripMenuItem8
            // 
            this.refreshToolStripMenuItem8.Enabled = false;
            this.refreshToolStripMenuItem8.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem8.Name = "refreshToolStripMenuItem8";
            this.refreshToolStripMenuItem8.Size = new System.Drawing.Size(213, 38);
            this.refreshToolStripMenuItem8.Text = "Refresh";
            // 
            // btnRemoveFarmServer
            // 
            this.btnRemoveFarmServer.Name = "btnRemoveFarmServer";
            this.btnRemoveFarmServer.Size = new System.Drawing.Size(213, 38);
            this.btnRemoveFarmServer.Text = "Remove";
            this.btnRemoveFarmServer.Click += new System.EventHandler(this.btnRemoveFarmServer_Click);
            // 
            // toolStripMenuItem31
            // 
            this.toolStripMenuItem31.Name = "toolStripMenuItem31";
            this.toolStripMenuItem31.Size = new System.Drawing.Size(210, 6);
            // 
            // btnAddFarmServer
            // 
            this.btnAddFarmServer.Name = "btnAddFarmServer";
            this.btnAddFarmServer.Size = new System.Drawing.Size(213, 38);
            this.btnAddFarmServer.Text = "Add Server...";
            this.btnAddFarmServer.Click += new System.EventHandler(this.btnAddFarmServer_Click);
            // 
            // toolStripMenuItem32
            // 
            this.toolStripMenuItem32.Name = "toolStripMenuItem32";
            this.toolStripMenuItem32.Size = new System.Drawing.Size(210, 6);
            // 
            // btnRenameFarmServer
            // 
            this.btnRenameFarmServer.Enabled = false;
            this.btnRenameFarmServer.Name = "btnRenameFarmServer";
            this.btnRenameFarmServer.Size = new System.Drawing.Size(213, 38);
            this.btnRenameFarmServer.Text = "Rename";
            // 
            // toolStripMenuItem33
            // 
            this.toolStripMenuItem33.Name = "toolStripMenuItem33";
            this.toolStripMenuItem33.Size = new System.Drawing.Size(210, 6);
            // 
            // switchToContentViewToolStripMenuItem5
            // 
            this.switchToContentViewToolStripMenuItem5.Enabled = false;
            this.switchToContentViewToolStripMenuItem5.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.switchToContentViewToolStripMenuItem5.Name = "switchToContentViewToolStripMenuItem5";
            this.switchToContentViewToolStripMenuItem5.Size = new System.Drawing.Size(213, 38);
            this.switchToContentViewToolStripMenuItem5.Text = "Switch to Content View";
            // 
            // cmsServers
            // 
            this.cmsServers.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsServers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem9,
            this.toolStripMenuItem34,
            this.addServerToolStripMenuItem,
            this.toolStripMenuItem35,
            this.switchToContentViewToolStripMenuItem6});
            this.cmsServers.Name = "cmsServers";
            this.cmsServers.Size = new System.Drawing.Size(214, 130);
            // 
            // refreshToolStripMenuItem9
            // 
            this.refreshToolStripMenuItem9.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.refreshToolStripMenuItem9.Name = "refreshToolStripMenuItem9";
            this.refreshToolStripMenuItem9.Size = new System.Drawing.Size(213, 38);
            this.refreshToolStripMenuItem9.Text = "Refresh";
            // 
            // toolStripMenuItem34
            // 
            this.toolStripMenuItem34.Name = "toolStripMenuItem34";
            this.toolStripMenuItem34.Size = new System.Drawing.Size(210, 6);
            // 
            // addServerToolStripMenuItem
            // 
            this.addServerToolStripMenuItem.Name = "addServerToolStripMenuItem";
            this.addServerToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.addServerToolStripMenuItem.Text = "Add Server...";
            // 
            // toolStripMenuItem35
            // 
            this.toolStripMenuItem35.Name = "toolStripMenuItem35";
            this.toolStripMenuItem35.Size = new System.Drawing.Size(210, 6);
            // 
            // switchToContentViewToolStripMenuItem6
            // 
            this.switchToContentViewToolStripMenuItem6.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.switchToContentViewToolStripMenuItem6.Name = "switchToContentViewToolStripMenuItem6";
            this.switchToContentViewToolStripMenuItem6.Size = new System.Drawing.Size(213, 38);
            this.switchToContentViewToolStripMenuItem6.Text = "Switch to Content View";
            // 
            // cmsVirtualDirectory
            // 
            this.cmsVirtualDirectory.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsVirtualDirectory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem36,
            this.toolStripMenuItem37,
            this.toolStripSeparator3,
            this.convertToApplicationToolStripMenuItem,
            this.toolStripMenuItem38,
            this.toolStripMenuItem39,
            this.toolStripSeparator4,
            this.manageVirtualDirectoryToolStripMenuItem,
            this.toolStripSeparator6,
            this.toolStripMenuItem43,
            this.toolStripMenuItem44,
            this.toolStripSeparator7,
            this.toolStripMenuItem45});
            this.cmsVirtualDirectory.Name = "cmsApplication";
            this.cmsVirtualDirectory.Size = new System.Drawing.Size(222, 370);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(218, 6);
            // 
            // convertToApplicationToolStripMenuItem
            // 
            this.convertToApplicationToolStripMenuItem.Image = global::JexusManager.Main.Properties.Resources.application_new_16;
            this.convertToApplicationToolStripMenuItem.Name = "convertToApplicationToolStripMenuItem";
            this.convertToApplicationToolStripMenuItem.Size = new System.Drawing.Size(221, 38);
            this.convertToApplicationToolStripMenuItem.Text = "Convert to Application";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(218, 6);
            // 
            // manageVirtualDirectoryToolStripMenuItem
            // 
            this.manageVirtualDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem41,
            this.toolStripSeparator5,
            this.toolStripMenuItem42});
            this.manageVirtualDirectoryToolStripMenuItem.Name = "manageVirtualDirectoryToolStripMenuItem";
            this.manageVirtualDirectoryToolStripMenuItem.Size = new System.Drawing.Size(221, 38);
            this.manageVirtualDirectoryToolStripMenuItem.Text = "Manage Virtual Directory";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(178, 6);
            // 
            // toolStripMenuItem42
            // 
            this.toolStripMenuItem42.Enabled = false;
            this.toolStripMenuItem42.Name = "toolStripMenuItem42";
            this.toolStripMenuItem42.Size = new System.Drawing.Size(181, 22);
            this.toolStripMenuItem42.Text = "Advanced Settings...";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(218, 6);
            // 
            // toolStripMenuItem43
            // 
            this.toolStripMenuItem43.Enabled = false;
            this.toolStripMenuItem43.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.toolStripMenuItem43.Name = "toolStripMenuItem43";
            this.toolStripMenuItem43.Size = new System.Drawing.Size(221, 38);
            this.toolStripMenuItem43.Text = "Refresh";
            // 
            // toolStripMenuItem44
            // 
            this.toolStripMenuItem44.Name = "toolStripMenuItem44";
            this.toolStripMenuItem44.Size = new System.Drawing.Size(221, 38);
            this.toolStripMenuItem44.Text = "Remove";
            this.toolStripMenuItem44.Click += new System.EventHandler(this.btnRemoveVirtualDirectory_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(218, 6);
            // 
            // toolStripMenuItem45
            // 
            this.toolStripMenuItem45.Enabled = false;
            this.toolStripMenuItem45.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.toolStripMenuItem45.Name = "toolStripMenuItem45";
            this.toolStripMenuItem45.Size = new System.Drawing.Size(221, 38);
            this.toolStripMenuItem45.Text = "Switch to Content View";
            // 
            // cmsPhysicalDirectory
            // 
            this.cmsPhysicalDirectory.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsPhysicalDirectory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem46,
            this.toolStripMenuItem47,
            this.toolStripSeparator8,
            this.toolStripMenuItem48,
            this.toolStripMenuItem49,
            this.toolStripMenuItem50,
            this.toolStripSeparator9,
            this.manageFolderToolStripMenuItem,
            this.toolStripSeparator11,
            this.toolStripMenuItem54,
            this.toolStripSeparator12,
            this.toolStripMenuItem56});
            this.cmsPhysicalDirectory.Name = "cmsPhysicalDirectory";
            this.cmsPhysicalDirectory.Size = new System.Drawing.Size(214, 332);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(210, 6);
            // 
            // toolStripMenuItem48
            // 
            this.toolStripMenuItem48.Image = global::JexusManager.Main.Properties.Resources.application_new_16;
            this.toolStripMenuItem48.Name = "toolStripMenuItem48";
            this.toolStripMenuItem48.Size = new System.Drawing.Size(213, 38);
            this.toolStripMenuItem48.Text = "Convert to Application";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(210, 6);
            // 
            // manageFolderToolStripMenuItem
            // 
            this.manageFolderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem52});
            this.manageFolderToolStripMenuItem.Name = "manageFolderToolStripMenuItem";
            this.manageFolderToolStripMenuItem.Size = new System.Drawing.Size(213, 38);
            this.manageFolderToolStripMenuItem.Text = "Manage Folder";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(210, 6);
            // 
            // toolStripMenuItem54
            // 
            this.toolStripMenuItem54.Enabled = false;
            this.toolStripMenuItem54.Image = global::JexusManager.Main.Properties.Resources.refresh_16;
            this.toolStripMenuItem54.Name = "toolStripMenuItem54";
            this.toolStripMenuItem54.Size = new System.Drawing.Size(213, 38);
            this.toolStripMenuItem54.Text = "Refresh";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(210, 6);
            // 
            // toolStripMenuItem56
            // 
            this.toolStripMenuItem56.Enabled = false;
            this.toolStripMenuItem56.Image = global::JexusManager.Main.Properties.Resources.switch_16;
            this.toolStripMenuItem56.Name = "toolStripMenuItem56";
            this.toolStripMenuItem56.Size = new System.Drawing.Size(213, 38);
            this.toolStripMenuItem56.Text = "Switch to Content View";
            // 
            // _logSplitter
            // 
            this._logSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logSplitter.Location = new System.Drawing.Point(0, 24);
            this._logSplitter.Name = "_logSplitter";
            this._logSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _logSplitter.Panel1
            // 
            this._logSplitter.Panel1.Controls.Add(this.scMain);
            this._logSplitter.Panel1MinSize = 100;
            // 
            // _logSplitter.Panel2
            // 
            this._logSplitter.Panel2.Controls.Add(this._logPanel);
            this._logSplitter.Panel2Collapsed = true;
            this._logSplitter.Panel2MinSize = 50;
            this._logSplitter.Size = new System.Drawing.Size(915, 451);
            this._logSplitter.SplitterDistance = 350;
            this._logSplitter.SplitterWidth = 5;
            this._logSplitter.TabIndex = 6;
            // 
            // _logPanel
            // 
            this._logPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._logPanel.Location = new System.Drawing.Point(0, 0);
            this._logPanel.Name = "_logPanel";
            this._logPanel.Size = new System.Drawing.Size(915, 96);
            this._logPanel.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 497);
            this.Controls.Add(this._logSplitter);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(738, 504);
            this.Name = "MainForm";
            this.Text = "Jexus Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.scMain.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.cmsServer.ResumeLayout(false);
            this.cmsApplicationPools.ResumeLayout(false);
            this.cmsSites.ResumeLayout(false);
            this.cmsSite.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.actionList1)).EndInit();
            this.cmsIis.ResumeLayout(false);
            this.cmsApplication.ResumeLayout(false);
            this.cmsFarm.ResumeLayout(false);
            this.cmsFarmServer.ResumeLayout(false);
            this.cmsServers.ResumeLayout(false);
            this.cmsVirtualDirectory.ResumeLayout(false);
            this.cmsPhysicalDirectory.ResumeLayout(false);
            this._logSplitter.Panel1.ResumeLayout(false);
            this._logSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._logSplitter)).EndInit();
            this._logSplitter.ResumeLayout(false);
            this._logPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SplitContainer scMain;
        private TreeView treeView1;
        private ContextMenuStrip cmsServer;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem addWebsiteToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem btnStartServer;
        private ToolStripMenuItem btnStopServer;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem btnRenameServer;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem switchToContentViewToolStripMenuItem;
        private ContextMenuStrip cmsApplicationPools;
        private ToolStripMenuItem addApplicationPoolToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem refreshToolStripMenuItem1;
        private ContextMenuStrip cmsSites;
        private ToolStripMenuItem addWebsiteToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem refreshToolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem switchToContentViewToolStripMenuItem1;
        private ContextMenuStrip cmsSite;
        private ToolStripMenuItem exploreToolStripMenuItem;
        private ToolStripMenuItem editPermissionsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem btnApplication;
        private ToolStripMenuItem addVirtualDirectoryToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem editBindingsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripMenuItem manageWebsiteToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem13;
        private ToolStripMenuItem refreshToolStripMenuItem3;
        private ToolStripMenuItem removeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem14;
        private ToolStripMenuItem renameToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem17;
        private ToolStripMenuItem switchToContentViewToolStripMenuItem2;
        private ImageList imageList1;
        private StatusStrip statusStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem iISHelpToolStripMenuItem;
        private ToolStripMenuItem iISOnMSDNOnlineToolStripMenuItem;
        private ToolStripMenuItem iISNETOnlineToolStripMenuItem;
        private ToolStripMenuItem iISKBsOnlineToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem18;
        private ToolStripMenuItem btnAbout;
        private ToolStripMenuItem backToolStripMenuItem;
        private ToolStripMenuItem forwardToolStripMenuItem;
        private ToolStripMenuItem upOneLevelToolStripMenuItem;
        private ToolStripMenuItem homeToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem19;
        private ToolStripMenuItem stopToolStripMenuItem1;
        private ToolStripMenuItem refreshToolStripMenuItem4;
        private ToolStripSeparator toolStripMenuItem20;
        private ToolStripMenuItem groupByToolStripMenuItem;
        private ToolStripMenuItem sortByToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private ToolStrip toolStrip2;
        private ToolStripButton btnUp;
        private Panel panel1;
        private Label label1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem saveConnectionsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem21;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripSplitButton toolStripButton3;
        private ToolStripMenuItem btnServer;
        private ToolStripMenuItem connectToWebsiteToolStripMenuItem;
        private ToolStripMenuItem connectToApplicationToolStripMenuItem;
        private ToolStripButton btnSave;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnDisconnect;
        private ToolStripMenuItem connectToAServerToolStripMenuItem;
        private ToolStripMenuItem connectToAWebsiteToolStripMenuItem;
        private ToolStripMenuItem connectToAnApplicationToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem22;
        private ToolStripMenuItem disconnectToolStripMenuItem;
        private ActionList actionList1;
        private Action actDisconnect;
        private Action actUp;
        private Action actConnectServer;
        private Action actConnectSite;
        private Action actConnectionApplication;
        private Action actSave;
        private ContextMenuStrip cmsIis;
        private ToolStripMenuItem refreshToolStripMenuItem5;
        private ToolStripSeparator toolStripMenuItem23;
        private ToolStripMenuItem connectToAServerToolStripMenuItem1;
        private ToolStripMenuItem connectToAWebsiteToolStripMenuItem1;
        private ToolStripMenuItem connectToAnApplicationToolStripMenuItem1;
        private ContextMenuStrip cmsApplication;
        private ToolStripMenuItem removeConnectionToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private Action actCreateSite;
        private Action actCreateApplication;
        private ToolStripMenuItem exploreToolStripMenuItem1;
        private ToolStripMenuItem editPermissionsToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem addApplicationToolStripMenuItem;
        private ToolStripMenuItem addVirtualDirectoryToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem15;
        private ToolStripMenuItem manageApplicationToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem16;
        private ToolStripMenuItem refreshToolStripMenuItem6;
        private ToolStripMenuItem removeToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem24;
        private ToolStripMenuItem switchToContentViewToolStripMenuItem3;
        private ToolStripMenuItem btnBrowseApplication;
        private ToolStripSeparator toolStripMenuItem25;
        private ToolStripMenuItem advancedSettingsToolStripMenuItem;
        private ToolStripMenuItem btnRestartSite;
        private ToolStripMenuItem startToolStripMenuItem1;
        private ToolStripMenuItem stopToolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem26;
        private ToolStripMenuItem btnBrowseSite;
        private ToolStripSeparator toolStripMenuItem27;
        private ToolStripMenuItem advancedSettingsToolStripMenuItem1;
        private ToolStripStatusLabel txtInfo;
        private ToolStripSeparator toolStripMenuItem28;
        private ToolStripMenuItem btnUpdate;
        private ContextMenuStrip cmsFarm;
        private ToolStripMenuItem refreshToolStripMenuItem7;
        private ToolStripSeparator toolStripMenuItem29;
        private ToolStripMenuItem btnCreateFarm;
        private ToolStripSeparator toolStripMenuItem30;
        private ToolStripMenuItem switchToContentViewToolStripMenuItem4;
        private ContextMenuStrip cmsFarmServer;
        private ToolStripMenuItem refreshToolStripMenuItem8;
        private ToolStripMenuItem btnRemoveFarmServer;
        private ToolStripSeparator toolStripMenuItem31;
        private ToolStripMenuItem btnAddFarmServer;
        private ToolStripSeparator toolStripMenuItem32;
        private ToolStripMenuItem switchToContentViewToolStripMenuItem5;
        private ToolStripMenuItem btnRenameFarmServer;
        private ToolStripSeparator toolStripMenuItem33;
        private ContextMenuStrip cmsServers;
        private ToolStripMenuItem refreshToolStripMenuItem9;
        private ToolStripSeparator toolStripMenuItem34;
        private ToolStripMenuItem addServerToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem35;
        private ToolStripMenuItem switchToContentViewToolStripMenuItem6;
        private ContextMenuStrip cmsVirtualDirectory;
        private ToolStripMenuItem toolStripMenuItem36;
        private ToolStripMenuItem toolStripMenuItem37;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem convertToApplicationToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem38;
        private ToolStripMenuItem toolStripMenuItem39;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem manageVirtualDirectoryToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem41;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem toolStripMenuItem42;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem toolStripMenuItem43;
        private ToolStripMenuItem toolStripMenuItem44;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem toolStripMenuItem45;
        private ContextMenuStrip cmsPhysicalDirectory;
        private ToolStripMenuItem toolStripMenuItem46;
        private ToolStripMenuItem toolStripMenuItem47;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem toolStripMenuItem48;
        private ToolStripMenuItem toolStripMenuItem49;
        private ToolStripMenuItem toolStripMenuItem50;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem manageFolderToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem52;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem toolStripMenuItem54;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripMenuItem toolStripMenuItem56;
        private Action actCreateVirtualDirectory;
        private Action actExplore;
        private Action actEditPermissions;
        private Action actBrowse;
        private ToolStripMenuItem btnOpenConfig;
        private ToolStripSeparator toolStripMenuItem40;
        private Action actRunAsAdmin;
        private ToolStripMenuItem runAsAdministratorToolStripMenuItem;
        private Action actBack;
        private Action actForward;
        private SplitContainer _logSplitter;
        private Panel _logPanel;
        private ToolStripStatusLabel txtPathToSite;
    }
}

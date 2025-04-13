namespace JexusManager
{
    using System.ComponentModel;
    using System.Windows.Forms;

    using Crad.Windows.Forms.Actions;
    using JexusManager.Breadcrumb;

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
            components = new Container();
            scMain = new SplitContainer();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            treeView1 = new TreeView();
            imageList1 = new ImageList(components);
            toolStrip2 = new ToolStrip();
            toolStripButton3 = new ToolStripSplitButton();
            btnServer = new ToolStripMenuItem();
            connectToWebsiteToolStripMenuItem = new ToolStripMenuItem();
            connectToApplicationToolStripMenuItem = new ToolStripMenuItem();
            btnSave = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            btnUp = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnDisconnect = new ToolStripButton();
            panel1 = new Panel();
            label1 = new Label();
            cmsServer = new ContextMenuStrip(components);
            refreshToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            removeConnectionToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            addWebsiteToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            btnStartServer = new ToolStripMenuItem();
            btnStopServer = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            btnRenameServer = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            btnOpenConfig = new ToolStripMenuItem();
            toolStripMenuItem40 = new ToolStripSeparator();
            switchToContentViewToolStripMenuItem = new ToolStripMenuItem();
            cmsApplicationPools = new ContextMenuStrip(components);
            addApplicationPoolToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripSeparator();
            refreshToolStripMenuItem1 = new ToolStripMenuItem();
            cmsSites = new ContextMenuStrip(components);
            addWebsiteToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripSeparator();
            refreshToolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripSeparator();
            switchToContentViewToolStripMenuItem1 = new ToolStripMenuItem();
            cmsSite = new ContextMenuStrip(components);
            exploreToolStripMenuItem = new ToolStripMenuItem();
            editPermissionsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem10 = new ToolStripSeparator();
            btnApplication = new ToolStripMenuItem();
            addVirtualDirectoryToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem11 = new ToolStripSeparator();
            editBindingsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem12 = new ToolStripSeparator();
            manageWebsiteToolStripMenuItem = new ToolStripMenuItem();
            btnRestartSite = new ToolStripMenuItem();
            startToolStripMenuItem1 = new ToolStripMenuItem();
            stopToolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem26 = new ToolStripSeparator();
            btnBrowseSite = new ToolStripMenuItem();
            toolStripMenuItem27 = new ToolStripSeparator();
            advancedSettingsToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem13 = new ToolStripSeparator();
            refreshToolStripMenuItem3 = new ToolStripMenuItem();
            removeToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem14 = new ToolStripSeparator();
            renameToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem17 = new ToolStripSeparator();
            switchToContentViewToolStripMenuItem2 = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            txtInfo = new ToolStripStatusLabel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            connectToAServerToolStripMenuItem = new ToolStripMenuItem();
            connectToAWebsiteToolStripMenuItem = new ToolStripMenuItem();
            connectToAnApplicationToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem22 = new ToolStripSeparator();
            saveConnectionsToolStripMenuItem = new ToolStripMenuItem();
            disconnectToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem21 = new ToolStripSeparator();
            runAsAdministratorToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            backToolStripMenuItem = new ToolStripMenuItem();
            forwardToolStripMenuItem = new ToolStripMenuItem();
            upOneLevelToolStripMenuItem = new ToolStripMenuItem();
            homeToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem19 = new ToolStripSeparator();
            stopToolStripMenuItem1 = new ToolStripMenuItem();
            refreshToolStripMenuItem4 = new ToolStripMenuItem();
            toolStripMenuItem20 = new ToolStripSeparator();
            groupByToolStripMenuItem = new ToolStripMenuItem();
            sortByToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem1 = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            iISHelpToolStripMenuItem = new ToolStripMenuItem();
            iISOnMSDNOnlineToolStripMenuItem = new ToolStripMenuItem();
            iISNETOnlineToolStripMenuItem = new ToolStripMenuItem();
            iISKBsOnlineToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem28 = new ToolStripSeparator();
            btnUpdate = new ToolStripMenuItem();
            toolStripMenuItem18 = new ToolStripSeparator();
            btnAbout = new ToolStripMenuItem();
            actionList1 = new ActionList();
            actConnectServer = new Action();
            actSave = new Action();
            actUp = new Action();
            actDisconnect = new Action();
            actCreateSite = new Action();
            actExplore = new Action();
            actEditPermissions = new Action();
            actCreateApplication = new Action();
            actCreateVirtualDirectory = new Action();
            actBrowse = new Action();
            actConnectSite = new Action();
            actConnectionApplication = new Action();
            actRunAsAdmin = new Action();
            actBack = new Action();
            actForward = new Action();
            connectToAServerToolStripMenuItem1 = new ToolStripMenuItem();
            connectToAWebsiteToolStripMenuItem1 = new ToolStripMenuItem();
            connectToAnApplicationToolStripMenuItem1 = new ToolStripMenuItem();
            addApplicationToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem38 = new ToolStripMenuItem();
            toolStripMenuItem49 = new ToolStripMenuItem();
            exploreToolStripMenuItem1 = new ToolStripMenuItem();
            editPermissionsToolStripMenuItem1 = new ToolStripMenuItem();
            addVirtualDirectoryToolStripMenuItem1 = new ToolStripMenuItem();
            btnBrowseApplication = new ToolStripMenuItem();
            toolStripMenuItem36 = new ToolStripMenuItem();
            toolStripMenuItem37 = new ToolStripMenuItem();
            toolStripMenuItem39 = new ToolStripMenuItem();
            toolStripMenuItem41 = new ToolStripMenuItem();
            toolStripMenuItem46 = new ToolStripMenuItem();
            toolStripMenuItem47 = new ToolStripMenuItem();
            toolStripMenuItem50 = new ToolStripMenuItem();
            toolStripMenuItem52 = new ToolStripMenuItem();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            tsbPath = new ToolStripBreadcrumbItem();
            cmsIis = new ContextMenuStrip(components);
            refreshToolStripMenuItem5 = new ToolStripMenuItem();
            toolStripMenuItem23 = new ToolStripSeparator();
            cmsApplication = new ContextMenuStrip(components);
            toolStripMenuItem9 = new ToolStripSeparator();
            toolStripMenuItem15 = new ToolStripSeparator();
            manageApplicationToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem25 = new ToolStripSeparator();
            advancedSettingsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem16 = new ToolStripSeparator();
            refreshToolStripMenuItem6 = new ToolStripMenuItem();
            removeToolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem24 = new ToolStripSeparator();
            switchToContentViewToolStripMenuItem3 = new ToolStripMenuItem();
            cmsServers = new ContextMenuStrip(components);
            refreshToolStripMenuItem9 = new ToolStripMenuItem();
            toolStripMenuItem34 = new ToolStripSeparator();
            addServerToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem35 = new ToolStripSeparator();
            switchToContentViewToolStripMenuItem6 = new ToolStripMenuItem();
            cmsVirtualDirectory = new ContextMenuStrip(components);
            toolStripSeparator3 = new ToolStripSeparator();
            convertToApplicationToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            manageVirtualDirectoryToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            toolStripMenuItem42 = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            toolStripMenuItem43 = new ToolStripMenuItem();
            toolStripMenuItem44 = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            toolStripMenuItem45 = new ToolStripMenuItem();
            cmsPhysicalDirectory = new ContextMenuStrip(components);
            toolStripSeparator8 = new ToolStripSeparator();
            toolStripMenuItem48 = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            manageFolderToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator11 = new ToolStripSeparator();
            toolStripMenuItem54 = new ToolStripMenuItem();
            toolStripSeparator12 = new ToolStripSeparator();
            toolStripMenuItem56 = new ToolStripMenuItem();
            _logSplitter = new SplitContainer();
            _logPanel = new Panel();
            tsTop = new ToolStrip();
            ((ISupportInitialize)scMain).BeginInit();
            scMain.Panel1.SuspendLayout();
            scMain.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            toolStrip2.SuspendLayout();
            panel1.SuspendLayout();
            cmsServer.SuspendLayout();
            cmsApplicationPools.SuspendLayout();
            cmsSites.SuspendLayout();
            cmsSite.SuspendLayout();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((ISupportInitialize)actionList1).BeginInit();
            cmsIis.SuspendLayout();
            cmsApplication.SuspendLayout();
            cmsServers.SuspendLayout();
            cmsVirtualDirectory.SuspendLayout();
            cmsPhysicalDirectory.SuspendLayout();
            ((ISupportInitialize)_logSplitter).BeginInit();
            _logSplitter.Panel1.SuspendLayout();
            _logSplitter.Panel2.SuspendLayout();
            _logSplitter.SuspendLayout();
            tsTop.SuspendLayout();
            SuspendLayout();
            // 
            // scMain
            // 
            scMain.Dock = DockStyle.Fill;
            scMain.Location = new System.Drawing.Point(0, 0);
            scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            scMain.Panel1.Controls.Add(tableLayoutPanel1);
            scMain.Panel1MinSize = 150;
            scMain.Size = new System.Drawing.Size(915, 426);
            scMain.SplitterDistance = 175;
            scMain.SplitterWidth = 5;
            scMain.TabIndex = 0;
            scMain.SplitterMoved += scMain_SplitterMoved;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.Size = new System.Drawing.Size(175, 426);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(treeView1);
            panel2.Controls.Add(toolStrip2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(3, 30);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(169, 393);
            panel2.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.HideSelection = false;
            treeView1.ImageIndex = 0;
            treeView1.ImageList = imageList1;
            treeView1.LabelEdit = true;
            treeView1.Location = new System.Drawing.Point(0, 25);
            treeView1.Name = "treeView1";
            treeView1.SelectedImageIndex = 0;
            treeView1.Size = new System.Drawing.Size(169, 368);
            treeView1.TabIndex = 0;
            treeView1.BeforeLabelEdit += treeView1_BeforeLabelEdit;
            treeView1.AfterLabelEdit += treeView1_AfterLabelEdit;
            treeView1.BeforeExpand += treeView1_BeforeExpand;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.NodeMouseClick += treeView1_NodeMouseClick;
            treeView1.NodeMouseDoubleClick += treeView1_NodeMouseDoubleClick;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip2
            // 
            toolStrip2.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip2.Items.AddRange(new ToolStripItem[] { toolStripButton3, btnSave, toolStripSeparator2, btnUp, toolStripSeparator1, btnDisconnect });
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Padding = new Padding(0, 0, 2, 0);
            toolStrip2.Size = new System.Drawing.Size(169, 25);
            toolStrip2.TabIndex = 6;
            toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.DropDownItems.AddRange(new ToolStripItem[] { btnServer, connectToWebsiteToolStripMenuItem, connectToApplicationToolStripMenuItem });
            toolStripButton3.Image = Main.Properties.Resources.connect_16;
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(32, 22);
            toolStripButton3.Text = "Create New Connection";
            // 
            // btnServer
            // 
            actionList1.SetAction(btnServer, actConnectServer);
            btnServer.Name = "btnServer";
            btnServer.Size = new System.Drawing.Size(222, 22);
            btnServer.Text = "Connect to a Server...";
            btnServer.ToolTipText = "Connect to a Server...";
            // 
            // connectToWebsiteToolStripMenuItem
            // 
            connectToWebsiteToolStripMenuItem.Enabled = false;
            connectToWebsiteToolStripMenuItem.Image = Main.Properties.Resources.site_16;
            connectToWebsiteToolStripMenuItem.Name = "connectToWebsiteToolStripMenuItem";
            connectToWebsiteToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            connectToWebsiteToolStripMenuItem.Text = "Connect to a Website...";
            // 
            // connectToApplicationToolStripMenuItem
            // 
            connectToApplicationToolStripMenuItem.Enabled = false;
            connectToApplicationToolStripMenuItem.Image = Main.Properties.Resources.application_16;
            connectToApplicationToolStripMenuItem.Name = "connectToApplicationToolStripMenuItem";
            connectToApplicationToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            connectToApplicationToolStripMenuItem.Text = "Connect to an Application...";
            // 
            // btnSave
            // 
            actionList1.SetAction(btnSave, actSave);
            btnSave.AutoToolTip = false;
            btnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSave.Enabled = false;
            btnSave.Name = "btnSave";
            btnSave.Size = new System.Drawing.Size(23, 22);
            btnSave.Text = "Save Connections";
            btnSave.ToolTipText = "Save Connections";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUp
            // 
            actionList1.SetAction(btnUp, actUp);
            btnUp.AutoToolTip = false;
            btnUp.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnUp.Enabled = false;
            btnUp.Name = "btnUp";
            btnUp.Size = new System.Drawing.Size(23, 22);
            btnUp.Text = "Up One Level";
            btnUp.ToolTipText = "Up One Level";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDisconnect
            // 
            actionList1.SetAction(btnDisconnect, actDisconnect);
            btnDisconnect.AutoToolTip = false;
            btnDisconnect.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDisconnect.Enabled = false;
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new System.Drawing.Size(23, 22);
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.ToolTipText = "Disconnect";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(175, 27);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(3, 6);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 13);
            label1.TabIndex = 0;
            label1.Text = "Connections";
            // 
            // cmsServer
            // 
            cmsServer.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem, toolStripMenuItem1, removeConnectionToolStripMenuItem, toolStripMenuItem4, addWebsiteToolStripMenuItem, toolStripMenuItem2, btnStartServer, btnStopServer, toolStripMenuItem3, btnRenameServer, toolStripMenuItem5, btnOpenConfig, toolStripMenuItem40, switchToContentViewToolStripMenuItem });
            cmsServer.Name = "cmsServer";
            cmsServer.Size = new System.Drawing.Size(202, 216);
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Enabled = false;
            refreshToolStripMenuItem.Image = Main.Properties.Resources.refresh_16;
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            refreshToolStripMenuItem.Text = "Refresh";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(198, 6);
            // 
            // removeConnectionToolStripMenuItem
            // 
            actionList1.SetAction(removeConnectionToolStripMenuItem, actDisconnect);
            removeConnectionToolStripMenuItem.Enabled = false;
            removeConnectionToolStripMenuItem.Name = "removeConnectionToolStripMenuItem";
            removeConnectionToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            removeConnectionToolStripMenuItem.Text = "Disconnect";
            removeConnectionToolStripMenuItem.ToolTipText = "Disconnect";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(198, 6);
            // 
            // addWebsiteToolStripMenuItem
            // 
            actionList1.SetAction(addWebsiteToolStripMenuItem, actCreateSite);
            addWebsiteToolStripMenuItem.AutoToolTip = true;
            addWebsiteToolStripMenuItem.Name = "addWebsiteToolStripMenuItem";
            addWebsiteToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            addWebsiteToolStripMenuItem.Text = "Add Website...";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(198, 6);
            // 
            // btnStartServer
            // 
            btnStartServer.Enabled = false;
            btnStartServer.Image = Main.Properties.Resources.start_16;
            btnStartServer.Name = "btnStartServer";
            btnStartServer.Size = new System.Drawing.Size(201, 22);
            btnStartServer.Text = "Start";
            // 
            // btnStopServer
            // 
            btnStopServer.Enabled = false;
            btnStopServer.Image = Main.Properties.Resources.stop_16;
            btnStopServer.Name = "btnStopServer";
            btnStopServer.Size = new System.Drawing.Size(201, 22);
            btnStopServer.Text = "Stop";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(198, 6);
            // 
            // btnRenameServer
            // 
            btnRenameServer.Enabled = false;
            btnRenameServer.Name = "btnRenameServer";
            btnRenameServer.Size = new System.Drawing.Size(201, 22);
            btnRenameServer.Text = "Rename";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(198, 6);
            // 
            // btnOpenConfig
            // 
            btnOpenConfig.Name = "btnOpenConfig";
            btnOpenConfig.Size = new System.Drawing.Size(201, 22);
            btnOpenConfig.Text = "Open Configuration File";
            btnOpenConfig.Click += btnOpenConfig_Click;
            // 
            // toolStripMenuItem40
            // 
            toolStripMenuItem40.Name = "toolStripMenuItem40";
            toolStripMenuItem40.Size = new System.Drawing.Size(198, 6);
            // 
            // switchToContentViewToolStripMenuItem
            // 
            switchToContentViewToolStripMenuItem.Enabled = false;
            switchToContentViewToolStripMenuItem.Image = Main.Properties.Resources.switch_16;
            switchToContentViewToolStripMenuItem.Name = "switchToContentViewToolStripMenuItem";
            switchToContentViewToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
            switchToContentViewToolStripMenuItem.Text = "Switch to Content View";
            // 
            // cmsApplicationPools
            // 
            cmsApplicationPools.Items.AddRange(new ToolStripItem[] { addApplicationPoolToolStripMenuItem, toolStripMenuItem6, refreshToolStripMenuItem1 });
            cmsApplicationPools.Name = "cmsApplicationPools";
            cmsApplicationPools.Size = new System.Drawing.Size(197, 54);
            // 
            // addApplicationPoolToolStripMenuItem
            // 
            addApplicationPoolToolStripMenuItem.Enabled = false;
            addApplicationPoolToolStripMenuItem.Name = "addApplicationPoolToolStripMenuItem";
            addApplicationPoolToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            addApplicationPoolToolStripMenuItem.Text = "Add Application Pool...";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(193, 6);
            // 
            // refreshToolStripMenuItem1
            // 
            refreshToolStripMenuItem1.Enabled = false;
            refreshToolStripMenuItem1.Image = Main.Properties.Resources.refresh_16;
            refreshToolStripMenuItem1.Name = "refreshToolStripMenuItem1";
            refreshToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            refreshToolStripMenuItem1.Text = "Refresh";
            // 
            // cmsSites
            // 
            cmsSites.Items.AddRange(new ToolStripItem[] { addWebsiteToolStripMenuItem1, toolStripMenuItem7, refreshToolStripMenuItem2, toolStripMenuItem8, switchToContentViewToolStripMenuItem1 });
            cmsSites.Name = "cmsSites";
            cmsSites.Size = new System.Drawing.Size(198, 82);
            // 
            // addWebsiteToolStripMenuItem1
            // 
            actionList1.SetAction(addWebsiteToolStripMenuItem1, actCreateSite);
            addWebsiteToolStripMenuItem1.AutoToolTip = true;
            addWebsiteToolStripMenuItem1.Name = "addWebsiteToolStripMenuItem1";
            addWebsiteToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            addWebsiteToolStripMenuItem1.Text = "Add Website...";
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new System.Drawing.Size(194, 6);
            // 
            // refreshToolStripMenuItem2
            // 
            refreshToolStripMenuItem2.Enabled = false;
            refreshToolStripMenuItem2.Name = "refreshToolStripMenuItem2";
            refreshToolStripMenuItem2.Size = new System.Drawing.Size(197, 22);
            refreshToolStripMenuItem2.Text = "Refresh";
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new System.Drawing.Size(194, 6);
            // 
            // switchToContentViewToolStripMenuItem1
            // 
            switchToContentViewToolStripMenuItem1.Enabled = false;
            switchToContentViewToolStripMenuItem1.Name = "switchToContentViewToolStripMenuItem1";
            switchToContentViewToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            switchToContentViewToolStripMenuItem1.Text = "Switch to Content View";
            // 
            // cmsSite
            // 
            cmsSite.Items.AddRange(new ToolStripItem[] { exploreToolStripMenuItem, editPermissionsToolStripMenuItem, toolStripMenuItem10, btnApplication, addVirtualDirectoryToolStripMenuItem, toolStripMenuItem11, editBindingsToolStripMenuItem, toolStripMenuItem12, manageWebsiteToolStripMenuItem, toolStripMenuItem13, refreshToolStripMenuItem3, removeToolStripMenuItem, toolStripMenuItem14, renameToolStripMenuItem1, toolStripMenuItem17, switchToContentViewToolStripMenuItem2 });
            cmsSite.Name = "cmsSite";
            cmsSite.Size = new System.Drawing.Size(198, 260);
            // 
            // exploreToolStripMenuItem
            // 
            actionList1.SetAction(exploreToolStripMenuItem, actExplore);
            exploreToolStripMenuItem.AutoToolTip = true;
            exploreToolStripMenuItem.Name = "exploreToolStripMenuItem";
            exploreToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            exploreToolStripMenuItem.Text = "Explore";
            // 
            // editPermissionsToolStripMenuItem
            // 
            actionList1.SetAction(editPermissionsToolStripMenuItem, actEditPermissions);
            editPermissionsToolStripMenuItem.AutoToolTip = true;
            editPermissionsToolStripMenuItem.Name = "editPermissionsToolStripMenuItem";
            editPermissionsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            editPermissionsToolStripMenuItem.Text = "Edit Permissions...";
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new System.Drawing.Size(194, 6);
            // 
            // btnApplication
            // 
            actionList1.SetAction(btnApplication, actCreateApplication);
            btnApplication.AutoToolTip = true;
            btnApplication.Name = "btnApplication";
            btnApplication.Size = new System.Drawing.Size(197, 22);
            btnApplication.Text = "Add Application...";
            // 
            // addVirtualDirectoryToolStripMenuItem
            // 
            actionList1.SetAction(addVirtualDirectoryToolStripMenuItem, actCreateVirtualDirectory);
            addVirtualDirectoryToolStripMenuItem.AutoToolTip = true;
            addVirtualDirectoryToolStripMenuItem.Name = "addVirtualDirectoryToolStripMenuItem";
            addVirtualDirectoryToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            addVirtualDirectoryToolStripMenuItem.Text = "Add Virtual Directory...";
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new System.Drawing.Size(194, 6);
            // 
            // editBindingsToolStripMenuItem
            // 
            editBindingsToolStripMenuItem.Name = "editBindingsToolStripMenuItem";
            editBindingsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            editBindingsToolStripMenuItem.Text = "Edit Bindings...";
            editBindingsToolStripMenuItem.Click += editBindingsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem12
            // 
            toolStripMenuItem12.Name = "toolStripMenuItem12";
            toolStripMenuItem12.Size = new System.Drawing.Size(194, 6);
            // 
            // manageWebsiteToolStripMenuItem
            // 
            manageWebsiteToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { btnRestartSite, startToolStripMenuItem1, stopToolStripMenuItem2, toolStripMenuItem26, btnBrowseSite, toolStripMenuItem27, advancedSettingsToolStripMenuItem1 });
            manageWebsiteToolStripMenuItem.Name = "manageWebsiteToolStripMenuItem";
            manageWebsiteToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            manageWebsiteToolStripMenuItem.Text = "Manage Website";
            // 
            // btnRestartSite
            // 
            btnRestartSite.Image = Main.Properties.Resources.restart_16;
            btnRestartSite.Name = "btnRestartSite";
            btnRestartSite.Size = new System.Drawing.Size(181, 22);
            btnRestartSite.Text = "Restart";
            btnRestartSite.Click += btnRestartSite_Click;
            // 
            // startToolStripMenuItem1
            // 
            startToolStripMenuItem1.Enabled = false;
            startToolStripMenuItem1.Image = Main.Properties.Resources.start_16;
            startToolStripMenuItem1.Name = "startToolStripMenuItem1";
            startToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            startToolStripMenuItem1.Text = "Start";
            // 
            // stopToolStripMenuItem2
            // 
            stopToolStripMenuItem2.Enabled = false;
            stopToolStripMenuItem2.Image = Main.Properties.Resources.stop_16;
            stopToolStripMenuItem2.Name = "stopToolStripMenuItem2";
            stopToolStripMenuItem2.Size = new System.Drawing.Size(181, 22);
            stopToolStripMenuItem2.Text = "Stop";
            // 
            // toolStripMenuItem26
            // 
            toolStripMenuItem26.Name = "toolStripMenuItem26";
            toolStripMenuItem26.Size = new System.Drawing.Size(178, 6);
            // 
            // btnBrowseSite
            // 
            actionList1.SetAction(btnBrowseSite, actBrowse);
            btnBrowseSite.AutoToolTip = true;
            btnBrowseSite.Name = "btnBrowseSite";
            btnBrowseSite.Size = new System.Drawing.Size(181, 22);
            btnBrowseSite.Text = "Browse";
            // 
            // toolStripMenuItem27
            // 
            toolStripMenuItem27.Name = "toolStripMenuItem27";
            toolStripMenuItem27.Size = new System.Drawing.Size(178, 6);
            // 
            // advancedSettingsToolStripMenuItem1
            // 
            advancedSettingsToolStripMenuItem1.Enabled = false;
            advancedSettingsToolStripMenuItem1.Name = "advancedSettingsToolStripMenuItem1";
            advancedSettingsToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            advancedSettingsToolStripMenuItem1.Text = "Advanced Settings...";
            // 
            // toolStripMenuItem13
            // 
            toolStripMenuItem13.Name = "toolStripMenuItem13";
            toolStripMenuItem13.Size = new System.Drawing.Size(194, 6);
            // 
            // refreshToolStripMenuItem3
            // 
            refreshToolStripMenuItem3.Enabled = false;
            refreshToolStripMenuItem3.Image = Main.Properties.Resources.refresh_16;
            refreshToolStripMenuItem3.Name = "refreshToolStripMenuItem3";
            refreshToolStripMenuItem3.Size = new System.Drawing.Size(197, 22);
            refreshToolStripMenuItem3.Text = "Refresh";
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            removeToolStripMenuItem.Text = "Remove";
            removeToolStripMenuItem.Click += btnRemoveSite_Click;
            // 
            // toolStripMenuItem14
            // 
            toolStripMenuItem14.Name = "toolStripMenuItem14";
            toolStripMenuItem14.Size = new System.Drawing.Size(194, 6);
            // 
            // renameToolStripMenuItem1
            // 
            renameToolStripMenuItem1.Name = "renameToolStripMenuItem1";
            renameToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            renameToolStripMenuItem1.Text = "Rename";
            renameToolStripMenuItem1.Click += renameToolStripMenuItem1_Click;
            // 
            // toolStripMenuItem17
            // 
            toolStripMenuItem17.Name = "toolStripMenuItem17";
            toolStripMenuItem17.Size = new System.Drawing.Size(194, 6);
            // 
            // switchToContentViewToolStripMenuItem2
            // 
            switchToContentViewToolStripMenuItem2.Enabled = false;
            switchToContentViewToolStripMenuItem2.Image = Main.Properties.Resources.switch_16;
            switchToContentViewToolStripMenuItem2.Name = "switchToContentViewToolStripMenuItem2";
            switchToContentViewToolStripMenuItem2.Size = new System.Drawing.Size(197, 22);
            switchToContentViewToolStripMenuItem2.Text = "Switch to Content View";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { txtInfo });
            statusStrip1.Location = new System.Drawing.Point(0, 475);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(915, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // txtInfo
            // 
            txtInfo.Image = Main.Properties.Resources.info_16;
            txtInfo.Name = "txtInfo";
            txtInfo.Size = new System.Drawing.Size(134, 17);
            txtInfo.Text = "toolStripStatusLabel1";
            txtInfo.Visible = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 25);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(915, 24);
            menuStrip1.TabIndex = 5;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { connectToAServerToolStripMenuItem, connectToAWebsiteToolStripMenuItem, connectToAnApplicationToolStripMenuItem, toolStripMenuItem22, saveConnectionsToolStripMenuItem, disconnectToolStripMenuItem, toolStripMenuItem21, runAsAdministratorToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // connectToAServerToolStripMenuItem
            // 
            actionList1.SetAction(connectToAServerToolStripMenuItem, actConnectServer);
            connectToAServerToolStripMenuItem.Name = "connectToAServerToolStripMenuItem";
            connectToAServerToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            connectToAServerToolStripMenuItem.Text = "Connect to a Server...";
            connectToAServerToolStripMenuItem.ToolTipText = "Connect to a Server...";
            // 
            // connectToAWebsiteToolStripMenuItem
            // 
            actionList1.SetAction(connectToAWebsiteToolStripMenuItem, actConnectSite);
            connectToAWebsiteToolStripMenuItem.Enabled = false;
            connectToAWebsiteToolStripMenuItem.Name = "connectToAWebsiteToolStripMenuItem";
            connectToAWebsiteToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            connectToAWebsiteToolStripMenuItem.Text = "Connect to a Website...";
            connectToAWebsiteToolStripMenuItem.ToolTipText = "Connect to a Website...";
            // 
            // connectToAnApplicationToolStripMenuItem
            // 
            actionList1.SetAction(connectToAnApplicationToolStripMenuItem, actConnectionApplication);
            connectToAnApplicationToolStripMenuItem.Enabled = false;
            connectToAnApplicationToolStripMenuItem.Name = "connectToAnApplicationToolStripMenuItem";
            connectToAnApplicationToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            connectToAnApplicationToolStripMenuItem.Text = "Connect to an Application...";
            connectToAnApplicationToolStripMenuItem.ToolTipText = "Connect to an Application...";
            // 
            // toolStripMenuItem22
            // 
            toolStripMenuItem22.Name = "toolStripMenuItem22";
            toolStripMenuItem22.Size = new System.Drawing.Size(219, 6);
            // 
            // saveConnectionsToolStripMenuItem
            // 
            actionList1.SetAction(saveConnectionsToolStripMenuItem, actSave);
            saveConnectionsToolStripMenuItem.Enabled = false;
            saveConnectionsToolStripMenuItem.Name = "saveConnectionsToolStripMenuItem";
            saveConnectionsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            saveConnectionsToolStripMenuItem.Text = "Save Connections";
            saveConnectionsToolStripMenuItem.ToolTipText = "Save Connections";
            // 
            // disconnectToolStripMenuItem
            // 
            actionList1.SetAction(disconnectToolStripMenuItem, actDisconnect);
            disconnectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            disconnectToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            disconnectToolStripMenuItem.Text = "Disconnect";
            disconnectToolStripMenuItem.ToolTipText = "Disconnect";
            // 
            // toolStripMenuItem21
            // 
            toolStripMenuItem21.Name = "toolStripMenuItem21";
            toolStripMenuItem21.Size = new System.Drawing.Size(219, 6);
            // 
            // runAsAdministratorToolStripMenuItem
            // 
            actionList1.SetAction(runAsAdministratorToolStripMenuItem, actRunAsAdmin);
            runAsAdministratorToolStripMenuItem.AutoToolTip = true;
            runAsAdministratorToolStripMenuItem.Name = "runAsAdministratorToolStripMenuItem";
            runAsAdministratorToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            runAsAdministratorToolStripMenuItem.Text = "Run as Administrator";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { backToolStripMenuItem, forwardToolStripMenuItem, upOneLevelToolStripMenuItem, homeToolStripMenuItem, toolStripMenuItem19, stopToolStripMenuItem1, refreshToolStripMenuItem4, toolStripMenuItem20, groupByToolStripMenuItem, sortByToolStripMenuItem, viewToolStripMenuItem1 });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // backToolStripMenuItem
            // 
            actionList1.SetAction(backToolStripMenuItem, actBack);
            backToolStripMenuItem.Enabled = false;
            backToolStripMenuItem.Name = "backToolStripMenuItem";
            backToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            backToolStripMenuItem.Text = "Back";
            // 
            // forwardToolStripMenuItem
            // 
            actionList1.SetAction(forwardToolStripMenuItem, actForward);
            forwardToolStripMenuItem.Enabled = false;
            forwardToolStripMenuItem.Name = "forwardToolStripMenuItem";
            forwardToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            forwardToolStripMenuItem.Text = "Forward";
            // 
            // upOneLevelToolStripMenuItem
            // 
            actionList1.SetAction(upOneLevelToolStripMenuItem, actUp);
            upOneLevelToolStripMenuItem.Enabled = false;
            upOneLevelToolStripMenuItem.Name = "upOneLevelToolStripMenuItem";
            upOneLevelToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            upOneLevelToolStripMenuItem.Text = "Up One Level";
            upOneLevelToolStripMenuItem.ToolTipText = "Up One Level";
            // 
            // homeToolStripMenuItem
            // 
            homeToolStripMenuItem.Enabled = false;
            homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            homeToolStripMenuItem.ShortcutKeys = Keys.Alt | Keys.Home;
            homeToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            homeToolStripMenuItem.Text = "Home";
            // 
            // toolStripMenuItem19
            // 
            toolStripMenuItem19.Name = "toolStripMenuItem19";
            toolStripMenuItem19.Size = new System.Drawing.Size(167, 6);
            // 
            // stopToolStripMenuItem1
            // 
            stopToolStripMenuItem1.Enabled = false;
            stopToolStripMenuItem1.Name = "stopToolStripMenuItem1";
            stopToolStripMenuItem1.Size = new System.Drawing.Size(170, 22);
            stopToolStripMenuItem1.Text = "Stop";
            // 
            // refreshToolStripMenuItem4
            // 
            refreshToolStripMenuItem4.Enabled = false;
            refreshToolStripMenuItem4.Image = Main.Properties.Resources.refresh_16;
            refreshToolStripMenuItem4.Name = "refreshToolStripMenuItem4";
            refreshToolStripMenuItem4.Size = new System.Drawing.Size(170, 22);
            refreshToolStripMenuItem4.Text = "Refresh";
            // 
            // toolStripMenuItem20
            // 
            toolStripMenuItem20.Name = "toolStripMenuItem20";
            toolStripMenuItem20.Size = new System.Drawing.Size(167, 6);
            // 
            // groupByToolStripMenuItem
            // 
            groupByToolStripMenuItem.Enabled = false;
            groupByToolStripMenuItem.Name = "groupByToolStripMenuItem";
            groupByToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            groupByToolStripMenuItem.Text = "Group By";
            // 
            // sortByToolStripMenuItem
            // 
            sortByToolStripMenuItem.Enabled = false;
            sortByToolStripMenuItem.Name = "sortByToolStripMenuItem";
            sortByToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            sortByToolStripMenuItem.Text = "Sort By";
            // 
            // viewToolStripMenuItem1
            // 
            viewToolStripMenuItem1.Enabled = false;
            viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            viewToolStripMenuItem1.Size = new System.Drawing.Size(170, 22);
            viewToolStripMenuItem1.Text = "View";
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { iISHelpToolStripMenuItem, iISOnMSDNOnlineToolStripMenuItem, iISNETOnlineToolStripMenuItem, iISKBsOnlineToolStripMenuItem, toolStripMenuItem28, btnUpdate, toolStripMenuItem18, btnAbout });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // iISHelpToolStripMenuItem
            // 
            iISHelpToolStripMenuItem.Image = Main.Properties.Resources.help_16;
            iISHelpToolStripMenuItem.Name = "iISHelpToolStripMenuItem";
            iISHelpToolStripMenuItem.ShortcutKeys = Keys.F1;
            iISHelpToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            iISHelpToolStripMenuItem.Text = "IIS Help";
            // 
            // iISOnMSDNOnlineToolStripMenuItem
            // 
            iISOnMSDNOnlineToolStripMenuItem.Name = "iISOnMSDNOnlineToolStripMenuItem";
            iISOnMSDNOnlineToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            iISOnMSDNOnlineToolStripMenuItem.Text = "IIS on MSDN Online";
            iISOnMSDNOnlineToolStripMenuItem.Click += iISOnMSDNOnlineToolStripMenuItem_Click;
            // 
            // iISNETOnlineToolStripMenuItem
            // 
            iISNETOnlineToolStripMenuItem.Name = "iISNETOnlineToolStripMenuItem";
            iISNETOnlineToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            iISNETOnlineToolStripMenuItem.Text = "IIS.NET Online";
            iISNETOnlineToolStripMenuItem.Click += iISNETOnlineToolStripMenuItem_Click;
            // 
            // iISKBsOnlineToolStripMenuItem
            // 
            iISKBsOnlineToolStripMenuItem.Name = "iISKBsOnlineToolStripMenuItem";
            iISKBsOnlineToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            iISKBsOnlineToolStripMenuItem.Text = "IIS KBs Online";
            iISKBsOnlineToolStripMenuItem.Click += iISKBsOnlineToolStripMenuItem_Click;
            // 
            // toolStripMenuItem28
            // 
            toolStripMenuItem28.Name = "toolStripMenuItem28";
            toolStripMenuItem28.Size = new System.Drawing.Size(184, 6);
            // 
            // btnUpdate
            // 
            btnUpdate.Image = Main.Properties.Resources.update_16;
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(187, 22);
            btnUpdate.Text = "Check Update";
            btnUpdate.Click += btnUpdate_Click;
            // 
            // toolStripMenuItem18
            // 
            toolStripMenuItem18.Name = "toolStripMenuItem18";
            toolStripMenuItem18.Size = new System.Drawing.Size(184, 6);
            // 
            // btnAbout
            // 
            btnAbout.Name = "btnAbout";
            btnAbout.Size = new System.Drawing.Size(187, 22);
            btnAbout.Text = "About Jexus Manager";
            btnAbout.Click += btnAbout_Click;
            // 
            // actionList1
            // 
            actionList1.Actions.Add(actDisconnect);
            actionList1.Actions.Add(actUp);
            actionList1.Actions.Add(actConnectServer);
            actionList1.Actions.Add(actConnectSite);
            actionList1.Actions.Add(actConnectionApplication);
            actionList1.Actions.Add(actSave);
            actionList1.Actions.Add(actCreateSite);
            actionList1.Actions.Add(actCreateApplication);
            actionList1.Actions.Add(actCreateVirtualDirectory);
            actionList1.Actions.Add(actExplore);
            actionList1.Actions.Add(actEditPermissions);
            actionList1.Actions.Add(actBrowse);
            actionList1.Actions.Add(actRunAsAdmin);
            actionList1.Actions.Add(actBack);
            actionList1.Actions.Add(actForward);
            actionList1.ContainerControl = this;
            // 
            // actConnectServer
            // 
            actConnectServer.Image = Main.Properties.Resources.server_16;
            actConnectServer.Text = "Connect to a Server...";
            actConnectServer.ToolTipText = "Connect to a Server...";
            actConnectServer.Execute += actConnectServer_Execute;
            // 
            // actSave
            // 
            actSave.Enabled = false;
            actSave.Image = Main.Properties.Resources.save_16;
            actSave.Text = "Save Connections";
            actSave.ToolTipText = "Save Connections";
            actSave.Execute += actSave_Execute;
            // 
            // actUp
            // 
            actUp.Enabled = false;
            actUp.Image = Main.Properties.Resources.up_16;
            actUp.ShortcutKeys = Keys.Alt | Keys.Up;
            actUp.Text = "Up One Level";
            actUp.ToolTipText = "Up One Level";
            actUp.Execute += actUp_Execute;
            // 
            // actDisconnect
            // 
            actDisconnect.Enabled = false;
            actDisconnect.Image = Main.Properties.Resources.disconnected_16;
            actDisconnect.Text = "Disconnect";
            actDisconnect.ToolTipText = "Disconnect";
            actDisconnect.Execute += actDisconnect_Execute;
            // 
            // actCreateSite
            // 
            actCreateSite.Image = Main.Properties.Resources.site_new_16;
            actCreateSite.Text = "Add Website...";
            actCreateSite.Execute += actCreateSite_Execute;
            // 
            // actExplore
            // 
            actExplore.Image = Main.Properties.Resources.explore_16;
            actExplore.Text = "Explore";
            actExplore.Execute += actExplore_Execute;
            // 
            // actEditPermissions
            // 
            actEditPermissions.Text = "Edit Permissions...";
            actEditPermissions.Execute += actEditPermissions_Execute;
            // 
            // actCreateApplication
            // 
            actCreateApplication.Image = Main.Properties.Resources.application_new_16;
            actCreateApplication.Text = "Add Application...";
            actCreateApplication.Execute += actCreateApplication_Execute;
            // 
            // actCreateVirtualDirectory
            // 
            actCreateVirtualDirectory.Image = Main.Properties.Resources.virtual_directory_new_16;
            actCreateVirtualDirectory.Text = "Add Virtual Directory...";
            actCreateVirtualDirectory.Execute += actCreateVirtualDirectory_Execute;
            // 
            // actBrowse
            // 
            actBrowse.Image = Main.Properties.Resources.browse_16;
            actBrowse.Text = "Browse";
            actBrowse.Execute += actBrowse_Execute;
            // 
            // actConnectSite
            // 
            actConnectSite.Enabled = false;
            actConnectSite.Image = Main.Properties.Resources.site_16;
            actConnectSite.Text = "Connect to a Website...";
            actConnectSite.ToolTipText = "Connect to a Website...";
            actConnectSite.Execute += actConnectSite_Execute;
            // 
            // actConnectionApplication
            // 
            actConnectionApplication.Enabled = false;
            actConnectionApplication.Image = Main.Properties.Resources.application_16;
            actConnectionApplication.Text = "Connect to an Application...";
            actConnectionApplication.ToolTipText = "Connect to an Application...";
            // 
            // actRunAsAdmin
            // 
            actRunAsAdmin.Text = "Run as Administrator";
            actRunAsAdmin.Execute += actRunAsAdmin_Execute;
            // 
            // actBack
            // 
            actBack.Enabled = false;
            actBack.Image = Main.Properties.Resources.back_16;
            actBack.ShortcutKeys = Keys.Alt | Keys.Left;
            actBack.Text = "Back";
            actBack.Execute += actBack_Execute;
            // 
            // actForward
            // 
            actForward.Enabled = false;
            actForward.Image = Main.Properties.Resources.forward_16;
            actForward.ShortcutKeys = Keys.Alt | Keys.Right;
            actForward.Text = "Forward";
            actForward.Execute += actForward_Execute;
            // 
            // connectToAServerToolStripMenuItem1
            // 
            actionList1.SetAction(connectToAServerToolStripMenuItem1, actConnectServer);
            connectToAServerToolStripMenuItem1.Image = Main.Properties.Resources.server_16;
            connectToAServerToolStripMenuItem1.Name = "connectToAServerToolStripMenuItem1";
            connectToAServerToolStripMenuItem1.Size = new System.Drawing.Size(222, 22);
            connectToAServerToolStripMenuItem1.Text = "Connect to a Server...";
            connectToAServerToolStripMenuItem1.ToolTipText = "Connect to a Server...";
            // 
            // connectToAWebsiteToolStripMenuItem1
            // 
            actionList1.SetAction(connectToAWebsiteToolStripMenuItem1, actConnectSite);
            connectToAWebsiteToolStripMenuItem1.Enabled = false;
            connectToAWebsiteToolStripMenuItem1.Image = Main.Properties.Resources.site_16;
            connectToAWebsiteToolStripMenuItem1.Name = "connectToAWebsiteToolStripMenuItem1";
            connectToAWebsiteToolStripMenuItem1.Size = new System.Drawing.Size(222, 22);
            connectToAWebsiteToolStripMenuItem1.Text = "Connect to a Website...";
            connectToAWebsiteToolStripMenuItem1.ToolTipText = "Connect to a Website...";
            // 
            // connectToAnApplicationToolStripMenuItem1
            // 
            actionList1.SetAction(connectToAnApplicationToolStripMenuItem1, actConnectionApplication);
            connectToAnApplicationToolStripMenuItem1.Enabled = false;
            connectToAnApplicationToolStripMenuItem1.Image = Main.Properties.Resources.application_16;
            connectToAnApplicationToolStripMenuItem1.Name = "connectToAnApplicationToolStripMenuItem1";
            connectToAnApplicationToolStripMenuItem1.Size = new System.Drawing.Size(222, 22);
            connectToAnApplicationToolStripMenuItem1.Text = "Connect to an Application...";
            connectToAnApplicationToolStripMenuItem1.ToolTipText = "Connect to an Application...";
            // 
            // addApplicationToolStripMenuItem
            // 
            actionList1.SetAction(addApplicationToolStripMenuItem, actCreateApplication);
            addApplicationToolStripMenuItem.Image = Main.Properties.Resources.application_new_16;
            addApplicationToolStripMenuItem.Name = "addApplicationToolStripMenuItem";
            addApplicationToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            addApplicationToolStripMenuItem.Text = "Add Application...";
            // 
            // toolStripMenuItem38
            // 
            actionList1.SetAction(toolStripMenuItem38, actCreateApplication);
            toolStripMenuItem38.Image = Main.Properties.Resources.application_new_16;
            toolStripMenuItem38.Name = "toolStripMenuItem38";
            toolStripMenuItem38.Size = new System.Drawing.Size(205, 22);
            toolStripMenuItem38.Text = "Add Application...";
            // 
            // toolStripMenuItem49
            // 
            actionList1.SetAction(toolStripMenuItem49, actCreateApplication);
            toolStripMenuItem49.Image = Main.Properties.Resources.application_new_16;
            toolStripMenuItem49.Name = "toolStripMenuItem49";
            toolStripMenuItem49.Size = new System.Drawing.Size(197, 22);
            toolStripMenuItem49.Text = "Add Application...";
            // 
            // exploreToolStripMenuItem1
            // 
            actionList1.SetAction(exploreToolStripMenuItem1, actExplore);
            exploreToolStripMenuItem1.Image = Main.Properties.Resources.explore_16;
            exploreToolStripMenuItem1.Name = "exploreToolStripMenuItem1";
            exploreToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            exploreToolStripMenuItem1.Text = "Explore";
            // 
            // editPermissionsToolStripMenuItem1
            // 
            actionList1.SetAction(editPermissionsToolStripMenuItem1, actEditPermissions);
            editPermissionsToolStripMenuItem1.Name = "editPermissionsToolStripMenuItem1";
            editPermissionsToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            editPermissionsToolStripMenuItem1.Text = "Edit Permissions...";
            // 
            // addVirtualDirectoryToolStripMenuItem1
            // 
            actionList1.SetAction(addVirtualDirectoryToolStripMenuItem1, actCreateVirtualDirectory);
            addVirtualDirectoryToolStripMenuItem1.Image = Main.Properties.Resources.virtual_directory_new_16;
            addVirtualDirectoryToolStripMenuItem1.Name = "addVirtualDirectoryToolStripMenuItem1";
            addVirtualDirectoryToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            addVirtualDirectoryToolStripMenuItem1.Text = "Add Virtual Directory...";
            // 
            // btnBrowseApplication
            // 
            actionList1.SetAction(btnBrowseApplication, actBrowse);
            btnBrowseApplication.Image = Main.Properties.Resources.browse_16;
            btnBrowseApplication.Name = "btnBrowseApplication";
            btnBrowseApplication.Size = new System.Drawing.Size(181, 22);
            btnBrowseApplication.Text = "Browse";
            // 
            // toolStripMenuItem36
            // 
            actionList1.SetAction(toolStripMenuItem36, actExplore);
            toolStripMenuItem36.Image = Main.Properties.Resources.explore_16;
            toolStripMenuItem36.Name = "toolStripMenuItem36";
            toolStripMenuItem36.Size = new System.Drawing.Size(205, 22);
            toolStripMenuItem36.Text = "Explore";
            // 
            // toolStripMenuItem37
            // 
            actionList1.SetAction(toolStripMenuItem37, actEditPermissions);
            toolStripMenuItem37.Name = "toolStripMenuItem37";
            toolStripMenuItem37.Size = new System.Drawing.Size(205, 22);
            toolStripMenuItem37.Text = "Edit Permissions...";
            // 
            // toolStripMenuItem39
            // 
            actionList1.SetAction(toolStripMenuItem39, actCreateVirtualDirectory);
            toolStripMenuItem39.Image = Main.Properties.Resources.virtual_directory_new_16;
            toolStripMenuItem39.Name = "toolStripMenuItem39";
            toolStripMenuItem39.Size = new System.Drawing.Size(205, 22);
            toolStripMenuItem39.Text = "Add Virtual Directory...";
            // 
            // toolStripMenuItem41
            // 
            actionList1.SetAction(toolStripMenuItem41, actBrowse);
            toolStripMenuItem41.Image = Main.Properties.Resources.browse_16;
            toolStripMenuItem41.Name = "toolStripMenuItem41";
            toolStripMenuItem41.Size = new System.Drawing.Size(181, 22);
            toolStripMenuItem41.Text = "Browse";
            // 
            // toolStripMenuItem46
            // 
            actionList1.SetAction(toolStripMenuItem46, actExplore);
            toolStripMenuItem46.Image = Main.Properties.Resources.explore_16;
            toolStripMenuItem46.Name = "toolStripMenuItem46";
            toolStripMenuItem46.Size = new System.Drawing.Size(197, 22);
            toolStripMenuItem46.Text = "Explore";
            // 
            // toolStripMenuItem47
            // 
            actionList1.SetAction(toolStripMenuItem47, actEditPermissions);
            toolStripMenuItem47.Name = "toolStripMenuItem47";
            toolStripMenuItem47.Size = new System.Drawing.Size(197, 22);
            toolStripMenuItem47.Text = "Edit Permissions...";
            // 
            // toolStripMenuItem50
            // 
            actionList1.SetAction(toolStripMenuItem50, actCreateVirtualDirectory);
            toolStripMenuItem50.Image = Main.Properties.Resources.virtual_directory_new_16;
            toolStripMenuItem50.Name = "toolStripMenuItem50";
            toolStripMenuItem50.Size = new System.Drawing.Size(197, 22);
            toolStripMenuItem50.Text = "Add Virtual Directory...";
            // 
            // toolStripMenuItem52
            // 
            actionList1.SetAction(toolStripMenuItem52, actBrowse);
            toolStripMenuItem52.Image = Main.Properties.Resources.browse_16;
            toolStripMenuItem52.Name = "toolStripMenuItem52";
            toolStripMenuItem52.Size = new System.Drawing.Size(112, 22);
            toolStripMenuItem52.Text = "Browse";
            // 
            // toolStripButton1
            // 
            actionList1.SetAction(toolStripButton1, actBack);
            toolStripButton1.AutoToolTip = false;
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Enabled = false;
            toolStripButton1.Image = Main.Properties.Resources.back_16;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(23, 22);
            toolStripButton1.Text = "Back";
            // 
            // toolStripButton2
            // 
            actionList1.SetAction(toolStripButton2, actForward);
            toolStripButton2.AutoToolTip = false;
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Enabled = false;
            toolStripButton2.Image = Main.Properties.Resources.forward_16;
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(23, 22);
            toolStripButton2.Text = "Forward";
            // 
            // tsbPath
            // 
            tsbPath.AutoSize = false;
            tsbPath.BackColor = System.Drawing.SystemColors.Window;
            tsbPath.ForeColor = System.Drawing.SystemColors.WindowText;
            tsbPath.HighlightColor = System.Drawing.SystemColors.Highlight;
            tsbPath.Name = "tsbPath";
            tsbPath.Padding = new Padding(2);
            tsbPath.Size = new System.Drawing.Size(800, 22);
            tsbPath.ItemClicked += TsbPath_ItemClicked;
            // 
            // cmsIis
            // 
            cmsIis.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem5, toolStripMenuItem23, connectToAServerToolStripMenuItem1, connectToAWebsiteToolStripMenuItem1, connectToAnApplicationToolStripMenuItem1 });
            cmsIis.Name = "cmsIis";
            cmsIis.Size = new System.Drawing.Size(223, 98);
            // 
            // refreshToolStripMenuItem5
            // 
            refreshToolStripMenuItem5.Enabled = false;
            refreshToolStripMenuItem5.Image = Main.Properties.Resources.refresh_16;
            refreshToolStripMenuItem5.Name = "refreshToolStripMenuItem5";
            refreshToolStripMenuItem5.Size = new System.Drawing.Size(222, 22);
            refreshToolStripMenuItem5.Text = "Refresh";
            // 
            // toolStripMenuItem23
            // 
            toolStripMenuItem23.Name = "toolStripMenuItem23";
            toolStripMenuItem23.Size = new System.Drawing.Size(219, 6);
            // 
            // cmsApplication
            // 
            cmsApplication.Items.AddRange(new ToolStripItem[] { exploreToolStripMenuItem1, editPermissionsToolStripMenuItem1, toolStripMenuItem9, addApplicationToolStripMenuItem, addVirtualDirectoryToolStripMenuItem1, toolStripMenuItem15, manageApplicationToolStripMenuItem, toolStripMenuItem16, refreshToolStripMenuItem6, removeToolStripMenuItem1, toolStripMenuItem24, switchToContentViewToolStripMenuItem3 });
            cmsApplication.Name = "cmsApplication";
            cmsApplication.Size = new System.Drawing.Size(198, 204);
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripMenuItem15
            // 
            toolStripMenuItem15.Name = "toolStripMenuItem15";
            toolStripMenuItem15.Size = new System.Drawing.Size(194, 6);
            // 
            // manageApplicationToolStripMenuItem
            // 
            manageApplicationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { btnBrowseApplication, toolStripMenuItem25, advancedSettingsToolStripMenuItem });
            manageApplicationToolStripMenuItem.Name = "manageApplicationToolStripMenuItem";
            manageApplicationToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            manageApplicationToolStripMenuItem.Text = "Manage Application";
            // 
            // toolStripMenuItem25
            // 
            toolStripMenuItem25.Name = "toolStripMenuItem25";
            toolStripMenuItem25.Size = new System.Drawing.Size(178, 6);
            // 
            // advancedSettingsToolStripMenuItem
            // 
            advancedSettingsToolStripMenuItem.Enabled = false;
            advancedSettingsToolStripMenuItem.Name = "advancedSettingsToolStripMenuItem";
            advancedSettingsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            advancedSettingsToolStripMenuItem.Text = "Advanced Settings...";
            // 
            // toolStripMenuItem16
            // 
            toolStripMenuItem16.Name = "toolStripMenuItem16";
            toolStripMenuItem16.Size = new System.Drawing.Size(194, 6);
            // 
            // refreshToolStripMenuItem6
            // 
            refreshToolStripMenuItem6.Enabled = false;
            refreshToolStripMenuItem6.Image = Main.Properties.Resources.refresh_16;
            refreshToolStripMenuItem6.Name = "refreshToolStripMenuItem6";
            refreshToolStripMenuItem6.Size = new System.Drawing.Size(197, 22);
            refreshToolStripMenuItem6.Text = "Refresh";
            // 
            // removeToolStripMenuItem1
            // 
            removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            removeToolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            removeToolStripMenuItem1.Text = "Remove";
            removeToolStripMenuItem1.Click += btnRemoveApplication_Click;
            // 
            // toolStripMenuItem24
            // 
            toolStripMenuItem24.Name = "toolStripMenuItem24";
            toolStripMenuItem24.Size = new System.Drawing.Size(194, 6);
            // 
            // switchToContentViewToolStripMenuItem3
            // 
            switchToContentViewToolStripMenuItem3.Enabled = false;
            switchToContentViewToolStripMenuItem3.Image = Main.Properties.Resources.switch_16;
            switchToContentViewToolStripMenuItem3.Name = "switchToContentViewToolStripMenuItem3";
            switchToContentViewToolStripMenuItem3.Size = new System.Drawing.Size(197, 22);
            switchToContentViewToolStripMenuItem3.Text = "Switch to Content View";
            // 
            // cmsServers
            // 
            cmsServers.Items.AddRange(new ToolStripItem[] { refreshToolStripMenuItem9, toolStripMenuItem34, addServerToolStripMenuItem, toolStripMenuItem35, switchToContentViewToolStripMenuItem6 });
            cmsServers.Name = "cmsServers";
            cmsServers.Size = new System.Drawing.Size(198, 82);
            // 
            // refreshToolStripMenuItem9
            // 
            refreshToolStripMenuItem9.Image = Main.Properties.Resources.refresh_16;
            refreshToolStripMenuItem9.Name = "refreshToolStripMenuItem9";
            refreshToolStripMenuItem9.Size = new System.Drawing.Size(197, 22);
            refreshToolStripMenuItem9.Text = "Refresh";
            // 
            // toolStripMenuItem34
            // 
            toolStripMenuItem34.Name = "toolStripMenuItem34";
            toolStripMenuItem34.Size = new System.Drawing.Size(194, 6);
            // 
            // addServerToolStripMenuItem
            // 
            addServerToolStripMenuItem.Name = "addServerToolStripMenuItem";
            addServerToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            addServerToolStripMenuItem.Text = "Add Server...";
            // 
            // toolStripMenuItem35
            // 
            toolStripMenuItem35.Name = "toolStripMenuItem35";
            toolStripMenuItem35.Size = new System.Drawing.Size(194, 6);
            // 
            // switchToContentViewToolStripMenuItem6
            // 
            switchToContentViewToolStripMenuItem6.Image = Main.Properties.Resources.switch_16;
            switchToContentViewToolStripMenuItem6.Name = "switchToContentViewToolStripMenuItem6";
            switchToContentViewToolStripMenuItem6.Size = new System.Drawing.Size(197, 22);
            switchToContentViewToolStripMenuItem6.Text = "Switch to Content View";
            // 
            // cmsVirtualDirectory
            // 
            cmsVirtualDirectory.Items.AddRange(new ToolStripItem[] { toolStripMenuItem36, toolStripMenuItem37, toolStripSeparator3, convertToApplicationToolStripMenuItem, toolStripMenuItem38, toolStripMenuItem39, toolStripSeparator4, manageVirtualDirectoryToolStripMenuItem, toolStripSeparator6, toolStripMenuItem43, toolStripMenuItem44, toolStripSeparator7, toolStripMenuItem45 });
            cmsVirtualDirectory.Name = "cmsApplication";
            cmsVirtualDirectory.Size = new System.Drawing.Size(206, 226);
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(202, 6);
            // 
            // convertToApplicationToolStripMenuItem
            // 
            convertToApplicationToolStripMenuItem.Image = Main.Properties.Resources.application_new_16;
            convertToApplicationToolStripMenuItem.Name = "convertToApplicationToolStripMenuItem";
            convertToApplicationToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            convertToApplicationToolStripMenuItem.Text = "Convert to Application";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(202, 6);
            // 
            // manageVirtualDirectoryToolStripMenuItem
            // 
            manageVirtualDirectoryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem41, toolStripSeparator5, toolStripMenuItem42 });
            manageVirtualDirectoryToolStripMenuItem.Name = "manageVirtualDirectoryToolStripMenuItem";
            manageVirtualDirectoryToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            manageVirtualDirectoryToolStripMenuItem.Text = "Manage Virtual Directory";
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(178, 6);
            // 
            // toolStripMenuItem42
            // 
            toolStripMenuItem42.Enabled = false;
            toolStripMenuItem42.Name = "toolStripMenuItem42";
            toolStripMenuItem42.Size = new System.Drawing.Size(181, 22);
            toolStripMenuItem42.Text = "Advanced Settings...";
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(202, 6);
            // 
            // toolStripMenuItem43
            // 
            toolStripMenuItem43.Enabled = false;
            toolStripMenuItem43.Image = Main.Properties.Resources.refresh_16;
            toolStripMenuItem43.Name = "toolStripMenuItem43";
            toolStripMenuItem43.Size = new System.Drawing.Size(205, 22);
            toolStripMenuItem43.Text = "Refresh";
            // 
            // toolStripMenuItem44
            // 
            toolStripMenuItem44.Name = "toolStripMenuItem44";
            toolStripMenuItem44.Size = new System.Drawing.Size(205, 22);
            toolStripMenuItem44.Text = "Remove";
            toolStripMenuItem44.Click += btnRemoveVirtualDirectory_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(202, 6);
            // 
            // toolStripMenuItem45
            // 
            toolStripMenuItem45.Enabled = false;
            toolStripMenuItem45.Image = Main.Properties.Resources.switch_16;
            toolStripMenuItem45.Name = "toolStripMenuItem45";
            toolStripMenuItem45.Size = new System.Drawing.Size(205, 22);
            toolStripMenuItem45.Text = "Switch to Content View";
            // 
            // cmsPhysicalDirectory
            // 
            cmsPhysicalDirectory.Items.AddRange(new ToolStripItem[] { toolStripMenuItem46, toolStripMenuItem47, toolStripSeparator8, toolStripMenuItem48, toolStripMenuItem49, toolStripMenuItem50, toolStripSeparator9, manageFolderToolStripMenuItem, toolStripSeparator11, toolStripMenuItem54, toolStripSeparator12, toolStripMenuItem56 });
            cmsPhysicalDirectory.Name = "cmsPhysicalDirectory";
            cmsPhysicalDirectory.Size = new System.Drawing.Size(198, 204);
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripMenuItem48
            // 
            toolStripMenuItem48.Image = Main.Properties.Resources.application_new_16;
            toolStripMenuItem48.Name = "toolStripMenuItem48";
            toolStripMenuItem48.Size = new System.Drawing.Size(197, 22);
            toolStripMenuItem48.Text = "Convert to Application";
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(194, 6);
            // 
            // manageFolderToolStripMenuItem
            // 
            manageFolderToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem52 });
            manageFolderToolStripMenuItem.Name = "manageFolderToolStripMenuItem";
            manageFolderToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            manageFolderToolStripMenuItem.Text = "Manage Folder";
            // 
            // toolStripSeparator11
            // 
            toolStripSeparator11.Name = "toolStripSeparator11";
            toolStripSeparator11.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripMenuItem54
            // 
            toolStripMenuItem54.Enabled = false;
            toolStripMenuItem54.Image = Main.Properties.Resources.refresh_16;
            toolStripMenuItem54.Name = "toolStripMenuItem54";
            toolStripMenuItem54.Size = new System.Drawing.Size(197, 22);
            toolStripMenuItem54.Text = "Refresh";
            // 
            // toolStripSeparator12
            // 
            toolStripSeparator12.Name = "toolStripSeparator12";
            toolStripSeparator12.Size = new System.Drawing.Size(194, 6);
            // 
            // toolStripMenuItem56
            // 
            toolStripMenuItem56.Enabled = false;
            toolStripMenuItem56.Image = Main.Properties.Resources.switch_16;
            toolStripMenuItem56.Name = "toolStripMenuItem56";
            toolStripMenuItem56.Size = new System.Drawing.Size(197, 22);
            toolStripMenuItem56.Text = "Switch to Content View";
            // 
            // _logSplitter
            // 
            _logSplitter.Dock = DockStyle.Fill;
            _logSplitter.Location = new System.Drawing.Point(0, 49);
            _logSplitter.Name = "_logSplitter";
            _logSplitter.Orientation = Orientation.Horizontal;
            // 
            // _logSplitter.Panel1
            // 
            _logSplitter.Panel1.Controls.Add(scMain);
            _logSplitter.Panel1MinSize = 100;
            // 
            // _logSplitter.Panel2
            // 
            _logSplitter.Panel2.Controls.Add(_logPanel);
            _logSplitter.Panel2Collapsed = true;
            _logSplitter.Panel2MinSize = 50;
            _logSplitter.Size = new System.Drawing.Size(915, 426);
            _logSplitter.SplitterDistance = 100;
            _logSplitter.SplitterWidth = 5;
            _logSplitter.TabIndex = 6;
            // 
            // _logPanel
            // 
            _logPanel.Dock = DockStyle.Fill;
            _logPanel.Location = new System.Drawing.Point(0, 0);
            _logPanel.Name = "_logPanel";
            _logPanel.Size = new System.Drawing.Size(150, 46);
            _logPanel.TabIndex = 0;
            // 
            // tsTop
            // 
            tsTop.AllowMerge = false;
            tsTop.GripStyle = ToolStripGripStyle.Hidden;
            tsTop.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2, tsbPath });
            tsTop.Location = new System.Drawing.Point(0, 0);
            tsTop.Name = "tsTop";
            tsTop.Size = new System.Drawing.Size(915, 25);
            tsTop.TabIndex = 9;
            tsTop.Text = "toolStrip1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(915, 497);
            Controls.Add(_logSplitter);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(tsTop);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            MinimumSize = new System.Drawing.Size(738, 504);
            Name = "MainForm";
            Text = "Jexus Manager";
            FormClosing += Form1FormClosing;
            Load += MainForm_Load;
            scMain.Panel1.ResumeLayout(false);
            ((ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            cmsServer.ResumeLayout(false);
            cmsApplicationPools.ResumeLayout(false);
            cmsSites.ResumeLayout(false);
            cmsSite.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((ISupportInitialize)actionList1).EndInit();
            cmsIis.ResumeLayout(false);
            cmsApplication.ResumeLayout(false);
            cmsServers.ResumeLayout(false);
            cmsVirtualDirectory.ResumeLayout(false);
            cmsPhysicalDirectory.ResumeLayout(false);
            _logSplitter.Panel1.ResumeLayout(false);
            _logSplitter.Panel2.ResumeLayout(false);
            ((ISupportInitialize)_logSplitter).EndInit();
            _logSplitter.ResumeLayout(false);
            tsTop.ResumeLayout(false);
            tsTop.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private ToolStrip tsTop;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripBreadcrumbItem tsbPath;
    }
}

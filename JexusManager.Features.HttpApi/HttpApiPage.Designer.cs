namespace JexusManager.Features.HttpApi
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class HttpApiPage
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmsActionPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpIP = new System.Windows.Forms.TabPage();
            this.lvIP = new System.Windows.Forms.ListView();
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chApp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAppId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpSNI = new System.Windows.Forms.TabPage();
            this.lvSni = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpURL = new System.Windows.Forms.TabPage();
            this.lvURL = new System.Windows.Forms.ListView();
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tsActionPanel = new System.Windows.Forms.ToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpIP.SuspendLayout();
            this.tpSNI.SuspendLayout();
            this.tpURL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.ContextMenuStrip = this.cmsActionPanel;
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 200;
            this.splitContainer1.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.SplitterDistance = 580;
            this.splitContainer1.TabIndex = 6;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // cmsActionPanel
            // 
            this.cmsActionPanel.Name = "contextMenuStrip1";
            this.cmsActionPanel.Size = new System.Drawing.Size(61, 4);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(10, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(560, 540);
            this.panel2.TabIndex = 9;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpIP);
            this.tabControl1.Controls.Add(this.tpSNI);
            this.tabControl1.Controls.Add(this.tpURL);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 527);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1SelectedIndexChanged);
            // 
            // tpIP
            // 
            this.tpIP.Controls.Add(this.lvIP);
            this.tpIP.Location = new System.Drawing.Point(4, 22);
            this.tpIP.Name = "tpIP";
            this.tpIP.Padding = new System.Windows.Forms.Padding(3);
            this.tpIP.Size = new System.Drawing.Size(552, 501);
            this.tpIP.TabIndex = 0;
            this.tpIP.Text = "IP Based";
            this.tpIP.UseVisualStyleBackColor = true;
            // 
            // lvIP
            // 
            this.lvIP.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAddress,
            this.chPort,
            this.chApp,
            this.chAppId,
            this.chHash,
            this.chStore});
            this.lvIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvIP.FullRowSelect = true;
            this.lvIP.HideSelection = false;
            this.lvIP.Location = new System.Drawing.Point(3, 3);
            this.lvIP.Margin = new System.Windows.Forms.Padding(5);
            this.lvIP.MultiSelect = false;
            this.lvIP.Name = "lvIP";
            this.lvIP.Size = new System.Drawing.Size(546, 495);
            this.lvIP.TabIndex = 1;
            this.lvIP.UseCompatibleStateImageBehavior = false;
            this.lvIP.View = System.Windows.Forms.View.Details;
            this.lvIP.SelectedIndexChanged += new System.EventHandler(this.LvIPSelectedIndexChanged);
            // 
            // chAddress
            // 
            this.chAddress.Text = "Address";
            this.chAddress.Width = 180;
            // 
            // chPort
            // 
            this.chPort.Text = "Port";
            this.chPort.Width = 80;
            // 
            // chApp
            // 
            this.chApp.Text = "Application Name";
            this.chApp.Width = 100;
            // 
            // chAppId
            // 
            this.chAppId.Text = "Application ID";
            this.chAppId.Width = 180;
            // 
            // chHash
            // 
            this.chHash.Text = "Certificate Hash";
            this.chHash.Width = 180;
            // 
            // chStore
            // 
            this.chStore.Text = "Certificate Store";
            this.chStore.Width = 120;
            // 
            // tpSNI
            // 
            this.tpSNI.Controls.Add(this.lvSni);
            this.tpSNI.Location = new System.Drawing.Point(4, 22);
            this.tpSNI.Name = "tpSNI";
            this.tpSNI.Padding = new System.Windows.Forms.Padding(3);
            this.tpSNI.Size = new System.Drawing.Size(552, 501);
            this.tpSNI.TabIndex = 1;
            this.tpSNI.Text = "SNI";
            this.tpSNI.UseVisualStyleBackColor = true;
            // 
            // lvSni
            // 
            this.lvSni.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lvSni.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSni.FullRowSelect = true;
            this.lvSni.HideSelection = false;
            this.lvSni.Location = new System.Drawing.Point(3, 3);
            this.lvSni.Margin = new System.Windows.Forms.Padding(5);
            this.lvSni.MultiSelect = false;
            this.lvSni.Name = "lvSni";
            this.lvSni.Size = new System.Drawing.Size(546, 495);
            this.lvSni.TabIndex = 2;
            this.lvSni.UseCompatibleStateImageBehavior = false;
            this.lvSni.View = System.Windows.Forms.View.Details;
            this.lvSni.SelectedIndexChanged += new System.EventHandler(this.LvSniSelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Host Name";
            this.columnHeader1.Width = 180;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Port";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Application Name";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Application ID";
            this.columnHeader4.Width = 180;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Certificate Hash";
            this.columnHeader5.Width = 180;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Certificate Store";
            this.columnHeader6.Width = 120;
            // 
            // tpURL
            // 
            this.tpURL.Controls.Add(this.lvURL);
            this.tpURL.Location = new System.Drawing.Point(4, 22);
            this.tpURL.Name = "tpURL";
            this.tpURL.Padding = new System.Windows.Forms.Padding(3);
            this.tpURL.Size = new System.Drawing.Size(552, 501);
            this.tpURL.TabIndex = 2;
            this.tpURL.Text = "URL Reservation";
            this.tpURL.UseVisualStyleBackColor = true;
            // 
            // lvURL
            // 
            this.lvURL.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader7,
            this.columnHeader10});
            this.lvURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvURL.FullRowSelect = true;
            this.lvURL.HideSelection = false;
            this.lvURL.Location = new System.Drawing.Point(3, 3);
            this.lvURL.Margin = new System.Windows.Forms.Padding(5);
            this.lvURL.MultiSelect = false;
            this.lvURL.Name = "lvURL";
            this.lvURL.Size = new System.Drawing.Size(546, 495);
            this.lvURL.TabIndex = 2;
            this.lvURL.UseCompatibleStateImageBehavior = false;
            this.lvURL.View = System.Windows.Forms.View.Details;
            this.lvURL.SelectedIndexChanged += new System.EventHandler(this.LvURLSelectedIndexChanged);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "URL Prefix";
            this.columnHeader7.Width = 180;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Security Descriptor";
            this.columnHeader10.Width = 180;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(523, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Use this feature to manage certificate bindings that the Web server can use with " +
    "websites configured for SSL.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(48, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "HTTP API";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tsActionPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(216, 600);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(216, 23);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Actions";
            // 
            // tsActionPanel
            // 
            this.tsActionPanel.CanOverflow = false;
            this.tsActionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tsActionPanel.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsActionPanel.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsActionPanel.Location = new System.Drawing.Point(0, 23);
            this.tsActionPanel.Name = "tsActionPanel";
            this.tsActionPanel.Size = new System.Drawing.Size(216, 577);
            this.tsActionPanel.TabIndex = 2;
            this.tsActionPanel.Text = "toolStrip1";
            // 
            // HttpApiPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "HttpApiPage";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpIP.ResumeLayout(false);
            this.tpSNI.ResumeLayout(false);
            this.tpURL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private Label label2;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label label1;
        private ToolStrip tsActionPanel;
        private ContextMenuStrip cmsActionPanel;
        private PictureBox pictureBox1;
        private Panel panel2;
        private TabControl tabControl1;
        private TabPage tpIP;
        private ListView lvIP;
        private ColumnHeader chAddress;
        private ColumnHeader chPort;
        private ColumnHeader chApp;
        private ColumnHeader chAppId;
        private ColumnHeader chHash;
        private ColumnHeader chStore;
        private TabPage tpSNI;
        private TabPage tpURL;
        private ListView lvSni;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ListView lvURL;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader10;
    }
}

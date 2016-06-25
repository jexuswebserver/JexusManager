namespace JexusManager.Features.RequestFiltering
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class RequestFilteringPage
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
            this.tpExtensions = new System.Windows.Forms.TabPage();
            this.lvExtensions = new System.Windows.Forms.ListView();
            this.chExtension = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAllowedExtension = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpRules = new System.Windows.Forms.TabPage();
            this.lvRules = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpSegments = new System.Windows.Forms.TabPage();
            this.lvSegments = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpUrl = new System.Windows.Forms.TabPage();
            this.lvUrls = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpVerbs = new System.Windows.Forms.TabPage();
            this.lvVerbs = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpHeaders = new System.Windows.Forms.TabPage();
            this.lvHeaders = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpQuery = new System.Windows.Forms.TabPage();
            this.lvQueries = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
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
            this.tpExtensions.SuspendLayout();
            this.tpRules.SuspendLayout();
            this.tpSegments.SuspendLayout();
            this.tpUrl.SuspendLayout();
            this.tpVerbs.SuspendLayout();
            this.tpHeaders.SuspendLayout();
            this.tpQuery.SuspendLayout();
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
            this.splitContainer1.TabIndex = 4;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitContainer1SplitterMoved);
            // 
            // cmsActionPanel
            // 
            this.cmsActionPanel.Name = "cmsActionPanel";
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
            this.panel2.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpExtensions);
            this.tabControl1.Controls.Add(this.tpRules);
            this.tabControl1.Controls.Add(this.tpSegments);
            this.tabControl1.Controls.Add(this.tpUrl);
            this.tabControl1.Controls.Add(this.tpVerbs);
            this.tabControl1.Controls.Add(this.tpHeaders);
            this.tabControl1.Controls.Add(this.tpQuery);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(560, 527);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1SelectedIndexChanged);
            // 
            // tpExtensions
            // 
            this.tpExtensions.Controls.Add(this.lvExtensions);
            this.tpExtensions.Location = new System.Drawing.Point(4, 23);
            this.tpExtensions.Name = "tpExtensions";
            this.tpExtensions.Size = new System.Drawing.Size(552, 500);
            this.tpExtensions.TabIndex = 2;
            this.tpExtensions.Text = "File Name Extensions";
            this.tpExtensions.UseVisualStyleBackColor = true;
            // 
            // lvExtensions
            // 
            this.lvExtensions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chExtension,
            this.chAllowedExtension});
            this.lvExtensions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvExtensions.FullRowSelect = true;
            this.lvExtensions.HideSelection = false;
            this.lvExtensions.Location = new System.Drawing.Point(0, 0);
            this.lvExtensions.Margin = new System.Windows.Forms.Padding(5);
            this.lvExtensions.MultiSelect = false;
            this.lvExtensions.Name = "lvExtensions";
            this.lvExtensions.Size = new System.Drawing.Size(552, 500);
            this.lvExtensions.TabIndex = 1;
            this.lvExtensions.UseCompatibleStateImageBehavior = false;
            this.lvExtensions.View = System.Windows.Forms.View.Details;
            this.lvExtensions.SelectedIndexChanged += new System.EventHandler(this.LvExtensionsSelectedIndexChanged);
            // 
            // chExtension
            // 
            this.chExtension.Text = "Extension";
            this.chExtension.Width = 105;
            // 
            // chAllowedExtension
            // 
            this.chAllowedExtension.Text = "Allowed";
            // 
            // tpRules
            // 
            this.tpRules.Controls.Add(this.lvRules);
            this.tpRules.Location = new System.Drawing.Point(4, 23);
            this.tpRules.Name = "tpRules";
            this.tpRules.Size = new System.Drawing.Size(552, 500);
            this.tpRules.TabIndex = 3;
            this.tpRules.Text = "Rules";
            this.tpRules.UseVisualStyleBackColor = true;
            // 
            // lvRules
            // 
            this.lvRules.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
            this.lvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvRules.FullRowSelect = true;
            this.lvRules.HideSelection = false;
            this.lvRules.Location = new System.Drawing.Point(0, 0);
            this.lvRules.Margin = new System.Windows.Forms.Padding(5);
            this.lvRules.MultiSelect = false;
            this.lvRules.Name = "lvRules";
            this.lvRules.Size = new System.Drawing.Size(552, 500);
            this.lvRules.TabIndex = 1;
            this.lvRules.UseCompatibleStateImageBehavior = false;
            this.lvRules.View = System.Windows.Forms.View.Details;
            this.lvRules.SelectedIndexChanged += new System.EventHandler(this.LvRuleSelectedIndexChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 105;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Scan";
            this.columnHeader10.Width = 105;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Applied To";
            this.columnHeader11.Width = 105;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Deny Strings";
            this.columnHeader12.Width = 105;
            // 
            // tpSegments
            // 
            this.tpSegments.Controls.Add(this.lvSegments);
            this.tpSegments.Location = new System.Drawing.Point(4, 23);
            this.tpSegments.Name = "tpSegments";
            this.tpSegments.Padding = new System.Windows.Forms.Padding(3);
            this.tpSegments.Size = new System.Drawing.Size(552, 500);
            this.tpSegments.TabIndex = 0;
            this.tpSegments.Text = "Hidden Segments";
            this.tpSegments.UseVisualStyleBackColor = true;
            // 
            // lvSegments
            // 
            this.lvSegments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lvSegments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSegments.FullRowSelect = true;
            this.lvSegments.HideSelection = false;
            this.lvSegments.Location = new System.Drawing.Point(3, 3);
            this.lvSegments.Margin = new System.Windows.Forms.Padding(5);
            this.lvSegments.MultiSelect = false;
            this.lvSegments.Name = "lvSegments";
            this.lvSegments.Size = new System.Drawing.Size(546, 494);
            this.lvSegments.TabIndex = 0;
            this.lvSegments.UseCompatibleStateImageBehavior = false;
            this.lvSegments.View = System.Windows.Forms.View.Details;
            this.lvSegments.SelectedIndexChanged += new System.EventHandler(this.LvSegmentsSelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Segment";
            this.chName.Width = 200;
            // 
            // tpUrl
            // 
            this.tpUrl.Controls.Add(this.lvUrls);
            this.tpUrl.Location = new System.Drawing.Point(4, 23);
            this.tpUrl.Name = "tpUrl";
            this.tpUrl.Padding = new System.Windows.Forms.Padding(3);
            this.tpUrl.Size = new System.Drawing.Size(552, 500);
            this.tpUrl.TabIndex = 1;
            this.tpUrl.Text = "URL";
            this.tpUrl.UseVisualStyleBackColor = true;
            // 
            // lvUrls
            // 
            this.lvUrls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader9});
            this.lvUrls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvUrls.FullRowSelect = true;
            this.lvUrls.HideSelection = false;
            this.lvUrls.Location = new System.Drawing.Point(3, 3);
            this.lvUrls.Margin = new System.Windows.Forms.Padding(5);
            this.lvUrls.MultiSelect = false;
            this.lvUrls.Name = "lvUrls";
            this.lvUrls.Size = new System.Drawing.Size(546, 494);
            this.lvUrls.TabIndex = 1;
            this.lvUrls.UseCompatibleStateImageBehavior = false;
            this.lvUrls.View = System.Windows.Forms.View.Details;
            this.lvUrls.SelectedIndexChanged += new System.EventHandler(this.LvUrlsSelectedIndexChanged);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Url";
            this.columnHeader3.Width = 145;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Action";
            this.columnHeader9.Width = 145;
            // 
            // tpVerbs
            // 
            this.tpVerbs.Controls.Add(this.lvVerbs);
            this.tpVerbs.Location = new System.Drawing.Point(4, 23);
            this.tpVerbs.Name = "tpVerbs";
            this.tpVerbs.Size = new System.Drawing.Size(552, 500);
            this.tpVerbs.TabIndex = 4;
            this.tpVerbs.Text = "HTTP Verbs";
            this.tpVerbs.UseVisualStyleBackColor = true;
            // 
            // lvVerbs
            // 
            this.lvVerbs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader8});
            this.lvVerbs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvVerbs.FullRowSelect = true;
            this.lvVerbs.HideSelection = false;
            this.lvVerbs.Location = new System.Drawing.Point(0, 0);
            this.lvVerbs.Margin = new System.Windows.Forms.Padding(5);
            this.lvVerbs.MultiSelect = false;
            this.lvVerbs.Name = "lvVerbs";
            this.lvVerbs.Size = new System.Drawing.Size(552, 500);
            this.lvVerbs.TabIndex = 1;
            this.lvVerbs.UseCompatibleStateImageBehavior = false;
            this.lvVerbs.View = System.Windows.Forms.View.Details;
            this.lvVerbs.SelectedIndexChanged += new System.EventHandler(this.LvVerbsSelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Verb";
            this.columnHeader4.Width = 175;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Allowed";
            // 
            // tpHeaders
            // 
            this.tpHeaders.Controls.Add(this.lvHeaders);
            this.tpHeaders.Location = new System.Drawing.Point(4, 23);
            this.tpHeaders.Name = "tpHeaders";
            this.tpHeaders.Size = new System.Drawing.Size(552, 500);
            this.tpHeaders.TabIndex = 5;
            this.tpHeaders.Text = "Headers";
            this.tpHeaders.UseVisualStyleBackColor = true;
            // 
            // lvHeaders
            // 
            this.lvHeaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader7});
            this.lvHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHeaders.FullRowSelect = true;
            this.lvHeaders.HideSelection = false;
            this.lvHeaders.Location = new System.Drawing.Point(0, 0);
            this.lvHeaders.Margin = new System.Windows.Forms.Padding(5);
            this.lvHeaders.MultiSelect = false;
            this.lvHeaders.Name = "lvHeaders";
            this.lvHeaders.Size = new System.Drawing.Size(552, 500);
            this.lvHeaders.TabIndex = 1;
            this.lvHeaders.UseCompatibleStateImageBehavior = false;
            this.lvHeaders.View = System.Windows.Forms.View.Details;
            this.lvHeaders.SelectedIndexChanged += new System.EventHandler(this.LvHeadersSelectedIndexChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Header";
            this.columnHeader5.Width = 175;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Size Limit";
            // 
            // tpQuery
            // 
            this.tpQuery.Controls.Add(this.lvQueries);
            this.tpQuery.Location = new System.Drawing.Point(4, 23);
            this.tpQuery.Name = "tpQuery";
            this.tpQuery.Size = new System.Drawing.Size(552, 500);
            this.tpQuery.TabIndex = 6;
            this.tpQuery.Text = "Query Strings";
            this.tpQuery.UseVisualStyleBackColor = true;
            // 
            // lvQueries
            // 
            this.lvQueries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader1});
            this.lvQueries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvQueries.FullRowSelect = true;
            this.lvQueries.HideSelection = false;
            this.lvQueries.Location = new System.Drawing.Point(0, 0);
            this.lvQueries.Margin = new System.Windows.Forms.Padding(5);
            this.lvQueries.MultiSelect = false;
            this.lvQueries.Name = "lvQueries";
            this.lvQueries.Size = new System.Drawing.Size(552, 500);
            this.lvQueries.TabIndex = 1;
            this.lvQueries.UseCompatibleStateImageBehavior = false;
            this.lvQueries.View = System.Windows.Forms.View.Details;
            this.lvQueries.SelectedIndexChanged += new System.EventHandler(this.LvQueriesSelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Query String";
            this.columnHeader6.Width = 145;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Action";
            this.columnHeader1.Width = 145;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Use this feature to configure filtering rules.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(48, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Request Filtering";
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
            // RequestFilteringPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RequestFilteringPage";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpExtensions.ResumeLayout(false);
            this.tpRules.ResumeLayout(false);
            this.tpSegments.ResumeLayout(false);
            this.tpUrl.ResumeLayout(false);
            this.tpVerbs.ResumeLayout(false);
            this.tpHeaders.ResumeLayout(false);
            this.tpQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private ListView lvSegments;
        private ColumnHeader chName;
        private Label label2;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label label1;
        private ToolStrip tsActionPanel;
        private Panel panel2;
        private PictureBox pictureBox1;
        private ContextMenuStrip cmsActionPanel;
        private TabControl tabControl1;
        private TabPage tpExtensions;
        private TabPage tpRules;
        private TabPage tpSegments;
        private TabPage tpUrl;
        private TabPage tpVerbs;
        private TabPage tpHeaders;
        private TabPage tpQuery;
        private ImageList imageList1;
        private ListView lvExtensions;
        private ColumnHeader chExtension;
        private ListView lvRules;
        private ColumnHeader columnHeader2;
        private ListView lvUrls;
        private ColumnHeader columnHeader3;
        private ListView lvVerbs;
        private ColumnHeader columnHeader4;
        private ListView lvHeaders;
        private ColumnHeader columnHeader5;
        private ListView lvQueries;
        private ColumnHeader columnHeader6;
        private ColumnHeader chAllowedExtension;
        private ColumnHeader columnHeader10;
        private ColumnHeader columnHeader11;
        private ColumnHeader columnHeader12;
        private ColumnHeader columnHeader9;
        private ColumnHeader columnHeader8;
        private ColumnHeader columnHeader7;
        private ColumnHeader columnHeader1;
    }
}

namespace JexusManager.Features.RequestFiltering
{
    using System;
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
            components = new Container();
            splitContainer1 = new SplitContainer();
            cmsActionPanel = new ContextMenuStrip(components);
            panel2 = new Panel();
            tabControl1 = new TabControl();
            tpExtensions = new TabPage();
            lvExtensions = new ListView();
            chExtension = new ColumnHeader();
            chAllowedExtension = new ColumnHeader();
            tpRules = new TabPage();
            lvRules = new ListView();
            columnHeader2 = new ColumnHeader();
            columnHeader10 = new ColumnHeader();
            columnHeader11 = new ColumnHeader();
            columnHeader12 = new ColumnHeader();
            tpSegments = new TabPage();
            lvSegments = new ListView();
            chName = new ColumnHeader();
            tpUrl = new TabPage();
            lvUrls = new ListView();
            columnHeader3 = new ColumnHeader();
            columnHeader9 = new ColumnHeader();
            tpVerbs = new TabPage();
            lvVerbs = new ListView();
            columnHeader4 = new ColumnHeader();
            columnHeader8 = new ColumnHeader();
            tpHeaders = new TabPage();
            lvHeaders = new ListView();
            columnHeader5 = new ColumnHeader();
            columnHeader7 = new ColumnHeader();
            tpQuery = new TabPage();
            lvQueries = new ListView();
            columnHeader6 = new ColumnHeader();
            columnHeader1 = new ColumnHeader();
            imageList1 = new ImageList(components);
            label2 = new Label();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            label1 = new Label();
            tsActionPanel = new ToolStrip();
            ((ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel2.SuspendLayout();
            tabControl1.SuspendLayout();
            tpExtensions.SuspendLayout();
            tpRules.SuspendLayout();
            tpSegments.SuspendLayout();
            tpUrl.SuspendLayout();
            tpVerbs.SuspendLayout();
            tpHeaders.SuspendLayout();
            tpQuery.SuspendLayout();
            ((ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            splitContainer1.Panel1.ContextMenuStrip = cmsActionPanel;
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            splitContainer1.Panel1.Controls.Add(label3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tableLayoutPanel1);
            splitContainer1.Panel2MinSize = 200;
            splitContainer1.Size = new System.Drawing.Size(800, 600);
            splitContainer1.SplitterDistance = 580;
            splitContainer1.TabIndex = 4;
            splitContainer1.SplitterMoved += SplitContainer1SplitterMoved;
            // 
            // cmsActionPanel
            // 
            cmsActionPanel.Name = "cmsActionPanel";
            cmsActionPanel.Size = new System.Drawing.Size(61, 4);
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.Controls.Add(tabControl1);
            panel2.Controls.Add(label2);
            panel2.Location = new System.Drawing.Point(10, 50);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(560, 540);
            panel2.TabIndex = 6;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tpExtensions);
            tabControl1.Controls.Add(tpRules);
            tabControl1.Controls.Add(tpSegments);
            tabControl1.Controls.Add(tpUrl);
            tabControl1.Controls.Add(tpVerbs);
            tabControl1.Controls.Add(tpHeaders);
            tabControl1.Controls.Add(tpQuery);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.ImageList = imageList1;
            tabControl1.Location = new System.Drawing.Point(0, 13);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(560, 527);
            tabControl1.TabIndex = 2;
            tabControl1.SelectedIndexChanged += TabControl1SelectedIndexChanged;
            // 
            // tpExtensions
            // 
            tpExtensions.Controls.Add(lvExtensions);
            tpExtensions.Location = new System.Drawing.Point(4, 23);
            tpExtensions.Name = "tpExtensions";
            tpExtensions.Size = new System.Drawing.Size(552, 500);
            tpExtensions.TabIndex = 2;
            tpExtensions.Text = "File Name Extensions";
            tpExtensions.UseVisualStyleBackColor = true;
            // 
            // lvExtensions
            // 
            lvExtensions.Columns.AddRange(new ColumnHeader[] { chExtension, chAllowedExtension });
            lvExtensions.Dock = DockStyle.Fill;
            lvExtensions.FullRowSelect = true;
            lvExtensions.Location = new System.Drawing.Point(0, 0);
            lvExtensions.Margin = new Padding(5);
            lvExtensions.MultiSelect = false;
            lvExtensions.Name = "lvExtensions";
            lvExtensions.Size = new System.Drawing.Size(552, 500);
            lvExtensions.TabIndex = 1;
            lvExtensions.UseCompatibleStateImageBehavior = false;
            lvExtensions.View = View.Details;
            lvExtensions.SelectedIndexChanged += LvExtensionsSelectedIndexChanged;
            lvExtensions.KeyDown += LvExtensions_KeyDown;
            lvExtensions.MouseDoubleClick += LvExtensions_MouseDoubleClick;
            // 
            // chExtension
            // 
            chExtension.Text = "Extension";
            chExtension.Width = 105;
            // 
            // chAllowedExtension
            // 
            chAllowedExtension.Text = "Allowed";
            // 
            // tpRules
            // 
            tpRules.Controls.Add(lvRules);
            tpRules.Location = new System.Drawing.Point(4, 23);
            tpRules.Name = "tpRules";
            tpRules.Size = new System.Drawing.Size(552, 500);
            tpRules.TabIndex = 3;
            tpRules.Text = "Rules";
            tpRules.UseVisualStyleBackColor = true;
            // 
            // lvRules
            // 
            lvRules.Columns.AddRange(new ColumnHeader[] { columnHeader2, columnHeader10, columnHeader11, columnHeader12 });
            lvRules.Dock = DockStyle.Fill;
            lvRules.FullRowSelect = true;
            lvRules.Location = new System.Drawing.Point(0, 0);
            lvRules.Margin = new Padding(5);
            lvRules.MultiSelect = false;
            lvRules.Name = "lvRules";
            lvRules.Size = new System.Drawing.Size(552, 500);
            lvRules.TabIndex = 1;
            lvRules.UseCompatibleStateImageBehavior = false;
            lvRules.View = View.Details;
            lvRules.SelectedIndexChanged += LvRulesSelectedIndexChanged;
            lvRules.KeyDown += LvRules_KeyDown;
            lvRules.MouseDoubleClick += LvRules_MouseDoubleClick;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Name";
            columnHeader2.Width = 105;
            // 
            // columnHeader10
            // 
            columnHeader10.Text = "Scan";
            columnHeader10.Width = 105;
            // 
            // columnHeader11
            // 
            columnHeader11.Text = "Applied To";
            columnHeader11.Width = 105;
            // 
            // columnHeader12
            // 
            columnHeader12.Text = "Deny Strings";
            columnHeader12.Width = 105;
            // 
            // tpSegments
            // 
            tpSegments.Controls.Add(lvSegments);
            tpSegments.Location = new System.Drawing.Point(4, 23);
            tpSegments.Name = "tpSegments";
            tpSegments.Padding = new Padding(3);
            tpSegments.Size = new System.Drawing.Size(552, 500);
            tpSegments.TabIndex = 0;
            tpSegments.Text = "Hidden Segments";
            tpSegments.UseVisualStyleBackColor = true;
            // 
            // lvSegments
            // 
            lvSegments.Columns.AddRange(new ColumnHeader[] { chName });
            lvSegments.Dock = DockStyle.Fill;
            lvSegments.FullRowSelect = true;
            lvSegments.Location = new System.Drawing.Point(3, 3);
            lvSegments.Margin = new Padding(5);
            lvSegments.MultiSelect = false;
            lvSegments.Name = "lvSegments";
            lvSegments.Size = new System.Drawing.Size(552, 500);
            lvSegments.TabIndex = 0;
            lvSegments.UseCompatibleStateImageBehavior = false;
            lvSegments.View = View.Details;
            lvSegments.SelectedIndexChanged += LvSegmentsSelectedIndexChanged;
            lvSegments.KeyDown += LvSegments_KeyDown;
            lvSegments.MouseDoubleClick += LvSegments_MouseDoubleClick;
            // 
            // chName
            // 
            chName.Text = "Segment";
            chName.Width = 200;
            // 
            // tpUrl
            // 
            tpUrl.Controls.Add(lvUrls);
            tpUrl.Location = new System.Drawing.Point(4, 24);
            tpUrl.Name = "tpUrl";
            tpUrl.Padding = new Padding(3);
            tpUrl.Size = new System.Drawing.Size(552, 500);
            tpUrl.TabIndex = 1;
            tpUrl.Text = "URL";
            tpUrl.UseVisualStyleBackColor = true;
            // 
            // lvUrls
            // 
            lvUrls.Columns.AddRange(new ColumnHeader[] { columnHeader3, columnHeader9 });
            lvUrls.Dock = DockStyle.Fill;
            lvUrls.FullRowSelect = true;
            lvUrls.Location = new System.Drawing.Point(3, 3);
            lvUrls.Margin = new Padding(5);
            lvUrls.MultiSelect = false;
            lvUrls.Name = "lvUrls";
            lvUrls.Size = new System.Drawing.Size(552, 500);
            lvUrls.TabIndex = 1;
            lvUrls.UseCompatibleStateImageBehavior = false;
            lvUrls.View = View.Details;
            lvUrls.SelectedIndexChanged += LvUrlsSelectedIndexChanged;
            lvUrls.KeyDown += LvUrls_KeyDown;
            lvUrls.MouseDoubleClick += LvUrls_MouseDoubleClick;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Url";
            columnHeader3.Width = 145;
            // 
            // columnHeader9
            // 
            columnHeader9.Text = "Action";
            columnHeader9.Width = 145;
            // 
            // tpVerbs
            // 
            tpVerbs.Controls.Add(lvVerbs);
            tpVerbs.Location = new System.Drawing.Point(4, 23);
            tpVerbs.Name = "tpVerbs";
            tpVerbs.Size = new System.Drawing.Size(552, 500);
            tpVerbs.TabIndex = 4;
            tpVerbs.Text = "HTTP Verbs";
            tpVerbs.UseVisualStyleBackColor = true;
            // 
            // lvVerbs
            // 
            lvVerbs.Columns.AddRange(new ColumnHeader[] { columnHeader4, columnHeader8 });
            lvVerbs.Dock = DockStyle.Fill;
            lvVerbs.FullRowSelect = true;
            lvVerbs.Location = new System.Drawing.Point(0, 0);
            lvVerbs.Margin = new Padding(5);
            lvVerbs.MultiSelect = false;
            lvVerbs.Name = "lvVerbs";
            lvVerbs.Size = new System.Drawing.Size(552, 500);
            lvVerbs.TabIndex = 1;
            lvVerbs.UseCompatibleStateImageBehavior = false;
            lvVerbs.View = View.Details;
            lvVerbs.SelectedIndexChanged += LvVerbsSelectedIndexChanged;
            lvVerbs.KeyDown += LvVerbs_KeyDown;
            lvVerbs.MouseDoubleClick += LvVerbs_MouseDoubleClick;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Verb";
            columnHeader4.Width = 175;
            // 
            // columnHeader8
            // 
            columnHeader8.Text = "Allowed";
            // 
            // tpHeaders
            // 
            tpHeaders.Controls.Add(lvHeaders);
            tpHeaders.Location = new System.Drawing.Point(4, 23);
            tpHeaders.Name = "tpHeaders";
            tpHeaders.Size = new System.Drawing.Size(552, 500);
            tpHeaders.TabIndex = 5;
            tpHeaders.Text = "Headers";
            tpHeaders.UseVisualStyleBackColor = true;
            // 
            // lvHeaders
            // 
            lvHeaders.Columns.AddRange(new ColumnHeader[] { columnHeader5, columnHeader7 });
            lvHeaders.Dock = DockStyle.Fill;
            lvHeaders.FullRowSelect = true;
            lvHeaders.Location = new System.Drawing.Point(0, 0);
            lvHeaders.Margin = new Padding(5);
            lvHeaders.MultiSelect = false;
            lvHeaders.Name = "lvHeaders";
            lvHeaders.Size = new System.Drawing.Size(552, 500);
            lvHeaders.TabIndex = 1;
            lvHeaders.UseCompatibleStateImageBehavior = false;
            lvHeaders.View = View.Details;
            lvHeaders.SelectedIndexChanged += LvHeadersSelectedIndexChanged;
            lvHeaders.KeyDown += LvHeaders_KeyDown;
            lvHeaders.MouseDoubleClick += LvHeaders_MouseDoubleClick;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Header";
            columnHeader5.Width = 175;
            // 
            // columnHeader7
            // 
            columnHeader7.Text = "Size Limit";
            // 
            // tpQuery
            // 
            tpQuery.Controls.Add(lvQueries);
            tpQuery.Location = new System.Drawing.Point(4, 23);
            tpQuery.Name = "tpQuery";
            tpQuery.Size = new System.Drawing.Size(552, 500);
            tpQuery.TabIndex = 6;
            tpQuery.Text = "Query Strings";
            tpQuery.UseVisualStyleBackColor = true;
            // 
            // lvQueries
            // 
            lvQueries.Columns.AddRange(new ColumnHeader[] { columnHeader6, columnHeader1 });
            lvQueries.Dock = DockStyle.Fill;
            lvQueries.FullRowSelect = true;
            lvQueries.Location = new System.Drawing.Point(0, 0);
            lvQueries.Margin = new Padding(5);
            lvQueries.MultiSelect = false;
            lvQueries.Name = "lvQueries";
            lvQueries.Size = new System.Drawing.Size(552, 500);
            lvQueries.TabIndex = 1;
            lvQueries.UseCompatibleStateImageBehavior = false;
            lvQueries.View = View.Details;
            lvQueries.SelectedIndexChanged += LvQueriesSelectedIndexChanged;
            lvQueries.KeyDown += LvQueries_KeyDown;
            lvQueries.MouseDoubleClick += LvQueries_MouseDoubleClick;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Query String";
            columnHeader6.Width = 145;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Action";
            columnHeader1.Width = 145;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Margin = new Padding(5);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(204, 13);
            label2.TabIndex = 1;
            label2.Text = "Use this feature to configure filtering rules.";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new System.Drawing.Point(10, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(32, 32);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label3.Location = new System.Drawing.Point(48, 10);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(175, 25);
            label3.TabIndex = 2;
            label3.Text = "Request Filtering";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(tsActionPanel, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(216, 600);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(216, 23);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(3, 5);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 13);
            label1.TabIndex = 0;
            label1.Text = "Actions";
            // 
            // tsActionPanel
            // 
            tsActionPanel.CanOverflow = false;
            tsActionPanel.Dock = DockStyle.Fill;
            tsActionPanel.GripStyle = ToolStripGripStyle.Hidden;
            tsActionPanel.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
            tsActionPanel.Location = new System.Drawing.Point(0, 23);
            tsActionPanel.Name = "tsActionPanel";
            tsActionPanel.Size = new System.Drawing.Size(216, 577);
            tsActionPanel.TabIndex = 2;
            tsActionPanel.Text = "toolStrip1";
            // 
            // RequestFilteringPage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Name = "RequestFilteringPage";
            Size = new System.Drawing.Size(800, 600);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabControl1.ResumeLayout(false);
            tpExtensions.ResumeLayout(false);
            tpRules.ResumeLayout(false);
            tpSegments.ResumeLayout(false);
            tpUrl.ResumeLayout(false);
            tpVerbs.ResumeLayout(false);
            tpHeaders.ResumeLayout(false);
            tpQuery.ResumeLayout(false);
            ((ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
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

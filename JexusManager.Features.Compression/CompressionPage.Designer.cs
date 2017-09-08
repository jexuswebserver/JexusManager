namespace JexusManager.Features.Compression
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class CompressionPage
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
            this.gbStatic = new System.Windows.Forms.GroupBox();
            this.txtDiskspaceLimit = new System.Windows.Forms.TextBox();
            this.cbDiskspaceLimit = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileSize = new System.Windows.Forms.TextBox();
            this.cbFileSize = new System.Windows.Forms.CheckBox();
            this.cbDynamic = new System.Windows.Forms.CheckBox();
            this.cbStatic = new System.Windows.Forms.CheckBox();
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
            this.gbStatic.SuspendLayout();
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
            this.splitContainer1.TabIndex = 5;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
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
            this.panel2.Controls.Add(this.gbStatic);
            this.panel2.Controls.Add(this.cbDynamic);
            this.panel2.Controls.Add(this.cbStatic);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(10, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(560, 540);
            this.panel2.TabIndex = 11;
            // 
            // gbStatic
            // 
            this.gbStatic.Controls.Add(this.txtDiskspaceLimit);
            this.gbStatic.Controls.Add(this.cbDiskspaceLimit);
            this.gbStatic.Controls.Add(this.btnBrowse);
            this.gbStatic.Controls.Add(this.txtPath);
            this.gbStatic.Controls.Add(this.label4);
            this.gbStatic.Controls.Add(this.txtFileSize);
            this.gbStatic.Controls.Add(this.cbFileSize);
            this.gbStatic.Location = new System.Drawing.Point(16, 80);
            this.gbStatic.Name = "gbStatic";
            this.gbStatic.Size = new System.Drawing.Size(450, 210);
            this.gbStatic.TabIndex = 5;
            this.gbStatic.TabStop = false;
            this.gbStatic.Text = "Static Compression";
            // 
            // txtDiskspaceLimit
            // 
            this.txtDiskspaceLimit.Location = new System.Drawing.Point(27, 172);
            this.txtDiskspaceLimit.Name = "txtDiskspaceLimit";
            this.txtDiskspaceLimit.Size = new System.Drawing.Size(110, 20);
            this.txtDiskspaceLimit.TabIndex = 6;
            this.txtDiskspaceLimit.TextChanged += new System.EventHandler(this.cbStatic_CheckedChanged);
            // 
            // cbDiskspaceLimit
            // 
            this.cbDiskspaceLimit.AutoSize = true;
            this.cbDiskspaceLimit.Location = new System.Drawing.Point(6, 142);
            this.cbDiskspaceLimit.Name = "cbDiskspaceLimit";
            this.cbDiskspaceLimit.Size = new System.Drawing.Size(232, 17);
            this.cbDiskspaceLimit.TabIndex = 5;
            this.cbDiskspaceLimit.Text = "Per application pool disk space limit (in MB):";
            this.cbDiskspaceLimit.UseVisualStyleBackColor = true;
            this.cbDiskspaceLimit.CheckedChanged += new System.EventHandler(this.cbDiskspaceLimit_CheckedChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(387, 107);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(30, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtPath
            // 
            this.txtPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.txtPath.Location = new System.Drawing.Point(6, 109);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(375, 20);
            this.txtPath.TabIndex = 3;
            this.txtPath.TextChanged += new System.EventHandler(this.cbStatic_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Cache directory:";
            // 
            // txtFileSize
            // 
            this.txtFileSize.Location = new System.Drawing.Point(27, 46);
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.Size = new System.Drawing.Size(110, 20);
            this.txtFileSize.TabIndex = 1;
            this.txtFileSize.TextChanged += new System.EventHandler(this.txtFileSize_TextChanged);
            // 
            // cbFileSize
            // 
            this.cbFileSize.AutoSize = true;
            this.cbFileSize.Location = new System.Drawing.Point(6, 19);
            this.cbFileSize.Name = "cbFileSize";
            this.cbFileSize.Size = new System.Drawing.Size(217, 17);
            this.cbFileSize.TabIndex = 0;
            this.cbFileSize.Text = "Only compress files larger than (in bytes):";
            this.cbFileSize.UseVisualStyleBackColor = true;
            this.cbFileSize.CheckedChanged += new System.EventHandler(this.cbFileSize_CheckedChanged);
            // 
            // cbDynamic
            // 
            this.cbDynamic.AutoSize = true;
            this.cbDynamic.Location = new System.Drawing.Point(16, 21);
            this.cbDynamic.Name = "cbDynamic";
            this.cbDynamic.Size = new System.Drawing.Size(202, 17);
            this.cbDynamic.TabIndex = 4;
            this.cbDynamic.Text = "Enable dynamic content compression";
            this.cbDynamic.UseVisualStyleBackColor = true;
            this.cbDynamic.CheckedChanged += new System.EventHandler(this.cbDynamic_CheckedChanged);
            // 
            // cbStatic
            // 
            this.cbStatic.AutoSize = true;
            this.cbStatic.Location = new System.Drawing.Point(16, 44);
            this.cbStatic.Name = "cbStatic";
            this.cbStatic.Size = new System.Drawing.Size(188, 17);
            this.cbStatic.TabIndex = 3;
            this.cbStatic.Text = "Enable static content compression";
            this.cbStatic.UseVisualStyleBackColor = true;
            this.cbStatic.CheckedChanged += new System.EventHandler(this.cbStatic_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(818, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Use this feature to configure settings for compression of responses. This can imp" +
    "rove the perceived performance of a website greatly and reduce bandwidth-related" +
    " charges.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(48, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Compression";
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
            // CompressionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CompressionPage";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gbStatic.ResumeLayout(false);
            this.gbStatic.PerformLayout();
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
        private CheckBox cbStatic;
        private Panel panel2;
        private PictureBox pictureBox1;
        private CheckBox cbDynamic;
        private GroupBox gbStatic;
        private TextBox txtDiskspaceLimit;
        private CheckBox cbDiskspaceLimit;
        private Button btnBrowse;
        private TextBox txtPath;
        private Label label4;
        private TextBox txtFileSize;
        private CheckBox cbFileSize;
        private ContextMenuStrip cmsActionPanel;
    }
}

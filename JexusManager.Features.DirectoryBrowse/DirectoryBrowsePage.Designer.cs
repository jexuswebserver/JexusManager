namespace JexusManager.Features.DirectoryBrowse
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class DirectoryBrowsePage
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
            this.cbLongDate = new System.Windows.Forms.CheckBox();
            this.cbDate = new System.Windows.Forms.CheckBox();
            this.cbExtension = new System.Windows.Forms.CheckBox();
            this.cbSize = new System.Windows.Forms.CheckBox();
            this.cbTime = new System.Windows.Forms.CheckBox();
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
            this.panel2.Controls.Add(this.cbLongDate);
            this.panel2.Controls.Add(this.cbDate);
            this.panel2.Controls.Add(this.cbExtension);
            this.panel2.Controls.Add(this.cbSize);
            this.panel2.Controls.Add(this.cbTime);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(10, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(560, 540);
            this.panel2.TabIndex = 11;
            // 
            // cbLongDate
            // 
            this.cbLongDate.AutoSize = true;
            this.cbLongDate.Enabled = false;
            this.cbLongDate.Location = new System.Drawing.Point(33, 158);
            this.cbLongDate.Name = "cbLongDate";
            this.cbLongDate.Size = new System.Drawing.Size(74, 17);
            this.cbLongDate.TabIndex = 8;
            this.cbLongDate.Text = "Long date";
            this.cbLongDate.UseVisualStyleBackColor = true;
            this.cbLongDate.CheckedChanged += new System.EventHandler(this.CbSslCheckedChanged);
            // 
            // cbDate
            // 
            this.cbDate.AutoSize = true;
            this.cbDate.Enabled = false;
            this.cbDate.Location = new System.Drawing.Point(16, 125);
            this.cbDate.Name = "cbDate";
            this.cbDate.Size = new System.Drawing.Size(49, 17);
            this.cbDate.TabIndex = 7;
            this.cbDate.Text = "Date";
            this.cbDate.UseVisualStyleBackColor = true;
            this.cbDate.CheckedChanged += new System.EventHandler(this.CbDateCheckedChanged);
            // 
            // cbExtension
            // 
            this.cbExtension.AutoSize = true;
            this.cbExtension.Enabled = false;
            this.cbExtension.Location = new System.Drawing.Point(16, 90);
            this.cbExtension.Name = "cbExtension";
            this.cbExtension.Size = new System.Drawing.Size(72, 17);
            this.cbExtension.TabIndex = 6;
            this.cbExtension.Text = "Extension";
            this.cbExtension.UseVisualStyleBackColor = true;
            this.cbExtension.CheckedChanged += new System.EventHandler(this.CbSslCheckedChanged);
            // 
            // cbSize
            // 
            this.cbSize.AutoSize = true;
            this.cbSize.Enabled = false;
            this.cbSize.Location = new System.Drawing.Point(16, 54);
            this.cbSize.Name = "cbSize";
            this.cbSize.Size = new System.Drawing.Size(46, 17);
            this.cbSize.TabIndex = 5;
            this.cbSize.Text = "Size";
            this.cbSize.UseVisualStyleBackColor = true;
            this.cbSize.CheckedChanged += new System.EventHandler(this.CbSslCheckedChanged);
            // 
            // cbTime
            // 
            this.cbTime.AutoSize = true;
            this.cbTime.Enabled = false;
            this.cbTime.Location = new System.Drawing.Point(16, 21);
            this.cbTime.Name = "cbTime";
            this.cbTime.Size = new System.Drawing.Size(49, 17);
            this.cbTime.TabIndex = 4;
            this.cbTime.Text = "Time";
            this.cbTime.UseVisualStyleBackColor = true;
            this.cbTime.CheckedChanged += new System.EventHandler(this.CbSslCheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(357, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Use this feature to specify the information that displays in a directory listing." +
    "";
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
            this.label3.Size = new System.Drawing.Size(192, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Directory Browsing";
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
            // DirectoryBrowsePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DirectoryBrowsePage";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
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
        private Panel panel2;
        private PictureBox pictureBox1;
        private CheckBox cbTime;
        private ContextMenuStrip cmsActionPanel;
        private CheckBox cbLongDate;
        private CheckBox cbDate;
        private CheckBox cbExtension;
        private CheckBox cbSize;
    }
}

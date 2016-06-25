namespace JexusManager.Features.Rewrite
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class RewritePage
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lvIn = new System.Windows.Forms.ListView();
            this.chNameIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chInputIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMatchIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPatternIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chActionIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUrl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStopIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTypeIn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvOut = new System.Windows.Forms.ListView();
            this.chNameOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chInputOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMatchOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPatternOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chActionOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStopOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTypeOut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            this.tableLayoutPanel2.SuspendLayout();
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
            this.panel2.Controls.Add(this.tableLayoutPanel2);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(10, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(560, 540);
            this.panel2.TabIndex = 6;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.lvIn, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lvOut, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 13);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(560, 527);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // lvIn
            // 
            this.lvIn.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chNameIn,
            this.chInputIn,
            this.chMatchIn,
            this.chPatternIn,
            this.chActionIn,
            this.chUrl,
            this.chStopIn,
            this.chTypeIn});
            this.lvIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvIn.FullRowSelect = true;
            this.lvIn.HideSelection = false;
            this.lvIn.LabelEdit = true;
            this.lvIn.Location = new System.Drawing.Point(5, 30);
            this.lvIn.Margin = new System.Windows.Forms.Padding(5);
            this.lvIn.MultiSelect = false;
            this.lvIn.Name = "lvIn";
            this.lvIn.Size = new System.Drawing.Size(550, 218);
            this.lvIn.TabIndex = 1;
            this.lvIn.UseCompatibleStateImageBehavior = false;
            this.lvIn.View = System.Windows.Forms.View.Details;
            this.lvIn.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.LvInAfterLabelEdit);
            this.lvIn.SelectedIndexChanged += new System.EventHandler(this.LvInSelectedIndexChanged);
            this.lvIn.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvInMouseDoubleClick);
            // 
            // chNameIn
            // 
            this.chNameIn.Text = "Name";
            this.chNameIn.Width = 180;
            // 
            // chInputIn
            // 
            this.chInputIn.Text = "Input";
            this.chInputIn.Width = 180;
            // 
            // chMatchIn
            // 
            this.chMatchIn.Text = "Match";
            this.chMatchIn.Width = 180;
            // 
            // chPatternIn
            // 
            this.chPatternIn.Text = "Pattern";
            this.chPatternIn.Width = 180;
            // 
            // chActionIn
            // 
            this.chActionIn.Text = "Action Type";
            this.chActionIn.Width = 100;
            // 
            // chUrl
            // 
            this.chUrl.Text = "Action URL";
            this.chUrl.Width = 100;
            // 
            // chStopIn
            // 
            this.chStopIn.Text = "Stop Processing";
            this.chStopIn.Width = 80;
            // 
            // chTypeIn
            // 
            this.chTypeIn.Text = "Entry Type";
            this.chTypeIn.Width = 80;
            // 
            // lvOut
            // 
            this.lvOut.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chNameOut,
            this.chInputOut,
            this.chMatchOut,
            this.chPatternOut,
            this.chActionOut,
            this.chValue,
            this.chStopOut,
            this.chTypeOut});
            this.lvOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvOut.FullRowSelect = true;
            this.lvOut.HideSelection = false;
            this.lvOut.Location = new System.Drawing.Point(5, 283);
            this.lvOut.Margin = new System.Windows.Forms.Padding(5);
            this.lvOut.MultiSelect = false;
            this.lvOut.Name = "lvOut";
            this.lvOut.Size = new System.Drawing.Size(550, 218);
            this.lvOut.TabIndex = 0;
            this.lvOut.UseCompatibleStateImageBehavior = false;
            this.lvOut.View = System.Windows.Forms.View.Details;
            this.lvOut.SelectedIndexChanged += new System.EventHandler(this.LvOutSelectedIndexChanged);
            this.lvOut.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvOutMouseDoubleClick);
            // 
            // chNameOut
            // 
            this.chNameOut.Text = "Name";
            this.chNameOut.Width = 180;
            // 
            // chInputOut
            // 
            this.chInputOut.Text = "Input";
            this.chInputOut.Width = 100;
            // 
            // chMatchOut
            // 
            this.chMatchOut.Text = "Match";
            this.chMatchOut.Width = 100;
            // 
            // chPatternOut
            // 
            this.chPatternOut.Text = "Pattern";
            this.chPatternOut.Width = 100;
            // 
            // chActionOut
            // 
            this.chActionOut.Text = "Action Type";
            this.chActionOut.Width = 100;
            // 
            // chValue
            // 
            this.chValue.Text = "Action Value";
            this.chValue.Width = 100;
            // 
            // chStopOut
            // 
            this.chStopOut.Text = "Stop Processing";
            this.chStopOut.Width = 80;
            // 
            // chTypeOut
            // 
            this.chTypeOut.Text = "Entry Type";
            this.chTypeOut.Width = 80;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(554, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "Inbound rules that are applied to the requested URL address:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(403, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Outbound rules that are applied to the headers or the content of an HTTP response" +
    ":";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(551, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Provides rewriting capabilities based on rules for the requested URL address and " +
    "the content of an HTTP response.";
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
            this.label3.Size = new System.Drawing.Size(132, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "URL Rewrite";
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
            // RewritePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RewritePage";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private TableLayoutPanel tableLayoutPanel2;
        private ListView lvIn;
        private ColumnHeader chNameIn;
        private ColumnHeader chInputIn;
        private ColumnHeader chMatchIn;
        private ColumnHeader chPatternIn;
        private ColumnHeader chActionIn;
        private ColumnHeader chUrl;
        private ColumnHeader chStopIn;
        private ColumnHeader chTypeIn;
        private ListView lvOut;
        private ColumnHeader chNameOut;
        private Label label4;
        private Label label5;
        private Label label2;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label label1;
        private ToolStrip tsActionPanel;
        private ColumnHeader chInputOut;
        private ColumnHeader chMatchOut;
        private ColumnHeader chPatternOut;
        private ColumnHeader chActionOut;
        private ColumnHeader chValue;
        private ColumnHeader chStopOut;
        private ColumnHeader chTypeOut;
        private Panel panel2;
        private PictureBox pictureBox1;
        private ContextMenuStrip cmsActionPanel;
    }
}

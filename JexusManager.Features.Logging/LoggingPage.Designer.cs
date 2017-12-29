namespace JexusManager.Features.Logging
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class LoggingPage
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
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbRollover = new System.Windows.Forms.GroupBox();
            this.cbLocalTime = new System.Windows.Forms.CheckBox();
            this.rbNoFile = new System.Windows.Forms.RadioButton();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.rbSize = new System.Windows.Forms.RadioButton();
            this.cbSchedule = new System.Windows.Forms.ComboBox();
            this.rbSchedule = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbBoth = new System.Windows.Forms.RadioButton();
            this.rbEvent = new System.Windows.Forms.RadioButton();
            this.rbFile = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbLogType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbEncoding = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cbFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.gbRollover.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.label2);
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
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // cmsActionPanel
            // 
            this.cmsActionPanel.Name = "cmsActionPanel";
            this.cmsActionPanel.Size = new System.Drawing.Size(61, 4);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(338, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Use this feature to configure how IIS logs requests on the Web server.";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.gbRollover);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cbLogType);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(10, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(560, 540);
            this.panel2.TabIndex = 7;
            // 
            // gbRollover
            // 
            this.gbRollover.Controls.Add(this.cbLocalTime);
            this.gbRollover.Controls.Add(this.rbNoFile);
            this.gbRollover.Controls.Add(this.txtSize);
            this.gbRollover.Controls.Add(this.rbSize);
            this.gbRollover.Controls.Add(this.cbSchedule);
            this.gbRollover.Controls.Add(this.rbSchedule);
            this.gbRollover.Controls.Add(this.label9);
            this.gbRollover.Location = new System.Drawing.Point(6, 429);
            this.gbRollover.Name = "gbRollover";
            this.gbRollover.Size = new System.Drawing.Size(517, 219);
            this.gbRollover.TabIndex = 7;
            this.gbRollover.TabStop = false;
            this.gbRollover.Text = "Log File Rollover";
            // 
            // cbLocalTime
            // 
            this.cbLocalTime.AutoSize = true;
            this.cbLocalTime.Location = new System.Drawing.Point(9, 189);
            this.cbLocalTime.Name = "cbLocalTime";
            this.cbLocalTime.Size = new System.Drawing.Size(218, 17);
            this.cbLocalTime.TabIndex = 6;
            this.cbLocalTime.Text = "Use local time for file naming and rollover";
            this.cbLocalTime.UseVisualStyleBackColor = true;
            this.cbLocalTime.CheckedChanged += new System.EventHandler(this.cbLocalTime_CheckedChanged);
            // 
            // rbNoFile
            // 
            this.rbNoFile.AutoSize = true;
            this.rbNoFile.Location = new System.Drawing.Point(9, 154);
            this.rbNoFile.Name = "rbNoFile";
            this.rbNoFile.Size = new System.Drawing.Size(151, 17);
            this.rbNoFile.TabIndex = 5;
            this.rbNoFile.TabStop = true;
            this.rbNoFile.Text = "Do not create new log files";
            this.rbNoFile.UseVisualStyleBackColor = true;
            this.rbNoFile.CheckedChanged += new System.EventHandler(this.rbSchedule_CheckedChanged);
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(24, 128);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(110, 20);
            this.txtSize.TabIndex = 4;
            this.txtSize.TextChanged += new System.EventHandler(this.txtSize_TextChanged);
            // 
            // rbSize
            // 
            this.rbSize.AutoSize = true;
            this.rbSize.Location = new System.Drawing.Point(9, 105);
            this.rbSize.Name = "rbSize";
            this.rbSize.Size = new System.Drawing.Size(154, 17);
            this.rbSize.TabIndex = 3;
            this.rbSize.TabStop = true;
            this.rbSize.Text = "Maximum file size (in bytes):";
            this.rbSize.UseVisualStyleBackColor = true;
            this.rbSize.CheckedChanged += new System.EventHandler(this.rbSchedule_CheckedChanged);
            // 
            // cbSchedule
            // 
            this.cbSchedule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSchedule.FormattingEnabled = true;
            this.cbSchedule.Items.AddRange(new object[] {
            "Hourly",
            "Daily",
            "Weekly",
            "Monthly"});
            this.cbSchedule.Location = new System.Drawing.Point(24, 78);
            this.cbSchedule.Name = "cbSchedule";
            this.cbSchedule.Size = new System.Drawing.Size(175, 21);
            this.cbSchedule.TabIndex = 2;
            this.cbSchedule.SelectedIndexChanged += new System.EventHandler(this.cbSchedule_SelectedIndexChanged);
            // 
            // rbSchedule
            // 
            this.rbSchedule.AutoSize = true;
            this.rbSchedule.Location = new System.Drawing.Point(9, 55);
            this.rbSchedule.Name = "rbSchedule";
            this.rbSchedule.Size = new System.Drawing.Size(73, 17);
            this.rbSchedule.TabIndex = 1;
            this.rbSchedule.TabStop = true;
            this.rbSchedule.Text = "Schedule:";
            this.rbSchedule.UseVisualStyleBackColor = true;
            this.rbSchedule.CheckedChanged += new System.EventHandler(this.rbSchedule_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(268, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Select the method that IIS uses to create a new log file.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbBoth);
            this.groupBox2.Controls.Add(this.rbEvent);
            this.groupBox2.Controls.Add(this.rbFile);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(6, 265);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(517, 147);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log Event Destination";
            // 
            // rbBoth
            // 
            this.rbBoth.AutoSize = true;
            this.rbBoth.Location = new System.Drawing.Point(9, 110);
            this.rbBoth.Name = "rbBoth";
            this.rbBoth.Size = new System.Drawing.Size(159, 17);
            this.rbBoth.TabIndex = 3;
            this.rbBoth.TabStop = true;
            this.rbBoth.Text = "Both log file and ETW event";
            this.rbBoth.UseVisualStyleBackColor = true;
            this.rbBoth.CheckedChanged += new System.EventHandler(this.rbFile_CheckedChanged);
            // 
            // rbEvent
            // 
            this.rbEvent.AutoSize = true;
            this.rbEvent.Location = new System.Drawing.Point(9, 83);
            this.rbEvent.Name = "rbEvent";
            this.rbEvent.Size = new System.Drawing.Size(102, 17);
            this.rbEvent.TabIndex = 2;
            this.rbEvent.TabStop = true;
            this.rbEvent.Text = "ETW event only";
            this.rbEvent.UseVisualStyleBackColor = true;
            this.rbEvent.CheckedChanged += new System.EventHandler(this.rbFile_CheckedChanged);
            // 
            // rbFile
            // 
            this.rbFile.AutoSize = true;
            this.rbFile.Location = new System.Drawing.Point(9, 56);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(81, 17);
            this.rbFile.TabIndex = 1;
            this.rbFile.TabStop = true;
            this.rbFile.Text = "Log file only";
            this.rbFile.UseVisualStyleBackColor = true;
            this.rbFile.CheckedChanged += new System.EventHandler(this.rbFile_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(254, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Select the destination where IIS will write log events.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "One log file per:";
            // 
            // cbLogType
            // 
            this.cbLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLogType.Enabled = false;
            this.cbLogType.FormattingEnabled = true;
            this.cbLogType.Items.AddRange(new object[] {
            "Server",
            "Site"});
            this.cbLogType.Location = new System.Drawing.Point(6, 22);
            this.cbLogType.Name = "cbLogType";
            this.cbLogType.Size = new System.Drawing.Size(121, 21);
            this.cbLogType.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbEncoding);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtPath);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.cbFormat);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(6, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 188);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log File";
            // 
            // cbEncoding
            // 
            this.cbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncoding.Enabled = false;
            this.cbEncoding.FormattingEnabled = true;
            this.cbEncoding.Items.AddRange(new object[] {
            "UTF-8",
            "ANSI"});
            this.cbEncoding.Location = new System.Drawing.Point(9, 152);
            this.cbEncoding.Name = "cbEncoding";
            this.cbEncoding.Size = new System.Drawing.Size(158, 21);
            this.cbEncoding.TabIndex = 7;
            this.cbEncoding.SelectedIndexChanged += new System.EventHandler(this.cbEncoding_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 136);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Encoding:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(379, 95);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(95, 23);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtPath
            // 
            this.txtPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.txtPath.Location = new System.Drawing.Point(9, 97);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(364, 20);
            this.txtPath.TabIndex = 4;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Directory:";
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(173, 42);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(95, 23);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select Fields...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cbFormat
            // 
            this.cbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFormat.Enabled = false;
            this.cbFormat.FormattingEnabled = true;
            this.cbFormat.Items.AddRange(new object[] {
            "IIS",
            "NCSA",
            "W3C",
            "Custom"});
            this.cbFormat.Location = new System.Drawing.Point(9, 44);
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(158, 21);
            this.cbFormat.TabIndex = 1;
            this.cbFormat.SelectedIndexChanged += new System.EventHandler(this.cbFormat_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Format:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(48, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Logging";
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
            this.label1.Location = new System.Drawing.Point(3, -17);
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
            // LoggingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "LoggingPage";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gbRollover.ResumeLayout(false);
            this.gbRollover.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private GroupBox groupBox1;
        private ComboBox cbEncoding;
        private Label label7;
        private Button btnBrowse;
        private TextBox txtPath;
        private Label label6;
        private Button btnSelect;
        private ComboBox cbFormat;
        private Label label5;
        private ComboBox cbLogType;
        private Label label4;
        private Panel panel2;
        private PictureBox pictureBox1;
        private ContextMenuStrip cmsActionPanel;
        private GroupBox gbRollover;
        private CheckBox cbLocalTime;
        private RadioButton rbNoFile;
        private TextBox txtSize;
        private RadioButton rbSize;
        private ComboBox cbSchedule;
        private RadioButton rbSchedule;
        private Label label9;
        private GroupBox groupBox2;
        private RadioButton rbBoth;
        private RadioButton rbEvent;
        private RadioButton rbFile;
        private Label label8;
    }
}

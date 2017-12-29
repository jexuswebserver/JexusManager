namespace JexusManager.Features.Rewrite.Inbound
{
    using System.ComponentModel;
    using System.Windows.Forms;

    using MakarovDev.ExpandCollapsePanel;

    partial class InboundRulePage
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.advancedFlowLayoutPanel1 = new MakarovDev.ExpandCollapsePanel.AdvancedFlowLayoutPanel();
            this.gbMatch = new MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel();
            this.cbIgnoreCase = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbMatch = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbConditions = new MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lvConditions = new System.Windows.Forms.ListView();
            this.chInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPattern = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbTrack = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cbAny = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.gbVariables = new MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel();
            this.btnVarRemove = new System.Windows.Forms.Button();
            this.btnVarEdit = new System.Windows.Forms.Button();
            this.btnVarAdd = new System.Windows.Forms.Button();
            this.btnVarUp = new System.Windows.Forms.Button();
            this.btnVarDown = new System.Windows.Forms.Button();
            this.lvVariables = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chReplace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel();
            this.gbRedirect = new System.Windows.Forms.GroupBox();
            this.cbRedirect = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbAppendRedirect = new System.Windows.Forms.CheckBox();
            this.txtRedirect = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.gbCustom = new System.Windows.Forms.GroupBox();
            this.txtError = new System.Windows.Forms.TextBox();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSubstatus = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbStop = new System.Windows.Forms.CheckBox();
            this.gbRewrite = new System.Windows.Forms.GroupBox();
            this.cbLog = new System.Windows.Forms.CheckBox();
            this.cbAppend = new System.Windows.Forms.CheckBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
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
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.advancedFlowLayoutPanel1.SuspendLayout();
            this.gbMatch.SuspendLayout();
            this.gbConditions.SuspendLayout();
            this.gbVariables.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbRedirect.SuspendLayout();
            this.gbCustom.SuspendLayout();
            this.gbRewrite.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 200;
            this.splitContainer1.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.SplitterDistance = 580;
            this.splitContainer1.TabIndex = 6;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitContainer1SplitterMoved);
            // 
            // cmsActionPanel
            // 
            this.cmsActionPanel.Name = "cmsActionPanel";
            this.cmsActionPanel.Size = new System.Drawing.Size(61, 4);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(580, 600);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.advancedFlowLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 63);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(574, 514);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // advancedFlowLayoutPanel1
            // 
            this.advancedFlowLayoutPanel1.AutoScroll = true;
            this.advancedFlowLayoutPanel1.Controls.Add(this.gbMatch);
            this.advancedFlowLayoutPanel1.Controls.Add(this.gbConditions);
            this.advancedFlowLayoutPanel1.Controls.Add(this.gbVariables);
            this.advancedFlowLayoutPanel1.Controls.Add(this.groupBox2);
            this.advancedFlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedFlowLayoutPanel1.Location = new System.Drawing.Point(3, 63);
            this.advancedFlowLayoutPanel1.Name = "advancedFlowLayoutPanel1";
            this.advancedFlowLayoutPanel1.Size = new System.Drawing.Size(568, 448);
            this.advancedFlowLayoutPanel1.TabIndex = 7;
            // 
            // gbMatch
            // 
            this.gbMatch.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.gbMatch.Controls.Add(this.cbIgnoreCase);
            this.gbMatch.Controls.Add(this.btnTest);
            this.gbMatch.Controls.Add(this.txtPattern);
            this.gbMatch.Controls.Add(this.label6);
            this.gbMatch.Controls.Add(this.cbType);
            this.gbMatch.Controls.Add(this.label5);
            this.gbMatch.Controls.Add(this.cbMatch);
            this.gbMatch.Controls.Add(this.label4);
            this.gbMatch.ExpandedHeight = 182;
            this.gbMatch.Location = new System.Drawing.Point(3, 3);
            this.gbMatch.Name = "gbMatch";
            this.gbMatch.Size = new System.Drawing.Size(545, 182);
            this.gbMatch.TabIndex = 5;
            this.gbMatch.Text = "Match URL";
            this.gbMatch.UseAnimation = true;
            // 
            // cbIgnoreCase
            // 
            this.cbIgnoreCase.AutoSize = true;
            this.cbIgnoreCase.Location = new System.Drawing.Point(18, 153);
            this.cbIgnoreCase.Name = "cbIgnoreCase";
            this.cbIgnoreCase.Size = new System.Drawing.Size(90, 19);
            this.cbIgnoreCase.TabIndex = 7;
            this.cbIgnoreCase.Text = "Ignore case";
            this.cbIgnoreCase.UseVisualStyleBackColor = true;
            this.cbIgnoreCase.CheckedChanged += new System.EventHandler(this.CbAppendRedirectCheckedChanged);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(549, 116);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(95, 23);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test patterns...";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTestClick);
            // 
            // txtPattern
            // 
            this.txtPattern.Location = new System.Drawing.Point(18, 126);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(525, 21);
            this.txtPattern.TabIndex = 5;
            this.txtPattern.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Pattern:";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "Regular Expressions",
            "Wildcards",
            "Exact Match"});
            this.cbType.Location = new System.Drawing.Point(379, 56);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(265, 21);
            this.cbType.TabIndex = 3;
            this.cbType.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(376, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Using:";
            // 
            // cbMatch
            // 
            this.cbMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMatch.FormattingEnabled = true;
            this.cbMatch.Items.AddRange(new object[] {
            "Matches the Pattern",
            "Does Not Match the Pattern"});
            this.cbMatch.Location = new System.Drawing.Point(18, 56);
            this.cbMatch.Name = "cbMatch";
            this.cbMatch.Size = new System.Drawing.Size(265, 21);
            this.cbMatch.TabIndex = 1;
            this.cbMatch.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Requested URL:";
            // 
            // gbConditions
            // 
            this.gbConditions.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.gbConditions.Controls.Add(this.btnDown);
            this.gbConditions.Controls.Add(this.btnUp);
            this.gbConditions.Controls.Add(this.btnRemove);
            this.gbConditions.Controls.Add(this.btnEdit);
            this.gbConditions.Controls.Add(this.lvConditions);
            this.gbConditions.Controls.Add(this.cbTrack);
            this.gbConditions.Controls.Add(this.btnAdd);
            this.gbConditions.Controls.Add(this.cbAny);
            this.gbConditions.Controls.Add(this.label20);
            this.gbConditions.ExpandedHeight = 300;
            this.gbConditions.IsExpanded = false;
            this.gbConditions.Location = new System.Drawing.Point(3, 191);
            this.gbConditions.Name = "gbConditions";
            this.gbConditions.Size = new System.Drawing.Size(545, 35);
            this.gbConditions.TabIndex = 9;
            this.gbConditions.Text = "Conditions";
            this.gbConditions.UseAnimation = true;
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnDown.Location = new System.Drawing.Point(539, 233);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(95, 23);
            this.btnDown.TabIndex = 12;
            this.btnDown.Text = "Move Down";
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.BtnDownClick);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnUp.Location = new System.Drawing.Point(539, 204);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(95, 23);
            this.btnUp.TabIndex = 11;
            this.btnUp.Text = "Move Up";
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.BtnUpClick);
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnRemove.Location = new System.Drawing.Point(539, 155);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(95, 23);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnEdit.Location = new System.Drawing.Point(539, 126);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(95, 23);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit...";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.BtnEditClick);
            // 
            // lvConditions
            // 
            this.lvConditions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lvConditions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chInput,
            this.chType,
            this.chPattern});
            this.lvConditions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lvConditions.FullRowSelect = true;
            this.lvConditions.HideSelection = false;
            this.lvConditions.Location = new System.Drawing.Point(8, 84);
            this.lvConditions.MultiSelect = false;
            this.lvConditions.Name = "lvConditions";
            this.lvConditions.Size = new System.Drawing.Size(525, 185);
            this.lvConditions.TabIndex = 8;
            this.lvConditions.UseCompatibleStateImageBehavior = false;
            this.lvConditions.View = System.Windows.Forms.View.Details;
            // 
            // chInput
            // 
            this.chInput.Text = "Input";
            this.chInput.Width = 100;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 160;
            // 
            // chPattern
            // 
            this.chPattern.Text = "Pattern";
            this.chPattern.Width = 180;
            // 
            // cbTrack
            // 
            this.cbTrack.AutoSize = true;
            this.cbTrack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbTrack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.cbTrack.Location = new System.Drawing.Point(18, 275);
            this.cbTrack.Name = "cbTrack";
            this.cbTrack.Size = new System.Drawing.Size(239, 19);
            this.cbTrack.TabIndex = 7;
            this.cbTrack.Text = "Track capture groups across conditions";
            this.cbTrack.UseVisualStyleBackColor = false;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnAdd.Location = new System.Drawing.Point(539, 97);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(95, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add...";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
            // 
            // cbAny
            // 
            this.cbAny.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbAny.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAny.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbAny.FormattingEnabled = true;
            this.cbAny.Items.AddRange(new object[] {
            "Match All",
            "Match Any"});
            this.cbAny.Location = new System.Drawing.Point(18, 56);
            this.cbAny.Name = "cbAny";
            this.cbAny.Size = new System.Drawing.Size(131, 21);
            this.cbAny.TabIndex = 1;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.label20.Location = new System.Drawing.Point(15, 40);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(102, 15);
            this.label20.TabIndex = 0;
            this.label20.Text = "Logical grouping:";
            // 
            // gbVariables
            // 
            this.gbVariables.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.gbVariables.Controls.Add(this.btnVarRemove);
            this.gbVariables.Controls.Add(this.btnVarEdit);
            this.gbVariables.Controls.Add(this.btnVarAdd);
            this.gbVariables.Controls.Add(this.btnVarUp);
            this.gbVariables.Controls.Add(this.btnVarDown);
            this.gbVariables.Controls.Add(this.lvVariables);
            this.gbVariables.ExpandedHeight = 229;
            this.gbVariables.IsExpanded = false;
            this.gbVariables.Location = new System.Drawing.Point(3, 232);
            this.gbVariables.Name = "gbVariables";
            this.gbVariables.Size = new System.Drawing.Size(545, 35);
            this.gbVariables.TabIndex = 8;
            this.gbVariables.Text = "Server Variables";
            this.gbVariables.UseAnimation = true;
            // 
            // btnVarRemove
            // 
            this.btnVarRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVarRemove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnVarRemove.Location = new System.Drawing.Point(539, 113);
            this.btnVarRemove.Name = "btnVarRemove";
            this.btnVarRemove.Size = new System.Drawing.Size(95, 23);
            this.btnVarRemove.TabIndex = 17;
            this.btnVarRemove.Text = "Remove";
            this.btnVarRemove.UseVisualStyleBackColor = false;
            this.btnVarRemove.Click += new System.EventHandler(this.BtnVarRemoveClick);
            // 
            // btnVarEdit
            // 
            this.btnVarEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVarEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnVarEdit.Location = new System.Drawing.Point(539, 84);
            this.btnVarEdit.Name = "btnVarEdit";
            this.btnVarEdit.Size = new System.Drawing.Size(95, 23);
            this.btnVarEdit.TabIndex = 16;
            this.btnVarEdit.Text = "Edit..";
            this.btnVarEdit.UseVisualStyleBackColor = false;
            this.btnVarEdit.Click += new System.EventHandler(this.BtnVarEditClick);
            // 
            // btnVarAdd
            // 
            this.btnVarAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVarAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnVarAdd.Location = new System.Drawing.Point(539, 55);
            this.btnVarAdd.Name = "btnVarAdd";
            this.btnVarAdd.Size = new System.Drawing.Size(95, 23);
            this.btnVarAdd.TabIndex = 15;
            this.btnVarAdd.Text = "Add...";
            this.btnVarAdd.UseVisualStyleBackColor = false;
            this.btnVarAdd.Click += new System.EventHandler(this.BtnVarAddClick);
            // 
            // btnVarUp
            // 
            this.btnVarUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVarUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnVarUp.Location = new System.Drawing.Point(539, 166);
            this.btnVarUp.Name = "btnVarUp";
            this.btnVarUp.Size = new System.Drawing.Size(95, 23);
            this.btnVarUp.TabIndex = 14;
            this.btnVarUp.Text = "Move Up";
            this.btnVarUp.UseVisualStyleBackColor = false;
            this.btnVarUp.Click += new System.EventHandler(this.BtnVarUpClick);
            // 
            // btnVarDown
            // 
            this.btnVarDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVarDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnVarDown.Location = new System.Drawing.Point(539, 195);
            this.btnVarDown.Name = "btnVarDown";
            this.btnVarDown.Size = new System.Drawing.Size(95, 23);
            this.btnVarDown.TabIndex = 13;
            this.btnVarDown.Text = "Move Down";
            this.btnVarDown.UseVisualStyleBackColor = false;
            this.btnVarDown.Click += new System.EventHandler(this.BtnVarDownClick);
            // 
            // lvVariables
            // 
            this.lvVariables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lvVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chValue,
            this.chReplace});
            this.lvVariables.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lvVariables.FullRowSelect = true;
            this.lvVariables.HideSelection = false;
            this.lvVariables.Location = new System.Drawing.Point(8, 43);
            this.lvVariables.MultiSelect = false;
            this.lvVariables.Name = "lvVariables";
            this.lvVariables.Size = new System.Drawing.Size(525, 185);
            this.lvVariables.TabIndex = 9;
            this.lvVariables.UseCompatibleStateImageBehavior = false;
            this.lvVariables.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 100;
            // 
            // chValue
            // 
            this.chValue.Text = "Value";
            this.chValue.Width = 160;
            // 
            // chReplace
            // 
            this.chReplace.Text = "Replace";
            this.chReplace.Width = 180;
            // 
            // groupBox2
            // 
            this.groupBox2.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.groupBox2.Controls.Add(this.gbRedirect);
            this.groupBox2.Controls.Add(this.gbCustom);
            this.groupBox2.Controls.Add(this.cbStop);
            this.groupBox2.Controls.Add(this.gbRewrite);
            this.groupBox2.Controls.Add(this.cbAction);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.ExpandedHeight = 261;
            this.groupBox2.Location = new System.Drawing.Point(3, 273);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(545, 261);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.Text = "Action";
            this.groupBox2.UseAnimation = true;
            // 
            // gbRedirect
            // 
            this.gbRedirect.Controls.Add(this.cbRedirect);
            this.gbRedirect.Controls.Add(this.label14);
            this.gbRedirect.Controls.Add(this.cbAppendRedirect);
            this.gbRedirect.Controls.Add(this.txtRedirect);
            this.gbRedirect.Controls.Add(this.label9);
            this.gbRedirect.Location = new System.Drawing.Point(18, 84);
            this.gbRedirect.Name = "gbRedirect";
            this.gbRedirect.Size = new System.Drawing.Size(626, 176);
            this.gbRedirect.TabIndex = 4;
            this.gbRedirect.TabStop = false;
            this.gbRedirect.Text = "Action Properties";
            // 
            // cbRedirect
            // 
            this.cbRedirect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRedirect.FormattingEnabled = true;
            this.cbRedirect.Items.AddRange(new object[] {
            "Permanent (301)",
            "Found (302)",
            "See Other (303)",
            "Temporary (307)"});
            this.cbRedirect.Location = new System.Drawing.Point(21, 122);
            this.cbRedirect.Name = "cbRedirect";
            this.cbRedirect.Size = new System.Drawing.Size(584, 21);
            this.cbRedirect.TabIndex = 4;
            this.cbRedirect.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 101);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(81, 15);
            this.label14.TabIndex = 3;
            this.label14.Text = "Redirect type:";
            // 
            // cbAppendRedirect
            // 
            this.cbAppendRedirect.AutoSize = true;
            this.cbAppendRedirect.Checked = true;
            this.cbAppendRedirect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAppendRedirect.Location = new System.Drawing.Point(21, 72);
            this.cbAppendRedirect.Name = "cbAppendRedirect";
            this.cbAppendRedirect.Size = new System.Drawing.Size(134, 19);
            this.cbAppendRedirect.TabIndex = 2;
            this.cbAppendRedirect.Text = "Append query string";
            this.cbAppendRedirect.UseVisualStyleBackColor = true;
            this.cbAppendRedirect.CheckedChanged += new System.EventHandler(this.CbAppendRedirectCheckedChanged);
            // 
            // txtRedirect
            // 
            this.txtRedirect.Location = new System.Drawing.Point(21, 43);
            this.txtRedirect.Name = "txtRedirect";
            this.txtRedirect.Size = new System.Drawing.Size(584, 21);
            this.txtRedirect.TabIndex = 1;
            this.txtRedirect.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "Redirect URL:";
            // 
            // gbCustom
            // 
            this.gbCustom.Controls.Add(this.txtError);
            this.gbCustom.Controls.Add(this.txtReason);
            this.gbCustom.Controls.Add(this.label13);
            this.gbCustom.Controls.Add(this.label12);
            this.gbCustom.Controls.Add(this.txtStatus);
            this.gbCustom.Controls.Add(this.label11);
            this.gbCustom.Controls.Add(this.txtSubstatus);
            this.gbCustom.Controls.Add(this.label10);
            this.gbCustom.Location = new System.Drawing.Point(18, 84);
            this.gbCustom.Name = "gbCustom";
            this.gbCustom.Size = new System.Drawing.Size(626, 197);
            this.gbCustom.TabIndex = 4;
            this.gbCustom.TabStop = false;
            this.gbCustom.Text = "Action Properties";
            // 
            // txtError
            // 
            this.txtError.Location = new System.Drawing.Point(21, 164);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(584, 21);
            this.txtError.TabIndex = 9;
            this.txtError.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(21, 117);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(584, 21);
            this.txtReason.TabIndex = 8;
            this.txtReason.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 143);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(100, 15);
            this.label13.TabIndex = 7;
            this.label13.Text = "Error description:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 15);
            this.label12.TabIndex = 6;
            this.label12.Text = "Reason:";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(133, 23);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(472, 21);
            this.txtStatus.TabIndex = 5;
            this.txtStatus.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 64);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 15);
            this.label11.TabIndex = 4;
            this.label11.Text = "Substatus code:";
            // 
            // txtSubstatus
            // 
            this.txtSubstatus.Location = new System.Drawing.Point(133, 61);
            this.txtSubstatus.Name = "txtSubstatus";
            this.txtSubstatus.Size = new System.Drawing.Size(472, 21);
            this.txtSubstatus.TabIndex = 1;
            this.txtSubstatus.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 15);
            this.label10.TabIndex = 0;
            this.label10.Text = "Status code:";
            // 
            // cbStop
            // 
            this.cbStop.AutoSize = true;
            this.cbStop.Location = new System.Drawing.Point(18, 243);
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(224, 19);
            this.cbStop.TabIndex = 3;
            this.cbStop.Text = "Stop processing of subsequent rules";
            this.cbStop.UseVisualStyleBackColor = true;
            this.cbStop.CheckedChanged += new System.EventHandler(this.CbAppendRedirectCheckedChanged);
            // 
            // gbRewrite
            // 
            this.gbRewrite.Controls.Add(this.cbLog);
            this.gbRewrite.Controls.Add(this.cbAppend);
            this.gbRewrite.Controls.Add(this.txtUrl);
            this.gbRewrite.Controls.Add(this.label8);
            this.gbRewrite.Location = new System.Drawing.Point(18, 84);
            this.gbRewrite.Name = "gbRewrite";
            this.gbRewrite.Size = new System.Drawing.Size(626, 143);
            this.gbRewrite.TabIndex = 2;
            this.gbRewrite.TabStop = false;
            this.gbRewrite.Text = "Action Properties";
            // 
            // cbLog
            // 
            this.cbLog.AutoSize = true;
            this.cbLog.Location = new System.Drawing.Point(21, 108);
            this.cbLog.Name = "cbLog";
            this.cbLog.Size = new System.Drawing.Size(125, 19);
            this.cbLog.TabIndex = 3;
            this.cbLog.Text = "Log rewritten URL";
            this.cbLog.UseVisualStyleBackColor = true;
            this.cbLog.CheckedChanged += new System.EventHandler(this.CbAppendRedirectCheckedChanged);
            // 
            // cbAppend
            // 
            this.cbAppend.AutoSize = true;
            this.cbAppend.Checked = true;
            this.cbAppend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAppend.Location = new System.Drawing.Point(21, 72);
            this.cbAppend.Name = "cbAppend";
            this.cbAppend.Size = new System.Drawing.Size(134, 19);
            this.cbAppend.TabIndex = 2;
            this.cbAppend.Text = "Append query string";
            this.cbAppend.UseVisualStyleBackColor = true;
            this.cbAppend.CheckedChanged += new System.EventHandler(this.CbAppendRedirectCheckedChanged);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(21, 43);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(584, 21);
            this.txtUrl.TabIndex = 1;
            this.txtUrl.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Rewrite URL:";
            // 
            // cbAction
            // 
            this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAction.FormattingEnabled = true;
            this.cbAction.Items.AddRange(new object[] {
            "None",
            "Rewrite",
            "Redirect",
            "Custom Response",
            "Abort Request"});
            this.cbAction.Location = new System.Drawing.Point(18, 56);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(140, 21);
            this.cbAction.TabIndex = 1;
            this.cbAction.SelectedIndexChanged += new System.EventHandler(this.CbActionSelectedIndexChanged);
            this.cbAction.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Action type:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 54);
            this.panel2.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(21, 27);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(665, 20);
            this.txtName.TabIndex = 4;
            this.txtName.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(574, 54);
            this.panel3.TabIndex = 8;
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
            this.label3.Size = new System.Drawing.Size(182, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Edit Inbound Rule";
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
            // InboundRulePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "InboundRulePage";
            this.Size = new System.Drawing.Size(800, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.advancedFlowLayoutPanel1.ResumeLayout(false);
            this.gbMatch.ResumeLayout(false);
            this.gbMatch.PerformLayout();
            this.gbConditions.ResumeLayout(false);
            this.gbConditions.PerformLayout();
            this.gbVariables.ResumeLayout(false);
            this.gbVariables.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbRedirect.ResumeLayout(false);
            this.gbRedirect.PerformLayout();
            this.gbCustom.ResumeLayout(false);
            this.gbCustom.PerformLayout();
            this.gbRewrite.ResumeLayout(false);
            this.gbRewrite.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer splitContainer1;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private Label label1;
        private ToolStrip tsActionPanel;
        private PictureBox pictureBox1;
        private ContextMenuStrip cmsActionPanel;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private AdvancedFlowLayoutPanel advancedFlowLayoutPanel1;
        private ExpandCollapsePanel gbMatch;
        private CheckBox cbIgnoreCase;
        private Button btnTest;
        private TextBox txtPattern;
        private Label label6;
        private ComboBox cbType;
        private Label label5;
        private ComboBox cbMatch;
        private Label label4;
        private ExpandCollapsePanel gbConditions;
        private Button btnDown;
        private Button btnUp;
        private Button btnRemove;
        private Button btnEdit;
        private ListView lvConditions;
        private ColumnHeader chInput;
        private ColumnHeader chType;
        private ColumnHeader chPattern;
        private CheckBox cbTrack;
        private Button btnAdd;
        private ComboBox cbAny;
        private Label label20;
        private ExpandCollapsePanel gbVariables;
        private Button btnVarRemove;
        private Button btnVarEdit;
        private Button btnVarAdd;
        private Button btnVarUp;
        private Button btnVarDown;
        private ListView lvVariables;
        private ColumnHeader chName;
        private ColumnHeader chValue;
        private ColumnHeader chReplace;
        private ExpandCollapsePanel groupBox2;
        private GroupBox gbRedirect;
        private ComboBox cbRedirect;
        private Label label14;
        private CheckBox cbAppendRedirect;
        private TextBox txtRedirect;
        private Label label9;
        private GroupBox gbCustom;
        private TextBox txtError;
        private TextBox txtReason;
        private Label label13;
        private Label label12;
        private TextBox txtStatus;
        private Label label11;
        private TextBox txtSubstatus;
        private Label label10;
        private CheckBox cbStop;
        private GroupBox gbRewrite;
        private CheckBox cbLog;
        private CheckBox cbAppend;
        private TextBox txtUrl;
        private Label label8;
        private ComboBox cbAction;
        private Label label7;
        private Panel panel2;
        private Label label2;
        private TextBox txtName;
        private Panel panel3;
    }
}

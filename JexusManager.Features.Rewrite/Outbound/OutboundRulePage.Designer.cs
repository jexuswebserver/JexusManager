namespace JexusManager.Features.Rewrite.Outbound
{
    using System.ComponentModel;
    using System.Windows.Forms;

    using MakarovDev.ExpandCollapsePanel;

    using PresentationControls;

    partial class OutboundRulePage
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
            PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cmsActionPanel = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.advancedFlowLayoutPanel1 = new MakarovDev.ExpandCollapsePanel.AdvancedFlowLayoutPanel();
            this.gbMatch = new MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel();
            this.lblVariable = new System.Windows.Forms.Label();
            this.lblTagMatch = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cbUsing = new System.Windows.Forms.ComboBox();
            this.cbTags = new System.Windows.Forms.ComboBox();
            this.lblCustomTags = new System.Windows.Forms.Label();
            this.cbContent = new System.Windows.Forms.ComboBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.cbIgnoreCase = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbMatch = new PresentationControls.CheckBoxComboBox();
            this.cbScope = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVariable = new System.Windows.Forms.TextBox();
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
            this.label19 = new System.Windows.Forms.Label();
            this.cbAny = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.groupBox2 = new MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel();
            this.cbStop = new System.Windows.Forms.CheckBox();
            this.gbRewrite = new System.Windows.Forms.GroupBox();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnEditPrecondition = new System.Windows.Forms.Button();
            this.cbPreCondition = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
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
            this.groupBox2.SuspendLayout();
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
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(574, 514);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // advancedFlowLayoutPanel1
            // 
            this.advancedFlowLayoutPanel1.AutoScroll = true;
            this.advancedFlowLayoutPanel1.Controls.Add(this.gbMatch);
            this.advancedFlowLayoutPanel1.Controls.Add(this.gbConditions);
            this.advancedFlowLayoutPanel1.Controls.Add(this.groupBox2);
            this.advancedFlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advancedFlowLayoutPanel1.Location = new System.Drawing.Point(3, 123);
            this.advancedFlowLayoutPanel1.Name = "advancedFlowLayoutPanel1";
            this.advancedFlowLayoutPanel1.Size = new System.Drawing.Size(568, 388);
            this.advancedFlowLayoutPanel1.TabIndex = 7;
            // 
            // gbMatch
            // 
            this.gbMatch.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.gbMatch.Controls.Add(this.lblVariable);
            this.gbMatch.Controls.Add(this.lblTagMatch);
            this.gbMatch.Controls.Add(this.label18);
            this.gbMatch.Controls.Add(this.cbUsing);
            this.gbMatch.Controls.Add(this.cbTags);
            this.gbMatch.Controls.Add(this.lblCustomTags);
            this.gbMatch.Controls.Add(this.cbContent);
            this.gbMatch.Controls.Add(this.lblContent);
            this.gbMatch.Controls.Add(this.cbIgnoreCase);
            this.gbMatch.Controls.Add(this.btnTest);
            this.gbMatch.Controls.Add(this.txtPattern);
            this.gbMatch.Controls.Add(this.label6);
            this.gbMatch.Controls.Add(this.cbMatch);
            this.gbMatch.Controls.Add(this.cbScope);
            this.gbMatch.Controls.Add(this.label4);
            this.gbMatch.Controls.Add(this.txtVariable);
            this.gbMatch.ExpandedHeight = 182;
            this.gbMatch.Location = new System.Drawing.Point(3, 3);
            this.gbMatch.Name = "gbMatch";
            this.gbMatch.Size = new System.Drawing.Size(545, 300);
            this.gbMatch.TabIndex = 5;
            this.gbMatch.Text = "Match";
            // 
            // lblVariable
            // 
            this.lblVariable.AutoSize = true;
            this.lblVariable.Location = new System.Drawing.Point(15, 76);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(90, 15);
            this.lblVariable.TabIndex = 15;
            this.lblVariable.Text = "Variable name:";
            // 
            // lblTagMatch
            // 
            this.lblTagMatch.AutoSize = true;
            this.lblTagMatch.Location = new System.Drawing.Point(15, 76);
            this.lblTagMatch.Name = "lblTagMatch";
            this.lblTagMatch.Size = new System.Drawing.Size(142, 15);
            this.lblTagMatch.TabIndex = 14;
            this.lblTagMatch.Text = "Match the content within:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(397, 145);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(42, 15);
            this.label18.TabIndex = 13;
            this.label18.Text = "Using:";
            // 
            // cbUsing
            // 
            this.cbUsing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsing.FormattingEnabled = true;
            this.cbUsing.Items.AddRange(new object[] {
            "Regular Expressions",
            "Wildcards",
            "Exact Match"});
            this.cbUsing.Location = new System.Drawing.Point(400, 163);
            this.cbUsing.Name = "cbUsing";
            this.cbUsing.Size = new System.Drawing.Size(265, 21);
            this.cbUsing.TabIndex = 12;
            // 
            // cbTags
            // 
            this.cbTags.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTags.Enabled = false;
            this.cbTags.FormattingEnabled = true;
            this.cbTags.Location = new System.Drawing.Point(400, 94);
            this.cbTags.Name = "cbTags";
            this.cbTags.Size = new System.Drawing.Size(265, 21);
            this.cbTags.TabIndex = 11;
            this.cbTags.SelectedIndexChanged += new System.EventHandler(this.CbTagsSelectedIndexChanged);
            // 
            // lblCustomTags
            // 
            this.lblCustomTags.AutoSize = true;
            this.lblCustomTags.Location = new System.Drawing.Point(397, 76);
            this.lblCustomTags.Name = "lblCustomTags";
            this.lblCustomTags.Size = new System.Drawing.Size(78, 15);
            this.lblCustomTags.TabIndex = 10;
            this.lblCustomTags.Text = "Custom tags:";
            // 
            // cbContent
            // 
            this.cbContent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbContent.FormattingEnabled = true;
            this.cbContent.Items.AddRange(new object[] {
            "Matches the Pattern",
            "Does Not Match the Pattern"});
            this.cbContent.Location = new System.Drawing.Point(18, 163);
            this.cbContent.Name = "cbContent";
            this.cbContent.Size = new System.Drawing.Size(270, 21);
            this.cbContent.TabIndex = 9;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(15, 145);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(52, 15);
            this.lblContent.TabIndex = 8;
            this.lblContent.Text = "Content:";
            // 
            // cbIgnoreCase
            // 
            this.cbIgnoreCase.AutoSize = true;
            this.cbIgnoreCase.Location = new System.Drawing.Point(18, 263);
            this.cbIgnoreCase.Name = "cbIgnoreCase";
            this.cbIgnoreCase.Size = new System.Drawing.Size(90, 19);
            this.cbIgnoreCase.TabIndex = 7;
            this.cbIgnoreCase.Text = "Ignore case";
            this.cbIgnoreCase.UseVisualStyleBackColor = true;
            this.cbIgnoreCase.CheckedChanged += new System.EventHandler(this.CbAppendRedirectCheckedChanged);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(549, 236);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(95, 23);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test patterns...";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTestClick);
            // 
            // txtPattern
            // 
            this.txtPattern.Location = new System.Drawing.Point(18, 236);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(525, 21);
            this.txtPattern.TabIndex = 5;
            this.txtPattern.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 218);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Pattern:";
            // 
            // cbMatch
            // 
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbMatch.CheckBoxProperties = checkBoxProperties1;
            this.cbMatch.DisplayMemberSingleItem = "";
            this.cbMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMatch.FormattingEnabled = true;
            this.cbMatch.Items.AddRange(new object[] {
            "A (href attribute)",
            "Area (href attribute)",
            "Base (href attribute)",
            "Form (action attribute)",
            "Frame (src,longdesc attributes)",
            "Head (profile attribute)",
            "IFrame (src,longdesc attributes)",
            "Img (src,longdesc,usemap attributes)",
            "Input (src,usemap attributes)",
            "Link (href attribute)",
            "Script (src attribute)",
            "Use Custom Tags"});
            this.cbMatch.Location = new System.Drawing.Point(18, 94);
            this.cbMatch.Name = "cbMatch";
            this.cbMatch.Size = new System.Drawing.Size(270, 21);
            this.cbMatch.TabIndex = 3;
            this.cbMatch.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // cbScope
            // 
            this.cbScope.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScope.FormattingEnabled = true;
            this.cbScope.Items.AddRange(new object[] {
            "Response",
            "Server Variable"});
            this.cbScope.Location = new System.Drawing.Point(18, 43);
            this.cbScope.Name = "cbScope";
            this.cbScope.Size = new System.Drawing.Size(140, 21);
            this.cbScope.TabIndex = 1;
            this.cbScope.SelectedIndexChanged += new System.EventHandler(this.CbScopeSelectedIndexChanged);
            this.cbScope.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Matching scope:";
            // 
            // txtVariable
            // 
            this.txtVariable.Location = new System.Drawing.Point(18, 94);
            this.txtVariable.Name = "txtVariable";
            this.txtVariable.Size = new System.Drawing.Size(553, 21);
            this.txtVariable.TabIndex = 16;
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
            this.gbConditions.Controls.Add(this.label19);
            this.gbConditions.Controls.Add(this.cbAny);
            this.gbConditions.Controls.Add(this.label20);
            this.gbConditions.ExpandedHeight = 288;
            this.gbConditions.IsExpanded = false;
            this.gbConditions.Location = new System.Drawing.Point(3, 309);
            this.gbConditions.Name = "gbConditions";
            this.gbConditions.Size = new System.Drawing.Size(545, 35);
            this.gbConditions.TabIndex = 9;
            this.gbConditions.Text = "Conditions";
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.btnDown.Location = new System.Drawing.Point(539, 219);
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
            this.btnUp.Location = new System.Drawing.Point(539, 190);
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
            this.btnRemove.Location = new System.Drawing.Point(539, 141);
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
            this.btnEdit.Location = new System.Drawing.Point(539, 112);
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
            this.lvConditions.Location = new System.Drawing.Point(8, 70);
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
            this.cbTrack.Location = new System.Drawing.Point(18, 261);
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
            this.btnAdd.Location = new System.Drawing.Point(539, 83);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(95, 23);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "Add...";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.label19.Location = new System.Drawing.Point(376, 27);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(42, 15);
            this.label19.TabIndex = 2;
            this.label19.Text = "Using:";
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
            this.cbAny.Location = new System.Drawing.Point(18, 43);
            this.cbAny.Name = "cbAny";
            this.cbAny.Size = new System.Drawing.Size(131, 21);
            this.cbAny.TabIndex = 1;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.label20.Location = new System.Drawing.Point(15, 27);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(102, 15);
            this.label20.TabIndex = 0;
            this.label20.Text = "Logical grouping:";
            // 
            // groupBox2
            // 
            this.groupBox2.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.groupBox2.Controls.Add(this.cbStop);
            this.groupBox2.Controls.Add(this.gbRewrite);
            this.groupBox2.Controls.Add(this.cbAction);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.ExpandedHeight = 261;
            this.groupBox2.Location = new System.Drawing.Point(3, 350);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(545, 264);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.Text = "Action";
            // 
            // cbStop
            // 
            this.cbStop.AutoSize = true;
            this.cbStop.Location = new System.Drawing.Point(17, 178);
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(224, 19);
            this.cbStop.TabIndex = 3;
            this.cbStop.Text = "Stop processing of subsequent rules";
            this.cbStop.UseVisualStyleBackColor = true;
            this.cbStop.CheckedChanged += new System.EventHandler(this.CbAppendRedirectCheckedChanged);
            // 
            // gbRewrite
            // 
            this.gbRewrite.Controls.Add(this.txtValue);
            this.gbRewrite.Controls.Add(this.label8);
            this.gbRewrite.Location = new System.Drawing.Point(18, 70);
            this.gbRewrite.Name = "gbRewrite";
            this.gbRewrite.Size = new System.Drawing.Size(626, 90);
            this.gbRewrite.TabIndex = 2;
            this.gbRewrite.TabStop = false;
            this.gbRewrite.Text = "Action Properties";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(21, 43);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(584, 21);
            this.txtValue.TabIndex = 1;
            this.txtValue.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Value:";
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
            this.cbAction.Location = new System.Drawing.Point(18, 42);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(140, 21);
            this.cbAction.TabIndex = 1;
            this.cbAction.SelectedIndexChanged += new System.EventHandler(this.CbActionSelectedIndexChanged);
            this.cbAction.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Action type:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnEditPrecondition);
            this.panel2.Controls.Add(this.cbPreCondition);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(568, 114);
            this.panel2.TabIndex = 11;
            // 
            // btnEditPrecondition
            // 
            this.btnEditPrecondition.Enabled = false;
            this.btnEditPrecondition.Location = new System.Drawing.Point(542, 88);
            this.btnEditPrecondition.Name = "btnEditPrecondition";
            this.btnEditPrecondition.Size = new System.Drawing.Size(53, 23);
            this.btnEditPrecondition.TabIndex = 7;
            this.btnEditPrecondition.Text = "Edit...";
            this.btnEditPrecondition.UseVisualStyleBackColor = true;
            this.btnEditPrecondition.Click += new System.EventHandler(this.BtnEditPreconditionClick);
            // 
            // cbPreCondition
            // 
            this.cbPreCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPreCondition.FormattingEnabled = true;
            this.cbPreCondition.Location = new System.Drawing.Point(21, 90);
            this.cbPreCondition.Name = "cbPreCondition";
            this.cbPreCondition.Size = new System.Drawing.Size(515, 21);
            this.cbPreCondition.TabIndex = 6;
            this.cbPreCondition.SelectedIndexChanged += new System.EventHandler(this.CbPreConditionSelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 74);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "Precondition:";
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
            this.label3.Size = new System.Drawing.Size(199, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Edit Outbound Rule";
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
            // OutboundRulePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "OutboundRulePage";
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private CheckBoxComboBox cbMatch;
        private ComboBox cbScope;
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
        private Label label19;
        private ComboBox cbAny;
        private Label label20;
        private ExpandCollapsePanel groupBox2;
        private CheckBox cbStop;
        private GroupBox gbRewrite;
        private TextBox txtValue;
        private Label label8;
        private ComboBox cbAction;
        private Label label7;
        private Panel panel2;
        private Label label2;
        private TextBox txtName;
        private Panel panel3;
        private Button btnEditPrecondition;
        private ComboBox cbPreCondition;
        private Label label15;
        private Label lblTagMatch;
        private Label label18;
        private ComboBox cbUsing;
        private ComboBox cbTags;
        private Label lblCustomTags;
        private ComboBox cbContent;
        private Label lblContent;
        private TextBox txtVariable;
        private Label lblVariable;
    }
}

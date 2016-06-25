namespace JexusManager.WizardPanels
{
    partial class AddressPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cbOnline = new System.Windows.Forms.CheckBox();
            this.txtAdvanced = new System.Windows.Forms.LinkLabel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.lvServers = new System.Windows.Forms.ListView();
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pgAdvanced = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(21, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server address:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(24, 36);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(220, 20);
            this.txtName.TabIndex = 8;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // cbOnline
            // 
            this.cbOnline.AutoSize = true;
            this.cbOnline.Enabled = false;
            this.cbOnline.Location = new System.Drawing.Point(24, 62);
            this.cbOnline.Name = "cbOnline";
            this.cbOnline.Size = new System.Drawing.Size(56, 17);
            this.cbOnline.TabIndex = 9;
            this.cbOnline.Text = "Online";
            this.cbOnline.UseVisualStyleBackColor = true;
            // 
            // txtAdvanced
            // 
            this.txtAdvanced.AutoSize = true;
            this.txtAdvanced.Location = new System.Drawing.Point(25, 82);
            this.txtAdvanced.Name = "txtAdvanced";
            this.txtAdvanced.Size = new System.Drawing.Size(104, 13);
            this.txtAdvanced.TabIndex = 10;
            this.txtAdvanced.TabStop = true;
            this.txtAdvanced.Text = "Advanced settings...";
            this.txtAdvanced.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.txtAdvanced_LinkClicked);
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(358, 34);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(140, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(358, 82);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(140, 23);
            this.btnRemove.TabIndex = 12;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvServers
            // 
            this.lvServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAddress,
            this.chStatus});
            this.lvServers.FullRowSelect = true;
            this.lvServers.Location = new System.Drawing.Point(24, 104);
            this.lvServers.MultiSelect = false;
            this.lvServers.Name = "lvServers";
            this.lvServers.Size = new System.Drawing.Size(322, 250);
            this.lvServers.TabIndex = 13;
            this.lvServers.UseCompatibleStateImageBehavior = false;
            this.lvServers.View = System.Windows.Forms.View.Details;
            this.lvServers.SelectedIndexChanged += new System.EventHandler(this.lvServers_SelectedIndexChanged);
            // 
            // chAddress
            // 
            this.chAddress.Text = "Server Address";
            this.chAddress.Width = 160;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Status";
            this.chStatus.Width = 120;
            // 
            // pgAdvanced
            // 
            this.pgAdvanced.HelpVisible = false;
            this.pgAdvanced.Location = new System.Drawing.Point(24, 104);
            this.pgAdvanced.Name = "pgAdvanced";
            this.pgAdvanced.Size = new System.Drawing.Size(322, 119);
            this.pgAdvanced.TabIndex = 14;
            this.pgAdvanced.ToolbarVisible = false;
            this.pgAdvanced.Visible = false;
            // 
            // AddressPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pgAdvanced);
            this.Controls.Add(this.lvServers);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtAdvanced);
            this.Controls.Add(this.cbOnline);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Name = "AddressPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox cbOnline;
        private System.Windows.Forms.LinkLabel txtAdvanced;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListView lvServers;
        private System.Windows.Forms.ColumnHeader chAddress;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.PropertyGrid pgAdvanced;
    }
}

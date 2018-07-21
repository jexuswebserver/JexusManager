namespace JexusManager.Dialogs
{
    using System.ComponentModel;
    using System.Windows.Forms;

    sealed partial class NewApplicationDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPath = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSite = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAlias = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPool = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPhysicalPath = new System.Windows.Forms.TextBox();
            this.txtConnectAs = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.cbPreload = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSite);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtPath
            // 
            this.txtPath.AutoSize = true;
            this.txtPath.Location = new System.Drawing.Point(96, 49);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(0, 13);
            this.txtPath.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Path:";
            // 
            // txtSite
            // 
            this.txtSite.AutoSize = true;
            this.txtSite.Location = new System.Drawing.Point(96, 20);
            this.txtSite.Name = "txtSite";
            this.txtSite.Size = new System.Drawing.Size(0, 13);
            this.txtSite.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Site name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Alias:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Application Pool:";
            // 
            // txtAlias
            // 
            this.txtAlias.Location = new System.Drawing.Point(15, 127);
            this.txtAlias.Name = "txtAlias";
            this.txtAlias.Size = new System.Drawing.Size(185, 20);
            this.txtAlias.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Examples: sales";
            // 
            // txtPool
            // 
            this.txtPool.Location = new System.Drawing.Point(206, 127);
            this.txtPool.Name = "txtPool";
            this.txtPool.ReadOnly = true;
            this.txtPool.Size = new System.Drawing.Size(185, 20);
            this.txtPool.TabIndex = 5;
            this.txtPool.Text = "DefaultAppPool";
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(396, 125);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(95, 23);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "Select...";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Physical path:";
            // 
            // txtPhysicalPath
            // 
            this.txtPhysicalPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtPhysicalPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.txtPhysicalPath.Location = new System.Drawing.Point(15, 196);
            this.txtPhysicalPath.Name = "txtPhysicalPath";
            this.txtPhysicalPath.Size = new System.Drawing.Size(324, 20);
            this.txtPhysicalPath.TabIndex = 8;
            // 
            // txtConnectAs
            // 
            this.txtConnectAs.AutoSize = true;
            this.txtConnectAs.Location = new System.Drawing.Point(12, 235);
            this.txtConnectAs.Name = "txtConnectAs";
            this.txtConnectAs.Size = new System.Drawing.Size(139, 13);
            this.txtConnectAs.TabIndex = 9;
            this.txtConnectAs.Text = "Pass-through authentication";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(15, 251);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(94, 23);
            this.btnConnect.TabIndex = 10;
            this.btnConnect.Text = "Connect as...";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(115, 251);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(94, 23);
            this.btnTest.TabIndex = 11;
            this.btnTest.Text = "Test Settings...";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // cbPreload
            // 
            this.cbPreload.AutoSize = true;
            this.cbPreload.Enabled = false;
            this.cbPreload.Location = new System.Drawing.Point(15, 280);
            this.cbPreload.Name = "cbPreload";
            this.cbPreload.Size = new System.Drawing.Size(98, 17);
            this.cbPreload.TabIndex = 12;
            this.cbPreload.Text = "Enable Preload";
            this.cbPreload.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(345, 194);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(33, 23);
            this.btnBrowse.TabIndex = 13;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(306, 316);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(407, 316);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // NewApplicationDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(514, 351);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.cbPreload);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtConnectAs);
            this.Controls.Add(this.txtPhysicalPath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtPool);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtAlias);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Name = "NewApplicationDialog";
            this.Text = "Add Application";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.NewApplicationDialog_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupBox1;
        private Label txtPath;
        private Label label3;
        private Label txtSite;
        private Label label1;
        private Label label2;
        private Label label4;
        private TextBox txtAlias;
        private Label label5;
        private TextBox txtPool;
        private Button btnSelect;
        private Label label6;
        private TextBox txtPhysicalPath;
        private Label txtConnectAs;
        private Button btnConnect;
        private Button btnTest;
        private CheckBox cbPreload;
        private Button btnBrowse;
        private Button btnOK;
        private Button btnCancel;
    }
}
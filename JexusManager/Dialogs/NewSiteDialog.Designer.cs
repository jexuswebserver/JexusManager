namespace JexusManager.Dialogs
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class NewSiteDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPool = new System.Windows.Forms.TextBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtConnectAs = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCertificates = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cbCertificates = new System.Windows.Forms.ComboBox();
            this.cbSniRequired = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.cbAddress = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbStart = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Site name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(25, 40);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(210, 20);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Application pool:";
            // 
            // txtPool
            // 
            this.txtPool.Location = new System.Drawing.Point(253, 40);
            this.txtPool.Name = "txtPool";
            this.txtPool.ReadOnly = true;
            this.txtPool.Size = new System.Drawing.Size(185, 20);
            this.txtPool.TabIndex = 3;
            this.txtPool.Text = "DefaultAppPool";
            // 
            // btnChoose
            // 
            this.btnChoose.Enabled = false;
            this.btnChoose.Location = new System.Drawing.Point(444, 38);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(95, 23);
            this.btnChoose.TabIndex = 4;
            this.btnChoose.Text = "Select...";
            this.btnChoose.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.txtConnectAs);
            this.groupBox1.Controls.Add(this.btnBrowse);
            this.groupBox1.Controls.Add(this.txtPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(25, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(514, 134);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Content Directory";
            // 
            // btnTest
            // 
            this.btnTest.Enabled = false;
            this.btnTest.Location = new System.Drawing.Point(115, 98);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(90, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test Settings...";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(24, 98);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(85, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect as...";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // txtConnectAs
            // 
            this.txtConnectAs.AutoSize = true;
            this.txtConnectAs.Location = new System.Drawing.Point(21, 69);
            this.txtConnectAs.Name = "txtConnectAs";
            this.txtConnectAs.Size = new System.Drawing.Size(139, 13);
            this.txtConnectAs.TabIndex = 3;
            this.txtConnectAs.Text = "Pass-through authentication";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(346, 44);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(38, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            // 
            // txtPath
            // 
            this.txtPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.txtPath.Location = new System.Drawing.Point(24, 46);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(316, 20);
            this.txtPath.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Physical path:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCertificates);
            this.groupBox2.Controls.Add(this.btnView);
            this.groupBox2.Controls.Add(this.btnSelect);
            this.groupBox2.Controls.Add(this.cbCertificates);
            this.groupBox2.Controls.Add(this.cbSniRequired);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtHost);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtPort);
            this.groupBox2.Controls.Add(this.cbAddress);
            this.groupBox2.Controls.Add(this.cbType);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(25, 214);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(514, 254);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Binding";
            // 
            // txtCertificates
            // 
            this.txtCertificates.AutoSize = true;
            this.txtCertificates.Location = new System.Drawing.Point(18, 189);
            this.txtCertificates.Name = "txtCertificates";
            this.txtCertificates.Size = new System.Drawing.Size(79, 13);
            this.txtCertificates.TabIndex = 29;
            this.txtCertificates.Text = "SSL certificate:";
            this.txtCertificates.Visible = false;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(421, 203);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 23);
            this.btnView.TabIndex = 28;
            this.btnView.Text = "View...";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(330, 203);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(85, 23);
            this.btnSelect.TabIndex = 27;
            this.btnSelect.Text = "Select...";
            this.btnSelect.UseVisualStyleBackColor = true;
            // 
            // cbCertificates
            // 
            this.cbCertificates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCertificates.FormattingEnabled = true;
            this.cbCertificates.Location = new System.Drawing.Point(21, 205);
            this.cbCertificates.Name = "cbCertificates";
            this.cbCertificates.Size = new System.Drawing.Size(303, 21);
            this.cbCertificates.TabIndex = 26;
            // 
            // cbSniRequired
            // 
            this.cbSniRequired.AutoSize = true;
            this.cbSniRequired.Location = new System.Drawing.Point(24, 136);
            this.cbSniRequired.Name = "cbSniRequired";
            this.cbSniRequired.Size = new System.Drawing.Size(177, 17);
            this.cbSniRequired.TabIndex = 25;
            this.cbSniRequired.Text = "Require Server Name Indication";
            this.cbSniRequired.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(266, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Example: www.contoso.com or marketing.contoso.com";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(21, 86);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(303, 20);
            this.txtHost.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Host name:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(358, 33);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(48, 20);
            this.txtPort.TabIndex = 21;
            this.txtPort.Text = "80";
            // 
            // cbAddress
            // 
            this.cbAddress.FormattingEnabled = true;
            this.cbAddress.Location = new System.Drawing.Point(142, 33);
            this.cbAddress.Name = "cbAddress";
            this.cbAddress.Size = new System.Drawing.Size(210, 21);
            this.cbAddress.TabIndex = 20;
            this.cbAddress.Text = "All Unassigned";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "http",
            "https"});
            this.cbType.Location = new System.Drawing.Point(21, 33);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(115, 21);
            this.cbType.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(355, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Port:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(139, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "IP address:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Type:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(477, 501);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(376, 501);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // cbStart
            // 
            this.cbStart.AutoSize = true;
            this.cbStart.Checked = true;
            this.cbStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStart.Location = new System.Drawing.Point(25, 474);
            this.cbStart.Name = "cbStart";
            this.cbStart.Size = new System.Drawing.Size(147, 17);
            this.cbStart.TabIndex = 9;
            this.cbStart.Text = "Start Website immediately";
            this.cbStart.UseVisualStyleBackColor = true;
            // 
            // NewSiteDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 536);
            this.Controls.Add(this.cbStart);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.txtPool);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Name = "NewSiteDialog";
            this.Text = "Add Website";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.NewSiteDialogHelpButtonClicked);
            this.Load += new System.EventHandler(this.NewSiteDialogLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox txtName;
        private Label label2;
        private TextBox txtPool;
        private Button btnChoose;
        private GroupBox groupBox1;
        private Button btnTest;
        private Button btnConnect;
        private Label txtConnectAs;
        private Button btnBrowse;
        private TextBox txtPath;
        private Label label3;
        private GroupBox groupBox2;
        private Button btnView;
        private Button btnSelect;
        private ComboBox cbCertificates;
        private CheckBox cbSniRequired;
        private Label label5;
        private TextBox txtHost;
        private Label label6;
        private TextBox txtPort;
        private ComboBox cbAddress;
        private ComboBox cbType;
        private Label label7;
        private Label label8;
        private Label label9;
        private Button btnCancel;
        private Button btnOK;
        private CheckBox cbStart;
        private Label txtCertificates;
    }
}

namespace JexusManager.Features.Main
{
    using System.ComponentModel;
    using System.Windows.Forms;

    sealed partial class BindingDialog
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbAddress = new System.Windows.Forms.ComboBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbSniRequired = new System.Windows.Forms.CheckBox();
            this.txtCertificates = new System.Windows.Forms.Label();
            this.cbCertificates = new System.Windows.Forms.ComboBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(417, 244);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 25);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(321, 244);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP address:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(346, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Port:";
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "http",
            "https"});
            this.cbType.Location = new System.Drawing.Point(12, 30);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(115, 21);
            this.cbType.TabIndex = 5;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.CbTypeSelectedIndexChanged);
            // 
            // cbAddress
            // 
            this.cbAddress.FormattingEnabled = true;
            this.cbAddress.Location = new System.Drawing.Point(133, 30);
            this.cbAddress.Name = "cbAddress";
            this.cbAddress.Size = new System.Drawing.Size(210, 21);
            this.cbAddress.TabIndex = 6;
            this.cbAddress.Text = "All Unassigned";
            this.cbAddress.TextChanged += new System.EventHandler(this.CbAddressTextChanged);
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(349, 30);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(48, 20);
            this.txtPort.TabIndex = 7;
            this.txtPort.Text = "80";
            this.txtPort.TextChanged += new System.EventHandler(this.CbAddressTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Host name:";
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(12, 83);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(303, 20);
            this.txtHost.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(266, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Example: www.contoso.com or marketing.contoso.com";
            // 
            // cbSniRequired
            // 
            this.cbSniRequired.AutoSize = true;
            this.cbSniRequired.Location = new System.Drawing.Point(15, 133);
            this.cbSniRequired.Name = "cbSniRequired";
            this.cbSniRequired.Size = new System.Drawing.Size(177, 17);
            this.cbSniRequired.TabIndex = 11;
            this.cbSniRequired.Text = "Require Server Name Indication";
            this.cbSniRequired.UseVisualStyleBackColor = true;
            // 
            // txtCertificates
            // 
            this.txtCertificates.AutoSize = true;
            this.txtCertificates.Location = new System.Drawing.Point(12, 186);
            this.txtCertificates.Name = "txtCertificates";
            this.txtCertificates.Size = new System.Drawing.Size(79, 13);
            this.txtCertificates.TabIndex = 12;
            this.txtCertificates.Text = "SSL certificate:";
            // 
            // cbCertificates
            // 
            this.cbCertificates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCertificates.FormattingEnabled = true;
            this.cbCertificates.Location = new System.Drawing.Point(12, 202);
            this.cbCertificates.Name = "cbCertificates";
            this.cbCertificates.Size = new System.Drawing.Size(303, 21);
            this.cbCertificates.TabIndex = 13;
            this.cbCertificates.SelectedIndexChanged += new System.EventHandler(this.CbCertificatesSelectedIndexChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(321, 200);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(85, 23);
            this.btnSelect.TabIndex = 14;
            this.btnSelect.Text = "Select...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelectClick);
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(412, 200);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(85, 23);
            this.btnView.TabIndex = 15;
            this.btnView.Text = "View...";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.BtnViewClick);
            // 
            // BindingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(524, 281);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.cbCertificates);
            this.Controls.Add(this.txtCertificates);
            this.Controls.Add(this.cbSniRequired);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.cbAddress);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Name = "BindingDialog";
            this.Text = "Add Site Binding";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.BindingDialogHelpButtonClicked);
            this.Load += new System.EventHandler(this.BindingDialogLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnClose;
        private Button btnOK;
        private Label label1;
        private Label label2;
        private Label label3;
        private ComboBox cbType;
        private ComboBox cbAddress;
        private TextBox txtPort;
        private Label label4;
        private TextBox txtHost;
        private Label label5;
        private CheckBox cbSniRequired;
        private Label txtCertificates;
        private ComboBox cbCertificates;
        private Button btnSelect;
        private Button btnView;
    }
}
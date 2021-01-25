namespace JexusManager.Features.Authentication
{
    using System.Windows.Forms;

    partial class FormsEditDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.gbCookie = new System.Windows.Forms.GroupBox();
            this.cbExpire = new System.Windows.Forms.CheckBox();
            this.cbSSL = new System.Windows.Forms.CheckBox();
            this.cbProtectedMode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbCookie.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(287, 396);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(191, 396);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Login URL:";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(12, 34);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(209, 20);
            this.txtURL.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Authentication cookie time-out (in minutes):";
            // 
            // txtTimeout
            // 
            this.txtTimeout.Location = new System.Drawing.Point(12, 86);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(92, 20);
            this.txtTimeout.TabIndex = 5;
            // 
            // gbCookie
            // 
            this.gbCookie.Controls.Add(this.cbExpire);
            this.gbCookie.Controls.Add(this.cbSSL);
            this.gbCookie.Controls.Add(this.cbProtectedMode);
            this.gbCookie.Controls.Add(this.label5);
            this.gbCookie.Controls.Add(this.txtName);
            this.gbCookie.Controls.Add(this.label4);
            this.gbCookie.Controls.Add(this.cbMode);
            this.gbCookie.Controls.Add(this.label3);
            this.gbCookie.Location = new System.Drawing.Point(12, 112);
            this.gbCookie.Name = "gbCookie";
            this.gbCookie.Size = new System.Drawing.Size(370, 264);
            this.gbCookie.TabIndex = 6;
            this.gbCookie.TabStop = false;
            this.gbCookie.Text = "Cookie settings";
            // 
            // cbExpire
            // 
            this.cbExpire.AutoSize = true;
            this.cbExpire.Location = new System.Drawing.Point(19, 224);
            this.cbExpire.Name = "cbExpire";
            this.cbExpire.Size = new System.Drawing.Size(224, 17);
            this.cbExpire.TabIndex = 7;
            this.cbExpire.Text = "Extend cookie expiration on every request";
            this.cbExpire.UseVisualStyleBackColor = true;
            // 
            // cbSSL
            // 
            this.cbSSL.AutoSize = true;
            this.cbSSL.Location = new System.Drawing.Point(19, 190);
            this.cbSSL.Name = "cbSSL";
            this.cbSSL.Size = new System.Drawing.Size(86, 17);
            this.cbSSL.TabIndex = 6;
            this.cbSSL.Text = "Require SSL";
            this.cbSSL.UseVisualStyleBackColor = true;
            // 
            // cbProtectedMode
            // 
            this.cbProtectedMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProtectedMode.FormattingEnabled = true;
            this.cbProtectedMode.Items.AddRange(new object[] {
            "Encryption and validation",
            "None",
            "Encryption",
            "Validation"});
            this.cbProtectedMode.Location = new System.Drawing.Point(19, 153);
            this.cbProtectedMode.Name = "cbProtectedMode";
            this.cbProtectedMode.Size = new System.Drawing.Size(176, 21);
            this.cbProtectedMode.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Protected mode:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(19, 97);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(176, 20);
            this.txtName.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Name:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Do not use cookies",
            "Use cookies",
            "Auto Detect",
            "Use device profile"});
            this.cbMode.Location = new System.Drawing.Point(19, 43);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(176, 21);
            this.cbMode.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mode:";
            // 
            // FormsEditDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(394, 431);
            this.Controls.Add(this.gbCookie);
            this.Controls.Add(this.txtTimeout);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "FormsEditDialog";
            this.Text = "Edit Forms Authentication Settings";
            this.gbCookie.ResumeLayout(false);
            this.gbCookie.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.GroupBox gbCookie;
        private System.Windows.Forms.CheckBox cbExpire;
        private System.Windows.Forms.CheckBox cbSSL;
        private System.Windows.Forms.ComboBox cbProtectedMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbMode;
        private System.Windows.Forms.Label label3;
    }
}

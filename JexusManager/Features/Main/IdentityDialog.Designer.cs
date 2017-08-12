namespace JexusManager.Features.Main
{
    partial class IdentityDialog
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
            this.rbBuiltin = new System.Windows.Forms.RadioButton();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.cbBuiltin = new System.Windows.Forms.ComboBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbBuiltin
            // 
            this.rbBuiltin.AutoSize = true;
            this.rbBuiltin.Location = new System.Drawing.Point(12, 12);
            this.rbBuiltin.Name = "rbBuiltin";
            this.rbBuiltin.Size = new System.Drawing.Size(101, 17);
            this.rbBuiltin.TabIndex = 0;
            this.rbBuiltin.TabStop = true;
            this.rbBuiltin.Text = "Built-in account:";
            this.rbBuiltin.UseVisualStyleBackColor = true;
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(12, 74);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(105, 17);
            this.rbCustom.TabIndex = 2;
            this.rbCustom.TabStop = true;
            this.rbCustom.Text = "Custom account:";
            this.rbCustom.UseVisualStyleBackColor = true;
            // 
            // txtCustom
            // 
            this.txtCustom.Location = new System.Drawing.Point(26, 97);
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.ReadOnly = true;
            this.txtCustom.Size = new System.Drawing.Size(275, 20);
            this.txtCustom.TabIndex = 3;
            // 
            // cbBuiltin
            // 
            this.cbBuiltin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuiltin.FormattingEnabled = true;
            this.cbBuiltin.Items.AddRange(new object[] {
            "LocalService",
            "LocalSystem",
            "NetworkService",
            "ApplicationPoolIdentity"});
            this.cbBuiltin.Location = new System.Drawing.Point(26, 35);
            this.cbBuiltin.Name = "cbBuiltin";
            this.cbBuiltin.Size = new System.Drawing.Size(290, 21);
            this.cbBuiltin.TabIndex = 4;
            // 
            // btnSet
            // 
            this.btnSet.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSet.Location = new System.Drawing.Point(307, 95);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(95, 23);
            this.btnSet.TabIndex = 5;
            this.btnSet.Text = "Set...";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 176);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(211, 176);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // IdentityDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 211);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.cbBuiltin);
            this.Controls.Add(this.txtCustom);
            this.Controls.Add(this.rbCustom);
            this.Controls.Add(this.rbBuiltin);
            this.Name = "IdentityDialog";
            this.Text = "Application Pool Identity";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.IdentityDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbBuiltin;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.TextBox txtCustom;
        private System.Windows.Forms.ComboBox cbBuiltin;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}

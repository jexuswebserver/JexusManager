namespace JexusManager.Features.IsapiCgiRestriction
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal partial class SettingsDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbCgi = new System.Windows.Forms.CheckBox();
            this.cbIsapi = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbCgi
            // 
            this.cbCgi.AutoSize = true;
            this.cbCgi.Location = new System.Drawing.Point(12, 12);
            this.cbCgi.Name = "cbCgi";
            this.cbCgi.Size = new System.Drawing.Size(171, 17);
            this.cbCgi.TabIndex = 1;
            this.cbCgi.Text = "Allow unspecified CGI modules";
            this.cbCgi.UseVisualStyleBackColor = true;
            // 
            // cbIsapi
            // 
            this.cbIsapi.AutoSize = true;
            this.cbIsapi.Location = new System.Drawing.Point(12, 49);
            this.cbIsapi.Name = "cbIsapi";
            this.cbIsapi.Size = new System.Drawing.Size(180, 17);
            this.cbIsapi.TabIndex = 2;
            this.cbIsapi.Text = "Allow unspecified ISAPI modules";
            this.cbIsapi.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(227, 96);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(131, 96);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // SettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 131);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbIsapi);
            this.Controls.Add(this.cbCgi);
            this.Name = "SettingsDialog";
            this.Text = "Edit ISAPI and CGI Restrictions Settings";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.PermissionsDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CheckBox cbCgi;
        private CheckBox cbIsapi;
        private Button btnCancel;
        private Button btnOK;
    }
}

namespace JexusManager.Features.Handlers
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal partial class PermissionsDialog
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
            this.cbRead = new System.Windows.Forms.CheckBox();
            this.cbScript = new System.Windows.Forms.CheckBox();
            this.cbExecute = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Permissions:";
            // 
            // cbRead
            // 
            this.cbRead.AutoSize = true;
            this.cbRead.Location = new System.Drawing.Point(15, 38);
            this.cbRead.Name = "cbRead";
            this.cbRead.Size = new System.Drawing.Size(52, 17);
            this.cbRead.TabIndex = 1;
            this.cbRead.Text = "Read";
            this.cbRead.UseVisualStyleBackColor = true;
            // 
            // cbScript
            // 
            this.cbScript.AutoSize = true;
            this.cbScript.Location = new System.Drawing.Point(15, 61);
            this.cbScript.Name = "cbScript";
            this.cbScript.Size = new System.Drawing.Size(53, 17);
            this.cbScript.TabIndex = 2;
            this.cbScript.Text = "Script";
            this.cbScript.UseVisualStyleBackColor = true;
            // 
            // cbExecute
            // 
            this.cbExecute.AutoSize = true;
            this.cbExecute.Location = new System.Drawing.Point(37, 84);
            this.cbExecute.Name = "cbExecute";
            this.cbExecute.Size = new System.Drawing.Size(65, 17);
            this.cbExecute.TabIndex = 3;
            this.cbExecute.Text = "Execute";
            this.cbExecute.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(192, 131);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(96, 131);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // PermissionsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(299, 166);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbExecute);
            this.Controls.Add(this.cbScript);
            this.Controls.Add(this.cbRead);
            this.Controls.Add(this.label1);
            this.Name = "PermissionsDialog";
            this.Text = "Edit Feature Permissions";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private CheckBox cbRead;
        private CheckBox cbScript;
        private CheckBox cbExecute;
        private Button btnCancel;
        private Button btnOK;
    }
}

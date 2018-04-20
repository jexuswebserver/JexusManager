namespace JexusManager.Features.Caching
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal partial class CachingSettingsDialog
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
            this.cbUser = new System.Windows.Forms.CheckBox();
            this.cbKernel = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.cbLimit = new System.Windows.Forms.CheckBox();
            this.txtLimit = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbUser
            // 
            this.cbUser.AutoSize = true;
            this.cbUser.Location = new System.Drawing.Point(12, 12);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(92, 17);
            this.cbUser.TabIndex = 1;
            this.cbUser.Text = "Enable cache";
            this.cbUser.UseVisualStyleBackColor = true;
            // 
            // cbKernel
            // 
            this.cbKernel.AutoSize = true;
            this.cbKernel.Location = new System.Drawing.Point(12, 49);
            this.cbKernel.Name = "cbKernel";
            this.cbKernel.Size = new System.Drawing.Size(124, 17);
            this.cbKernel.TabIndex = 2;
            this.cbKernel.Text = "Enable kernel cache";
            this.cbKernel.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(227, 191);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(131, 191);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Maximum cached response size (in bytes):";
            // 
            // txtSize
            // 
            this.txtSize.Enabled = false;
            this.txtSize.Location = new System.Drawing.Point(12, 99);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(249, 20);
            this.txtSize.TabIndex = 7;
            // 
            // cbLimit
            // 
            this.cbLimit.AutoSize = true;
            this.cbLimit.Enabled = false;
            this.cbLimit.Location = new System.Drawing.Point(12, 125);
            this.cbLimit.Name = "cbLimit";
            this.cbLimit.Size = new System.Drawing.Size(137, 17);
            this.cbLimit.TabIndex = 8;
            this.cbLimit.Text = "Cache size limit (in MB):";
            this.cbLimit.UseVisualStyleBackColor = true;
            // 
            // txtLimit
            // 
            this.txtLimit.Enabled = false;
            this.txtLimit.Location = new System.Drawing.Point(30, 148);
            this.txtLimit.Name = "txtLimit";
            this.txtLimit.Size = new System.Drawing.Size(245, 20);
            this.txtLimit.TabIndex = 9;
            // 
            // CachingSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 226);
            this.Controls.Add(this.txtLimit);
            this.Controls.Add(this.cbLimit);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cbKernel);
            this.Controls.Add(this.cbUser);
            this.Name = "CachingSettingsDialog";
            this.Text = "Edit Output Cache Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CheckBox cbUser;
        private CheckBox cbKernel;
        private Button btnCancel;
        private Button btnOK;
        private Label label1;
        private TextBox txtSize;
        private CheckBox cbLimit;
        private TextBox txtLimit;
    }
}

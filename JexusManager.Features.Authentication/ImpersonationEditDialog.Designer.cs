namespace JexusManager.Features.Authentication
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class ImpersonationEditDialog
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
            this.rbSpecific = new System.Windows.Forms.RadioButton();
            this.rbPool = new System.Windows.Forms.RadioButton();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Identity to impersonate:";
            // 
            // rbSpecific
            // 
            this.rbSpecific.AutoSize = true;
            this.rbSpecific.Location = new System.Drawing.Point(10, 42);
            this.rbSpecific.Name = "rbSpecific";
            this.rbSpecific.Size = new System.Drawing.Size(89, 17);
            this.rbSpecific.TabIndex = 1;
            this.rbSpecific.TabStop = true;
            this.rbSpecific.Text = "Specific user:";
            this.rbSpecific.UseVisualStyleBackColor = true;
            // 
            // rbPool
            // 
            this.rbPool.AutoSize = true;
            this.rbPool.Location = new System.Drawing.Point(10, 111);
            this.rbPool.Name = "rbPool";
            this.rbPool.Size = new System.Drawing.Size(114, 17);
            this.rbPool.TabIndex = 2;
            this.rbPool.TabStop = true;
            this.rbPool.Text = "Authenticated user";
            this.rbPool.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(29, 74);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(275, 20);
            this.txtName.TabIndex = 3;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(310, 72);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(95, 23);
            this.btnSet.TabIndex = 4;
            this.btnSet.Text = "Set...";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(313, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(217, 146);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // ImpersonationEditDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 181);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.rbPool);
            this.Controls.Add(this.rbSpecific);
            this.Controls.Add(this.label1);
            this.Name = "ImpersonationEditDialog";
            this.Text = "Edit ASP.NET Impersonation Settings";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.AnonymousEditDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private RadioButton rbSpecific;
        private RadioButton rbPool;
        private TextBox txtName;
        private Button btnSet;
        private Button btnCancel;
        private Button btnOK;
    }
}

namespace JexusManager.Features.IpSecurity
{
    using System.ComponentModel;
    using System.Windows.Forms;

    sealed partial class NewRestrictionDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.Label();
            this.rbAddress = new System.Windows.Forms.RadioButton();
            this.rbRange = new System.Windows.Forms.RadioButton();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtRange = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMask = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(327, 336);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(226, 336);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.AutoSize = true;
            this.txtDescription.Location = new System.Drawing.Point(12, 9);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(280, 13);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Text = "Allow access for the following IP address or domain name:";
            // 
            // rbAddress
            // 
            this.rbAddress.AutoSize = true;
            this.rbAddress.Checked = true;
            this.rbAddress.Location = new System.Drawing.Point(15, 34);
            this.rbAddress.Name = "rbAddress";
            this.rbAddress.Size = new System.Drawing.Size(116, 17);
            this.rbAddress.TabIndex = 3;
            this.rbAddress.TabStop = true;
            this.rbAddress.Text = "Specify IP address:";
            this.rbAddress.UseVisualStyleBackColor = true;
            // 
            // rbRange
            // 
            this.rbRange.AutoSize = true;
            this.rbRange.Location = new System.Drawing.Point(15, 92);
            this.rbRange.Name = "rbRange";
            this.rbRange.Size = new System.Drawing.Size(108, 17);
            this.rbRange.TabIndex = 4;
            this.rbRange.Text = "IP address range:";
            this.rbRange.UseVisualStyleBackColor = true;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(33, 57);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(314, 20);
            this.txtAddress.TabIndex = 5;
            // 
            // txtRange
            // 
            this.txtRange.Enabled = false;
            this.txtRange.Location = new System.Drawing.Point(33, 125);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(314, 20);
            this.txtRange.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mask or Prefix:";
            // 
            // txtMask
            // 
            this.txtMask.Enabled = false;
            this.txtMask.Location = new System.Drawing.Point(33, 192);
            this.txtMask.Name = "txtMask";
            this.txtMask.Size = new System.Drawing.Size(314, 20);
            this.txtMask.TabIndex = 8;
            // 
            // NewRestrictionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 371);
            this.Controls.Add(this.txtMask);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRange);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.rbRange);
            this.Controls.Add(this.rbAddress);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewRestrictionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Allow Restriction Rule";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.NewRestrictionDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label txtDescription;
        private RadioButton rbAddress;
        private RadioButton rbRange;
        private TextBox txtAddress;
        private TextBox txtRange;
        private Label label2;
        private TextBox txtMask;
    }
}

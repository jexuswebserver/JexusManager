namespace JexusManager.Dialogs
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class ConnectAsDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rbSpecific = new System.Windows.Forms.RadioButton();
            this.rbPassThrough = new System.Windows.Forms.RadioButton();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path credentials:";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(215, 151);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(311, 151);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // rbSpecific
            // 
            this.rbSpecific.AutoSize = true;
            this.rbSpecific.Location = new System.Drawing.Point(5, 40);
            this.rbSpecific.Name = "rbSpecific";
            this.rbSpecific.Size = new System.Drawing.Size(86, 17);
            this.rbSpecific.TabIndex = 6;
            this.rbSpecific.TabStop = true;
            this.rbSpecific.Text = "Special user:";
            this.rbSpecific.UseVisualStyleBackColor = true;
            // 
            // rbPassThrough
            // 
            this.rbPassThrough.AutoSize = true;
            this.rbPassThrough.Location = new System.Drawing.Point(5, 108);
            this.rbPassThrough.Name = "rbPassThrough";
            this.rbPassThrough.Size = new System.Drawing.Size(240, 17);
            this.rbPassThrough.TabIndex = 7;
            this.rbPassThrough.TabStop = true;
            this.rbPassThrough.Text = "Application user (pass-through authentication)";
            this.rbPassThrough.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(21, 73);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(284, 20);
            this.txtName.TabIndex = 8;
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(311, 71);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(95, 23);
            this.btnSet.TabIndex = 9;
            this.btnSet.Text = "Set...";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // ConnectAsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(418, 186);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.rbPassThrough);
            this.Controls.Add(this.rbSpecific);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label1);
            this.Name = "ConnectAsDialog";
            this.Text = "Connect As";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Button btnOK;
        private Button btnCancel;
        private RadioButton rbSpecific;
        private RadioButton rbPassThrough;
        private TextBox txtName;
        private Button btnSet;
    }
}

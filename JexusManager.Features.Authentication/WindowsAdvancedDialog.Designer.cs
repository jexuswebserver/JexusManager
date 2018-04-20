namespace JexusManager.Features.Authentication
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class WindowsAdvancedDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowsAdvancedDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.cbExtended = new System.Windows.Forms.ComboBox();
            this.cbKernelMode = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Extended Protection:";
            // 
            // cbExtended
            // 
            this.cbExtended.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbExtended.FormattingEnabled = true;
            this.cbExtended.Items.AddRange(new object[] {
            "Off",
            "Accept",
            "Required"});
            this.cbExtended.Location = new System.Drawing.Point(16, 39);
            this.cbExtended.Name = "cbExtended";
            this.cbExtended.Size = new System.Drawing.Size(121, 21);
            this.cbExtended.TabIndex = 1;
            // 
            // cbKernelMode
            // 
            this.cbKernelMode.AutoSize = true;
            this.cbKernelMode.Checked = true;
            this.cbKernelMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKernelMode.Location = new System.Drawing.Point(16, 89);
            this.cbKernelMode.Name = "cbKernelMode";
            this.cbKernelMode.Size = new System.Drawing.Size(191, 17);
            this.cbKernelMode.TabIndex = 2;
            this.cbKernelMode.Text = "Enable Kernel-mode authentication";
            this.cbKernelMode.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 112);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(461, 158);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(286, 301);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(382, 301);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // WindowsAdvancedDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(489, 336);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbKernelMode);
            this.Controls.Add(this.cbExtended);
            this.Controls.Add(this.label1);
            this.Name = "WindowsAdvancedDialog";
            this.Text = "Advanced Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private ComboBox cbExtended;
        private CheckBox cbKernelMode;
        private TextBox textBox1;
        private Button btnOK;
        private Button btnCancel;
    }
}

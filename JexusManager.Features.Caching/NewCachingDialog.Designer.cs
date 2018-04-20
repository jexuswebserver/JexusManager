namespace JexusManager.Features.Caching
{
    using System.ComponentModel;
    using System.Windows.Forms;

    sealed partial class NewCachingDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.Label();
            this.txtExtension = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbUser = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.txtUserTime = new System.Windows.Forms.TextBox();
            this.rbUserNo = new System.Windows.Forms.RadioButton();
            this.rbUserTime = new System.Windows.Forms.RadioButton();
            this.rbUserFile = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtKernelTime = new System.Windows.Forms.TextBox();
            this.rbKernelNo = new System.Windows.Forms.RadioButton();
            this.rbKernelTime = new System.Windows.Forms.RadioButton();
            this.rbKernelFile = new System.Windows.Forms.RadioButton();
            this.cbKernel = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(377, 506);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(276, 506);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.AutoSize = true;
            this.txtDescription.Location = new System.Drawing.Point(9, 48);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(113, 13);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Text = "Example: .aspx or .axd";
            // 
            // txtExtension
            // 
            this.txtExtension.Location = new System.Drawing.Point(12, 25);
            this.txtExtension.Name = "txtExtension";
            this.txtExtension.Size = new System.Drawing.Size(338, 20);
            this.txtExtension.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "File name extension:";
            // 
            // cbUser
            // 
            this.cbUser.AutoSize = true;
            this.cbUser.Location = new System.Drawing.Point(12, 89);
            this.cbUser.Name = "cbUser";
            this.cbUser.Size = new System.Drawing.Size(118, 17);
            this.cbUser.TabIndex = 16;
            this.cbUser.Text = "User-mode caching";
            this.cbUser.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAdvanced);
            this.groupBox1.Controls.Add(this.txtUserTime);
            this.groupBox1.Controls.Add(this.rbUserNo);
            this.groupBox1.Controls.Add(this.rbUserTime);
            this.groupBox1.Controls.Add(this.rbUserFile);
            this.groupBox1.Location = new System.Drawing.Point(15, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 192);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Cache Monitoring";
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(280, 163);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(152, 23);
            this.btnAdvanced.TabIndex = 8;
            this.btnAdvanced.Text = "Advanced...";
            this.btnAdvanced.UseVisualStyleBackColor = true;
            // 
            // txtUserTime
            // 
            this.txtUserTime.Location = new System.Drawing.Point(35, 76);
            this.txtUserTime.Name = "txtUserTime";
            this.txtUserTime.Size = new System.Drawing.Size(190, 20);
            this.txtUserTime.TabIndex = 7;
            // 
            // rbUserNo
            // 
            this.rbUserNo.AutoSize = true;
            this.rbUserNo.Location = new System.Drawing.Point(15, 125);
            this.rbUserNo.Name = "rbUserNo";
            this.rbUserNo.Size = new System.Drawing.Size(116, 17);
            this.rbUserNo.TabIndex = 6;
            this.rbUserNo.TabStop = true;
            this.rbUserNo.Text = "Prevent all caching";
            this.rbUserNo.UseVisualStyleBackColor = true;
            // 
            // rbUserTime
            // 
            this.rbUserTime.AutoSize = true;
            this.rbUserTime.Location = new System.Drawing.Point(15, 53);
            this.rbUserTime.Name = "rbUserTime";
            this.rbUserTime.Size = new System.Drawing.Size(155, 17);
            this.rbUserTime.TabIndex = 5;
            this.rbUserTime.TabStop = true;
            this.rbUserTime.Text = "At time intervals (hh:mm:ss):";
            this.rbUserTime.UseVisualStyleBackColor = true;
            // 
            // rbUserFile
            // 
            this.rbUserFile.AutoSize = true;
            this.rbUserFile.Location = new System.Drawing.Point(15, 19);
            this.rbUserFile.Name = "rbUserFile";
            this.rbUserFile.Size = new System.Drawing.Size(166, 17);
            this.rbUserFile.TabIndex = 4;
            this.rbUserFile.TabStop = true;
            this.rbUserFile.Text = "Using file change notifications";
            this.rbUserFile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtKernelTime);
            this.groupBox2.Controls.Add(this.rbKernelNo);
            this.groupBox2.Controls.Add(this.rbKernelTime);
            this.groupBox2.Controls.Add(this.rbKernelFile);
            this.groupBox2.Location = new System.Drawing.Point(15, 333);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(438, 164);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Cache Monitoring";
            // 
            // txtKernelTime
            // 
            this.txtKernelTime.Location = new System.Drawing.Point(35, 76);
            this.txtKernelTime.Name = "txtKernelTime";
            this.txtKernelTime.Size = new System.Drawing.Size(190, 20);
            this.txtKernelTime.TabIndex = 3;
            // 
            // rbKernelNo
            // 
            this.rbKernelNo.AutoSize = true;
            this.rbKernelNo.Location = new System.Drawing.Point(15, 125);
            this.rbKernelNo.Name = "rbKernelNo";
            this.rbKernelNo.Size = new System.Drawing.Size(116, 17);
            this.rbKernelNo.TabIndex = 2;
            this.rbKernelNo.TabStop = true;
            this.rbKernelNo.Text = "Prevent all caching";
            this.rbKernelNo.UseVisualStyleBackColor = true;
            // 
            // rbKernelTime
            // 
            this.rbKernelTime.AutoSize = true;
            this.rbKernelTime.Location = new System.Drawing.Point(15, 53);
            this.rbKernelTime.Name = "rbKernelTime";
            this.rbKernelTime.Size = new System.Drawing.Size(152, 17);
            this.rbKernelTime.TabIndex = 1;
            this.rbKernelTime.TabStop = true;
            this.rbKernelTime.Text = "At time intervals (hh:mm:ss)";
            this.rbKernelTime.UseVisualStyleBackColor = true;
            // 
            // rbKernelFile
            // 
            this.rbKernelFile.AutoSize = true;
            this.rbKernelFile.Location = new System.Drawing.Point(15, 19);
            this.rbKernelFile.Name = "rbKernelFile";
            this.rbKernelFile.Size = new System.Drawing.Size(166, 17);
            this.rbKernelFile.TabIndex = 0;
            this.rbKernelFile.TabStop = true;
            this.rbKernelFile.Text = "Using file change notifications";
            this.rbKernelFile.UseVisualStyleBackColor = true;
            // 
            // cbKernel
            // 
            this.cbKernel.AutoSize = true;
            this.cbKernel.Location = new System.Drawing.Point(12, 310);
            this.cbKernel.Name = "cbKernel";
            this.cbKernel.Size = new System.Drawing.Size(126, 17);
            this.cbKernel.TabIndex = 18;
            this.cbKernel.Text = "Kernel-mode caching";
            this.cbKernel.UseVisualStyleBackColor = true;
            // 
            // NewCachingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 541);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbKernel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtExtension);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "NewCachingDialog";
            this.Text = "Add Cache Rule";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label txtDescription;
        private TextBox txtExtension;
        private Label label3;
        private CheckBox cbUser;
        private GroupBox groupBox1;
        private Button btnAdvanced;
        private TextBox txtUserTime;
        private RadioButton rbUserNo;
        private RadioButton rbUserTime;
        private RadioButton rbUserFile;
        private GroupBox groupBox2;
        private TextBox txtKernelTime;
        private RadioButton rbKernelNo;
        private RadioButton rbKernelTime;
        private RadioButton rbKernelFile;
        private CheckBox cbKernel;
    }
}

namespace JexusManager.Features.IpSecurity
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class DynamicDialog
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
            this.cbConcurrent = new System.Windows.Forms.CheckBox();
            this.cbInterval = new System.Windows.Forms.CheckBox();
            this.cbLogging = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConcurrent = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNumer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPeriod = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbConcurrent
            // 
            this.cbConcurrent.AutoSize = true;
            this.cbConcurrent.Location = new System.Drawing.Point(31, 22);
            this.cbConcurrent.Name = "cbConcurrent";
            this.cbConcurrent.Size = new System.Drawing.Size(317, 17);
            this.cbConcurrent.TabIndex = 0;
            this.cbConcurrent.Text = "Deny IP Address based on the number of concurrent requests";
            this.cbConcurrent.UseVisualStyleBackColor = true;
            // 
            // cbInterval
            // 
            this.cbInterval.AutoSize = true;
            this.cbInterval.Location = new System.Drawing.Point(31, 141);
            this.cbInterval.Name = "cbInterval";
            this.cbInterval.Size = new System.Drawing.Size(362, 17);
            this.cbInterval.TabIndex = 1;
            this.cbInterval.Text = "Deny IP Address based on the number of requests over a period of time";
            this.cbInterval.UseVisualStyleBackColor = true;
            // 
            // cbLogging
            // 
            this.cbLogging.AutoSize = true;
            this.cbLogging.Location = new System.Drawing.Point(31, 298);
            this.cbLogging.Name = "cbLogging";
            this.cbLogging.Size = new System.Drawing.Size(154, 17);
            this.cbLogging.TabIndex = 2;
            this.cbLogging.Text = "Enable Logging Only Mode";
            this.cbLogging.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Maximum number of concurrent requests:";
            // 
            // txtConcurrent
            // 
            this.txtConcurrent.Enabled = false;
            this.txtConcurrent.Location = new System.Drawing.Point(59, 81);
            this.txtConcurrent.Name = "txtConcurrent";
            this.txtConcurrent.Size = new System.Drawing.Size(140, 20);
            this.txtConcurrent.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Maximum number of requests:";
            // 
            // txtNumer
            // 
            this.txtNumer.Enabled = false;
            this.txtNumer.Location = new System.Drawing.Point(59, 199);
            this.txtNumer.Name = "txtNumer";
            this.txtNumer.Size = new System.Drawing.Size(140, 20);
            this.txtNumer.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Time Period (in milliseconds):";
            // 
            // txtPeriod
            // 
            this.txtPeriod.Enabled = false;
            this.txtPeriod.Location = new System.Drawing.Point(59, 262);
            this.txtPeriod.Name = "txtPeriod";
            this.txtPeriod.Size = new System.Drawing.Size(140, 20);
            this.txtPeriod.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(442, 356);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(346, 356);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // DynamicDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(549, 391);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtPeriod);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNumer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConcurrent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLogging);
            this.Controls.Add(this.cbInterval);
            this.Controls.Add(this.cbConcurrent);
            this.Name = "DynamicDialog";
            this.Text = "Dynamic IP Restriction Settings";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.DynamicDialog_HelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox cbConcurrent;
        private CheckBox cbInterval;
        private CheckBox cbLogging;
        private Label label1;
        private TextBox txtConcurrent;
        private Label label2;
        private TextBox txtNumer;
        private Label label3;
        private TextBox txtPeriod;
        private Button btnCancel;
        private Button btnOK;
    }
}

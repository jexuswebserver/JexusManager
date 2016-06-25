namespace JexusManager.Features.Certificates.Wizards.CertificateRenewWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class OptionsPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RenewDomain = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rbRequestRenewal = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbCompleteRenewal = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // RenewDomain
            // 
            this.RenewDomain.AutoSize = true;
            this.RenewDomain.Checked = true;
            this.RenewDomain.Location = new System.Drawing.Point(27, 42);
            this.RenewDomain.Name = "RenewDomain";
            this.RenewDomain.Size = new System.Drawing.Size(161, 17);
            this.RenewDomain.TabIndex = 0;
            this.RenewDomain.TabStop = true;
            this.RenewDomain.Text = "Renew an existing certificate";
            this.RenewDomain.UseVisualStyleBackColor = true;
            this.RenewDomain.Click += new System.EventHandler(this.Option_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(387, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lets you renew an existing certificate by using a certificate server in your doma" +
    "in.";
            // 
            // rbRequestRenewal
            // 
            this.rbRequestRenewal.AutoSize = true;
            this.rbRequestRenewal.Location = new System.Drawing.Point(27, 99);
            this.rbRequestRenewal.Name = "rbRequestRenewal";
            this.rbRequestRenewal.Size = new System.Drawing.Size(192, 17);
            this.rbRequestRenewal.TabIndex = 2;
            this.rbRequestRenewal.Text = "Create a renewal certificate request";
            this.rbRequestRenewal.UseVisualStyleBackColor = true;
            this.rbRequestRenewal.Click += new System.EventHandler(this.Option_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(421, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Lets you package renewal information to submit to a certification authority at a " +
    "later time.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(466, 26);
            this.label3.TabIndex = 4;
            this.label3.Text = "Lets you complete the certificate renewal request by using the certificate that w" +
    "as received from a\r\ncertification authority.";
            // 
            // rbCompleteRenewal
            // 
            this.rbCompleteRenewal.AutoSize = true;
            this.rbCompleteRenewal.Location = new System.Drawing.Point(27, 156);
            this.rbCompleteRenewal.Name = "rbCompleteRenewal";
            this.rbCompleteRenewal.Size = new System.Drawing.Size(196, 17);
            this.rbCompleteRenewal.TabIndex = 5;
            this.rbCompleteRenewal.Text = "Complete certificate renewal request";
            this.rbCompleteRenewal.UseVisualStyleBackColor = true;
            this.rbCompleteRenewal.Click += new System.EventHandler(this.Option_Click);
            // 
            // OptionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbCompleteRenewal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbRequestRenewal);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RenewDomain);
            this.Name = "OptionsPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton RenewDomain;
        private Label label1;
        private RadioButton rbRequestRenewal;
        private Label label2;
        private Label label3;
        private RadioButton rbCompleteRenewal;
    }
}

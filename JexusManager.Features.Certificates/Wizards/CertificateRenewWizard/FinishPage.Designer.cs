namespace JexusManager.Features.Certificates.Wizards.CertificateRenewWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class FinishPage
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtConnect = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbOpen = new System.Windows.Forms.RadioButton();
            this.rbIis = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Specify a file name for the certificate renewal request:";
            // 
            // txtConnect
            // 
            this.txtConnect.AutoSize = true;
            this.txtConnect.Location = new System.Drawing.Point(20, 25);
            this.txtConnect.Name = "txtConnect";
            this.txtConnect.Size = new System.Drawing.Size(503, 26);
            this.txtConnect.TabIndex = 6;
            this.txtConnect.Text = "Specify the file name for the certificate request. This information can be sent t" +
    "o a certification authority for \r\nsigning.";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(23, 90);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(358, 20);
            this.txtPath.TabIndex = 8;
            this.txtPath.TextChanged += new System.EventHandler(this.txtPath_TextChanged);
            this.txtPath.VisibleChanged += new System.EventHandler(this.txtPath_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(387, 88);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(30, 25);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbOpen);
            this.groupBox1.Controls.Add(this.rbIis);
            this.groupBox1.Location = new System.Drawing.Point(23, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Request file style";
            // 
            // rbOpen
            // 
            this.rbOpen.AutoSize = true;
            this.rbOpen.Location = new System.Drawing.Point(15, 55);
            this.rbOpen.Name = "rbOpen";
            this.rbOpen.Size = new System.Drawing.Size(71, 17);
            this.rbOpen.TabIndex = 12;
            this.rbOpen.Text = "OpenSSL";
            this.rbOpen.UseVisualStyleBackColor = true;
            this.rbOpen.CheckedChanged += new System.EventHandler(this.FileStyle_Changed);
            // 
            // rbIis
            // 
            this.rbIis.AutoSize = true;
            this.rbIis.Checked = true;
            this.rbIis.Location = new System.Drawing.Point(15, 32);
            this.rbIis.Name = "rbIis";
            this.rbIis.Size = new System.Drawing.Size(38, 17);
            this.rbIis.TabIndex = 11;
            this.rbIis.TabStop = true;
            this.rbIis.Text = "IIS";
            this.rbIis.UseVisualStyleBackColor = true;
            this.rbIis.CheckedChanged += new System.EventHandler(this.FileStyle_Changed);
            // 
            // FinishPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConnect);
            this.Name = "FinishPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label label2;
        private Label txtConnect;
        private TextBox txtPath;
        private Button btnBrowse;
        private GroupBox groupBox1;
        private RadioButton rbOpen;
        private RadioButton rbIis;
    }
}

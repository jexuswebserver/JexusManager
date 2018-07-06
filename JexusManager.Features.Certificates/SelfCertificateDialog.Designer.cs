namespace JexusManager.Features.Certificates
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class SelfCertificateDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbStore = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCommonName = new System.Windows.Forms.TextBox();
            this.cbLength = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbHashing = new System.Windows.Forms.ComboBox();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbGenerate = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.txtTitle);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(670, 65);
            this.panel1.TabIndex = 11;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::JexusManager.Features.Certificates.Properties.Resources.certificates_48;
            this.pictureBox1.Location = new System.Drawing.Point(10, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // txtTitle
            // 
            this.txtTitle.AutoSize = true;
            this.txtTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTitle.Location = new System.Drawing.Point(87, 22);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(200, 24);
            this.txtTitle.TabIndex = 2;
            this.txtTitle.Text = "Specify Friendly Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(483, 26);
            this.label1.TabIndex = 12;
            this.label1.Text = "Specify a file name for the certificate request. This information can be sent to " +
    "a certificate authority for\r\nsigning:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Specify a friendly name for the certificate:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(26, 142);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(360, 20);
            this.txtName.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(229, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Select a certificate store for the new certificate:";
            // 
            // cbStore
            // 
            this.cbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStore.FormattingEnabled = true;
            this.cbStore.Items.AddRange(new object[] {
            "Personal",
            "Web Hosting"});
            this.cbStore.Location = new System.Drawing.Point(26, 195);
            this.cbStore.Name = "cbStore";
            this.cbStore.Size = new System.Drawing.Size(238, 21);
            this.cbStore.TabIndex = 16;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(466, 441);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(85, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(557, 441);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 235);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Specify DNS names:";
            // 
            // txtCommonName
            // 
            this.txtCommonName.Location = new System.Drawing.Point(26, 251);
            this.txtCommonName.Name = "txtCommonName";
            this.txtCommonName.Size = new System.Drawing.Size(360, 20);
            this.txtCommonName.TabIndex = 20;
            // 
            // cbLength
            // 
            this.cbLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLength.FormattingEnabled = true;
            this.cbLength.Items.AddRange(new object[] {
            "384",
            "512",
            "1024",
            "2048",
            "4096",
            "8192",
            "16384"});
            this.cbLength.Location = new System.Drawing.Point(26, 331);
            this.cbLength.Name = "cbLength";
            this.cbLength.Size = new System.Drawing.Size(238, 21);
            this.cbLength.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 315);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Bit length:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 372);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Hashing method:";
            // 
            // cbHashing
            // 
            this.cbHashing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHashing.FormattingEnabled = true;
            this.cbHashing.Items.AddRange(new object[] {
            "SHA1",
            "SHA2 (SHA256)"});
            this.cbHashing.Location = new System.Drawing.Point(26, 388);
            this.cbHashing.Name = "cbHashing";
            this.cbHashing.Size = new System.Drawing.Size(238, 21);
            this.cbHashing.TabIndex = 24;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(351, 332);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpFrom.TabIndex = 25;
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(351, 388);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 20);
            this.dtpTo.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(351, 372);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Valid to:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(351, 315);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Valid from:";
            // 
            // cbGenerate
            // 
            this.cbGenerate.AutoSize = true;
            this.cbGenerate.Checked = true;
            this.cbGenerate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGenerate.Location = new System.Drawing.Point(26, 416);
            this.cbGenerate.Name = "cbGenerate";
            this.cbGenerate.Size = new System.Drawing.Size(198, 17);
            this.cbGenerate.TabIndex = 29;
            this.cbGenerate.Text = "Generate Subject Alternative Names";
            this.cbGenerate.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 278);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(188, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Example: consoto.com, *.consoto.com";
            // 
            // SelfCertificateDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(654, 476);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbGenerate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.cbHashing);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbLength);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCommonName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cbStore);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "SelfCertificateDialog";
            this.Text = "Create Self-Signed Certificate";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label txtTitle;
        private Label label1;
        private Label label2;
        private TextBox txtName;
        private Label label3;
        private ComboBox cbStore;
        private Button btnOK;
        private Button btnCancel;
        private Label label4;
        private TextBox txtCommonName;
        private ComboBox cbLength;
        private Label label5;
        private Label label6;
        private ComboBox cbHashing;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Label label7;
        private Label label8;
        private CheckBox cbGenerate;
        private Label label9;
    }
}

namespace JexusManager.Features.Certificates.Wizards.CertificateRequestWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class KeysPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeysPage));
            this.txtConnect = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbProvider = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbLength = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtConnect
            // 
            this.txtConnect.AutoSize = true;
            this.txtConnect.Location = new System.Drawing.Point(20, 25);
            this.txtConnect.Name = "txtConnect";
            this.txtConnect.Size = new System.Drawing.Size(515, 39);
            this.txtConnect.TabIndex = 0;
            this.txtConnect.Text = resources.GetString("txtConnect.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cryptographic service provider:";
            // 
            // cbProvider
            // 
            this.cbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProvider.FormattingEnabled = true;
            this.cbProvider.Items.AddRange(new object[] {
            "Microsoft DH SChannel Cryptographic Provider",
            "Microsoft RSA SChannel Cryptographic Provider"});
            this.cbProvider.Location = new System.Drawing.Point(23, 89);
            this.cbProvider.Name = "cbProvider";
            this.cbProvider.Size = new System.Drawing.Size(392, 21);
            this.cbProvider.TabIndex = 0;
            this.cbProvider.SelectedIndexChanged += new System.EventHandler(this.CbProviderSelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Bit length:";
            // 
            // cbLength
            // 
            this.cbLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLength.FormattingEnabled = true;
            this.cbLength.Location = new System.Drawing.Point(23, 140);
            this.cbLength.Name = "cbLength";
            this.cbLength.Size = new System.Drawing.Size(121, 21);
            this.cbLength.TabIndex = 4;
            this.cbLength.SelectedIndexChanged += new System.EventHandler(this.CbLengthSelectedIndexChanged);
            // 
            // KeysPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbLength);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbProvider);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtConnect);
            this.Name = "KeysPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label txtConnect;
        private Label label2;
        private ComboBox cbProvider;
        private Label label3;
        private ComboBox cbLength;
    }
}

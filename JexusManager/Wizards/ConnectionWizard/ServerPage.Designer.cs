namespace JexusManager.Wizards.ConnectionWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class ServerPage
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
            this.label3 = new System.Windows.Forms.Label();
            this.cbHostName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(21, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Example: localhost, www.site.net, or WESTSRV01:8080\r\n";
            // 
            // cbHostName
            // 
            this.cbHostName.FormattingEnabled = true;
            this.cbHostName.Location = new System.Drawing.Point(24, 36);
            this.cbHostName.Name = "cbHostName";
            this.cbHostName.Size = new System.Drawing.Size(335, 21);
            this.cbHostName.TabIndex = 7;
            this.cbHostName.TextChanged += new System.EventHandler(this.cbHostName_TextChanged);
            // 
            // ServerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbHostName);
            this.Name = "ServerPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label2;
        private Label label3;
        private ComboBox cbHostName;
    }
}

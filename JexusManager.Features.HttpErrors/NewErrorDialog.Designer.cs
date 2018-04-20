namespace JexusManager.Features.HttpErrors
{
    using System.ComponentModel;
    using System.Windows.Forms;

    sealed partial class NewErrorDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtStatusCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRedirect = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtExecute = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbLocalize = new System.Windows.Forms.CheckBox();
            this.txtStatic = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbRedirect = new System.Windows.Forms.RadioButton();
            this.rbExecute = new System.Windows.Forms.RadioButton();
            this.rbStatic = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status code:";
            // 
            // txtStatusCode
            // 
            this.txtStatusCode.Location = new System.Drawing.Point(12, 25);
            this.txtStatusCode.Name = "txtStatusCode";
            this.txtStatusCode.Size = new System.Drawing.Size(120, 20);
            this.txtStatusCode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Example: 404 or 404.2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtRedirect);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtExecute);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cbLocalize);
            this.groupBox1.Controls.Add(this.txtStatic);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.rbRedirect);
            this.groupBox1.Controls.Add(this.rbExecute);
            this.groupBox1.Controls.Add(this.rbStatic);
            this.groupBox1.Location = new System.Drawing.Point(12, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 343);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Response Action";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(376, 75);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(88, 23);
            this.btnSet.TabIndex = 12;
            this.btnSet.Text = "Set...";
            this.btnSet.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 306);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(220, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Example: http://www.contoso.com/404.aspx";
            // 
            // txtRedirect
            // 
            this.txtRedirect.Location = new System.Drawing.Point(36, 283);
            this.txtRedirect.Name = "txtRedirect";
            this.txtRedirect.Size = new System.Drawing.Size(334, 20);
            this.txtRedirect.TabIndex = 10;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 267);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Absolute URL:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Example: /ErrorPages/404.aspx";
            // 
            // txtExecute
            // 
            this.txtExecute.Location = new System.Drawing.Point(36, 184);
            this.txtExecute.Name = "txtExecute";
            this.txtExecute.Size = new System.Drawing.Size(334, 20);
            this.txtExecute.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "URL (relative to site root):";
            // 
            // cbLocalize
            // 
            this.cbLocalize.AutoSize = true;
            this.cbLocalize.Location = new System.Drawing.Point(36, 104);
            this.cbLocalize.Name = "cbLocalize";
            this.cbLocalize.Size = new System.Drawing.Size(245, 17);
            this.cbLocalize.TabIndex = 5;
            this.cbLocalize.Text = "Try to return the error file in the client language";
            this.cbLocalize.UseVisualStyleBackColor = true;
            // 
            // txtStatic
            // 
            this.txtStatic.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtStatic.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtStatic.Location = new System.Drawing.Point(36, 77);
            this.txtStatic.Name = "txtStatic";
            this.txtStatic.Size = new System.Drawing.Size(334, 20);
            this.txtStatic.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "File path:";
            // 
            // rbRedirect
            // 
            this.rbRedirect.AutoSize = true;
            this.rbRedirect.Location = new System.Drawing.Point(15, 235);
            this.rbRedirect.Name = "rbRedirect";
            this.rbRedirect.Size = new System.Drawing.Size(158, 17);
            this.rbRedirect.TabIndex = 2;
            this.rbRedirect.Text = "Respond with a 302 redirect";
            this.rbRedirect.UseVisualStyleBackColor = true;
            // 
            // rbExecute
            // 
            this.rbExecute.AutoSize = true;
            this.rbExecute.Checked = true;
            this.rbExecute.Location = new System.Drawing.Point(15, 137);
            this.rbExecute.Name = "rbExecute";
            this.rbExecute.Size = new System.Drawing.Size(150, 17);
            this.rbExecute.TabIndex = 1;
            this.rbExecute.TabStop = true;
            this.rbExecute.Text = "Execute a URL on the site";
            this.rbExecute.UseVisualStyleBackColor = true;
            // 
            // rbStatic
            // 
            this.rbStatic.AutoSize = true;
            this.rbStatic.Location = new System.Drawing.Point(15, 28);
            this.rbStatic.Name = "rbStatic";
            this.rbStatic.Size = new System.Drawing.Size(265, 17);
            this.rbStatic.TabIndex = 0;
            this.rbStatic.Text = "Insert content from static file into the error response";
            this.rbStatic.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(387, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(287, 451);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // NewErrorDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(494, 486);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtStatusCode);
            this.Controls.Add(this.label1);
            this.Name = "NewErrorDialog";
            this.Text = "Add Custom Error Page";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox txtStatusCode;
        private Label label2;
        private GroupBox groupBox1;
        private Label label6;
        private TextBox txtRedirect;
        private Label label7;
        private Label label5;
        private TextBox txtExecute;
        private Label label4;
        private CheckBox cbLocalize;
        private TextBox txtStatic;
        private Label label3;
        private RadioButton rbRedirect;
        private RadioButton rbExecute;
        private RadioButton rbStatic;
        private Button btnCancel;
        private Button btnOK;
        private Button btnSet;
    }
}

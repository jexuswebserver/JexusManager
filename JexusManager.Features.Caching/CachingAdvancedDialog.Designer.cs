namespace JexusManager.Features.Caching
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal partial class CachingAdvancedDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHeaders = new System.Windows.Forms.TextBox();
            this.txtString = new System.Windows.Forms.TextBox();
            this.cbHeaders = new System.Windows.Forms.CheckBox();
            this.cbString = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(347, 256);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(251, 256);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtHeaders);
            this.groupBox1.Controls.Add(this.txtString);
            this.groupBox1.Controls.Add(this.cbHeaders);
            this.groupBox1.Controls.Add(this.cbString);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 238);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Multiple File Versions";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Example: Accept-Language, Accept-Charset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Example: Locale, Culture";
            // 
            // txtHeaders
            // 
            this.txtHeaders.Enabled = false;
            this.txtHeaders.Location = new System.Drawing.Point(40, 160);
            this.txtHeaders.Name = "txtHeaders";
            this.txtHeaders.Size = new System.Drawing.Size(321, 20);
            this.txtHeaders.TabIndex = 4;
            // 
            // txtString
            // 
            this.txtString.Enabled = false;
            this.txtString.Location = new System.Drawing.Point(40, 79);
            this.txtString.Name = "txtString";
            this.txtString.Size = new System.Drawing.Size(321, 20);
            this.txtString.TabIndex = 3;
            // 
            // cbHeaders
            // 
            this.cbHeaders.AutoSize = true;
            this.cbHeaders.Location = new System.Drawing.Point(21, 137);
            this.cbHeaders.Name = "cbHeaders";
            this.cbHeaders.Size = new System.Drawing.Size(69, 17);
            this.cbHeaders.TabIndex = 2;
            this.cbHeaders.Text = "Headers:";
            this.cbHeaders.UseVisualStyleBackColor = true;
            // 
            // cbString
            // 
            this.cbString.AutoSize = true;
            this.cbString.Location = new System.Drawing.Point(21, 56);
            this.cbString.Name = "cbString";
            this.cbString.Size = new System.Drawing.Size(136, 17);
            this.cbString.TabIndex = 1;
            this.cbString.Text = "Query string variable(s):";
            this.cbString.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cache different versions of a file based on:";
            // 
            // CachingAdvancedDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(454, 291);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "CachingAdvancedDialog";
            this.Text = "Advanced Output Cache Rule Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private GroupBox groupBox1;
        private Label label3;
        private Label label2;
        private TextBox txtHeaders;
        private TextBox txtString;
        private CheckBox cbHeaders;
        private CheckBox cbString;
        private Label label1;
    }
}

namespace JexusManager.Features.Rewrite
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal partial class NewRuleBlockingDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbInput = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbMatch = new System.Windows.Forms.ComboBox();
            this.lblPattern = new System.Windows.Forms.Label();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.lblExample = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbResponse = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(282, 361);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(186, 361);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Block access based on:";
            // 
            // cbInput
            // 
            this.cbInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInput.FormattingEnabled = true;
            this.cbInput.Items.AddRange(new object[] {
            "URL Path",
            "User-agent Header",
            "IP Address",
            "Query String",
            "Referer",
            "Host Header"});
            this.cbInput.Location = new System.Drawing.Point(12, 29);
            this.cbInput.Name = "cbInput";
            this.cbInput.Size = new System.Drawing.Size(365, 21);
            this.cbInput.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Block request that:";
            // 
            // cbMatch
            // 
            this.cbMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMatch.FormattingEnabled = true;
            this.cbMatch.Items.AddRange(new object[] {
            "Matches the Pattern",
            "Does Not Match the Pattern"});
            this.cbMatch.Location = new System.Drawing.Point(12, 88);
            this.cbMatch.Name = "cbMatch";
            this.cbMatch.Size = new System.Drawing.Size(365, 21);
            this.cbMatch.TabIndex = 5;
            // 
            // lblPattern
            // 
            this.lblPattern.AutoSize = true;
            this.lblPattern.Location = new System.Drawing.Point(13, 135);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(100, 13);
            this.lblPattern.TabIndex = 6;
            this.lblPattern.Text = "Pattern (URL Path):";
            // 
            // txtPattern
            // 
            this.txtPattern.Location = new System.Drawing.Point(12, 151);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(365, 20);
            this.txtPattern.TabIndex = 7;
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.Location = new System.Drawing.Point(13, 174);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(94, 13);
            this.lblExample.TabIndex = 8;
            this.lblExample.Text = "Example: IMG*.jpg";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 212);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Using:";
            // 
            // cbMode
            // 
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Regular Expressions",
            "Wildcards"});
            this.cbMode.Location = new System.Drawing.Point(12, 228);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(365, 21);
            this.cbMode.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 271);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "How to block:";
            // 
            // cbResponse
            // 
            this.cbResponse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResponse.FormattingEnabled = true;
            this.cbResponse.Items.AddRange(new object[] {
            "Send an HTTP 401 (Unauthorized) Response",
            "Send an HTTP 403 (Forbidden) Response",
            "Send an HTTP 404 (File Not Found) Response",
            "Abort Request"});
            this.cbResponse.Location = new System.Drawing.Point(12, 287);
            this.cbResponse.Name = "cbResponse";
            this.cbResponse.Size = new System.Drawing.Size(365, 21);
            this.cbResponse.TabIndex = 12;
            // 
            // NewRuleBlockingDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(389, 396);
            this.Controls.Add(this.cbResponse);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblExample);
            this.Controls.Add(this.txtPattern);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.cbMatch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "NewRuleBlockingDialog";
            this.Text = "Add Request Blocking Rule";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.NewRuleBlockingHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label label1;
        private ComboBox cbInput;
        private Label label2;
        private ComboBox cbMatch;
        private Label lblPattern;
        private TextBox txtPattern;
        private Label lblExample;
        private Label label5;
        private ComboBox cbMode;
        private Label label6;
        private ComboBox cbResponse;
    }
}

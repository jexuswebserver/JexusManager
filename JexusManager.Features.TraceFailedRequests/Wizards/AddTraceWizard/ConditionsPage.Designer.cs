namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class ConditionsPage
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
            this.txtConnect = new System.Windows.Forms.Label();
            this.cbCodes = new System.Windows.Forms.CheckBox();
            this.txtCodes = new System.Windows.Forms.TextBox();
            this.cbTime = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.cbEventSeverity = new System.Windows.Forms.CheckBox();
            this.cbSeverity = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtConnect
            // 
            this.txtConnect.AutoSize = true;
            this.txtConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnect.Location = new System.Drawing.Point(20, 25);
            this.txtConnect.Name = "txtConnect";
            this.txtConnect.Size = new System.Drawing.Size(311, 13);
            this.txtConnect.TabIndex = 6;
            this.txtConnect.Text = "Under which condition(s) should a request be traced?";
            // 
            // cbCodes
            // 
            this.cbCodes.AutoSize = true;
            this.cbCodes.Location = new System.Drawing.Point(23, 62);
            this.cbCodes.Name = "cbCodes";
            this.cbCodes.Size = new System.Drawing.Size(97, 17);
            this.cbCodes.TabIndex = 7;
            this.cbCodes.Text = "Status code(s):";
            this.cbCodes.UseVisualStyleBackColor = true;
            // 
            // txtCodes
            // 
            this.txtCodes.Enabled = false;
            this.txtCodes.Location = new System.Drawing.Point(49, 86);
            this.txtCodes.Name = "txtCodes";
            this.txtCodes.Size = new System.Drawing.Size(282, 20);
            this.txtCodes.TabIndex = 8;
            // 
            // cbTime
            // 
            this.cbTime.AutoSize = true;
            this.cbTime.Location = new System.Drawing.Point(23, 143);
            this.cbTime.Name = "cbTime";
            this.cbTime.Size = new System.Drawing.Size(142, 17);
            this.cbTime.TabIndex = 9;
            this.cbTime.Text = "Time taken (in seconds):";
            this.cbTime.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Example: 401.3-999,405";
            // 
            // txtTime
            // 
            this.txtTime.Enabled = false;
            this.txtTime.Location = new System.Drawing.Point(49, 166);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(123, 20);
            this.txtTime.TabIndex = 11;
            // 
            // cbEventSeverity
            // 
            this.cbEventSeverity.AutoSize = true;
            this.cbEventSeverity.Location = new System.Drawing.Point(23, 203);
            this.cbEventSeverity.Name = "cbEventSeverity";
            this.cbEventSeverity.Size = new System.Drawing.Size(98, 17);
            this.cbEventSeverity.TabIndex = 12;
            this.cbEventSeverity.Text = "Event Severity:";
            this.cbEventSeverity.UseVisualStyleBackColor = true;
            // 
            // cbSeverity
            // 
            this.cbSeverity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSeverity.Enabled = false;
            this.cbSeverity.FormattingEnabled = true;
            this.cbSeverity.Items.AddRange(new object[] {
            "Error",
            "Critical Error",
            "Warning"});
            this.cbSeverity.Location = new System.Drawing.Point(49, 226);
            this.cbSeverity.Name = "cbSeverity";
            this.cbSeverity.Size = new System.Drawing.Size(239, 21);
            this.cbSeverity.TabIndex = 13;
            // 
            // ConditionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbSeverity);
            this.Controls.Add(this.cbEventSeverity);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTime);
            this.Controls.Add(this.txtCodes);
            this.Controls.Add(this.cbCodes);
            this.Controls.Add(this.txtConnect);
            this.Name = "ConditionsPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label txtConnect;
        private CheckBox cbCodes;
        private TextBox txtCodes;
        private CheckBox cbTime;
        private Label label1;
        private TextBox txtTime;
        private CheckBox cbEventSeverity;
        private ComboBox cbSeverity;
    }
}

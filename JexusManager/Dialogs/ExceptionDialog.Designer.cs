namespace JexusManager.Dialogs
{
    using System.ComponentModel;

    partial class ExceptionDialog
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
            this.btnReport = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.txtStep = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(265, 253);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(100, 23);
            this.btnReport.TabIndex = 7;
            this.btnReport.Text = "Report to GitHub";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(159, 253);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 23);
            this.btnCopy.TabIndex = 6;
            this.btnCopy.Text = "Copy to Clipboard";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(12, 49);
            this.txtInfo.Multiline = true;
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInfo.Size = new System.Drawing.Size(353, 198);
            this.txtInfo.TabIndex = 5;
            // 
            // txtStep
            // 
            this.txtStep.Location = new System.Drawing.Point(12, 12);
            this.txtStep.Name = "txtStep";
            this.txtStep.Size = new System.Drawing.Size(353, 46);
            this.txtStep.TabIndex = 4;
            this.txtStep.Text = "An unhandled exception is detected. Please copy the information to clipboard and " +
    "then report to GitHub.";
            // 
            // ExceptionDialog
            // 
            this.AcceptButton = this.btnReport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 288);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.txtStep);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unhandled Exception";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Label txtStep;
    }
}

namespace JexusManager.Features.ResponseHeaders
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class SetCommonHeadersDialog
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
            this.cbKeepAlive = new System.Windows.Forms.CheckBox();
            this.cbExpired = new System.Windows.Forms.CheckBox();
            this.rbImmediate = new System.Windows.Forms.RadioButton();
            this.rbAfter = new System.Windows.Forms.RadioButton();
            this.txtAfter = new System.Windows.Forms.TextBox();
            this.cbUnit = new System.Windows.Forms.ComboBox();
            this.rbTime = new System.Windows.Forms.RadioButton();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbKeepAlive
            // 
            this.cbKeepAlive.AutoSize = true;
            this.cbKeepAlive.Location = new System.Drawing.Point(12, 12);
            this.cbKeepAlive.Name = "cbKeepAlive";
            this.cbKeepAlive.Size = new System.Drawing.Size(143, 17);
            this.cbKeepAlive.TabIndex = 0;
            this.cbKeepAlive.Text = "Enable HTTP keep-alive";
            this.cbKeepAlive.UseVisualStyleBackColor = true;
            // 
            // cbExpired
            // 
            this.cbExpired.AutoSize = true;
            this.cbExpired.Location = new System.Drawing.Point(12, 35);
            this.cbExpired.Name = "cbExpired";
            this.cbExpired.Size = new System.Drawing.Size(123, 17);
            this.cbExpired.TabIndex = 1;
            this.cbExpired.Text = "Expire Web content:";
            this.cbExpired.UseVisualStyleBackColor = true;
            this.cbExpired.CheckedChanged += new System.EventHandler(this.cbExpired_CheckedChanged);
            // 
            // rbImmediate
            // 
            this.rbImmediate.AutoSize = true;
            this.rbImmediate.Enabled = false;
            this.rbImmediate.Location = new System.Drawing.Point(33, 58);
            this.rbImmediate.Name = "rbImmediate";
            this.rbImmediate.Size = new System.Drawing.Size(80, 17);
            this.rbImmediate.TabIndex = 2;
            this.rbImmediate.TabStop = true;
            this.rbImmediate.Text = "Immediately";
            this.rbImmediate.UseVisualStyleBackColor = true;
            // 
            // rbAfter
            // 
            this.rbAfter.AutoSize = true;
            this.rbAfter.Enabled = false;
            this.rbAfter.Location = new System.Drawing.Point(33, 81);
            this.rbAfter.Name = "rbAfter";
            this.rbAfter.Size = new System.Drawing.Size(47, 17);
            this.rbAfter.TabIndex = 3;
            this.rbAfter.TabStop = true;
            this.rbAfter.Text = "After";
            this.rbAfter.UseVisualStyleBackColor = true;
            this.rbAfter.CheckedChanged += new System.EventHandler(this.rbAfter_CheckedChanged);
            // 
            // txtAfter
            // 
            this.txtAfter.Enabled = false;
            this.txtAfter.Location = new System.Drawing.Point(53, 104);
            this.txtAfter.Name = "txtAfter";
            this.txtAfter.Size = new System.Drawing.Size(60, 20);
            this.txtAfter.TabIndex = 4;
            // 
            // cbUnit
            // 
            this.cbUnit.Enabled = false;
            this.cbUnit.FormattingEnabled = true;
            this.cbUnit.Items.AddRange(new object[] {
            "Second(s)",
            "Minute(s)",
            "Hour(s)",
            "Day(s)"});
            this.cbUnit.Location = new System.Drawing.Point(119, 104);
            this.cbUnit.Name = "cbUnit";
            this.cbUnit.Size = new System.Drawing.Size(121, 21);
            this.cbUnit.TabIndex = 5;
            // 
            // rbTime
            // 
            this.rbTime.AutoSize = true;
            this.rbTime.Enabled = false;
            this.rbTime.Location = new System.Drawing.Point(33, 130);
            this.rbTime.Name = "rbTime";
            this.rbTime.Size = new System.Drawing.Size(209, 17);
            this.rbTime.TabIndex = 6;
            this.rbTime.TabStop = true;
            this.rbTime.Text = "On (Coordinated Universal Time (UTC))";
            this.rbTime.UseVisualStyleBackColor = true;
            this.rbTime.CheckedChanged += new System.EventHandler(this.rbTime_CheckedChanged);
            // 
            // dtpDate
            // 
            this.dtpDate.Enabled = false;
            this.dtpDate.Location = new System.Drawing.Point(53, 166);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 20);
            this.dtpDate.TabIndex = 7;
            // 
            // dtpTime
            // 
            this.dtpTime.Enabled = false;
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTime.Location = new System.Drawing.Point(259, 166);
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.Size = new System.Drawing.Size(101, 20);
            this.dtpTime.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(352, 221);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(256, 221);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SetCommonHeadersDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(459, 256);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.rbTime);
            this.Controls.Add(this.cbUnit);
            this.Controls.Add(this.txtAfter);
            this.Controls.Add(this.rbAfter);
            this.Controls.Add(this.rbImmediate);
            this.Controls.Add(this.cbExpired);
            this.Controls.Add(this.cbKeepAlive);
            this.Name = "SetCommonHeadersDialog";
            this.Text = "Set Common HTTP Response Headers";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.SetCommonHeadersDialog_HelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox cbKeepAlive;
        private CheckBox cbExpired;
        private RadioButton rbImmediate;
        private RadioButton rbAfter;
        private TextBox txtAfter;
        private ComboBox cbUnit;
        private RadioButton rbTime;
        private DateTimePicker dtpDate;
        private DateTimePicker dtpTime;
        private Button btnCancel;
        private Button btnOK;
    }
}
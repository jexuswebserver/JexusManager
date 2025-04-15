namespace JexusManager.Notifications.Demo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise.</param>
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
            this.btnInfo = new System.Windows.Forms.Button();
            this.btnSuccess = new System.Windows.Forms.Button();
            this.btnWarning = new System.Windows.Forms.Button();
            this.btnError = new System.Windows.Forms.Button();
            this.grpCustom = new System.Windows.Forms.GroupBox();
            this.btnCustomMessage = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.numFadeDuration = new System.Windows.Forms.NumericUpDown();
            this.lblFadeDuration = new System.Windows.Forms.Label();
            this.numDuration = new System.Windows.Forms.NumericUpDown();
            this.lblDuration = new System.Windows.Forms.Label();
            this.grpType = new System.Windows.Forms.GroupBox();
            this.rbError = new System.Windows.Forms.RadioButton();
            this.rbWarning = new System.Windows.Forms.RadioButton();
            this.rbSuccess = new System.Windows.Forms.RadioButton();
            this.rbInfo = new System.Windows.Forms.RadioButton();
            this.grpPosition = new System.Windows.Forms.GroupBox();
            this.rbTopLeft = new System.Windows.Forms.RadioButton();
            this.rbTopRight = new System.Windows.Forms.RadioButton();
            this.rbBottomLeft = new System.Windows.Forms.RadioButton();
            this.rbBottomRight = new System.Windows.Forms.RadioButton();
            this.rbBottomCenter = new System.Windows.Forms.RadioButton();
            this.btnChangePosition = new System.Windows.Forms.Button();
            this.lblCurrentPosition = new System.Windows.Forms.Label();
            this.btnDismissAll = new System.Windows.Forms.Button();
            this.chkShowClickMessage = new System.Windows.Forms.CheckBox();
            this.grpQuickNotifications = new System.Windows.Forms.GroupBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.grpCustom.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).BeginInit();
            this.grpType.SuspendLayout();
            this.grpPosition.SuspendLayout();
            this.grpQuickNotifications.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(17, 30);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(120, 30);
            this.btnInfo.TabIndex = 0;
            this.btnInfo.Text = "Info Notification";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnSuccess
            // 
            this.btnSuccess.Location = new System.Drawing.Point(143, 30);
            this.btnSuccess.Name = "btnSuccess";
            this.btnSuccess.Size = new System.Drawing.Size(120, 30);
            this.btnSuccess.TabIndex = 1;
            this.btnSuccess.Text = "Success Notification";
            this.btnSuccess.UseVisualStyleBackColor = true;
            this.btnSuccess.Click += new System.EventHandler(this.btnSuccess_Click);
            // 
            // btnWarning
            // 
            this.btnWarning.Location = new System.Drawing.Point(17, 66);
            this.btnWarning.Name = "btnWarning";
            this.btnWarning.Size = new System.Drawing.Size(120, 30);
            this.btnWarning.TabIndex = 2;
            this.btnWarning.Text = "Warning Notification";
            this.btnWarning.UseVisualStyleBackColor = true;
            this.btnWarning.Click += new System.EventHandler(this.btnWarning_Click);
            // 
            // btnError
            // 
            this.btnError.Location = new System.Drawing.Point(143, 66);
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(120, 30);
            this.btnError.TabIndex = 3;
            this.btnError.Text = "Error Notification";
            this.btnError.UseVisualStyleBackColor = true;
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // grpCustom
            // 
            this.grpCustom.Controls.Add(this.btnCustomMessage);
            this.grpCustom.Controls.Add(this.txtMessage);
            this.grpCustom.Controls.Add(this.lblMessage);
            this.grpCustom.Controls.Add(this.txtTitle);
            this.grpCustom.Controls.Add(this.lblTitle);
            this.grpCustom.Location = new System.Drawing.Point(12, 214);
            this.grpCustom.Name = "grpCustom";
            this.grpCustom.Size = new System.Drawing.Size(552, 146);
            this.grpCustom.TabIndex = 4;
            this.grpCustom.TabStop = false;
            this.grpCustom.Text = "Custom Notification";
            // 
            // btnCustomMessage
            // 
            this.btnCustomMessage.Location = new System.Drawing.Point(417, 102);
            this.btnCustomMessage.Name = "btnCustomMessage";
            this.btnCustomMessage.Size = new System.Drawing.Size(120, 30);
            this.btnCustomMessage.TabIndex = 4;
            this.btnCustomMessage.Text = "Show Notification";
            this.btnCustomMessage.UseVisualStyleBackColor = true;
            this.btnCustomMessage.Click += new System.EventHandler(this.btnCustomMessage_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(86, 61);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(451, 35);
            this.txtMessage.TabIndex = 3;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(14, 64);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(66, 17);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "Message:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(86, 28);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(451, 22);
            this.txtTitle.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(14, 31);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(39, 17);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title:";
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.numFadeDuration);
            this.grpSettings.Controls.Add(this.lblFadeDuration);
            this.grpSettings.Controls.Add(this.numDuration);
            this.grpSettings.Controls.Add(this.lblDuration);
            this.grpSettings.Location = new System.Drawing.Point(12, 366);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(268, 100);
            this.grpSettings.TabIndex = 5;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Duration Settings";
            // 
            // numFadeDuration
            // 
            this.numFadeDuration.Location = new System.Drawing.Point(121, 62);
            this.numFadeDuration.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numFadeDuration.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numFadeDuration.Name = "numFadeDuration";
            this.numFadeDuration.Size = new System.Drawing.Size(120, 22);
            this.numFadeDuration.TabIndex = 3;
            this.numFadeDuration.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // lblFadeDuration
            // 
            this.lblFadeDuration.AutoSize = true;
            this.lblFadeDuration.Location = new System.Drawing.Point(14, 64);
            this.lblFadeDuration.Name = "lblFadeDuration";
            this.lblFadeDuration.Size = new System.Drawing.Size(101, 17);
            this.lblFadeDuration.TabIndex = 2;
            this.lblFadeDuration.Text = "Fade Duration:";
            // 
            // numDuration
            // 
            this.numDuration.Location = new System.Drawing.Point(121, 29);
            this.numDuration.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDuration.Name = "numDuration";
            this.numDuration.Size = new System.Drawing.Size(120, 22);
            this.numDuration.TabIndex = 1;
            this.numDuration.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(14, 31);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(101, 17);
            this.lblDuration.TabIndex = 0;
            this.lblDuration.Text = "Duration (sec):";
            // 
            // grpType
            // 
            this.grpType.Controls.Add(this.rbError);
            this.grpType.Controls.Add(this.rbWarning);
            this.grpType.Controls.Add(this.rbSuccess);
            this.grpType.Controls.Add(this.rbInfo);
            this.grpType.Location = new System.Drawing.Point(286, 366);
            this.grpType.Name = "grpType";
            this.grpType.Size = new System.Drawing.Size(278, 100);
            this.grpType.TabIndex = 6;
            this.grpType.TabStop = false;
            this.grpType.Text = "Notification Type";
            // 
            // rbError
            // 
            this.rbError.AutoSize = true;
            this.rbError.Location = new System.Drawing.Point(143, 62);
            this.rbError.Name = "rbError";
            this.rbError.Size = new System.Drawing.Size(58, 21);
            this.rbError.TabIndex = 3;
            this.rbError.Text = "Error";
            this.rbError.UseVisualStyleBackColor = true;
            // 
            // rbWarning
            // 
            this.rbWarning.AutoSize = true;
            this.rbWarning.Location = new System.Drawing.Point(17, 62);
            this.rbWarning.Name = "rbWarning";
            this.rbWarning.Size = new System.Drawing.Size(83, 21);
            this.rbWarning.TabIndex = 2;
            this.rbWarning.Text = "Warning";
            this.rbWarning.UseVisualStyleBackColor = true;
            // 
            // rbSuccess
            // 
            this.rbSuccess.AutoSize = true;
            this.rbSuccess.Location = new System.Drawing.Point(143, 29);
            this.rbSuccess.Name = "rbSuccess";
            this.rbSuccess.Size = new System.Drawing.Size(82, 21);
            this.rbSuccess.TabIndex = 1;
            this.rbSuccess.Text = "Success";
            this.rbSuccess.UseVisualStyleBackColor = true;
            // 
            // rbInfo
            // 
            this.rbInfo.AutoSize = true;
            this.rbInfo.Checked = true;
            this.rbInfo.Location = new System.Drawing.Point(17, 29);
            this.rbInfo.Name = "rbInfo";
            this.rbInfo.Size = new System.Drawing.Size(51, 21);
            this.rbInfo.TabIndex = 0;
            this.rbInfo.TabStop = true;
            this.rbInfo.Text = "Info";
            this.rbInfo.UseVisualStyleBackColor = true;
            // 
            // grpPosition
            // 
            this.grpPosition.Controls.Add(this.rbTopLeft);
            this.grpPosition.Controls.Add(this.rbTopRight);
            this.grpPosition.Controls.Add(this.rbBottomLeft);
            this.grpPosition.Controls.Add(this.rbBottomRight);
            this.grpPosition.Controls.Add(this.rbBottomCenter);
            this.grpPosition.Controls.Add(this.btnChangePosition);
            this.grpPosition.Location = new System.Drawing.Point(12, 472);
            this.grpPosition.Name = "grpPosition";
            this.grpPosition.Size = new System.Drawing.Size(552, 122);
            this.grpPosition.TabIndex = 7;
            this.grpPosition.TabStop = false;
            this.grpPosition.Text = "Notification Position";
            // 
            // rbTopLeft
            // 
            this.rbTopLeft.AutoSize = true;
            this.rbTopLeft.Location = new System.Drawing.Point(143, 62);
            this.rbTopLeft.Name = "rbTopLeft";
            this.rbTopLeft.Size = new System.Drawing.Size(82, 21);
            this.rbTopLeft.TabIndex = 7;
            this.rbTopLeft.Text = "Top Left";
            this.rbTopLeft.UseVisualStyleBackColor = true;
            // 
            // rbTopRight
            // 
            this.rbTopRight.AutoSize = true;
            this.rbTopRight.Location = new System.Drawing.Point(17, 62);
            this.rbTopRight.Name = "rbTopRight";
            this.rbTopRight.Size = new System.Drawing.Size(91, 21);
            this.rbTopRight.TabIndex = 6;
            this.rbTopRight.Text = "Top Right";
            this.rbTopRight.UseVisualStyleBackColor = true;
            // 
            // rbBottomLeft
            // 
            this.rbBottomLeft.AutoSize = true;
            this.rbBottomLeft.Location = new System.Drawing.Point(143, 29);
            this.rbBottomLeft.Name = "rbBottomLeft";
            this.rbBottomLeft.Size = new System.Drawing.Size(102, 21);
            this.rbBottomLeft.TabIndex = 5;
            this.rbBottomLeft.Text = "Bottom Left";
            this.rbBottomLeft.UseVisualStyleBackColor = true;
            // 
            // rbBottomRight
            // 
            this.rbBottomRight.AutoSize = true;
            this.rbBottomRight.Checked = true;
            this.rbBottomRight.Location = new System.Drawing.Point(17, 29);
            this.rbBottomRight.Name = "rbBottomRight";
            this.rbBottomRight.Size = new System.Drawing.Size(111, 21);
            this.rbBottomRight.TabIndex = 4;
            this.rbBottomRight.TabStop = true;
            this.rbBottomRight.Text = "Bottom Right";
            this.rbBottomRight.UseVisualStyleBackColor = true;
            // 
            // rbBottomCenter
            // 
            this.rbBottomCenter.AutoSize = true;
            this.rbBottomCenter.Location = new System.Drawing.Point(17, 95);
            this.rbBottomCenter.Name = "rbBottomCenter";
            this.rbBottomCenter.Size = new System.Drawing.Size(122, 21);
            this.rbBottomCenter.TabIndex = 8;
            this.rbBottomCenter.Text = "Bottom Center";
            this.rbBottomCenter.UseVisualStyleBackColor = true;
            // 
            // btnChangePosition
            // 
            this.btnChangePosition.Location = new System.Drawing.Point(295, 29);
            this.btnChangePosition.Name = "btnChangePosition";
            this.btnChangePosition.Size = new System.Drawing.Size(242, 30);
            this.btnChangePosition.TabIndex = 5;
            this.btnChangePosition.Text = "Apply Position Change";
            this.btnChangePosition.UseVisualStyleBackColor = true;
            this.btnChangePosition.Click += new System.EventHandler(this.btnChangePosition_Click);
            // 
            // lblCurrentPosition
            // 
            this.lblCurrentPosition.AutoSize = true;
            this.lblCurrentPosition.Location = new System.Drawing.Point(307, 511);
            this.lblCurrentPosition.Name = "lblCurrentPosition";
            this.lblCurrentPosition.Size = new System.Drawing.Size(176, 17);
            this.lblCurrentPosition.TabIndex = 6;
            this.lblCurrentPosition.Text = "Current Position: BottomRight";
            // 
            // btnDismissAll
            // 
            this.btnDismissAll.Location = new System.Drawing.Point(295, 62);
            this.btnDismissAll.Name = "btnDismissAll";
            this.btnDismissAll.Size = new System.Drawing.Size(242, 30);
            this.btnDismissAll.TabIndex = 6;
            this.btnDismissAll.Text = "Dismiss All Notifications";
            this.btnDismissAll.UseVisualStyleBackColor = true;
            this.btnDismissAll.Click += new System.EventHandler(this.btnDismissAll_Click);
            // 
            // chkShowClickMessage
            // 
            this.chkShowClickMessage.AutoSize = true;
            this.chkShowClickMessage.Location = new System.Drawing.Point(12, 578);
            this.chkShowClickMessage.Name = "chkShowClickMessage";
            this.chkShowClickMessage.Size = new System.Drawing.Size(235, 21);
            this.chkShowClickMessage.TabIndex = 8;
            this.chkShowClickMessage.Text = "Show message when notification clicked";
            this.chkShowClickMessage.UseVisualStyleBackColor = true;
            // 
            // grpQuickNotifications
            // 
            this.grpQuickNotifications.Controls.Add(this.btnInfo);
            this.grpQuickNotifications.Controls.Add(this.btnSuccess);
            this.grpQuickNotifications.Controls.Add(this.btnDismissAll);
            this.grpQuickNotifications.Controls.Add(this.btnWarning);
            this.grpQuickNotifications.Controls.Add(this.btnError);
            this.grpQuickNotifications.Location = new System.Drawing.Point(12, 95);
            this.grpQuickNotifications.Name = "grpQuickNotifications";
            this.grpQuickNotifications.Size = new System.Drawing.Size(552, 113);
            this.grpQuickNotifications.TabIndex = 9;
            this.grpQuickNotifications.TabStop = false;
            this.grpQuickNotifications.Text = "Quick Notifications";
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.Location = new System.Drawing.Point(12, 9);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(552, 83);
            this.lblDescription.TabIndex = 10;
            this.lblDescription.Text = "This demo shows how to use the JexusManager.Notifications control to display notif" +
    "ications similar to Microsoft Teams. Use the options below to try different notif" +
    "ication types and settings.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 611);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.grpQuickNotifications);
            this.Controls.Add(this.chkShowClickMessage);
            this.Controls.Add(this.lblCurrentPosition);
            this.Controls.Add(this.grpPosition);
            this.Controls.Add(this.grpType);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpCustom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Notification Control Demo";
            this.grpCustom.ResumeLayout(false);
            this.grpCustom.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFadeDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDuration)).EndInit();
            this.grpType.ResumeLayout(false);
            this.grpType.PerformLayout();
            this.grpPosition.ResumeLayout(false);
            this.grpPosition.PerformLayout();
            this.grpQuickNotifications.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Button btnSuccess;
        private System.Windows.Forms.Button btnWarning;
        private System.Windows.Forms.Button btnError;
        private System.Windows.Forms.GroupBox grpCustom;
        private System.Windows.Forms.Button btnCustomMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.NumericUpDown numFadeDuration;
        private System.Windows.Forms.Label lblFadeDuration;
        private System.Windows.Forms.NumericUpDown numDuration;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.GroupBox grpType;
        private System.Windows.Forms.RadioButton rbError;
        private System.Windows.Forms.RadioButton rbWarning;
        private System.Windows.Forms.RadioButton rbSuccess;
        private System.Windows.Forms.RadioButton rbInfo;
        private System.Windows.Forms.GroupBox grpPosition;
        private System.Windows.Forms.RadioButton rbTopLeft;
        private System.Windows.Forms.RadioButton rbTopRight;
        private System.Windows.Forms.RadioButton rbBottomLeft;
        private System.Windows.Forms.RadioButton rbBottomRight;
        private System.Windows.Forms.RadioButton rbBottomCenter;
        private System.Windows.Forms.Button btnChangePosition;
        private System.Windows.Forms.Label lblCurrentPosition;
        private System.Windows.Forms.Button btnDismissAll;
        private System.Windows.Forms.CheckBox chkShowClickMessage;
        private System.Windows.Forms.GroupBox grpQuickNotifications;
        private System.Windows.Forms.Label lblDescription;
    }
}
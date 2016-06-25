namespace JexusManager.Features.IpSecurity
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class SetRestrictionsDialog
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
            this.cbAccess = new System.Windows.Forms.ComboBox();
            this.cbDomain = new System.Windows.Forms.CheckBox();
            this.cbProxy = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbAction = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(242, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(146, 201);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Access for unspecified clients:";
            // 
            // cbAccess
            // 
            this.cbAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccess.FormattingEnabled = true;
            this.cbAccess.Items.AddRange(new object[] {
            "Allow",
            "Deny"});
            this.cbAccess.Location = new System.Drawing.Point(12, 25);
            this.cbAccess.Name = "cbAccess";
            this.cbAccess.Size = new System.Drawing.Size(224, 21);
            this.cbAccess.TabIndex = 3;
            // 
            // cbDomain
            // 
            this.cbDomain.AutoSize = true;
            this.cbDomain.Location = new System.Drawing.Point(12, 64);
            this.cbDomain.Name = "cbDomain";
            this.cbDomain.Size = new System.Drawing.Size(149, 17);
            this.cbDomain.TabIndex = 4;
            this.cbDomain.Text = "Enable domain restrictions";
            this.cbDomain.UseVisualStyleBackColor = true;
            // 
            // cbProxy
            // 
            this.cbProxy.AutoSize = true;
            this.cbProxy.Location = new System.Drawing.Point(12, 96);
            this.cbProxy.Name = "cbProxy";
            this.cbProxy.Size = new System.Drawing.Size(118, 17);
            this.cbProxy.TabIndex = 5;
            this.cbProxy.Text = "Enable Proxy Mode";
            this.cbProxy.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Deny Action Type:";
            // 
            // cbAction
            // 
            this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAction.FormattingEnabled = true;
            this.cbAction.Items.AddRange(new object[] {
            "Unauthorized",
            "Forbidden",
            "Not Found",
            "Abort"});
            this.cbAction.Location = new System.Drawing.Point(12, 147);
            this.cbAction.Name = "cbAction";
            this.cbAction.Size = new System.Drawing.Size(178, 21);
            this.cbAction.TabIndex = 7;
            // 
            // SetRestrictionsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(349, 236);
            this.Controls.Add(this.cbAction);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbProxy);
            this.Controls.Add(this.cbDomain);
            this.Controls.Add(this.cbAccess);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "SetRestrictionsDialog";
            this.Text = "Edit IP and Domain Restrictions Settings";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.SetRestrictionsDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label label1;
        private ComboBox cbAccess;
        private CheckBox cbDomain;
        private CheckBox cbProxy;
        private Label label2;
        private ComboBox cbAction;
    }
}

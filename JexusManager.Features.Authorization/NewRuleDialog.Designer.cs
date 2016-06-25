namespace JexusManager.Features.Authorization
{
    using System.ComponentModel;
    using System.Windows.Forms;

    sealed partial class NewRuleDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.Label();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbRoles = new System.Windows.Forms.RadioButton();
            this.txtRoles = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsers = new System.Windows.Forms.TextBox();
            this.rbAnonymous = new System.Windows.Forms.RadioButton();
            this.rbUsers = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cbVerbs = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVerbs = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(342, 386);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(241, 386);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // txtDescription
            // 
            this.txtDescription.AutoSize = true;
            this.txtDescription.Location = new System.Drawing.Point(12, 9);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(180, 13);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Text = "Allow access to this Web content to:";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(15, 34);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(64, 17);
            this.rbAll.TabIndex = 3;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All users";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbRoles
            // 
            this.rbRoles.AutoSize = true;
            this.rbRoles.Location = new System.Drawing.Point(15, 102);
            this.rbRoles.Name = "rbRoles";
            this.rbRoles.Size = new System.Drawing.Size(167, 17);
            this.rbRoles.TabIndex = 4;
            this.rbRoles.Text = "Specified roles or user groups:";
            this.rbRoles.UseVisualStyleBackColor = true;
            // 
            // txtRoles
            // 
            this.txtRoles.Enabled = false;
            this.txtRoles.Location = new System.Drawing.Point(33, 125);
            this.txtRoles.Name = "txtRoles";
            this.txtRoles.Size = new System.Drawing.Size(404, 20);
            this.txtRoles.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Example: Administrators";
            // 
            // txtUsers
            // 
            this.txtUsers.Enabled = false;
            this.txtUsers.Location = new System.Drawing.Point(33, 196);
            this.txtUsers.Name = "txtUsers";
            this.txtUsers.Size = new System.Drawing.Size(404, 20);
            this.txtUsers.TabIndex = 8;
            // 
            // rbAnonymous
            // 
            this.rbAnonymous.AutoSize = true;
            this.rbAnonymous.Location = new System.Drawing.Point(15, 69);
            this.rbAnonymous.Name = "rbAnonymous";
            this.rbAnonymous.Size = new System.Drawing.Size(121, 17);
            this.rbAnonymous.TabIndex = 9;
            this.rbAnonymous.TabStop = true;
            this.rbAnonymous.Text = "All anonymous users";
            this.rbAnonymous.UseVisualStyleBackColor = true;
            // 
            // rbUsers
            // 
            this.rbUsers.AutoSize = true;
            this.rbUsers.Location = new System.Drawing.Point(15, 173);
            this.rbUsers.Name = "rbUsers";
            this.rbUsers.Size = new System.Drawing.Size(100, 17);
            this.rbUsers.TabIndex = 10;
            this.rbUsers.TabStop = true;
            this.rbUsers.Text = "Specified users:";
            this.rbUsers.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 219);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Example: User1, User2";
            // 
            // cbVerbs
            // 
            this.cbVerbs.AutoSize = true;
            this.cbVerbs.Location = new System.Drawing.Point(15, 253);
            this.cbVerbs.Name = "cbVerbs";
            this.cbVerbs.Size = new System.Drawing.Size(174, 17);
            this.cbVerbs.TabIndex = 12;
            this.cbVerbs.Text = "Apply this rule to specific verbs:";
            this.cbVerbs.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 299);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Example: GET, POST";
            // 
            // txtVerbs
            // 
            this.txtVerbs.Enabled = false;
            this.txtVerbs.Location = new System.Drawing.Point(33, 276);
            this.txtVerbs.Name = "txtVerbs";
            this.txtVerbs.Size = new System.Drawing.Size(404, 20);
            this.txtVerbs.TabIndex = 14;
            // 
            // NewRuleDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(449, 421);
            this.Controls.Add(this.txtVerbs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbVerbs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbUsers);
            this.Controls.Add(this.rbAnonymous);
            this.Controls.Add(this.txtUsers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRoles);
            this.Controls.Add(this.rbRoles);
            this.Controls.Add(this.rbAll);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "NewRuleDialog";
            this.Text = "Add Allow Restriction Rule";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.NewRestrictionDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label txtDescription;
        private RadioButton rbAll;
        private RadioButton rbRoles;
        private TextBox txtRoles;
        private Label label2;
        private TextBox txtUsers;
        private RadioButton rbAnonymous;
        private RadioButton rbUsers;
        private Label label1;
        private CheckBox cbVerbs;
        private Label label3;
        private TextBox txtVerbs;
    }
}

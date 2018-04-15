namespace JexusManager.Features.Rewrite.Inbound
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal partial class AddConditionDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCheck = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.cbIgnore = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(352, 241);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(256, 241);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Condition input:";
            // 
            // txtInput
            // 
            this.txtInput.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtInput.Location = new System.Drawing.Point(12, 25);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(435, 20);
            this.txtInput.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Check if input string:";
            // 
            // cbCheck
            // 
            this.cbCheck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCheck.FormattingEnabled = true;
            this.cbCheck.Items.AddRange(new object[] {
            "Is a File",
            "Is Not a File",
            "Is a Directory",
            "Is Not a Directory",
            "Matches the Pattern",
            "Does Not Match the Pattern"});
            this.cbCheck.Location = new System.Drawing.Point(12, 83);
            this.cbCheck.Name = "cbCheck";
            this.cbCheck.Size = new System.Drawing.Size(243, 21);
            this.cbCheck.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Pattern:";
            // 
            // txtPattern
            // 
            this.txtPattern.Location = new System.Drawing.Point(12, 146);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(305, 20);
            this.txtPattern.TabIndex = 7;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(323, 144);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(125, 23);
            this.btnTest.TabIndex = 8;
            this.btnTest.Text = "Test pattern...";
            this.btnTest.UseVisualStyleBackColor = true;
            // 
            // cbIgnore
            // 
            this.cbIgnore.AutoSize = true;
            this.cbIgnore.Location = new System.Drawing.Point(12, 184);
            this.cbIgnore.Name = "cbIgnore";
            this.cbIgnore.Size = new System.Drawing.Size(82, 17);
            this.cbIgnore.TabIndex = 9;
            this.cbIgnore.Text = "Ignore case";
            this.cbIgnore.UseVisualStyleBackColor = true;
            // 
            // AddConditionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(459, 276);
            this.Controls.Add(this.cbIgnore);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtPattern);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "AddConditionDialog";
            this.Text = "Add Condition";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.AddConditionDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label label1;
        private TextBox txtInput;
        private Label label2;
        private ComboBox cbCheck;
        private Label label3;
        private TextBox txtPattern;
        private Button btnTest;
        private CheckBox cbIgnore;
    }
}

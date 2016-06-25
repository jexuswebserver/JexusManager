namespace JexusManager.Features.Rewrite.Outbound
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class AddPreConditionDialog
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbUsing = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbGrouping = new System.Windows.Forms.ComboBox();
            this.lvConditions = new System.Windows.Forms.ListView();
            this.chInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPattern = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 34);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(228, 20);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Using:";
            // 
            // cbUsing
            // 
            this.cbUsing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUsing.FormattingEnabled = true;
            this.cbUsing.Items.AddRange(new object[] {
            "Regular Expressions",
            "Wildcards"});
            this.cbUsing.Location = new System.Drawing.Point(12, 97);
            this.cbUsing.Name = "cbUsing";
            this.cbUsing.Size = new System.Drawing.Size(228, 21);
            this.cbUsing.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Logical grouping:";
            // 
            // cbGrouping
            // 
            this.cbGrouping.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGrouping.FormattingEnabled = true;
            this.cbGrouping.Items.AddRange(new object[] {
            "Matches All",
            "Matches Any"});
            this.cbGrouping.Location = new System.Drawing.Point(12, 162);
            this.cbGrouping.Name = "cbGrouping";
            this.cbGrouping.Size = new System.Drawing.Size(134, 21);
            this.cbGrouping.TabIndex = 5;
            // 
            // lvConditions
            // 
            this.lvConditions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chInput,
            this.chType,
            this.chPattern});
            this.lvConditions.FullRowSelect = true;
            this.lvConditions.HideSelection = false;
            this.lvConditions.Location = new System.Drawing.Point(12, 189);
            this.lvConditions.MultiSelect = false;
            this.lvConditions.Name = "lvConditions";
            this.lvConditions.Size = new System.Drawing.Size(539, 204);
            this.lvConditions.TabIndex = 6;
            this.lvConditions.UseCompatibleStateImageBehavior = false;
            this.lvConditions.View = System.Windows.Forms.View.Details;
            // 
            // chInput
            // 
            this.chInput.Text = "Input";
            this.chInput.Width = 105;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 160;
            // 
            // chPattern
            // 
            this.chPattern.Text = "Pattern";
            this.chPattern.Width = 180;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(557, 194);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(110, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add...";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(557, 223);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(110, 23);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "Edit...";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(557, 252);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(110, 23);
            this.btnRemove.TabIndex = 9;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Location = new System.Drawing.Point(557, 332);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(110, 23);
            this.btnMoveUp.TabIndex = 10;
            this.btnMoveUp.Text = "Move Up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Location = new System.Drawing.Point(557, 361);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(110, 23);
            this.btnMoveDown.TabIndex = 11;
            this.btnMoveDown.Text = "Move Down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(572, 431);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(476, 431);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // AddPreconditionDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(679, 466);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnMoveDown);
            this.Controls.Add(this.btnMoveUp);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lvConditions);
            this.Controls.Add(this.cbGrouping);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbUsing);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Name = "AddPreconditionDialog";
            this.Text = "Add Precondition";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.AddPreconditionDialogHelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox txtName;
        private Label label2;
        private ComboBox cbUsing;
        private Label label3;
        private ComboBox cbGrouping;
        private ListView lvConditions;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnRemove;
        private Button btnMoveUp;
        private Button btnMoveDown;
        private Button btnCancel;
        private Button btnOK;
        private ColumnHeader chInput;
        private ColumnHeader chType;
        private ColumnHeader chPattern;
    }
}

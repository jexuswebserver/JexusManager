namespace JexusManager.Features.Logging
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class FieldsDialog
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
            this.lvStandard = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.lvCustom = new System.Windows.Forms.ListView();
            this.chField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvStandard
            // 
            this.lvStandard.CheckBoxes = true;
            this.lvStandard.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lvStandard.FullRowSelect = true;
            this.lvStandard.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvStandard.Location = new System.Drawing.Point(12, 36);
            this.lvStandard.Name = "lvStandard";
            this.lvStandard.Size = new System.Drawing.Size(545, 235);
            this.lvStandard.TabIndex = 0;
            this.lvStandard.UseCompatibleStateImageBehavior = false;
            this.lvStandard.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 350;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Standard Fields:";
            // 
            // lvCustom
            // 
            this.lvCustom.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chField,
            this.chType,
            this.chSource});
            this.lvCustom.FullRowSelect = true;
            this.lvCustom.Location = new System.Drawing.Point(12, 315);
            this.lvCustom.Name = "lvCustom";
            this.lvCustom.Size = new System.Drawing.Size(545, 200);
            this.lvCustom.TabIndex = 2;
            this.lvCustom.UseCompatibleStateImageBehavior = false;
            this.lvCustom.View = System.Windows.Forms.View.Details;
            // 
            // chField
            // 
            this.chField.Text = "Log Field";
            this.chField.Width = 175;
            // 
            // chType
            // 
            this.chType.Text = "Source Type";
            this.chType.Width = 175;
            // 
            // chSource
            // 
            this.chSource.Text = "Source";
            this.chSource.Width = 175;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 299);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Custom Fields:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 521);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add Field...";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(118, 521);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(105, 23);
            this.btnRemove.TabIndex = 5;
            this.btnRemove.Text = "Remove Field";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(366, 567);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(462, 567);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(467, 521);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(90, 23);
            this.btnEdit.TabIndex = 8;
            this.btnEdit.Text = "Edit Field...";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // FieldsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(569, 602);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvCustom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvStandard);
            this.Name = "FieldsDialog";
            this.Text = "W3C Logging Fields";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FieldsDialog_HelpButtonClicked);
            this.Shown += new System.EventHandler(this.FieldsDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView lvStandard;
        private Label label1;
        private ListView lvCustom;
        private ColumnHeader chField;
        private ColumnHeader chType;
        private ColumnHeader chSource;
        private Label label2;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnOK;
        private Button btnCancel;
        private Button btnEdit;
        private ColumnHeader chName;
    }
}

namespace JexusManager.Features.RequestFiltering
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal sealed partial class NewRuleDialog
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbUrl = new System.Windows.Forms.CheckBox();
            this.cbQuery = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvHeaders = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvExtensions = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvStrings = new System.Windows.Forms.DataGridView();
            this.chString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chExtension = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExtensions)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrings)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(15, 25);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(320, 20);
            this.txtName.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(251, 591);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(352, 591);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbUrl
            // 
            this.cbUrl.AutoSize = true;
            this.cbUrl.Location = new System.Drawing.Point(15, 62);
            this.cbUrl.Name = "cbUrl";
            this.cbUrl.Size = new System.Drawing.Size(65, 17);
            this.cbUrl.TabIndex = 8;
            this.cbUrl.Text = "Scan url";
            this.cbUrl.UseVisualStyleBackColor = true;
            // 
            // cbQuery
            // 
            this.cbQuery.AutoSize = true;
            this.cbQuery.Location = new System.Drawing.Point(15, 97);
            this.cbQuery.Name = "cbQuery";
            this.cbQuery.Size = new System.Drawing.Size(108, 17);
            this.cbQuery.TabIndex = 9;
            this.cbQuery.Text = "Scan query string";
            this.cbQuery.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvHeaders);
            this.groupBox1.Location = new System.Drawing.Point(15, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(432, 130);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Headers";
            // 
            // dgvHeaders
            // 
            this.dgvHeaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHeaders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chHeader});
            this.dgvHeaders.Location = new System.Drawing.Point(6, 19);
            this.dgvHeaders.Name = "dgvHeaders";
            this.dgvHeaders.Size = new System.Drawing.Size(420, 105);
            this.dgvHeaders.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvExtensions);
            this.groupBox2.Location = new System.Drawing.Point(15, 266);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(432, 130);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Applied To";
            // 
            // dgvExtensions
            // 
            this.dgvExtensions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExtensions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chExtension});
            this.dgvExtensions.Location = new System.Drawing.Point(6, 19);
            this.dgvExtensions.Name = "dgvExtensions";
            this.dgvExtensions.Size = new System.Drawing.Size(420, 105);
            this.dgvExtensions.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvStrings);
            this.groupBox3.Location = new System.Drawing.Point(15, 418);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(432, 130);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Deny Strings";
            // 
            // dgvStrings
            // 
            this.dgvStrings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStrings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chString});
            this.dgvStrings.Location = new System.Drawing.Point(6, 19);
            this.dgvStrings.Name = "dgvStrings";
            this.dgvStrings.Size = new System.Drawing.Size(420, 105);
            this.dgvStrings.TabIndex = 0;
            // 
            // chString
            // 
            this.chString.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chString.HeaderText = "String";
            this.chString.Name = "chString";
            // 
            // chExtension
            // 
            this.chExtension.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chExtension.HeaderText = "File Extension";
            this.chExtension.Name = "chExtension";
            // 
            // chHeader
            // 
            this.chHeader.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.chHeader.HeaderText = "Header";
            this.chHeader.Name = "chHeader";
            // 
            // NewRuleDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(459, 626);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbQuery);
            this.Controls.Add(this.cbUrl);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "NewRuleDialog";
            this.Text = "Add Filtering Rule";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.NewHiddenSegmentDialogHelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHeaders)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExtensions)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblName;
        private TextBox txtName;
        private Button btnOK;
        private Button btnCancel;
        private CheckBox cbUrl;
        private CheckBox cbQuery;
        private GroupBox groupBox1;
        private DataGridView dgvHeaders;
        private DataGridViewTextBoxColumn chHeader;
        private GroupBox groupBox2;
        private DataGridView dgvExtensions;
        private DataGridViewTextBoxColumn chExtension;
        private GroupBox groupBox3;
        private DataGridView dgvStrings;
        private DataGridViewTextBoxColumn chString;
    }
}

namespace JexusManager.Wizards.ConnectionWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class BrowsePage
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
            this.txtType = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtType
            // 
            this.txtType.AutoSize = true;
            this.txtType.Location = new System.Drawing.Point(34, 75);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(117, 13);
            this.txtType.TabIndex = 1;
            this.txtType.Text = "Configuration file name:";
            // 
            // txtName
            // 
            this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtName.Location = new System.Drawing.Point(37, 91);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(305, 20);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.TxtNameTextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(348, 89);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(32, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowseClick);
            // 
            // BrowsePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtType);
            this.Name = "BrowsePage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label txtType;
        private TextBox txtName;
        private Button btnBrowse;
    }
}

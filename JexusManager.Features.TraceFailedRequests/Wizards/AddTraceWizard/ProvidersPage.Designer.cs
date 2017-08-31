namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class ProvidersPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.clbProviders = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbAreas = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbVerbosity = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Providers:";
            // 
            // clbProviders
            // 
            this.clbProviders.FormattingEnabled = true;
            this.clbProviders.Location = new System.Drawing.Point(19, 32);
            this.clbProviders.Name = "clbProviders";
            this.clbProviders.Size = new System.Drawing.Size(156, 289);
            this.clbProviders.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Verbosity:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbAreas);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbVerbosity);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(198, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 289);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Provider Properties";
            // 
            // clbAreas
            // 
            this.clbAreas.FormattingEnabled = true;
            this.clbAreas.Location = new System.Drawing.Point(26, 96);
            this.clbAreas.Name = "clbAreas";
            this.clbAreas.Size = new System.Drawing.Size(222, 184);
            this.clbAreas.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Areas:";
            // 
            // cbVerbosity
            // 
            this.cbVerbosity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVerbosity.FormattingEnabled = true;
            this.cbVerbosity.Items.AddRange(new object[] {
            "General ",
            "Critical Errors",
            "Errors",
            "Warning",
            "Information",
            "Verbose"});
            this.cbVerbosity.Location = new System.Drawing.Point(26, 45);
            this.cbVerbosity.Name = "cbVerbosity";
            this.cbVerbosity.Size = new System.Drawing.Size(222, 21);
            this.cbVerbosity.TabIndex = 3;
            // 
            // ProvidersPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.clbProviders);
            this.Controls.Add(this.label1);
            this.Name = "ProvidersPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private CheckedListBox clbProviders;
        private Label label2;
        private GroupBox groupBox1;
        private CheckedListBox clbAreas;
        private Label label3;
        private ComboBox cbVerbosity;
    }
}

namespace JexusManager.Features.RequestFiltering
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class SegmentSettingsDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbDouble = new System.Windows.Forms.CheckBox();
            this.cbHigh = new System.Windows.Forms.CheckBox();
            this.cbVerb = new System.Windows.Forms.CheckBox();
            this.cbExtension = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbDouble);
            this.groupBox1.Controls.Add(this.cbHigh);
            this.groupBox1.Controls.Add(this.cbVerb);
            this.groupBox1.Controls.Add(this.cbExtension);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 164);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // cbDouble
            // 
            this.cbDouble.AutoSize = true;
            this.cbDouble.Location = new System.Drawing.Point(6, 125);
            this.cbDouble.Name = "cbDouble";
            this.cbDouble.Size = new System.Drawing.Size(132, 17);
            this.cbDouble.TabIndex = 3;
            this.cbDouble.Text = "Allow double escaping";
            this.cbDouble.UseVisualStyleBackColor = true;
            // 
            // cbHigh
            // 
            this.cbHigh.AutoSize = true;
            this.cbHigh.Location = new System.Drawing.Point(6, 90);
            this.cbHigh.Name = "cbHigh";
            this.cbHigh.Size = new System.Drawing.Size(141, 17);
            this.cbHigh.TabIndex = 2;
            this.cbHigh.Text = "Allow high-bit characters";
            this.cbHigh.UseVisualStyleBackColor = true;
            // 
            // cbVerb
            // 
            this.cbVerb.AutoSize = true;
            this.cbVerb.Location = new System.Drawing.Point(6, 53);
            this.cbVerb.Name = "cbVerb";
            this.cbVerb.Size = new System.Drawing.Size(119, 17);
            this.cbVerb.TabIndex = 1;
            this.cbVerb.Text = "Allow unlisted verbs";
            this.cbVerb.UseVisualStyleBackColor = true;
            // 
            // cbExtension
            // 
            this.cbExtension.AutoSize = true;
            this.cbExtension.Location = new System.Drawing.Point(6, 19);
            this.cbExtension.Name = "cbExtension";
            this.cbExtension.Size = new System.Drawing.Size(188, 17);
            this.cbExtension.TabIndex = 0;
            this.cbExtension.Text = "Allow unlisted file name extensions";
            this.cbExtension.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtQuery);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtURL);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtContent);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 182);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 184);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Request Limits";
            // 
            // txtQuery
            // 
            this.txtQuery.Location = new System.Drawing.Point(6, 146);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(228, 20);
            this.txtQuery.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Maximum query string (Bytes):";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(6, 91);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(228, 20);
            this.txtURL.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Maximum URL length (Bytes):";
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(6, 43);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(228, 20);
            this.txtContent.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Maximum allowed content length (Bytes):";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(272, 401);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(176, 401);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // SegmentSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(379, 436);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SegmentSettingsDialog";
            this.Text = "Edit Request Filtering Settings";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.SegmentSettingsDialog_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private CheckBox cbDouble;
        private CheckBox cbHigh;
        private CheckBox cbVerb;
        private CheckBox cbExtension;
        private GroupBox groupBox2;
        private TextBox txtQuery;
        private Label label3;
        private TextBox txtURL;
        private Label label2;
        private TextBox txtContent;
        private Label label1;
        private Button btnCancel;
        private Button btnOK;
    }
}

namespace JexusManager.Features.TraceFailedRequests.Wizards.AddTraceWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class ContentPage
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
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbAspNet = new System.Windows.Forms.RadioButton();
            this.rbAsp = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(27, 42);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(88, 17);
            this.rbAll.TabIndex = 0;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "All content (*)";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbAspNet
            // 
            this.rbAspNet.AutoSize = true;
            this.rbAspNet.Location = new System.Drawing.Point(27, 84);
            this.rbAspNet.Name = "rbAspNet";
            this.rbAspNet.Size = new System.Drawing.Size(109, 17);
            this.rbAspNet.TabIndex = 2;
            this.rbAspNet.Text = "ASP.NET (*.aspx)";
            this.rbAspNet.UseVisualStyleBackColor = true;
            // 
            // rbAsp
            // 
            this.rbAsp.AutoSize = true;
            this.rbAsp.Location = new System.Drawing.Point(27, 127);
            this.rbAsp.Name = "rbAsp";
            this.rbAsp.Size = new System.Drawing.Size(79, 17);
            this.rbAsp.TabIndex = 5;
            this.rbAsp.Text = "ASP (*.asp)";
            this.rbAsp.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(177, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "What would you like to trace?";
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(27, 173);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(63, 17);
            this.rbCustom.TabIndex = 7;
            this.rbCustom.Text = "Custom:";
            this.rbCustom.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Example: tr*.aspx";
            // 
            // txtContent
            // 
            this.txtContent.Enabled = false;
            this.txtContent.Location = new System.Drawing.Point(45, 196);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(195, 20);
            this.txtContent.TabIndex = 9;
            // 
            // ContentPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbCustom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rbAsp);
            this.Controls.Add(this.rbAspNet);
            this.Controls.Add(this.rbAll);
            this.Name = "ContentPage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton rbAll;
        private RadioButton rbAspNet;
        private RadioButton rbAsp;
        private Label label4;
        private RadioButton rbCustom;
        private Label label1;
        private TextBox txtContent;
    }
}

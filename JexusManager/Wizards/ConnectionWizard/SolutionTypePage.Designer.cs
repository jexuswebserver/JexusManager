namespace JexusManager.Wizards.ConnectionWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class SolutionTypePage
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.rbVisualStudio = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.rbRider = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(21, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Configuration source:";
            // 
            // rbVisualStudio
            // 
            this.rbVisualStudio.AutoSize = true;
            this.rbVisualStudio.Location = new System.Drawing.Point(24, 51);
            this.rbVisualStudio.Name = "rbVisualStudio";
            this.rbVisualStudio.Size = new System.Drawing.Size(158, 17);
            this.rbVisualStudio.TabIndex = 7;
            this.rbVisualStudio.Text = "From Visual Studio .vs folder";
            this.rbVisualStudio.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(374, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "IIS Express configuration file used by Microsoft Visual Studio 2015 and above.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(241, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "IIS Express configuration used by JetBrains Rider.";
            // 
            // rbRider
            // 
            this.rbRider.AutoSize = true;
            this.rbRider.Location = new System.Drawing.Point(24, 123);
            this.rbRider.Name = "rbRider";
            this.rbRider.Size = new System.Drawing.Size(131, 17);
            this.rbRider.TabIndex = 12;
            this.rbRider.Text = "From Rider .idea folder";
            this.rbRider.UseVisualStyleBackColor = true;
            // 
            // SolutionTypePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbRider);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbVisualStudio);
            this.Controls.Add(this.label2);
            this.Name = "SolutionTypePage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label2;
        private RadioButton rbVisualStudio;
        private Label label3;
        private ToolTip toolTip1;
        private Label label5;
        private RadioButton rbRider;
    }
}

namespace JexusManager.Wizards.ConnectionWizard
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class TypePage
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
            this.rbJexus = new System.Windows.Forms.RadioButton();
            this.rbIisExpress = new System.Windows.Forms.RadioButton();
            this.rbIis = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.rbVisualStudio = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(21, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Server type:";
            // 
            // rbJexus
            // 
            this.rbJexus.AutoSize = true;
            this.rbJexus.Location = new System.Drawing.Point(24, 54);
            this.rbJexus.Name = "rbJexus";
            this.rbJexus.Size = new System.Drawing.Size(86, 17);
            this.rbJexus.TabIndex = 6;
            this.rbJexus.Text = "Jexus Server";
            this.rbJexus.UseVisualStyleBackColor = true;
            this.rbJexus.CheckedChanged += new System.EventHandler(this.RbJexusCheckedChanged);
            // 
            // rbIisExpress
            // 
            this.rbIisExpress.AutoSize = true;
            this.rbIisExpress.Location = new System.Drawing.Point(24, 126);
            this.rbIisExpress.Name = "rbIisExpress";
            this.rbIisExpress.Size = new System.Drawing.Size(162, 17);
            this.rbIisExpress.TabIndex = 7;
            this.rbIisExpress.Text = "IIS Express Configuration File";
            this.rbIisExpress.UseVisualStyleBackColor = true;
            this.rbIisExpress.CheckedChanged += new System.EventHandler(this.RbJexusCheckedChanged);
            // 
            // rbIis
            // 
            this.rbIis.AutoSize = true;
            this.rbIis.Enabled = false;
            this.rbIis.Location = new System.Drawing.Point(24, 270);
            this.rbIis.Name = "rbIis";
            this.rbIis.Size = new System.Drawing.Size(78, 17);
            this.rbIis.TabIndex = 8;
            this.rbIis.Text = "Remote IIS";
            this.rbIis.UseVisualStyleBackColor = true;
            this.rbIis.CheckedChanged += new System.EventHandler(this.RbJexusCheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(385, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Jexus web server is a Linux based web server that supports ASP.NET and PHP.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(338, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "IIS Express configuration files can be added as individual web servers.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 290);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Remote IIS web servers (unavailable)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(280, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Open a solution file to locate IIS Express configuration file.";
            // 
            // rbVisualStudio
            // 
            this.rbVisualStudio.AutoSize = true;
            this.rbVisualStudio.Location = new System.Drawing.Point(24, 198);
            this.rbVisualStudio.Name = "rbVisualStudio";
            this.rbVisualStudio.Size = new System.Drawing.Size(226, 17);
            this.rbVisualStudio.TabIndex = 12;
            this.rbVisualStudio.Text = "Visual Studio IIS Express Configuration File";
            this.rbVisualStudio.UseVisualStyleBackColor = true;
            this.rbVisualStudio.CheckedChanged += new System.EventHandler(this.RbJexusCheckedChanged);
            // 
            // TypePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbVisualStudio);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbIis);
            this.Controls.Add(this.rbIisExpress);
            this.Controls.Add(this.rbJexus);
            this.Controls.Add(this.label2);
            this.Name = "TypePage";
            this.Size = new System.Drawing.Size(670, 380);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label2;
        private RadioButton rbJexus;
        private RadioButton rbIisExpress;
        private RadioButton rbIis;
        private Label label1;
        private Label label3;
        private Label label4;
        private ToolTip toolTip1;
        private Label label5;
        private RadioButton rbVisualStudio;
    }
}

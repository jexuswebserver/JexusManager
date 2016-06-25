namespace JexusManager.Features.Main
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class HomePage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLeXtudio = new System.Windows.Forms.LinkLabel();
            this.txtHomepageChinese = new System.Windows.Forms.LinkLabel();
            this.txtHomepage = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 60);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "by LeXtudio";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Jexus Manager";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(739, 424);
            this.panel2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(739, 5);
            this.panel3.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.txtLeXtudio);
            this.groupBox1.Controls.Add(this.txtHomepageChinese);
            this.groupBox1.Controls.Add(this.txtHomepage);
            this.groupBox1.Location = new System.Drawing.Point(19, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 374);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Online Resources";
            // 
            // txtLeXtudio
            // 
            this.txtLeXtudio.AutoSize = true;
            this.txtLeXtudio.Location = new System.Drawing.Point(21, 74);
            this.txtLeXtudio.Name = "txtLeXtudio";
            this.txtLeXtudio.Size = new System.Drawing.Size(104, 13);
            this.txtLeXtudio.TabIndex = 3;
            this.txtLeXtudio.TabStop = true;
            this.txtLeXtudio.Text = "LeXtudio Homepage";
            this.txtLeXtudio.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.txtLeXtudio_LinkClicked);
            // 
            // txtHomepageChinese
            // 
            this.txtHomepageChinese.AutoSize = true;
            this.txtHomepageChinese.Location = new System.Drawing.Point(21, 50);
            this.txtHomepageChinese.Name = "txtHomepageChinese";
            this.txtHomepageChinese.Size = new System.Drawing.Size(136, 13);
            this.txtHomepageChinese.TabIndex = 2;
            this.txtHomepageChinese.TabStop = true;
            this.txtHomepageChinese.Text = "Jexus Homepage (Chinese)";
            this.txtHomepageChinese.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.txtHomepageChinese_LinkClicked);
            // 
            // txtHomepage
            // 
            this.txtHomepage.AutoSize = true;
            this.txtHomepage.Location = new System.Drawing.Point(21, 27);
            this.txtHomepage.Name = "txtHomepage";
            this.txtHomepage.Size = new System.Drawing.Size(89, 13);
            this.txtHomepage.TabIndex = 1;
            this.txtHomepage.TabStop = true;
            this.txtHomepage.Text = "Jexus Homepage";
            this.txtHomepage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.txtHomepage_LinkClicked);
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "HomePage";
            this.Size = new System.Drawing.Size(739, 484);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Label label1;
        private Panel panel2;
        private GroupBox groupBox1;
        private LinkLabel txtLeXtudio;
        private LinkLabel txtHomepageChinese;
        private LinkLabel txtHomepage;
        private Panel panel3;
        private Label label3;
    }
}

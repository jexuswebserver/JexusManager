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
            panel1 = new Panel();
            label2 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            tableLayoutPanel = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            btnSponsor = new Button();
            txtStudio = new LinkLabel();
            txtHome = new LinkLabel();
            groupBox2 = new GroupBox();
            lblManualUpdate = new LinkLabel();
            btnRetry = new Button();
            lblCurrentVersion = new Label();
            lblUpdateStatus = new Label();
            btnDownloadUpdate = new Button();
            label3 = new Label();
            panel3 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(862, 69);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(244, 33);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(70, 15);
            label2.TabIndex = 0;
            label2.Text = "by LeXtudio";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            label1.Location = new System.Drawing.Point(16, 20);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(189, 29);
            label1.TabIndex = 0;
            label1.Text = "Jexus Manager";
            // 
            // panel2
            // 
            panel2.BackColor = System.Drawing.Color.White;
            panel2.Controls.Add(tableLayoutPanel);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(panel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new System.Drawing.Point(0, 69);
            panel2.Margin = new Padding(4, 3, 4, 3);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(862, 489);
            panel2.TabIndex = 1;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel.Location = new System.Drawing.Point(20, 32);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.Padding = new Padding(12);
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel.Size = new System.Drawing.Size(820, 442);
            tableLayoutPanel.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSponsor);
            groupBox1.Controls.Add(txtStudio);
            groupBox1.Controls.Add(txtHome);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(26, 26);
            groupBox1.Margin = new Padding(14);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new System.Drawing.Size(370, 390);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Online Resources";
            // 
            // btnSponsor
            // 
            btnSponsor.Image = JexusManager.Main.Properties.Resources.iis_16;
            btnSponsor.Location = new System.Drawing.Point(24, 120);
            btnSponsor.Name = "btnSponsor";
            btnSponsor.Size = new System.Drawing.Size(200, 60);
            btnSponsor.TabIndex = 3;
            btnSponsor.Text = "Support This Project";
            btnSponsor.TextImageRelation = TextImageRelation.TextAboveImage;
            btnSponsor.UseVisualStyleBackColor = true;
            btnSponsor.Click += btnSponsor_Click;
            // 
            // txtStudio
            // 
            txtStudio.AutoSize = true;
            txtStudio.Location = new System.Drawing.Point(24, 76);
            txtStudio.Margin = new Padding(4, 0, 4, 0);
            txtStudio.Name = "txtStudio";
            txtStudio.Size = new System.Drawing.Size(116, 15);
            txtStudio.TabIndex = 4;
            txtStudio.TabStop = true;
            txtStudio.Text = "LeXtudio Homepage";
            txtStudio.LinkClicked += txtStudio_LinkClicked;
            // 
            // txtHome
            // 
            txtHome.AutoSize = true;
            txtHome.Location = new System.Drawing.Point(24, 33);
            txtHome.Margin = new Padding(4, 0, 4, 0);
            txtHome.Name = "txtHome";
            txtHome.Size = new System.Drawing.Size(146, 15);
            txtHome.TabIndex = 3;
            txtHome.TabStop = true;
            txtHome.Text = "Jexus Manager Homepage";
            txtHome.LinkClicked += txtHome_LinkClicked;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lblManualUpdate);
            groupBox2.Controls.Add(btnRetry);
            groupBox2.Controls.Add(lblCurrentVersion);
            groupBox2.Controls.Add(lblUpdateStatus);
            groupBox2.Controls.Add(btnDownloadUpdate);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new System.Drawing.Point(424, 26);
            groupBox2.Margin = new Padding(14);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new System.Drawing.Size(370, 390);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Updates";
            // 
            // lblManualUpdate
            // 
            lblManualUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblManualUpdate.Location = new System.Drawing.Point(24, 96);
            lblManualUpdate.Name = "lblManualUpdate";
            lblManualUpdate.Size = new System.Drawing.Size(328, 15);
            lblManualUpdate.TabIndex = 4;
            lblManualUpdate.TabStop = true;
            lblManualUpdate.Text = "https://github.com/jexuswebserver/JexusManager/releases";
            lblManualUpdate.Visible = false;
            lblManualUpdate.LinkClicked += lblManualUpdate_LinkClicked;
            // 
            // btnRetry
            // 
            btnRetry.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRetry.Location = new System.Drawing.Point(152, 135);
            btnRetry.Name = "btnRetry";
            btnRetry.Size = new System.Drawing.Size(200, 30);
            btnRetry.TabIndex = 3;
            btnRetry.Text = "Retry";
            btnRetry.UseVisualStyleBackColor = true;
            btnRetry.Visible = false;
            btnRetry.Click += btnRetry_Click;
            // 
            // lblCurrentVersion
            // 
            lblCurrentVersion.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblCurrentVersion.Location = new System.Drawing.Point(24, 33);
            lblCurrentVersion.Name = "lblCurrentVersion";
            lblCurrentVersion.Size = new System.Drawing.Size(328, 20);
            lblCurrentVersion.TabIndex = 0;
            lblCurrentVersion.Text = "Current Version:";
            // 
            // lblUpdateStatus
            // 
            lblUpdateStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblUpdateStatus.Location = new System.Drawing.Point(24, 63);
            lblUpdateStatus.Name = "lblUpdateStatus";
            lblUpdateStatus.Size = new System.Drawing.Size(328, 30);
            lblUpdateStatus.TabIndex = 1;
            lblUpdateStatus.Text = "Checking for updates...";
            // 
            // btnDownloadUpdate
            // 
            btnDownloadUpdate.Location = new System.Drawing.Point(24, 135);
            btnDownloadUpdate.Name = "btnDownloadUpdate";
            btnDownloadUpdate.Size = new System.Drawing.Size(140, 30);
            btnDownloadUpdate.TabIndex = 2;
            btnDownloadUpdate.Text = "Download Update";
            btnDownloadUpdate.UseVisualStyleBackColor = true;
            btnDownloadUpdate.Visible = false;
            btnDownloadUpdate.Click += btnDownloadUpdate_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(19, 32);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(0, 15);
            label3.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = System.Drawing.SystemColors.MenuHighlight;
            panel3.Dock = DockStyle.Top;
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Margin = new Padding(4, 3, 4, 3);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(862, 6);
            panel3.TabIndex = 1;
            // 
            // HomePage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "HomePage";
            Size = new System.Drawing.Size(862, 558);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tableLayoutPanel.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label label2;
        private Label label1;
        private Panel panel2;
        private GroupBox groupBox1;
        private LinkLabel txtHome;
        private Panel panel3;
        private Label label3;
        private LinkLabel txtStudio;
        private Button btnSponsor;
        private GroupBox groupBox2;
        private Label lblCurrentVersion;
        private Label lblUpdateStatus;
        private Button btnDownloadUpdate;
        private Button btnRetry;
        private LinkLabel lblManualUpdate;
        private TableLayoutPanel tableLayoutPanel;
    }
}

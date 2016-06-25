namespace JexusManager.Features.Rewrite
{
    using System.ComponentModel;
    using System.Windows.Forms;

    partial class NewRewriteRuleDialog
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
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ilLarge = new System.Windows.Forms.ImageList(this.components);
            this.ilSmall = new System.Windows.Forms.ImageList(this.components);
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnBig = new System.Windows.Forms.ToolStripButton();
            this.btnSmall = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(567, 386);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(471, 386);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select a rule template:";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(25, 41);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(622, 258);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.SmallIcon;
            // 
            // ilLarge
            // 
            this.ilLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilLarge.ImageSize = new System.Drawing.Size(32, 32);
            this.ilLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ilSmall
            // 
            this.ilSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.ilSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(25, 299);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(622, 64);
            this.txtDescription.TabIndex = 4;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(50, 0);
            this.toolStripContainer1.Location = new System.Drawing.Point(597, 13);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(50, 23);
            this.toolStripContainer1.TabIndex = 5;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBig,
            this.btnSmall});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(49, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // btnBig
            // 
            this.btnBig.CheckOnClick = true;
            this.btnBig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBig.Name = "btnBig";
            this.btnBig.Size = new System.Drawing.Size(23, 22);
            this.btnBig.Text = "Large icon view";
            // 
            // btnSmall
            // 
            this.btnSmall.Checked = true;
            this.btnSmall.CheckOnClick = true;
            this.btnSmall.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnSmall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSmall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSmall.Name = "btnSmall";
            this.btnSmall.Size = new System.Drawing.Size(23, 22);
            this.btnSmall.Text = "Small icon view";
            // 
            // NewRewriteRuleDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(674, 421);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "NewRewriteRuleDialog";
            this.Text = "Add Rule(s)";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.NewRewriteRuleDialogHelpButtonClicked);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private Label label1;
        private ListView listView1;
        private TextBox txtDescription;
        private ImageList ilSmall;
        private ImageList ilLarge;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip toolStrip1;
        private ToolStripButton btnBig;
        private ToolStripButton btnSmall;
    }
}

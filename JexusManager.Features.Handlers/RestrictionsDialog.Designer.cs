namespace JexusManager.Features.Handlers
{
    using System.ComponentModel;
    using System.Windows.Forms;

    internal partial class RestrictionsDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.rbEither = new System.Windows.Forms.RadioButton();
            this.rbFolder = new System.Windows.Forms.RadioButton();
            this.rbFile = new System.Windows.Forms.RadioButton();
            this.cbInvoke = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVerbs = new System.Windows.Forms.TextBox();
            this.rbSelectedVerbs = new System.Windows.Forms.RadioButton();
            this.rbAllVerbs = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.rbExecute = new System.Windows.Forms.RadioButton();
            this.rbScript = new System.Windows.Forms.RadioButton();
            this.rbWrite = new System.Windows.Forms.RadioButton();
            this.rbRead = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(332, 301);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(236, 301);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(415, 283);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.rbEither);
            this.tabPage1.Controls.Add(this.rbFolder);
            this.tabPage1.Controls.Add(this.rbFile);
            this.tabPage1.Controls.Add(this.cbInvoke);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(407, 257);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mapping";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // rbEither
            // 
            this.rbEither.AutoSize = true;
            this.rbEither.Enabled = false;
            this.rbEither.Location = new System.Drawing.Point(44, 110);
            this.rbEither.Name = "rbEither";
            this.rbEither.Size = new System.Drawing.Size(82, 17);
            this.rbEither.TabIndex = 3;
            this.rbEither.TabStop = true;
            this.rbEither.Text = "File or folder";
            this.rbEither.UseVisualStyleBackColor = true;
            // 
            // rbFolder
            // 
            this.rbFolder.AutoSize = true;
            this.rbFolder.Enabled = false;
            this.rbFolder.Location = new System.Drawing.Point(44, 73);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(54, 17);
            this.rbFolder.TabIndex = 2;
            this.rbFolder.TabStop = true;
            this.rbFolder.Text = "Folder";
            this.rbFolder.UseVisualStyleBackColor = true;
            // 
            // rbFile
            // 
            this.rbFile.AutoSize = true;
            this.rbFile.Enabled = false;
            this.rbFile.Location = new System.Drawing.Point(44, 40);
            this.rbFile.Name = "rbFile";
            this.rbFile.Size = new System.Drawing.Size(41, 17);
            this.rbFile.TabIndex = 1;
            this.rbFile.TabStop = true;
            this.rbFile.Text = "File";
            this.rbFile.UseVisualStyleBackColor = true;
            // 
            // cbInvoke
            // 
            this.cbInvoke.AutoSize = true;
            this.cbInvoke.Location = new System.Drawing.Point(23, 17);
            this.cbInvoke.Name = "cbInvoke";
            this.cbInvoke.Size = new System.Drawing.Size(231, 17);
            this.cbInvoke.TabIndex = 0;
            this.cbInvoke.Text = "Invoke handler only if request is mapped to:";
            this.cbInvoke.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtVerbs);
            this.tabPage2.Controls.Add(this.rbSelectedVerbs);
            this.tabPage2.Controls.Add(this.rbAllVerbs);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(407, 257);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Verbs";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Example: GET, POST";
            // 
            // txtVerbs
            // 
            this.txtVerbs.Enabled = false;
            this.txtVerbs.Location = new System.Drawing.Point(52, 114);
            this.txtVerbs.Name = "txtVerbs";
            this.txtVerbs.Size = new System.Drawing.Size(241, 20);
            this.txtVerbs.TabIndex = 3;
            // 
            // rbSelectedVerbs
            // 
            this.rbSelectedVerbs.AutoSize = true;
            this.rbSelectedVerbs.Location = new System.Drawing.Point(34, 82);
            this.rbSelectedVerbs.Name = "rbSelectedVerbs";
            this.rbSelectedVerbs.Size = new System.Drawing.Size(151, 17);
            this.rbSelectedVerbs.TabIndex = 2;
            this.rbSelectedVerbs.TabStop = true;
            this.rbSelectedVerbs.Text = "One of the following verbs:";
            this.rbSelectedVerbs.UseVisualStyleBackColor = true;
            // 
            // rbAllVerbs
            // 
            this.rbAllVerbs.AutoSize = true;
            this.rbAllVerbs.Location = new System.Drawing.Point(34, 47);
            this.rbAllVerbs.Name = "rbAllVerbs";
            this.rbAllVerbs.Size = new System.Drawing.Size(65, 17);
            this.rbAllVerbs.TabIndex = 1;
            this.rbAllVerbs.TabStop = true;
            this.rbAllVerbs.Text = "All verbs";
            this.rbAllVerbs.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Specify the verbs to be handled:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.rbExecute);
            this.tabPage3.Controls.Add(this.rbScript);
            this.tabPage3.Controls.Add(this.rbWrite);
            this.tabPage3.Controls.Add(this.rbRead);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.rbNone);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(407, 257);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Access";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // rbExecute
            // 
            this.rbExecute.AutoSize = true;
            this.rbExecute.Location = new System.Drawing.Point(47, 138);
            this.rbExecute.Name = "rbExecute";
            this.rbExecute.Size = new System.Drawing.Size(64, 17);
            this.rbExecute.TabIndex = 5;
            this.rbExecute.TabStop = true;
            this.rbExecute.Text = "Execute";
            this.rbExecute.UseVisualStyleBackColor = true;
            // 
            // rbScript
            // 
            this.rbScript.AutoSize = true;
            this.rbScript.Location = new System.Drawing.Point(47, 115);
            this.rbScript.Name = "rbScript";
            this.rbScript.Size = new System.Drawing.Size(52, 17);
            this.rbScript.TabIndex = 4;
            this.rbScript.TabStop = true;
            this.rbScript.Text = "Script";
            this.rbScript.UseVisualStyleBackColor = true;
            // 
            // rbWrite
            // 
            this.rbWrite.AutoSize = true;
            this.rbWrite.Location = new System.Drawing.Point(47, 92);
            this.rbWrite.Name = "rbWrite";
            this.rbWrite.Size = new System.Drawing.Size(50, 17);
            this.rbWrite.TabIndex = 3;
            this.rbWrite.TabStop = true;
            this.rbWrite.Text = "Write";
            this.rbWrite.UseVisualStyleBackColor = true;
            // 
            // rbRead
            // 
            this.rbRead.AutoSize = true;
            this.rbRead.Location = new System.Drawing.Point(47, 69);
            this.rbRead.Name = "rbRead";
            this.rbRead.Size = new System.Drawing.Size(51, 17);
            this.rbRead.TabIndex = 2;
            this.rbRead.TabStop = true;
            this.rbRead.Text = "Read";
            this.rbRead.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(211, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Specify the access required by the handler:";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Location = new System.Drawing.Point(47, 46);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(51, 17);
            this.rbNone.TabIndex = 0;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "None";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // RestrictionsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(439, 336);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "RestrictionsDialog";
            this.Text = "Request Restrictions";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.RestrictionsDialogHelpButtonClicked);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnCancel;
        private Button btnOK;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private RadioButton rbEither;
        private RadioButton rbFolder;
        private RadioButton rbFile;
        private CheckBox cbInvoke;
        private TabPage tabPage2;
        private Label label2;
        private TextBox txtVerbs;
        private RadioButton rbSelectedVerbs;
        private RadioButton rbAllVerbs;
        private Label label1;
        private TabPage tabPage3;
        private RadioButton rbExecute;
        private RadioButton rbScript;
        private RadioButton rbWrite;
        private RadioButton rbRead;
        private Label label3;
        private RadioButton rbNone;
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class WizardForm : BaseTaskForm
#else
    public abstract class WizardForm : BaseTaskForm
#endif
    {
        private WizardPage[] _pages;
        private Panel _panel1;
        private Label _txtTitle;
        private Panel _pnlContainer;
        private Button _btnCancel;
        private Button _btnFinish;
        private Button _btnNext;
        private Button _btnPrevious;
        private ProgressBar _pbMain;
        private int _index;
        private PictureBox _pictureBox1;
        private Image _taskGlyph;

#if DESIGN
        public WizardForm()
        {
            InitializeComponent();
        }
#endif

        protected WizardForm(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();
        }

        protected virtual void CancelWizard()
        {
            Close();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            NavigateToPreviousPage();
            Update();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            StartTaskProgress();
            _btnPrevious.Enabled = false;
            _btnNext.Enabled = false;
            _btnFinish.Enabled = false;
            _btnCancel.Focus();
            NavigateToNextPage();
            StopTaskProgress();
            Update();
        }

#if DESIGN
        protected virtual void CompleteWizard() { }
        protected virtual WizardPage[] GetWizardPages()
        {
            return new WizardPage[0];
        }
#else
        protected abstract void CompleteWizard();
        protected abstract WizardPage[] GetWizardPages();
#endif

        protected void NavigateToNextPage()
        {
            if (!CurrentPage.OnNext())
            {
                return;
            }

            OnPageChanging(EventArgs.Empty);
            var next = CurrentPage.NextPage;
            _index = this.Pages.IndexOf(next);
            OnPageChanged(EventArgs.Empty);
        }

        protected void NavigateToPreviousPage()
        {
            if (!CurrentPage.OnPrevious())
            {
                return;
            }

            OnPageChanging(EventArgs.Empty);
            var previous = this.CurrentPage.PreviousPage;
            _index = this.Pages.IndexOf(previous);
            OnPageChanged(EventArgs.Empty);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        protected override void OnInitialActivated(EventArgs e)
        {
            base.OnInitialActivated(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _index = StartPageIndex;
            OnPageChanged(e);
            Update();
        }

        protected virtual void OnPageChanged(EventArgs e)
        {
            if (CurrentPage == null)
            {
                return;
            }

            _txtTitle.Text = CurrentPage.Caption;
            _pnlContainer.Controls.Clear();
            _pnlContainer.Controls.Add(CurrentPage);
            CurrentPage.Activate();
        }

        protected virtual void OnPageChanging(EventArgs e)
        { }

        protected override void OnPaint(PaintEventArgs e)
        { }

        protected virtual bool ShouldShowPage(WizardPage page)
        {
            return false;
        }

        protected override void ShowHelp()
        {
            throw new NotImplementedException();
        }

        protected override void StartTaskProgress()
        {
            _pbMain.Visible = true;
        }

        protected override void StopTaskProgress()
        {
            _pbMain.Visible = false;
        }

        protected override sealed void Update()
        {
            if (CurrentPage == null)
            {
                return;
            }

            _btnNext.Enabled = CurrentPage.CanNavigateNext && _index + 1 != _pages.Length;
            _btnPrevious.Enabled = CurrentPage.CanNavigatePrevious;
            _btnFinish.Enabled = CanComplete;
            if (_btnFinish.Enabled)
            {
                AcceptButton = _btnFinish;
            }
            else if (_btnNext.Enabled)
            {
                AcceptButton = _btnNext;
            }
            else
            {
                AcceptButton = null;
            }
        }

        protected internal void UpdateWizard()
        {
            Update();
        }

        protected override int BorderMargin { get { return 0; } }
        protected virtual bool CanCancel { get { return true; } }

        protected virtual bool CanComplete
        {
            get { return _pages.Length == _index + 1 && CurrentPage.CanNavigateNext; }
        }

        protected override bool CanShowHelp
        {
            get { return CurrentPage.CanShowHelp; }
        }

        protected WizardPage CurrentPage
        {
            get
            {
                return Pages.Count == 0 ? null : (WizardPage)Pages[_index];
            }
        }

        protected virtual bool IsCancellable { get { return false; } }

        protected internal IList Pages
        {
            get { return _pages ?? (_pages = GetWizardPages()); }
        }

        protected virtual int StartPageIndex { get { return 0; } }
        public string TaskCaption { get; set; }
        public BorderStyle TaskCaptionBorderStyle { get; set; }
        public string TaskDescription { get; set; }

        public Image TaskGlyph
        {
            get
            {
                return _taskGlyph;
            }

            set
            {
                _taskGlyph = value;
                _pictureBox1.Image = value;
            }
        }

        public Color TaskProgressEndColor { get; set; }
        public int TaskProgressGradientSpeed { get; set; }
        public int TaskProgressScrollSpeed { get; set; }
        public Color TaskProgressStartColor { get; set; }
        protected internal virtual object WizardData { get; }

        private void InitializeComponent()
        {
            _panel1 = new Panel();
            _pictureBox1 = new PictureBox();
            _txtTitle = new Label();
            _pnlContainer = new Panel();
            _btnCancel = new Button();
            _btnFinish = new Button();
            _btnNext = new Button();
            _btnPrevious = new Button();
            _pbMain = new ProgressBar();
            _panel1.SuspendLayout();
            ((ISupportInitialize)(_pictureBox1)).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            _panel1.Anchor = (AnchorStyles.Top | AnchorStyles.Left)
                            | AnchorStyles.Right;
            _panel1.BackColor = Color.White;
            _panel1.Controls.Add(_pictureBox1);
            _panel1.Controls.Add(_txtTitle);
            _panel1.Location = new Point(-1, 6);
            _panel1.Name = "panel1";
            _panel1.Size = new Size(670, 65);
            _panel1.TabIndex = 10;
            // 
            // pictureBox1
            // 
            _pictureBox1.Location = new Point(10, 10);
            _pictureBox1.Name = "pictureBox1";
            _pictureBox1.Size = new Size(48, 48);
            _pictureBox1.TabIndex = 1;
            _pictureBox1.TabStop = false;
            // 
            // txtTitle
            // 
            _txtTitle.AutoSize = true;
            _txtTitle.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            _txtTitle.Location = new Point(87, 22);
            _txtTitle.Name = "txtTitle";
            _txtTitle.Size = new Size(0, 24);
            _txtTitle.TabIndex = 2;
            // 
            // pnlContainer
            // 
            _pnlContainer.Location = new Point(-1, 66);
            _pnlContainer.Name = "pnlContainer";
            _pnlContainer.Size = new Size(670, 380);
            _pnlContainer.TabIndex = 15;
            // 
            // btnCancel
            // 
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.Location = new Point(571, 452);
            _btnCancel.Name = "btnCancel";
            _btnCancel.Size = new Size(85, 23);
            _btnCancel.TabIndex = 14;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnFinish
            // 
            _btnFinish.DialogResult = DialogResult.OK;
            _btnFinish.Enabled = false;
            _btnFinish.Location = new Point(480, 452);
            _btnFinish.Name = "btnFinish";
            _btnFinish.Size = new Size(85, 23);
            _btnFinish.TabIndex = 13;
            _btnFinish.Text = "Finish";
            _btnFinish.UseVisualStyleBackColor = true;
            _btnFinish.Click += btnFinish_Click;
            // 
            // btnNext
            // 
            _btnNext.Enabled = false;
            _btnNext.Location = new Point(389, 452);
            _btnNext.Name = "btnNext";
            _btnNext.Size = new Size(85, 23);
            _btnNext.TabIndex = 12;
            _btnNext.Text = "Next";
            _btnNext.UseVisualStyleBackColor = true;
            _btnNext.Click += btnNext_Click;
            // 
            // btnPrevious
            // 
            _btnPrevious.Enabled = false;
            _btnPrevious.Location = new Point(298, 452);
            _btnPrevious.Name = "btnPrevious";
            _btnPrevious.Size = new Size(85, 23);
            _btnPrevious.TabIndex = 11;
            _btnPrevious.Text = "Previous";
            _btnPrevious.UseVisualStyleBackColor = true;
            _btnPrevious.Click += btnPrevious_Click;
            // 
            // pbMain
            // 
            _pbMain.Location = new Point(-1, 72);
            _pbMain.Name = "pbMain";
            _pbMain.Size = new Size(670, 4);
            _pbMain.Style = ProgressBarStyle.Marquee;
            _pbMain.TabIndex = 16;
            _pbMain.Visible = false;
            // 
            // WizardForm
            // 
            ClientSize = new Size(669, 481);
            Controls.Add(_pbMain);
            Controls.Add(_panel1);
            Controls.Add(_pnlContainer);
            Controls.Add(_btnCancel);
            Controls.Add(_btnFinish);
            Controls.Add(_btnNext);
            Controls.Add(_btnPrevious);
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WizardForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            HelpRequested += WizardForm_HelpRequested;
            _panel1.ResumeLayout(false);
            _panel1.PerformLayout();
            ((ISupportInitialize)(_pictureBox1)).EndInit();
            ResumeLayout(false);
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            CompleteWizard();
        }

        private void WizardForm_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            ShowHelp();
        }
    }
}

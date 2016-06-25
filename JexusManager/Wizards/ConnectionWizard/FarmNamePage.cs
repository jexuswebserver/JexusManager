using System;
using System.Windows.Forms;

namespace JexusManager.WizardPanels
{
    public partial class FarmNamePage : UserControl, IWizardPage
    {
        FarmWizard _host;
        public FarmNamePage(FarmWizard host)
        {
            InitializeComponent();
            _host = host;
        }

        public IWizardPage Previous { get; set; }

        public IWizardPage Next { get; set; }

        public bool CanPrevious
        {
            get { return Previous != null; }
        }

        public bool CanNext
        {
            get { return Next != null && !string.IsNullOrWhiteSpace(txtName.Text); }
        }

        public bool CanFinish
        {
            get { return false; }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            _host.FarmName = txtName.Text;
            _host.SetNext(CanNext);
        }

        public string Title
        {
            get { return "Specify Server Farm Name"; }
        }

        public void SetFocus()
        {
            txtName.Focus();
        }
    }
}

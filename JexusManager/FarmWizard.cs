using System.IO;
using JexusManager.Dialogs;
using JexusManager.WizardPanels;
using Microsoft.Web.Administration;
using System;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace JexusManager
{
    public partial class FarmWizard : Form
    {
        private FarmNamePage _name;
        private AddressPage _address;
        private IWizardPage _current;

        public FarmWizard()
        {
            InitializeComponent();
        }

        private void UpdateButtons(IWizardPage page)
        {
            txtTitle.Text = page.Title;
            pnlContainer.Controls.Clear();
            pnlContainer.Controls.Add((UserControl)page);        
            txtTitle.Text = page.Title;
            btnNext.Enabled = page.CanNext;
            btnPrevious.Enabled = page.CanPrevious;
            btnFinish.Enabled = page.CanFinish;
            SetAcceptButton();
            page.SetFocus();
        }

        private void SetAcceptButton()
        {
            if (btnFinish.Enabled)
            {
                AcceptButton = btnFinish;
            }
            else if (btnNext.Enabled)
            {
                AcceptButton = btnNext;
            }
            else
            {
                AcceptButton = null;
            }
        }

        internal void SetFinish(bool canFinish)
        {
            btnFinish.Enabled = canFinish;
            SetAcceptButton();
        }

        internal void SetNext(bool canNext)
        {
            btnNext.Enabled = canNext;
            SetAcceptButton();
        }

        public string FarmName { get; set; }

        public List<FarmServerAdvancedSettings> Servers
        {
            get { return _address.Servers; }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            _current = _current.Previous;
            UpdateButtons(_current);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {            
            _current = _current.Next;
            UpdateButtons(_current);
        }

        private void ConnectionWizard_Shown(object sender, EventArgs e)
        {
            _name = new FarmNamePage(this);
            _address = new AddressPage(this);
            _name.Next = _address;
            _address.Previous = _name;
            _current = _name;
            UpdateButtons(_name);
        }
    }
}

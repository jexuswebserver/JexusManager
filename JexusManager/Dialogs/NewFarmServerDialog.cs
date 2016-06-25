using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace JexusManager.Dialogs
{
    public partial class NewFarmServerDialog : Form
    {
        public NewFarmServerDialog()
        {
            InitializeComponent();
            addressPage1.ServersChanged +=
                (obj, args) =>
                {
                    btnOK.Enabled = addressPage1.Servers.Count() > 0;
                };
        }

        public List<FarmServerAdvancedSettings> Servers
        {
            get { return addressPage1.Servers; }
        }

        private void NewFarmServerDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?linkid=139346");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JexusManager.WizardPanels
{
    public partial class AddressPage : UserControl, IWizardPage
    {        
        FarmWizard _host;

        public AddressPage()
            : this(null)
        { }

        public AddressPage(FarmWizard host)
        {
            InitializeComponent();
            _host = host;
            Reset();
        }

        private void Reset()
        {
            txtName.Text = string.Empty;
            var settings = new FarmServerAdvancedSettings
            {
                HttpPort = 80,
                HttpsPort = 443,
                Weight = 100
            };
            pgAdvanced.SelectedObject = settings;
        }

        public IWizardPage Previous { get; set; }

        public IWizardPage Next { get; set; }

        public bool CanPrevious
        {
            get { return Previous != null; }
        }

        public bool CanNext
        {
            get { return false; }
        }

        public bool CanFinish
        {
            get { return true; }
        }

        private void cbHostName_TextChanged(object sender, EventArgs e)
        {
            if (_host == null)
            {
                return;
            }

            _host.Name = txtName.Text;
            _host.SetNext(CanNext);
        }

        public string Title
        {
            get { return "Add Server"; }
        }

        public void SetFocus()
        {
            txtName.Focus();
        }

        private void txtAdvanced_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtAdvanced.Text = txtAdvanced.Text == "Advanced settings..." 
                ? "Hide advanced settings..." 
                : "Advanced settings...";
            pgAdvanced.Visible = txtAdvanced.Text != "Advanced settings...";
            lvServers.Height = txtAdvanced.Text == "Advanced settings..." ? 250 : 110;
            lvServers.Location = new System.Drawing.Point(24, txtAdvanced.Text == "Advanced settings..." ? 104 : 243);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
        }

        private void lvServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = lvServers.SelectedItems.Count > 0;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lvServers.SelectedItems[0].Remove();
            var handler = ServersChanged;
            if (handler == null)
            {
                return;
            }

            handler(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var settings = (FarmServerAdvancedSettings)pgAdvanced.SelectedObject;
            settings.Name = txtName.Text;
            var item = new ListViewItem(new[]
            {
                txtName.Text,
                "Online"
            })
            {
                Tag = settings
            };
            lvServers.Items.Add(item);
            Reset();
            var handler = ServersChanged;
            if (handler == null)
            {
                return;
            }

            handler(sender, e);
        }

        public List<FarmServerAdvancedSettings> Servers
        {
            get
            {
                var result = new List<FarmServerAdvancedSettings>();
                foreach (ListViewItem item in lvServers.Items)
                {
                    result.Add((FarmServerAdvancedSettings)item.Tag);
                }

                return result;
            }
        }

        public event EventHandler<EventArgs> ServersChanged;
    }
}

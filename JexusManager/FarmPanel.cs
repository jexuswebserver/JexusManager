using System;
using System.Windows.Forms;
using Microsoft.Web.Administration;
using System.Diagnostics;
using JexusManager.Properties;

namespace JexusManager
{
    public partial class FarmPanel : UserControl
    {
        private Form1 _form;
        private ServerManager _server;

        public FarmPanel(ServerManager server, Form1 form)
        {
            InitializeComponent();
            imageList1.Images.Add(Resources.farm_server_16);
            _server = server;
            _form = form;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (splitContainer1.Panel2.Width > 500)
            {
                splitContainer1.SplitterDistance = splitContainer1.Width - 500;
            }
        }

        private void actRemove_Execute(object sender, EventArgs e)
        {
            var node = listView1.SelectedItems[0];
            node.Remove();
            var config = _server.GetApplicationHostConfiguration();
            ConfigurationSection webFarmsSection = config.GetSection("webFarms");
            ConfigurationElementCollection webFarmsCollection = webFarmsSection.GetCollection();
            ConfigurationElement remove = null;
            foreach (ConfigurationElement item in webFarmsCollection)
            {
                if (item["name"].ToString() == node.Text)
                {
                    remove = item;
                }
            }

            webFarmsCollection.Remove(remove);
            _form.RemoveFarmNode(node.Text);
        }

        private void actAdd_Execute(object sender, EventArgs e)
        {
            var dialog = new FarmWizard();
            if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            var item = new ListViewItem(new[]
            {
                dialog.FarmName,
                "Online"
            })
            { Tag = dialog.Servers, ImageIndex = 0, StateImageIndex = 0 };
            listView1.Items.Add(item);
            var config = _server.GetApplicationHostConfiguration();
            ConfigurationSection webFarmsSection = config.GetSection("webFarms");
            ConfigurationElementCollection webFarmsCollection = webFarmsSection.GetCollection();
            ConfigurationElement webFarmElement = webFarmsCollection.CreateElement("webFarm");
            webFarmElement["name"] = dialog.FarmName;
            ConfigurationElementCollection webFarmCollection = webFarmElement.GetCollection();

            foreach (var server in dialog.Servers)
            {
                ConfigurationElement serverElement = webFarmCollection.CreateElement("server");
                serverElement["address"] = server.Name;
                serverElement["enabled"] = true;

                ConfigurationElement applicationRequestRoutingElement = serverElement.GetChildElement("applicationRequestRouting");
                applicationRequestRoutingElement["weight"] = server.Weight;
                applicationRequestRoutingElement["httpPort"] = server.HttpPort;
                applicationRequestRoutingElement["httpsPort"] = server.HttpsPort;
                webFarmCollection.Add(serverElement);
            }

            webFarmsCollection.Add(webFarmElement);
            _form.AddFarmNode(dialog.FarmName, dialog.Servers);
        }

        private void actHelp_Execute(object sender, EventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?linkid=139355");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            actRemove.Visible = listView1.SelectedItems.Count > 0;
            actOffline.Visible = listView1.SelectedItems.Count > 0;
            toolStripSeparator5.Visible = listView1.SelectedItems.Count > 0;
            toolStripLabel4.Visible = listView1.SelectedItems.Count > 0;
            toolStripSeparator9.Visible = listView1.SelectedItems.Count > 0;
            toolStripLabel5.Visible = listView1.SelectedItems.Count > 0;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewRestrictionDialog : DialogForm
    {
        public NewRestrictionDialog(IServiceProvider serviceProvider, bool allowed, IpSecurityFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = allowed ? "Add Allow Restriction Rule" : "Add Deny Restriction Rule";
            txtDescription.Text = allowed
                ? "Allow access for the following IP address or domain name:"
                : "Deny access for the following IP address or domain name:";

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbAddress, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbRange, "CheckedChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtAddress.Enabled = rbAddress.Checked;
                    txtRange.Enabled = rbRange.Checked;
                    txtMask.Enabled = rbRange.Checked;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (rbAddress.Checked)
                    {
                        IPAddress result;
                        var passed = IPAddress.TryParse(txtAddress.Text, out result);
                        if (!passed)
                        {
                            MessageBox.Show(string.Format("'{0}' is an invalid IP address.", txtAddress.Text), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        Item = new IpSecurityItem(null);
                        Item.Address = txtAddress.Text;
                        Item.Mask = string.Empty;
                        Item.Allowed = allowed;
                        if (feature.Items.Any(item => item.Match(Item)))
                        {
                            ShowMessage(
                                "A restriction for this domain name or IP address already exists.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        DialogResult = DialogResult.OK;
                        return;
                    }

                    IPAddress result1;
                    var passed1 = IPAddress.TryParse(txtRange.Text, out result1);
                    if (!passed1)
                    {
                        ShowMessage(
                            string.Format("'{0}' is an invalid IP address.", txtRange.Text),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    var passed2 = IPAddress.TryParse(txtMask.Text, out result1);
                    if (!passed2)
                    {
                        int value;
                        var passed3 = int.TryParse(txtMask.Text, out value);
                        if (!passed3 ||
                            (result1.AddressFamily == AddressFamily.InterNetwork
                            && (value < 0 || value > 32)) ||
                            (result1.AddressFamily == AddressFamily.InterNetworkV6
                            && (value < 0 || value > 128)))
                        {
                            ShowMessage(
                                string.Format(
                                    "'{0}' is an invalid subnet mask. It must be a valid IP address or an integer value between 0-32 for IPv4 ad 0-128 for IPv6.",
                                    txtMask.Text),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }
                    }

                    Item = new IpSecurityItem(null);
                    Item.Address = txtRange.Text;
                    Item.Mask = txtMask.Text;
                    Item.Allowed = allowed;
                    if (feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "A restriction for this domain name or IP address already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IpSecurityItem Item { get; set; }
    }
}

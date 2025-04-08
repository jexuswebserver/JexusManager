// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;
    using JexusManager.Services;
    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class AddProviderSettingDialog : DialogForm
    {
        private readonly ProviderItem _provider;

        public AddProviderSettingDialog(Module module, ProviderItem provider, SettingItem existing = null)
            : base(module)
        {
            InitializeComponent();
            _provider = provider;

            Text = existing == null ? "Add Provider Setting" : "Edit Provider Setting";

            // Add common setting names to the combo box
            cbName.Items.Add("connectionString");
            cbName.Items.Add("defaultProvider");
            cbName.Items.Add("providerName");

            if (existing != null)
            {
                cbName.Text = existing.Key;
                cbName.Enabled = false;
                txtValue.Text = existing.Value;
                // Note: SettingItem doesn't have an Encrypted property
                // So we'll disable this feature when editing existing items
                cbEncrypt.Enabled = false;
            }
            else
            {
                cbName.SelectedIndex = 0;
            }

            SettingItem = existing;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtValue, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(cbName.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (string.IsNullOrEmpty(cbName.Text))
                    {
                        ShowMessage("Setting name cannot be empty.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    // Check for duplicate setting names if adding a new setting
                    if (SettingItem == null && _provider != null &&
                        _provider.Settings != null &&
                        _provider.Settings.Any(item => item.Key == cbName.Text))
                    {
                        ShowMessage("A setting with this name already exists.", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    var service = (IConfigurationService)GetService(typeof(IConfigurationService));
                    if (SettingItem == null)
                    {
                        var settingsCollection = _provider.Element.GetCollection("settings");
                        var element = settingsCollection.CreateElement();

                        SettingItem = new SettingItem(element);
                        SettingItem.Key = cbName.Text;
                        SettingItem.Value = txtValue.Text;

                        // Handle encryption if needed
                        if (cbEncrypt.Checked)
                        {
                            // TODO: Implement encryption logic if required
                            // Since SettingItem doesn't have built-in encryption support
                        }

                        SettingItem.Apply();
                        settingsCollection.Add(element);
                        service.ServerManager.CommitChanges();
                    }
                    else
                    {
                        // Update existing setting
                        SettingItem.Value = txtValue.Text;
                        SettingItem.Apply();
                        service.ServerManager.CommitChanges();
                    }

                    DialogResult = DialogResult.OK;
                }));
            container.Add(Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130403&amp;clcid=0x409");
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SettingItem SettingItem { get; private set; }
    }
}

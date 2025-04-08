// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Client.Win32;

namespace JexusManager.Features.Rewrite
{
    internal partial class AddProviderSettingDialog : DialogForm
    {
        private readonly SettingsFeature _feature;

        public AddProviderSettingDialog(Module module, SettingsFeature feature, SettingItem existing = null)
            : base(module)
        {
            InitializeComponent();
            _feature = feature;

            Text = existing == null ? "Add Provider Setting" : "Edit Provider Setting";

            // Add common setting names to the combo box
            cbName.Items.Add("connectionString");
            cbName.Items.Add("defaultProvider");
            cbName.Items.Add("providerName");

            if (existing != null)
            {
                cbName.Text = existing.Key;
                txtValue.Text = existing.Value;
                // TODO: disabled temp
                cbEncrypt.Enabled = false;
            }
            else
            {
                cbName.SelectedIndex = 0;
            }

            Item = existing;

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
                    if (_feature.FindDuplicate(item => item.Key, cbName.Text))
                    {
                        ShowMessage(
                            "A rewrite provider setting with this name already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    Item ??= new SettingItem(null);
                    Item.Key = cbName.Text;
                    Item.Value = txtValue.Text;
                    Item.Encrypted = cbEncrypt.Checked;

                    DialogResult = DialogResult.OK;
                }));
            container.Add(Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    _feature.ShowHelp();
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SettingItem Item { get; private set; }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewModuleDialog : DialogForm
    {
        public NewModuleDialog(IServiceProvider serviceProvider, ModulesItem existing, ModulesFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = existing == null ? "Add Managed Module" : "Edit Managed Module";
            txtName.ReadOnly = existing != null;
            Item = existing ?? new ModulesItem(null);
            if (existing != null)
            {
                txtPath.Text = Item.Type;
                txtName.Text = Item.Name;
                cbManagedHandler.Checked = Item.ForManagedOnly;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    Item.Type = txtPath.Text;
                    Item.Name = txtName.Text;
                    Item.ForManagedOnly = cbManagedHandler.Checked;
                    if (!txtName.ReadOnly && feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "A module with this name already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                                    && !string.IsNullOrWhiteSpace(txtPath.Text);
                }));
        }

        public ModulesItem Item { get; set; }

        private void NewRestrictionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210521");
        }
    }
}

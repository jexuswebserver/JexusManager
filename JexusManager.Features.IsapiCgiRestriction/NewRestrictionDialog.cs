// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiCgiRestriction
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewRestrictionDialog : DialogForm
    {
        public NewRestrictionDialog(IServiceProvider serviceProvider, IsapiCgiRestrictionItem existing, IsapiCgiRestrictionFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = existing == null ? "Add ISAPI or CGI Restriction" : "Edit ISAPI or CGI Restriction";
            Item = existing ?? new IsapiCgiRestrictionItem(null);
            if (existing != null)
            {
                txtPath.Text = Item.Path;
                txtName.Text = Item.Description;
                cbAllowed.Checked = Item.Allowed;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.Path = txtPath.Text;
                    Item.Description = txtName.Text;
                    Item.Allowed = cbAllowed.Checked;
                    if (feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "This restriction already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtPath.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowBrowseDialog(txtPath);
                }));
        }

        public IsapiCgiRestrictionItem Item { get; set; }

        private void NewRestrictionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210515");
        }
    }
}

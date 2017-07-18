// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Disposables;

    public partial class ImpersonationEditDialog : DialogForm
    {
        public ImpersonationEditDialog(IServiceProvider serviceProvider, ImpersonationItem existing)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    rbPool.Checked = txtName.Text.Length == 0;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSet, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var dialog = new CredentialsDialog(ServiceProvider, existing.Name);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtName.Text = dialog.UserName;
                    existing.Name = dialog.UserName;
                    existing.Password = dialog.Password;
                    SetButton();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogResult = DialogResult.OK;
                    if (rbPool.Checked)
                    {
                        existing.Name = string.Empty;
                        // TODO: reset password.
                        existing.Password = null;
                    }

                    existing.Apply();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbPool, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbSpecific, "CheckedChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnSet.Enabled = !rbPool.Checked;
                    SetButton();
                    var toElevate = !rbPool.Checked;
                    if (toElevate)
                    {
                        NativeMethods.TryAddShieldToButton(btnOK);
                    }
                    else
                    {
                        NativeMethods.RemoveShieldFromButton(btnOK);
                    }
                }));

            txtName.Text = "test"; // IMPORTANT: trigger a change event.
            txtName.Text = existing.Name;
        }

        private void SetButton()
        {
            // TODO: disable if not elevated. Need to find an in-place elevation approach.
            btnOK.Enabled = rbPool.Checked || (txtName.Text.Length != 0 && Microsoft.Web.Administration.PublicNativeMethods.IsProcessElevated);
        }

        private void AnonymousEditDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210461#Impersonation");
        }
    }
}

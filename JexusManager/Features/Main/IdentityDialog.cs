// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public partial class IdentityDialog : DialogForm
    {
        public IdentityDialog(IServiceProvider serviceProvider, ApplicationPoolProcessModel element)
            : base(serviceProvider)
        {
            InitializeComponent();
            rbBuiltin.Checked = element.IdentityType != ProcessModelIdentityType.SpecificUser;
            rbCustom.Checked = !rbBuiltin.Checked;
            if (element.IdentityType == ProcessModelIdentityType.SpecificUser)
            {
                txtCustom.Text = element.UserName;
            }
            else
            {
                switch (element.IdentityType)
                {
                    case ProcessModelIdentityType.LocalSystem:
                        cbBuiltin.SelectedIndex = 1;
                        break;
                    case ProcessModelIdentityType.LocalService:
                        cbBuiltin.SelectedIndex = 0;
                        break;
                    case ProcessModelIdentityType.NetworkService:
                        cbBuiltin.SelectedIndex = 2;
                        break;
                    case ProcessModelIdentityType.ApplicationPoolIdentity:
                        cbBuiltin.SelectedIndex = 3;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (rbCustom.Checked)
                    {
                        element.UserName = txtCustom.Text;
                        element.IdentityType = ProcessModelIdentityType.SpecificUser;
                    }

                    if (rbBuiltin.Checked)
                    {
                        element.UserName = string.Empty;
                        switch (cbBuiltin.SelectedIndex)
                        {
                            case 0:
                                element.IdentityType = ProcessModelIdentityType.LocalService;
                                break;
                            case 1:
                                element.IdentityType = ProcessModelIdentityType.LocalSystem;
                                break;
                            case 2:
                                element.IdentityType = ProcessModelIdentityType.NetworkService;
                                break;
                            case 3:
                                element.IdentityType = ProcessModelIdentityType.ApplicationPoolIdentity;
                                break;
                        }
                    }

                    this.DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbBuiltin, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbCustom, "CheckedChanged"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtCustom.Enabled = btnSet.Enabled = rbCustom.Checked;
                    cbBuiltin.Enabled = rbBuiltin.Checked;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSet, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using var dialog = new CredentialsDialog(null, txtCustom.Text);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtCustom.Text = dialog.UserName;
                    element.Password = dialog.Password;
                }));
        }

        private void IdentityDialogHelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210456#ApplicationPoolIdentity");
        }
    }
}

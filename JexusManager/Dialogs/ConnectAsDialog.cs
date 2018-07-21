// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.Windows.Forms;

    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Microsoft.Web.Management.Client.Win32;

    public partial class ConnectAsDialog : DialogForm
    {
        public ConnectAsDialog(Microsoft.Web.Administration.Application application)
        {
            InitializeComponent();
            var existing = application.VirtualDirectories[0];

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    rbPassThrough.Checked = txtName.Text.Length == 0;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSet, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var dialog = new CredentialsDialog(ServiceProvider, existing.UserName);
                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    txtName.Text = dialog.UserName;
                    existing.UserName = dialog.UserName;
                    existing.Password = dialog.Password;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbPassThrough, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbSpecific, "CheckedChanged"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtName.Enabled = btnSet.Enabled = rbSpecific.Checked;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ProcessStart("https://go.microsoft.com/fwlink/?LinkId=210531#Edit_Site");
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Clicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogResult = DialogResult.OK;
                    if (rbPassThrough.Checked)
                    {
                        existing.UserName = string.Empty;
                        existing.Password = string.Empty;
                    }
                }));

            txtName.Text = "test";
            txtName.Text = existing.UserName;
        }
    }
}

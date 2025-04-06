﻿// Copyright (c) Lex Li. All rights reserved.
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
#if DESIGN
        public ConnectAsDialog()
        {
            InitializeComponent();
        }
#endif
        public ConnectAsDialog(IServiceProvider serviceProvider, ConnectAsItem item)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    rbPassThrough.Checked = txtName.Text.Length == 0;
                    RefreshButton();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSet, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using (var dialog = new CredentialsDialog(ServiceProvider, item.UserName))
                    {
                        if (dialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        txtName.Text = dialog.UserName;
                        item.UserName = dialog.UserName;
                        item.Password = dialog.Password;
                    }
                    RefreshButton();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbPassThrough, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbSpecific, "CheckedChanged"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    txtName.Enabled = btnSet.Enabled = rbSpecific.Checked;
                    RefreshButton();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ProcessStart("https://go.microsoft.com/fwlink/?LinkId=210531#Edit_Site");
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogResult = DialogResult.OK;
                    if (rbPassThrough.Checked)
                    {
                        item.UserName = string.Empty;
                        item.Password = string.Empty;
                    }
                }));

            txtName.Text = "test";
            txtName.Text = item.UserName;
            var passThrough = string.IsNullOrEmpty(item.UserName);
            rbPassThrough.Checked = passThrough;
            rbSpecific.Checked = !passThrough;
        }

        private void RefreshButton()
        {
            btnOK.Enabled = rbPassThrough.Checked || !string.IsNullOrEmpty(txtName.Text);
        }
    }
}

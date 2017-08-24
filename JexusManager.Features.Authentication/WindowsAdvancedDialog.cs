// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using Microsoft.Web.Management.Client.Win32;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    public partial class WindowsAdvancedDialog : DialogForm
    {
        public WindowsAdvancedDialog(IServiceProvider serviceProvider, WindowsItem item)
            : base(serviceProvider)
        {
            InitializeComponent();
            btnOK.Enabled = false;
            cbExtended.SelectedIndex = item.TokenChecking;
            cbKernelMode.Checked = item.UseKernelMode;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbExtended, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbKernelMode, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    item.TokenChecking = cbExtended.SelectedIndex;
                    item.UseKernelMode = cbKernelMode.Checked;
                    item.Apply();
                    DialogResult = DialogResult.OK;
                }));
        }

        private void WindowsAdvancedDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210461#Advanced_Windows");
        }
    }
}

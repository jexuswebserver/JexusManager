// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Handlers
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class PermissionsDialog : DialogForm
    {
        public PermissionsDialog(IServiceProvider serviceProvider, HandlersFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbRead.Checked = (feature.AccessPolicy & 1L) == 1L;
            cbScript.Checked = (feature.AccessPolicy & 512L) == 512L;
            cbExecute.Checked = (feature.AccessPolicy & 4L) == 4L;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    if (cbRead.Checked)
                    {
                        feature.AccessPolicy |= 1L;
                    }
                    else
                    {
                        feature.AccessPolicy &= ~1L;
                    }

                    if (cbScript.Checked)
                    {
                        feature.AccessPolicy |= 512L;
                    }
                    else
                    {
                        feature.AccessPolicy &= ~512L;
                    }

                    if (cbExecute.Checked)
                    {
                        feature.AccessPolicy |= 4L;
                    }
                    else
                    {
                        feature.AccessPolicy &= ~4L;
                    }

                    DialogResult = DialogResult.OK;
                }));
        }

        private void PermissionsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210505");
        }
    }
}

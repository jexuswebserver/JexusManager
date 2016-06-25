// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Caching
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class CachingSettingsDialog : DialogForm
    {
        public CachingSettingsDialog(IServiceProvider serviceProvider, ConfigurationElement element)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbUser.Checked = (bool)element["enabled"];
            cbKernel.Checked = (bool)element["enableKernelCache"];
            txtSize.Text = element["maxResponseSize"].ToString();
            var limit = (uint)element["maxCacheSize"];
            cbLimit.Checked = limit != 0;
            if (cbLimit.Checked)
            {
                txtLimit.Text = limit.ToString();
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    element["enableKernelCache"] = cbKernel.Checked;
                    element["enabled"] = cbUser.Checked;
                    element["maxResponseSize"] = uint.Parse(txtSize.Text);
                    if (!cbLimit.Checked)
                    {
                        element["maxCacheSize"] = 0U;
                    }
                    else
                    {
                        element["maxCacheSize"] = uint.Parse(txtLimit.Text);
                    }

                    DialogResult = DialogResult.OK;
                }));
        }

        private void PermissionsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210522");
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class MapSettingsDialog : DialogForm
    {
        public MapSettingsDialog(IServiceProvider serviceProvider, MapItem item)
            : base(serviceProvider)
        {
            InitializeComponent();
            if (item != null)
            {
                txtName.Text = item.Name;
                txtName.ReadOnly = true;
                txtDefaultValue.Text = item.DefaultValue;
                cbIgnore.Checked = item.IgnoreCase;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    item.Name = txtName.Text;
                    item.DefaultValue = txtDefaultValue.Text;
                    item.IgnoreCase = cbIgnore.Checked;
                    item.Apply();
                    DialogResult = DialogResult.OK;
                }));
        }

        private void MapSettingsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkID=130406&amp;clcid=0x409");
        }
    }
}

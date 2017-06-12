// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddVariableDialog.cs" company="">
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class AddAllowedVariableDialog : DialogForm
    {
        public AddAllowedVariableDialog(IServiceProvider serviceProvider, AllowedVariablesFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (feature.Items.Any(item => txtName.Text == item.Name))
                    {
                        ShowMessage(
                            "The specified server variable already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    Item = new AllowedVariableItem(null, feature) { Name = txtName.Text };
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
                }));
        }

        private void AddVariableDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=163108");
        }

        public AllowedVariableItem Item { get; set; }
    }
}

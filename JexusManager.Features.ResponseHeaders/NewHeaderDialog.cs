// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.ResponseHeaders
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewHeaderDialog : DialogForm
    {
        public NewHeaderDialog(IServiceProvider serviceProvider, ResponseHeadersItem selectedItem, ResponseHeadersFeature feature)
            : base(serviceProvider)
        {
            Item = selectedItem;
            InitializeComponent();
            Text = selectedItem == null ? "Add Custom HTTP Response Header" : "Edit Custom HTTP Response Header";
            if (selectedItem != null)
            {
                txtName.Text = Item.Name;
                txtValue.Text = Item.Value;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    if (feature.Items.Any(item => item != Item && txtName.Text == item.Name))
                    {
                        ShowMessage(
                            "A header with this name already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (Item == null)
                    {
                        Item = new ResponseHeadersItem(null);
                    }

                    Item.Name = txtName.Text;
                    Item.Value = txtValue.Text;

                    DialogResult = DialogResult.OK;
                }));
        }

        private void NewHeaderDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210509");
        }

        public ResponseHeadersItem Item { get; set; }
    }
}

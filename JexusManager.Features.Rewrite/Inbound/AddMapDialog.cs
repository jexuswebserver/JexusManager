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

    internal partial class AddMapDialog : DialogForm
    {
        public MapRule Item { get; set; }

        public AddMapDialog(IServiceProvider serviceProvider, MapRule existing, MapsFeature feature)
            : base(serviceProvider)
        {
            Item = existing;
            InitializeComponent();
            Text = existing == null ? "Add Mapping Entry" : "Edit Mapping Entry";

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtOriginal, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtOriginal.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    if (Item == null)
                    {
                        Item = new MapRule(null, feature);
                    }

                    Item.Original = txtOriginal.Text;
                    Item.New = txtNew.Text;
                    DialogResult = DialogResult.OK;
                }));
        }

        private void AddMapDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkID=130406&amp;clcid=0x409");
        }
    }
}

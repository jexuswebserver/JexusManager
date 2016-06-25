// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewHeaderDialog : DialogForm
    {
        public NewHeaderDialog(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtLimit, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                && !string.IsNullOrWhiteSpace(txtLimit.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    Item = new HeadersItem(null) { Header = txtName.Text, SizeLimit = uint.Parse(txtLimit.Text) };
                    DialogResult = DialogResult.OK;
                }));
        }

        public HeadersItem Item { get; set; }

        private void NewHiddenSegmentDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210526#Allow_Header");
        }
    }
}

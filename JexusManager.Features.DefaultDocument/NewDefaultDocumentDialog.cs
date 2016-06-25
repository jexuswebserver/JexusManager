// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.DefaultDocument
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    public partial class NewDefaultDocumentDialog : DialogForm
    {
        public NewDefaultDocumentDialog(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    var invalid = "*".ToCharArray();
                    foreach (var ch in invalid)
                    {
                        if (txtName.Text.Contains(ch))
                        {
                            MessageBox.Show(string.Format("The specific default document contains the following invalid character: {0}.", ch), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    Item = new DocumentItem(null);
                    Item.Name = txtName.Text;
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text);
                }));
        }

        public DocumentItem Item { get; set; }

        private void NewDefaultDocumentDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210478");
        }
    }
}

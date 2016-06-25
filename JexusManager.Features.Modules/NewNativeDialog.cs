// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Modules
{
    using System;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewNativeDialog : DialogForm
    {
        public NewNativeDialog(IServiceProvider serviceProvider, GlobalModule existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            if (existing != null)
            {
                txtName.Text = existing.Name;
                txtPath.Text = existing.Image;
                this.Text = "Edit Native Module Registration";
                txtName.SelectAll();
                this.Item = existing;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .Subscribe(evt =>
                {
                    DialogHelper.ShowFileDialog(txtPath, "(*.dll)|*.dll|All Files (*.*)|*.*");
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                                   && !string.IsNullOrWhiteSpace(txtPath.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    if (Item == null)
                    {
                        Item = new GlobalModule(null);
                    }

                    this.Item.Name = txtName.Text;
                    this.Item.Image = txtPath.Text;
                }));
        }

        public GlobalModule Item { get; set; }
    }
}

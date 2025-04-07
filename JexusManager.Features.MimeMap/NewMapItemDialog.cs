﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.MimeMap
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewMapItemDialog : DialogForm
    {
        public NewMapItemDialog(IServiceProvider serviceProvider, MimeMapItem existing, MimeMapFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = string.Format("{0} MIME Type", existing == null ? "Add" : "Edit");
            Item = existing ?? new MimeMapItem(null);
            if (existing != null)
            {
                txtExtension.Text = Item.FileExtension;
                txtType.Text = Item.MimeType;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.FileExtension = txtExtension.Text;
                    Item.MimeType = txtType.Text;
                    if (feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "This MIME map already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtExtension, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtType, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtExtension.Text)
                        && !string.IsNullOrWhiteSpace(txtType.Text);
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MimeMapItem Item { get; set; }
    }
}

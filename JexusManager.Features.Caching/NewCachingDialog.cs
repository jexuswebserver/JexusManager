﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Caching
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewCachingDialog : DialogForm
    {
        public NewCachingDialog(IServiceProvider serviceProvider, CachingItem existing, CachingFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = existing == null ? "Add Cache Rule" : "Edit Cache Rule";
            Item = existing ?? new CachingItem(null);
            if (existing == null)
            {
                return;
            }

            txtExtension.Text = Item.Extension;
            cbUser.Checked = Item.Policy != 0L;
            rbUserFile.Checked = Item.Policy == 1L;
            rbUserTime.Checked = Item.Policy == 2L;
            rbUserNo.Checked = Item.Policy == 3L;

            cbKernel.Checked = Item.KernelCachePolicy != 0L;
            rbKernelFile.Checked = Item.KernelCachePolicy == 1L;
            rbKernelTime.Checked = Item.KernelCachePolicy == 2L;
            rbKernelNo.Checked = Item.KernelCachePolicy == 3L;

            txtKernelTime.Text = txtUserTime.Text = Item.Duration.ToString();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.Extension = txtExtension.Text;
                    Item.Duration = TimeSpan.Parse(txtKernelTime.Text);
                    if (!cbUser.Checked)
                    {
                        Item.Policy = 0L;
                    }
                    else if (rbUserFile.Checked)
                    {
                        Item.Policy = 1L;
                    }
                    else if (rbUserTime.Checked)
                    {
                        Item.Policy = 2L;
                    }
                    else if (rbUserNo.Checked)
                    {
                        Item.Policy = 3L;
                    }

                    if (!cbKernel.Checked)
                    {
                        Item.KernelCachePolicy = 0L;
                    }
                    else if (rbKernelFile.Checked)
                    {
                        Item.KernelCachePolicy = 1L;
                    }
                    else if (rbKernelTime.Checked)
                    {
                        Item.KernelCachePolicy = 2L;
                    }
                    else if (rbKernelNo.Checked)
                    {
                        Item.KernelCachePolicy = 3L;
                    }

                    if (feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "This rule already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnAdvanced, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    using var dialog = new CachingAdvancedDialog(ServiceProvider, Item, feature);
                    dialog.ShowDialog();
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtExtension, "TextChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtExtension.Text);
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
        public CachingItem Item { get; set; }
    }
}

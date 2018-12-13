// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IsapiFilters
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewFilterDialog : DialogForm
    {
        public NewFilterDialog(IServiceProvider serviceProvider, IsapiFiltersItem existing, IsapiFiltersFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = existing == null ? "Add ISAPI Filter" : "Edit ISAPI Filter";
            txtName.ReadOnly = existing != null;
            Item = existing ?? new IsapiFiltersItem(null);
            if (existing != null)
            {
                txtPath.Text = Item.Path;
                txtName.Text = Item.Name;
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.Path = txtPath.Text;
                    Item.Name = txtName.Text;
                    if (!txtName.ReadOnly && feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "A filter with this name already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    try
                    {
                        var bit32Condition = "bitness32";
                        var bit64Condition = "bitness64";
                        var bit32 = DialogHelper.GetImageArchitecture(txtPath.Text);
                        if (bit32 && !Item.PreConditions.Contains(bit32Condition))
                        {
                            Item.PreConditions.Add(bit32Condition);
                        }
                        else if (!bit32 && !Item.PreConditions.Contains(bit64Condition))
                        {
                            Item.PreConditions.Add(bit64Condition);
                        }
                    }
                    catch (Exception)
                    {
                        ShowMessage(
                            "The specific filter is invalid.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text)
                                    && !string.IsNullOrWhiteSpace(txtPath.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowOpenFileDialog(txtPath, "(*.dll)|*.dll|All Files (*.*)|*.*", null);
                }));

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        public IsapiFiltersItem Item { get; set; }
    }
}

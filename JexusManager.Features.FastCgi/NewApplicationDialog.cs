// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewApplicationDialog : DialogForm
    {
        private readonly FastCgiFeature _feature;

        public NewApplicationDialog(IServiceProvider serviceProvider, FastCgiItem existing, FastCgiFeature feature)
            : base(serviceProvider)
        {
            this.InitializeComponent();
            this.Text = string.Format("{0} FastCGI Application", existing == null ? "Add" : "Edit");
            _feature = feature;
            txtPath.ReadOnly = txtArguments.ReadOnly = existing != null;
            this.Item = existing ?? new FastCgiItem(null);
            txtPath.Text = this.Item.Path;
            txtArguments.Text = this.Item.Arguments;
            pgProperties.SelectedObject = this.Item;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    this.Item.Path = txtPath.Text;
                    Item.Arguments = txtArguments.Text;

                    if (!txtPath.ReadOnly && _feature.Items.Any(item => item.Match(this.Item)))
                    {
                        ShowMessage(
                            "This FastCGI application already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    this.DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnBrowse, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    DialogHelper.ShowFileDialog(txtPath, "CGI Executables|*.exe|CGI Files|*.dll|All Files|*.*");
                }));
        }

        public FastCgiItem Item { get; set; }

        private void NewRestrictionDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210483");
        }
    }
}

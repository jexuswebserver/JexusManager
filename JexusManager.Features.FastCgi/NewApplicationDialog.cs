// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewApplicationDialog : DialogForm
    {
        public NewApplicationDialog(IServiceProvider serviceProvider, FastCgiItem existing, FastCgiFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            Text = string.Format("{0} FastCGI Application", existing == null ? "Add" : "Edit");
            txtPath.ReadOnly = txtArguments.ReadOnly = existing != null;
            Item = existing ?? new FastCgiItem(null);
            txtPath.Text = Item.Path;
            txtArguments.Text = Item.Arguments;
            pgProperties.SelectedObject = Item;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Item.Path = txtPath.Text;
                    Item.Arguments = txtArguments.Text;

                    if (!txtPath.ReadOnly && feature.Items.Any(item => item.Match(Item)))
                    {
                        ShowMessage(
                            "This FastCGI application already exists.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPath, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(pgProperties, "PropertyValueChanged"))
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
                    DialogHelper.ShowOpenFileDialog(txtPath, "CGI Executables|*.exe|CGI Files|*.dll|All Files|*.*", null);
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
        public FastCgiItem Item { get; set; }
    }
}

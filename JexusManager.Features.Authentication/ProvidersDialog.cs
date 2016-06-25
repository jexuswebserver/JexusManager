// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Authentication
{
    using System;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;
    using System.Reactive.Disposables;
    public partial class ProvidersDialog : DialogForm
    {
        private readonly string[] _allProviders = { "Negotiate", "NTLM", "Negotiate:Kerberos" };

        public ProvidersDialog(IServiceProvider serviceProvider, WindowsItem item)
            : base(serviceProvider)
        {
            InitializeComponent();
            foreach (var provider in item.Providers)
            {
                lbProviders.Items.Add(provider.Value);
            }

            foreach (var each in _allProviders)
            {
                if (!lbProviders.Items.Contains(each))
                {
                    cbAvailable.Items.Add(each);
                }
            }

            btnOK.Enabled = false;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnRemove, "Click")
                .Subscribe(evt =>
                {
                    cbAvailable.Items.Add(lbProviders.SelectedItem);
                    lbProviders.Items.Remove(lbProviders.SelectedItem);
                    cbAvailable.Enabled = true;
                    btnOK.Enabled = true;
                }));

            var add = Observable.FromEventPattern<EventArgs>(btnAdd, "Click");
            var load = Observable.FromEventPattern<EventArgs>(this, "Load");
            container.Add(
                add.Subscribe(evt =>
                {
                    var selected = cbAvailable.Text;
                    lbProviders.Items.Add(selected);
                    cbAvailable.Items.RemoveAt(cbAvailable.SelectedIndex);
                    cbAvailable.SelectedIndex = -1;
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbAvailable, "SelectedIndexChanged")
                .Merge(load)
                .Merge(add)
                .Subscribe(evt =>
                {
                    btnAdd.Enabled = cbAvailable.SelectedIndex > -1;
                    if (cbAvailable.Items.Count == 0)
                    {
                        cbAvailable.Enabled = false;
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    item.Providers.Clear();
                    foreach (string provider in lbProviders.Items)
                    {
                        item.Providers.Add(new ProviderItem(null) { Value = provider });
                    }

                    item.Apply();
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(lbProviders, "SelectedIndexChanged")
                .Merge(load)
                .Subscribe(evt =>
                {
                    btnRemove.Enabled = lbProviders.SelectedIndex > -1;
                    btnUp.Enabled = lbProviders.SelectedIndex > 0;
                    btnDown.Enabled = lbProviders.SelectedIndex > -1 && lbProviders.SelectedIndex < lbProviders.Items.Count - 1;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnUp, "Click")
                .Subscribe(evt =>
                {
                    var current = lbProviders.SelectedItem;
                    var up = lbProviders.SelectedIndex - 1;
                    lbProviders.Items.RemoveAt(lbProviders.SelectedIndex);
                    lbProviders.Items.Insert(up, current);
                    lbProviders.SelectedIndex = up;
                    btnOK.Enabled = true;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnDown, "Click")
                .Subscribe(evt =>
                {
                    var current = lbProviders.SelectedItem;
                    var down = lbProviders.SelectedIndex + 1;
                    lbProviders.Items.RemoveAt(lbProviders.SelectedIndex);
                    lbProviders.Items.Insert(down, current);
                    lbProviders.SelectedIndex = down;
                    btnOK.Enabled = true;
                }));
        }
    }
}

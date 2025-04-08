// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Dialogs
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;

    public partial class SelectPoolDialog : Form
    {
        public SelectPoolDialog(string name, ServerManager server)
        {
            InitializeComponent();

            int selected = 0;
            foreach (ApplicationPool pool in server.ApplicationPools)
            {
                int index = cbPools.Items.Add(pool);
                if (pool.Name != name)
                {
                    continue;
                }

                selected = index;
                btnOK.Enabled = true;
            }

            if (server.ApplicationPools.Count == 0)
            {
                cbPools.Items.Add(name);
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbPools, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (!(cbPools.SelectedItem is ApplicationPool item))
                    {
                        return;
                    }

                    Selected = item;
                    txtVersion.Text = $".Net CLR Version: {item.ManagedRuntimeVersion.RuntimeVersionToDisplay()}";
                    txtMode.Text = $"Pipeline mode: {item.ManagedPipelineMode}";
                    btnOK.Enabled = true;
                }));

            cbPools.SelectedIndex = selected;
        }

        private void SelectPoolDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210458");
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal ApplicationPool Selected { get; set; }
    }
}

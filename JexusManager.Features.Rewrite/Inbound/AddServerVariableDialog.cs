// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite.Inbound
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class AddServerVariableDialog : DialogForm
    {
        public AddServerVariableDialog(IServiceProvider serviceProvider, ServerVariableItem existing)
            : base(serviceProvider)
        {
            InitializeComponent();
            Item = existing ?? new ServerVariableItem(null);
            cbName.Text = Item.Name;
            txtValue.Text = Item.Value;
            cbReplace.Checked = Item.Replace;
            var service = (IConfigurationService)serviceProvider.GetService(typeof(IConfigurationService));
            var rulesSection = service.GetSection("system.webServer/rewrite/allowedServerVariables");
            ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();
            foreach (ConfigurationElement ruleElement in rulesCollection)
            {
                cbName.Items.Add(ruleElement["name"]);
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtValue, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(cbName, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(cbName.Text) && !string.IsNullOrWhiteSpace(txtValue.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    Item.Name = cbName.Text;
                    Item.Value = txtValue.Text;
                    Item.Replace = cbReplace.Checked;
                    DialogResult = DialogResult.OK;
                }));
        }

        public ServerVariableItem Item { get; set; }

        private void AddServerVariableDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=163108");
        }
    }
}

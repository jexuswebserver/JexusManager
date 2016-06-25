// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.IpSecurity
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class SetRestrictionsDialog : DialogForm
    {
        public SetRestrictionsDialog(IServiceProvider serviceProvider, ConfigurationSection section)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbDomain.Checked = (bool)section["enableReverseDns"];
            cbAccess.SelectedIndex = (bool)section["allowUnlisted"] ? 0 : 1;
            cbProxy.Enabled = section.Schema.AttributeSchemas["enabled"] != null;
            if (cbProxy.Enabled)
            {
                cbProxy.Checked = (bool)section["enableProxyMode"];
            }

            cbAction.Enabled = section.Schema.AttributeSchemas["denyAction"] != null;
            if (cbAction.Enabled)
            {
                var action = (long)section["denyAction"];
                if (action == 0L)
                {
                    cbAction.SelectedIndex = 0;
                }
                else if (action == 401L)
                {
                    cbAction.SelectedIndex = 1;
                }
                else if (action == 403L)
                {
                    cbAction.SelectedIndex = 2;
                }
                else
                {
                    cbAction.SelectedIndex = 3;
                }
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    section["enableReverseDns"] = cbDomain.Checked;
                    section["allowUnlisted"] = cbAccess.SelectedIndex == 0;
                    if (cbProxy.Enabled)
                    {
                        section["enableProxyMode"] = cbProxy.Checked;
                    }

                    if (cbAction.Enabled)
                    {
                        if (cbAction.SelectedIndex == 0)
                        {
                            section["denyAction"] = 0L;
                        }
                        else if (cbAction.SelectedIndex == 1)
                        {
                            section["denyAction"] = 401L;
                        }
                        else if (cbAction.SelectedIndex == 2)
                        {
                            section["denyAction"] = 403L;
                        }
                        else
                        {
                            section["denyAction"] = 404L;
                        }
                    }

                    DialogResult = DialogResult.OK;
                }));
        }

        private void SetRestrictionsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210513");
        }
    }
}

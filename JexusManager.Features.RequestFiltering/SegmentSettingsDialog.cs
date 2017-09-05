// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class SegmentSettingsDialog : DialogForm
    {
        public SegmentSettingsDialog(IServiceProvider serviceProvider, ConfigurationSection section)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbExtension.Checked = (bool)section.ChildElements["fileExtensions"]["allowUnlisted"];
            cbVerb.Checked = (bool)section.ChildElements["verbs"]["allowUnlisted"];
            cbHigh.Checked = (bool)section["allowHighBitCharacters"];
            cbDouble.Checked = (bool)section["allowDoubleEscaping"];
            var limits = section.ChildElements["requestLimits"];
            txtContent.Text = limits["maxAllowedContentLength"].ToString();
            txtURL.Text = limits["maxUrl"].ToString();
            txtQuery.Text = limits["maxQueryString"].ToString();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    section.ChildElements["fileExtensions"]["allowUnlisted"] = cbExtension.Checked;
                    section.ChildElements["verbs"]["allowUnlisted"] = cbVerb.Checked;
                    section["allowHighBitCharacters"] = cbHigh.Checked;
                    section["allowDoubleEscaping"] = cbDouble.Checked;

                    uint result;
                    if (!uint.TryParse(txtContent.Text, out result))
                    {
                        ShowMessage(
                            string.Format(
                                "'{0}' is an invalid maximum {1}. Enter an integer between 0 and 4294967295",
                                txtContent.Text,
                                "content length"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        txtContent.SelectAll();
                        return;
                    }

                    limits["maxAllowedContentLength"] = result;

                    if (!uint.TryParse(txtURL.Text, out result))
                    {
                        ShowMessage(
                            string.Format(
                                "'{0}' is an invalid maximum {1}. Enter an integer between 0 and 4294967295",
                                txtURL.Text,
                                "URL length"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        txtURL.SelectAll();
                        return;
                    }

                    limits["maxUrl"] = result;

                    if (!uint.TryParse(txtQuery.Text, out result))
                    {
                        ShowMessage(
                            string.Format(
                                "'{0}' is an invalid maximum {1}. Enter an integer between 0 and 4294967295",
                                txtQuery.Text,
                                "query string length"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        txtQuery.SelectAll();
                        return;
                    }

                    limits["maxQueryString"] = result;
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtContent, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtQuery, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtURL, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtContent.Text) &&
                                                !string.IsNullOrWhiteSpace(txtURL.Text) &&
                                                !string.IsNullOrWhiteSpace(txtQuery.Text);
                }));
        }

        private void SegmentSettingsDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210526#Edit_Filtering");
        }
    }
}

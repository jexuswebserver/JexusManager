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
        public SegmentSettingsDialog(IServiceProvider serviceProvider, RequestFilteringSettings settings)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbExtension.Checked = settings.FileExtensionsAllowUnlisted;
            cbVerb.Checked = settings.VerbsAllowUnlisted;
            cbHigh.Checked = settings.AllowHighBitCharacters;
            cbDouble.Checked = settings.AllowDoubleEscaping;
            txtContent.Text = settings.MaxAllowedContentLength.ToString();
            txtURL.Text = settings.MaxUrl.ToString();
            txtQuery.Text = settings.MaxQueryString.ToString();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    settings.FileExtensionsAllowUnlisted = cbExtension.Checked;
                    settings.VerbsAllowUnlisted = cbVerb.Checked;
                    settings.AllowHighBitCharacters = cbHigh.Checked;
                    settings.AllowDoubleEscaping = cbDouble.Checked;

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

                    settings.MaxAllowedContentLength = result;

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

                    settings.MaxUrl = result;

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

                    settings.MaxQueryString = result;
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtContent, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtQuery, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtURL, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(0.5))
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

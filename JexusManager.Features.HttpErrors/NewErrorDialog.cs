// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.HttpErrors
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    internal sealed partial class NewErrorDialog : DialogForm
    {
        public NewErrorDialog(IServiceProvider serviceProvider, HttpErrorsItem item, HttpErrorsFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtStatusCode.ReadOnly = item != null;
            if (item != null)
            {
                txtStatusCode.Text = item.Code;
            }

            Text = item == null ? "Add Custom Error Page" : "Edit Custom Error Page";
            Item = item ?? new HttpErrorsItem(null);
            var mode = Item.Response;
            rbStatic.Checked = mode == "File";
            rbExecute.Checked = mode == "ExecuteURL";
            rbRedirect.Checked = mode == "Redirect";
            UpdateUI(Item);

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtStatusCode, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(txtRedirect, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtExecute, "TextChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(txtStatic, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtStatusCode.Text) &&
                        !(string.IsNullOrWhiteSpace(txtExecute.Text) && string.IsNullOrWhiteSpace(txtStatic.Text) && string.IsNullOrWhiteSpace(txtRedirect.Text));
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var index = txtStatusCode.Text.IndexOf('.');
                    if (index == -1)
                    {
                        int statusCode;
                        var statusParsed = int.TryParse(txtStatusCode.Text, out statusCode);
                        if (!statusParsed || statusCode < 100 || statusCode > 999)
                        {
                            ShowMessage(
                                string.Format(
                                    "'{0}' is an invalid status code. Status codes must be numbers in the form of 400 or 400.1. Status codes must be between 100 and 999. Sub-status codes must be between 1 and 999.",
                                    txtStatusCode.Text),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        Item.Status = uint.Parse(txtStatusCode.Text);
                        Item.Substatus = -1;
                    }
                    else
                    {
                        var status = index == 0 ? string.Empty : txtStatusCode.Text.Substring(0, index);
                        var substatus = index == txtStatusCode.Text.Length - 1 ? string.Empty : txtStatusCode.Text.Substring(index + 1);
                        uint statusCode;
                        var statusParsed = uint.TryParse(status, out statusCode);
                        int substatusCode;
                        var substatusParsed = int.TryParse(substatus, out substatusCode);
                        if (!statusParsed || statusCode < 100 || statusCode > 999 || !substatusParsed || substatusCode < 1 ||
                            substatusCode > 999)
                        {
                            ShowMessage(
                                string.Format(
                                    "'{0}' is an invalid status code. Status codes must be numbers in the form of 400 or 400.1. Status codes must be between 100 and 999. Sub-status codes must be between 1 and 999.",
                                    txtStatusCode.Text),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        Item.Status = statusCode;
                        Item.Substatus = substatusCode;
                    }

                    if (Item.Response == "Redirect")
                    {
                        Item.Path = txtRedirect.Text;
                    }
                    else if (Item.Response == "ExecuteURL")
                    {
                        if (!Item.Path.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                        {
                            ShowMessage(
                                "The specific URL is invalid. It must be a relative URL path that begins with \"/\"",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        Item.Path = txtExecute.Text;
                    }
                    else
                    {
                        Item.Path = txtStatic.Text;
                    }

                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(rbRedirect, "CheckedChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(rbExecute, "CheckedChanged"))
                .Merge(Observable.FromEventPattern<EventArgs>(rbStatic, "CheckedChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (rbStatic.Checked)
                    {
                        Item.Response = "File";
                    }
                    else if (rbExecute.Checked)
                    {
                        Item.Response = "ExecuteURL";
                    }
                    else
                    {
                        Item.Response = "Redirect";
                    }

                    UpdateUI(Item);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnSet, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (cbLocalize.Checked)
                    {
                        var dialog = new LocalErrorDialog(ServiceProvider, Item);
                        if (dialog.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        txtStatic.Text = Item.FullPath;
                    }
                    else
                    {
                        DialogHelper.ShowFileDialog(txtStatic, string.Empty);
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbLocalize, "CheckedChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnSet.Text = cbLocalize.Checked ? "Set..." : "Browse...";
                    txtStatic.Text = string.Empty;
                    txtStatic.ReadOnly = cbLocalize.Checked;
                }));
        }

        private void UpdateUI(HttpErrorsItem item)
        {
            cbLocalize.Checked = !string.IsNullOrEmpty(item.Prefix);
            var mode = item.Response;
            if (mode == "File")
            {
                txtStatic.Text = string.IsNullOrEmpty(item.Prefix) ? item.Path : item.FullPath;
                txtStatic.ReadOnly = cbLocalize.Checked;
                txtExecute.Text = string.Empty;
                txtRedirect.Text = string.Empty;
            }
            else if (mode == "ExecuteURL")
            {
                txtStatic.Text = string.Empty;
                txtExecute.Text = Item.Path;
                txtRedirect.Text = string.Empty;
            }
            else
            {
                txtStatic.Text = string.Empty;
                txtExecute.Text = string.Empty;
                txtRedirect.Text = Item.Path;
            }

            txtExecute.Enabled = mode == "ExecuteURL";
            txtRedirect.Enabled = mode == "Redirect";
            cbLocalize.Enabled = btnSet.Enabled = txtStatic.Enabled = mode == "File";
        }

        public HttpErrorsItem Item { get; set; }

        private void NewCustomErrorDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210481");
        }
    }
}

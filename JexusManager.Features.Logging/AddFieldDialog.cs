// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Logging
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class AddFieldDialog : DialogForm
    {
        public CustomLogField Custom { get; private set; }

        public AddFieldDialog(IServiceProvider serviceProvider, CustomLogField custom, Fields logFile)
            : base(serviceProvider)
        {
            Custom = custom;
            InitializeComponent();

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(this, "Load")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt => 
                {
                    if (custom != null)
                    {
                        txtName.Text = custom.LogFieldName;
                        cbType.SelectedIndex = (int)custom.SourceType;
                        cbSource.Text = custom.SourceName;
                    }
                    else
                    {
                        cbType.SelectedIndex = 0;
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtName, "TextChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(cbSource, "TextChanged"))
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(cbSource.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbType, "SelectedIndexChanged")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    cbSource.Items.Clear();
                    if (cbType.SelectedIndex == 0)
                    {
                        cbSource.Items.AddRange(new object[]
                        {
                    "Accept",
                    "Accept_Charset",
                    "Accept_Encoding",
                    "Authorization",
                    "Cache-Control",
                    "Connection",
                    "Content-Length",
                    "Content-MD5",
                    "Content-Type",
                    "Date",
                    "Expect",
                    "From",
                    "Host",
                    "If-Match",
                    "If-Modified-Since",
                    "If-None-Match",
                    "If-Range",
                    "If-Unmodified-Since",
                    "Max-Forwards",
                    "Pragma",
                    "Proxy-Authorization",
                    "Range",
                    "Referer",
                    "TE",
                    "Upgrade",
                    "User-Agent",
                    "Via",
                    "Warning"
                        });
                    }
                    else if (cbType.SelectedIndex == 1)
                    {
                        cbSource.Items.AddRange(new object[]
                        {
                    "Accept-Range",
                    "Content-Type",
                    "ETag",
                    "Last-Modified",
                    "Server"
                        });
                    }
                    else if (cbType.SelectedIndex == 2)
                    {
                        cbSource.Items.AddRange(DialogHelper.Conditions);
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var type = (CustomLogFieldSourceType)Enum.ToObject(typeof(CustomLogFieldSourceType), cbType.SelectedIndex);
                    if (Custom == null)
                    {
                        Custom = logFile.Element.CustomLogFields.CreateElement();
                        logFile.CustomLogFields.Add(Custom);
                    }

                    Custom.LogFieldName = txtName.Text;
                    Custom.SourceType = type;
                    Custom.SourceName = cbSource.Text;

                    DialogResult = DialogResult.OK;
                }));
        }

        private void AddFieldDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210517#W3CLoggingFields");
        }
    }
}

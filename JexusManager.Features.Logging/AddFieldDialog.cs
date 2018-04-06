// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Logging
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    public partial class AddFieldDialog : DialogForm
    {
        public CustomLogField Custom { get; private set; }

        public AddFieldDialog(IServiceProvider serviceProvider, CustomLogField custom, SiteLogFile logFile)
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
                        cbSource.Items.AddRange(new object[]
                            {
                        "ALL_HTTP",
                        "ALL_RAW",
                        "APPL_MD_PATH",
                        "APPL_PHYSICAL_PATH",
                        "AUTH_PASSWORD",
                        "AUTH_TYPE",
                        "AUTH_USER",
                        "CERT_COOKIE",
                        "CERT_FLAGS",
                        "CERT_ISSUER",
                        "CERT_KEYSIZE",
                        "CERT_SECRETKEYSIZE",
                        "CERT_SERIALNUMBER",
                        "CERT_SERVER_ISSUER",
                        "CERT_SERVER_SUBJECT",
                        "CERT_SUBJECT",
                        "CONTENT_LENGTH",
                        "CONTENT_TYPE",
                        "GATEWAY_INTERFACE",
                        "HTTP_ACCEPT",
                        "HTTP_ACCEPT_ENCODING",
                        "HTTP_ACCEPT_LANGUAGE",
                        "HTTP_CONNECTION",
                        "HTTP_COOKIE",
                        "HTTP_HOST",
                        "HTTP_METHOD",
                        "HTTP_REFERER",
                        "HTTP_URL",
                        "HTTP_USER_AGENT",
                        "HTTP_VERSION",
                        "HTTPS",
                        "HTTPS_KEYSIZE",
                        "HTTPS_SECRETKEYSIZE",
                        "HTTPS_SERVER_ISSUER",
                        "HTTPS_SERVER_SUBJECT",
                        "INSTANCE_ID",
                        "INSTANCE_META_PATH",
                        "LOCAL_ADDR",
                        "LOGON_USER",
                        "PATH_INFO",
                        "PATH_TRANSLATED",
                        "QUERY_STRING",
                        "REMOTE_ADDR",
                        "REMOTE_HOST",
                        "REMOTE_PORT",
                        "REMOTE_USER",
                        "REQUEST_METHOD",
                        "SCRIPT_NAME",
                        "SERVER_NAME",
                        "SERVER_PORT",
                        "SERVER_PORT_SECURE",
                        "SERVER_PROTOCOL",
                        "SERVER_SOFTWARE",
                        "UNMAPPED_REMOTE_USER",
                        "URL"
                            });
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (Custom == null)
                    {
                        Custom = logFile.CustomLogFields.CreateElement();
                    }

                    Custom.LogFieldName = txtName.Text;
                    Custom.SourceType = (CustomLogFieldSourceType)Enum.ToObject(typeof(CustomLogFieldSourceType), cbType.SelectedIndex);
                    Custom.SourceName = cbSource.Text;
                    if (Custom == null)
                    {
                        logFile.CustomLogFields.Add(Custom);
                    }

                    DialogResult = DialogResult.OK;
                }));
        }

        private void AddFieldDialog_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkId=210517#W3CLoggingFields");
        }
    }
}

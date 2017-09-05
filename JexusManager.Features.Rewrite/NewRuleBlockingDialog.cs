// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// --------------------------------------------------------------------------------------------------------------------
//   
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace JexusManager.Features.Rewrite
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using JexusManager.Features.Rewrite.Inbound;

    using Microsoft.Web.Management.Client.Win32;

    internal partial class NewRuleBlockingDialog : DialogForm
    {
        public NewRuleBlockingDialog(IServiceProvider serviceProvider, InboundFeature rewriteFeature)
            : base(serviceProvider)
        {
            InitializeComponent();
            cbMode.SelectedIndex = 1;
            cbInput.SelectedIndex = 0;
            cbMatch.SelectedIndex = 0;
            cbResponse.SelectedIndex = 1;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbInput, "SelectedIndexChanged")
                .Merge(Observable.FromEventPattern<EventArgs>(cbMode, "SelectedIndexChanged"))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    switch (cbInput.SelectedIndex)
                    {
                        case 0:
                            lblPattern.Text = "Pattern (URL Path):";
                            lblExample.Text = cbMode.SelectedIndex == 1 ? "Example: IMG*.jpg" : "Example: ^IMG.*\\.jpg$";
                            break;
                        case 1:
                            lblPattern.Text = "Pattern (User-agent Header):";
                            lblExample.Text = cbMode.SelectedIndex == 1 ? "Example: Mozilla/4*" : "Example: ^Mozilla/[1234].*";
                            break;
                        case 2:
                            lblPattern.Text = "Pattern (IP Address):";
                            lblExample.Text = cbMode.SelectedIndex == 1 ? "Exmaple: 192.168.1.*" : "Example: 192\\.168\\.1\\.[1-9]";
                            break;
                        case 3:
                            lblPattern.Text = "Pattern (Query String):";
                            lblExample.Text = cbMode.SelectedIndex == 1 ? "Example: *id=*&p=*" : "Example: id=[0-9]+&p=[a-z]+";
                            break;
                        case 4:
                            lblPattern.Text = "Pattern (Referer):";
                            lblExample.Text = cbMode.SelectedIndex == 1 ? "Example: http://*.consoto.com" : "Example: http://(?:www\\.)?contoso\\.com$";
                            break;
                        case 5:
                            lblPattern.Text = "Pattern (Host Header):";
                            lblExample.Text = cbMode.SelectedIndex == 1 ? "Example: *.consoto.com" : "Example: (?:www\\.)?contoso\\.com$";
                            break;
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPattern, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnOK.Enabled = !string.IsNullOrWhiteSpace(txtPattern.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    var inputType = cbInput.SelectedIndex;
                    var pattern = txtPattern.Text;
                    var match = cbMatch.SelectedIndex;
                    var response = cbResponse.SelectedIndex;
                    var mode = cbMode.SelectedIndex;

                    int index = 0;
                    string name;
                    do
                    {
                        index++;
                        name = string.Format("RequestBlockingRule{0}", index);
                    }
                    while (rewriteFeature.Items.All(item => item.Name != name));
                    var newRule = new InboundRule(null);
                    newRule.Name = name;
                    newRule.Input = "URL Path";
                    newRule.Enabled = true;
                    newRule.PatternSyntax = mode == 0 ? 0L : 1L;
                    newRule.PatternUrl = mode == 0 ? ".*" : "*";
                    newRule.Type = response == 3 ? 4L : 3L;
                    newRule.ActionUrl = "{C:1}";
                    newRule.RedirectType = 301;
                    newRule.StatusCode = GetStatusCode(response);
                    newRule.SubStatusCode = 0;
                    newRule.StatusReason = GetReason(response);
                    newRule.StatusDescription = GetMessage(response);
                    newRule.Conditions.Add(
                        new ConditionItem(null)
                        {
                            Input = GetInput(inputType),
                            MatchType = match == 0 ? 4 : 5,
                            Pattern = pattern,
                            IgnoreCase = true
                        });
                    rewriteFeature.AddItem(newRule);
                    DialogResult = DialogResult.OK;
                }));
        }

        private void NewRuleBlockingHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130412&amp;clcid=0x409");
        }

        private string GetInput(int inputType)
        {
            switch (inputType)
            {
                case 0:
                    return "{URL}";
                case 1:
                    return "{HTTP_USER_AGENT}";
                case 2:
                    return "{REMOTE_ADDR}";
                case 3:
                    return "{QUERY_STRING}";
                case 4:
                    return "{HTTP_REFERER}";
                case 5:
                    return "{HTTP_HOST}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType));
            }
        }

        private string GetMessage(int response)
        {
            return response == 0
                       ? UnauthorizedText
                       : response == 1 ? ForbiddenText : response == 2 ? FileNotFoundText : string.Empty;
        }

        private string GetReason(int response)
        {
            return response == 0
                       ? "Unauthorized: Access is denied due to invalid credentials"
                       : response == 1
                             ? "Forbidden: Access is denied."
                             : response == 2 ? "File or directory not found." : string.Empty;
        }

        private uint GetStatusCode(int response)
        {
            return response == 0 ? 401U : response == 1 ? 403U : response == 2 ? 404U : 0U;
        }

        private const string UnauthorizedText = "You do not have permission to view this directory or page using the credentials that you supplied.";

        private const string ForbiddenText =
            "You do not have permission to view this directory or page using the credentials that you supplied.";

        private const string FileNotFoundText = "The resource you are looking for might have been removed, had its name changed, or is temporarily unavailable.";
    }
}

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
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class NewRuleWithRewriteMapsDialog : DialogForm
    {
        public NewRuleWithRewriteMapsDialog(IServiceProvider serviceProvider, InboundFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            var rulesSection = service.GetSection("system.webServer/rewrite/rewriteMaps");
            ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();
            foreach (ConfigurationElement ruleElement in rulesCollection)
            {
                cbMap.Items.Add(ruleElement["name"]);
            }

            cbAction.SelectedIndex = 0;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbMap, "SelectedIndexChanged")
                .Subscribe(evt =>
                {
                    btnOK.Enabled = cbMap.SelectedIndex > -1;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnOK, "Click")
                .Subscribe(evt =>
                {
                    var action = cbAction.Text;
                    var map = cbMap.Text;

                    int index = 0;
                    string name;
                    do
                    {
                        index++;
                        name = string.Format("{0} rule{1} for {2}", action, index, map);
                    }
                    while (feature.Items.All(item => item.Name != name));
                    var rule = new InboundRule(null);
                    rule.Name = name;
                    rule.Input = "URL Path";
                    rule.PatternSyntax = 0L;
                    rule.PatternUrl = ".*";
                    rule.Type = action == "Rewrite" ? 1L : 2L;
                    rule.ActionUrl = "{C:1}";
                    rule.RedirectType = 301;
                    rule.Conditions.Add(
                        new ConditionItem(null)
                        {
                            Input = string.Format("{{{0}:{{REQUEST_URI}}}}", map),
                            MatchType = 4,
                            Pattern = "(.+)",
                            IgnoreCase = true
                        });

                    feature.AddItem(rule);
                    DialogResult = DialogResult.OK;
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(cbAction, "SelectedIndexChanged")
                .Subscribe(evt =>
                {
                    lblDescription.Text = cbAction.SelectedIndex == 0 ? s_rewriteText : s_redirectText;
                }));
        }

        private void NewRuleWithRewriteMapsDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkID=130413&amp;clcid=0x409");
        }

        private static readonly string s_rewriteText =
            "The map for this rule should define a set of static mappings between" + Environment.NewLine
            + "original URLs and rewritten URLs." + Environment.NewLine + Environment.NewLine
            + "For example, the map can contain an original URL as:" + Environment.NewLine + Environment.NewLine
            + "/home" + Environment.NewLine + Environment.NewLine + "and the corresponding rewritten URL as:"
            + Environment.NewLine + Environment.NewLine + "/default.aspx? id = 1";

        private static readonly string s_redirectText =
            "The map for this rule should define a set of static mappings between" + Environment.NewLine
            + "requested URLs and redirection URLs." + Environment.NewLine + Environment.NewLine
            + "For example, the map can contain a requested URL as:" + Environment.NewLine + Environment.NewLine
            + "/default.aspx?tabid=1&&id=234" + Environment.NewLine + Environment.NewLine
            + "and the corresponding redirection URL as:" + Environment.NewLine + Environment.NewLine
            + "/about/contact, when redirecting within the same domain, or" + Environment.NewLine
            + "http://contoso.com/about/contact, when redirecting across domains.";
    }
}

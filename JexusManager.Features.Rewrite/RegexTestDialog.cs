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
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using Properties;

    using Microsoft.Web.Management.Client.Win32;

    public partial class RegexTestDialog : DialogForm
    {
        private const string Failed = "The input data to test does not match the pattern.";
        private const string Hint = "Specify the input data to test, and then click \"Test\" to see the results.";
        private const string Succeeded = "The input URL path matches the pattern.";
        private readonly string _pattern;
        private readonly bool _ignore;

        private readonly bool _condition;

        public RegexTestDialog(IServiceProvider serviceProvider, string pattern, bool ignoreCase, bool condition)
            : base(serviceProvider)
        {
            InitializeComponent();
            txtPattern.Text = pattern;
            cbIgnoreCase.Checked = ignoreCase;
            _pattern = pattern;
            _ignore = ignoreCase;
            _condition = condition;
            txtMessage.Text = Hint;
            pbMessage.Image = Resources.info_16;

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<EventArgs>(txtPattern, "TextChanged")
                .Sample(TimeSpan.FromSeconds(1))
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    btnTest.Enabled = !string.IsNullOrWhiteSpace(txtPattern.Text);
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnTest, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    Regex expression = null;
                    try
                    {
                        expression = new Regex(
                            txtPattern.Text,
                            cbIgnoreCase.Checked ? RegexOptions.IgnoreCase : RegexOptions.None);
                    }
                    catch (ArgumentException ex)
                    {
                        // TODO: find a way to generate better error message.
                        ShowMessage(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }

                    bool success = false;
                    Match result = null;
                    if (expression != null)
                    {
                        result = expression.Match(txtData.Text);
                        success = result.Success;
                    }

                    pbMessage.Image = success ? Resources.tick_16 : Resources.error_16;
                    txtMessage.Text = success ? Succeeded : Failed;
                    txtTitle.Visible = success;
                    lvResults.Visible = success;
                    if (success)
                    {
                        lvResults.Items.Clear();
                        var count = 0;
                        foreach (Group group in result.Groups)
                        {
                            lvResults.Items.Add(new ListViewItem(new[]
                            {
                                string.Format(_condition ? "{{C:{0}}}" : "{{R:{0}}}", count++),
                                group.Value
                            }));
                        }
                    }
                }));

            container.Add(
                Observable.FromEventPattern<EventArgs>(btnClose, "Click")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(evt =>
                {
                    if (txtPattern.Text == _pattern && cbIgnoreCase.Checked == _ignore)
                    {
                        return;
                    }

                    var result = MessageBox.Show("The pattern configuration has been changed. Do you want to save these changes?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DialogResult = DialogResult.OK;
                    }
                    else if (result == DialogResult.No)
                    {
                        return;
                    }

                    DialogResult = DialogResult.None;
                }));
        }

        public bool IgnoreCase
        {
            get { return cbIgnoreCase.Checked; }
        }

        public string Pattern
        {
            get { return txtPattern.Text; }
        }

        private void RegexTestDialogHelpButtonClicked(object sender, CancelEventArgs e)
        {
            DialogHelper.ProcessStart("http://go.microsoft.com/fwlink/?LinkID=130409&amp;clcid=0x409");
        }
    }
}

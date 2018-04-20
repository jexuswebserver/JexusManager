// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.ResponseHeaders
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Forms;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client.Win32;

    internal partial class SetCommonHeadersDialog : DialogForm
    {
        private readonly ConfigurationSection _httpProtocolSection;
        private readonly ConfigurationElement _staticContent;

        public SetCommonHeadersDialog(IServiceProvider serviceProvider, ConfigurationSection httpProtocolSection, ConfigurationElement staticContent, ResponseHeadersFeature feature)
            : base(serviceProvider)
        {
            InitializeComponent();
            _httpProtocolSection = httpProtocolSection;
            _staticContent = staticContent;
            cbKeepAlive.Checked = (bool)httpProtocolSection["allowKeepAlive"];
            var mode = (long)_staticContent["cacheControlMode"];
            cbExpired.Checked = mode != 0L;
            rbImmediate.Checked = mode == 1L;
            rbAfter.Checked = mode == 2L;
            rbTime.Checked = mode == 3L;
            string time = (string)_staticContent["httpExpires"];
            DateTime output;
            if (DateTime.TryParse(time, out output))
            {
                dtpDate.Value = output;
                dtpTime.Value = output;
            }

            var span = (TimeSpan)_staticContent["cacheControlMaxAge"];
            if (span.Seconds == 0)
            {
                if (span.Minutes == 0)
                {
                    if (span.Hours == 0)
                    {
                        cbUnit.SelectedIndex = 3;
                        txtAfter.Text = span.Days.ToString();
                    }
                    else
                    {
                        cbUnit.SelectedIndex = 2;
                        txtAfter.Text = (span.Hours + span.Days * 24).ToString();
                    }
                }
                else
                {
                    cbUnit.SelectedIndex = 1;
                    txtAfter.Text = (span.Minutes + (span.Hours + span.Days * 24) * 60).ToString();
                }
            }
            else
            {
                cbUnit.SelectedIndex = 0;
                txtAfter.Text = (span.Seconds + (span.Minutes + (span.Hours + span.Days * 24) * 60) * 60).ToString();
            }

            var container = new CompositeDisposable();
            FormClosed += (sender, args) => container.Dispose();

            container.Add(
                Observable.FromEventPattern<CancelEventArgs>(this, "HelpButtonClicked")
                .ObserveOn(System.Threading.SynchronizationContext.Current)
                .Subscribe(EnvironmentVariableTarget =>
                {
                    feature.ShowHelp();
                }));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _httpProtocolSection["allowKeepAlive"] = cbKeepAlive.Checked;
            if (cbExpired.Checked)
            {
                if (rbImmediate.Checked)
                {
                    _staticContent["cacheControlMode"] = 1L;
                }
                else if (rbAfter.Checked)
                {
                    _staticContent["cacheControlMode"] = 2L;
                    long value;
                    if (!long.TryParse(txtAfter.Text, out value) || value < 0)
                    {
                        ShowMessage(
                            GetMessage(cbUnit.SelectedIndex),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (cbUnit.SelectedIndex == 0)
                    {
                        if (value > GetMax(cbUnit.SelectedIndex))
                        {
                            ShowMessage(
                                GetMessage(cbUnit.SelectedIndex),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        _staticContent["cacheControlMaxAge"] = TimeSpan.FromSeconds(value);
                    }
                    else if (cbUnit.SelectedIndex == 1)
                    {
                        if (value > GetMax(cbUnit.SelectedIndex))
                        {
                            ShowMessage(
                                GetMessage(cbUnit.SelectedIndex),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        _staticContent["cacheControlMaxAge"] = TimeSpan.FromMinutes(value);
                    }
                    else if (cbUnit.SelectedIndex == 2)
                    {
                        if (value > GetMax(cbUnit.SelectedIndex))
                        {
                            ShowMessage(
                                GetMessage(cbUnit.SelectedIndex),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        _staticContent["cacheControlMaxAge"] = TimeSpan.FromHours(value);
                    }
                    else
                    {
                        if (value > GetMax(cbUnit.SelectedIndex))
                        {
                            ShowMessage(
                                GetMessage(cbUnit.SelectedIndex),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button1);
                            return;
                        }

                        _staticContent["cacheControlMaxAge"] = TimeSpan.FromDays(value);
                    }
                }
                else
                {
                    _staticContent["cacheControlMode"] = 3L;
                    // Wed, 27 May 2015 00:00:00 GMT
                    _staticContent["httpExpires"] = (dtpDate.Value.Date + dtpTime.Value.TimeOfDay).ToString("R");
                }
            }
            else
            {
                _staticContent["cacheControlMode"] = 0L;
            }

            DialogResult = DialogResult.OK;
        }

        private string GetMessage(int selectedIndex)
        {
            return string.Format(
                "The specified expiration value is invalid. The valid range is between 0 and {0} {1}.", GetMax(selectedIndex), cbUnit.Text);
        }

        private readonly long[] _max = { 922337203686, 15372286728, 256204778, 10675199 };

        private long GetMax(int index)
        {
            return _max[index];
        }

        private void cbExpired_CheckedChanged(object sender, EventArgs e)
        {
            rbImmediate.Enabled = cbExpired.Checked;
            rbAfter.Enabled = cbExpired.Checked;
            rbTime.Enabled = cbExpired.Checked;
            rbAfter_CheckedChanged(sender, e);
            rbTime_CheckedChanged(sender, e);
        }

        private void rbAfter_CheckedChanged(object sender, EventArgs e)
        {
            txtAfter.Enabled = rbAfter.Checked && rbAfter.Enabled;
            cbUnit.Enabled = rbAfter.Checked && rbAfter.Enabled;
        }

        private void rbTime_CheckedChanged(object sender, EventArgs e)
        {
            dtpDate.Enabled = rbTime.Checked && rbTime.Enabled;
            dtpTime.Enabled = rbTime.Checked && rbTime.Enabled;
        }
    }
}

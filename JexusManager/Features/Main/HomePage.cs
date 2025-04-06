// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.Main
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    public partial class HomePage : ModulePage
    {
        private UpdateHelper.UpdateInfo _updateInfo;

        public HomePage()
        {
            InitializeComponent();
            LoadUpdateInfo();
        }

        private async void LoadUpdateInfo()
        {
            try
            {
                // Update UI to show checking state
                lblUpdateStatus.Text = "Checking for updates...";
                lblUpdateStatus.ForeColor = Color.Black;
                btnDownloadUpdate.Visible = false;
                btnRetry.Visible = false;
                
                _updateInfo = await UpdateHelper.CheckForUpdate();
                UpdateVersionDisplay();
            }
            catch (Exception ex)
            {
                lblUpdateStatus.Text = $"Error checking for updates: {ex.Message}";
                lblUpdateStatus.ForeColor = Color.Red;
                btnRetry.Visible = true;
            }
        }

        private void UpdateVersionDisplay()
        {
            if (_updateInfo == null)
                return;

            // Always show current version
            lblCurrentVersion.Text = $"Current Version: {_updateInfo.CurrentVersion}";

            if (!string.IsNullOrEmpty(_updateInfo.ErrorMessage))
            {
                if (_updateInfo.ErrorType == UpdateHelper.UpdateErrorType.ConnectionError)
                {
                    // Create a more helpful message for connectivity issues
                    lblUpdateStatus.Text = "Cannot connect to GitHub. You can check for updates manually at:";
                    lblUpdateStatus.ForeColor = Color.Red;
                    
                    // Show link label for manual update check
                    lblManualUpdate.Text = _updateInfo.ReleaseUrl;
                    lblManualUpdate.Visible = true;
                }
                else
                {
                    lblUpdateStatus.Text = _updateInfo.ErrorMessage;
                    lblUpdateStatus.ForeColor = Color.Red;
                    lblManualUpdate.Visible = false;
                }
                
                btnDownloadUpdate.Visible = false;
                btnRetry.Visible = true;
                return;
            }

            // Reset manual update link visibility
            lblManualUpdate.Visible = false;

            if (_updateInfo.UpdateAvailable)
            {
                lblUpdateStatus.Text = $"An update is available: {_updateInfo.LatestVersion}";
                lblUpdateStatus.ForeColor = Color.Green;
                btnDownloadUpdate.Visible = true;
                btnRetry.Visible = false;
            }
            else
            {
                lblUpdateStatus.Text = "You are using the latest version.";
                lblUpdateStatus.ForeColor = Color.Green;
                btnDownloadUpdate.Visible = false;
                btnRetry.Visible = false;
            }
        }

        private void txtHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogHelper.ProcessStart("https://www.jexusmanager.com");
        }

        protected override bool ShowTaskList => false;

        private void txtStudio_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogHelper.ProcessStart("https://halfblood.pro");
        }

        private void btnSponsor_Click(object sender, System.EventArgs e)
        {
            DialogHelper.ProcessStart("https://github.com/sponsors/lextm");
        }

        private void btnDownloadUpdate_Click(object sender, EventArgs e)
        {
            if (_updateInfo != null && !string.IsNullOrEmpty(_updateInfo.ReleaseUrl))
            {
                DialogHelper.ProcessStart(_updateInfo.ReleaseUrl);
            }
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            // Hide the manual update link when retrying
            lblManualUpdate.Visible = false;
            
            // Reload update info
            LoadUpdateInfo();
        }

        private void lblManualUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_updateInfo?.ReleaseUrl))
            {
                DialogHelper.ProcessStart(_updateInfo.ReleaseUrl);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Configure ManualUpdate link label text wrapping
            ConfigureManualUpdateLink();
        }

        private void ConfigureManualUpdateLink()
        {
            // Enable text wrapping for the manual update link
            lblManualUpdate.MaximumSize = new System.Drawing.Size(
                groupBox2.Width - lblManualUpdate.Left - 30, 0);
            lblManualUpdate.AutoSize = true;
            
            // Ensure the link resizes when the container resizes
            groupBox2.SizeChanged += (sender, e) => 
            {
                lblManualUpdate.MaximumSize = new System.Drawing.Size(
                    groupBox2.Width - lblManualUpdate.Left - 30, 0);
            };
        }
    }
}

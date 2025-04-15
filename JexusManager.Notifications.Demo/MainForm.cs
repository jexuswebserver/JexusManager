using System;
using System.Windows.Forms;

namespace JexusManager.Notifications.Demo
{
    public partial class MainForm : Form
    {
        private NotificationManager _notificationManager;

        public MainForm()
        {
            InitializeComponent();
            
            // Create notification manager with bottom right position
            _notificationManager = NotificationManager.GetInstance(this, NotificationManager.NotificationPosition.BottomRight);
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            ShowNotification("Information", "This is an information notification.", 
                NotificationControl.NotificationType.Info);
        }

        private void btnSuccess_Click(object sender, EventArgs e)
        {
            ShowNotification("Success", "The operation completed successfully.", 
                NotificationControl.NotificationType.Success);
        }

        private void btnWarning_Click(object sender, EventArgs e)
        {
            ShowNotification("Warning", "Please be careful with this operation.", 
                NotificationControl.NotificationType.Warning);
        }

        private void btnError_Click(object sender, EventArgs e)
        {
            ShowNotification("Error", "An error occurred while performing the operation.", 
                NotificationControl.NotificationType.Error);
        }

        private void btnCustomMessage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTitle.Text) && !string.IsNullOrEmpty(txtMessage.Text))
            {
                ShowNotification(txtTitle.Text, txtMessage.Text, GetSelectedNotificationType());
            }
            else
            {
                MessageBox.Show("Please enter both a title and a message.", "Input Required", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnChangePosition_Click(object sender, EventArgs e)
        {
            // Create new notification manager with selected position
            _notificationManager = NotificationManager.GetInstance(this, GetSelectedPosition());
            lblCurrentPosition.Text = $"Current Position: {GetSelectedPosition()}";
        }

        private void btnDismissAll_Click(object sender, EventArgs e)
        {
            _notificationManager.DismissAll();
        }

        private NotificationControl.NotificationType GetSelectedNotificationType()
        {
            if (rbSuccess.Checked)
                return NotificationControl.NotificationType.Success;
            if (rbWarning.Checked)
                return NotificationControl.NotificationType.Warning;
            if (rbError.Checked)
                return NotificationControl.NotificationType.Error;
            
            return NotificationControl.NotificationType.Info;
        }

        private NotificationManager.NotificationPosition GetSelectedPosition()
        {
            if (rbBottomLeft.Checked)
                return NotificationManager.NotificationPosition.BottomLeft;
            if (rbTopRight.Checked)
                return NotificationManager.NotificationPosition.TopRight;
            if (rbTopLeft.Checked)
                return NotificationManager.NotificationPosition.TopLeft;
            if (rbBottomCenter.Checked)
                return NotificationManager.NotificationPosition.BottomCenter;
            
            return NotificationManager.NotificationPosition.BottomRight;
        }

        private void ShowNotification(string title, string message, NotificationControl.NotificationType type)
        {
            int duration = (int)numDuration.Value * 1000;
            int fade = (int)numFadeDuration.Value;
            
            _notificationManager.Show(title, message, type, duration, fade, () => 
            {
                if (chkShowClickMessage.Checked)
                {
                    MessageBox.Show($"You clicked a notification: {title}", "Notification Clicked", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            });
        }
    }
}
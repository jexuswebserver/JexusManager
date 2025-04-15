using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace JexusManager.Notifications
{
    /// <summary>
    /// Manager class for displaying multiple notifications
    /// </summary>
    public class NotificationManager
    {
        private static readonly Dictionary<Form, NotificationManager> Instances = new Dictionary<Form, NotificationManager>();
        private readonly Form _parentForm;
        private readonly List<NotificationControl> _activeNotifications = new List<NotificationControl>();
        private readonly int _spacing = 10;
        private readonly NotificationPosition _position;
        
        /// <summary>
        /// Position for notifications on screen
        /// </summary>
        public enum NotificationPosition
        {
            /// <summary>
            /// Bottom right corner
            /// </summary>
            BottomRight,
            
            /// <summary>
            /// Bottom left corner
            /// </summary>
            BottomLeft,
            
            /// <summary>
            /// Top right corner
            /// </summary>
            TopRight,
            
            /// <summary>
            /// Top left corner
            /// </summary>
            TopLeft,
            
            /// <summary>
            /// Bottom center
            /// </summary>
            BottomCenter
        }

        /// <summary>
        /// Gets the notification manager for a specific form
        /// </summary>
        /// <param name="form">Parent form</param>
        /// <param name="position">Position for notifications</param>
        /// <returns>Notification manager instance</returns>
        public static NotificationManager GetInstance(Form form, NotificationPosition position = NotificationPosition.BottomRight)
        {
            if (!Instances.TryGetValue(form, out var manager))
            {
                manager = new NotificationManager(form, position);
                Instances.Add(form, manager);

                // Add handler to clean up the instance when form is closed
                form.FormClosed += (sender, args) =>
                {
                    if (sender is Form closedForm && Instances.ContainsKey(closedForm))
                    {
                        Instances.Remove(closedForm);
                    }
                };
            }
            
            return manager;
        }

        private NotificationManager(Form parentForm, NotificationPosition position)
        {
            _parentForm = parentForm;
            _position = position;
        }

        /// <summary>
        /// Shows a notification
        /// </summary>
        /// <param name="title">Notification title</param>
        /// <param name="message">Notification message content</param>
        /// <param name="type">Type of notification</param>
        /// <param name="displayDuration">How long to display in milliseconds</param>
        /// <param name="fadeDuration">How long to fade out in milliseconds</param>
        /// <param name="clickAction">Optional action to perform when notification is clicked</param>
        /// <returns>The created notification control</returns>
        public NotificationControl Show(string title, string message, 
            NotificationControl.NotificationType type = NotificationControl.NotificationType.Info, 
            int displayDuration = 5000, int fadeDuration = 500, Action clickAction = null)
        {
            // Create the notification control
            var notification = new NotificationControl(title, message, type, displayDuration, fadeDuration, clickAction)
            {
                Visible = false // Will be shown after positioning
            };
            
            _parentForm.BeginInvoke(new Action(() =>
            {
                // Add to parent form
                _parentForm.Controls.Add(notification);
                notification.BringToFront();
                
                // Add to active notifications and position
                _activeNotifications.Add(notification);
                
                // Register for removal
                notification.Disposed += (sender, args) =>
                {
                    if (sender is NotificationControl n)
                    {
                        _activeNotifications.Remove(n);
                        RepositionNotifications();
                    }
                };
                
                RepositionNotifications();
                notification.Show();
            }));
            
            return notification;
        }

        /// <summary>
        /// Recalculate and reposition all active notifications
        /// </summary>
        private void RepositionNotifications()
        {
            // If there are no notifications, nothing to reposition
            if (_activeNotifications.Count == 0)
            {
                return;
            }
            
            int x = _spacing;
            int y = _spacing;
            int notificationWidth = _activeNotifications[0].Width;
            int notificationHeight = _activeNotifications[0].Height;
            
            // Set starting position based on parent form and notification position setting
            switch (_position)
            {
                case NotificationPosition.BottomRight:
                    x = _parentForm.ClientSize.Width - notificationWidth - _spacing;
                    y = _parentForm.ClientSize.Height - notificationHeight - _spacing;
                    break;
                    
                case NotificationPosition.BottomLeft:
                    x = _spacing;
                    y = _parentForm.ClientSize.Height - notificationHeight - _spacing;
                    break;
                    
                case NotificationPosition.BottomCenter:
                    x = (_parentForm.ClientSize.Width - notificationWidth) / 2;
                    y = _parentForm.ClientSize.Height - notificationHeight - _spacing;
                    break;
                    
                case NotificationPosition.TopRight:
                    x = _parentForm.ClientSize.Width - notificationWidth - _spacing;
                    y = _spacing;
                    break;
                    
                case NotificationPosition.TopLeft:
                    x = _spacing;
                    y = _spacing;
                    break;
            }

            // Position all notifications
            for (int i = 0; i < _activeNotifications.Count; i++)
            {
                var notification = _activeNotifications[i];
                
                switch (_position)
                {
                    case NotificationPosition.BottomRight:
                    case NotificationPosition.BottomLeft:
                    case NotificationPosition.BottomCenter:
                        // Stack upwards from bottom
                        notification.Location = new Point(x, y - (i * (notification.Height + _spacing)));
                        break;
                        
                    case NotificationPosition.TopRight:
                    case NotificationPosition.TopLeft:
                        // Stack downwards from top
                        notification.Location = new Point(x, y + (i * (notification.Height + _spacing)));
                        break;
                }
            }
        }

        /// <summary>
        /// Dismiss all active notifications
        /// </summary>
        public void DismissAll()
        {
            foreach (var notification in _activeNotifications.ToArray())
            {
                notification.StartFade();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace JexusManager.Notifications
{
    /// <summary>
    /// A control that displays notifications similar to Teams messages
    /// </summary>
    public class NotificationControl : Control
    {
        private readonly Timer _fadeTimer = new Timer();
        private readonly int _displayDuration;
        private readonly int _fadeDuration;
        private double _opacity = 1.0;
        private readonly Color _backColor;
        private readonly Color _borderColor;
        private readonly Color _textColor;
        private readonly string _title;
        private readonly string _message;
        private readonly Image _icon;
        private readonly Action _clickAction;
        private readonly Font _titleFont;
        private readonly Font _messageFont;
        private bool _isFading;
        private readonly NotificationType _type;
        
        /// <summary>
        /// The type of notification to display
        /// </summary>
        public enum NotificationType
        {
            /// <summary>
            /// Information notification
            /// </summary>
            Info,
            
            /// <summary>
            /// Success notification
            /// </summary>
            Success,
            
            /// <summary>
            /// Warning notification
            /// </summary>
            Warning,
            
            /// <summary>
            /// Error notification
            /// </summary>
            Error
        }
        
        /// <summary>
        /// Creates a new notification control
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="message">Message content</param>
        /// <param name="type">Type of notification</param>
        /// <param name="displayDuration">How long to display in milliseconds</param>
        /// <param name="fadeDuration">How long to fade out in milliseconds</param>
        /// <param name="clickAction">Optional action to perform when clicked</param>
        public NotificationControl(string title, string message, NotificationType type = NotificationType.Info,
            int displayDuration = 5000, int fadeDuration = 500, Action clickAction = null)
        {
            _title = title;
            _message = message;
            _type = type;
            _displayDuration = displayDuration;
            _fadeDuration = fadeDuration;
            _clickAction = clickAction;

            // Set up default sizes and styling
            Width = 300;
            Height = 80;
            DoubleBuffered = true;
            
            // Determine styling based on notification type
            switch (type)
            {
                case NotificationType.Success:
                    _backColor = Color.FromArgb(232, 245, 233);
                    _borderColor = Color.FromArgb(76, 175, 80);
                    _textColor = Color.FromArgb(30, 70, 32);
                    _icon = SystemIcons.Information.ToBitmap();
                    break;
                case NotificationType.Warning:
                    _backColor = Color.FromArgb(255, 243, 224);
                    _borderColor = Color.FromArgb(255, 152, 0);
                    _textColor = Color.FromArgb(102, 60, 0);
                    _icon = SystemIcons.Warning.ToBitmap();
                    break;
                case NotificationType.Error:
                    _backColor = Color.FromArgb(253, 236, 234);
                    _borderColor = Color.FromArgb(244, 67, 54);
                    _textColor = Color.FromArgb(97, 26, 21);
                    _icon = SystemIcons.Error.ToBitmap();
                    break;
                case NotificationType.Info:
                default:
                    _backColor = Color.FromArgb(227, 242, 253);
                    _borderColor = Color.FromArgb(33, 150, 243);
                    _textColor = Color.FromArgb(13, 71, 161);
                    _icon = SystemIcons.Information.ToBitmap();
                    break;
            }

            // Font settings
            _titleFont = new Font(Font.FontFamily, 10f, FontStyle.Bold);
            _messageFont = new Font(Font.FontFamily, 9f);
            
            // Set up the timer for auto-dismiss
            _fadeTimer.Interval = 50;
            _fadeTimer.Tick += FadeTimer_Tick;
            
            // Handle clicks on the notification
            Click += NotificationControl_Click;
        }
        
        /// <summary>
        /// Starts the display and fade out timers
        /// </summary>
        public new void Show()
        {
            // Reset opacity
            _opacity = 1.0;
            Visible = true;
            
            // Start the timer to begin the fade after display duration
            if (_displayDuration > 0)
            {
                Timer displayTimer = new Timer { Interval = _displayDuration };
                displayTimer.Tick += (sender, args) =>
                {
                    displayTimer.Stop();
                    StartFade();
                };
                displayTimer.Start();
            }
        }

        /// <summary>
        /// Begins the fade out animation
        /// </summary>
        public void StartFade()
        {
            if (_fadeDuration > 0)
            {
                _isFading = true;
                _fadeTimer.Start();
            }
            else
            {
                // If no fade, just hide immediately
                Visible = false;
                Parent?.Controls.Remove(this);
                Dispose();
            }
        }
        
        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            // Reduce opacity gradually
            _opacity -= 1.0 / (_fadeDuration / _fadeTimer.Interval);
            
            if (_opacity <= 0)
            {
                _opacity = 0;
                _fadeTimer.Stop();
                Visible = false;
                Parent?.Controls.Remove(this);
                Dispose();
            }
            
            Invalidate();
        }
        
        private void NotificationControl_Click(object sender, EventArgs e)
        {
            _clickAction?.Invoke();
            
            // Stop fading if currently fading
            if (_isFading)
            {
                _fadeTimer.Stop();
            }
            
            // Hide and dispose the notification
            Visible = false;
            Parent?.Controls.Remove(this);
            Dispose();
        }

        /// <summary>
        /// Paints the notification
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Apply opacity
            ColorMatrix matrix = new ColorMatrix { Matrix33 = (float)_opacity };
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix);
            
            // Define the rounded rectangle path
            GraphicsPath path = new GraphicsPath();
            int radius = 6;
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            
            // Top left corner
            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            
            // Top right corner
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            
            // Bottom right corner
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            
            // Bottom left corner
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            
            path.CloseAllFigures();
            
            // Fill the background
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(_opacity * 255), _backColor)))
            {
                g.FillPath(brush, path);
            }
            
            // Draw the border
            using (Pen pen = new Pen(Color.FromArgb((int)(_opacity * 255), _borderColor), 2))
            {
                g.DrawPath(pen, path);
            }
            
            // Left side color bar
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(_opacity * 255), _borderColor)))
            {
                g.FillRectangle(brush, new Rectangle(1, 1, 5, Height - 2));
            }
            
            // Draw icon if available
            if (_icon != null)
            {
                g.DrawImage(_icon, new Rectangle(15, 10, 24, 24));
            }
            
            // Draw title
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(_opacity * 255), _textColor)))
            {
                g.DrawString(_title, _titleFont, brush, new PointF(50, 10));
                
                // Draw message
                g.DrawString(_message, _messageFont, brush, new RectangleF(50, 32, Width - 60, Height - 40));
            }
            
            // Draw close button
            using (Pen pen = new Pen(Color.FromArgb((int)(_opacity * 200), _textColor), 2))
            {
                // X shape for close button
                g.DrawLine(pen, Width - 18, 12, Width - 12, 18);
                g.DrawLine(pen, Width - 18, 18, Width - 12, 12);
            }
        }

        /// <summary>
        /// Handles disposal of resources
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fadeTimer.Stop();
                _fadeTimer.Dispose();
                _titleFont.Dispose();
                _messageFont.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

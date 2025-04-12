using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JexusManager.Breadcrumb
{
    /// <summary>
    /// A breadcrumb navigation control for Windows Forms applications.
    /// </summary>
    [ToolboxItem(true)]
    public class BreadcrumbControl : Control
    {
        private readonly List<BreadcrumbItem> _items = new List<BreadcrumbItem>();
        private int _selectedIndex = -1;
        private int _hoveredIndex = -1;
        private bool _mouseDown = false;
        private Font _boldFont;
        private int _scrollOffset = 0;
        private bool _showScrollButtons = false;
        private Rectangle _leftScrollButtonRect;
        private Rectangle _rightScrollButtonRect;
        private bool _leftScrollHovered;
        private bool _rightScrollHovered;

        // Fields for textbox mode
        private bool _inTextBoxMode = false;
        private TextBox _pathTextBox;
        private string _pathSeparator = "\\";
        private BorderStyle _borderStyle = BorderStyle.Fixed3D;

        // Fields for context menu
        private ContextMenuStrip _contextMenu;
        private int _contextMenuSourceIndex = -1;
        private bool _showDropdownButtons = true;
        private int _dropdownButtonWidth = 16;
        private bool _showIconArea = true;
        private int _iconSize = 16;
        private Rectangle _iconAreaRect;
        private bool _showSelectedItemIcon = true;

        /// <summary>
        /// Occurs when a breadcrumb item is clicked.
        /// </summary>
        public event EventHandler<BreadcrumbItemClickedEventArgs> ItemClicked;

        /// <summary>
        /// Gets or sets the selected item index.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (_selectedIndex != value && value >= -1 && value < _items.Count)
                {
                    _selectedIndex = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the collection of breadcrumb items.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<BreadcrumbItem> Items => _items;

        /// <summary>
        /// Gets or sets the background color for highlighted items.
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HighlightColor { get; set; } = SystemColors.Highlight;

        /// <summary>
        /// Gets or sets whether horizontal scrolling is enabled.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool EnableScrolling { get; set; } = true;

        /// <summary>
        /// Gets or sets the width of the scroll buttons.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(16)]
        public int ScrollButtonWidth { get; set; } = 16;

        /// <summary>
        /// Gets the total width of all items.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TotalItemsWidth { get; private set; }

        /// <summary>
        /// Gets or sets the border style for the control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(BorderStyle.Fixed3D)]
        public BorderStyle BorderStyle
        {
            get => _borderStyle;
            set
            {
                if (_borderStyle != value)
                {
                    _borderStyle = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the character used to separate path components in text mode.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue("\\")]
        public string PathSeparator
        {
            get => _pathSeparator;
            set
            {
                if (value != null && _pathSeparator != value)
                {
                    _pathSeparator = value;
                    if (_inTextBoxMode && _pathTextBox != null)
                    {
                        _pathTextBox.Text = GetFullPath();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the control allows entering text mode by clicking on empty areas.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AllowTextMode { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the control is currently in text editing mode.
        /// </summary>
        [Browsable(false)]
        public bool IsInTextMode => _inTextBoxMode;

        /// <summary>
        /// Gets or sets whether to show dropdown buttons for breadcrumb items.
        /// </summary>
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool ShowDropdownButtons
        {
            get => _showDropdownButtons;
            set
            {
                if (_showDropdownButtons != value)
                {
                    _showDropdownButtons = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the dropdown buttons.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(16)]
        public int DropdownButtonWidth
        {
            get => _dropdownButtonWidth;
            set
            {
                if (_dropdownButtonWidth != value && value > 0)
                {
                    _dropdownButtonWidth = value;
                    Invalidate();
                }
            }
        }        /// <summary>
                 /// Gets or sets whether to show a dedicated icon area on the left.
                 /// </summary>
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowIconArea
        {
            get => _showIconArea;
            set
            {
                if (_showIconArea != value)
                {
                    _showIconArea = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of icons used in the breadcrumb control.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(16)]
        public int IconSize
        {
            get => _iconSize;
            set
            {
                if (_iconSize != value && value > 0)
                {
                    _iconSize = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Occurs when the control requests child items for a breadcrumb item.
        /// </summary>
        public event EventHandler<BreadcrumbItemEventArgs> ItemChildrenRequested;        /// <summary>
                                                                                         /// Initializes a new instance of the BreadcrumbControl class.
                                                                                         /// </summary>
        public BreadcrumbControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);

            this.BackColor = SystemColors.Window;
            this.ForeColor = SystemColors.WindowText;

            _boldFont = new Font(this.Font, FontStyle.Bold);

            // Initialize context menu
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Opening += ContextMenu_Opening;
            _contextMenu.Closed += ContextMenu_Closed;
        }

        /// <summary>
        /// Clears all items from the breadcrumb control.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            _selectedIndex = -1;
            _hoveredIndex = -1;
            Invalidate();
        }

        /// <summary>
        /// Adds a new item to the breadcrumb control.
        /// </summary>
        /// <param name="text">The text of the item.</param>
        /// <param name="tag">The tag object associated with the item.</param>
        /// <returns>The newly added BreadcrumbItem.</returns>
        public BreadcrumbItem AddItem(string text, object tag = null)
        {
            var item = new BreadcrumbItem(text, tag);
            _items.Add(item);
            Invalidate();
            return item;
        }

        /// <summary>
        /// Adds a new item to the breadcrumb control.
        /// </summary>
        /// <param name="text">The text of the item.</param>
        /// <param name="tag">The tag object associated with the item.</param>
        /// <param name="icon">The icon to display for this item.</param>
        /// <returns>The newly added BreadcrumbItem.</returns>
        public BreadcrumbItem AddItem(string text, object tag = null, Image icon = null)
        {
            var item = new BreadcrumbItem(text, tag, icon);
            _items.Add(item);
            Invalidate();
            return item;
        }

        /// <summary>
        /// Removes the last item from the breadcrumb control.
        /// </summary>
        public void RemoveLastItem()
        {
            if (_items.Count > 0)
            {
                _items.RemoveAt(_items.Count - 1);
                if (_selectedIndex >= _items.Count)
                {
                    _selectedIndex = _items.Count - 1;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _boldFont?.Dispose();
            }
            base.Dispose(disposing);
        }        /// <summary>
                 /// Raises the Paint event.
                 /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // If in text mode, we don't draw breadcrumb items
            if (_inTextBoxMode)
            {
                // Just draw the border
                DrawBorder(g);
                return;
            }

            int availableWidth = Width - Padding.Horizontal;
            int contentWidth = MeasureTotalItemsWidth(g);
            TotalItemsWidth = contentWidth;

            // Determine if we need to show scroll buttons
            _showScrollButtons = EnableScrolling && contentWidth > availableWidth;

            // Adjust available width if scroll buttons are visible
            if (_showScrollButtons)
            {
                availableWidth -= ScrollButtonWidth * 2;
            }

            // Ensure scroll offset is within valid range
            if (_scrollOffset < 0)
            {
                _scrollOffset = 0;
            }
            else if (_scrollOffset > Math.Max(0, contentWidth - availableWidth))
            {
                _scrollOffset = Math.Max(0, contentWidth - availableWidth);
            }

            // Draw scroll buttons if needed
            if (_showScrollButtons)
            {
                DrawScrollButtons(g);
            }
            // Calculate starting x position
            int x = Padding.Left;

            // Add space for icon area if enabled
            int iconAreaWidth = 0;
            if (_showIconArea && _selectedIndex >= 0 && _selectedIndex < _items.Count && _items[_selectedIndex].Icon != null)
            {
                iconAreaWidth = IconSize + 8; // Icon width plus padding

                // Draw icon area with subtle separator
                _iconAreaRect = new Rectangle(x, 0, iconAreaWidth, Height);

                // Draw a light background for the icon area
                using (Brush iconAreaBrush = new SolidBrush(Color.FromArgb(20, SystemColors.ControlDark)))
                {
                    g.FillRectangle(iconAreaBrush, _iconAreaRect);
                }

                // Draw a subtle separator line
                using (Pen separatorPen = new Pen(Color.FromArgb(100, SystemColors.ControlDark)))
                {
                    g.DrawLine(separatorPen,
                        x + iconAreaWidth - 1, 0,
                        x + iconAreaWidth - 1, Height);
                }

                // Draw the icon centered in the icon area
                if (_items[_selectedIndex].Icon != null)
                {
                    int iconX = x + (iconAreaWidth - IconSize) / 2;
                    int iconY = (Height - IconSize) / 2;
                    g.DrawImage(_items[_selectedIndex].Icon, new Rectangle(iconX, iconY, IconSize, IconSize));
                }

                // Update starting position for breadcrumb items
                x += iconAreaWidth;
            }

            if (_showScrollButtons)
            {
                x += ScrollButtonWidth;
            }

            // Apply scroll offset
            x -= _scrollOffset;

            // Draw items
            for (int i = 0; i < _items.Count; i++)
            {
                BreadcrumbItem item = _items[i];

                // Determine font and colors based on state
                Font itemFont = i == _selectedIndex ? _boldFont : Font;
                Color textColor = ForeColor;
                Brush backgroundBrush = null;

                if (i == _hoveredIndex)
                {
                    backgroundBrush = new SolidBrush(Color.FromArgb(40, HighlightColor));
                    if (_mouseDown)
                    {
                        backgroundBrush = new SolidBrush(Color.FromArgb(70, HighlightColor));
                    }
                }

                // Measure text
                SizeF textSize = g.MeasureString(item.Text, itemFont);

                // Add extra width for dropdown button if needed
                int dropdownButtonSpace = (_showDropdownButtons) ? DropdownButtonWidth : 0;

                // Add space for icon if this is the selected item and ShowSelectedItemIcon is true
                int iconSpace = 0;
                if (i == _selectedIndex && _showSelectedItemIcon && item.Icon != null)
                {
                    iconSpace = IconSize + 4; // Icon width plus a little padding
                }

                int itemWidth = (int)Math.Ceiling(textSize.Width) + 10 + dropdownButtonSpace + iconSpace; // Add padding
                int itemHeight = (int)Math.Ceiling(textSize.Height) + 4; // Add padding

                // Calculate the bounds for this item
                Rectangle itemBounds = new Rectangle(x, (Height - itemHeight) / 2, itemWidth, itemHeight);
                item.Bounds = itemBounds;

                // Calculate dropdown button bounds if needed
                if (_showDropdownButtons)
                {
                    item.DropdownButtonBounds = new Rectangle(
                        itemBounds.Right - DropdownButtonWidth - 2,
                        itemBounds.Y,
                        DropdownButtonWidth,
                        itemBounds.Height);
                }

                // Check if item is at least partially visible
                if (itemBounds.Right > Padding.Left + (_showScrollButtons ? ScrollButtonWidth : 0) &&
                    itemBounds.Left < Width - Padding.Right - (_showScrollButtons ? ScrollButtonWidth : 0))
                {
                    // Draw background if hovered/selected
                    if (backgroundBrush != null)
                    {
                        // Make sure we clip to visible area
                        Rectangle visibleBounds = Rectangle.Intersect(itemBounds,
                            new Rectangle(
                                Padding.Left + (_showScrollButtons ? ScrollButtonWidth : 0),
                                0,
                                Width - Padding.Horizontal - (_showScrollButtons ? ScrollButtonWidth * 2 : 0),
                                Height));

                        if (!visibleBounds.IsEmpty)
                        {
                            using GraphicsPath path = RoundedRectangle(visibleBounds, 3);
                            g.FillPath(backgroundBrush, path);
                        }

                        backgroundBrush.Dispose();
                    }

                    // Create a clipping region to avoid drawing outside visible area
                    Rectangle clipRect = new Rectangle(
                        Padding.Left + (_showScrollButtons ? ScrollButtonWidth : 0),
                        0,
                        Width - Padding.Horizontal - (_showScrollButtons ? ScrollButtonWidth * 2 : 0),
                        Height);

                    Region oldClip = g.Clip;
                    g.Clip = new Region(clipRect);                    // Set initial text position - no more inline icon drawing as we have dedicated icon area
                    int textX = x + 5;

                    // Draw text
                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        g.DrawString(item.Text, itemFont, textBrush,
                            textX, (Height - textSize.Height) / 2);
                    }                    // Draw dropdown button if enabled
                    if (_showDropdownButtons)
                    {
                        // Draw a small solid triangle dropdown indicator
                        int arrowX = itemBounds.Right - DropdownButtonWidth / 2 - 2;
                        int arrowY = itemBounds.Y + itemBounds.Height / 2;
                        int arrowSize = 4;

                        Point[] trianglePoints = new Point[]
                        {
                                new Point(arrowX - arrowSize, arrowY - arrowSize),
                                new Point(arrowX, arrowY),
                                new Point(arrowX - arrowSize, arrowY + arrowSize)
                        };

                        using Brush triangleBrush = new SolidBrush(Color.FromArgb(180, textColor));
                        g.FillPolygon(triangleBrush, trianglePoints);
                    }

                    g.Clip = oldClip;
                }                // Update x position
                x += itemWidth;
            }

            // Draw the border
            DrawBorder(g);
        }

        /// <summary>
        /// Draws the border around the control.
        /// </summary>
        private void DrawBorder(Graphics g)
        {
            if (_borderStyle == BorderStyle.None)
                return;

            Color borderColor = SystemColors.ControlDark;
            Rectangle borderRect = new Rectangle(0, 0, Width - 1, Height - 1);

            // Draw the appropriate border style
            if (_borderStyle == BorderStyle.FixedSingle)
            {
                using Pen borderPen = new Pen(borderColor);
                g.DrawRectangle(borderPen, borderRect);
            }
            else if (_borderStyle == BorderStyle.Fixed3D)
            {
                // Draw 3D border
                using (Pen outerPen = new Pen(SystemColors.ControlDark))
                {
                    g.DrawRectangle(outerPen, borderRect);
                }

                using Pen innerPen = new Pen(SystemColors.ControlLightLight);
                Rectangle innerRect = new Rectangle(1, 1, Width - 3, Height - 3);
                g.DrawRectangle(innerPen, innerRect);
            }
        }
        /// <summary>
        /// Draws the left and right scroll buttons when scrolling is active.
        /// </summary>
        private void DrawScrollButtons(Graphics g)
        {
            int buttonSize = ScrollButtonWidth;
            int yCenter = Height / 2;

            // Left scroll button
            _leftScrollButtonRect = new Rectangle(Padding.Left, 0, buttonSize, Height);
            bool leftEnabled = _scrollOffset > 0;

            // Right scroll button
            _rightScrollButtonRect = new Rectangle(Width - Padding.Right - buttonSize, 0, buttonSize, Height);
            bool rightEnabled = _scrollOffset < Math.Max(0, TotalItemsWidth - (Width - Padding.Horizontal - buttonSize * 2));

            // Draw left button
            Color leftBgColor = _leftScrollHovered
                ? Color.FromArgb(leftEnabled ? 100 : 50, HighlightColor)
                : Color.FromArgb(leftEnabled ? 60 : 30, SystemColors.ControlDark);

            Color leftArrowColor = leftEnabled
                ? ForeColor
                : Color.FromArgb(120, SystemColors.ControlDark);

            using (Brush bgBrush = new SolidBrush(leftBgColor))
            {
                // Draw a rounded rectangle for the left button background
                using GraphicsPath path = RoundedRectangle(new Rectangle(
                    _leftScrollButtonRect.X,
                    (_leftScrollButtonRect.Height - _leftScrollButtonRect.Width) / 2,
                    _leftScrollButtonRect.Width,
                    _leftScrollButtonRect.Width), 3);
                g.FillPath(bgBrush, path);

                // Add a subtle border
                using Pen borderPen = new Pen(Color.FromArgb(30, SystemColors.ControlDarkDark));
                g.DrawPath(borderPen, path);
            }

            // Draw right button
            Color rightBgColor = _rightScrollHovered
                ? Color.FromArgb(rightEnabled ? 100 : 50, HighlightColor)
                : Color.FromArgb(rightEnabled ? 60 : 30, SystemColors.ControlDark);

            Color rightArrowColor = rightEnabled
                ? ForeColor
                : Color.FromArgb(120, SystemColors.ControlDark);

            using (Brush bgBrush = new SolidBrush(rightBgColor))
            {
                // Draw a rounded rectangle for the right button background
                using GraphicsPath path = RoundedRectangle(new Rectangle(
                    _rightScrollButtonRect.X,
                    (_rightScrollButtonRect.Height - _rightScrollButtonRect.Width) / 2,
                    _rightScrollButtonRect.Width,
                    _rightScrollButtonRect.Width), 3);
                g.FillPath(bgBrush, path);

                // Add a subtle border
                using Pen borderPen = new Pen(Color.FromArgb(30, SystemColors.ControlDarkDark));
                g.DrawPath(borderPen, path);
            }

            // Draw left arrow with a more distinct appearance
            using (Pen arrowPen = new Pen(leftArrowColor, 2.0f))
            {
                int centerX = _leftScrollButtonRect.X + _leftScrollButtonRect.Width / 2;
                int centerY = Height / 2;
                int arrowSize = 4;

                // Draw a left-pointing triangle
                Point[] arrowPoints = new Point[]
                {
                    new Point(centerX + arrowSize, centerY - arrowSize),
                    new Point(centerX - arrowSize, centerY),
                    new Point(centerX + arrowSize, centerY + arrowSize)
                };

                g.DrawLines(arrowPen, arrowPoints);
            }

            // Draw right arrow with a more distinct appearance
            using (Pen arrowPen = new Pen(rightArrowColor, 2.0f))
            {
                int centerX = _rightScrollButtonRect.X + _rightScrollButtonRect.Width / 2;
                int centerY = Height / 2;
                int arrowSize = 4;

                // Draw a right-pointing triangle
                Point[] arrowPoints = new Point[]
                {
                    new Point(centerX - arrowSize, centerY - arrowSize),
                    new Point(centerX + arrowSize, centerY),
                    new Point(centerX - arrowSize, centerY + arrowSize)
                };

                g.DrawLines(arrowPen, arrowPoints);
            }
        }

        /// <summary>
        /// Measures the total width required to display all items.
        /// </summary>
        private int MeasureTotalItemsWidth(Graphics g)
        {
            int totalWidth = 0;

            for (int i = 0; i < _items.Count; i++)
            {
                // Measure item
                Font itemFont = i == _selectedIndex ? _boldFont : Font;
                SizeF textSize = g.MeasureString(_items[i].Text, itemFont);

                // Add extra width for dropdown button if needed
                int dropdownButtonSpace = (_showDropdownButtons) ? DropdownButtonWidth : 0;
                // Add space for icon if this is the first item and ShowFirstItemIcon is true
                int iconSpace = 0;
                if (i == 0 && _showSelectedItemIcon && _items[i].Icon != null)
                {
                    iconSpace = IconSize + 4; // Icon width plus a little padding
                }

                int itemWidth = (int)Math.Ceiling(textSize.Width) + 10 + dropdownButtonSpace + iconSpace; // Add padding

                totalWidth += itemWidth;
            }

            return totalWidth;
        }

        /// <summary>
        /// Creates a rounded rectangle path.
        /// </summary>
        private GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            // Top left arc
            path.AddArc(arc, 180, 90);

            // Top right arc
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right arc
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left arc
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }        /// <summary>
                 /// Raises the MouseMove event.
                 /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Check if hovering over scroll buttons
            bool oldLeftHovered = _leftScrollHovered;
            bool oldRightHovered = _rightScrollHovered;

            _leftScrollHovered = _showScrollButtons && _leftScrollButtonRect.Contains(e.Location);
            _rightScrollHovered = _showScrollButtons && _rightScrollButtonRect.Contains(e.Location);

            // Only check for item hovering if not hovering over scroll buttons
            int oldHoveredIndex = _hoveredIndex;
            _hoveredIndex = -1;

            if (!_leftScrollHovered && !_rightScrollHovered)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].Bounds.Contains(e.Location))
                    {
                        _hoveredIndex = i;
                        break;
                    }
                }
            }

            // Check if we need to repaint
            if (oldHoveredIndex != _hoveredIndex ||
                oldLeftHovered != _leftScrollHovered ||
                oldRightHovered != _rightScrollHovered)
            {
                Invalidate();
            }

            // Update cursor
            if (_hoveredIndex >= 0)
            {
                Cursor = Cursors.Hand;
            }
            else if (_leftScrollHovered || _rightScrollHovered)
            {
                Cursor = Cursors.Hand;
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Raises the MouseDown event.
        /// </summary>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                _mouseDown = true;

                // Check for scroll button clicks
                if (_leftScrollHovered)
                {
                    ScrollLeft();
                }
                else if (_rightScrollHovered)
                {
                    ScrollRight();
                }

                Invalidate();
            }
        }        /// <summary>
                 /// Raises the MouseUp event.
                 /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                _mouseDown = false;

                // Only process clicks when not in text mode
                if (!_inTextBoxMode)
                {
                    // If clicking on scroll buttons, they're handled in MouseDown
                    if (_hoveredIndex >= 0 && !_leftScrollHovered && !_rightScrollHovered)
                    {
                        BreadcrumbItem item = _items[_hoveredIndex];

                        // Check if click was on dropdown button
                        if (_showDropdownButtons && item.DropdownButtonBounds.Contains(e.Location))
                        {
                            // Show the dropdown context menu
                            ShowDropdownMenu(_hoveredIndex, item.DropdownButtonBounds);
                        }
                        else
                        {
                            // Normal item click
                            _selectedIndex = _hoveredIndex;
                            ItemClicked?.Invoke(this, new BreadcrumbItemClickedEventArgs(item, _hoveredIndex));
                        }
                    }
                    else if (!_leftScrollHovered && !_rightScrollHovered && AllowTextMode)
                    {
                        // Click on empty area - enter text mode
                        EnterTextMode();
                    }
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Raises the MouseLeave event.
        /// </summary>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            bool needsInvalidate = _hoveredIndex >= 0 || _leftScrollHovered || _rightScrollHovered;

            _hoveredIndex = -1;
            _leftScrollHovered = false;
            _rightScrollHovered = false;

            if (needsInvalidate)
            {
                Invalidate();
            }

            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Scrolls the items to the left (revealing items on the right).
        /// </summary>
        public void ScrollRight()
        {
            if (_showScrollButtons)
            {
                int visibleWidth = Width - Padding.Horizontal - (ScrollButtonWidth * 2);
                _scrollOffset += Math.Min(visibleWidth / 2, 50); // Scroll by half the visible area or 50px, whichever is smaller

                // Make sure we don't scroll too far
                int maxOffset = Math.Max(0, TotalItemsWidth - visibleWidth);
                if (_scrollOffset > maxOffset)
                {
                    _scrollOffset = maxOffset;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Scrolls the items to the right (revealing items on the left).
        /// </summary>
        public void ScrollLeft()
        {
            if (_showScrollButtons)
            {
                int visibleWidth = Width - Padding.Horizontal - (ScrollButtonWidth * 2);
                _scrollOffset -= Math.Min(visibleWidth / 2, 50); // Scroll by half the visible area or 50px, whichever is smaller

                // Make sure we don't scroll too far
                if (_scrollOffset < 0)
                {
                    _scrollOffset = 0;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Ensures the specified item is visible in the current view.
        /// </summary>
        /// <param name="index">The index of the item to scroll into view.</param>
        public void EnsureVisible(int index)
        {
            if (index < 0 || index >= _items.Count || !_showScrollButtons)
                return;

            BreadcrumbItem item = _items[index];

            // Calculate visible area
            int visibleLeft = Padding.Left + ScrollButtonWidth;
            int visibleWidth = Width - Padding.Horizontal - (ScrollButtonWidth * 2);
            int visibleRight = visibleLeft + visibleWidth;

            // Calculate item's current position
            int itemLeft = item.Bounds.Left + _scrollOffset;
            int itemRight = item.Bounds.Right + _scrollOffset;

            // If item is to the left of visible area, scroll left
            if (itemLeft < visibleLeft)
            {
                _scrollOffset = itemLeft - Padding.Left - ScrollButtonWidth;
            }
            // If item is to the right of visible area, scroll right
            else if (itemRight > visibleRight)
            {
                _scrollOffset = itemRight - visibleWidth - Padding.Left - ScrollButtonWidth;
            }

            // Ensure scroll offset is within valid range
            if (_scrollOffset < 0)
            {
                _scrollOffset = 0;
            }
            else
            {
                int maxOffset = Math.Max(0, TotalItemsWidth - visibleWidth);
                if (_scrollOffset > maxOffset)
                {
                    _scrollOffset = maxOffset;
                }
            }

            Invalidate();
        }

        /// <summary>
        /// Gets the full path representation of the breadcrumb items.
        /// </summary>
        /// <returns>A string representing the full path.</returns>
        public string GetFullPath()
        {
            if (_items.Count == 0)
                return string.Empty;

            string path = string.Empty;

            for (int i = 0; i < _items.Count; i++)
            {
                path += _items[i].Text;

                if (i < _items.Count - 1)
                {
                    path += PathSeparator;
                }
            }

            return path;
        }

        /// <summary>
        /// Enters text editing mode.
        /// </summary>
        public void EnterTextMode()
        {
            if (_inTextBoxMode || !AllowTextMode)
                return;

            _inTextBoxMode = true;

            // Create the text box if it doesn't exist
            if (_pathTextBox == null)
            {
                _pathTextBox = new TextBox();
                _pathTextBox.BorderStyle = BorderStyle.None;
                _pathTextBox.Font = this.Font;
                _pathTextBox.KeyDown += PathTextBox_KeyDown;
                _pathTextBox.LostFocus += PathTextBox_LostFocus;
                Controls.Add(_pathTextBox);
            }

            // Set initial path text
            _pathTextBox.Text = GetFullPath();

            // Position and size the text box
            _pathTextBox.Location = new Point(Padding.Left + 3, (Height - _pathTextBox.Height) / 2);
            _pathTextBox.Width = Width - Padding.Horizontal - 6;
            _pathTextBox.Visible = true;
            _pathTextBox.BringToFront();
            _pathTextBox.Focus();
            _pathTextBox.SelectAll();

            Invalidate();
        }

        /// <summary>
        /// Exits text editing mode.
        /// </summary>
        /// <param name="applyChanges">Whether to apply any changes made in the text box.</param>
        public void ExitTextMode(bool applyChanges)
        {
            if (!_inTextBoxMode)
                return;

            _inTextBoxMode = false;

            if (_pathTextBox != null)
            {
                if (applyChanges)
                {
                    TryNavigateToPath(_pathTextBox.Text);
                }

                _pathTextBox.Visible = false;
            }

            Invalidate();
        }

        /// <summary>
        /// Tries to navigate to the specified path.
        /// </summary>
        /// <param name="path">The path to navigate to.</param>
        /// <returns>True if navigation was successful, false otherwise.</returns>
        public bool TryNavigateToPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // Split the path into segments
            string[] segments = path.Split(new[] { PathSeparator }, StringSplitOptions.None);

            // Validate segments against current items
            bool validPath = true;
            int matchedSegments = 0;

            for (int i = 0; i < segments.Length && i < _items.Count; i++)
            {
                if (segments[i] != _items[i].Text)
                {
                    validPath = false;
                    break;
                }

                matchedSegments++;
            }

            // If path starts with valid segments but has more segments than current path,
            // we could potentially extend the path (but we won't here, just consider it valid)
            if (validPath && matchedSegments == _items.Count && segments.Length > _items.Count)
            {
                // Path is valid but has additional segments we don't have navigation for
                // For this implementation, we'll consider this invalid
                validPath = false;
            }

            // If path is valid but shorter than current path, truncate current path
            if (validPath && matchedSegments < _items.Count)
            {
                while (_items.Count > matchedSegments)
                {
                    _items.RemoveAt(_items.Count - 1);
                }

                if (_items.Count > 0)
                {
                    _selectedIndex = _items.Count - 1;
                    ItemClicked?.Invoke(this, new BreadcrumbItemClickedEventArgs(_items[_selectedIndex], _selectedIndex));
                }
                else
                {
                    _selectedIndex = -1;
                }
            }

            return validPath;
        }

        private void PathTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ExitTextMode(true);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                ExitTextMode(false);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void PathTextBox_LostFocus(object sender, EventArgs e)
        {
            // Exit text mode when focus is lost
            BeginInvoke(new Action(() => ExitTextMode(false)));
        }

        /// <summary>
        /// Shows the dropdown menu for a breadcrumb item.
        /// </summary>
        /// <param name="itemIndex">The index of the item to show the dropdown for.</param>
        /// <param name="dropdownButtonBounds">The bounds of the dropdown button.</param>
        private void ShowDropdownMenu(int itemIndex, Rectangle dropdownButtonBounds)
        {
            if (itemIndex < 0 || itemIndex >= _items.Count)
                return;

            // Set the current context menu source
            _contextMenuSourceIndex = itemIndex;

            // Position the context menu
            Point menuLocation = PointToScreen(new Point(
                dropdownButtonBounds.Right - dropdownButtonBounds.Width,
                dropdownButtonBounds.Bottom));

            // Show the context menu
            _contextMenu.Show(menuLocation);
        }

        /// <summary>
        /// Handles the Opening event of the context menu.
        /// </summary>
        private void ContextMenu_Opening(object sender, CancelEventArgs e)
        {
            // Clear existing items
            _contextMenu.Items.Clear();

            if (_contextMenuSourceIndex < 0 || _contextMenuSourceIndex >= _items.Count)
            {
                e.Cancel = true;
                return;
            }

            BreadcrumbItem sourceItem = _items[_contextMenuSourceIndex];

            // Check if the item has any child items
            if (sourceItem.ChildItems.Count == 0)
            {
                // Raise event to request child items
                ItemChildrenRequested?.Invoke(this, new BreadcrumbItemEventArgs(sourceItem, _contextMenuSourceIndex));
            }

            // If there are still no child items, cancel the menu
            if (sourceItem.ChildItems.Count == 0)
            {
                e.Cancel = true;
                return;
            }

            // Add items to the context menu
            foreach (BreadcrumbItem childItem in sourceItem.ChildItems)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(childItem.Text);

                // Set the tag to the child item for reference
                menuItem.Tag = childItem;

                // Add an icon if available
                if (childItem.Icon != null)
                {
                    menuItem.Image = childItem.Icon;
                }

                // Handle click event
                menuItem.Click += DropdownMenuItem_Click;

                // Add to the menu
                _contextMenu.Items.Add(menuItem);
            }
        }

        /// <summary>
        /// Handles the Closed event of the context menu.
        /// </summary>
        private void ContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _contextMenuSourceIndex = -1;
        }

        /// <summary>
        /// Handles the Click event of a dropdown menu item.
        /// </summary>
        private void DropdownMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is BreadcrumbItem selectedChildItem)
            {
                // If we're not at the end of the breadcrumb, trim the trail
                if (_contextMenuSourceIndex < _items.Count - 1)
                {
                    while (_items.Count > _contextMenuSourceIndex + 1)
                    {
                        _items.RemoveAt(_items.Count - 1);
                    }
                }

                // Add the selected child item to the breadcrumb
                _items.Add(selectedChildItem);
                _selectedIndex = _items.Count - 1;

                // Notify of navigation
                ItemClicked?.Invoke(this, new BreadcrumbItemClickedEventArgs(selectedChildItem, _selectedIndex));

                // Redraw
                Invalidate();
            }
        }
    }
}

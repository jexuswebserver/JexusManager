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
        /// Gets or sets the separator character between items.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(">")]
        public string Separator { get; set; } = ">";

        /// <summary>
        /// Gets or sets the background color for highlighted items.
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HighlightColor { get; set; } = SystemColors.Highlight;

        /// <summary>
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
        }

        /// <summary>
        /// Raises the Paint event.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            // Track locations and widths for hit testing
            int x = Padding.Left;
            
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
                int itemWidth = (int)Math.Ceiling(textSize.Width) + 10; // Add padding
                int itemHeight = (int)Math.Ceiling(textSize.Height) + 4; // Add padding
                
                // Set the item's bounds for hit testing
                item.Bounds = new Rectangle(x, (Height - itemHeight) / 2, itemWidth, itemHeight);
                
                // Draw background if hovered/selected
                if (backgroundBrush != null)
                {
                    using (GraphicsPath path = RoundedRectangle(item.Bounds, 3))
                    {
                        g.FillPath(backgroundBrush, path);
                    }
                    backgroundBrush.Dispose();
                }
                
                // Draw text
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(item.Text, itemFont, textBrush, 
                        x + 5, (Height - textSize.Height) / 2);
                }
                
                // Update x position
                x += itemWidth;
                
                // Draw separator if not the last item
                if (i < _items.Count - 1)
                {
                    using Brush separatorBrush = new SolidBrush(Color.FromArgb(180, ForeColor));
                    SizeF sepSize = g.MeasureString(Separator, Font);
                    g.DrawString(Separator, Font, separatorBrush,
                        x + 2, (Height - sepSize.Height) / 2);
                    x += (int)Math.Ceiling(sepSize.Width) + 6;
                }
            }
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
        }

        /// <summary>
        /// Raises the MouseMove event.
        /// </summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            int oldHoveredIndex = _hoveredIndex;
            _hoveredIndex = -1;
            
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Bounds.Contains(e.Location))
                {
                    _hoveredIndex = i;
                    break;
                }
            }
            
            if (oldHoveredIndex != _hoveredIndex)
            {
                Invalidate();
                Cursor = _hoveredIndex >= 0 ? Cursors.Hand : Cursors.Default;
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
                Invalidate();
            }
        }

        /// <summary>
        /// Raises the MouseUp event.
        /// </summary>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)
            {
                _mouseDown = false;
                if (_hoveredIndex >= 0)
                {
                    _selectedIndex = _hoveredIndex;
                    ItemClicked?.Invoke(this, new BreadcrumbItemClickedEventArgs(_items[_hoveredIndex], _hoveredIndex));
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
            if (_hoveredIndex >= 0)
            {
                _hoveredIndex = -1;
                Invalidate();
            }
            Cursor = Cursors.Default;
        }
    }
}

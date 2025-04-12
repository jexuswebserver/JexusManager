using System.Drawing;

namespace JexusManager.Breadcrumb
{
    /// <summary>
    /// Represents an individual item in a breadcrumb navigation control.
    /// </summary>
    public class BreadcrumbItem
    {
        /// <summary>
        /// Gets or sets the display text of the breadcrumb item.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the tag object associated with this breadcrumb item.
        /// </summary>
        public object? Tag { get; set; }

        /// <summary>
        /// Gets or sets the bounds rectangle for this item.
        /// </summary>
        internal Rectangle Bounds { get; set; }

        /// <summary>
        /// Initializes a new instance of the BreadcrumbItem class.
        /// </summary>
        /// <param name="text">The display text of the breadcrumb item.</param>
        /// <param name="tag">The tag object associated with this breadcrumb item.</param>
        public BreadcrumbItem(string text, object? tag = null)
        {
            Text = text;
            Tag = tag;
        }

        /// <summary>
        /// Returns a string representation of this breadcrumb item.
        /// </summary>
        public override string ToString()
        {
            return Text;
        }
    }
}

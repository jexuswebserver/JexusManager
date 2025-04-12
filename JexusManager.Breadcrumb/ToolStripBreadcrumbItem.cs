using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace JexusManager.Breadcrumb
{
    /// <summary>
    /// A ToolStripControlHost that wraps a BreadcrumbControl for use in ToolStrip containers.
    /// </summary>
    [ToolboxItem(true)]
    public class ToolStripBreadcrumbItem : ToolStripControlHost
    {
        /// <summary>
        /// Gets the underlying BreadcrumbControl being hosted.
        /// </summary>
        public BreadcrumbControl BreadcrumbControl => Control as BreadcrumbControl;        /// <summary>
                                                                                           /// Initializes a new instance of the ToolStripBreadcrumbItem class.
                                                                                           /// </summary>
        public ToolStripBreadcrumbItem() : base(CreateControlInstance())
        {
            // Set default appearance
            AutoSize = false;

            // Set a minimum width to ensure the breadcrumb is visible
            Width = 200;

            // Adjust layout
            Padding = new Padding(2);
            Margin = new Padding(0, 1, 0, 2);
        }
        private static Control CreateControlInstance()
        {
            return new BreadcrumbControl
            {
                Margin = new Padding(0),
                Padding = new Padding(0),
                Dock = DockStyle.Fill,
                Height = 22  // Set an explicit height to ensure visibility
            };
        }

        /// <summary>
        /// Gets or sets the selected item index in the breadcrumb control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => BreadcrumbControl.SelectedIndex;
            set => BreadcrumbControl.SelectedIndex = value;
        }

        /// <summary>
        /// Gets the collection of breadcrumb items.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Collections.Generic.List<BreadcrumbItem> Items => BreadcrumbControl.Items;

        /// <summary>
        /// Gets or sets the background color for highlighted items.
        /// </summary>
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color HighlightColor
        {
            get => BreadcrumbControl.HighlightColor;
            set => BreadcrumbControl.HighlightColor = value;
        }        /// <summary>
                 /// Subscribes to events from the hosted control.
                 /// </summary>
        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);

            // Subscribe to events from the BreadcrumbControl
            if (control is BreadcrumbControl breadcrumbControl)
            {
                breadcrumbControl.ItemClicked += BreadcrumbControl_ItemClicked;
            }
        }

        /// <summary>
        /// Unsubscribes from events from the hosted control.
        /// </summary>
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);

            // Unsubscribe from events from the BreadcrumbControl
            if (control is BreadcrumbControl breadcrumbControl)
            {
                breadcrumbControl.ItemClicked -= BreadcrumbControl_ItemClicked;
            }
        }

        // Forward events from the BreadcrumbControl        // Forward events from the BreadcrumbControl
        private void BreadcrumbControl_ItemClicked(object sender, BreadcrumbItemClickedEventArgs e)
        {
            ItemClicked?.Invoke(this, e);
        }

        /// <summary>
        /// Occurs when a breadcrumb item is clicked.
        /// </summary>
        public event System.EventHandler<BreadcrumbItemClickedEventArgs> ItemClicked;

        /// <summary>
        /// Clears all items from the breadcrumb control.
        /// </summary>
        public void Clear()
        {
            BreadcrumbControl.Clear();
        }

        /// <summary>
        /// Adds a new item to the breadcrumb control.
        /// </summary>
        /// <param name="text">The text of the item.</param>
        /// <param name="tag">The tag object associated with the item.</param>
        /// <returns>The newly added BreadcrumbItem.</returns>
        public BreadcrumbItem AddItem(string text, object tag = null)
        {
            return BreadcrumbControl.AddItem(text, tag);
        }

        /// <summary>
        /// Removes the last item from the breadcrumb control.
        /// </summary>
        public void RemoveLastItem()
        {
            BreadcrumbControl.RemoveLastItem();
        }
    }
}

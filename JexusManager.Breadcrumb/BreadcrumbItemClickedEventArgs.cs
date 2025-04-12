using System;

namespace JexusManager.Breadcrumb
{
    /// <summary>
    /// Provides data for the BreadcrumbControl.ItemClicked event.
    /// </summary>
    public class BreadcrumbItemClickedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the BreadcrumbItem that was clicked.
        /// </summary>
        public BreadcrumbItem Item { get; }

        /// <summary>
        /// Gets the index of the BreadcrumbItem that was clicked.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Initializes a new instance of the BreadcrumbItemClickedEventArgs class.
        /// </summary>
        /// <param name="item">The BreadcrumbItem that was clicked.</param>
        /// <param name="index">The index of the BreadcrumbItem that was clicked.</param>
        public BreadcrumbItemClickedEventArgs(BreadcrumbItem item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

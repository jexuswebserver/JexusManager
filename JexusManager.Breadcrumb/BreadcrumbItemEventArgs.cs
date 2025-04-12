using System;

namespace JexusManager.Breadcrumb
{
    /// <summary>
    /// Provides data for the BreadcrumbControl.ItemChildrenRequested event.
    /// </summary>
    public class BreadcrumbItemEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the BreadcrumbItem for which children are requested.
        /// </summary>
        public BreadcrumbItem Item { get; }

        /// <summary>
        /// Gets the index of the BreadcrumbItem in the breadcrumb trail.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Initializes a new instance of the BreadcrumbItemEventArgs class.
        /// </summary>
        /// <param name="item">The BreadcrumbItem for which children are requested.</param>
        /// <param name="index">The index of the BreadcrumbItem in the breadcrumb trail.</param>
        public BreadcrumbItemEventArgs(BreadcrumbItem item, int index)
        {
            Item = item;
            Index = index;
        }
    }
}

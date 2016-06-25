// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
    public class SortableListView : UserControl
    {
        public SortableListView(
            List<ColumnHeader> columnHeaders
            )
        { }

        public void AddItems(
            List<ListViewItem> itemsList
            )
        { }

        protected override void Dispose(
            bool disposing
            )
        {
        }

        protected virtual IComparer GetItemComparer(
            ColumnHeader sortColumn,
            SortOrder sortOrder
            )
        {
            return null;
        }

        public void SetItems(
            List<ListViewItem> itemsList
            )
        { }

        public void ShowSortColumn(
            ColumnHeader column,
            SortOrder sortOrder
            )
        { }

        protected void Sort() { }

        public void Sort(
            ColumnHeader column,
            SortOrder sortOrder
            )
        { }

        public ListView ListView { get; }
        public ColumnHeader SortColumn { get; }
        public SortOrder SortOrder { get; }
    }
}
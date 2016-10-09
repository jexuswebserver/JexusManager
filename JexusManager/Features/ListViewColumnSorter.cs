using System.Collections.Generic;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;

namespace JexusManager.Main.Features
{
    public abstract class ListViewColumnSorter : IComparer<ListViewSubItem>
    {
        public int ColumnIndex { get; set; }

        public SortOrder Order { get; set; } = SortOrder.None;

        public abstract int Compare(ListViewSubItem x, ListViewSubItem y);
    }
}
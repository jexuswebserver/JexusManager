using System;
using System.Windows.Forms;

namespace JexusManager.Main.Features
{
    public class ListViewColumnCustomSorter : ListViewColumnSorter
    {
        private readonly Func<ListViewItem.ListViewSubItem, ListViewItem.ListViewSubItem, int> _comparer;

        public ListViewColumnCustomSorter(Func<ListViewItem.ListViewSubItem, ListViewItem.ListViewSubItem, int> comparer)
        {
            _comparer = comparer;
        }

        public override int Compare(ListViewItem.ListViewSubItem x, ListViewItem.ListViewSubItem y)
        {
            if (Order == SortOrder.None)
                return 0;
            var temp = _comparer(x, y);
            if (Order == SortOrder.Ascending)
                return temp;
            if (Order == SortOrder.Descending)
                return -temp;
            return 0;
        }
    }
}
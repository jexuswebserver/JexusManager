using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace JexusManager.Main.Features
{
    public class ListViewItemSorter : IComparer
    {
        private int _effectiveCount;

        private readonly LinkedList<ListViewColumnSorter> sortList = new LinkedList<ListViewColumnSorter>();

        public int Compare(object x, object y)
        {
            var xObj = x as ListViewItem;
            var yObj = y as ListViewItem;
            var listViewMain = xObj.ListView;
            var sorting = listViewMain.Sorting;
            //if ((sorting== SortOrder.None))
            //    return 0;
            foreach (var c in sortList)
                if (c.ColumnIndex < _effectiveCount)
                {
                    var temp = c.Compare(xObj.SubItems[c.ColumnIndex], yObj.SubItems[c.ColumnIndex]);
                    if (temp != 0)
                    {
                        if (sorting == SortOrder.Ascending)
                            return temp;
                        if (sorting == SortOrder.Descending)
                            return -temp;
                        return temp;
                    }
                }
            return 0;
        }

        public LinkedListNode<ListViewColumnSorter> Find(int columnIndex)
        {
            var node = sortList.First;
            while (node != null)
            {
                if (node.Value.ColumnIndex == columnIndex)
                    return node;
                node = node.Next;
            }
            return sortList.AddFirst(new ListViewColumnTextSorter
            {
                ColumnIndex = columnIndex,
                Order = SortOrder.Ascending
            });
        }

        public SortOrder PushSort(int column, int effectiveCount)
        {
            _effectiveCount = effectiveCount;
            var node = Find(column);
            if (node.Value.Order == SortOrder.None)
                node.Value.Order = SortOrder.Ascending;
            else if (node.Value.Order == SortOrder.Ascending)
                node.Value.Order = SortOrder.Descending;
            else if (node.Value.Order == SortOrder.Descending)
                node.Value.Order = SortOrder.None;
            //node.List.Remove(node.Value);
            sortList.Remove(node);
            sortList.AddFirst(node);


            return node.Value.Order;
        }
    }
}
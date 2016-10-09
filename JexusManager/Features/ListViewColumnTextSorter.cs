using System.Windows.Forms;

namespace JexusManager.Main.Features
{
    public class ListViewColumnTextSorter : ListViewColumnSorter
    {
        public override int Compare(ListViewItem.ListViewSubItem x, ListViewItem.ListViewSubItem y)
        {
            if (Order == SortOrder.None)
                return 0;
            var temp = (int) InnerCompare(x.Text, y.Text);
            if (Order == SortOrder.Ascending)
                return temp;
            if (Order == SortOrder.Descending)
                return -temp;
            return 0;
        }


        protected virtual ComparerResult InnerCompare(string a, string b)
        {
            // Null parsing.
            if ((a == null) && (b == null))
                return ComparerResult.Equals;
            if ((a == null) && (b != null))
                return ComparerResult.LessThan;
            if ((a != null) && (b == null))
                return ComparerResult.GreaterThan;
            var temp = string.Compare(a, b);

            if (temp == 0) return ComparerResult.Equals;
            if (temp < 0) return ComparerResult.LessThan;
            return ComparerResult.GreaterThan;
        }
    }
}
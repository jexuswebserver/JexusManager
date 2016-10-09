using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Enums;
using Types;
using UI.Enums;
using static System.Windows.Forms.ListViewItem;
using System.Collections;

namespace JexusManager.Main.Features
{
    public abstract class ListViewColumnSorter : IComparer<ListViewSubItem>
    {

        private SortOrder _order = SortOrder.None;

        public int ColumnIndex { get; set; }

        public SortOrder Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public abstract int Compare(ListViewSubItem x, ListViewSubItem y);

    }
    public class ListViewColumnTextSorter : ListViewColumnSorter
    {
        public override int Compare(ListViewSubItem x, ListViewSubItem y)
        {
            if (Order == SortOrder.None)
                return 0;
            else
            {
                var temp = (int)InnerCompare(x.Text, y.Text);
                if (Order == SortOrder.Ascending)
                    return temp;
                else if (Order == SortOrder.Descending)
                    return -temp;
                else
                    return 0;
            }

        }


        virtual protected ComparerResult InnerCompare(string a, string b)
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
            else if (temp < 0) return ComparerResult.LessThan;
            else return ComparerResult.GreaterThan;

        }

    }
    public class ListViewColumnNumericSorter : ListViewColumnTextSorter
    {
        override protected ComparerResult InnerCompare(string a, string b)
        {
            // Null parsing.
            if ((a == null) && (b == null))
                return ComparerResult.Equals;
            if ((a == null) && (b != null))
                return ComparerResult.LessThan;
            if ((a != null) && (b == null))
                return ComparerResult.GreaterThan;
            float singleA;
            float singleB;

            // True And True.
            if (float.TryParse((string)a, out singleA) && float.TryParse((string)b, out singleB))
                return (ComparerResult)singleA.CompareTo(singleB);
            if (float.TryParse((string)a, out singleA) && !float.TryParse((string)b, out singleB))
                return ComparerResult.GreaterThan;
            if (!float.TryParse((string)a, out singleA) && float.TryParse((string)b, out singleB))
                return ComparerResult.LessThan;
            return (ComparerResult)a.CompareTo(b);
        }

    }
    public class ListViewColumnDateSorter : ListViewColumnTextSorter
    {
        public ComparerResult InnerCompare(string a, string b)
        {
            // Null parsing.
            if ((a == null) && (b == null))
                return ComparerResult.Equals;
            if ((a == null) && (b != null))
                return ComparerResult.LessThan;
            if ((a != null) && (b == null))
                return ComparerResult.GreaterThan;
            DateTime dateA;
            DateTime dateB;

            // True And True.
            if (DateTime.TryParse((string)a, out dateA) && DateTime.TryParse((string)b, out dateB))
                return (ComparerResult)dateA.CompareTo(dateB);
            if (DateTime.TryParse((string)a, out dateA) && !DateTime.TryParse((string)b, out dateB))
                return ComparerResult.LessThan;
            if (!DateTime.TryParse((string)a, out dateA) && DateTime.TryParse((string)b, out dateB))
                return ComparerResult.GreaterThan;
            return (ComparerResult)a.CompareTo(b);
        }

    }

    public class ListViewColumnCustomSorter : ListViewColumnSorter
    {
        private Func<ListViewSubItem, ListViewSubItem, int> _comparer;

        public ListViewColumnCustomSorter(Func<ListViewSubItem, ListViewSubItem, int> comparer)
        {
            _comparer = comparer;
        }

        public override int Compare(ListViewSubItem x, ListViewSubItem y)
        {

            if (Order == SortOrder.None)
                return 0;
            else
            {
                var temp = _comparer(x, y);
                if (Order == SortOrder.Ascending)
                    return temp;
                else if (Order == SortOrder.Descending)
                    return -temp;
                else
                    return 0;
            }
        }
    }
}
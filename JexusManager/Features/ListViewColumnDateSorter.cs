using System;

namespace JexusManager.Main.Features
{
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
            if (DateTime.TryParse(a, out dateA) && DateTime.TryParse(b, out dateB))
                return (ComparerResult) dateA.CompareTo(dateB);
            if (DateTime.TryParse(a, out dateA) && !DateTime.TryParse(b, out dateB))
                return ComparerResult.LessThan;
            if (!DateTime.TryParse(a, out dateA) && DateTime.TryParse(b, out dateB))
                return ComparerResult.GreaterThan;
            return (ComparerResult) a.CompareTo(b);
        }
    }
}
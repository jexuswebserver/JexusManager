namespace JexusManager.Main.Features
{
    public class ListViewColumnNumericSorter : ListViewColumnTextSorter
    {
        protected override ComparerResult InnerCompare(string a, string b)
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
            if (float.TryParse(a, out singleA) && float.TryParse(b, out singleB))
                return (ComparerResult) singleA.CompareTo(singleB);
            if (float.TryParse(a, out singleA) && !float.TryParse(b, out singleB))
                return ComparerResult.GreaterThan;
            if (!float.TryParse(a, out singleA) && float.TryParse(b, out singleB))
                return ComparerResult.LessThan;
            return (ComparerResult) a.CompareTo(b);
        }
    }
}
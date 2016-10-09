// ***********************************************************************
// Author   : Elektro
// Modified : 06-January-2016
// ***********************************************************************

#region " Public Members Summary "

#region " Constructors "

// New()

#endregion

#region " Functions "

// Compare(Object, Object) As Integer : Implements IComparer.Compare

#endregion

#endregion

#region " Option Statements "

#endregion

#region " Imports "

using System;
using System.Collections;
using System.Diagnostics;
using Enums;

#endregion

#region " Date Comparer "

namespace Types
{
    /// ----------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Performs a comparison between two <see cref="Date" /> instances.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    public sealed class DateComparer : AestheticObject, IComparer
    {
        #region " Constructors "

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="DateComparer" /> class.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        [DebuggerNonUserCode]
        public DateComparer()
        {
        }

        #endregion

        #region " Public Methods "

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <param name="a">
        ///     The first object to compare.
        /// </param>
        /// <param name="b">
        ///     The second object to compare.
        /// </param>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        ///     A signed integer that indicates the relative values of <paramref name="a" /> and <paramref name="b" />.
        ///     <para></para>
        ///     0: <paramref name="a" /> equals <paramref name="b" />.
        ///     <para></para>
        ///     Less than 0: <paramref name="a" /> is less than <paramref name="b" />.
        ///     <para></para>
        ///     Greater than 0: <paramref name="a" /> is greater than <paramref name="b" />.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        [DebuggerStepThrough]
        int IComparer.Compare(object a, object b)
        {
            return (int) InnerCompare(a, b);
        }

        public ComparerResult InnerCompare(object a, object b)
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
            if (DateTime.TryParse((string) a, out dateA) && DateTime.TryParse((string) b, out dateB))
                return (ComparerResult) dateA.CompareTo(dateB);
            if (DateTime.TryParse((string) a, out dateA) && !DateTime.TryParse((string) b, out dateB))
                return ComparerResult.LessThan;
            if (!DateTime.TryParse((string) a, out dateA) && DateTime.TryParse((string) b, out dateB))
                return ComparerResult.GreaterThan;
            return (ComparerResult) a.ToString().CompareTo(b.ToString());
        }

        #endregion
    }
}

#endregion
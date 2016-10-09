// ***********************************************************************
// Author   : Elektro
// Modified : 06-January-2016
// ***********************************************************************

#region " Public Members Summary "

#region " Constructors "

// New()

#endregion

#region " Functions "

// Compare(Object, Object) As Integer

#endregion

#endregion

#region " Option Statements "

#endregion

#region " Imports "

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using Enums;

#endregion

#region " Text Comparer "

namespace Types
{
    /// ----------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Performs a comparison between two string values.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    public sealed class TextComparer : CaseInsensitiveComparer
    {
        #region " Constructors "

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TextComparer" /> class.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        [DebuggerNonUserCode]
        public TextComparer()
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
        public new int Compare(object a, object b)
        {
            // Null parsing.
            if ((a == null) && (b == null))
                return (int) ComparerResult.Equals;
            if ((a == null) && (b != null))
                return (int) ComparerResult.LessThan;
            if ((a != null) && (b == null))
                return (int) ComparerResult.GreaterThan;
            // True And True.
            if (a is string && b is string)
                return base.Compare(a, b);
            if (a is string && !(b is string))
                return (int) ComparerResult.GreaterThan;
            if (!(a is string) && b is string)
                return (int) ComparerResult.LessThan;
            return (int) ComparerResult.Equals;
        }

        #endregion

        #region " Hidden Base Members "

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerNonUserCode]
        public new int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the <see cref="Type" /> of the current instance.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        ///     The exact runtime type of the current instance.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerNonUserCode]
        public new Type GetType()
        {
            return base.GetType();
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determines whether the specified <see cref="Object" /> is equal to this instance.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <param name="obj">
        ///     Another object to compare to.
        /// </param>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        ///     <see langword="True" /> if the specified <see cref="Object" /> is equal to this instance;
        ///     otherwise, <see langword="False" />.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerNonUserCode]
        public new bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determines whether the specified <see cref="Object" /> instances are considered equal.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <param name="objA">
        ///     The first object to compare.
        /// </param>
        /// <param name="objB">
        ///     The second object to compare.
        /// </param>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        ///     <see langword="True" /> if the objects are considered equal; otherwise, <see langword="False" />.
        ///     <para></para>
        ///     If both <paramref name="objA" /> and <paramref name="objB" /> are <see langword="Nothing" />,
        ///     the method returns <see langword="True" />.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerNonUserCode]
        public new static bool Equals(object objA, object objB)
        {
            return object.Equals(objA, objB);
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determines whether the specified <see cref="Object" /> instances are the same instance.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <param name="objA">
        ///     The first object to compare.
        /// </param>
        /// ----------------------------------------------------------------------------------------------------
        /// <param name="objB">
        ///     The second object to compare.
        /// </param>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        ///     <see langword="True" /> if <paramref name="objA" /> is the same instance as <paramref name="objB" />
        ///     or if both are <see langword="Nothing" />; otherwise, <see langword="False" />.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerNonUserCode]
        public new static bool ReferenceEquals(object objA, object objB)
        {
            return object.ReferenceEquals(objA, objB);
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a String that represents the current object.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        /// ----------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerNonUserCode]
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion
    }
}

#endregion
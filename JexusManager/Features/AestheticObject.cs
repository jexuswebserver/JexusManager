// ***********************************************************************
// Author   : Elektro
// Modified : 25-June-2016
// ***********************************************************************

#region " Option Statements "

#endregion

#region " Imports "

using System;
using System.ComponentModel;
using System.Diagnostics;

#endregion

#region " Aesthetic Object "

namespace Types
{
    /// ----------------------------------------------------------------------------------------------------
    /// <summary>
    ///     This is a class to consume for aesthetic purposes.
    ///     <para></para>
    ///     A default (emptyness) class that inherits from <see cref="object" />, with these base members hidden:
    ///     <para></para>
    ///     <see cref="System.Object.GetHashCode" />, <see cref="System.Object.GetType" />,
    ///     <see cref="System.Object.Equals(object)" />, <see cref="System.Object.Equals(object, object)" />,
    ///     <see cref="System.Object.ReferenceEquals(object, object)" />, <see cref="System.Object.ToString()" />.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    public abstract class AestheticObject : object
    {
        #region " Hidden Base Members (Object) "

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
        public new virtual int GetHashCode()
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
        public new virtual Type GetType()
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
        public new virtual bool Equals(object obj)
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
// ***********************************************************************
// Author   : Elektro
// Modified : 06-January-2016
// ***********************************************************************

#region " Option Statements "

#endregion

#region " Comparer Result "

namespace Enums
{
    /// ----------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Specifies a comparison result.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    public enum ComparerResult
    {
        /// <summary>
        ///     'A' is equals to 'B'.
        /// </summary>
        Equals = 0,

        /// <summary>
        ///     'A' is less than 'B'.
        /// </summary>
        LessThan = -1,

        /// <summary>
        ///     'A' is greater than 'B'.
        /// </summary>
        GreaterThan = 1
    }
}

#endregion
// ***********************************************************************
// Author   : Elektro
// Modified : 06-January-2016
// ***********************************************************************

#region " Option Statements "

#endregion

#region " Sort Modifiers "

namespace UI.Enums
{
    /// ----------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Specifies a Sorting Modifier.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    public enum SortModifiers
    {
        /// <summary>
        ///     Treats the values ​​as text.
        /// </summary>
        SortByText = 0,

        /// <summary>
        ///     Treats the values ​​as numbers.
        /// </summary>
        SortByNumber = 1,

        /// <summary>
        ///     Treats valuesthe values ​​as dates.
        /// </summary>
        SortByDate = 2,
        /// <summary>
        ///  Treats valuesthe values ​​as Custom.
        /// </summary>
        Custom = 3
    }
}

#endregion
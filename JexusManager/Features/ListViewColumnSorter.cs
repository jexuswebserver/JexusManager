// ***********************************************************************
// Author   : Elektro
// Modified : 06-January-2016
// ***********************************************************************

#region " Public Members Summary "

#region " Constructors "

// New()

#endregion

#region " Properties "

// ColumnIndex As Integer
// Order As SortOrder
// SortModifier As SortModifiers

#endregion

#region " Functions "

// Compare(Object, Object) As Integer : Implements IComparer.Compare

#endregion

#endregion

#region " Usage Examples "

// Public Class Form1 : Inherits Form
// 
//     Friend WithEvents MyListView As New ListView
//     Private sorter As New ListViewColumnSorter
// 
//     Public Sub New()
// 
//         MyClass.InitializeComponent()
// 
//         With Me.MyListView
//
//             ' Set the sorter, our ListViewColumnSorter.
//             .ListViewItemSorter = sorter
// 
//             ' The initial direction for the sorting.
//             .Sorting = SortOrder.Ascending
// 
//             ' Set the initial sort-modifier.
//             sorter.SortModifier = SortModifiers.SortByText
// 
//             ' Add some columns.
//             .Columns.Add("Text").Tag = SortModifiers.SortByText
//             .Columns.Add("Numbers").Tag = SortModifiers.SortByNumber
//             .Columns.Add("Dates").Tag = SortModifiers.SortByDate
// 
//             ' Adjust the column sizes.
//             For Each col As ColumnHeader In Me.MyListView.Columns
//                 col.Width = 100
//             Next
// 
//             ' Add some items.
//             .Items.Add("hello").SubItems.AddRange({"2", "11/11/2000"})
//             .Items.Add("yeehaa!").SubItems.AddRange({"1", "9/9/1999"})
//             .Items.Add("El3ktr0").SubItems.AddRange({"100", "21/08/2014"})
//             .Items.Add("wow").SubItems.AddRange({"10", "11-11-2000"})
// 
//             ' Styling things.
//             .Dock = DockStyle.Fill
//             .View = View.Details
//             .FullRowSelect = True
//
//         End With
// 
//         With Me ' Styling things.
//             .Size = New Size(400, 200)
//             .FormBorderStyle =WinForms.FormBorderStyle.FixedSingle
//             .MaximizeBox = False
//             .StartPosition = FormStartPosition.CenterScreen
//             .Text = "ListViewColumnSorter TestForm"
//         End With
// 
//         Me.Controls.Add(Me.MyListView)
// 
//     End Sub
// 
//     ''' ----------------------------------------------------------------------------------------------------
//     ''' <summary>
//     ''' Handles the <see cref="ListView.ColumnClick"/> event of the <see cref="MyListView"/> control.
//     ''' </summary>
//     ''' ----------------------------------------------------------------------------------------------------
//     ''' <param name="sender">
//     ''' The source of the event.
//     ''' </param>
//     ''' 
//     ''' <param name="e">
//     ''' The <see cref="ColumnClickEventArgs"/> instance containing the event data.
//     ''' </param>
//     ''' ----------------------------------------------------------------------------------------------------
//     Private Sub MyListView_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) _
//     Handles MyListView.ColumnClick
// 
//         Dim lv As ListView = DirectCast(sender, ListView)
// 
//         ' Dinamycaly sets the sort-modifier to sort the column by text, number, or date.
//         sorter.SortModifier = DirectCast(lv.Columns(e.Column).Tag, SortModifiers)
// 
//         ' Determine whether clicked column is already the column that is being sorted.
//         If (e.Column = sorter.ColumnIndex) Then
// 
//             ' Reverse the current sort direction for this column.
//             If (sorter.Order = SortOrder.Ascending) Then
//                 sorter.Order = SortOrder.Descending
// 
//             Else
//                 sorter.Order = SortOrder.Ascending
// 
//             End If
// 
//         Else
//             ' Set the column number that is to be sorted, default to ascending.
//             sorter.ColumnIndex = e.Column
//             sorter.Order = SortOrder.Ascending
// 
//         End If ' e.Column
// 
//         ' Perform the sort.
//         lv.Sort()
// 
//     End Sub
// 
// End Class

#endregion

#region " Option Statements "

#endregion

#region " Imports "

using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using Enums;
using Types;
using UI.Enums;

//using Elektro.Application.Enums;
//using Elektro.Application.Types;
//using Elektro.Application.UI.Enums;
//using Elektro.Core.Types;

#endregion

#region " ListView's Column Sorter "

namespace UI.Types
{
    /// ----------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Performs a sorting operation in a <see cref="ListView" />.
    /// </summary>
    /// ----------------------------------------------------------------------------------------------------
    /// <example>
    ///     This is a code example.
    ///     <code>
    /// Public Class Form1 : Inherits Form
    /// 
    ///     Friend WithEvents MyListView As New ListView
    ///     Private sorter As New ListViewColumnSorter
    /// 
    ///     Public Sub New()
    /// 
    ///         MyClass.InitializeComponent()
    /// 
    ///         With Me.MyListView
    /// 
    ///             ' Set the sorter, our ListViewColumnSorter.
    ///             .ListViewItemSorter = sorter
    /// 
    ///             ' The initial direction for the sorting.
    ///             .Sorting = SortOrder.Ascending
    /// 
    ///             ' Set the initial sort-modifier.
    ///             sorter.SortModifier = SortModifiers.SortByText
    /// 
    ///             ' Add some columns.
    ///             .Columns.Add("Text").Tag = SortModifiers.SortByText
    ///             .Columns.Add("Numbers").Tag = SortModifiers.SortByNumber
    ///             .Columns.Add("Dates").Tag = SortModifiers.SortByDate
    /// 
    ///             ' Adjust the column sizes.
    ///             For Each col As ColumnHeader In Me.MyListView.Columns
    ///                 col.Width = 100
    ///             Next
    /// 
    ///             ' Add some items.
    ///             .Items.Add("hello").SubItems.AddRange({"2", "11/11/2000"})
    ///             .Items.Add("yeehaa!").SubItems.AddRange({"1", "9/9/1999"})
    ///             .Items.Add("El3ktr0").SubItems.AddRange({"100", "21/08/2014"})
    ///             .Items.Add("wow").SubItems.AddRange({"10", "11-11-2000"})
    /// 
    ///             ' Styling things.
    ///             .Dock = DockStyle.Fill
    ///             .View = View.Details
    ///             .FullRowSelect = True
    ///         End With
    /// 
    ///         With Me ' Styling things.
    ///             .Size = New Size(400, 200)
    ///             .FormBorderStyle =WinForms.FormBorderStyle.FixedSingle
    ///             .MaximizeBox = False
    ///             .StartPosition = FormStartPosition.CenterScreen
    ///             .Text = "ListViewColumnSorter TestForm"
    ///         End With
    /// 
    ///         Me.Controls.Add(Me.MyListView)
    /// 
    ///     End Sub
    /// 
    ///     ''' ----------------------------------------------------------------------------------------------------
    ///     ''' &lt;summary&gt;
    ///     ''' Handles the &lt;see cref="ListView.ColumnClick"/&gt; event of the &lt;see cref="MyListView"/&gt; control.
    ///     ''' &lt;/summary&gt;
    ///     ''' ----------------------------------------------------------------------------------------------------
    ///     ''' &lt;param name="sender"&gt;
    ///     ''' The source of the event.
    ///     ''' &lt;/param&gt;
    ///     ''' 
    ///     ''' &lt;param name="e"&gt;
    ///     ''' The &lt;see cref="ColumnClickEventArgs"/&gt; instance containing the event data.
    ///     ''' &lt;/param&gt;
    ///     ''' ----------------------------------------------------------------------------------------------------
    ///     Private Sub MyListView_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) _
    ///     Handles MyListView.ColumnClick
    /// 
    ///         Dim lv As ListView = DirectCast(sender, ListView)
    /// 
    ///         ' Dinamycaly sets the sort-modifier to sort the column by text, number, or date.
    ///         sorter.SortModifier = DirectCast(lv.Columns(e.Column).Tag, SortModifiers)
    /// 
    ///         ' Determine whether clicked column is already the column that is being sorted.
    ///         If (e.Column = sorter.ColumnIndex) Then
    /// 
    ///             ' Reverse the current sort direction for this column.
    ///             If (sorter.Order = SortOrder.Ascending) Then
    ///                 sorter.Order = SortOrder.Descending
    /// 
    ///             Else
    ///                 sorter.Order = SortOrder.Ascending
    /// 
    ///             End If
    /// 
    ///         Else
    ///             ' Set the column number that is to be sorted, default to ascending.
    ///             sorter.ColumnIndex = e.Column
    ///             sorter.Order = SortOrder.Ascending
    /// 
    ///         End If ' e.Column
    /// 
    ///         ' Perform the sort.
    ///         lv.Sort()
    /// 
    ///     End Sub
    /// 
    /// End Class
    /// </code>
    /// </example>
    /// ----------------------------------------------------------------------------------------------------
    public sealed class ListViewColumnSorter_ : AestheticObject, IComparer
    {
        #region " Private Fields "

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The comparer instance.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        private IComparer comparer;

        #endregion

        #region " Constructors "

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="ListViewColumnSorter" /> class.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        [DebuggerNonUserCode]
        public ListViewColumnSorter_()
        {
            comparer = new TextComparer();
            columnIndexB = 0;
            orderB = SortOrder.None;
            sortModifierB = SortModifiers.SortByText;
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
            var compareResult = ComparerResult.Equals;
            ListViewItem lvItemA;
            ListViewItem lvItemB;

            // Cast the objects to be compared
            lvItemA = (ListViewItem)a;
            lvItemB = (ListViewItem)b;

            var strA = !(lvItemA.SubItems.Count <= columnIndexB) ? lvItemA.SubItems[columnIndexB].Text : null;

            var strB = !(lvItemB.SubItems.Count <= columnIndexB) ? lvItemB.SubItems[columnIndexB].Text : null;

            var listViewMain = lvItemA.ListView;

            // Calculate correct return value based on object comparison

            if (listViewMain.Sorting == SortOrder.None)
                return (int)ComparerResult.Equals;


            if (sortModifierB == SortModifiers.SortByText)
            {
                // Compare the two items
                if ((lvItemA.SubItems.Count <= columnIndexB) && (lvItemB.SubItems.Count <= columnIndexB))
                    compareResult = (ComparerResult)comparer.Compare(null, null);
                else if ((lvItemA.SubItems.Count <= columnIndexB) && (lvItemB.SubItems.Count > columnIndexB))
                    compareResult = (ComparerResult)comparer.Compare(null, strB);
                else if ((lvItemA.SubItems.Count > columnIndexB) && (lvItemB.SubItems.Count <= columnIndexB))
                    compareResult = (ComparerResult)comparer.Compare(strA, null);
                else
                    compareResult = (ComparerResult)comparer.Compare(strA, strB);

                // Me.sortModifierB IsNot SortModifiers.SortByText.
            }
            else
            {
                switch (sortModifierB)
                {
                    case SortModifiers.SortByNumber:
                        if (comparer.GetType() != typeof(NumericComparer))
                            comparer = new NumericComparer();
                        break;
                    case SortModifiers.SortByDate:
                        if (comparer.GetType() != typeof(DateComparer))
                            comparer = new DateComparer();
                        break;
                    default:
                        if (comparer.GetType() != typeof(TextComparer))
                            comparer = new TextComparer();
                        break;
                }

                compareResult = (ComparerResult)comparer.Compare(strA, strB);
            }
            // Me.sortModifierB.Equals(...)

            // Calculate the proper return value based on object comparison.
            if (orderB == SortOrder.Ascending)
                return (int)compareResult;
            if (orderB == SortOrder.Descending)
                return -(int)compareResult;
            return 0;
            // Me.orderB = ...
        }

        #endregion

        #region " Properties "

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the index of the column to which to apply the sorting operation (default index is <c>0</c>).
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <value>
        ///     The index of the column to which to apply the sorting operation (default index is <c>0</c>).
        /// </value>
        /// ----------------------------------------------------------------------------------------------------
        public int ColumnIndex
        {
            [DebuggerStepThrough]
            get { return columnIndexB; }
            [DebuggerStepThrough]
            set { columnIndexB = value; }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     ( Backing field )
        ///     The index of the column to which to apply the sorting operation (default index is <c>0</c>).
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        private int columnIndexB;

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the order of sorting to apply.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <value>
        ///     The order of sorting to apply.
        /// </value>
        /// ----------------------------------------------------------------------------------------------------
        public SortOrder Order
        {
            [DebuggerStepThrough]
            get { return orderB; }
            [DebuggerStepThrough]
            set { orderB = value; }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     ( Backing field )
        ///     The order of sorting to apply.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        private SortOrder orderB;

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the sort modifier.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        /// <value>
        ///     The sort modifier.
        /// </value>
        /// ----------------------------------------------------------------------------------------------------
        public SortModifiers SortModifier
        {
            [DebuggerStepThrough]
            get { return sortModifierB; }
            [DebuggerStepThrough]
            set { sortModifierB = value; }
        }

        public IComparer Comparer
        {
            get
            {
                return comparer;
            }

            set
            {
                comparer = value;
            }
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     ( Backing field )
        ///     The sort modifier.
        /// </summary>
        /// ----------------------------------------------------------------------------------------------------
        private SortModifiers sortModifierB;

        #endregion
    }
}

#endregion
// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;
using System.Windows.Forms;
using System;

namespace JexusManager
{
    using System.Windows.Forms;

    public class ColumnClickHook
    {
        // Simple comparer for sorting ListView items by text.
        private class ListViewItemComparer : System.Collections.IComparer
        {
            private readonly int _column;
            private readonly SortOrder _order;

            public ListViewItemComparer(int column, SortOrder order)
            {
                _column = column;
                _order = order;
            }

            public int Compare(object x, object y)
            {
                var itemX = x as ListViewItem;
                var itemY = y as ListViewItem;

                if (itemX == null || itemY == null)
                {
                    return 0;
                }

                int result = string.Compare(itemX.SubItems[_column].Text, itemY.SubItems[_column].Text);

                return _order == SortOrder.Descending ? -result : result;
            }
        }
            
        private int _sortColumn = -1;

        public void HandleColumnClick(ListView listView1)
        {
            listView1.ColumnClick += (o, e) =>
            {
                // Determine if the clicked column is already the one being sorted.
                if (e.Column == _sortColumn)
                {
                    // Reverse the current sort direction for this column.
                    listView1.Sorting = listView1.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    _sortColumn = e.Column;
                    listView1.Sorting = SortOrder.Ascending;
                }

                // Sort the items in the ListView.
                listView1.Sorting = listView1.Sorting;
                listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, listView1.Sorting);
                listView1.Sort();

                // Update the column header to show the sorting indicator
                IndicatorHelper.SetSortIcon(listView1, e.Column, listView1.Sorting);
            };
        }
    }

    public static class IndicatorHelper
    {
        private const int HDI_FORMAT = 0x0004;
        private const int HDF_SORTUP = 0x0400; // Arrow pointing up
        private const int HDF_SORTDOWN = 0x0200; // Arrow pointing down
        private const int LVM_GETHEADER = 0x101F;
        private const int HDM_GETITEM = 0x120B;
        private const int HDM_SETITEM = 0x120C;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct HDITEM
        {
            public int mask;
            public int cxy;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public int fmt;
            public IntPtr lParam;
            // If running on 64-bit, add padding
            public int iImage;
            public int iOrder;
        }

        public static void SetSortIcon(ListView listView, int columnIndex, SortOrder order)
        {
            IntPtr headerHandle = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int i = 0; i < listView.Columns.Count; i++)
            {
                var item = new HDITEM
                {
                    mask = HDI_FORMAT
                };

                // Get the current header item
                IntPtr itemPtr = Marshal.AllocHGlobal(Marshal.SizeOf(item));
                Marshal.StructureToPtr(item, itemPtr, false);
                SendMessage(headerHandle, HDM_GETITEM, new IntPtr(i), itemPtr);

                // Update the format to add or remove the sort arrow
                item = Marshal.PtrToStructure<HDITEM>(itemPtr);
                if (i == columnIndex)
                {
                    if (order == SortOrder.Ascending)
                    {
                        item.fmt &= ~HDF_SORTDOWN; // Remove the descending flag
                        item.fmt |= HDF_SORTUP;   // Add the ascending flag
                    }
                    else if (order == SortOrder.Descending)
                    {
                        item.fmt &= ~HDF_SORTUP;  // Remove the ascending flag
                        item.fmt |= HDF_SORTDOWN; // Add the descending flag
                    }
                }
                else
                {
                    // Remove any sorting indicator for other columns
                    item.fmt &= ~HDF_SORTUP;
                    item.fmt &= ~HDF_SORTDOWN;
                }

                // Set the updated header item
                Marshal.StructureToPtr(item, itemPtr, false);
                SendMessage(headerHandle, HDM_SETITEM, new IntPtr(i), itemPtr);
                Marshal.FreeHGlobal(itemPtr);
            }
        }
    }
}

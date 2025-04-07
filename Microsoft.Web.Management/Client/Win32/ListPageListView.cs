// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Forms;
using System.ComponentModel;

namespace Microsoft.Web.Management.Client.Win32
{
    public class ListPageListView : ListView
    {
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        public void ShowSortColumn(ColumnHeader column, SortOrder sortOrder)
        {
        }

        protected override void WndProc(ref Message m)
        { }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override ContextMenuStrip ContextMenuStrip { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new View View
        {
            get
            {
                return base.View;
            }
            set
            {
                base.View = value;
                // TODO:
            }
        }
    }
}

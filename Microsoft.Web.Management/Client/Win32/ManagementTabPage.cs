// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace Microsoft.Web.Management.Client.Win32
{
    public class ManagementTabPage : TabPage
    {
        protected override void OnRightToLeftChanged(
            EventArgs e
            )
        { }

        protected override CreateParams CreateParams { get; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RightToLeftLayout { get; set; }
    }
}

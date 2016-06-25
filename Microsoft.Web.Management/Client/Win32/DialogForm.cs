// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
#if DESIGN
    public class DialogForm : BaseForm
#else
    public abstract class DialogForm : BaseForm
#endif
    {
#if DESIGN
        public DialogForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;
            ShowIcon = false;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
        }
#endif

        protected DialogForm(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;
            ShowIcon = false;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
        }
    }
}
// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.Web.Management.Client.Win32
{
    public interface IManagementUIService
    {
        void ShowContextMenu(Point location);

        DialogResult ShowDialog(DialogForm form);

        void ShowError(Exception exception, string message, string caption, bool isWarning);

        void ShowMessage(string text, string caption);

        DialogResult ShowMessage(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);

        DialogResult ShowMessage(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);

        Object StartProgress(string text, MethodInvoker cancelMethod);

        void StopProgress(Object token);

        void Update();

        ManagementUIColorTable Colors { get; }
        IWin32Window DialogOwner { get; }
        bool RightToLeftLayout { get; }
        IDictionary Styles { get; }
    }
}

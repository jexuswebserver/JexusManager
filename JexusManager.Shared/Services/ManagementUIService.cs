// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Services
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    using Microsoft.Web.Management.Client.Win32;

    public sealed class ManagementUIService : IManagementUIService
    {
        public ManagementUIService(IWin32Window dialogOwner)
        {
            DialogOwner = dialogOwner;
        }

        public void ShowContextMenu(Point location)
        {
        }

        public DialogResult ShowDialog(DialogForm form)
        {
            return form.ShowDialog(DialogOwner);
        }

        public void ShowError(Exception exception, string message, string caption, bool isWarning)
        {
        }

        public void ShowMessage(string text, string caption)
        {
            MessageBox.Show(DialogOwner, text, caption);
        }

        public DialogResult ShowMessage(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(DialogOwner, text, caption, buttons, icon);
        }

        public DialogResult ShowMessage(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton)
        {
            return MessageBox.Show(DialogOwner, text, caption, buttons, icon, defaultButton);
        }

        public object StartProgress(string text, MethodInvoker cancelMethod)
        {
            return null;
        }

        public void StopProgress(object token)
        {
        }

        public void Update()
        {
        }

        public ManagementUIColorTable Colors
        {
            get { return null; }
        }

        public IWin32Window DialogOwner { get; }

        public bool RightToLeftLayout
        {
            get { return false; }
        }

        public IDictionary Styles
        {
            get { return null; }
        }
    }
}
// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;

// ReSharper disable InconsistentNaming

namespace JexusManager
{
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class NativeMethods
    {
        [DllImport("user32")]
        public static extern uint SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        internal const int BCM_SETSHIELD = (0x1600 + 0x000C); //Elevated button

        private static void AddShieldToButton(Button b)
        {
            b.FlatStyle = FlatStyle.System;
            SendMessage(b.Handle, BCM_SETSHIELD, IntPtr.Zero, (IntPtr)1);
        }

        public static void TryAddShieldToButton(Button button)
        {
            if (!PublicNativeMethods.IsProcessElevated)
            {
                AddShieldToButton(button);
            }
        }

        public static void RemoveShieldFromButton(Button b)
        {
            b.FlatStyle = FlatStyle.System;
            SendMessage(b.Handle, BCM_SETSHIELD, IntPtr.Zero, (IntPtr)0);
        }

        public const int NonExistingStore = -2147024894; //0x80070002
        public const int UserCancelled = -2147023673;
        public const int BadKeySet = -2146893802;
    }
}

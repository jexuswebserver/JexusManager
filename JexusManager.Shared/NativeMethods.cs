// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Web.Administration;

// ReSharper disable InconsistentNaming

namespace JexusManager
{
    using System.Drawing;
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

       
        // BOOL WINAPI CryptAcquireCertificatePrivateKey(
        //		PCCERT_CONTEXT pCert,
        //		DWORD dwFlags,
        //		void* pvReserved,
        //		HCRYPTPROV* phCryptProv,
        //		DWORD* pdwKeySpec,
        //		BOOL* pfCallerFreeProv
        // );
        [DllImport("Crypt32.dll", SetLastError = true)]

        public static extern bool CryptAcquireCertificatePrivateKey(
            IntPtr pCert,
            int dwFlags,
            IntPtr pvReserved,
            ref IntPtr phCryptProv,
            ref int pdwKeySpec,
            ref bool pfCallerFreeProv);
        
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("user32.dll")]
        static extern IntPtr LoadImage(
            IntPtr hinst,
            string lpszName,
            uint uType,
            int cxDesired,
            int cyDesired,
            uint fuLoad);

        public static Bitmap GetShieldIcon()
        {
            var size = SystemInformation.SmallIconSize;
            var image = LoadImage(IntPtr.Zero, "#106", 1, size.Width, size.Height, 0);

            if (image == IntPtr.Zero)
            {
                return null;
            }

            using var icon = Icon.FromHandle(image);
            var bitmap = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.DrawIcon(icon, new Rectangle(0, 0, size.Width, size.Height));
            }

            return bitmap;
        }
    }
}

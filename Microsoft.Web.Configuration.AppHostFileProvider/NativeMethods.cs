// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Security.Cryptography;

namespace Microsoft.ApplicationHost
{
    internal static class NativeMethods
    {
        internal sealed class SafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeCryptProvHandle() : base(ownsHandle: true)
            {
            }

            public SafeCryptProvHandle(nuint handle) : base(ownsHandle: true)
            {
                SetHandle(NUintToIntPtr(handle));
            }

            internal unsafe static SafeCryptProvHandle AcquireMachineContext(string keyContainerName, string providerName, uint providerType, bool useMachineContainer)
            {
                uint dwFlags = useMachineContainer ? (uint)CRYPT_KEY_FLAGS.CRYPT_MACHINE_KEYSET : 0u;
                SafeCryptProvHandle hCryptProv;
                
                unsafe
                {
                    nuint phProv;
                    bool success = PInvoke.CryptAcquireContext(
                        out phProv,
                        keyContainerName,
                        providerName,
                        providerType,
                        dwFlags);
                    
                    hCryptProv = new SafeCryptProvHandle(phProv);
                    
                    if (!success)
                    {
                        int lastWin32Error = Marshal.GetLastWin32Error();
                        if (lastWin32Error != HRESULT.NTE_EXISTS)
                        {
                            throw new CryptographicException(lastWin32Error);
                        }
                    }
                }
                
                return hCryptProv;
            }

            protected override bool ReleaseHandle()
            {
                unsafe
                {
                    return PInvoke.CryptReleaseContext(IntPtrToNUint(handle), 0);
                }
            }
        }

        internal static nuint IntPtrToNUint(IntPtr ptr)
        {
            return unchecked((nuint)(ulong)ptr);
        }

        internal static IntPtr NUintToIntPtr(nuint ptr)
        {
            return unchecked((IntPtr)(ulong)ptr);
        }

        #region CNG Support
        [DllImport("cngkeyhelper.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint IisCngDecrypt(string pszKeyName, [In][Out][MarshalAs(UnmanagedType.LPArray)] byte[] pbData, [In][Out] ref uint pcbData);

        [DllImport("cngkeyhelper.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint IisCngEncrypt(string pszKeyName, string pszClearText, [Out][MarshalAs(UnmanagedType.LPArray)] byte[] pbOutput, out uint pcbResult);

        public static string CngDecrypt(byte[] encrypted, string keyContainer)
        {
            uint pcbData = (uint)encrypted.Length;
            byte[] array = new byte[pcbData];
            encrypted.CopyTo(array, 0);
            if (IisCngDecrypt(keyContainer, array, ref pcbData) != 0L)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true).GetString(array, 0, (int)(pcbData - 2));
        }

        public static byte[] CngEncrypt(string data, string keyContainer)
        {
            uint pcbResult = 0u;
            byte[] pbOutput = null;
            if (IisCngEncrypt(keyContainer, data, pbOutput, out pcbResult) != 0L)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            pbOutput = new byte[pcbResult];
            if (IisCngEncrypt(keyContainer, data, pbOutput, out pcbResult) != 0L)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return pbOutput;
        }
        #endregion
    }
}

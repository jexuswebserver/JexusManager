// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.ApplicationHost
{
    internal static class NativeMethods
    {
        #region AES
        internal const int PROV_RSA_AES = 24;

        internal const uint AT_KEYEXCHANGE = 1u;

        internal const int ERROR_MORE_DATA = 234;

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptDecrypt(IntPtr hKey, IntPtr hHash, [MarshalAs(UnmanagedType.Bool)] bool Final, uint dwFlags, byte[] pbData, ref int pdwDataLen);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern bool CryptDestroyKey([In] IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, uint dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool CryptImportKey(SafeCryptProvHandle hProviderKey, byte[] SessionKeyData, int dwDataLength, IntPtr hPubKey, uint dwFlags, ref IntPtr hEncryptKey);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CryptGetUserKey(SafeCryptProvHandle hCryptProv, uint pdwKeySpec, ref IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool CryptGenRandom(SafeCryptProvHandle hProviderKey, uint dwLen, byte[] pbBuffer);

        internal sealed class SafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private const int NTE_EXISTS = -2146893809;

            private const uint CRYPT_NEWKEYSET = 8u;

            private const uint CRYPT_MACHINE_KEYSET = 32u;

            private SafeCryptProvHandle()
                : base(ownsHandle: true)
            {
            }

            public SafeCryptProvHandle(IntPtr handle)
                : base(ownsHandle: true)
            {
                SetHandle(handle);
            }

            internal static SafeCryptProvHandle AcquireMachineContext(string keyContainerName, string providerName, uint providerType, bool useMachineContainer)
            {
                uint dwFlags = 0u;
                if (useMachineContainer)
                {
                    dwFlags = 32u;
                }
                SafeCryptProvHandle hCryptProv;
                bool num = CryptAcquireContextW(out hCryptProv, keyContainerName, providerName, providerType, dwFlags);
                int lastWin32Error = Marshal.GetLastWin32Error();
                if (!num && !CryptAcquireContextW(out hCryptProv, keyContainerName, providerName, providerType, dwFlags))
                {
                    lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error != -2146893809)
                    {
                        throw new CryptographicException(lastWin32Error);
                    }
                }
                return hCryptProv;
            }

            protected override bool ReleaseHandle()
            {
                return CryptReleaseContext(handle, 0u);
            }

            [DllImport("advapi32.dll")]
            private static extern bool CryptReleaseContext(IntPtr hCryptProv, uint dwFlags);

            [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern bool CryptAcquireContextW(out SafeCryptProvHandle hCryptProv, [In][MarshalAs(UnmanagedType.LPWStr)] string pszContainer, [In][MarshalAs(UnmanagedType.LPWStr)] string pszProvider, [In] uint dwProvType, [In] uint dwFlags);
        }
        #endregion

        #region CNG
        [DllImport("cngkeyhelper.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint IisCngDecrypt(string pszKeyName, [In][Out][MarshalAs(UnmanagedType.LPArray)] byte[] pbData, [In][Out] ref uint pcbData);

        [DllImport("cngkeyhelper.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint IisCngEncrypt(string pszKeyName, string pszClearText, [Out][MarshalAs(UnmanagedType.LPArray)] byte[] pbOutput, out uint pcbResult);

        public static string CngDecrypt(byte[] encrypted, string keyContainer)
        {
            uint pcbData = (uint)encrypted.Length;
            byte[] array = new byte[pcbData];
            encrypted.CopyTo(array, 0);
            if ((long)IisCngDecrypt(keyContainer, array, ref pcbData) != 0L)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true).GetString(array, 0, (int)(pcbData - 2));
        }

        public static byte[] CngEncrypt(string data, string keyContainer)
        {
            uint pcbResult = 0u;
            byte[] pbOutput = null;
            if ((long)IisCngEncrypt(keyContainer, data, pbOutput, out pcbResult) != 0L)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            pbOutput = new byte[pcbResult];
            if ((long)IisCngEncrypt(keyContainer, data, pbOutput, out pcbResult) != 0L)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return pbOutput;
        }

        #endregion
    }
}

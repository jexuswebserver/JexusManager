// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System;

namespace Microsoft.ApplicationHost
{
    internal class AesEncryptionProvider : IEncryptionProvider
    {
        private string _sessionKey;

        private bool _useOAEP;

        private string _keyContainerName;

        private bool _userMachineContainer;

        private string _cspProviderName;

        public AesEncryptionProvider(string sessionKey, string keyContainerName, string cspProviderName, bool useOAEP, bool userMachineContainer)
        {
            _sessionKey = sessionKey;
            _keyContainerName = keyContainerName;
            _cspProviderName = cspProviderName;
            _useOAEP = useOAEP;
            _userMachineContainer = userMachineContainer;
        }

        public string Decrypt(string data)
        {
            return AesDecrypt(EncodingHelper.Decode(data), _keyContainerName, _cspProviderName, EncodingHelper.Decode(_sessionKey), _useOAEP, _userMachineContainer);
        }

        public string Encrypt(string data)
        {
            return EncodingHelper.Encode(AesEncrypt(data, _keyContainerName, _cspProviderName, EncodingHelper.Decode(_sessionKey), _useOAEP, _userMachineContainer));
        }

	    public static string AesDecrypt(byte[] encrypted, string keyContainer, string cspProvider, byte[] sessionKey, bool useOAEP, bool useMachineContainer)
        {
            using NativeMethods.SafeCryptProvHandle safeCryptProvHandle = NativeMethods.SafeCryptProvHandle.AcquireMachineContext(keyContainer, cspProvider, 24u, useMachineContainer);
            IntPtr hKey = IntPtr.Zero;
            IntPtr hEncryptKey = IntPtr.Zero;
            try
            {
                if (!NativeMethods.CryptGetUserKey(safeCryptProvHandle, 1u, ref hKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                uint dwFlags = 0u;
                if (useOAEP)
                {
                    dwFlags = 64u;
                }
                if (!NativeMethods.CryptImportKey(safeCryptProvHandle, sessionKey, sessionKey.Length, hKey, dwFlags, ref hEncryptKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                byte[] array = new byte[encrypted.Length];
                encrypted.CopyTo(array, 0);
                int pdwDataLen = array.Length;
                if (!NativeMethods.CryptDecrypt(hEncryptKey, IntPtr.Zero, Final: true, 0u, array, ref pdwDataLen))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                int num = 0;
                for (int i = 0; i < array.Length; i += 2)
                {
                    if (array[i] == 0 && array[i + 1] == 0)
                    {
                        num = i;
                        break;
                    }
                }
                return Encoding.Unicode.GetString(array, 4, num - 4);
            }
            finally
            {
                if (hKey != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyKey(hKey);
                }
                if (hEncryptKey != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyKey(hEncryptKey);
                }
            }
        }

        public static byte[] AesEncrypt(string data, string keyContainer, string cspProvider, byte[] sessionKey, bool useOAEP, bool useMachineContainer)
        {
            using NativeMethods.SafeCryptProvHandle safeCryptProvHandle = NativeMethods.SafeCryptProvHandle.AcquireMachineContext(keyContainer, cspProvider, 24u, useMachineContainer);
            IntPtr hKey = IntPtr.Zero;
            IntPtr hEncryptKey = IntPtr.Zero;
            try
            {
                byte[] array = PrepareInputData(data, safeCryptProvHandle);
                if (!NativeMethods.CryptGetUserKey(safeCryptProvHandle, 1u, ref hKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                uint dwFlags = 0u;
                if (useOAEP)
                {
                    dwFlags = 64u;
                }
                if (!NativeMethods.CryptImportKey(safeCryptProvHandle, sessionKey, sessionKey.Length, hKey, dwFlags, ref hEncryptKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                int pdwDataLen = array.Length;
                if (!NativeMethods.CryptEncrypt(hEncryptKey, IntPtr.Zero, Final: true, 0u, array, ref pdwDataLen, 0))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error != 234)
                    {
                        throw new Win32Exception(lastWin32Error);
                    }
                }
                byte[] array2 = new byte[pdwDataLen];
                array.CopyTo(array2, 0);
                pdwDataLen = array.Length;
                if (!NativeMethods.CryptEncrypt(hEncryptKey, IntPtr.Zero, Final: true, 0u, array2, ref pdwDataLen, array2.Length))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                return array2;
            }
            finally
            {
                if (hKey != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyKey(hKey);
                }
                if (hEncryptKey != IntPtr.Zero)
                {
                    NativeMethods.CryptDestroyKey(hEncryptKey);
                }
            }
        }

        private static byte[] PrepareInputData(string data, NativeMethods.SafeCryptProvHandle hProvider)
        {
            byte[] array = new byte[1];
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            if (!NativeMethods.CryptGenRandom(hProvider, 1u, array))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            uint num = (uint)array[0] % 16u;
            byte[] array2 = new byte[4 + bytes.Length + num + 2];
            byte[] array3 = new byte[4];
            if (!NativeMethods.CryptGenRandom(hProvider, 4u, array3))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            byte[] array4 = new byte[num + 2];
            if (!NativeMethods.CryptGenRandom(hProvider, num + 2, array4))
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            array4[0] = 0;
            array4[1] = 0;
            array3.CopyTo(array2, 0);
            bytes.CopyTo(array2, 4);
            array4.CopyTo(array2, 4 + bytes.Length);
            return array2;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System;
using Windows.Win32.Security.Cryptography;
using System.Runtime.CompilerServices;

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

	    public unsafe static string AesDecrypt(byte[] encrypted, string keyContainer, string cspProvider, byte[] sessionKey, bool useOAEP, bool useMachineContainer)
        {
            using NativeMethods.SafeCryptProvHandle safeCryptProvHandle = NativeMethods.SafeCryptProvHandle.AcquireMachineContext(keyContainer, cspProvider, 24u, useMachineContainer);
            nuint hKey = 0;
            nuint hEncryptKey = 0;
            try
            {
                var handle = NativeMethods.IntPtrToNUint(safeCryptProvHandle.DangerousGetHandle());
                if (!Windows.Win32.PInvoke.CryptGetUserKey(handle, 1u, out hKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                CRYPT_KEY_FLAGS dwFlags = 0u;
                if (useOAEP)
                {
                    dwFlags = CRYPT_KEY_FLAGS.CRYPT_OAEP;
                }

                ReadOnlySpan<byte> sessionPtr = sessionKey;
                if (!Windows.Win32.PInvoke.CryptImportKey(handle, sessionPtr, hKey, dwFlags, out hEncryptKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                byte[] array = new byte[encrypted.Length];
                encrypted.CopyTo(array, 0);
                uint pdwDataLen = (uint)array.Length;
                fixed (byte* arrayPtr = array)
                {
                    if (!Windows.Win32.PInvoke.CryptDecrypt(hEncryptKey, 0, Final: false, 0u, arrayPtr, ref pdwDataLen))
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
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
                if (hKey != 0)
                {
                    Windows.Win32.PInvoke.CryptDestroyKey(hKey);
                }
                if (hEncryptKey != 0)
                {
                    Windows.Win32.PInvoke.CryptDestroyKey(hEncryptKey);
                }
            }
        }

        public unsafe static byte[] AesEncrypt(string data, string keyContainer, string cspProvider, byte[] sessionKey, bool useOAEP, bool useMachineContainer)
        {
            using NativeMethods.SafeCryptProvHandle safeCryptProvHandle = NativeMethods.SafeCryptProvHandle.AcquireMachineContext(keyContainer, cspProvider, 24u, useMachineContainer);
            nuint hKey = 0;
            nuint hEncryptKey = 0;
            try
            {
                byte[] array = PrepareInputData(data, safeCryptProvHandle);
                nuint handle = NativeMethods.IntPtrToNUint(safeCryptProvHandle.DangerousGetHandle());
                if (!Windows.Win32.PInvoke.CryptGetUserKey(handle, 1u, out hKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                CRYPT_KEY_FLAGS dwFlags = 0u;
                if (useOAEP)
                {
                    dwFlags = CRYPT_KEY_FLAGS.CRYPT_OAEP;
                }

                ReadOnlySpan<byte> sessionPtr = sessionKey;
                if (!Windows.Win32.PInvoke.CryptImportKey(handle, sessionPtr, hKey, dwFlags, out hEncryptKey))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                uint pdwDataLen = (uint)array.Length;
                ReadOnlySpan<byte> arrayPtr = array;
                if (!Windows.Win32.PInvoke.CryptEncrypt(hEncryptKey, 0, Final: true, 0u, arrayPtr, ref pdwDataLen))
                {
                    int lastWin32Error = Marshal.GetLastWin32Error();
                    if (lastWin32Error != 234)
                    {
                        throw new Win32Exception(lastWin32Error);
                    }
                }

                byte[] array2 = new byte[pdwDataLen];
                array.CopyTo(array2, 0);
                pdwDataLen = (uint)array.Length;
                ReadOnlySpan<byte> array2Ptr = array2;
                if (!Windows.Win32.PInvoke.CryptEncrypt(hEncryptKey, 0, Final: true, 0u, array2Ptr, ref pdwDataLen))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                return array2;
            }
            finally
            {
                if (hKey != 0)
                {
                    Windows.Win32.PInvoke.CryptDestroyKey(hKey);
                }
                if (hEncryptKey != 0)
                {
                    Windows.Win32.PInvoke.CryptDestroyKey(hEncryptKey);
                }
            }
        }

        private unsafe static byte[] PrepareInputData(string data, NativeMethods.SafeCryptProvHandle hProvider)
        {
            byte[] array = new byte[1];
            byte[] bytes = Encoding.Unicode.GetBytes(data);
            nuint handle = NativeMethods.IntPtrToNUint(hProvider.DangerousGetHandle());
            fixed (byte* arrayPtr = array)
            {
                if (!Windows.Win32.PInvoke.CryptGenRandom(handle, 1u, arrayPtr))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            uint num = (uint)array[0] % 16u;
            byte[] array2 = new byte[4 + bytes.Length + num + 2];
            byte[] array3 = new byte[4];
            fixed (byte* array3Ptr = array3)
            {
                if (!Windows.Win32.PInvoke.CryptGenRandom(handle, 4u, array3Ptr))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            byte[] array4 = new byte[num + 2];
            fixed (byte* array4Ptr = array4)
            {
                if (!Windows.Win32.PInvoke.CryptGenRandom(handle, num + 2, array4Ptr))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
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

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.ApplicationHost
{
    internal class CngEncryptionProvider : IEncryptionProvider
    {
        private string _keyContainerName;

        public CngEncryptionProvider(string sessionKey, string keyContainerName, string cspProviderName, bool useOAEP, bool userMachineContainer)
        {
            _keyContainerName = keyContainerName;
        }

        public string Decrypt(string data)
        {
            return NativeMethods.CngDecrypt(EncodingHelper.Decode(data), _keyContainerName);
        }

        public string Encrypt(string data)
        {
            return EncodingHelper.Encode(NativeMethods.CngEncrypt(data, _keyContainerName));
        }
    }
}

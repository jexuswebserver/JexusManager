// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
//
// Authors:
//	Duncan Mak (duncan@ximian.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//

namespace Microsoft.Web.Administration
{
    internal class ProtectedConfigurationProvider //: ProviderBase
    {
        public static IEncryptionServiceProvider Provider { get; set; } = new DefaultEncryptionServiceProvider();

        public ProtectedConfigurationProvider(ProviderSettings ps)
        {
            Name = (string)ps["name"];
            _sessionKey = (string)ps["sessionKey"];
            var value = ps["useOAEP"];
            _useOAEP = value != null && bool.Parse(value.ToString());
            _keyContainerName = (string)ps["keyContainerName"];
            var value2 = ps["useMachineContainer"];
            _useMachineContainer = value2 != null && bool.Parse(value2.ToString());
            _cspProviderName = (string)ps["cspProviderName"];
        }

        private string _sessionKey;
        private bool _useOAEP;
        private string _keyContainerName;
        private bool _useMachineContainer;
        private string _cspProviderName;

        public string Decrypt(string data)
        {
            return Provider.Decrypt(data, _keyContainerName, _cspProviderName, _sessionKey, _useOAEP, _useMachineContainer);
        }

        public string Encrypt(string data)
        {
            return Provider.Encrypt(data, _keyContainerName, _cspProviderName, _sessionKey, _useOAEP, _useMachineContainer);
        }

        public string Name { get; }
    }
}

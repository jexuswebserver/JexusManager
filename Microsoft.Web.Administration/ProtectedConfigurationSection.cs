// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//
//
// Authors:
// 	Duncan Mak (duncan@ximian.com)
//	Chris Toshok (toshok@ximian.com)
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
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
//

namespace Microsoft.Web.Administration
{
    internal sealed class ProtectedConfigurationSection
    {
        private ProtectedConfigurationProviderCollection _providers;

        public ProtectedConfigurationSection(ConfigurationSection section)
        {
            DefaultProvider = (string)section["defaultProvider"];
            Providers = new ProviderSettingsCollection(section.GetCollection("providers"));
        }

        public string DefaultProvider { get; set; }

        public ProviderSettingsCollection Providers
        {
            get;
        }

        internal string EncryptSection(string clearXml, ProtectedConfigurationProvider protectionProvider)
        {
            var encryptedNode = protectionProvider.Encrypt(clearXml);
            return encryptedNode;
        }

        internal string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider)
        {
            var decryptedNode = protectionProvider.Decrypt(encryptedXml);
            return decryptedNode;
        }

        internal ProtectedConfigurationProviderCollection GetAllProviders()
        {
            if (_providers == null)
            {
                _providers = new ProtectedConfigurationProviderCollection();

                foreach (ProviderSettings ps in Providers)
                {
                    var provider = InstantiateProvider(ps);
                    if (provider != null)
                    {
                        _providers.Add(provider);
                    }
                }
            }

            return _providers;
        }

        private ProtectedConfigurationProvider InstantiateProvider(ProviderSettings ps)
        {
            // Here we create a new instance of our dynamic provider
            return new ProtectedConfigurationProvider(ps);
        }
    }
}

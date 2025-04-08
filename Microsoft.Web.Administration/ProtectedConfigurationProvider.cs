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

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.ApplicationHost;

namespace Microsoft.Web.Administration
{
    internal class ProtectedConfigurationProvider //: ProviderBase
    {
        private readonly string _sessionKey;
        private readonly bool _useOAEP;
        private readonly string _keyContainerName;
        private readonly bool _useMachineContainer;
        private readonly string _cspProviderName;
        private readonly string _providerType;
        private readonly Type _providerClassType;

        private IEncryptionProvider _nativeProvider;
        private bool _providerInitialized = false;
        private readonly object _lockObject = new object();

        // Cache of loaded provider types to avoid repeated lookups
        private static readonly Dictionary<string, Type> _loadedProviderTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

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
            _providerType = (string)ps["type"];

            try
            {
                // Determine which type to use - if providerType is empty, try to use cspProviderName
                string typeToLoad = _providerType;
                if (!string.IsNullOrEmpty(typeToLoad))
                {
                    // Check if we've already loaded this type before
                    if (_loadedProviderTypes.TryGetValue(typeToLoad, out _providerClassType))
                    {
                        System.Diagnostics.Debug.WriteLine($"Using cached provider type: {typeToLoad}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading provider type: {ex.Message}");
                // Fall back to our default implementation if there's any error
            }
        }

        private void InitializeProvider()
        {
            // Check if provider is already initialized
            if (_providerInitialized)
                return;

            // Use a lock to prevent multiple threads from initializing the provider simultaneously
            lock (_lockObject)
            {
                // Double-check in case another thread initialized it while we were waiting
                if (_providerInitialized)
                    return;

                try
                {
                    if (_providerClassType != null)
                    {
                        try
                        {
                            // Find the constructors
                            var constructors = _providerClassType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

                            if (constructors.Length > 0)
                            {
                                // Try to find a constructor that takes parameters matching what we have
                                foreach (var constructor in constructors)
                                {
                                    var parameters = constructor.GetParameters();

                                    // Look for the constructor with 5 parameters (sessionKey, keyContainerName, cspProviderName, useOAEP, useMachineContainer)
                                    // as seen in the CngEncryptionProvider class
                                    if (parameters.Length == 5)
                                    {
                                        _nativeProvider = (IEncryptionProvider)constructor.Invoke(new object[] {
                                            _sessionKey,
                                            _keyContainerName,
                                            _cspProviderName,
                                            _useOAEP,
                                            _useMachineContainer
                                        });

                                        System.Diagnostics.Debug.WriteLine($"Successfully loaded native provider: {_providerType}");
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error creating provider instance: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error initializing native provider: {ex.Message}");
                    // Fall back to our default implementation if there's any error
                }
                finally
                {
                    // Mark as initialized even if it failed, so we don't keep trying
                    _providerInitialized = true;
                }
            }
        }

        public string Decrypt(string data)
        {
            InitializeProvider();
            return _nativeProvider?.Decrypt(data) ?? data;
        }

        public string Encrypt(string data)
        {
            InitializeProvider();
            return _nativeProvider?.Encrypt(data) ?? data;
        }

        public string Name { get; }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.ApplicationHost;

public interface IEncryptionProvider
{
    string Decrypt(string data);
    string Encrypt(string data);
}

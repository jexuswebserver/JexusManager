// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("0ef8b038-9cf8-4f38-89a9-025da1247816")]
    public enum LoadOptions
    {
        None,
        MemoryBuffer,
        TempFile
    }
}
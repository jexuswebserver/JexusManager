// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("9a99d63e-b1be-457a-aad3-19e65259cf46")]
    public interface IEnumVirtualDirectory
    {
        [DispId(1)]
        IVirtualDirectory GetNext();
    }
}
// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("a4091b10-ad03-46a3-b6aa-bd6d8b312490")]
    public interface IEnumApplications
    {
        [DispId(1)]
        IApplication GetNext();
    }
}

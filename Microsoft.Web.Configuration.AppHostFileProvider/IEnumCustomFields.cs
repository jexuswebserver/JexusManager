// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("a27239f9-e13b-4fff-ae21-2804b6ef6352")]
    public interface IEnumCustomFields
    {
        [DispId(1)]
        ICustomField GetNext();
    }
}
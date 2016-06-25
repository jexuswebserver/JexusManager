// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("18d870b9-f8fc-494b-a762-3a30456911a5")]
    public interface IEnumBindings
    {
        [DispId(1)]
        ISiteBinding GetNext();
    }
}
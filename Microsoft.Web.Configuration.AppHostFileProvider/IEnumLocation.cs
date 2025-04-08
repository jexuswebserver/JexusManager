// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("f8bfb408-2172-46b6-ba25-10504f83d4e0")]
    public interface IEnumLocation
    {
        [DispId(1)]
        ILocation GetNext();
    }
}

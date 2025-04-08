// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("639fda3a-132d-46ce-9737-982bc1ebf7af")]
    public interface IEnumSite
    {
        [DispId(1)]
        IAppHostSite GetNext();
    }
}

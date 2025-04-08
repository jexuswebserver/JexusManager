// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("35cb91fa-9e14-4a54-af4a-dfd871f7cb95")]
    public interface IEnumSchedule
    {
        [DispId(1)]
        ISchedule GetNext();
    }
}

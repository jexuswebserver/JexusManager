// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("73c48d5c-de6b-4f16-8b9c-609af2586b8f")]
    public interface IEnumApplicationPool
    {
        [DispId(1)]
        IApplicationPool GetNext();
    }
}
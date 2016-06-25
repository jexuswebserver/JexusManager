// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("d6091346-8f36-4005-ae05-1be8d0c6e91d")]
    public interface IEnumListenerAdapter
    {
        [DispId(1)]
        IListenerAdapter GetNext();
    }
}
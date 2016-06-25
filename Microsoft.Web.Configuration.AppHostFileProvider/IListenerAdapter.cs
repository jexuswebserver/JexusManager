// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("0c86c5ea-5976-46f7-af2d-e2101f4ab029")]
    public interface IListenerAdapter
    {
        [DispId(1)]
        string Name
        {
            get;
        }
        [DispId(2)]
        string Identity
        {
            get;
        }
        [DispId(3)]
        string ProtocolManagerDll
        {
            get;
        }
        [DispId(4)]
        string ProtocolManagerDllInitFunction
        {
            get;
        }
    }
}
// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("82ab8971-405a-4e82-9bb8-cc48394e000e")]
    public interface ISiteBinding
    {
        [DispId(1)]
        string Protocol
        {
            get;
        }
        [DispId(2)]
        string BindingInformation
        {
            get;
        }
        [DispId(3)]
        uint SslFlags
        {
            get;
        }
    }
}

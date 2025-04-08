// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("66f6c229-8435-4bda-bec7-7562d11c4e33")]
    public interface ICustomField
    {
        [DispId(1)]
        string LogFieldName
        {
            get;
        }
        [DispId(2)]
        string SourceName
        {
            get;
        }
        [DispId(3)]
        uint SourceType
        {
            get;
        }
    }
}

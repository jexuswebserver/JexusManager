// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("d861da67-f550-4208-8c54-99d0f06597fe")]
    public interface ISiteTraceRequestsLogging
    {
        [DispId(1)]
        bool Enabled
        {
            get;
        }
        [DispId(2)]
        string Directory
        {
            get;
        }
        [DispId(3)]
        uint MaxLogFiles
        {
            get;
        }
        [DispId(4)]
        uint MaxLogFileSizeKB
        {
            get;
        }
        [DispId(5)]
        bool CustomActionsEnabled
        {
            get;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("69c3d64b-feba-4e44-b235-df5de5f593bd")]
    public interface ISiteLimits
    {
        [DispId(1)]
        uint MaxBandwidth
        {
            get;
        }
        [DispId(2)]
        uint MaxConnections
        {
            get;
        }
        [DispId(3)]
        long ConnectionTimeout
        {
            get;
        }
        [DispId(4)]
        uint MaxUrlSegments
        {
            get;
        }
    }
}

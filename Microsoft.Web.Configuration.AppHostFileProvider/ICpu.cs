// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("22dcee33-3344-43e4-a89d-c14544700448")]
    public interface ICpu
    {
        [DispId(1)]
        uint Limit
        {
            get;
        }
        [DispId(2)]
        uint Action
        {
            get;
        }
        [DispId(3)]
        long ResetInterval
        {
            get;
        }
        [DispId(4)]
        bool SmpAffinitized
        {
            get;
        }
        [DispId(5)]
        uint SmpProcessorAffinityMask
        {
            get;
        }
        [DispId(6)]
        uint SmpProcessorAffinityMask2
        {
            get;
        }
        [DispId(7)]
        int ProcessorGroup
        {
            get;
        }
        [DispId(8)]
        uint NumaNodeAssignment
        {
            get;
        }
        [DispId(9)]
        uint NumaNodeAffinityMode
        {
            get;
        }
    }
}

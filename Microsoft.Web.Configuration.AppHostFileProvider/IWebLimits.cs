// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("34d720e5-bdf2-4e54-81aa-29356a579eea")]
    public interface IWebLimits
    {
        [DispId(1)]
        uint MaxGlobalBandwidth
        {
            get;
        }
        [DispId(2)]
        long ConnectionTimeout
        {
            get;
        }
        [DispId(3)]
        uint DemandStartThreshold
        {
            get;
        }
        [DispId(4)]
        uint DynamicIdleThreshold
        {
            get;
        }
        [DispId(5)]
        long HeaderWaitTimeout
        {
            get;
        }
        [DispId(6)]
        uint MinBytesPerSec
        {
            get;
        }
        [DispId(7)]
        uint DynamicRegistrationThreshold
        {
            get;
        }
    }
}
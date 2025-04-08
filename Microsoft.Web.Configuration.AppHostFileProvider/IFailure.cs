// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("aaeb5bd0-d96f-4c1a-9737-25f404b2716e")]
    public interface IFailure
    {
        [DispId(1)]
        uint LoadBalancerCapabilities
        {
            get;
        }
        [DispId(2)]
        bool OrphanWorkerProcess
        {
            get;
        }
        [DispId(3)]
        string OrphanActionExe
        {
            get;
        }
        [DispId(4)]
        string OrphanActionParameters
        {
            get;
        }
        [DispId(5)]
        bool RapidFailProtection
        {
            get;
        }
        [DispId(6)]
        long RapidFailProtectionInterval
        {
            get;
        }
        [DispId(7)]
        uint RapidFailProtectionMaxCrashes
        {
            get;
        }
        [DispId(8)]
        string AutoShutdownExe
        {
            get;
        }
        [DispId(9)]
        string AutoShutdownParameters
        {
            get;
        }
    }
}

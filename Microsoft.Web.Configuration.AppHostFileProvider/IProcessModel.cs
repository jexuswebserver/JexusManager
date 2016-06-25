// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("18a51bad-f295-4689-8823-4752abbc76b2")]
    public interface IProcessModel
    {
        [DispId(1)]
        uint IdentityType
        {
            get;
        }
        [DispId(2)]
        string UserName
        {
            get;
        }
        [DispId(3)]
        string Password
        {
            get;
        }
        [DispId(4)]
        bool LoadUserProfile
        {
            get;
        }
        [DispId(5)]
        bool SetProfileEnvironment
        {
            get;
        }
        [DispId(6)]
        uint LogOnType
        {
            get;
        }
        [DispId(7)]
        bool ManualGroupMembership
        {
            get;
        }
        [DispId(8)]
        long IdleTimeout
        {
            get;
        }
        [DispId(9)]
        uint MaxProcesses
        {
            get;
        }
        [DispId(10)]
        long ShutdownTimeLimit
        {
            get;
        }
        [DispId(11)]
        long StartupTimeLimit
        {
            get;
        }
        [DispId(12)]
        bool PingingEnabled
        {
            get;
        }
        [DispId(13)]
        long PingInterval
        {
            get;
        }
        [DispId(14)]
        long PingResponseTime
        {
            get;
        }
        [DispId(15)]
        uint LogEventOnProcessModel
        {
            get;
        }
        [DispId(16)]
        uint IdleTimeoutAction
        {
            get;
        }
    }
}
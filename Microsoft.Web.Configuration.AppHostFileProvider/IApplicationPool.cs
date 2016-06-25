// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("304ee60a-c7d1-4f0d-baae-9a658593504f")]
    public interface IApplicationPool
    {
        [DispId(1)]
        string Name
        {
            get;
        }
        [DispId(2)]
        uint QueueLength
        {
            get;
        }
        [DispId(3)]
        bool AutoStart
        {
            get;
        }
        [DispId(4)]
        bool Enable32BitAppOnWin64
        {
            get;
        }
        [DispId(5)]
        string ManagedRuntimeVersion
        {
            get;
        }
        [DispId(6)]
        string ManagedRuntimeLoader
        {
            get;
        }
        [DispId(7)]
        bool EnableConfigurationOverride
        {
            get;
        }
        [DispId(8)]
        uint ManagedPipelineMode
        {
            get;
        }
        [DispId(9)]
        string ClrConfigFile
        {
            get;
        }
        [DispId(10)]
        bool PassAnonymousToken
        {
            get;
        }
        [DispId(11)]
        uint StartMode
        {
            get;
        }
        [DispId(12)]
        IProcessModel ProcessModel
        {
            get;
        }
        [DispId(13)]
        IRecycling Recycling
        {
            get;
        }
        [DispId(14)]
        IFailure Failure
        {
            get;
        }
        [DispId(15)]
        ICpu Cpu
        {
            get;
        }
        [DispId(16)]
        uint State
        {
            get;
        }
        [DispId(17)]
        IEnumEnvironmentVariables GetEnvironmentVariablesEnumerator();
    }
}
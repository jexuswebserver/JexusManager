// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("a3b9e5cd-a910-4409-9727-86564954d39d")]
    public interface IPeriodicRestart
    {
        [DispId(1)]
        uint Memory
        {
            get;
        }
        [DispId(2)]
        uint PrivateMemory
        {
            get;
        }
        [DispId(3)]
        uint Requests
        {
            get;
        }
        [DispId(4)]
        long Time
        {
            get;
        }
        [DispId(5)]
        IEnumSchedule GetScheduleEnumerator();
    }
}
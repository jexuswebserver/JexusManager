// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("a8ce5f8b-5e34-4992-9738-b7958097be48")]
    public interface IRecycling
    {
        [DispId(1)]
        bool DisallowOverlappingRotation
        {
            get;
        }
        [DispId(2)]
        bool DisallowRotationOnConfigChange
        {
            get;
        }
        [DispId(3)]
        uint LogEventOnRecycle
        {
            get;
        }
        [DispId(4)]
        IPeriodicRestart PeriodicRestart
        {
            get;
        }
    }
}
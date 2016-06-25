// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("4a6df970-adc2-42af-bf9b-ddb791088c65")]
    public interface IEnumEnvironmentVariables
    {
        [DispId(1)]
        IEnvironmentVariable GetNext();
    }
}
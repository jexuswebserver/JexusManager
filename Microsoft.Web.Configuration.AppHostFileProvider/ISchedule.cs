// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("56e54678-6e8f-4354-9a35-0f36266b89d3")]
    public interface ISchedule
    {
        [DispId(1)]
        long Item
        {
            get;
        }
    }
}
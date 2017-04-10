// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public enum RecyclingLogEventOnRecycle
    {
        None = 0,
        Time = 1,
        Requests = 2,
        Schedule = 4,
        Memory = 8,
        IsapiUnhealthy = 16,
        OnDemand = 32,
        ConfigChange = 64,
        PrivateMemory = 128
    }
}

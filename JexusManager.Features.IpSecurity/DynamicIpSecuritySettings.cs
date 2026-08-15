// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.IpSecurity
{
    [Serializable]
    internal sealed class DynamicIpSecuritySettings
    {
        public bool EnableConcurrentDenial { get; set; }

        public uint MaxConcurrentRequests { get; set; }

        public bool EnableRateDenial { get; set; }

        public uint MaxRequests { get; set; }

        public uint RequestIntervalInMilliseconds { get; set; }

        public bool EnableLoggingOnlyMode { get; set; }
    }
}

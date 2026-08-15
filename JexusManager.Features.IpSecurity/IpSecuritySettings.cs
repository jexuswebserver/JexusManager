// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.IpSecurity
{
    [Serializable]
    internal sealed class IpSecuritySettings
    {
        public bool EnableReverseDns { get; set; }

        public bool AllowUnlisted { get; set; }

        public bool? EnableProxyMode { get; set; }

        public long? DenyAction { get; set; }
    }
}

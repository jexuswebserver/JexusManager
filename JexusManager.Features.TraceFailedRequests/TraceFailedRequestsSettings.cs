// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace JexusManager.Features.TraceFailedRequests
{
    [Serializable]
    internal sealed class TraceFailedRequestsSettings
    {
        public bool Enabled { get; set; }

        public string Directory { get; set; } = string.Empty;

        public long MaxLogFiles { get; set; }
    }
}

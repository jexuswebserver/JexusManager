// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public sealed class ConnectionActiveState
    {
        public bool Active { get; }
        public bool DifferentCredentials { get; }
        public ConnectionInfo SimilarConnection { get; }
    }
}
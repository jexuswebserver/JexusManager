// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public sealed class ConnectionEventArgs : EventArgs
    {
        public Connection Connection { get; }
        public bool IsNewConnection { get; }
    }
}
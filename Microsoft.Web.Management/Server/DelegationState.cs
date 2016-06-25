// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public class DelegationState
    {
        public DelegationState(
            string mode,
            string text,
            string description
            )
        { }

        public string Description { get; }
        public string Mode { get; }
        public string Text { get; }
    }
}
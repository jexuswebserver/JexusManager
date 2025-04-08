// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Microsoft.Web.Management.Client
{
    public sealed class CredentialInfoEventArgs : CancelEventArgs
    {
        public CredentialInfoEventArgs(
            CredentialInfo credentials
            )
        {
            Credentials = credentials;
        }

        public CredentialInfo Credentials { get; }
    }
}

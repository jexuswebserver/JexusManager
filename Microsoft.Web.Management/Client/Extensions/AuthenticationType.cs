// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client.Extensions
{
    public enum AuthenticationType
    {
        ChallengeBase = 0,
        LoginRedirectBased = 1,
        ClientCertificateBased = 2,
        Other = 3
    }
}
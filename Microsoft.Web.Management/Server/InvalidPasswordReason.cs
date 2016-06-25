// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Server
{
    public enum InvalidPasswordReason
    {
        NoError = 0,
        PasswordTooShort = 1,
        PasswordTooLong = 2,
        PasswordNotComplexEnough = 3,
        PasswordFilterError = 4,
        UnknownError = 5
    }
}
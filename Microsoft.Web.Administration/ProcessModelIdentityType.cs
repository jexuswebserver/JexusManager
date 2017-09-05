// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public enum ProcessModelIdentityType
    {
        LocalSystem = 0,
        LocalService = 1,
        NetworkService = 2,
        SpecificUser = 3,
        ApplicationPoolIdentity = 4
    }
}

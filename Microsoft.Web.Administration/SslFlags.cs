// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    [Flags]
    public enum SslFlags
    {
        None = 0,
        Sni = 1,
        CentralCertStore = 2
    }
}

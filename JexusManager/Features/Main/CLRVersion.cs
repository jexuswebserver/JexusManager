// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager
{
    using System.ComponentModel;

    internal enum CLRVersion
    {
        [Description("v4.0")]
        V40,
        [Description("v2.0")]
        V20,
        [Description("No Managed Code")]
        NoManagedCode
    }
}

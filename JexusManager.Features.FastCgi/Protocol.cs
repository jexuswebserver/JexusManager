// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System.ComponentModel;

    internal enum Protocol : long
    {
        [Description("NamedPipe")]
        NamedPipe = 0,

        [Description("Tcp")]
        Tcp = 1
    }
}

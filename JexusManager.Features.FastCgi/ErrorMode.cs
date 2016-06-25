// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.FastCgi
{
    using System.ComponentModel;

    internal enum ErrorMode : long
    {
        [Description("ReturnStdErrIn500")]
        ReturnStdErrIn500 = 0,

        [Description("ReturnGeneric500")]
        ReturnGeneric500 = 1,

        [Description("IgnoreAndReturn200")]
        IgnoreAndReturn200 = 2,

        [Description("TerminateProcess")]
        TerminateProcess = 3
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Microsoft.Web.Administration
{
    public enum ProcessorAction : long
    {
        [Description("NoAction")]
        NoAction = 0,

        [Description("KillW3wp")]
        KillW3wp = 1,

        [Description("Throttle")]
        Throttle = 2,

        [Description("ThrottleUnderload")]
        ThrottleUnderLoad = 3
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Microsoft.Web.Administration
{
    public enum IdleTimeoutAction
    {
        [Description("Terminate")]
        Terminate = 0,

        [Description("Suspend")]
        Suspend = 1
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Microsoft.Web.Administration
{
    public enum StartMode
    {
        [Description("OnDemand")]
        OnDemand = 0,

        [Description("AlwaysRunning")]
        AlwaysRunning = 1
    }
}

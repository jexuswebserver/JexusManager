// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Microsoft.Web.Administration
{
    public enum WorkingMode
    {
        [Description("Not supported")]
        NotSupported = 0,

        [Description("IIS")]
        Iis = 1,

        [Description("IIS Express")]
        IisExpress = 2,

        [Description("Jexus")]
        Jexus = 3
    }
}

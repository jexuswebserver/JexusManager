// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Microsoft.Web.Administration
{
    public enum ManagedPipelineMode
    {
        [Description("Integrated")]
        Integrated = 0,

        [Description("Classic")]
        Classic = 1
    }
}

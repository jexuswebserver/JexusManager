// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    [Flags]
    public enum ModuleListPageViewModes
    {
        Details = 1,
        Icons = 2,
        Tiles = 4,
        List = 8
    }
}
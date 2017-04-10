// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Microsoft.Web.Administration
{
    [Obfuscation(Exclude = true)]
    public enum OverrideMode
    {
        Unknown = 0,
        Inherit = 1,
        Allow = 2,
        Deny = 3
    }
}

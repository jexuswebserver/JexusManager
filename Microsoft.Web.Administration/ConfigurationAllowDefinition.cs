// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    internal enum ConfigurationAllowDefinition
    {
        MachineOnly = 0,
        MachineToWebRoot = 100,
        MachineToApplication = 200,
        Everywhere = 300,
        AppHostOnly = 400
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features.RequestFiltering
{
    using Microsoft.Web.Management.Client;

    internal interface IRequestFilteringFeature
    {
        TaskList GetTaskList();

        bool ShowHelp();

        RequestFilteringSettingsSavedEventHandler RequestFilteringSettingsUpdate { get; }
    }
}

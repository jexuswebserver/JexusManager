// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.HttpApi
{
    internal interface IHttpApiFeature
    {
        TaskList GetTaskList();

        bool ShowHelp();

        HttpApiSettingsSavedEventHandler HttpApiSettingsUpdate { get; }
    }
}

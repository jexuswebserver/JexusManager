// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace Microsoft.Web.Management.Client
{
    public interface IModulePage : IDisposable
    {
        void Initialize(Module module, ModulePageInfo pageInfo, Object navigationData);

        void OnActivated(bool initialActivation);

        void OnDeactivating(CancelEventArgs e);

        void Refresh();

        bool ShowHelp();

        bool CanRefresh { get; }
        bool HasChanges { get; }
        ModulePageInfo PageInfo { get; }
        TaskListCollection Tasks { get; }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client.Extensions
{
    public abstract class HomepageExtension : IHomepageTaskListProvider
    {
        protected virtual TaskList GetTaskList(IServiceProvider serviceProvider, ModulePageInfo selectedModulePage)
        {
            return null;
        }

        protected virtual void OnRefresh() { }

        TaskList IHomepageTaskListProvider.GetTaskList(IServiceProvider serviceProvider, ModulePageInfo selectedModulePage)
        {
            throw new NotImplementedException();
        }
    }
}
// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Modules
{
    internal sealed class ModulesModuleProxy : ModuleServiceProxy
    {
        internal GlobalModule[] GetGlobalModules()
        {
            return (GlobalModule[])Invoke(nameof(GetGlobalModules));
        }

        internal ModulesItem[] GetItems()
        {
            return (ModulesItem[])Invoke(nameof(GetItems));
        }

        internal void Add(ModulesItem item)
        {
            Invoke(nameof(Add), item);
        }

        internal void Update(ModulesItem original, ModulesItem item)
        {
            Invoke(nameof(Update), original, item);
        }

        internal void Remove(ModulesItem item)
        {
            Invoke(nameof(Remove), item);
        }

        internal void AddGlobal(GlobalModule item)
        {
            Invoke(nameof(AddGlobal), item);
        }

        internal void UpdateGlobal(GlobalModule original, GlobalModule item)
        {
            Invoke(nameof(UpdateGlobal), original, item);
        }

        internal void RemoveGlobal(GlobalModule item)
        {
            Invoke(nameof(RemoveGlobal), item);
        }

        internal void MoveUp(ModulesItem item)
        {
            Invoke(nameof(MoveUp), item);
        }

        internal void MoveDown(ModulesItem item)
        {
            Invoke(nameof(MoveDown), item);
        }

        internal void Revert()
        {
            Invoke(nameof(Revert));
        }
    }
}

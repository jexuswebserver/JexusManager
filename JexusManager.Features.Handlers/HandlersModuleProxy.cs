// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.Handlers
{
    internal sealed class HandlersModuleProxy : ModuleServiceProxy
    {
        internal HandlersItem[] GetItems()
        {
            return (HandlersItem[])Invoke(nameof(GetItems));
        }

        internal void Add(HandlersItem item)
        {
            Invoke(nameof(Add), item);
        }

        internal void Update(HandlersItem original, HandlersItem item)
        {
            Invoke(nameof(Update), original, item);
        }

        internal void Remove(HandlersItem item)
        {
            Invoke(nameof(Remove), item);
        }

        internal void Rename(HandlersItem original, string name)
        {
            Invoke(nameof(Rename), original, name);
        }

        internal void MoveUp(HandlersItem item)
        {
            Invoke(nameof(MoveUp), item);
        }

        internal void MoveDown(HandlersItem item)
        {
            Invoke(nameof(MoveDown), item);
        }

        internal void Revert()
        {
            Invoke(nameof(Revert));
        }

        internal HandlersSettings GetSettings()
        {
            return (HandlersSettings)Invoke(nameof(GetSettings));
        }

        internal void ApplySettings(HandlersSettings settings)
        {
            Invoke(nameof(ApplySettings), settings);
        }
    }
}

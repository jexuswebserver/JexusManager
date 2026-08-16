// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.HttpErrors
{
    internal sealed class HttpErrorsModuleProxy : ModuleServiceProxy
    {
        internal HttpErrorsSettings GetSettings()
        {
            return (HttpErrorsSettings)Invoke(nameof(GetSettings));
        }

        internal void ApplySettings(HttpErrorsSettings settings)
        {
            Invoke(nameof(ApplySettings), settings);
        }

        internal HttpErrorsItem[] GetItems()
        {
            return (HttpErrorsItem[])Invoke(nameof(GetItems));
        }

        internal void Add(HttpErrorsItem item)
        {
            Invoke(nameof(Add), item);
        }

        internal void Update(HttpErrorsItem original, HttpErrorsItem item)
        {
            Invoke(nameof(Update), original, item);
        }

        internal void Remove(HttpErrorsItem item)
        {
            Invoke(nameof(Remove), item);
        }

        internal void MoveUp(HttpErrorsItem item)
        {
            Invoke(nameof(MoveUp), item);
        }

        internal void MoveDown(HttpErrorsItem item)
        {
            Invoke(nameof(MoveDown), item);
        }

        internal void Revert()
        {
            Invoke(nameof(Revert));
        }
    }
}

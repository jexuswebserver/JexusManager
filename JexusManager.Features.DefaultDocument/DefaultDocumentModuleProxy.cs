// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Management.Client;

namespace JexusManager.Features.DefaultDocument
{
    internal sealed class DefaultDocumentModuleProxy : ModuleServiceProxy
    {
        internal DefaultDocumentSnapshot GetSettings()
        {
            return (DefaultDocumentSnapshot)Invoke(nameof(GetSettings));
        }

        internal void SetEnabled(bool enabled)
        {
            Invoke(nameof(SetEnabled), enabled);
        }

        internal void Insert(string name, int index)
        {
            Invoke(nameof(Insert), name, index);
        }

        internal void Remove(string name)
        {
            Invoke(nameof(Remove), name);
        }

        internal void Move(string name, int offset)
        {
            Invoke(nameof(Move), name, offset);
        }

        internal void Revert()
        {
            Invoke(nameof(Revert));
        }
    }
}

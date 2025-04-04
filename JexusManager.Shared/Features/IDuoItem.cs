// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;

namespace JexusManager.Features
{
    public interface IDuoItem<T> : IItem<T>
    {
        bool Allowed { get; }
    }
}

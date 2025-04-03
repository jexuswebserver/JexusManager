// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    public interface IFeatureListViewItem<IItem>
    {
        IItem Item { get; }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public sealed class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(
            NavigationItem newItem,
            NavigationItem oldItem,
            bool isNew
            )
        {
            NewItem = newItem;
            OldItem = oldItem;
            IsNew = isNew;
        }

        public bool IsNew { get; }
        public NavigationItem NewItem { get; }
        public NavigationItem OldItem { get; }
    }
}
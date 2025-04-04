// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using Microsoft.Web.Administration;

    public static class ItemExtensions
    {
        public static void AppendTo<T>(this IItem<T> item, ConfigurationElementCollection collection)
        {
            item.Element = collection.CreateElement();
            item.Apply();
            collection.Add(item.Element);
        }
    }
}

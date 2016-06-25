// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace JexusManager.Features
{
    using System;

    using Microsoft.Web.Administration;

    public interface IItem<T> : IEquatable<T>
    {
        string Flag { get; set; }

        void Apply();

        ConfigurationElement Element { get; set; }

        bool Match(T other);
    }

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

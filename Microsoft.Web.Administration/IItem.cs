// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public interface IItem<T> : IEquatable<T>
    {
        string Flag { get; set; }

        void Apply();

        ConfigurationElement Element { get; set; }

        bool Match(T other);
    }
}

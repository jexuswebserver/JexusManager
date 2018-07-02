// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationMethodCollection : ICollection,
        IEnumerable<ConfigurationMethod>, IEnumerable
    {
        internal ConfigurationMethodCollection()
        { }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator<ConfigurationMethod> IEnumerable<ConfigurationMethod>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot { get; } = new object();

        public ConfigurationMethod this[int index]
        {
            get { return null; }
        }

        public ConfigurationMethod this[string methodName]
        {
            get { return null; }
        }
    }
}

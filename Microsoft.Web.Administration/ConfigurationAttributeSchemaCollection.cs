// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationAttributeSchemaCollection : IEnumerable<ConfigurationAttributeSchema>,
    ICollection, IEnumerable
    {
        private List<ConfigurationAttributeSchema> _list = new List<ConfigurationAttributeSchema>();

        internal ConfigurationAttributeSchemaCollection()
        { }

        internal void Add(ConfigurationAttributeSchema item)
        {
            _list.Add(item);
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

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator<ConfigurationAttributeSchema> IEnumerable<ConfigurationAttributeSchema>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public ConfigurationAttributeSchema this[int index]
        {
            get { return _list[index]; }
        }

        public ConfigurationAttributeSchema this[string name]
        {
            get { return _list.FirstOrDefault(item => item.Name == name); }
        }
    }
}

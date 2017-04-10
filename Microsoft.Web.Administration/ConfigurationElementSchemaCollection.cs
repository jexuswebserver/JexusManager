// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationElementSchemaCollection : IEnumerable<ConfigurationElementSchema>,
    ICollection, IEnumerable
    {
        private readonly List<ConfigurationElementSchema> _list = new List<ConfigurationElementSchema>();

        internal ConfigurationElementSchemaCollection()
        { }

        public void Add(ConfigurationElementSchema item)
        {
            _list.Add(item);
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo((ConfigurationElementSchema[])array, index);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return _list; }
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator<ConfigurationElementSchema> IEnumerable<ConfigurationElementSchema>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public ConfigurationElementSchema this[int index]
        {
            get { return _list[index]; }
        }

        public ConfigurationElementSchema this[string name]
        {
            get { return _list.FirstOrDefault(item => item.Name == name); }
        }
    }
}

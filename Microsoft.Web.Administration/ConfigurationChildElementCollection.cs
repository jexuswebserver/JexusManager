// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationChildElementCollection : ICollection, IEnumerable<ConfigurationElement>
    {
        private readonly List<ConfigurationElement> _list = new List<ConfigurationElement>();
        private readonly object _root = new object();

        internal ConfigurationChildElementCollection(ConfigurationElement parent)
        {
            Parent = parent;
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo((ConfigurationElement[])array, index);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        internal void Remove(ConfigurationElement item)
        {
            _list.Remove(item);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return _root; }
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator<ConfigurationElement> IEnumerable<ConfigurationElement>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public ConfigurationElement this[int index]
        {
            get { return _list[index]; }
        }

        public ConfigurationElement this[string name]
        {
            get
            {
                var result = _list.FirstOrDefault(item => item.ElementTagName == name);
                if (result != null)
                {
                    return result;
                }

                var schema = Parent.Schema.ChildElementSchemas[name];
                result = schema.CollectionSchema == null
                    ? new ConfigurationElement(null, name, schema, Parent, null, Parent.FileContext)
                    : new ConfigurationElementCollection(name, schema, Parent, null, Parent.FileContext);
                _list.Add(result);
                return result;
            }
        }

        internal void Add(ConfigurationElement element)
        {
            _list.Add(element);
        }

        internal ConfigurationElement Parent { get; }
    }
}

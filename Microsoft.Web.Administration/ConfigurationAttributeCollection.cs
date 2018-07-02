// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationAttributeCollection : ICollection,
    IEnumerable<ConfigurationAttribute>
    {
        private readonly List<ConfigurationAttribute> _list = new List<ConfigurationAttribute>();

        internal ConfigurationAttributeCollection(ConfigurationElement parent)
        {
            Parent = parent;
        }

        public IEnumerator<ConfigurationAttribute> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public ConfigurationAttribute this[int index]
        {
            get { return _list[index]; }
        }

        public ConfigurationAttribute this[string name]
        {
            get
            {
                var existing = _list.FirstOrDefault(item => item.Name == name);
                if (existing != null)
                {
                    return existing;
                }

                var schema = Parent.Schema.AttributeSchemas[name];
                if (!Parent.Schema.AllowUnrecognizedAttributes && schema == null)
                {
                    return null;
                }

                existing = new ConfigurationAttribute(name, schema, Parent);
                _list.Add(existing);
                return existing;
            }
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo((ConfigurationAttribute[])array, index);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot { get; } = new object();

        internal void Add(ConfigurationAttribute item)
        {
            _list.Add(item);
        }

        internal ConfigurationElement Parent { get; }
    }
}

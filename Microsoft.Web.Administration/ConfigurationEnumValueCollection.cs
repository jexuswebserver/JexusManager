// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationEnumValueCollection : ICollection,
        IEnumerable<ConfigurationEnumValue>
    {
        private readonly List<ConfigurationEnumValue> _list = new List<ConfigurationEnumValue>();

        private string _result;

        internal ConfigurationEnumValueCollection()
        { }

        internal void Add(ConfigurationEnumValue item)
        {
            _list.Add(item);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot { get; } = new object();

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator<ConfigurationEnumValue> IEnumerable<ConfigurationEnumValue>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public string GetName(long value)
        {
            return _list.FirstOrDefault(item => item.Value == value)?.Name;
        }

        public ConfigurationEnumValue this[int index]
        {
            get { return _list[index]; }
        }

        public ConfigurationEnumValue this[string name]
        {
            get { return _list.FirstOrDefault(item => item.Name == name); }
        }

        internal string FormattedString
        {
            get
            {
                if (_result != null)
                {
                    return _result;
                }

                return _result = StringExtensions.Combine(_list.Select(item => item.Name), ", ");
            }
        }
    }
}

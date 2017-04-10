// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class SectionDefinitionCollection : ICollection, IEnumerable<SectionDefinition>
    {
        private readonly FileContext _core;

        private readonly List<SectionDefinition> _list = new List<SectionDefinition>();
        private readonly object _root = new object();

        internal SectionDefinitionCollection(FileContext core)
        {
            _core = core;
        }

        public SectionDefinition Add(string sectionName)
        {
            var result = new SectionDefinition { Name = sectionName, FileContext = _core };
            _list.Add(result);
            return result;
        }

        internal void Add(SectionDefinition definition)
        {
            _list.Add(definition);
        }

        public IEnumerator<SectionDefinition> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Remove(string sectionName)
        {
            var item = this[sectionName];
            if (item != null)
            {
                _list.Remove(item);
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public SectionDefinition this[int index]
        {
            get { return _list[index]; }
        }

        public SectionDefinition this[string sectionName]
        {
            get { return _list.FirstOrDefault(item => item.Name == sectionName); }
        }

        public void CopyTo(Array array, int index)
        {
            _list.CopyTo((SectionDefinition[])array, index);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return _root; }
        }
    }
}

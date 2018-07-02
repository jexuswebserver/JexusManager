// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class SectionGroupCollection : ICollection, IEnumerable<SectionGroup>
    {
        private SectionGroup Parent { get; }
        private readonly FileContext _core;
        private readonly List<SectionGroup> _list = new List<SectionGroup>();

        internal SectionGroupCollection(FileContext core, SectionGroup parent)
        {
            Parent = parent;
            _core = core;
        }

        public SectionGroup Add(string sectionGroupName)
        {
            var found = this[sectionGroupName];
            if (found == null)
            {
                found = new SectionGroup(_core)
                {
                    Name = sectionGroupName,
                    Path = Parent.Path == string.Empty ? sectionGroupName : $"{Parent.Path}/{sectionGroupName}"
                };
                _list.Add(found);
            }

            return found;
        }

        public IEnumerator<SectionGroup> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public void Remove(string sectionGroupName)
        {
            var item = this[sectionGroupName];
            if (item != null)
            {
                _list.Remove(item);
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public SectionGroup this[int index]
        {
            get { return _list[index]; }
        }

        public SectionGroup this[string sectionGroupName]
        {
            get { return _list.FirstOrDefault(item => item.Name == sectionGroupName); }
        }


        public void CopyTo(Array array, int index)
        {
            _list.CopyTo((SectionGroup[])array, index);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot { get; } = new object();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

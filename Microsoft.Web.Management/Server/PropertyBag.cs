// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Specialized;
#if !NET7_0
using System.Web.UI;
#endif

namespace Microsoft.Web.Management.Server
{
    public sealed class PropertyBag : IDictionary
    {
        private readonly HybridDictionary _bag;
        private bool _isReadOnly;
        private readonly HybridDictionary _modifiedKeys;

        public PropertyBag()
            : this(false)
        {
        }

        public PropertyBag(bool trackState)
        {
            _bag = new HybridDictionary();
            if (trackState)
            {
                _modifiedKeys = new HybridDictionary();
            }

            IsTrackingState = trackState;
            _isReadOnly = false;
        }

        public void Add(int key, Object value)
        {
            if (key < 0)
            {
                throw new ArgumentOutOfRangeException("key");
            }

            if (_isReadOnly)
            {
                throw new InvalidOperationException("Cannot modify readonly collection");
            }

            _bag.Add(key, value);
        }

        public PropertyBag Clone()
        {
            return null;
        }

        public PropertyBag Clone(bool readOnly)
        {
            return null;
        }

        public bool Contains(int key)
        {
            return _bag.Contains(key);
        }

        public static PropertyBag CreatePropertyBagFromState(string state)
        {
            return CreatePropertyBagFromState(state, false);
        }

        public static PropertyBag CreatePropertyBagFromState(string state, bool readOnly)
        {
#if !NET7_0
            var formatter = new ObjectStateFormatter();
            var bag = (PropertyBag)formatter.Deserialize(state);
            if (readOnly)
            {
                bag._isReadOnly = true;
            }

            return bag;
#endif
            return null;
        }

        public string GetState()
        {
#if !NET7_0
            var formatter = new ObjectStateFormatter();
            return formatter.Serialize(this);
#endif
            return null;
        }

        public T GetValue<T>(int index)
        {
            return (T)this[index];
        }

        public T GetValue<T>(int index, T defaultValue)
        {
            return Contains(index) ? (T)this[index] : defaultValue;
        }

        public bool IsModified()
        {
            return false;
        }

        public bool IsModified(int key)
        {
            return false;
        }

        public void Remove(Object key)
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("Cannot modify readonly collection");
            }

            _bag.Remove(key);
        }

        object IDictionary.this[object key]
        {
            get { return _bag[key]; }
            set { _bag[key] = value; }
        }

        bool IDictionary.Contains(object key)
        {
            return _bag.Contains(key);
        }

        void IDictionary.Add(object key, object value)
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("Cannot modify readonly collection");
            }

            _bag.Add(key, value);
        }

        void IDictionary.Clear()
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("Cannot modify readonly collection");
            }

            _bag.Clear();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return _bag.GetEnumerator();
        }

        void IDictionary.Remove(object key)
        {
            if (_isReadOnly)
            {
                throw new InvalidOperationException("Cannot modify readonly collection");
            }

            _bag.Remove(key);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            _bag.CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _bag.GetEnumerator();
        }

        public int Count
        {
            get { return _bag.Count; }
        }

        public bool IsTrackingState { get; }

        public Object this[int index]
        {
            get
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return _bag[index];
            }

            set
            {
                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                if (_isReadOnly)
                {
                    throw new InvalidOperationException("Cannot modify readonly collection");
                }

                var oldItem = _bag[index];
                _bag[index] = value;
                if (_modifiedKeys != null && !Equals(oldItem, value))
                {
                    _modifiedKeys[index] = string.Empty;
                }
            }
        }

        public ICollection Keys
        {
            get { return _bag.Keys; }
        }

        public ICollection ModifiedKeys
        {
            get
            {
                if (!IsTrackingState)
                {
                    throw new InvalidOperationException("The state changes are not being tracked");
                }
                return _modifiedKeys.Keys;
            }
        }

        public ICollection Values
        {
            get { return _bag.Values; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return _isReadOnly; }
        }

        bool IDictionary.IsFixedSize
        {
            get { return _bag.IsFixedSize; }
        }

        object ICollection.SyncRoot
        {
            get { return _bag.SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { return _bag.IsSynchronized; }
        }
    }
}

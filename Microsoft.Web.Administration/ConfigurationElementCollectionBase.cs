// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.Web.Administration
{
    public class ConfigurationElementCollectionBase<T> : ConfigurationElement, ICollection, IEnumerable<T>
        where T : ConfigurationElement
    {
        internal readonly Collection<T> Exposed = new Collection<T>();

        internal readonly Collection<T> Real = new Collection<T>();

        private object _syncRoot;

        protected bool HasParent;

        protected ConfigurationElementCollectionBase()
            : this(null, null, null, null, null, null)
        { }

        internal ConfigurationElementCollectionBase(ConfigurationElement element, string name, ConfigurationElementSchema schema, ConfigurationElement parent, XElement entity, FileContext core)
            : base(element, name, schema, parent, entity, core)
        {
            AllowsAdd = Schema.CollectionSchema.AddSchemas.Count > 0;
            AllowsClear = Schema.CollectionSchema.ClearSchema != null;
            AllowsRemove = Schema.CollectionSchema.RemoveSchema != null;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.Exposed.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return this.Exposed.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            // TODO: how to fix real here.
            this.Exposed.CopyTo(array.Cast<T>().ToArray(), index);
        }

        public int Count
        {
            get { return this.Exposed.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }

                return _syncRoot;
            }
        }

        protected virtual T CreateNewElement(string elementTagName)
        {
            var schema = this.Schema.CollectionSchema.GetElementSchema(elementTagName);
            if (schema == null)
            {
                return null;
            }

            return (T)Activator.CreateInstance(typeof(T), null, elementTagName, schema, this, null);
        }

        internal void InternalAdd(T element)
        {
            this.SkipCheck = true;
            Add(element);
            this.SkipCheck = false;
        }

        public T Add(T element)
        {
            return AddAt(Count, element);
        }

        public T AddAt(int index, T element)
        {
            if (!this.SkipCheck)
            {
                this.FileContext.SetDirty();
            }

            element.Validate(false);
            element.ForceCreateEntity();
            this.CheckMatched(element);

            if (this.HasParent)
            {
                if (this.Count > 0)
                {
                    var left = index == 0 ? this.Exposed[0].IsLocallyStored : this.Exposed[index - 1].IsLocallyStored;
                    var right = index == Count
                                    ? this.Schema.Path != "system.webServer/defaultDocument/files"
                                    : this.Exposed[index].IsLocallyStored;
                    if (!left && !right)
                    {
                        foreach (var item in this.Real.ToList())
                        {
                            if (this.Schema.CollectionSchema.RemoveElementName == item.ElementTagName)
                            {
                                this.Real.Remove(item);
                                item.Entity.Remove();
                            }
                        }

                        var clear1 = CreateElement(Schema.CollectionSchema.ClearElementName);
                        this.Real.Insert(0, clear1);
                        clear1.ForceCreateEntity();
                        clear1.AppendToParentElement(clear1.Entity, true);
                        foreach (var item in Exposed)
                        {
                            if (item.IsLocallyStored)
                            {
                                continue;
                            }

                            item.IsLocallyStored = true;
                            item.AppendToParentElement(item.Entity, false);
                            this.Real.Add(item);
                        }
                    }
                }

                element.IsLocallyStored = true;
            }

            if (Count == index)
            {
                this.Real.Add(element);
                this.Exposed.Add(element);
                element.AppendToParentElement(element.Entity, false);
                return element;
            }

            this.Real.Insert(index, element);
            var previous = this.Exposed[index];
            this.Exposed.Insert(index, element);

            if (previous.IsLocallyStored)
            {
                previous.InnerEntity.AddBeforeSelf(element.InnerEntity);
            }

            element.AppendToParentElement(element.Entity, false);
            return element;
        }

        private void CheckMatched(T element)
        {
            foreach (var item in this.Exposed)
            {
                CheckItem(element, item);
            }
        }

        private void CheckItem(T element, T item)
        {
            List<Tuple<string, object>> keys = new List<Tuple<string, object>>();
            foreach (ConfigurationAttributeSchema attribute in element.Schema.AttributeSchemas)
            {
                if (!attribute.IsCombinedKey && !attribute.IsUniqueKey)
                {
                    continue;
                }

                if (!item[attribute.Name].Equals(element[attribute.Name]))
                {
                    return;
                }

                keys.Add(new Tuple<string, object>(attribute.Name, item[attribute.Name]));
            }

            if (keys.Count == 0)
            {
                return;
            }

            var line = (element.Entity as IXmlLineInfo).LineNumber;
            if (keys.Count == 1)
            {
                throw new COMException(
                    $"Filename: \\\\?\\{FileContext.FileName}\r\nLine number: {line}\r\nError: Cannot add duplicate collection entry of type '{element.ElementTagName}' with unique key attribute '{keys[0].Item1}' set to '{keys[0].Item2}'\r\n\r\n");
            }

            var keyNames = new string[keys.Count];
            var values = new object[keys.Count];
            for (var index = 0; index < keys.Count; index++)
            {
                var key = keys[index];
                keyNames[index] = key.Item1;
                values[index] = key.Item2;
            }

            throw new COMException(
                $"Filename: \\\\?\\{FileContext.FileName}\r\nLine number: {line}\r\nError: Cannot add duplicate collection entry of type '{element.ElementTagName}' with combined key attributes '{StringExtensions.Combine(keyNames, ", ")}' respectively set to '{StringExtensions.Combine(values.Select(value => value.ToString()), ", ")}'\r\n\r\n");
        }

        public void Clear()
        {
            this.FileContext.SetDirty();
            foreach (T element in this.Exposed)
            {
                if (element.IsLocallyStored)
                {
                    element.Delete();
                }
            }

            this.Exposed.Clear();
            this.Real.Clear();
            if (this.HasParent && this.AllowsClear)
            {
                var clear1 = CreateElement(Schema.CollectionSchema.ClearElementName);
                this.Real.Add(clear1);
                clear1.ForceCreateEntity();
                clear1.AppendToParentElement(clear1.Entity, true);
            }
            else
            {
                this.CleanEntity();
            }
        }

        public T CreateElement()
        {
            return CreateElement(Schema.CollectionSchema?.AddSchemas[0].Name);
        }

        public T CreateElement(string elementTagName)
        {
            return CreateNewElement(elementTagName);
        }

        public int IndexOf(T element)
        {
            return this.Exposed.IndexOf(element);
        }

        public void Remove(T element)
        {
            if (this.FileContext.ReadOnly)
            {
                throw new FileLoadException(
                    "Filename: \r\nError: This configuration section cannot be modified because it has been opened for read only access\r\n\r\n");
            }

            this.Exposed.Remove(element);
            if (element.IsLocallyStored)
            {
                this.Real.Remove(element);
                element.Delete();
                if (this.Real.Count == 0)
                {
                    this.CleanEntity();
                }
            }
            else
            {
                var addSchema =
                    Schema.CollectionSchema.AddSchemas.FirstOrDefault(item => item.Name == element.ElementTagName);
                if (addSchema == null)
                {
                    throw new InvalidOperationException();
                }

                var removal = CreateElement(Schema.CollectionSchema.RemoveElementName);
                this.Real.Insert(0, removal);
                removal.ForceCreateEntity();
                removal.AppendToParentElement(removal.Entity, true);

                foreach (ConfigurationAttributeSchema child in addSchema.AttributeSchemas)
                {
                    if (child.IsUniqueKey || child.IsCombinedKey)
                    {
                        removal[child.Name] = element[child.Name];
                    }
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.Exposed.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var item = this.Exposed[index];
            Remove(item);
        }

        public bool AllowsAdd { get; protected set; }

        public bool AllowsClear { get; protected set; }

        public bool AllowsRemove { get; protected set; }

        public T this[int index]
        {
            get { return this.Exposed[index]; }
        }

        internal void Clone(ConfigurationElement item, ConfigurationElement newItem)
        {
            newItem.SkipCheck = true;
            foreach (ConfigurationAttribute attribute in item.Attributes)
            {
                newItem[attribute.Name] = item[attribute.Name];
            }

            newItem.IsLocked = item.IsLocked;
            newItem.SkipCheck = false;
        }
    }
}

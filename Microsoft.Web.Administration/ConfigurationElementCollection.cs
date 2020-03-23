// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Web.Administration
{
    public class ConfigurationElementCollection : ConfigurationElementCollectionBase<ConfigurationElement>
    {
        private bool _initialized;

        public ConfigurationElementCollection()
        { }

        internal ConfigurationElementCollection(string name, ConfigurationElementSchema schema, ConfigurationElement parent, XElement entity, FileContext core)
            : base(null, name, schema, parent, entity, core)
        {
        }

        private static bool Match(ConfigurationElement existing, ConfigurationElement remove, ConfigurationElementSchema removeSchema)
        {
            foreach (ConfigurationAttributeSchema attribute in removeSchema.AttributeSchemas)
            {
                if (attribute.IsUniqueKey || attribute.IsCombinedKey)
                {
                    if (existing.Attributes[attribute.Name].Value.ToString() != remove.Attributes[attribute.Name].Value.ToString())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected override ConfigurationElement CreateNewElement(string elementTagName)
        {
            var schema = Schema.CollectionSchema.AddSchemas.FirstOrDefault(item => item.Name == elementTagName);
            return schema?.CollectionSchema == null
                ? new ConfigurationElement(null, elementTagName, schema, this, null, FileContext) /*{  IsLocallyStored = this.Schema.Path != "system.webServer/defaultDocument/files" }*/
                : new ConfigurationElementCollection(elementTagName, schema, this, null, FileContext);
        }

        internal override void AddChild(ConfigurationElement child)
        {
            ForceLoad();
            if (Schema.CollectionSchema.ContainsAddElement(child.ElementTagName))
            {
                child.AppendToParentElement(child.Entity, false);
                Real.Add(child);
                if (HasParent && Schema.Path == "system.webServer/defaultDocument/files")
                {
                    var index = Real.Count == 0 ? 0 : Exposed.IndexOf(Real[Real.Count - 1]) + 1;
                    Exposed.Insert(index, child);
                }
                else
                {
                    Exposed.Add(child);
                }
            }
            else if (child.ElementTagName == Schema.CollectionSchema.ClearElementName)
            {
                child.AppendToParentElement(child.Entity, false);
                Real.Add(child);
                Exposed.Clear();
            }
            else if (child.ElementTagName == Schema.CollectionSchema.RemoveElementName)
            {
                child.AppendToParentElement(child.Entity, false);
                Real.Add(child);
                foreach (var item in Exposed.ToList())
                {
                    if (Match(item, child, Schema.CollectionSchema.RemoveSchema))
                    {
                        // IMPORTANT: can remove from location tag in the same file, but not from child web.config.
                        if (item.IsLocked == "true" && item.CloneSource?.FileContext != FileContext)
                        {
                            throw new FileLoadException($"Filename: \\\\?\\{FileContext.FileName}\r\nLine number: {(child.Entity as IXmlLineInfo).LineNumber}\r\nError: Lock violation\r\n\r\n");
                        }

                        Exposed.Remove(item);
                    }
                }
            }
            else
            {
                base.AddChild(child);
            }
        }

        internal ConfigurationElementCollection ForceLoad()
        {
            if (_initialized)
            {
                return this;
            }

            _initialized = true;

            var duplicateElement = GetParentElement();
            var duplicateCollection = duplicateElement?.GetCollection();
            if (duplicateCollection != null)
            {
                HasParent = true;

                // IMPORTANT: load duplicate element.
                foreach (ConfigurationElement element in duplicateCollection.Exposed)
                {
                    var newItem = CreateNewElement(element.ElementTagName);
                    Clone(element, newItem);
                    newItem.IsLocallyStored = false;
                    newItem.CloneSource = element;
                    Exposed.Add(newItem);
                }

                return this;
            }

            var parentElement = FileContext.AppHost ? GetParentElement() : GetElementAtParentLocationInFileContext(FileContext.Parent);
            var parentCollection = parentElement?.GetCollection();
            if (parentCollection == null)
            {
                return this;
            }

            HasParent = true;
            foreach (ConfigurationElement element in parentCollection.Exposed)
            {
                var newItem = CreateNewElement(element.ElementTagName);
                Clone(element, newItem);
                newItem.IsLocallyStored = false;
                newItem.CloneSource = element;
                Exposed.Add(newItem);
            }

            return this;
        }

        internal void Revert()
        {
            FileContext.SetDirty();
            _initialized = false;
            HasParent = false;
            Real.Clear();
            Exposed.Clear();
            InnerEntity?.Remove();
            InnerEntity = null;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
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
                ? new ConfigurationElement(null, elementTagName, schema, this, null, this.FileContext) /*{  IsLocallyStored = this.Schema.Path != "system.webServer/defaultDocument/files" }*/
                : new ConfigurationElementCollection(elementTagName, schema, this, null, this.FileContext);
        }

        internal override void AddChild(ConfigurationElement child)
        {
            ForceLoad();
            if (Schema.CollectionSchema.ContainsAddElement(child.ElementTagName))
            {
                child.AppendToParentElement(child.Entity, false);
                this.Real.Add(child);
                if (this.HasParent && this.Schema.Path == "system.webServer/defaultDocument/files")
                {
                    var index = this.Real.Count == 0 ? 0 : this.Exposed.IndexOf(this.Real[this.Real.Count - 1]) + 1;
                    this.Exposed.Insert(index, child);
                }
                else
                {
                    this.Exposed.Add(child);
                }
            }
            else if (child.ElementTagName == Schema.CollectionSchema.ClearElementName)
            {
                child.AppendToParentElement(child.Entity, false);
                this.Real.Add(child);
                this.Exposed.Clear();
            }
            else if (child.ElementTagName == Schema.CollectionSchema.RemoveElementName)
            {
                child.AppendToParentElement(child.Entity, false);
                this.Real.Add(child);
                foreach (var item in this.Exposed.ToList())
                {
                    if (Match(item, child, this.Schema.CollectionSchema.RemoveSchema))
                    {
                        this.Exposed.Remove(item);
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

            var duplicateElement = GetElementInFileContext(this.FileContext.Parent);
            var duplicateCollection = duplicateElement?.GetCollection();
            if (duplicateCollection != null)
            {
                this.HasParent = true;

                // IMPORTANT: load duplicate element.
                foreach (ConfigurationElement element in duplicateCollection.Exposed)
                {
                    var newItem = CreateNewElement(element.ElementTagName);
                    this.Clone(element, newItem);
                    newItem.IsLocallyStored = false;
                    this.Exposed.Add(newItem);
                }

                return this;
            }

            var parentElement = this.FileContext.AppHost ? GetParentElement() : this.GetElementAtParentLocationInFileContext(this.FileContext.Parent);
            var parentCollection = parentElement?.GetCollection();
            if (parentCollection == null)
            {
                return this;
            }

            this.HasParent = true;
            foreach (ConfigurationElement element in parentCollection.Exposed)
            {
                var newItem = CreateNewElement(element.ElementTagName);
                this.Clone(element, newItem);
                newItem.IsLocallyStored = false;
                this.Exposed.Add(newItem);
            }

            return this;
        }

        private ConfigurationElement GetElementInFileContext(FileContext core)
        {
            if (Section.Location == null)
            {
                return null;
            }

            var section = core.GetSection(this.Section.SectionPath, this.Section.Location);
            return section?.GetElementByPath(this.Schema.Path);
        }

        internal void Revert()
        {
            this.FileContext.SetDirty();
            _initialized = false;
            this.HasParent = false;
            this.Real.Clear();
            this.Exposed.Clear();
            this.InnerEntity?.Remove();
            this.InnerEntity = null;
        }
    }
}

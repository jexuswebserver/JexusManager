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
                Exposed.Add(newItem);
            }

            return this;
        }

        private ConfigurationElement GetElementInFileContext(FileContext core)
        {
            if (Section.Location == null)
            {
                return null;
            }

            if (core == null)
            {
                return null;
            }

            var section = core.GetSection(Section.SectionPath, Section.Location);
            return section?.GetElementByPath(Schema.Path);
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

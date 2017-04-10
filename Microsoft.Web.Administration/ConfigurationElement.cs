// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;

namespace Microsoft.Web.Administration
{
    public class ConfigurationElement
    {
        internal ConfigurationElement(ConfigurationElement element, string name, ConfigurationElementSchema schema, ConfigurationElement parent, XElement entity, FileContext core)
        {
            Methods = new ConfigurationMethodCollection();
            this.FileContext = parent?.FileContext ?? element?.FileContext ?? core;
            Section = parent?.Section;
            this.InnerEntity = entity ?? element?.InnerEntity;
            if (element == null)
            {
                if (name == null)
                {
                    throw new ArgumentException("empty name");
                }

                ElementTagName = name;
                Attributes = new ConfigurationAttributeCollection(this);
                ChildElements = new ConfigurationChildElementCollection(this);
                Collections = new List<ConfigurationElementCollection>();
                RawAttributes = new Dictionary<string, string>();
                ParentElement = parent;
                if (parent == null)
                {
                    if (schema == null)
                    {
                        throw new ArgumentException();
                    }

                    Schema = schema;
                    IsLocallyStored = true;
                }
                else
                {
                    IsLocallyStored = !parent.Section.IsLocked;
                    var collection = parent.Schema.CollectionSchema;
                    if (collection == null)
                    {
                        Schema = parent.Schema.ChildElementSchemas[name];
                    }
                    else
                    {
                        Schema = parent.Schema.CollectionSchema.GetElementSchema(name) ?? parent.Schema.ChildElementSchemas[name];
                    }

                    if (Schema == null)
                    {
                        throw new ArgumentException("empty schema");
                    }
                }

                ParseAttributes();
            }
            else
            {
                IsLocallyStored = element.IsLocallyStored;
                ElementTagName = element.ElementTagName;
                Attributes = element.Attributes;
                ChildElements = element.ChildElements;
                Collections = element.Collections;
                RawAttributes = element.RawAttributes;
                Schema = element.Schema;
                ParentElement = parent ?? element.ParentElement;
                if (schema != null)
                {
                    // TODO: here we ignore second schema
                    //throw new ArgumentException();
                }
            }
        }

        internal XElement Entity
        {
            get { return this.InnerEntity ?? (this.InnerEntity = CreateEntity()); }
            set { this.InnerEntity = value; }
        }

        internal void ForceCreateEntity()
        {
            if (this.InnerEntity != null)
            {
                return;
            }

            this.InnerEntity = CreateEntity();
        }

        internal void Validate(bool loading)
        {
            foreach (ConfigurationAttributeSchema attribute in this.Schema.AttributeSchemas)
            {
                if (!attribute.IsRequired)
                {
                    continue;
                }

                if (!this.RawAttributes.ContainsKey(attribute.Name))
                {
                    if (loading)
                    {
                        var line = (this.Entity as IXmlLineInfo).LineNumber;
                        var error = string.Format("Missing required attribute '{0}'", attribute.Name);
                        throw new COMException(
                            string.Format("Line number: {0}\r\nError: {1}\r\n", line, error));
                    }

                    throw new FileNotFoundException(
                        string.Format(
                            "Filename: \r\nError: Element is missing required attributes {0}\r\n\r\n",
                            attribute.Name));
                }
            }
        }

        internal virtual XElement CreateEntity()
        {
            if (ElementTagName.Contains('/'))
            {
                return this.FileContext.CreateElement(ElementTagName, Section.Location);
            }

            var result = new XElement(ElementTagName);
            if (this.ParentElement is ConfigurationElementCollection
                && this.ParentElement.Schema.CollectionSchema.GetElementSchema(this.ElementTagName) != null)
            {
                // IMPORTANT: avoid appending to parent element.
                return result;
            }

            AppendToParentElement(result, false);
            return result;
        }

        internal void AppendToParentElement(XElement entity, bool top)
        {
            if (entity.Parent != null)
            {
                return;
            }

            if (!IsLocallyStored)
            {
                return;
            }

            if (top && ParentElement?.Entity.FirstNode != null)
            {
                ParentElement.Entity.FirstNode.AddBeforeSelf(entity);
            }
            else
            {
                ParentElement?.Entity.Add(entity);
            }
        }

        internal FileContext FileContext { get; set; }
        internal ConfigurationSection Section { get; set; }

        public void Delete()
        {
            this.FileContext.SetDirty();
            ParentElement?.ChildElements?.Remove(this);
            if (this.InnerEntity?.Parent != null) // for removing items in collection
            {
                this.InnerEntity?.Remove();
            }
        }

        public ConfigurationAttribute GetAttribute(string attributeName)
        {
            if (this.Schema != null && !this.Schema.AllowUnrecognizedAttributes
                && this.Schema.AttributeSchemas.All(schema => schema.Name != attributeName))
            {
                throw new COMException(
                    string.Format(
                        "Filename: \\\\?\\{0}\r\nLine number: {1}\r\nError: Unrecognized attribute '{2}'\r\n\r\n",
                        this.FileContext.FileName,
                        (this.Entity as IXmlLineInfo).LineNumber,
                        attributeName));
            }

            return Attributes[attributeName];
        }

        public object GetAttributeValue(string attributeName)
        {
            return this[attributeName];
        }

        public ConfigurationElement GetChildElement(string elementName)
        {
            return ChildElements[elementName];
        }

        public ConfigurationElement GetChildElement(string elementName, Type elementType)
        {
            return GetChildElement(elementName);
        }

        public ConfigurationElementCollection GetCollection()
        {
            var section = this as ConfigurationSection;
            return section == null ? (this as ConfigurationElementCollection)?.ForceLoad() : section.Root.ForceLoad();
        }

        public ConfigurationElementCollection GetCollection(string collectionName)
        {
            var child = ChildElements[collectionName];
            if (child != null)
            {
                return child.GetCollection();
            }

            var schema = Schema.ChildElementSchemas[collectionName];
            if (schema == null)
            {
                return null;
            }

            var result = new ConfigurationElementCollection(collectionName, schema, this, this.InnerEntity, null);
            ChildElements.Add(result);
            return result.GetCollection();
        }

        public ConfigurationElement GetCollection(Type collectionType)
        {
            throw new NotImplementedException();
        }

        public ConfigurationElement GetCollection(string collectionName, Type collectionType)
        {
            throw new NotImplementedException();
        }

        public Object GetMetadata(string metadataType)
        {
            throw new NotImplementedException();
        }

        public void SetAttributeValue(string attributeName, object value)
        {
            GetAttribute(attributeName).SetValue(SetAttributeValueInner(attributeName, value));
        }

        public void SetMetadata(string metadataType, object value)
        {
            throw new NotImplementedException();
        }

        public ConfigurationAttributeCollection Attributes { get; }
        public ConfigurationChildElementCollection ChildElements { get; protected set; }
        public string ElementTagName { get; }
        public bool IsLocallyStored { get; internal set; }
        public object this[string attributeName]
        {
            get { return GetAttribute(attributeName).Value; }
            set { SetAttributeValue(attributeName, value); }
        }

        public ConfigurationMethodCollection Methods { get; private set; }
        public IDictionary<string, string> RawAttributes { get; }
        public ConfigurationElementSchema Schema { get; }

        private List<ConfigurationElementCollection> Collections { get; }

        internal ConfigurationElement ParentElement { get; }

        private ConfigurationLockCollection _lockAllAttributesExcept;
        public ConfigurationLockCollection LockAllAttributesExcept
        {
            get
            {
                return _lockAllAttributesExcept ??
                       (_lockAllAttributesExcept =
                           new ConfigurationLockCollection(this,
                               ConfigurationLockType.Attribute | ConfigurationLockType.Exclude));
            }
        }

        private ConfigurationLockCollection _lockAllElementsExcept;
        public ConfigurationLockCollection LockAllElementsExcept
        {
            get
            {
                return _lockAllElementsExcept ??
                       (_lockAllElementsExcept =
                           new ConfigurationLockCollection(this,
                               ConfigurationLockType.Element | ConfigurationLockType.Exclude));
            }
        }

        private ConfigurationLockCollection _lockAttributes;
        public ConfigurationLockCollection LockAttributes
        {
            get
            {
                return _lockAttributes ??
                       (_lockAttributes = new ConfigurationLockCollection(this, ConfigurationLockType.Attribute));
            }
        }

        private ConfigurationLockCollection _lockElements;
        protected internal XElement InnerEntity;

        private string _isLocked;

        public ConfigurationLockCollection LockElements
        {
            get
            {
                return _lockElements ??
                       (_lockElements = new ConfigurationLockCollection(this, ConfigurationLockType.Element));
            }
        }

        internal string IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                _isLocked = value;
                Entity.SetAttributeValue("lockItem", _isLocked == "true" ? "true" : null);
            }
        }

        internal bool SkipCheck { get; set; }

        internal virtual void AddChild(ConfigurationElement child)
        {
            ChildElements.Add(child);
        }

        internal virtual ConfigurationElement GetElementByPath(string path)
        {
            var index = path.IndexOf(Schema.Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return null;
            }

            if (this.Schema.Path.Length != 0)
            {
                if (path.Length != this.Schema.Path.Length && path[this.Schema.Path.Length] != '/')
                {
                    return null;
                }
            }

            if (Schema.Path == path)
            {
                return this;
            }

            foreach (ConfigurationElement child in ChildElements)
            {
                var result = child.GetElementByPath(path);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private void ParseAttributes()
        {
            if (this.InnerEntity == null)
            {
                return;
            }

            foreach (var attribute in this.InnerEntity.Attributes())
            {
                try
                {
                    ParseAttribute(attribute);
                }
                catch (COMException ex)
                {
                    throw new COMException(
                        string.Format(
                            "Line number: {0}\r\nError: The '{1}' attribute is invalid.  {2}\r\n",
                            (this.InnerEntity as IXmlLineInfo).LineNumber,
                            attribute.Name,
                            ex.Message));
                }
                catch (ArgumentException ex)
                {
                    throw new COMException(
                        string.Format(
                            "Line number: {0}\r\nError: {1}\r\n",
                            (this.InnerEntity as IXmlLineInfo).LineNumber,
                            ex.Message));
                }
            }

            Validate(true);
        }

        private void ParseAttribute(XAttribute attribute)
        {
            var name = attribute.Name.LocalName;
            switch (name)
            {
                case "lockAllAttributesExcept":
                    LockAllAttributesExcept.SetFromList(attribute.Value);
                    return;
                case "lockAllElementsExcept":
                    LockAllElementsExcept.SetFromList(attribute.Value);
                    return;
                case "lockAttributes":
                    LockAttributes.SetFromList(attribute.Value);
                    return;
                case "lockElements":
                    LockElements.SetFromList(attribute.Value);
                    return;
                case "lockItem":
                    IsLocked = attribute.Value;
                    return;
            }

            RawAttributes.Add(name, attribute.Value);
            var child = Schema.AttributeSchemas[name];
            if (child == null && !Schema.AllowUnrecognizedAttributes)
            {
                throw new ArgumentException(String.Format("Unrecognized attribute '{0}'", name));
            }

            var item = new ConfigurationAttribute(name, child, attribute.Value, this);
            Attributes.Add(item);
        }

        internal ConfigurationElement GetElementAtParentLocationInFileContext(FileContext core)
        {
            if (Section == null)
            {
                return null;
            }

            if (Section.Location == null)
            {
                if (core != null && core.Parent == null && Schema.Path == "configProtectedData/providers")
                {
                    // IMPORTANT: to load providers from machine.config.
                    return core.GetSection(Section.SectionPath).GetChildElement("providers");
                }

                return null;
            }

            string parentLocation = Section.Location.GetParentPath();
            while (true)
            {
                var parentSection = core.GetSection(Section.SectionPath, parentLocation);
                // IMPORTANT: allow null section as web.config does not have the sections.
                var parentElement = parentSection?.GetElementByPath(Schema.Path);
                if (parentElement != null)
                {
                    return parentElement;
                }

                if (parentLocation == null)
                {
                    return null;
                }

                parentLocation = parentLocation.GetParentPath();
            }
        }

        internal ConfigurationElement GetParentElement()
        {
            var core = this.FileContext;
            if (core == null)
            {
                return null;
            }

            while (true)
            {
                // first check same file.
                var parentElement = this.GetElementAtParentLocationInFileContext(core);
                if (parentElement != null)
                {
                    return parentElement;
                }

                core = core.Parent;
                if (core == null)
                {
                    return null;
                }
            }
        }

        protected internal void CleanEntity()
        {
            if (this.InnerEntity == null || this.InnerEntity.HasElements || this.InnerEntity.HasAttributes)
            {
                return;
            }

            var parent = this.InnerEntity?.Parent;
            if (parent == null)
            {
                this.InnerEntity = null;
                return;
            }

            this.InnerEntity?.Remove();
            this.InnerEntity = null;
            if (this.ParentElement == null)
            {
                Clean(parent);
            }
            else
            {
                this.ParentElement.CleanEntity();
            }
        }

        private static void Clean(XElement entity)
        {
            if (entity.HasElements || entity.HasAttributes || entity.Name == "configuration")
            {
                return;
            }

            var parent = entity.Parent;
            if (parent == null)
            {
                return;
            }

            entity.Remove();
            Clean(parent);
        }

        internal object SetAttributeValueInner(string name, object value)
        {
            if (Section.IsLocked)
            {
                throw new FileLoadException("This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n");
            }

            if (!this.SkipCheck)
            {
                this.FileContext.SetDirty();
            }

            var attribute = this.GetAttribute(name);
            var result = attribute.TypeMatch(value);
            attribute.IsInheritedFromDefaultValue = (attribute.Schema == null || !attribute.Schema.IsRequired)
                                                    && result.Equals(attribute.ExtractDefaultValue());
            // IMPORTANT: remove attribute if value is equal to default.
            this.Entity.SetAttributeValue(
                name, attribute.IsInheritedFromDefaultValue
                    ? null
                    : attribute.Format(result));

            // IMPORTANT: IIS seems to use the following
            // this.Entity.SetAttributeValue(name, attribute.Format(_value));

            this.CleanEntity();
            if (attribute.IsInheritedFromDefaultValue)
            {
                this.RawAttributes.Remove(name);
            }
            else
            {
                if (RawAttributes.ContainsKey(name))
                {
                    RawAttributes[name] = value.ToString();
                }
                else
                {
                    RawAttributes.Add(name, value.ToString());
                }
            }

            return result;
        }
    }
}

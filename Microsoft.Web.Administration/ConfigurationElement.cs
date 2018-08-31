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
using System.Diagnostics;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{ElementTagName}")]
    public class ConfigurationElement
    {
        private ConfigurationElement _configSource;
        private string _overriddenFileName;

        internal ConfigurationElement(ConfigurationElement element, string name, ConfigurationElementSchema schema, ConfigurationElement parent, XElement entity, FileContext core, string fileName = null)
        {
            Methods = new ConfigurationMethodCollection();
            FileContext = parent?.FileContext ?? element?.FileContext ?? core;
            Section = parent?.Section;
            InnerEntity = entity ?? element?.InnerEntity;
            _overriddenFileName = fileName;
            if (element == null)
            {
                ElementTagName = name ?? throw new ArgumentException("empty name");
                _attributes = new ConfigurationAttributeCollection(this);
                ChildElements = new ConfigurationChildElementCollection(this);
                Collections = new List<ConfigurationElementCollection>();
                _rawAttributes = new Dictionary<string, string>();
                ParentElement = parent;
                if (parent == null)
                {
                    Schema = schema ?? throw new ArgumentException();
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

                ParseAttributes(_overriddenFileName ?? FileContext.FileName);
            }
            else
            {
                IsLocallyStored = element.IsLocallyStored;
                ElementTagName = element.ElementTagName;
                _attributes = element.Attributes;
                ChildElements = element.ChildElements;
                Collections = element.Collections;
                _rawAttributes = element.RawAttributes;
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
            get { return InnerEntity ?? (InnerEntity = CreateEntity()); }
            set { InnerEntity = value; }
        }

        internal void ForceCreateEntity()
        {
            if (InnerEntity != null)
            {
                return;
            }

            InnerEntity = CreateEntity();
        }

        internal void Validate(bool loading)
        {
            List<string> missing = null;
            foreach (ConfigurationAttributeSchema attribute in Schema.AttributeSchemas)
            {
                if (!attribute.IsRequired)
                {
                    continue;
                }

                if (!RawAttributes.ContainsKey(attribute.Name))
                {
                    if (missing == null)
                    {
                        missing = new List<string>();
                    }

                    missing.Add(attribute.Name);
                }
            }

            if (missing != null)
            {
                string error;
                if (!loading)
                {
                    error = $"required attributes {StringExtensions.Combine(missing, ",")}";
                }
                else if (missing.Count == 1)
                {
                    error = $"required attribute '{missing[0]}'";
                }
                else
                {
                    error = $"required attributes '{StringExtensions.Combine(missing, ",")}'";
                }

                if (loading)
                {
                    var line = (Entity as IXmlLineInfo).LineNumber;
                    throw new COMException(
                        string.Format("Line number: {0}\r\nError: Missing {1}\r\n", line, error));
                }

                throw new FileNotFoundException($"Filename: \r\nError: Element is missing {error}\r\n\r\n");
            }
        }

        internal virtual XElement CreateEntity()
        {
            if (ElementTagName.Contains('/'))
            {
                return FileContext.CreateElement(ElementTagName, Section.Location);
            }

            var result = new XElement(ElementTagName);
            if (ParentElement is ConfigurationElementCollection
                && ParentElement.Schema.CollectionSchema.GetElementSchema(ElementTagName) != null)
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
            FileContext.SetDirty();
            ParentElement?.ChildElements?.Remove(this);
            if (InnerEntity?.Parent != null) // for removing items in collection
            {
                InnerEntity?.Remove();
            }
        }

        internal bool ContainsAttribute(string attributeName)
        {
            return Schema == null || Schema.AllowUnrecognizedAttributes
                || Schema.AttributeSchemas.Any(schema => schema.Name == attributeName);
        }

        public ConfigurationAttribute GetAttribute(string attributeName)
        {
            if (!ContainsAttribute(attributeName))
            {
                throw new COMException(
                    string.Format(
                        "Filename: \\\\?\\{0}\r\nLine number: {1}\r\nError: Unrecognized attribute '{2}'\r\n\r\n",
                        FileContext.FileName,
                        (Entity as IXmlLineInfo).LineNumber,
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

            var result = new ConfigurationElementCollection(collectionName, schema, this, InnerEntity, null);
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

        public ConfigurationAttributeCollection Attributes => _configSource == null ? _attributes : _configSource.Attributes;
        public ConfigurationChildElementCollection ChildElements
        {
            get => _configSource == null ? _childElements : _configSource.ChildElements;
            protected set
            {
                if (_configSource == null)
                {
                    _childElements = value;
                }
                else
                {
                    _configSource.ChildElements = value;
                }
            }
        }

        public string ElementTagName { get; }
        public bool IsLocallyStored { get; internal set; }
        public object this[string attributeName]
        {
            get { return GetAttribute(attributeName).Value; }
            set { SetAttributeValue(attributeName, value); }
        }

        public ConfigurationMethodCollection Methods { get; private set; }
        public IDictionary<string, string> RawAttributes => _configSource == null ? _rawAttributes : _configSource.RawAttributes;
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
        private ConfigurationChildElementCollection _childElements;
        private readonly ConfigurationAttributeCollection _attributes;
        private readonly IDictionary<string, string> _rawAttributes;

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

            if (Schema.Path.Length != 0)
            {
                if (path.Length != Schema.Path.Length && path[Schema.Path.Length] != '/')
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

        private void ParseAttributes(string fileName)
        {
            if (InnerEntity == null)
            {
                return;
            }

            ParseAttributes(InnerEntity, fileName);
        }

        private void ParseAttributes(XElement entity, string fileName)
        {
            foreach (var attribute in entity.Attributes())
            {
                try
                {
                    ParseAttribute(attribute, fileName);
                }
                catch (COMException ex)
                {
                    throw new COMException(
                        string.Format(
                            "Line number: {0}\r\nError: The '{1}' attribute is invalid.  {2}\r\n",
                            (InnerEntity as IXmlLineInfo).LineNumber,
                            attribute.Name,
                            ex.Message));
                }
                catch (ArgumentException ex)
                {
                    throw new COMException(
                        string.Format(
                            "Line number: {0}\r\nError: {1}\r\n",
                            (InnerEntity as IXmlLineInfo).LineNumber,
                            ex.Message));
                }
            }

            Validate(true);
        }

        private void ParseAttribute(XAttribute attribute, string fileName)
        {
            if (attribute.Name.NamespaceName == "http://www.w3.org/2000/xmlns/")
            {
                return;
            }

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
                case "configSource":
                    if (Section.SectionPath.StartsWith("system.web/"))
                    {
                        var directory = Path.GetDirectoryName(fileName);
                        var file = Path.Combine(directory, attribute.Value);
                        var configSource = XDocument.Load(file, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);
                        var node = configSource.Root;
                        _configSource = new ConfigurationElement(null, ElementTagName, Schema, ParentElement, node, FileContext, file);
                        return;
                    }

                    throw new ArgumentException("Specified configSource cannot be parsed");
            }

            RawAttributes.Add(name, attribute.Value);
            var child = Schema.AttributeSchemas[name];
            if (child == null && !Schema.AllowUnrecognizedAttributes)
            {
                throw new ArgumentException($"Unrecognized attribute '{name}'");
            }

            var item = new ConfigurationAttribute(name, child, attribute.Value, this);
            Attributes.Add(item);
        }

        internal ConfigurationElement GetElementAtParentLocationInFileContext(FileContext core)
        {
            return GetElementAtParentLocationInFileContext(core, core);
        }

        internal ConfigurationElement GetElementAtParentLocationInFileContext(FileContext core, FileContext top)
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

            if (core == null)
            {
                return null;
            }

            if (core.Location != null && !Section.Location.StartsWith(core.Location + '/'))
            {
                // IMPORTANT: web.config should not check parent path.
                return null;
            }

            if (core != top)
            {
                var exact = core.GetSection(Section.SectionPath, Section.Location);
                var exactElement = exact?.GetElementByPath(Schema.Path);
                if (exactElement != null)
                {
                    return exactElement;
                }
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
            var core = FileContext;
            if (core == null)
            {
                return null;
            }

            while (true)
            {
                // first check same file.
                var parentElement = GetElementAtParentLocationInFileContext(core, FileContext);
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
            if (InnerEntity == null || InnerEntity.HasElements || InnerEntity.HasAttributes)
            {
                return;
            }

            var parent = InnerEntity?.Parent;
            if (parent == null)
            {
                InnerEntity = null;
                return;
            }

            InnerEntity?.Remove();
            InnerEntity = null;
            if (ParentElement == null)
            {
                Clean(parent);
            }
            else
            {
                ParentElement.CleanEntity();
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
            if (!SkipCheck)
            {
                if (Section.IsLocked)
                {
                    throw new FileLoadException("This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n");
                }

                FileContext.SetDirty();
            }

            var attribute = GetAttribute(name);
            var result = attribute.TypeMatch(value);
            attribute.IsInheritedFromDefaultValue = (attribute.Schema == null || !attribute.Schema.IsRequired)
                                                    && result.Equals(attribute.ExtractDefaultValue());
            // IMPORTANT: remove attribute if value is equal to default.
            Entity.SetAttributeValue(
                name, attribute.IsInheritedFromDefaultValue
                    ? null
                    : attribute.Format(result));

            // IMPORTANT: IIS seems to use the following
            // this.Entity.SetAttributeValue(name, attribute.Format(_value));

            CleanEntity();
            if (attribute.IsInheritedFromDefaultValue)
            {
                RawAttributes.Remove(name);
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

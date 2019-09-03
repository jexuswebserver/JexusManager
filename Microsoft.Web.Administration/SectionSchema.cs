// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{Name}")]
    internal class SectionSchema
    {
        public string Name { get; }
        internal ConfigurationElementSchema Root;

        public SectionSchema(string name, XElement element, string fileName)
        {
            Name = name;
            Root = new ConfigurationElementSchema(fileName)
            {
                Path = name,
                AllowUnrecognizedAttributes =
                               element.Attribute("allowUnrecognizedAttributes")
                               .LoadBoolean(false)
            };
        }

        internal void ParseSectionSchema(XElement element, ConfigurationElementSchema schema, string fileName)
        {
            foreach (var node in element.Nodes())
            {
                var item = node as XElement;
                if (item == null)
                {
                    continue;
                }

                if (item.Name.LocalName == "attribute")
                {
                    var attribute = new ConfigurationAttributeSchema
                    {
                        Name = item.Attribute("name").Value,
                        Type = item.Attribute("type").Value,
                        IsRequired = item.Attribute("required").LoadBoolean(false),
                        IsUniqueKey = item.Attribute("isUniqueKey").LoadBoolean(false),
                        IsCombinedKey = item.Attribute("isCombinedKey").LoadBoolean(false),
                        IsCaseSensitive = item.Attribute("caseSensitive").LoadBoolean(false),
                        IsEncrypted = item.Attribute("encrypted").LoadBoolean(false),
                        AllowInfinite = item.Attribute("allowInfinite").LoadBoolean(false),
                        TimeSpanFormat = item.Attribute("timeSpanFormat").LoadString("string"),
                        IsExpanded = item.Attribute("expanded").LoadBoolean(false),
                        ValidationType = item.Attribute("validationType").LoadString(null),
                        ValidationParameter = item.Attribute("validationParameter").LoadString(null)
                    };

                    if (attribute.Type == "enum" || attribute.Type == "flags")
                    {
                        attribute.LoadEnums(item);
                    }

                    attribute.CreateValidator();

                    var defaultValue = item.Attribute("defaultValue");
                    if (defaultValue != null)
                    {
                        attribute.SetDefaultValue(defaultValue.Value);
                    }

                    if (schema == null)
                    {
                        Root.AttributeSchemas.Add(attribute);
                    }
                    else
                    {
                        schema.AttributeSchemas.Add(attribute);
                    }

                    continue;
                }

                if (item.Name.LocalName == "element")
                {
                    var name = item.Attribute("name").Value;
                    var top = schema ?? Root;
                    ConfigurationElementSchema child = top.ChildElementSchemas[name];
                    if (child == null)
                    {
                        child = new ConfigurationElementSchema(fileName)
                        {
                            Name = name,
                            IsCollectionDefault = item.Attribute("isCollectionDefault").LoadBoolean(false),
                            AllowUnrecognizedAttributes = top.AllowUnrecognizedAttributes
                        };
                        top.ChildElementSchemas.Add(child);
                        child.Path = string.Format("{0}/{1}", schema == null ? Name : schema.Path, child.Name);
                    }
                    else
                    {
                        // TODO: validation
                        if (item.Attribute("isCollectionDefault").LoadBoolean(false) != child.IsCollectionDefault)
                        {
                            throw new ArgumentException($"isCollectionDefault not equals: item {fileName} child {child.IsCollectionDefault} {child.FileName}");
                        }

                        if (top.AllowUnrecognizedAttributes != child.AllowUnrecognizedAttributes)
                        {
                            throw new ArgumentException($"allowUnrecognizedAttributes not equals: {fileName} child {child.AllowUnrecognizedAttributes} {child.FileName}");
                        }
                    }

                    ParseSectionSchema(item, child, fileName);
                }
                else if (item.Name.LocalName == "collection")
                {
                    var path = (schema == null) ? Name : schema.Path;
                    var addElementNames = item.Attribute("addElement").LoadString(string.Empty);
                    var removeElementName = item.Attribute("removeElement").LoadString(string.Empty);
                    var clearElementName = item.Attribute("clearElement").LoadString(string.Empty);
                    var isMergeAppend = item.Attribute("mergeAppend").LoadBoolean(true);
                    var allowDuplicates = item.Attribute("allowDuplicates").LoadBoolean(false);
                    var allowUnrecognizedAttributes = item.Attribute("allowUnrecognizedAttributes").LoadBoolean(false);
                    var child = new ConfigurationCollectionSchema(path, addElementNames, removeElementName,
                        clearElementName, isMergeAppend, allowDuplicates, allowUnrecognizedAttributes, fileName);

                    var top = schema ?? Root;
                    if (top.CollectionSchema == null)
                    {
                        top.CollectionSchema = child;
                    }
                    else
                    {
                        top.CollectionSchema.Merge(child);
                    }

                    ParseSectionSchema(item, top.CollectionSchema.AddSchemas[0], fileName);
                    top.CollectionSchema.ReplicateAddSchema();
                }
            }
        }
    }
}

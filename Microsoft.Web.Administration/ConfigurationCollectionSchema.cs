// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Web.Administration
{
    public sealed class ConfigurationCollectionSchema
    {
        internal ConfigurationCollectionSchema(string path, string addElementNames, string removeElementName, string clearElementName, bool isMergeAppend, bool allowDuplicates, bool allowUnrecognizedAttributes, string fileName)
        {
            IsMergeAppend = isMergeAppend;
            AllowDuplicates = allowDuplicates;
            AddElementNames = addElementNames;
            AddSchemas = new List<ConfigurationElementSchema>(_addNames.Length);
            foreach (var name in _addNames)
            {
                AddSchemas.Add(new ConfigurationElementSchema(fileName) { Path = path + '/' + name, AllowUnrecognizedAttributes = allowUnrecognizedAttributes, Name = name });
            }

            RemoveElementName = removeElementName;
            RemoveSchema = RemoveElementName == null
                               ? null
                               : new ConfigurationElementSchema(fileName)
                               {
                                   Path = path + '/' + RemoveElementName,
                                   AllowUnrecognizedAttributes =
                                             allowUnrecognizedAttributes,
                                   Name = RemoveElementName
                               };
            ClearElementName = clearElementName;
            ClearSchema = ClearElementName == null
                              ? null
                              : new ConfigurationElementSchema(fileName)
                              {
                                  Path = path + '/' + ClearElementName,
                                  AllowUnrecognizedAttributes =
                                            allowUnrecognizedAttributes,
                                  Name = ClearElementName
                              };
            Path = path;
        }

        public ConfigurationElementSchema GetAddElementSchema(string elementName)
        {
            return (from name in _addNames
                    where elementName == name
                    select AddSchemas.First(item => item.Path.EndsWith(name, System.StringComparison.Ordinal))).FirstOrDefault();
        }

        public ConfigurationElementSchema GetClearElementSchema()
        {
            return ClearSchema;
        }

        public object GetMetadata(string metadataType)
        {
            return null;
        }

        public ConfigurationElementSchema GetRemoveElementSchema()
        {
            return RemoveSchema;
        }

        public string AddElementNames
        {
            get
            {
                return _addElementNames;
            }

            internal set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _addElementNames = "add";
                    _addNames = new[] { "add" };
                    return;
                }

                _addElementNames = value;
                _addNames = value.Split(',');
            }
        }

        public bool AllowDuplicates { get; internal set; }

        public string ClearElementName
        {
            get { return _clearElementName; }
            internal set { _clearElementName = string.IsNullOrWhiteSpace(value) ? null : value; }
        }

        public bool IsMergeAppend { get; internal set; }

        public string RemoveElementName
        {
            get { return _removeElementName; }
            internal set { _removeElementName = string.IsNullOrWhiteSpace(value) ? null : value; }
        }

        internal ConfigurationElementSchema RemoveSchema;
        internal ConfigurationElementSchema ClearSchema;
        internal List<ConfigurationElementSchema> AddSchemas;
        private string _addElementNames;
        private string[] _addNames;
        private string _clearElementName;
        private string _removeElementName;

        internal string Path { get; }

        internal ConfigurationElementSchema FindSchema(string path)
        {
            var remove = string.Format("{0}/{1}", Path, RemoveElementName);
            var clear = string.Format("{0}/{1}", Path, ClearElementName);
            if (path == remove)
            {
                return RemoveSchema;
            }

            if (path == clear)
            {
                return ClearSchema;
            }

            foreach (string name in _addNames)
            {
                string add = string.Format("{0}/{1}", Path, name);
                if (path == add)
                {
                    return GetAddElementSchema(name);
                }
            }

            foreach (var name in _addNames)
            {
                var addRoot = GetAddElementSchema(name);
                var addSchema = addRoot.FindSchema(path);
                if (addSchema != null)
                {
                    return addSchema;
                }
            }

            // TODO: do we need clear and remove check?
            if (ClearSchema != null)
            {
                var root = ClearSchema;
                var result = root.FindSchema(path);
                if (result != null)
                {
                    return result;
                }
            }

            if (RemoveSchema != null)
            {
                var root = RemoveSchema;
                var result = root.FindSchema(path);
                return result;
            }

            return null;
        }

        internal void Merge(ConfigurationCollectionSchema child)
        {
            // TODO: validation
            if (string.IsNullOrEmpty(AddElementNames))
            {
                AddElementNames = child.AddElementNames;
            }

            if (string.IsNullOrEmpty(ClearElementName))
            {
                ClearElementName = child.ClearElementName;
            }

            if (string.IsNullOrEmpty(RemoveElementName))
            {
                RemoveElementName = child.RemoveElementName;
            }

            // TODO: is it OK?
            if (RemoveSchema != null && child.RemoveSchema != null)
            {
                RemoveSchema.AllowUnrecognizedAttributes = child.RemoveSchema.AllowUnrecognizedAttributes;
            }

            if (ClearSchema != null && child.ClearSchema != null)
            {
                ClearSchema.AllowUnrecognizedAttributes = child.ClearSchema.AllowUnrecognizedAttributes;
            }
        }

        internal bool ContainsAddElement(string elementTagName)
        {
            return _addNames.Any(item => item == elementTagName);
        }

        internal ConfigurationElementSchema GetElementSchema(string name)
        {
            if (name == ClearElementName)
            {
                return GetClearElementSchema();
            }

            return name == RemoveElementName ? GetRemoveElementSchema() : GetAddElementSchema(name);
        }

        internal void ReplicateAddSchema()
        {
            for (int i = 1; i < AddSchemas.Count; i++)
            {
                AddSchemas[i].AttributeSchemas = AddSchemas[0].AttributeSchemas;
                AddSchemas[i].ChildElementSchemas = AddSchemas[0].ChildElementSchemas;
                AddSchemas[i].CollectionSchema = AddSchemas[0].CollectionSchema;
            }

            foreach (ConfigurationAttributeSchema schema in AddSchemas[0].AttributeSchemas)
            {
                if (!schema.IsUniqueKey && !schema.IsCombinedKey)
                {
                    continue;
                }

                this.RemoveSchema?.AttributeSchemas.Add(schema);
            }
        }
    }
}

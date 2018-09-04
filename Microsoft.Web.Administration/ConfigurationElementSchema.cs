// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{Name}[{Path}]")]
    public sealed class ConfigurationElementSchema
    {
        internal ConfigurationElementSchema(string fileName)
        {
            FileName = fileName;
            AttributeSchemas = new ConfigurationAttributeSchemaCollection();
            ChildElementSchemas = new ConfigurationElementSchemaCollection();
        }

        public object GetMetadata(string metadataType)
        {
            return null;
        }

        public bool AllowUnrecognizedAttributes { get; internal set; }
        public ConfigurationAttributeSchemaCollection AttributeSchemas { get; internal set; }
        public ConfigurationElementSchemaCollection ChildElementSchemas { get; internal set; }
        public ConfigurationCollectionSchema CollectionSchema { get; internal set; }
        public bool IsCollectionDefault { get; internal set; }
        public string Name { get; internal set; }
        internal string Path { get; set; }
        internal string FileName { get; }

        internal ConfigurationElementSchema FindSchema(string path)
        {
            var index = path.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return null;
            }

            if (Path.Length != 0)
            {
                if (path.Length != Path.Length && path[Path.Length] != '/')
                {
                    return null;
                }
            }

            if (path == Path)
            {
                return this;
            }

            foreach (ConfigurationElementSchema child in ChildElementSchemas)
            {
                var result = child.FindSchema(path);
                if (result != null)
                {
                    return result;
                }
            }

            return CollectionSchema?.FindSchema(path);
        }
    }
}

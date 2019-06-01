// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{ElementTagName}[{Location}]")]
    public class ConfigurationSection : ConfigurationElement
    {
        private ConfigurationElementCollection _root;

        internal ConfigurationSection(string path, ConfigurationElementSchema schema, string location, FileContext core, XElement entity)
            : base(null, path, schema, null, entity, core, null, true, location)
        {
        }

        public void RevertToParent()
        {
            throw new NotImplementedException();
        }

        internal override XElement CreateEntity()
        {
            return this.FileContext.CreateElement(SectionPath, Location);
        }

        public new bool IsLocked { get; internal set; }

        public OverrideMode OverrideMode { get; set; }

        public OverrideMode OverrideModeEffective { get; internal set; }

        public string SectionPath
        {
            get { return ElementTagName; }
        }

        internal ConfigurationElementCollection Root
        {
            get { return _root ?? (_root = new ConfigurationElementCollection(SectionPath, Schema, null, this.InnerEntity, this.FileContext) { Section = this }); }
        }

        internal string Location { get; set; }

        internal override void AddChild(ConfigurationElement child)
        {
            if (Schema?.CollectionSchema == null)
            {
                base.AddChild(child);
            }
            else if (Schema.CollectionSchema.ContainsAddElement(child.ElementTagName))
            {
                Root.AddChild(child);
            }
            else if (child.ElementTagName == Schema.CollectionSchema.ClearElementName)
            {
                // TODO: test this
                Root.AddChild(child);
            }
            else if (child.ElementTagName == Schema.CollectionSchema.RemoveElementName)
            {
                // TODO: test this
                Root.AddChild(child);
            }
            else
            {
                base.AddChild(child);
            }
        }

        internal override ConfigurationElement GetElementByPath(string path)
        {
            return Schema?.CollectionSchema == null ? base.GetElementByPath(path) : Root.GetElementByPath(path);
        }
    }
}

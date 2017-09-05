// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Administration
{
    public class Configuration
    {
        internal Configuration(FileContext fileContext)
        {
            this.FileContext = fileContext;
        }

        internal FileContext FileContext { get; }

        public SectionGroup GetEffectiveSectionGroup()
        {
            return FileContext.GetEffectiveSectionGroup();
        }

        public string[] GetLocationPaths()
        {
            return this.FileContext.GetLocationPaths();
        }

        public object GetMetadata(string metadataType)
        {
            return FileContext.GetMetadata(metadataType);
        }

        public ConfigurationSection GetSection(string sectionPath)
        {
            return this.FileContext.GetSection(sectionPath);
        }

        public ConfigurationSection GetSection(string sectionPath, string locationPath)
        {
            return this.FileContext.GetSection(sectionPath, locationPath);
        }

        public ConfigurationSection GetSection(string sectionPath, Type type)
        {
            return FileContext.GetSection(sectionPath, type);
        }

        public ConfigurationSection GetSection(string sectionPath, Type type, string locationPath)
        {
            return FileContext.GetSection(sectionPath, type, locationPath);
        }

        public void RemoveLocationPath(string locationPath)
        {
            FileContext.RemoveLocationPath(locationPath);
        }

        public void RenameLocationPath(string locationPath, string newLocationPath)
        {
            FileContext.RenameLocationPath(locationPath, newLocationPath);
        }

        public void SetMetadata(string metadataType, object value)
        {
            FileContext.SetMetadata(metadataType, value);
        }

        public SectionGroup RootSectionGroup
        {
            get { return this.FileContext.RootSectionGroup; }
        }

        public event EventHandler CacheInvalidated;

        internal virtual void OnCacheInvalidated()
        {
            this.CacheInvalidated?.Invoke(this, EventArgs.Empty);
        }
    }
}

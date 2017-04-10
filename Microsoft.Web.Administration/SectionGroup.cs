// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{Name} [{_core.FileName}]")]
    public sealed class SectionGroup
    {
        internal const string KEYWORD_TRUE = "true";
        internal const string KEYWORD_FALSE = "false";
        internal const string KEYWORD_SECTION = "section";
        internal const string KEYWORD_SECTION_NAME = "name";
        internal const string KEYWORD_SECTION_ALLOWLOCATION = "allowLocation";
        internal const string KEYWORD_SECTION_ALLOWDEFINITION = "allowDefinition";
        internal const string KEYWORD_SECTION_ALLOWDEFINITION_APPHOSTONLY = "AppHostOnly";
        internal const string KEYWORD_SECTION_ALLOWDEFINITION_EVERYWHERE = "Everywhere";
        internal const string KEYWORD_SECTION_ALLOWDEFINITION_MACHINEONLY = "MachineOnly";
        internal const string KEYWORD_SECTION_ALLOWDEFINITION_MACHINETOAPPLICATION = "MachineToApplication";
        internal const string KEYWORD_SECTION_ALLOWDEFINITION_MACHINETOWEBROOT = "MachineToWebRoot";
        internal const string KEYWORD_SECTIONGROUP = "sectionGroup";
        internal const string KEYWORD_SECTIONGROUP_NAME = "name";
        internal const string KEYWORD_SECTION_OVERRIDEMODEDEFAULT = "overrideModeDefault";
        internal const string KEYWORD_LOCATION_OVERRIDEMODE = "overrideMode";
        internal const string KEYWORD_OVERRIDEMODE_INHERIT = "Inherit";
        internal const string KEYWORD_OVERRIDEMODE_ALLOW = "Allow";
        internal const string KEYWORD_OVERRIDEMODE_DENY = "Deny";

        private readonly FileContext _core;

        internal SectionGroup(FileContext core)
        {
            _core = core;
            SectionGroups = new SectionGroupCollection(core, this);
            Sections = new SectionDefinitionCollection(core);
            ConfigurationSections = new List<ConfigurationSection>();
            Name = string.Empty;
            Type = string.Empty;
            Path = string.Empty;
        }

        public string Name { get; internal set; }

        public SectionGroupCollection SectionGroups { get; }
        public SectionDefinitionCollection Sections { get; }
        public string Type { get; set; }

        internal void ParseSectionDefinitions(XElement root, Dictionary<string, SectionSchema> sectionSchemas)
        {
            foreach (var node in root.Nodes())
            {
                var element = node as XElement;
                if (element == null)
                {
                    continue;
                }

                if (element.Name.LocalName == KEYWORD_SECTIONGROUP)
                {
                    var group = SectionGroups.Add(element.Attribute(KEYWORD_SECTIONGROUP_NAME).Value);
                    group.Path = Name == string.Empty ? group.Name : string.Format("{0}/{1}", Path, @group.Name);
                    group.ParseSectionDefinitions(element, sectionSchemas);
                    continue;
                }

                if (element.Name.LocalName == KEYWORD_SECTION)
                {
                    var sectionDefinition = Sections.Add(element.Attribute(KEYWORD_SECTION_NAME).Value);
                    sectionDefinition.OverrideModeDefault = element.Attribute(KEYWORD_SECTION_OVERRIDEMODEDEFAULT).LoadString(KEYWORD_OVERRIDEMODE_ALLOW);
                    sectionDefinition.AllowLocation = element.Attribute(KEYWORD_SECTION_ALLOWLOCATION).LoadString(KEYWORD_TRUE);
                    sectionDefinition.AllowDefinition = element.Attribute(KEYWORD_SECTION_ALLOWDEFINITION).LoadString(KEYWORD_SECTION_ALLOWDEFINITION_EVERYWHERE);
                    sectionDefinition.Path = Name == string.Empty ? sectionDefinition.Name : string.Format("{0}/{1}", Path, sectionDefinition.Name);

                    SectionSchema schema;
                    sectionSchemas.TryGetValue(sectionDefinition.Path, out schema);
                    sectionDefinition.Schema = schema;
                }
            }
        }

        internal string Path { get; set; }

        internal List<ConfigurationSection> ConfigurationSections { get; }

        internal ConfigurationSection FindSection(string sectionPath, string locationPath, FileContext core)
        {
            var temp = DetectSection(sectionPath, locationPath, core);
            if (temp != null)
            {
                return temp;
            }

            return CreateSection(sectionPath, locationPath, core, core);
        }

        private ConfigurationSection CreateSection(string sectionPath, string locationPath, FileContext core, FileContext top)
        {
            var index = sectionPath.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return null;
            }

            if (this.Path.Length != 0)
            {
                if (sectionPath.Length != this.Path.Length && sectionPath[this.Path.Length] != '/')
                {
                    return null;
                }
            }

            var definition = Sections.FirstOrDefault(item => item.Path == sectionPath);
            if (definition?.Schema != null)
            {
                var section = new ConfigurationSection(sectionPath, definition.Schema.Root, locationPath,
                    top, null);
                section.OverrideMode = OverrideMode.Inherit;
                section.OverrideModeEffective =
                    (OverrideMode)Enum.Parse(typeof(OverrideMode), definition.OverrideModeDefault);
                section.IsLocked = section.FileContext.FileName != definition.FileContext.FileName && section.OverrideModeEffective != OverrideMode.Allow;
                section.IsLocallyStored = section.FileContext.FileName == definition.FileContext.FileName;
                ConfigurationSections.Add(section);
                return section;
            }

            foreach (SectionGroup group in SectionGroups)
            {
                var created = group.CreateSection(sectionPath, locationPath, core, top);
                if (created != null)
                {
                    return created;
                }
            }

            var sectionBasedOnParent = core.Parent?.RootSectionGroup.CreateSection(sectionPath, locationPath, core.Parent, top);
            return sectionBasedOnParent;
        }

        private ConfigurationSection DetectSection(string sectionPath, string locationPath, FileContext core)
        {
            var index = sectionPath.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return null;
            }

            if (this.Path.Length != 0)
            {
                if (sectionPath.Length != this.Path.Length && sectionPath[this.Path.Length] != '/')
                {
                    return null;
                }
            }

            foreach (ConfigurationSection section in ConfigurationSections)
            {
                if (section.ElementTagName == sectionPath && section.Location == locationPath && !section.IsLocked)
                {
                    return section;
                }
            }

            foreach (SectionGroup group in SectionGroups)
            {
                var found = group.DetectSection(sectionPath, locationPath, core);
                if (found != null)
                {
                    return found;
                }
            }

            var definition = Sections.FirstOrDefault(item => item.Path == sectionPath);
            if (definition?.Schema == null)
            {
                var sectionInParent = core.Parent?.RootSectionGroup.DetectSection(sectionPath, locationPath, core.Parent);
                return sectionInParent;
            }

            return null;
        }

        internal bool Add(ConfigurationSection section)
        {
            var index = section.ElementTagName.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return false;
            }

            if (this.Path.Length != 0)
            {
                if (section.ElementTagName.Length != this.Path.Length && section.ElementTagName[this.Path.Length] != '/')
                {
                    return false;
                }
            }

            var definition = Sections.FirstOrDefault(item => item.Path == section.ElementTagName);
            if (definition != null)
            {
                if (definition.AllowLocation == KEYWORD_FALSE && section.Location != null)
                {
                    throw new ServerManagerException("Section is not allowed in location tag");
                }

                section.OverrideMode = OverrideMode.Inherit;
                section.OverrideModeEffective =
                    (OverrideMode)Enum.Parse(typeof(OverrideMode), definition.OverrideModeDefault);

                section.IsLocked = section.FileContext.FileName != definition.FileContext.FileName
                                   && section.OverrideModeEffective != OverrideMode.Allow;
                section.IsLocallyStored = section.FileContext.FileName == definition.FileContext.FileName;
                ConfigurationSections.Add(section);
                return true;
            }

            if (SectionGroups.Select(@group => @group.Add(section)).Any(result => result))
            {
                return true;
            }

            if (section.SectionPath == "configProtectedData")
            {
                ConfigurationSections.Add(section);
                return true;
            }

            throw new FileLoadException(string.Format(
                "Filename: \\\\?\\{0}\r\nLine number: {1}\r\nError: This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n\r\n",
                section.FileContext.FileName,
                (section.Entity as IXmlLineInfo).LineNumber));
        }

        static internal ConfigurationAllowDefinition AllowDefinitionToEnum(string allowDefinition)
        {
            switch (allowDefinition)
            {
                case KEYWORD_SECTION_ALLOWDEFINITION_EVERYWHERE:
                    return ConfigurationAllowDefinition.Everywhere;

                case KEYWORD_SECTION_ALLOWDEFINITION_MACHINEONLY:
                    return ConfigurationAllowDefinition.MachineOnly;

                case KEYWORD_SECTION_ALLOWDEFINITION_MACHINETOAPPLICATION:
                    return ConfigurationAllowDefinition.MachineToApplication;

                case KEYWORD_SECTION_ALLOWDEFINITION_MACHINETOWEBROOT:
                    return ConfigurationAllowDefinition.MachineToWebRoot;

                case KEYWORD_SECTION_ALLOWDEFINITION_APPHOSTONLY:
                    return ConfigurationAllowDefinition.AppHostOnly;

                default:
                    throw new ServerManagerException("Invalid allow definition string");
            }
        }
    }
}

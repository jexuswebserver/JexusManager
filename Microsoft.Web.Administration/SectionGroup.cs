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
        internal const string KEYWORD_SECTION_TYPE = "type";
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
                    sectionDefinition.Path = Name == string.Empty ? sectionDefinition.Name : $"{Path}/{sectionDefinition.Name}";
                    sectionDefinition.Type = element.Attribute(KEYWORD_SECTION_TYPE).LoadString(string.Empty);

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
                var duplicate = core.Parent?.RootSectionGroup.DetectSection(sectionPath, locationPath, core.Parent);
                if (duplicate != null && !temp.IsLocallyStored)
                {
                    temp.IsLocallyStored = true;
                }

                return temp;
            }

            // IMPORTANT: force system.web to go to root web.config.
            var top = locationPath == null && sectionPath.StartsWith("system.web/") && core.AppHost ? core.Parent : core;
            return CreateSection(sectionPath, locationPath, top, top);
        }

        private ConfigurationSection CreateSection(string sectionPath, string locationPath, FileContext core, FileContext top)
        {
            var index = sectionPath.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return null;
            }

            if (Path.Length != 0)
            {
                if (sectionPath.Length != Path.Length && sectionPath[Path.Length] != '/')
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
                if (locationPath == null)
                {
                    section.OverrideModeEffective = (OverrideMode)Enum.Parse(typeof(OverrideMode), definition.OverrideModeDefault);
                }
                else
                {
                    var parent = FindSection(sectionPath, locationPath.GetParentLocation(), core);
                    section.OverrideModeEffective = parent.OverrideModeEffective;
                }

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

            if (Path.Length != 0)
            {
                if (sectionPath.Length != Path.Length && sectionPath[Path.Length] != '/')
                {
                    return null;
                }
            }

            foreach (ConfigurationSection section in ConfigurationSections)
            {
                if (section.FileContext == core)
                {
                    if (section.ElementTagName == sectionPath && section.Location == locationPath && !section.IsLocked)
                    {
                        return section;
                    }
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

        internal bool Add(ConfigurationSection section, Location location)
        {
            var index = section.ElementTagName.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return false;
            }

            if (Path.Length != 0)
            {
                if (section.ElementTagName.Length != Path.Length && section.ElementTagName[Path.Length] != '/')
                {
                    return false;
                }
            }

            var definition = Sections.FirstOrDefault(item => item.Path == section.ElementTagName);
            if (definition != null)
            {
                if (definition.AllowLocation == KEYWORD_FALSE && location != null && location.FromTag)
                {
                    throw new ServerManagerException("Section is not allowed in location tag");
                }

                section.OverrideMode = location == null || location.OverrideMode == null 
                    ? OverrideMode.Inherit
                    : (OverrideMode)Enum.Parse(typeof(OverrideMode), location.OverrideMode);

                if (section.OverrideMode == OverrideMode.Inherit)
                {
                    var parent = location == null || location.Path == null ? null : FindSection(section.SectionPath, location.Path.GetParentLocation(), section.FileContext);
                    if (parent == null)
                    {
                        section.OverrideModeEffective = (OverrideMode)Enum.Parse(typeof(OverrideMode), definition.OverrideModeDefault);
                    }
                    else
                    {
                        section.OverrideModeEffective = parent.OverrideModeEffective;
                    }
                }
                else
                {
                    section.OverrideModeEffective = section.OverrideMode;
                }

                section.IsLocked = section.FileContext.FileName != definition.FileContext.FileName
                                   && section.OverrideModeEffective != OverrideMode.Allow;
                section.IsLocallyStored = section.FileContext.FileName == definition.FileContext.FileName;
                ConfigurationSections.Add(section);
                return true;
            }

            if (SectionGroups.Select(@group => @group.Add(section, location)).Any(result => result))
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

        internal static ConfigurationAllowDefinition AllowDefinitionToEnum(string allowDefinition)
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

        internal SectionDefinition GetChildSectionDefinition(string path)
        {
            var index = path.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return null;
            }

            foreach (var definition in Sections)
            {
                if (definition.Path.StartsWith(path + "/"))
                {
                    return definition;
                }
            }

            foreach (var child in SectionGroups)
            {
                var result = child.GetChildSectionDefinition(path);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        internal SectionDefinition GetSectionDefinition(string path)
        {
            var index = path.IndexOf(Path, StringComparison.Ordinal);
            if (index != 0)
            {
                return null;
            }

            foreach (var definition in Sections)
            {
                if (definition.Path == path)
                {
                    return definition;
                }
            }

            foreach (var child in SectionGroups)
            {
                var result = child.GetSectionDefinition(path);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}

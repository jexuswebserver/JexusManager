// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Microsoft.Web.Administration
{
    [DebuggerDisplay("{FileName}[{Location}]")]
    internal sealed class FileContext : IEquatable<FileContext>
    {
        private readonly ServerManager _server;
        private readonly object _locker = new object();
        private readonly bool _dontThrow;
        internal List<SectionDefinition> DefinitionCache = new List<SectionDefinition>();

        internal bool AppHost { get; }
        public bool ReadOnly { get; }

        private ProtectedConfiguration _protectedConfiguration;

        internal FileContext(ServerManager server, string fileName, FileContext parent, string location, bool appHost, bool dontThrow, bool readOnly, int lineNumber = 0)
        {
            _server = server;
            _dontThrow = dontThrow;
            AppHost = appHost;
            ReadOnly = readOnly;
            Locations = new List<Location>();
            FileName = fileName;
            Location = location;
            Parent = parent;
            _lineNumber = lineNumber;
        }

        private void Initialize()
        {
            Parent?.Initialize();
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            if (Parent != null)
            {
                foreach(var item in Parent.DefinitionCache)
                {
                    DefinitionCache.Add(item);
                }
            }

            LoadSchemas();
            _rootSectionGroup = new SectionGroup(this);
            if (FileName == null)
            {
                // TODO: merge with the other exception later.
                throw new FileNotFoundException(
                    $"Filename: \\\\?\\{_server.FileName}\r\nLine number: {_lineNumber}\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/{Location}'\r\n\r\n");
            }

            var file = FileName.ExpandIisExpressEnvironmentVariables();
            if (!File.Exists(file))
            {
                if (AppHost)
                {
                    _initialized = false;
                    throw new FileNotFoundException(
                        $"Filename: \\\\?\\{FileName}\r\nError: Cannot read configuration file\r\n\r\n",
                        FileName);
                }

                return;
            }

            try
            {
                LoadDocument(file, Location);
            }
            catch (XmlException ex)
            {
                _initialized = false;
                throw new COMException(
                    $"Filename: \\\\?\\{file}\r\nLine number: {(ex.LineNumber == 0 ? 1 : ex.LineNumber)}\r\nError: Configuration file is not well-formed XML\r\n\r\n");
            }
            catch (COMException ex)
            {
                _initialized = false;
                var exception = new COMException($"Filename: \\\\?\\{file}\r\n{ex.Message}\r\n");
                foreach (object key in ex.Data.Keys)
                {
                    exception.Data.Add(key, ex.Data[key]);
                }

                throw exception;
            }
        }

        private void LoadSchemas()
        {
            _sectionSchemas.Clear();
            if (Parent == null)
            {
                LoadSchemasFromMode();
            }
            else
            {
                foreach (var item in Parent._sectionSchemas)
                {
                    _sectionSchemas.Add(item.Key, item.Value);
                }
            }
        }

        public void Save()
        {
            if (!_initialized || !_dirty)
            {
                return;
            }

            lock (_locker)
            {
                if (AppHost)
                {
                    // TODO: load settings from applicationHost.config.
                    var historyFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Jexus Manager",
                        "history");
                    int last = 0;
                    if (!Directory.Exists(historyFolder))
                    {
                        Directory.CreateDirectory(historyFolder);
                    }
                    else
                    {
                        var existing = new DirectoryInfo(historyFolder).GetDirectories();
                        last =
                            existing.Select(found => found.Name.Substring("CFGHISTORY_".Length))
                                .Select(int.Parse)
                                .Concat(new[] { last })
                                .Max();
                    }

                    last++;
                    var revisionFolder = Path.Combine(historyFolder, "CFGHISTORY_" + last.ToString("D10"));
                    Directory.CreateDirectory(revisionFolder);
                    var fileName = Path.GetFileName(FileName);
                    if (fileName != null)
                    {
                        File.Copy(FileName, Path.Combine(revisionFolder, fileName));
                    }
                }

                if (_document != null)
                {
                    try
                    {
                        using (var stream = File.Open(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            _document.Save(stream);
                        }
                    }
                    catch (SystemException ex)
                    {
                        Rollbar.RollbarLocator.RollbarInstance.Error(ex);
                    }
                }
            }
        }

        public string FileName { get; set; }

        public FileContext Parent { get; }
        public string Location { get; }

        internal List<ConfigurationSection> ConfigurationSections { get; } = new List<ConfigurationSection>();

        private SectionGroup _effective;

        public SectionGroup GetEffectiveSectionGroup()
        {
            if (_effective != null)
            {
                return _effective;
            }

            _effective = new SectionGroup(this);
            FileContext core = this;
            while (core != null)
            {
                SectionGroup root = core.RootSectionGroup;
                foreach (SectionGroup item in root.SectionGroups)
                {
                    _effective.SectionGroups.Add(item);
                }

                foreach (SectionDefinition item in root.Sections)
                {
                    _effective.Sections.Add(item);
                }
                
                core = core.Parent;
            }

            return _effective;
        }

        public string[] GetLocationPaths()
        {
            Initialize();
            return Locations.Select(item => item.Path).ToArray();
        }

        public object GetMetadata(string metadataType)
        {
            throw new NotImplementedException();
        }

        public ConfigurationSection GetSection(string sectionPath)
        {
            return GetSection(sectionPath, Location);
        }

        public ConfigurationSection GetSection(string sectionPath, string locationPath)
        {
            Initialize();
            if (!_dontThrow)
            {
                _server.VerifyLocation(locationPath);
            }

            return FindSection(sectionPath, locationPath, this);
        }

        public ConfigurationSection GetSection(string sectionPath, Type type)
        {
            throw new NotImplementedException();
        }

        public ConfigurationSection GetSection(string sectionPath, Type type, string locationPath)
        {
            throw new NotImplementedException();
        }

        public void RemoveLocationPath(string locationPath)
        {
            Initialize();
            var location = Locations.FirstOrDefault(item => item.Path == locationPath);
            if (location != null)
            {
                Locations.Remove(location);
            }
        }

        public void RenameLocationPath(string locationPath, string newLocationPath)
        {
            Initialize();
            var location = Locations.FirstOrDefault(item => item.Path == locationPath);
            if (location != null)
            {
                location.Path = newLocationPath;
            }
        }

        public void SetMetadata(string metadataType, object value)
        {
            throw new NotImplementedException();
        }

        public SectionGroup RootSectionGroup
        {
            get
            {
                Initialize();
                return _rootSectionGroup;
            }
            set
            {
                _rootSectionGroup = value;
            }
        }

        private List<Location> Locations { get; }

        private readonly Dictionary<string, SectionSchema> _sectionSchemas = new Dictionary<string, SectionSchema>();
        private XDocument _document;

        private bool _initialized;

        private SectionGroup _rootSectionGroup;

        private bool _dirty;
        private readonly int _lineNumber;

        private XElement Root => _document?.Root;

        public ProtectedConfiguration ProtectedConfiguration
        {
            get
            {
                Initialize();
                return _protectedConfiguration
                       ?? (_protectedConfiguration =
                           new ProtectedConfiguration(GetSection("configProtectedData")));
            }
        }

        private ConfigurationElementSchema FindSchema(string path)
        {
            return _sectionSchemas.Values.Select(section => section.Root.FindSchema(path)).FirstOrDefault(result => result != null);
        }

        private SectionSchema FindSectionSchema(string path)
        {
            _sectionSchemas.TryGetValue(path, out SectionSchema result);
            return result;
        }

        private void LoadSchemasFromMode()
        {
            foreach (var file in _server.GetSchemaFiles())
            {
                var schemaDoc = XDocument.Load(file);
                LoadSchema(schemaDoc);
            }
        }

        private void LoadSchema(XDocument document)
        {
            if (document.Root == null)
            {
                return;
            }

            var nodes = document.Root.Nodes();
            foreach (var node in nodes)
            {
                if (node is XComment)
                {
                    continue;
                }

                if (!(node is XElement element))
                {
                    continue;
                }

                if (element.Name.LocalName != "sectionSchema")
                {
                    continue;
                }

                var name = element.Attribute("name")?.Value;
                var found = FindSectionSchema(name);
                if (found == null)
                {
                    found = new SectionSchema(name, element);
                    Debug.Assert(name != null, nameof(name) + " != null");
                    _sectionSchemas.Add(name, found);
                }

                found.ParseSectionSchema(element, null);
            }
        }

        private bool ParseSections(XElement element, ConfigurationElement parent, Location location)
        {
            var result = false;
            var path = element.GetPath();
            ConfigurationElement node = null;
            var schema = FindSchema(path);
            var sec = FindSectionSchema(path);
            var name = element.Name.LocalName;
            if (schema != null)
            {
                if (sec != null)
                {
                    var section = new ConfigurationSection(path, sec.Root, location?.Path, this, element);
                    Add(section, location, this);
                    node = section;
                }
                else
                {
                    node = schema.CollectionSchema == null
                               ? new ConfigurationElement(null, name, schema, parent, element, this)
                               : new ConfigurationElementCollection(
                                     name,
                                     schema,
                                     parent,
                                     element,
                                     this);
                }

                parent?.AddChild(node);
                result = true;
            }
            else
            {
                var found = DefinitionCache.FirstOrDefault(item => item.Path == path);
                if (found != null)
                {
                    if (found.Ignore)
                    {
                        if (path == "runtime")
                        {
                            // examples: <runtime> in machine.config.
                            return true;
                        }
                        else
                        {
                            if (!element.HasElements)
                            {
                                // like empty tag in web.config.
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    // TODO: improve performance.
                    var foundChild = DefinitionCache.FirstOrDefault(item => item.Path.StartsWith(path + '/'));
                    if (foundChild != null && !element.HasElements)
                    {
                        return true;
                    }
                }
            }

            foreach (var item in element.Nodes())
            {
                if (!(item is XElement child))
                {
                    continue;
                }

                var childAdded = ParseSections(child, node, location);
                result = result || childAdded;
            }

            if (!result && !_dontThrow && name != "location")
            {
                string link = null;
                string oob = null;
                if (path.StartsWith("system.webServer/aspNetCore/"))
                {
                    oob = "ASP.NET Core Module (system.webServer/aspNetCore/)";
                    link = "https://docs.microsoft.com/en-us/aspnet/core/publishing/iis?tabs=aspnetcore2x#install-the-net-core-windows-server-hosting-bundle";
                }
                else if (path.StartsWith("system.webServer/rewrite/"))
                {
                    oob = "URL Rewrite Module (system.webServer/rewrite/)";
                    link = "https://docs.microsoft.com/en-us/iis/extensions/url-rewrite-module/using-the-url-rewrite-module#where-to-get-the-url-rewrite-module";
                }
                else if (path.StartsWith("system.webServer/webFarms/"))
                {
                    oob = "Application Request Routing Module (system.webServer/webFarms/)";
                    link = "https://docs.microsoft.com/en-us/iis/extensions/configuring-application-request-routing-arr/define-and-configure-an-application-request-routing-server-farm#prerequisites";
                }
                else if (path.StartsWith("system.webServer/httpPlatform/"))
                {
                    oob = "HttpPlatformHandler Module (system.webServer/httpPlatform/)";
                    link = "https://docs.microsoft.com/en-us/iis/extensions/httpplatformhandler/httpplatformhandler-configuration-reference";
                }

                var exception = new COMException(
                    $"Line number: {(element as IXmlLineInfo).LineNumber}\r\nError: Unrecognized element '{name}'\r\n");
                if (oob != null)
                {
                    exception.Data.Add("oob", oob);
                    exception.Data.Add("link", link);
                }

                throw exception;
            }

            return result;
        }

        private void LoadDocument(string file, string location)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                _document = XDocument.Load(stream, LoadOptions.SetLineInfo);
            }

            if (Root == null)
            {
                return;
            }

            var nodes = Root.Nodes();
            foreach (var node in nodes)
            {
                if (node is XComment)
                {
                    continue;
                }

                if (node is XElement element)
                {
                    var tag = element.Name.LocalName;
                    if (tag == "configSections")
                    {
                        RootSectionGroup.ParseSectionDefinitions(element, _sectionSchemas);
                        if (Parent == null)
                        {
                            // machine.config
                            RootSectionGroup.AddSpecial();
                        }

                        // TODO: can we use _sectionSchemas.
                        RootSectionGroup.GetAllDefinitions(DefinitionCache);
                        continue;
                    }

                    if (tag == "location")
                    {
                        // IMPORTANT: allow null path due to root web.config.
                        var path = element.Attribute("path")?.Value;
                        var mode = element.Attribute("overrideMode").LoadString("Inherit");
                        var found = Locations.FirstOrDefault(item => item.Path == path);
                        if (found == null)
                        {
                            found = new Location(path, mode, element, true);
                            Locations.Add(found);
                        }

                        ParseSections(element, null, found);
                        continue;
                    }

                    if (location == null)
                    {
                        ParseSections(element, null, null);
                    }
                    else
                    {
                        var found = Locations.FirstOrDefault(item => item.Path == location);
                        if (found == null)
                        {
                            found = new Location(location, "Inherit", element);
                            Locations.Add(found);
                        }

                        ParseSections(element, null, found);
                    }
                }
            }
        }

        internal XElement CreateElement(string sectionPath, string locationPath)
        {
            var parts = sectionPath.Split('/');
            var top = EnsureDocumentExists();
            if (locationPath != null && locationPath != Location)
            {
                var elements = Root.XPathSelectElements($"//location[@path='{locationPath}']").ToList();
                if (!elements.Any())
                {
                    var location = new XElement("location",
                        new XAttribute("path", locationPath));
                    top.Add(location);
                    top = location;
                }
                else
                {
                    top = elements.First();
                }
            }

            foreach (var part in parts)
            {
                var current = top.Element(part);
                if (current == null)
                {
                    current = new XElement(part);
                    top.Add(current);
                }

                top = current;
            }

            return top;
        }

        private XElement EnsureDocumentExists()
        {
            if (Root != null)
            {
                return Root;
            }

            var file = FileName.ExpandIisExpressEnvironmentVariables();
            var directory = Path.GetDirectoryName(file);
            if (!Directory.Exists(directory))
            {
                Debug.Assert(directory != null, nameof(directory) + " != null");
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(file, @"<?xml version=""1.0"" encoding=""utf-8""?><configuration></configuration>");
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                _document = XDocument.Load(stream, LoadOptions.SetLineInfo);
            }

            return Root;
        }

        public bool Equals(FileContext other)
        {
            return other != null && other.FileName == FileName;
        }

        internal void SetDirty()
        {
            if (!_initialized)
            {
                return;
            }

            if (!_server.Initialized)
            {
                return;
            }

            if (ReadOnly)
            {
                throw new InvalidOperationException("The configuration object is read only, because it has been committed by a call to ServerManager.CommitChanges(). If write access is required, use ServerManager to get a new reference.");
            }

            _dirty = true;
        }

        internal ConfigurationSection FindSection(string sectionPath, string locationPath, FileContext core)
        {
            var temp = DetectExistingSection(sectionPath, locationPath);
            if (temp != null)
            {
                var duplicate = core.Parent?.DetectExistingSection(sectionPath, locationPath);
                if (duplicate != null && !temp.IsLocallyStored)
                {
                    temp.IsLocallyStored = true;
                }

                return temp;
            }

            // IMPORTANT: force system.web to go to root web.config.
            var top = locationPath == null && sectionPath.StartsWith("system.web/") && core.AppHost ? core.Parent : core;
            return CreateSection(sectionPath, locationPath, top);
        }

        private ConfigurationSection CreateSection(string sectionPath, string locationPath, FileContext core)
        {
            if (Location == null || Location == locationPath || locationPath.StartsWith(Location + '/'))
            {
                var definition = DefinitionCache.FirstOrDefault(item => item.Path == sectionPath);
                if (definition?.Schema != null)
                {
                    var section = new ConfigurationSection(
                            sectionPath,
                            definition.Schema.Root,
                            locationPath,
                            core,
                            null)
                        {OverrideMode = OverrideMode.Inherit};
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
            }

            var sectionBasedOnParent = core.Parent?.CreateSection(sectionPath, locationPath, core.Parent);
            return sectionBasedOnParent;
        }

        private ConfigurationSection DetectExistingSection(string sectionPath, string locationPath)
        {
            foreach (ConfigurationSection section in ConfigurationSections)
            {
                if (section.ElementTagName == sectionPath && section.Location == locationPath && !section.IsLocked)
                {
                    return section;
                }
            }

            return Parent?.DetectExistingSection(sectionPath, locationPath);
        }

        internal bool Add(ConfigurationSection section, Location location, FileContext top)
        {
            var definition = DefinitionCache.FirstOrDefault(item => item.Path == section.ElementTagName);
            if (definition != null)
            {
                if (definition.AllowLocation == SectionGroup.KEYWORD_FALSE && location != null && location.FromTag)
                {
                    throw new ServerManagerException("Section is not allowed in location tag");
                }

                section.OverrideMode = location?.OverrideMode == null
                    ? OverrideMode.Inherit
                    : (OverrideMode)Enum.Parse(typeof(OverrideMode), location.OverrideMode);

                if (section.OverrideMode == OverrideMode.Inherit)
                {
                    var parent = location?.Path == null ? null : section.FileContext.FindSection(section.SectionPath, location.Path.GetParentLocation(), section.FileContext);
                    if (parent == null)
                    {
                        section.OverrideModeEffective = (OverrideMode)Enum.Parse(typeof(OverrideMode), definition.OverrideModeDefault);
                    }
                    else
                    {
                        if (parent.OverrideModeEffective == OverrideMode.Deny && parent.FileContext != this)
                        {
                            throw new FileLoadException(
                                $"Filename: \\\\?\\{section.FileContext.FileName}\r\nLine number: {(section.Entity as IXmlLineInfo).LineNumber}\r\nError: This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n\r\n");
                        }

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
                top.ConfigurationSections.Add(section);
                return true;
            }

            if (Parent != null)
            {
                var parentContext = Parent.Add(section, location, top);
                if (parentContext)
                {
                    return true;
                }
            }

            throw new FileLoadException(
                $"Filename: \\\\?\\{section.FileContext.FileName}\r\nLine number: {(section.Entity as IXmlLineInfo).LineNumber}\r\nError: This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n\r\n");
        }
    }
}

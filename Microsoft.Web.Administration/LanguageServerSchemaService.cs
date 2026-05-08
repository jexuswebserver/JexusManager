#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Microsoft.Web.Administration
{
    internal enum LanguageServerSymbolKind
    {
        SectionGroup,
        Section,
        CollectionItem,
        Element,
        Attribute,
        EnumValue
    }

    internal sealed record LanguageServerSymbol(
        LanguageServerSymbolKind Kind,
        string Name,
        string Path,
        string? ParentPath,
        string? Type,
        string? DefaultValue,
        bool Required,
        IReadOnlyList<string> EnumValues,
        string? FilePath,
        int LineNumber);

    internal sealed class LanguageServerSchemaService
    {
        private readonly Dictionary<string, SectionSchema> _sectionSchemas = new(StringComparer.Ordinal);

        internal LanguageServerSchemaService(IEnumerable<string> schemaFiles)
        {
            foreach (var file in schemaFiles.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!File.Exists(file))
                {
                    continue;
                }

                var document = XDocument.Load(file, LoadOptions.SetLineInfo);
                LoadSchema(document, file);
            }

            var configBuilders = XDocument.Parse(
                """
                <configSchema>
                  <sectionSchema name="configBuilders">
                    <element name="builders">
                      <collection addElement="add" removeElement="remove" clearElement="clear" allowUnrecognizedAttributes="true">
                        <attribute name="name" required="true" isUniqueKey="true" type="string" />
                        <attribute name="type" required="true" type="string" />
                        <attribute name="prefix" type="string" />
                        <attribute name="mode" type="string" defaultValue="Strict" />
                        <attribute name="stripPrefix" type="bool" />
                        <attribute name="tokenPattern" type="string" />
                      </collection>
                    </element>
                  </sectionSchema>
                </configSchema>
                """,
                LoadOptions.SetLineInfo);
            LoadSchema(configBuilders, string.Empty);
        }

        internal static LanguageServerSchemaService CreateDefault()
        {
            var schemaFiles = new IisExpressServerManager(readOnly: true, applicationHostConfigurationPath: "applicationhost.config")
                .GetSchemaFiles();
            return new LanguageServerSchemaService(schemaFiles);
        }

        internal IReadOnlyList<string> GetChildElementNames(string elementPath)
        {
            var normalizedPath = NormalizePath(elementPath);
            if (string.IsNullOrEmpty(normalizedPath))
            {
                return [];
            }

            var groupMatch = ResolveSectionGroup(normalizedPath);
            if (groupMatch != null && groupMatch.Path == normalizedPath)
            {
                return _sectionSchemas.Keys
                    .Where(key => key.StartsWith(groupMatch.Path + "/", StringComparison.Ordinal))
                    .Select(key => key[(groupMatch.Path.Length + 1)..].Split('/')[0])
                    .Distinct(StringComparer.Ordinal)
                    .OrderBy(name => name, StringComparer.Ordinal)
                    .ToList();
            }

            var node = ResolveNode(normalizedPath);
            if (node == null)
            {
                return [];
            }

            var names = new List<string>();
            foreach (ConfigurationElementSchema child in node.Schema.ChildElementSchemas)
            {
                names.Add(child.Name);
            }

            if (node.Schema.CollectionSchema != null)
            {
                foreach (var addName in node.Schema.CollectionSchema.AddElementNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    names.Add(addName);
                }

                if (!string.IsNullOrEmpty(node.Schema.CollectionSchema.RemoveElementName))
                {
                    names.Add(node.Schema.CollectionSchema.RemoveElementName);
                }

                if (!string.IsNullOrEmpty(node.Schema.CollectionSchema.ClearElementName))
                {
                    names.Add(node.Schema.CollectionSchema.ClearElementName);
                }
            }

            return names
                .Distinct(StringComparer.Ordinal)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToList();
        }

        internal IReadOnlyList<string> GetAttributeNames(string elementPath)
        {
            var node = ResolveNode(NormalizePath(elementPath));
            return node == null
                ? []
                : node.Schema.AttributeSchemas.Select(schema => schema.Name).OrderBy(name => name, StringComparer.Ordinal).ToList();
        }

        internal string? GetAttributeType(string elementPath, string attributeName)
        {
            return ResolveAttribute(elementPath, attributeName)?.Type;
        }

        internal IReadOnlyList<string> GetAttributeValues(string elementPath, string attributeName)
        {
            return ResolveAttribute(elementPath, attributeName)?.EnumValues ?? [];
        }

        internal bool GetAllowUnrecognizedAttributes(string elementPath)
        {
            var node = ResolveNode(NormalizePath(elementPath));
            // If no schema found, be lenient and allow all attributes
            return node?.Schema.AllowUnrecognizedAttributes ?? true;
        }

        internal LanguageServerSymbol? ResolveElement(string elementPath)
        {
            var normalizedPath = NormalizePath(elementPath);
            if (string.IsNullOrEmpty(normalizedPath))
            {
                return null;
            }

            var group = ResolveSectionGroup(normalizedPath);
            if (group != null && group.Path == normalizedPath)
            {
                return group;
            }

            var node = ResolveNode(normalizedPath);
            return node == null ? null : CreateElementSymbol(node);
        }

        internal LanguageServerSymbol? ResolveAttribute(string elementPath, string attributeName)
        {
            var node = ResolveNode(NormalizePath(elementPath));
            if (node == null)
            {
                return null;
            }

            var attribute = node.Schema.AttributeSchemas[attributeName];
            return attribute == null ? null : CreateAttributeSymbol(node, attribute);
        }

        internal LanguageServerSymbol? ResolveAttributeValue(string elementPath, string attributeName, string? attributeValue)
        {
            var attribute = ResolveAttribute(elementPath, attributeName);
            if (attribute == null || string.IsNullOrEmpty(attributeValue))
            {
                return attribute;
            }

            var node = ResolveNode(NormalizePath(elementPath));
            if (node == null)
            {
                return attribute;
            }

            var attributeSchema = node.Schema.AttributeSchemas[attributeName];
            var enumValue = attributeSchema?.GetEnumValues()[attributeValue];
            if (enumValue == null)
            {
                return attribute;
            }

            return attribute with
            {
                Kind = LanguageServerSymbolKind.EnumValue,
                Name = attributeValue,
                Path = $"{attribute.Path}={attributeValue}"
            };
        }

        private void LoadSchema(XDocument document, string fileName)
        {
            if (document.Root == null)
            {
                return;
            }

            foreach (var node in document.Root.Nodes())
            {
                if (node is not XElement element || element.Name.LocalName != "sectionSchema")
                {
                    continue;
                }

                var name = element.Attribute("name")?.Value;
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                if (!_sectionSchemas.TryGetValue(name, out var section))
                {
                    section = new SectionSchema(name, element, fileName);
                    _sectionSchemas.Add(name, section);
                }

                section.ParseSectionSchema(element, null, fileName);
            }
        }

        private LanguageServerSymbol? ResolveSectionGroup(string path)
        {
            var prefix = _sectionSchemas.Keys
                .Where(key => key.StartsWith(path + "/", StringComparison.Ordinal))
                .OrderBy(key => key, StringComparer.Ordinal)
                .FirstOrDefault();
            if (prefix == null)
            {
                return null;
            }

            var section = _sectionSchemas[prefix];
            return new LanguageServerSymbol(
                LanguageServerSymbolKind.SectionGroup,
                path.Split('/').Last(),
                path,
                null,
                null,
                null,
                false,
                [],
                NormalizeFilePath(section.FileName),
                GetLineNumber(section.SourceElement));
        }

        private ResolvedSchemaNode? ResolveNode(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var sectionPath = _sectionSchemas.Keys
                .Where(key => path == key || path.StartsWith(key + "/", StringComparison.Ordinal))
                .OrderByDescending(key => key.Length)
                .FirstOrDefault();
            if (sectionPath == null)
            {
                return null;
            }

            var section = _sectionSchemas[sectionPath];
            var current = new ResolvedSchemaNode(
                section,
                section.Root,
                LanguageServerSymbolKind.Section,
                sectionPath.Split('/').Last(),
                sectionPath,
                section.SourceElement,
                section.FileName);

            if (path == sectionPath)
            {
                return current;
            }

            var remainingParts = path[(sectionPath.Length + 1)..].Split('/', StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in remainingParts)
            {
                if (current.Schema.CollectionSchema?.GetElementSchema(part) is ConfigurationElementSchema collectionElement)
                {
                    current = new ResolvedSchemaNode(
                        section,
                        collectionElement,
                        LanguageServerSymbolKind.CollectionItem,
                        part,
                        $"{current.Path}/{part}",
                        current.Schema.CollectionSchema.SourceElement ?? collectionElement.SourceElement,
                        collectionElement.FileName);
                    continue;
                }

                var child = current.Schema.ChildElementSchemas[part];
                if (child == null)
                {
                    return null;
                }

                current = new ResolvedSchemaNode(
                    section,
                    child,
                    LanguageServerSymbolKind.Element,
                    part,
                    $"{current.Path}/{part}",
                    child.SourceElement,
                    child.FileName);
            }

            return current;
        }

        private static LanguageServerSymbol CreateElementSymbol(ResolvedSchemaNode node)
        {
            return new LanguageServerSymbol(
                node.Kind,
                node.Name,
                node.Path,
                GetParentPath(node.Path),
                null,
                null,
                false,
                [],
                NormalizeFilePath(node.FileName),
                GetLineNumber(node.SourceElement));
        }

        private static LanguageServerSymbol CreateAttributeSymbol(ResolvedSchemaNode node, ConfigurationAttributeSchema attribute)
        {
            return new LanguageServerSymbol(
                LanguageServerSymbolKind.Attribute,
                attribute.Name,
                $"{node.Path}@{attribute.Name}",
                node.Path,
                attribute.Type,
                attribute.Format(attribute.DefaultValue),
                attribute.IsRequired,
                attribute.GetEnumValues().Select(item => item.Name).ToList(),
                NormalizeFilePath(node.FileName),
                GetLineNumber(attribute.SourceElement));
        }

        private static string NormalizePath(string elementPath)
        {
            if (string.IsNullOrEmpty(elementPath))
            {
                return string.Empty;
            }

            var normalized = elementPath.Replace('\\', '/').Trim('/');
            if (normalized.StartsWith("configuration/", StringComparison.Ordinal))
            {
                normalized = normalized["configuration/".Length..];
            }

            return normalized;
        }

        private static string? GetParentPath(string path)
        {
            var index = path.LastIndexOf('/');
            return index <= 0 ? null : path[..index];
        }

        private static string? NormalizeFilePath(string fileName)
        {
            return string.IsNullOrWhiteSpace(fileName) ? null : Path.GetFullPath(fileName);
        }

        private static int GetLineNumber(XElement? element)
        {
            return element is IXmlLineInfo lineInfo && lineInfo.HasLineInfo()
                ? Math.Max(lineInfo.LineNumber, 1)
                : 1;
        }

        private sealed record ResolvedSchemaNode(
            SectionSchema Section,
            ConfigurationElementSchema Schema,
            LanguageServerSymbolKind Kind,
            string Name,
            string Path,
            XElement? SourceElement,
            string FileName);
    }
}

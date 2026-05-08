using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Web.Administration;

namespace IIS.LanguageServer.Schema;

public class LanguageServerSchemaService
{
    private readonly Dictionary<string, SectionSchema> _schemas = new(StringComparer.OrdinalIgnoreCase);

    public LanguageServerSchemaService(IEnumerable<string> schemaFiles)
    {
        LoadFromFiles(schemaFiles);
        Console.Error.WriteLine($"[IIS LS] Loaded {_schemas.Count} section schemas");
    }

    public LanguageServerSchemaService(IEnumerable<(string Name, string Content)> embeddedSchemas)
    {
        foreach (var (name, content) in embeddedSchemas)
        {
            try
            {
                var doc = XDocument.Parse(content, LoadOptions.SetLineInfo);
                LoadSchema(doc, name);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[IIS LS] Failed to load embedded schema {name}: {ex.Message}");
            }
        }

        Console.Error.WriteLine($"[IIS LS] Loaded {_schemas.Count} section schemas from embedded resources");
    }

    private void LoadFromFiles(IEnumerable<string> schemaFiles)
    {
        foreach (var file in schemaFiles)
        {
            try
            {
                var doc = XDocument.Load(file, LoadOptions.SetLineInfo);
                LoadSchema(doc, file);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[IIS LS] Failed to load schema {file}: {ex.Message}");
            }
        }
    }

    private void LoadSchema(XDocument document, string fileName)
    {
        if (document.Root == null)
            return;

        foreach (var node in document.Root.Nodes())
        {
            if (node is not XElement element)
                continue;
            if (element.Name.LocalName != "sectionSchema")
                continue;

            var name = element.Attribute("name")?.Value;
            if (string.IsNullOrEmpty(name))
                continue;

            if (!_schemas.TryGetValue(name, out var found))
            {
                found = new SectionSchema(name, element, fileName);
                _schemas[name] = found;
            }

            found.ParseSectionSchema(element, null, fileName);
        }
    }

    // Strip the "configuration/" prefix that XmlPositionAnalyzer adds
    private static string NormalizePath(string elementPath)
    {
        if (string.Equals(elementPath, "configuration", StringComparison.OrdinalIgnoreCase))
            return string.Empty;
        if (elementPath.StartsWith("configuration/", StringComparison.OrdinalIgnoreCase))
            return elementPath["configuration/".Length..];
        return elementPath;
    }

    private ConfigurationElementSchema? FindElementSchema(string schemaPath)
    {
        if (string.IsNullOrEmpty(schemaPath))
            return null;

        foreach (var section in _schemas.Values)
        {
            var result = section.Root.FindSchema(schemaPath);
            if (result != null)
                return result;
        }

        return null;
    }

    public IEnumerable<string> GetChildElementNames(string elementPath)
    {
        var schemaPath = NormalizePath(elementPath);

        // At the "configuration" root level, return first-level section group segment names
        if (string.IsNullOrEmpty(schemaPath))
        {
            return _schemas.Keys
                .Select(k => k.Split('/')[0])
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        }

        var schema = FindElementSchema(schemaPath);
        if (schema != null)
        {
            var names = schema.ChildElementSchemas
                .Select(c => c.Name!)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList();

            if (schema.CollectionSchema != null)
            {
                var coll = schema.CollectionSchema;
                foreach (var addName in coll.AddElementNames.Split(','))
                {
                    var trimmed = addName.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                        names.Add(trimmed);
                }

                if (!string.IsNullOrEmpty(coll.RemoveElementName))
                    names.Add(coll.RemoveElementName);
                if (!string.IsNullOrEmpty(coll.ClearElementName))
                    names.Add(coll.ClearElementName);
            }

            return names.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
        }

        // It might be a section group path — return immediate child section names
        var prefix = schemaPath + "/";
        return _schemas.Keys
            .Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .Select(k => k[prefix.Length..].Split('/')[0])
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<string> GetAttributeNames(string elementPath)
    {
        var schema = FindElementSchema(NormalizePath(elementPath));
        if (schema == null)
            return [];

        return schema.AttributeSchemas
            .Select(a => a.Name!)
            .Where(n => !string.IsNullOrEmpty(n))
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase);
    }

    public string? GetAttributeType(string elementPath, string attributeName)
    {
        var schema = FindElementSchema(NormalizePath(elementPath));
        return schema?.AttributeSchemas[attributeName]?.Type;
    }

    public IEnumerable<string> GetAttributeValues(string elementPath, string attributeName)
    {
        var schema = FindElementSchema(NormalizePath(elementPath));
        if (schema == null)
            return [];

        var attrSchema = schema.AttributeSchemas[attributeName];
        if (attrSchema == null)
            return [];

        if (attrSchema.Type is not ("enum" or "flags"))
            return [];

        return attrSchema.GetEnumValues()
            .Cast<ConfigurationEnumValue>()
            .Select(e => e.Name!)
            .Where(n => !string.IsNullOrEmpty(n));
    }

    public LanguageServerSymbol? ResolveElement(string elementPath)
    {
        var schemaPath = NormalizePath(elementPath);

        if (string.IsNullOrEmpty(schemaPath))
            return null;

        // Try direct element schema
        var schema = FindElementSchema(schemaPath);
        if (schema != null)
        {
            // Check if it's the root of a section
            if (_schemas.TryGetValue(schemaPath, out var section))
            {
                return new LanguageServerSymbol(
                    Kind: LanguageServerSymbolKind.Section,
                    Name: section.Name,
                    Path: schemaPath,
                    ParentPath: null,
                    Type: null,
                    DefaultValue: null,
                    Required: false,
                    EnumValues: [],
                    FilePath: schema.FileName,
                    LineNumber: schema.LineNumber);
            }

            // Collection item (add/remove/clear)
            var kind = IsCollectionItem(schemaPath)
                ? LanguageServerSymbolKind.CollectionItem
                : LanguageServerSymbolKind.Element;

            return new LanguageServerSymbol(
                Kind: kind,
                Name: schema.Name ?? schemaPath.Split('/')[^1],
                Path: schemaPath,
                ParentPath: GetParentPath(schemaPath),
                Type: null,
                DefaultValue: null,
                Required: false,
                EnumValues: [],
                FilePath: schema.FileName,
                LineNumber: schema.LineNumber);
        }

        // Section group (no direct schema, but has children)
        var prefix = schemaPath + "/";
        if (_schemas.Keys.Any(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
        {
            return new LanguageServerSymbol(
                Kind: LanguageServerSymbolKind.SectionGroup,
                Name: schemaPath.Split('/')[^1],
                Path: schemaPath,
                ParentPath: GetParentPath(schemaPath),
                Type: null,
                DefaultValue: null,
                Required: false,
                EnumValues: [],
                FilePath: null,
                LineNumber: 0);
        }

        return null;
    }

    public LanguageServerSymbol? ResolveAttribute(string elementPath, string attributeName)
    {
        var schema = FindElementSchema(NormalizePath(elementPath));
        if (schema == null)
            return null;

        var attrSchema = schema.AttributeSchemas[attributeName];
        if (attrSchema == null)
            return null;

        var enumValues = attrSchema.Type is "enum" or "flags"
            ? attrSchema.GetEnumValues()
                .Cast<ConfigurationEnumValue>()
                .Select(e => e.Name!)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList()
            : (IReadOnlyList<string>)[];

        var defaultValue = attrSchema.DefaultValue?.ToString();

        return new LanguageServerSymbol(
            Kind: LanguageServerSymbolKind.Attribute,
            Name: attrSchema.Name!,
            Path: NormalizePath(elementPath) + "/@" + attrSchema.Name,
            ParentPath: NormalizePath(elementPath),
            Type: attrSchema.Type,
            DefaultValue: defaultValue,
            Required: attrSchema.IsRequired,
            EnumValues: enumValues,
            FilePath: schema.FileName,
            LineNumber: attrSchema.LineNumber);
    }

    public LanguageServerSymbol? ResolveAttributeValue(string elementPath, string attributeName, string? value)
    {
        var schema = FindElementSchema(NormalizePath(elementPath));
        if (schema == null)
            return null;

        var attrSchema = schema.AttributeSchemas[attributeName];
        if (attrSchema == null || attrSchema.Type is not ("enum" or "flags"))
            return null;

        if (value == null)
            return null;

        var enumVal = attrSchema.GetEnumValues()
            .Cast<ConfigurationEnumValue>()
            .FirstOrDefault(e => string.Equals(e.Name, value, StringComparison.OrdinalIgnoreCase));

        if (enumVal == null)
            return null;

        return new LanguageServerSymbol(
            Kind: LanguageServerSymbolKind.EnumValue,
            Name: enumVal.Name!,
            Path: NormalizePath(elementPath) + "/@" + attributeName + "=" + enumVal.Name,
            ParentPath: NormalizePath(elementPath) + "/@" + attributeName,
            Type: attrSchema.Type,
            DefaultValue: null,
            Required: false,
            EnumValues: [],
            FilePath: schema.FileName,
            LineNumber: attrSchema.LineNumber);
    }

    private static string? GetParentPath(string path)
    {
        var lastSlash = path.LastIndexOf('/');
        return lastSlash > 0 ? path[..lastSlash] : null;
    }

    private static bool IsCollectionItem(string schemaPath)
    {
        var last = schemaPath.Split('/')[^1];
        return last is "add" or "remove" or "clear";
    }
}

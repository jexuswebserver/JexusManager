using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IIS.LanguageServer.Schema;

public class SchemaCache
{
    private readonly Dictionary<string, XElement> _schemas;

    public SchemaCache()
    {
        _schemas = SchemaLoader.LoadAllSchemas();
        Console.Error.WriteLine($"Loaded {_schemas.Count} schema definitions");
    }

    public XElement? GetSectionSchema(string sectionName)
    {
        _schemas.TryGetValue(sectionName, out var schema);
        return schema;
    }

    public List<string> GetAvailableSchemas()
    {
        return _schemas.Keys.ToList();
    }

    public List<string> GetChildElementNames(string elementPath)
    {
        var elements = new List<string>();
        var parts = elementPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            return elements;
        }

        var section = GetSectionSchema(parts[0]);
        if (section == null)
        {
            return elements;
        }

        var currentElement = section;
        for (int i = 1; i < parts.Length; i++)
        {
            var child = currentElement.Elements("element").FirstOrDefault(e => e.Attribute("name")?.Value == parts[i]);
            if (child == null)
            {
                return elements;
            }

            currentElement = child;
        }

        var childElements = currentElement.Elements("element")
            .Select(e => e.Attribute("name")?.Value)
            .Where(n => !string.IsNullOrEmpty(n))
            .Cast<string>()
            .ToList();

        return childElements;
    }

    public List<string> GetAttributeNames(string elementPath)
    {
        var attributes = new List<string>();
        var parts = elementPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            return attributes;
        }

        var section = GetSectionSchema(parts[0]);
        if (section == null)
        {
            return attributes;
        }

        var currentElement = section;
        for (int i = 1; i < parts.Length; i++)
        {
            var child = currentElement.Elements("element").FirstOrDefault(e => e.Attribute("name")?.Value == parts[i]);
            if (child == null)
            {
                return attributes;
            }

            currentElement = child;
        }

        var attrs = currentElement.Elements("attribute")
            .Select(a => a.Attribute("name")?.Value)
            .Where(n => !string.IsNullOrEmpty(n))
            .Cast<string>()
            .ToList();

        return attrs;
    }

    public string? GetAttributeType(string elementPath, string attributeName)
    {
        var parts = elementPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return null;
        }

        var section = GetSectionSchema(parts[0]);
        if (section == null)
        {
            return null;
        }

        var currentElement = section;
        for (int i = 1; i < parts.Length; i++)
        {
            var child = currentElement.Elements("element").FirstOrDefault(e => e.Attribute("name")?.Value == parts[i]);
            if (child == null)
            {
                return null;
            }

            currentElement = child;
        }

        var attr = currentElement.Elements("attribute").FirstOrDefault(a => a.Attribute("name")?.Value == attributeName);
        return attr?.Attribute("type")?.Value;
    }
}

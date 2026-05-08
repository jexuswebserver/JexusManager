using System.Collections.Generic;
using Microsoft.Web.Administration;

namespace IIS.LanguageServer.Schema;

public class SchemaCache
{
    private readonly LanguageServerSchemaService _schemaService;

    public SchemaCache()
    {
        var diskFiles = SchemaLoader.FindSchemaFiles();
        _schemaService = diskFiles.Count > 0
            ? new LanguageServerSchemaService(diskFiles)
            : new LanguageServerSchemaService(SchemaLoader.GetEmbeddedSchemaFiles());
    }

    public SchemaCache(IEnumerable<string> schemaFiles)
        : this(new LanguageServerSchemaService(schemaFiles))
    {
    }

    internal SchemaCache(LanguageServerSchemaService schemaService)
    {
        _schemaService = schemaService;
    }

    public List<string> GetChildElementNames(string elementPath)
    {
        return [.. _schemaService.GetChildElementNames(elementPath)];
    }

    public List<string> GetAttributeNames(string elementPath)
    {
        return [.. _schemaService.GetAttributeNames(elementPath)];
    }

    public string? GetAttributeType(string elementPath, string attributeName)
    {
        return _schemaService.GetAttributeType(elementPath, attributeName);
    }

    public List<string> GetAttributeValues(string elementPath, string attributeName)
    {
        return [.. _schemaService.GetAttributeValues(elementPath, attributeName)];
    }

    internal LanguageServerSymbol? GetElementSymbol(string elementPath)
    {
        return _schemaService.ResolveElement(elementPath);
    }

    internal LanguageServerSymbol? GetAttributeSymbol(string elementPath, string attributeName)
    {
        return _schemaService.ResolveAttribute(elementPath, attributeName);
    }

    internal LanguageServerSymbol? GetAttributeValueSymbol(string elementPath, string attributeName, string? attributeValue)
    {
        return _schemaService.ResolveAttributeValue(elementPath, attributeName, attributeValue);
    }
}

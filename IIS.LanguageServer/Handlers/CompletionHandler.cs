using System;
using System.Collections.Generic;
using IIS.LanguageServer.Schema;

namespace IIS.LanguageServer.Handlers;

public class CompletionHandler
{
    private readonly SchemaCache _schemaCache;

    public CompletionHandler(SchemaCache schemaCache)
    {
        _schemaCache = schemaCache;
    }

    public List<CompletionItem> GetCompletions(string elementPath, string currentAttributeName)
    {
        var items = new List<CompletionItem>();

        // Suggest child elements
        var childElements = _schemaCache.GetChildElementNames(elementPath);
        foreach (var child in childElements)
        {
            items.Add(new CompletionItem
            {
                Label = child,
                Kind = "Struct",
                Detail = "Element",
                InsertText = child
            });
        }

        // Suggest attribute names
        var attributes = _schemaCache.GetAttributeNames(elementPath);
        foreach (var attr in attributes)
        {
            items.Add(new CompletionItem
            {
                Label = attr,
                Kind = "Property",
                Detail = $"Type: {_schemaCache.GetAttributeType(elementPath, attr) ?? "string"}",
                InsertText = $"{attr}=\"\""
            });
        }

        return items;
    }
}

public class CompletionItem
{
    public string Label { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string InsertText { get; set; } = string.Empty;
}

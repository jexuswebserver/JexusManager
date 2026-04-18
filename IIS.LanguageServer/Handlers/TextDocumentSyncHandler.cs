using System;
using System.Collections.Generic;
using IIS.LanguageServer.Schema;

namespace IIS.LanguageServer.Handlers;

public class TextDocumentSyncHandler
{
    private readonly Dictionary<string, string> _openDocuments = new();
    private readonly DiagnosticsHandler _diagnosticsHandler;

    public TextDocumentSyncHandler(SchemaCache schemaCache)
    {
        _diagnosticsHandler = new DiagnosticsHandler(schemaCache);
    }

    public void HandleDidOpen(string uri, string text)
    {
        _openDocuments[uri] = text;
        _diagnosticsHandler.ValidateDocument(text, uri);
    }

    public void HandleDidChange(string uri, string text)
    {
        _openDocuments[uri] = text;
        _diagnosticsHandler.ValidateDocument(text, uri);
    }

    public void HandleDidClose(string uri)
    {
        _openDocuments.Remove(uri);
    }

    public string? GetDocumentContent(string uri)
    {
        _openDocuments.TryGetValue(uri, out var content);
        return content;
    }
}

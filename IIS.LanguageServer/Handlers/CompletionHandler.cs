using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Completion;
using EmmyLua.LanguageServer.Framework.Protocol.Model;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Kind;
using EmmyLua.LanguageServer.Framework.Protocol.Model.TextEdit;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Union;
using EmmyLua.LanguageServer.Framework.Server.Handler;
using IIS.LanguageServer.Language;
using IIS.LanguageServer.Schema;

namespace IIS.LanguageServer.Handlers;

public class IISCompletionHandler : CompletionHandlerBase
{
    private readonly SchemaCache _schemaCache;
    private readonly TextDocumentSyncHandler _textDocumentSyncHandler;

    public IISCompletionHandler(SchemaCache schemaCache, TextDocumentSyncHandler textDocumentSyncHandler)
    {
        _schemaCache = schemaCache;
        _textDocumentSyncHandler = textDocumentSyncHandler;
    }

    protected override Task<CompletionResponse?> Handle(CompletionParams request, CancellationToken token)
    {
        var uri = request.TextDocument.Uri.Uri.AbsoluteUri;
        var documentContent = _textDocumentSyncHandler.GetDocumentContent(uri);

        return Task.FromResult(GetCompletionResponse(documentContent, request.Position.Line, request.Position.Character));
    }

    internal CompletionResponse? GetCompletionResponse(string? documentContent, int line, int character)
    {
        if (string.IsNullOrEmpty(documentContent))
            return null;

        var cursor = XmlCursorLocator.Locate(documentContent, line, character);
        var items = GetCompletions(cursor);
        return new CompletionResponse(items);
    }

    protected override Task<CompletionItem> Resolve(CompletionItem item, CancellationToken token)
    {
        return Task.FromResult(item);
    }

    public override void RegisterCapability(ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities)
    {
        serverCapabilities.CompletionProvider = new CompletionOptions
        {
            TriggerCharacters = new List<string> { "<", " ", "=", "\"", "'" },
            ResolveProvider = false
        };
    }

    private List<CompletionItem> GetCompletions(XmlCursorContext cursor)
    {
        var items = new List<CompletionItem>();

        switch (cursor.Context.Type)
        {
            case ContextType.AttributeValue when !string.IsNullOrEmpty(cursor.Context.CurrentAttributeName):
                AddAttributeValueCompletions(cursor, items);
                break;

            case ContextType.AttributeName:
                AddAttributeNameCompletions(cursor, items);
                break;

            case ContextType.ElementTag when cursor.TokenKind == XmlTokenKind.ElementName:
                // Completing the tag name itself — suggest child element names of the parent
                var parentPath = GetParentPath(cursor.Context.ElementPath);
                AddChildElementCompletions(parentPath, cursor, items);
                break;

            case ContextType.ElementContent:
                // Between tags — suggest child element names
                AddChildElementCompletions(cursor.Context.ElementPath, cursor, items);
                break;

            default:
                // Inside a tag but ambiguous — suggest attributes (most useful default)
                if (!string.IsNullOrEmpty(cursor.Context.ElementPath))
                    AddAttributeNameCompletions(cursor, items);
                break;
        }

        return items;
    }

    private void AddAttributeValueCompletions(XmlCursorContext cursor, List<CompletionItem> items)
    {
        var values = _schemaCache.GetAttributeValues(cursor.Context.ElementPath, cursor.Context.CurrentAttributeName!);
        foreach (var value in values)
        {
            items.Add(new CompletionItem
            {
                Label = value,
                Kind = CompletionItemKind.Value,
                Detail = "Value",
                FilterText = value,
                TextEdit = new TextEditOrInsertReplaceEdit(new TextEdit
                {
                    Range = DocumentRange.From(
                        new Position(cursor.Line, cursor.StartCharacter),
                        new Position(cursor.Line, cursor.EndCharacter)),
                    NewText = value
                })
            });
        }
    }

    private void AddAttributeNameCompletions(XmlCursorContext cursor, List<CompletionItem> items)
    {
        var attributes = _schemaCache.GetAttributeNames(cursor.Context.ElementPath);
        foreach (var attr in attributes)
        {
            items.Add(new CompletionItem
            {
                Label = attr,
                Kind = CompletionItemKind.Property,
                Detail = $"Type: {_schemaCache.GetAttributeType(cursor.Context.ElementPath, attr) ?? "string"}",
                TextEdit = new TextEditOrInsertReplaceEdit(new TextEdit
                {
                    Range = DocumentRange.From(
                        new Position(cursor.Line, cursor.StartCharacter),
                        new Position(cursor.Line, cursor.EndCharacter)),
                    NewText = $"{attr}=\"\""
                })
            });
        }
    }

    private void AddChildElementCompletions(string elementPath, XmlCursorContext cursor, List<CompletionItem> items)
    {
        var childElements = _schemaCache.GetChildElementNames(elementPath);
        foreach (var child in childElements)
        {
            items.Add(new CompletionItem
            {
                Label = child,
                Kind = CompletionItemKind.Struct,
                Detail = "Element",
                InsertText = child
            });
        }
    }

    private static string GetParentPath(string elementPath)
    {
        var lastSlash = elementPath.LastIndexOf('/');
        return lastSlash > 0 ? elementPath[..lastSlash] : elementPath;
    }
}

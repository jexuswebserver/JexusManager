using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Definition;
using EmmyLua.LanguageServer.Framework.Protocol.Model;
using EmmyLua.LanguageServer.Framework.Server.Handler;
using IIS.LanguageServer.Language;
using IIS.LanguageServer.Schema;
using Microsoft.Web.Administration;

namespace IIS.LanguageServer.Handlers;

public class IISDefinitionHandler : DefinitionHandlerBase
{
    private readonly SchemaCache _schemaCache;
    private readonly TextDocumentSyncHandler _textDocumentSyncHandler;

    public IISDefinitionHandler(SchemaCache schemaCache, TextDocumentSyncHandler textDocumentSyncHandler)
    {
        _schemaCache = schemaCache;
        _textDocumentSyncHandler = textDocumentSyncHandler;
    }

    protected override Task<DefinitionResponse?> Handle(DefinitionParams request, CancellationToken cancellationToken)
    {
        var uri = request.TextDocument.Uri.Uri.AbsoluteUri;
        var documentContent = _textDocumentSyncHandler.GetDocumentContent(uri);
        if (string.IsNullOrEmpty(documentContent))
        {
            return Task.FromResult<DefinitionResponse?>(null);
        }

        var cursor = XmlCursorLocator.Locate(documentContent, request.Position.Line, request.Position.Character);
        var elementPath = ResolveElementPath(cursor);
        LanguageServerSymbol? symbol = cursor.TokenKind switch
        {
            XmlTokenKind.ElementName when !string.IsNullOrEmpty(elementPath) =>
                _schemaCache.GetElementSymbol(elementPath),
            XmlTokenKind.AttributeName when !string.IsNullOrEmpty(cursor.TokenText) =>
                _schemaCache.GetAttributeSymbol(cursor.Context.ElementPath, cursor.TokenText),
            XmlTokenKind.AttributeValue when !string.IsNullOrEmpty(cursor.Context.CurrentAttributeName) =>
                _schemaCache.GetAttributeValueSymbol(
                    cursor.Context.ElementPath,
                    cursor.Context.CurrentAttributeName,
                    cursor.TokenText ?? cursor.Context.CurrentAttributeValue),
            _ when !string.IsNullOrEmpty(elementPath) =>
                _schemaCache.GetElementSymbol(elementPath),
            _ => null
        };

        if (symbol?.FilePath == null)
        {
            return Task.FromResult<DefinitionResponse?>(null);
        }

        var lineNumber = Math.Max(symbol.LineNumber - 1, 0);
        var lineLength = 1;
        if (File.Exists(symbol.FilePath))
        {
            var lines = File.ReadAllLines(symbol.FilePath);
            if (lineNumber < lines.Length)
            {
                lineLength = Math.Max(lines[lineNumber].Length, 1);
            }
        }

        var location = new EmmyLua.LanguageServer.Framework.Protocol.Model.Location(
            new Uri(symbol.FilePath),
            DocumentRange.From(
                new Position(lineNumber, 0),
                new Position(lineNumber, lineLength)));

        return Task.FromResult<DefinitionResponse?>(new DefinitionResponse(location));
    }

    public override void RegisterCapability(ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities)
    {
        serverCapabilities.DefinitionProvider = true;
    }

    private static string ResolveElementPath(XmlCursorContext cursor)
    {
        if (string.IsNullOrEmpty(cursor.Context.ElementPath))
        {
            return cursor.TokenKind == XmlTokenKind.ElementName
                ? cursor.TokenText ?? string.Empty
                : string.Empty;
        }

        if (cursor.TokenKind != XmlTokenKind.ElementName || string.IsNullOrEmpty(cursor.TokenText))
        {
            return cursor.Context.ElementPath;
        }

        var segments = cursor.Context.ElementPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            return cursor.TokenText;
        }

        if (!string.Equals(segments[^1], cursor.TokenText, StringComparison.Ordinal))
        {
            segments[^1] = cursor.TokenText;
        }

        return string.Join("/", segments);
    }
}

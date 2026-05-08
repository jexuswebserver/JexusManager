using System;
using System.Threading;
using System.Threading.Tasks;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Hover;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Markup;
using EmmyLua.LanguageServer.Framework.Server.Handler;
using IIS.LanguageServer.Language;
using IIS.LanguageServer.Schema;
using Microsoft.Web.Administration;
using System.IO;

namespace IIS.LanguageServer.Handlers;

public class IISHoverHandler : HoverHandlerBase
{
    private readonly SchemaCache _schemaCache;
    private readonly TextDocumentSyncHandler _textDocumentSyncHandler;

    public IISHoverHandler(SchemaCache schemaCache, TextDocumentSyncHandler textDocumentSyncHandler)
    {
        _schemaCache = schemaCache;
        _textDocumentSyncHandler = textDocumentSyncHandler;
    }

    protected override Task<HoverResponse?> Handle(HoverParams request, CancellationToken token)
    {
        var uri = request.TextDocument.Uri.Uri.AbsoluteUri;
        var documentContent = _textDocumentSyncHandler.GetDocumentContent(uri);

        return Task.FromResult(GetHoverResponse(documentContent, request.Position.Line, request.Position.Character));
    }

    internal HoverResponse? GetHoverResponse(string? documentContent, int line, int character)
    {

        if (string.IsNullOrEmpty(documentContent))
        {
            return null;
        }

        var cursor = XmlCursorLocator.Locate(documentContent, line, character);
        var hoverInfo = GetHoverInfo(cursor);
        if (string.IsNullOrEmpty(hoverInfo))
        {
            return null;
        }

        return new HoverResponse
        {
            Contents = new MarkupContent
            {
                Kind = MarkupKind.Markdown,
                Value = hoverInfo
            }
        };
    }

    public override void RegisterCapability(ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities)
    {
        serverCapabilities.HoverProvider = true;
    }

    private string? GetHoverInfo(XmlCursorContext cursor)
    {
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

        if (symbol == null)
        {
            return null;
        }

        return FormatHover(symbol);
    }

    private static string FormatHover(LanguageServerSymbol symbol)
    {
        var title = symbol.Kind switch
        {
            LanguageServerSymbolKind.SectionGroup => $"**Section Group** `{symbol.Path}`",
            LanguageServerSymbolKind.Section => $"**Section** `{symbol.Path}`",
            LanguageServerSymbolKind.CollectionItem => $"**Tag** `{symbol.Name}`",
            LanguageServerSymbolKind.Element => $"**Tag** `{symbol.Name}`",
            LanguageServerSymbolKind.Attribute => $"**Attribute** `{symbol.Name}`",
            LanguageServerSymbolKind.EnumValue => $"**Value** `{symbol.Name}`",
            _ => $"**Schema Symbol** `{symbol.Name}`"
        };

        var lines = new System.Collections.Generic.List<string> { title };

        if (symbol.Kind is LanguageServerSymbolKind.CollectionItem or LanguageServerSymbolKind.Element or LanguageServerSymbolKind.Section)
        {
            lines.Add($"Path: `{symbol.Path}`");
        }

        if (!string.IsNullOrEmpty(symbol.ParentPath) &&
            (symbol.Kind is LanguageServerSymbolKind.Attribute or LanguageServerSymbolKind.EnumValue))
        {
            lines.Add($"Owner: `{symbol.ParentPath}`");
        }

        if (!string.IsNullOrEmpty(symbol.Type))
        {
            lines.Add($"Type: `{symbol.Type}`");
        }

        if (symbol.DefaultValue != null)
        {
            lines.Add($"Default: `{symbol.DefaultValue}`");
        }

        if (symbol.Required)
        {
            lines.Add("Required: `true`");
        }

        if (symbol.EnumValues.Count > 0 && symbol.Kind != LanguageServerSymbolKind.EnumValue)
        {
            lines.Add($"Allowed values: `{string.Join("`, `", symbol.EnumValues)}`");
        }

        if (!string.IsNullOrEmpty(symbol.FilePath))
        {
            var uri = new UriBuilder(new Uri(symbol.FilePath))
            {
                Fragment = $"{Math.Max(symbol.LineNumber, 1)},1"
            }.Uri.AbsoluteUri;
            var fileName = Path.GetFileName(symbol.FilePath);
            lines.Add($"Schema file: `{symbol.FilePath}`");
            lines.Add($"Open schema: [{fileName}:{symbol.LineNumber}]({uri})");
            lines.Add("Go to Definition jumps to the exact schema node.");
        }

        return string.Join("  \n", lines);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Client.ClientCapabilities;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server;
using EmmyLua.LanguageServer.Framework.Protocol.Capabilities.Server.Options;
using EmmyLua.LanguageServer.Framework.Protocol.Message;
using EmmyLua.LanguageServer.Framework.Protocol.Message.TextDocument;
using EmmyLua.LanguageServer.Framework.Protocol.Model;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Kind;
using EmmyLua.LanguageServer.Framework.Protocol.Model.TextEdit;
using EmmyLua.LanguageServer.Framework.Server;
using EmmyLua.LanguageServer.Framework.Server.Handler;
using IIS.LanguageServer.Schema;

namespace IIS.LanguageServer.Handlers;

public class TextDocumentSyncHandler : TextDocumentHandlerBase
{
    private readonly Dictionary<string, string> _openDocuments = new();
    private readonly DiagnosticsHandler _diagnosticsHandler;

    public TextDocumentSyncHandler(SchemaCache schemaCache)
    {
        _diagnosticsHandler = new DiagnosticsHandler(schemaCache);
    }

    protected override Task Handle(DidOpenTextDocumentParams request, CancellationToken token)
    {
        var uri = request.TextDocument.Uri.Uri.AbsoluteUri;
        var text = request.TextDocument.Text;
        _openDocuments[uri] = text;
        _diagnosticsHandler.ValidateDocument(text, uri);
        return Task.CompletedTask;
    }

    protected override Task Handle(DidChangeTextDocumentParams request, CancellationToken token)
    {
        var uri = request.TextDocument.Uri.Uri.AbsoluteUri;
        var text = _openDocuments.TryGetValue(uri, out var existingText) ? existingText : string.Empty;

        foreach (var change in request.ContentChanges)
        {
            if (change.Range == null)
            {
                text = change.Text;
            }
            else
            {
                text = ApplyTextChange(text, change.Range.Value, change.Text);
            }
        }

        _openDocuments[uri] = text;
        _diagnosticsHandler.ValidateDocument(text, uri);
        return Task.CompletedTask;
    }

    protected override Task Handle(DidCloseTextDocumentParams request, CancellationToken token)
    {
        _openDocuments.Remove(request.TextDocument.Uri.Uri.AbsoluteUri);
        return Task.CompletedTask;
    }

    protected override Task Handle(WillSaveTextDocumentParams request, CancellationToken token)
    {
        return Task.CompletedTask;
    }

    protected override Task<List<TextEdit>?> HandleRequest(WillSaveTextDocumentParams request, CancellationToken token)
    {
        return Task.FromResult<List<TextEdit>?>(null);
    }

    public override void RegisterCapability(ServerCapabilities serverCapabilities, ClientCapabilities clientCapabilities)
    {
        serverCapabilities.TextDocumentSync = new TextDocumentSyncOptions
        {
            Change = TextDocumentSyncKind.Incremental,
            OpenClose = true,
            WillSave = false,
            WillSaveWaitUntil = false
        };
    }

    public string? GetDocumentContent(string uri)
    {
        _openDocuments.TryGetValue(uri, out var content);
        return content;
    }

    public void SetServer(LanguageServer server)
    {
        _diagnosticsHandler.SetServer(server);
    }

    internal void OpenDocumentForTesting(string uri, string text)
    {
        _openDocuments[uri] = text;
    }

    private string ApplyTextChange(string text, DocumentRange range, string newText)
    {
        var lines = text.Split('\n');
        var startLine = range.Start.Line;
        var startChar = range.Start.Character;
        var endLine = range.End.Line;
        var endChar = range.End.Character;

        if (startLine >= lines.Length)
            return text + newText;

        var before = text[..GetOffset(lines, startLine, startChar)];
        var after = text[GetOffset(lines, endLine, endChar)..];
        return before + newText + after;
    }

    private int GetOffset(string[] lines, int line, int character)
    {
        var offset = 0;
        for (int i = 0; i < line && i < lines.Length; i++)
        {
            offset += lines[i].Length + 1;
        }
        offset += character;
        return Math.Min(offset, lines.Sum(l => l.Length) + lines.Length);
    }
}

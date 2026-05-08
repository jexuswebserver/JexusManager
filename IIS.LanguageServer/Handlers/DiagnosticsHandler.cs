using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Client.PublishDiagnostics;
using EmmyLua.LanguageServer.Framework.Protocol.Model;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Diagnostic;
using EmmyLua.LanguageServer.Framework.Server;
using IIS.LanguageServer.Schema;
using LspServer = EmmyLua.LanguageServer.Framework.Server.LanguageServer;

namespace IIS.LanguageServer.Handlers;

public class DiagnosticsHandler
{
    private readonly SchemaCache _schemaCache;
    private LspServer? _server;

    public DiagnosticsHandler(SchemaCache schemaCache)
    {
        _schemaCache = schemaCache;
    }

    public void SetServer(LspServer server)
    {
        _server = server;
    }

    public void ValidateDocument(string documentText, string uri)
    {
        if (_server == null)
            return;

        var diagnostics = new List<Diagnostic>();

        try
        {
            if (string.IsNullOrWhiteSpace(documentText))
            {
                Publish(uri, diagnostics);
                return;
            }

            XDocument doc;
            try
            {
                doc = XDocument.Parse(documentText, LoadOptions.SetLineInfo);
            }
            catch (XmlException ex)
            {
                diagnostics.Add(CreateDiagnostic(
                    ex.LineNumber - 1, ex.LinePosition - 1, ex.LinePosition,
                    DiagnosticSeverity.Error, $"XML parse error: {ex.Message}", "xml"));
                Publish(uri, diagnostics);
                return;
            }

            var root = doc.Root;
            if (root == null || root.Name.LocalName != "configuration")
            {
                Publish(uri, diagnostics);
                return;
            }

            ValidateElement(root, "configuration", diagnostics);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[IIS LS] Diagnostics error for {uri}: {ex.Message}");
        }

        Publish(uri, diagnostics);
    }

    private void ValidateElement(XElement element, string elementPath, List<Diagnostic> diagnostics)
    {
        foreach (var child in element.Elements())
        {
            var childPath = $"{elementPath}/{child.Name.LocalName}";
            ValidateAttributes(child, childPath, diagnostics);
            ValidateElement(child, childPath, diagnostics);
        }
    }

    private void ValidateAttributes(XElement element, string elementPath, List<Diagnostic> diagnostics)
    {
        var knownAttributes = _schemaCache.GetAttributeNames(elementPath);
        if (knownAttributes.Count == 0)
            return; // No schema for this element path — skip

        // If the schema declares allowUnrecognizedAttributes, skip unknown-attribute checks
        var allowUnrecognized = _schemaCache.GetAllowUnrecognizedAttributes(elementPath);

        var lineInfo = (IXmlLineInfo)element;
        var line = lineInfo.HasLineInfo() ? lineInfo.LineNumber - 1 : 0;
        var col = lineInfo.HasLineInfo() ? lineInfo.LinePosition - 1 : 0;

        // Check for unknown attributes
        foreach (var attr in element.Attributes())
        {
            var attrName = attr.Name.LocalName;
            if (attrName.StartsWith("xmlns", StringComparison.Ordinal))
                continue;

            if (!knownAttributes.Contains(attrName, StringComparer.OrdinalIgnoreCase))
            {
                if (allowUnrecognized)
                    continue; // Schema says extra attributes are allowed here

                var attrLineInfo = (IXmlLineInfo)attr;
                var attrLine = attrLineInfo.HasLineInfo() ? attrLineInfo.LineNumber - 1 : line;
                var attrCol = attrLineInfo.HasLineInfo() ? attrLineInfo.LinePosition - 1 : col;

                diagnostics.Add(CreateDiagnostic(
                    attrLine, attrCol, attrCol + attrName.Length,
                    DiagnosticSeverity.Warning,
                    $"Unknown attribute '{attrName}' on element '{element.Name.LocalName}'.",
                    "iis-schema"));
            }
            else
            {
                // Validate enum/flags values
                var values = _schemaCache.GetAttributeValues(elementPath, attrName);
                if (values.Count > 0 && !string.IsNullOrEmpty(attr.Value))
                {
                    var attrType = _schemaCache.GetAttributeType(elementPath, attrName);
                    if (attrType == "enum" && !values.Contains(attr.Value, StringComparer.OrdinalIgnoreCase))
                    {
                        var attrLineInfo = (IXmlLineInfo)attr;
                        var attrLine = attrLineInfo.HasLineInfo() ? attrLineInfo.LineNumber - 1 : line;
                        var attrCol = attrLineInfo.HasLineInfo() ? attrLineInfo.LinePosition - 1 : col;

                        diagnostics.Add(CreateDiagnostic(
                            attrLine, attrCol, attrCol + attr.Value.Length,
                            DiagnosticSeverity.Error,
                            $"Invalid value '{attr.Value}' for attribute '{attrName}'. Allowed: {string.Join(", ", values)}.",
                            "iis-schema"));
                    }
                }
            }
        }

        // Check for missing required attributes
        foreach (var requiredAttr in GetRequiredAttributes(elementPath))
        {
            if (element.Attribute(requiredAttr) == null)
            {
                diagnostics.Add(CreateDiagnostic(
                    line, col, col + element.Name.LocalName.Length,
                    DiagnosticSeverity.Error,
                    $"Required attribute '{requiredAttr}' is missing on element '{element.Name.LocalName}'.",
                    "iis-schema"));
            }
        }
    }

    private List<string> GetRequiredAttributes(string elementPath)
    {
        // Use SchemaCache.GetAttributeNames to find attributes, then check type for required
        // We need to check each attr for IsRequired — delegate to SchemaCache symbols
        var required = new List<string>();
        foreach (var attrName in _schemaCache.GetAttributeNames(elementPath))
        {
            var symbol = _schemaCache.GetAttributeSymbol(elementPath, attrName);
            if (symbol?.Required == true)
                required.Add(attrName);
        }

        return required;
    }

    private static Diagnostic CreateDiagnostic(int line, int startCol, int endCol,
        DiagnosticSeverity severity, string message, string source)
    {
        return new Diagnostic
        {
            Range = DocumentRange.From(
                new Position(line, startCol),
                new Position(line, endCol)),
            Severity = severity,
            Message = message,
            Source = source
        };
    }

    private void Publish(string uri, List<Diagnostic> diagnostics)
    {
        _ = _server?.Client.PublishDiagnostics(new PublishDiagnosticsParams
        {
            Uri = new DocumentUri(new Uri(uri)),
            Diagnostics = diagnostics
        });
    }
}

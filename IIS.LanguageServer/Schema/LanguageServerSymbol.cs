using System.Collections.Generic;

namespace IIS.LanguageServer.Schema;

public record LanguageServerSymbol(
    LanguageServerSymbolKind Kind,
    string Name,
    string Path,
    string? ParentPath,
    string? Type,
    string? DefaultValue,
    bool Required,
    IReadOnlyList<string> EnumValues,
    string? FilePath,
    int LineNumber
);

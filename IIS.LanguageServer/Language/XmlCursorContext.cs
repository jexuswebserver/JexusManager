using System;
using System.Text.RegularExpressions;

namespace IIS.LanguageServer.Language;

public enum XmlTokenKind
{
    None,
    ElementName,
    AttributeName,
    AttributeValue
}

public record XmlCursorContext(
    XmlContext Context,
    XmlTokenKind TokenKind,
    string? TokenText,
    int Line,
    int StartCharacter,
    int EndCharacter
);

public static class XmlCursorLocator
{
    private const string XmlNamePattern = @"[\w.-]+(?::[\w.-]+)?";

    public static XmlCursorContext Locate(string documentText, int line, int character)
    {
        var offset = GetOffset(documentText, line, character);
        var context = XmlPositionAnalyzer.GetContext(documentText, offset);

        var tagStart = documentText.LastIndexOf('<', Math.Max(0, offset - 1));
        if (tagStart < 0)
        {
            return new XmlCursorContext(context, XmlTokenKind.None, null, line, character, character);
        }

        var lastCloseBeforeOffset = documentText.LastIndexOf('>', Math.Max(0, offset - 1));
        if (lastCloseBeforeOffset > tagStart)
        {
            return new XmlCursorContext(context, XmlTokenKind.None, null, line, character, character);
        }

        var tagEnd = documentText.IndexOf('>', tagStart);
        if (tagEnd < 0)
        {
            tagEnd = documentText.Length;
        }

        var tagText = documentText[tagStart..tagEnd];
        var relativeOffset = Math.Clamp(offset - tagStart, 0, tagText.Length);

        var elementMatch = Regex.Match(tagText, $@"^</?({XmlNamePattern})");
        if (elementMatch.Success)
        {
            var start = tagStart + elementMatch.Groups[1].Index;
            var end = start + elementMatch.Groups[1].Length;
            if (offset >= start && offset <= end)
            {
                return new XmlCursorContext(
                    context,
                    XmlTokenKind.ElementName,
                    elementMatch.Groups[1].Value,
                    line,
                    GetCharacterOnLine(documentText, start),
                    GetCharacterOnLine(documentText, end));
            }
        }

        foreach (Match attributeMatch in Regex.Matches(tagText, $@"({XmlNamePattern})\s*=\s*([\""'])([^\""'>]*)(?:\2)?"))
        {
            var nameStart = tagStart + attributeMatch.Groups[1].Index;
            var nameEnd = nameStart + attributeMatch.Groups[1].Length;
            if (offset >= nameStart && offset <= nameEnd)
            {
                return new XmlCursorContext(
                    context,
                    XmlTokenKind.AttributeName,
                    attributeMatch.Groups[1].Value,
                    line,
                    GetCharacterOnLine(documentText, nameStart),
                    GetCharacterOnLine(documentText, nameEnd));
            }

            var valueStart = tagStart + attributeMatch.Groups[3].Index;
            var valueEnd = valueStart + attributeMatch.Groups[3].Length;
            if (offset >= valueStart && offset <= valueEnd)
            {
                return new XmlCursorContext(
                    context,
                    XmlTokenKind.AttributeValue,
                    attributeMatch.Groups[3].Value,
                    line,
                    GetCharacterOnLine(documentText, valueStart),
                    GetCharacterOnLine(documentText, valueEnd));
            }
        }

        var prefix = tagText[..relativeOffset];
        var partialAttribute = Regex.Match(prefix, $@"(?:^|\s)({XmlNamePattern})$");
        if (partialAttribute.Success && partialAttribute.Groups[1].Value != context.CurrentElementName)
        {
            var tokenStart = tagStart + partialAttribute.Groups[1].Index;
            var tokenEnd = tokenStart + partialAttribute.Groups[1].Length;
            return new XmlCursorContext(
                context,
                XmlTokenKind.AttributeName,
                partialAttribute.Groups[1].Value,
                line,
                GetCharacterOnLine(documentText, tokenStart),
                GetCharacterOnLine(documentText, tokenEnd));
        }

        return new XmlCursorContext(context, XmlTokenKind.None, null, line, character, character);
    }

    private static int GetOffset(string text, int line, int character)
    {
        var currentLine = 0;
        var currentCharacter = 0;

        for (var index = 0; index < text.Length; index++)
        {
            if (currentLine == line && currentCharacter == character)
            {
                return index;
            }

            if (text[index] == '\r')
            {
                continue;
            }

            if (text[index] == '\n')
            {
                currentLine++;
                currentCharacter = 0;
                continue;
            }

            if (currentLine == line)
            {
                currentCharacter++;
            }
        }

        return text.Length;
    }

    private static int GetCharacterOnLine(string text, int offset)
    {
        var lineStart = text.LastIndexOf('\n', Math.Max(0, offset - 1));
        if (lineStart < 0)
        {
            return offset;
        }

        return offset - lineStart - 1;
    }
}

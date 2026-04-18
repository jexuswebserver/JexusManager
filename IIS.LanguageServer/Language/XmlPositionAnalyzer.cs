using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IIS.LanguageServer.Language;

public record XmlContext(
    string ElementPath,
    string? CurrentElementName,
    string? CurrentAttributeName,
    string? CurrentAttributeValue,
    ContextType Type
);

public enum ContextType
{
    Unknown,
    ElementTag,
    AttributeName,
    AttributeValue,
    ElementContent
}

public class XmlPositionAnalyzer
{
    public static XmlContext GetContext(string documentText, int position)
    {
        if (position < 0 || position > documentText.Length)
        {
            return new XmlContext(string.Empty, null, null, null, ContextType.Unknown);
        }

        var upToPosition = documentText[..position];
        var elementPath = ExtractElementPath(upToPosition);
        var currentElementName = ExtractCurrentElementName(upToPosition);

        var attributeName = ExtractCurrentAttributeName(upToPosition);
        var attributeValue = ExtractCurrentAttributeValue(upToPosition);

        var contextType = DetermineContextType(upToPosition);

        return new XmlContext(elementPath, currentElementName, attributeName, attributeValue, contextType);
    }

    private static string ExtractElementPath(string text)
    {
        var tags = new List<string>();
        var pattern = @"<(/?)(\w+(?::\w+)?)";
        var matches = Regex.Matches(text, pattern);

        foreach (Match match in matches)
        {
            var isClosing = !string.IsNullOrEmpty(match.Groups[1].Value);
            var tagName = match.Groups[2].Value;

            if (isClosing)
            {
                if (tags.Count > 0 && tags[^1] == tagName)
                {
                    tags.RemoveAt(tags.Count - 1);
                }
            }
            else
            {
                tags.Add(tagName);
            }
        }

        return string.Join("/", tags);
    }

    private static string? ExtractCurrentElementName(string text)
    {
        var match = Regex.Match(text, @"<(\w+(?::\w+)?)(?:\s|>|/)");
        return match.Success ? match.Groups[1].Value : null;
    }

    private static string? ExtractCurrentAttributeName(string text)
    {
        var lastOpenTag = text.LastIndexOf('<');
        if (lastOpenTag == -1)
        {
            return null;
        }

        var tagContent = text[lastOpenTag..];

        // Check if we're inside an attribute
        if (tagContent.Contains('"') || tagContent.Contains('\''))
        {
            var match = Regex.Match(tagContent, @"(\w+)=""[^""]*$|(\w+)='[^']*$");
            if (match.Success)
            {
                return match.Groups[1].Value ?? match.Groups[2].Value;
            }
        }

        // Look for attribute name before cursor
        var attrMatch = Regex.Match(tagContent, @"(\w+)\s*=$");
        return attrMatch.Success ? attrMatch.Groups[1].Value : null;
    }

    private static string? ExtractCurrentAttributeValue(string text)
    {
        var lastOpenTag = text.LastIndexOf('<');
        if (lastOpenTag == -1)
        {
            return null;
        }

        var tagContent = text[lastOpenTag..];

        var match = Regex.Match(tagContent, @"""([^""]*)$|'([^']*)$");
        if (match.Success)
        {
            return match.Groups[1].Value ?? match.Groups[2].Value;
        }

        return null;
    }

    private static ContextType DetermineContextType(string text)
    {
        var lastOpenTag = text.LastIndexOf('<');
        if (lastOpenTag == -1)
        {
            return ContextType.ElementContent;
        }

        var lastClose = text.LastIndexOf('>');
        if (lastClose < lastOpenTag)
        {
            // We're inside a tag
            var tagContent = text[lastOpenTag..];

            if (tagContent.Contains('"') || tagContent.Contains('\''))
            {
                return ContextType.AttributeValue;
            }

            if (tagContent.Contains('='))
            {
                return ContextType.AttributeName;
            }

            return ContextType.AttributeName;
        }

        return ContextType.ElementContent;
    }
}

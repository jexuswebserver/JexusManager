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
    private const string XmlNamePattern = @"[\w.-]+(?::[\w.-]+)?";

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
        var pattern = $@"<(/?)({XmlNamePattern})([^>]*)>";
        var matches = Regex.Matches(text, pattern);

        foreach (Match match in matches)
        {
            var isClosing = !string.IsNullOrEmpty(match.Groups[1].Value);
            var tagName = match.Groups[2].Value;
            var isSelfClosing = match.Groups[3].Value.TrimEnd().EndsWith("/");

            if (isClosing)
            {
                if (tags.Count > 0 && tags[^1] == tagName)
                {
                    tags.RemoveAt(tags.Count - 1);
                }
            }
            else if (!isSelfClosing)
            {
                tags.Add(tagName);
            }
        }

        var lastOpenTag = text.LastIndexOf('<');
        var lastCloseTag = text.LastIndexOf('>');
        if (lastOpenTag > lastCloseTag)
        {
            var openTagText = text[lastOpenTag..];
            var currentTag = Regex.Match(openTagText, $@"^<({XmlNamePattern})");
            if (currentTag.Success)
            {
                tags.Add(currentTag.Groups[1].Value);
            }
        }

        return string.Join("/", tags);
    }

    private static string? ExtractCurrentElementName(string text)
    {
        var lastOpenTag = text.LastIndexOf('<');
        if (lastOpenTag == -1)
        {
            return null;
        }

        var openTagText = text[lastOpenTag..];
        var currentTag = Regex.Match(openTagText, $@"^<({XmlNamePattern})");
        if (currentTag.Success)
        {
            return currentTag.Groups[1].Value;
        }

        var matches = Regex.Matches(text, $@"<({XmlNamePattern})(?:\s|>|/)");
        var match = matches.Count > 0 ? matches[^1] : null;
        return match?.Success == true ? match.Groups[1].Value : null;
    }

    private static string? ExtractCurrentAttributeName(string text)
    {
        var lastOpenTag = text.LastIndexOf('<');
        if (lastOpenTag == -1)
        {
            return null;
        }

        var tagContent = text[lastOpenTag..];
        var currentElement = ExtractCurrentElementName(text);
        var attributesText = Regex.Replace(tagContent, $@"^<{XmlNamePattern}\s*", string.Empty);

        var valueMatch = Regex.Match(
            attributesText,
            $@"({XmlNamePattern})=([""'])([^""']*)\2?$");
        if (valueMatch.Success)
        {
            return valueMatch.Groups[1].Value;
        }

        // Look for attribute name before cursor
        var attrMatch = Regex.Match(attributesText, $@"({XmlNamePattern})\s*=$");
        if (attrMatch.Success)
        {
            return attrMatch.Groups[1].Value;
        }

        var partialMatch = Regex.Match(attributesText, $@"(?:^|\s)({XmlNamePattern})$");
        if (partialMatch.Success && partialMatch.Groups[1].Value != currentElement)
        {
            return partialMatch.Groups[1].Value;
        }

        return null;
    }

    private static string? ExtractCurrentAttributeValue(string text)
    {
        var lastOpenTag = text.LastIndexOf('<');
        if (lastOpenTag == -1)
        {
            return null;
        }

        var tagContent = text[lastOpenTag..];

        var attributesText = Regex.Replace(tagContent, $@"^<{XmlNamePattern}\s*", string.Empty);
        var match = Regex.Match(attributesText, $@"({XmlNamePattern})=([""'])([^""']*)\2?$");
        if (match.Success)
        {
            return match.Groups[3].Value;
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
            var insideTagName = Regex.IsMatch(tagContent, $@"^</?{XmlNamePattern}$");

            if (insideTagName)
            {
                return ContextType.ElementTag;
            }

            if (tagContent.Contains('"') || tagContent.Contains('\''))
            {
                return ContextType.AttributeValue;
            }

            if (Regex.IsMatch(tagContent, $@"^<{XmlNamePattern}\s") || tagContent.Contains('='))
            {
                return ContextType.AttributeName;
            }

            return ContextType.ElementTag;
        }

        return ContextType.ElementContent;
    }
}

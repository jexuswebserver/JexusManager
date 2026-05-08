using System.IO;

namespace IIS.LanguageServer.Tests;

internal static class TestFixtureDocument
{
    internal static (string Text, int Line, int Character) LoadWithCursor(string filePath, string needle, int relativeOffset = 0)
    {
        var text = File.ReadAllText(filePath);
        var offset = text.IndexOf(needle, System.StringComparison.Ordinal);
        if (offset < 0)
        {
            throw new InvalidDataException($"Could not find '{needle}' in fixture '{filePath}'.");
        }

        offset += relativeOffset;

        var line = 0;
        var character = 0;
        for (var i = 0; i < offset; i++)
        {
            if (text[i] == '\n')
            {
                line++;
                character = 0;
                continue;
            }

            if (text[i] != '\r')
            {
                character++;
            }
        }

        return (text, line, character);
    }
}

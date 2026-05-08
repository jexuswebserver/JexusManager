using System.Collections.Generic;
using System.IO;
using IIS.LanguageServer.Schema;

namespace IIS.LanguageServer.Tests;

internal static class SchemaCacheFixture
{
    internal static SchemaCache Create()
    {
        return new SchemaCache(new[]
        {
            Path.GetFullPath("Fixtures/IIS_schema.xml")
        });
    }

    internal static SchemaCache CreateWithMissingFile()
    {
        return new SchemaCache(new[]
        {
            Path.GetFullPath("Fixtures/IIS_schema.xml"),
            Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), "missing_schema.xml")
        });
    }
}

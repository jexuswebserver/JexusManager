using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using IIS.LanguageServer.Schema;
using Xunit;

namespace IIS.LanguageServer.Tests;

public class SchemaLoaderTests
{
    [Fact]
    public void FindSchemaFiles_ReturnsListOfPaths()
    {
        var files = SchemaLoader.FindSchemaFiles();

        // Should return a list (empty if IIS not installed)
        files.Should().BeOfType<List<string>>();
    }

    [Fact]
    public void SchemaCache_LoadsFixtureSchemasThroughMicrosoftWebAdministration()
    {
        var cache = SchemaCacheFixture.Create();

        cache.GetChildElementNames("system.applicationHost").Should().Contain("applicationPools");
        cache.GetAttributeNames("system.applicationHost/applicationPools/add")
            .Should().Contain(new[] { "name", "managedRuntimeVersion", "managedPipelineMode" });
    }

    [Fact]
    public void SchemaCache_IgnoresMissingFixtureFiles()
    {
        var cache = SchemaCacheFixture.CreateWithMissingFile();

        cache.GetAttributeValues("system.applicationHost/applicationPools/add", "managedPipelineMode")
            .Should().Contain("Integrated");
    }
}

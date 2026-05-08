using FluentAssertions;
using IIS.LanguageServer.Schema;
using Xunit;

namespace IIS.LanguageServer.Tests;

public class SchemaCacheTests
{
    [Fact]
    public void GetChildElementNames_ReturnsChildElements()
    {
        var cache = SchemaCacheFixture.Create();

        var children = cache.GetChildElementNames("system.webServer");

        children.Should().Contain(new[] { "security", "defaultDocument", "requestFiltering" });
    }

    [Fact]
    public void GetAttributeNames_ReturnsAttributeNames()
    {
        var cache = SchemaCacheFixture.Create();

        var attrs = cache.GetAttributeNames("system.webServer/security");

        attrs.Should().Contain("requireClientCertificate");
    }

    [Fact]
    public void GetAttributeType_ReturnsAttributeType()
    {
        var cache = SchemaCacheFixture.Create();

        var type = cache.GetAttributeType("system.webServer/security", "requireClientCertificate");

        type.Should().Be("bool");
    }

    [Fact]
    public void GetAttributeValues_ReturnsEnumValues()
    {
        var cache = SchemaCacheFixture.Create();

        var values = cache.GetAttributeValues("system.applicationHost/applicationPools/add", "managedPipelineMode");

        values.Should().Equal(new[] { "Integrated", "Classic" });
    }

    [Fact]
    public void GetAttributeValues_ReturnsEmptyListForNonEnumAttribute()
    {
        var cache = SchemaCacheFixture.Create();

        var values = cache.GetAttributeValues("system.applicationHost/applicationPools/add", "name");

        values.Should().BeEmpty();
    }

    [Fact]
    public void GetAttributeType_ReturnsNullForUnknownAttribute()
    {
        var cache = SchemaCacheFixture.Create();

        var type = cache.GetAttributeType("system.webServer/security", "unknownAttribute");

        type.Should().BeNull();
    }

    [Fact]
    public void GetAttributeNames_UsesCollectionAddElementSchemaShape()
    {
        var cache = SchemaCacheFixture.Create();

        var attributes = cache.GetAttributeNames("configuration/system.applicationHost/applicationPools/add");
        var childElements = cache.GetChildElementNames("configuration/system.applicationHost");

        attributes.Should().Contain(new[] { "name", "managedRuntimeVersion", "managedPipelineMode" });
        childElements.Should().Contain("applicationPools");
    }
}

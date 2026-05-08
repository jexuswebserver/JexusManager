using System;
using System.IO;
using FluentAssertions;
using IIS.LanguageServer.Handlers;
using Xunit;

namespace IIS.LanguageServer.Tests;

public class HoverHandlerTests
{
    [Fact]
    public void GetHoverResponse_OnElement_ReturnsElementPath()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var hoverHandler = new IISHoverHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "defaultDocument",
            "defaultDocument".Length);

        var result = hoverHandler.GetHoverResponse(
            fixture.Text,
            fixture.Line,
            fixture.Character);

        result.Should().NotBeNull();
        result!.Contents.Value.Should().Contain("system.webServer/defaultDocument");
    }

    [Fact]
    public void GetHoverResponse_OnAttribute_ReturnsTypeInfo()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var hoverHandler = new IISHoverHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "requireClientCertificate",
            "requireClientCertificate".Length);

        var result = hoverHandler.GetHoverResponse(
            fixture.Text,
            fixture.Line,
            fixture.Character);

        result.Should().NotBeNull();
        result!.Contents.Value.Should().Contain("bool");
        result.Contents.Value.Should().Contain("requireClientCertificate");
    }

    [Fact]
    public void GetHoverResponse_OnSectionGroup_ShowsSchemaSource()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var hoverHandler = new IISHoverHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "system.applicationHost",
            "system.applicationHost".Length);

        var result = hoverHandler.GetHoverResponse(
            fixture.Text,
            fixture.Line,
            fixture.Character);

        result.Should().NotBeNull();
        result!.Contents.Value.Should().Contain("Section Group");
        result.Contents.Value.Should().Contain("IIS_schema.xml");
        result.Contents.Value.Should().Contain(Path.GetFullPath("Fixtures/IIS_schema.xml"));
    }

    [Fact]
    public void GetHoverResponse_OnContentWithoutSymbol_ReturnsElementContext()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var hoverHandler = new IISHoverHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "binding protocol",
            "binding".Length);

        var result = hoverHandler.GetHoverResponse(
            fixture.Text,
            fixture.Line,
            fixture.Character);

        result.Should().NotBeNull();
        result!.Contents.Value.Should().Contain("system.applicationHost/sites/site/bindings/binding");
    }

    [Fact]
    public void GetHoverResponse_OnCollectionTagAndAttribute_ReturnsDifferentMetadata()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var hoverHandler = new IISHoverHandler(schemaCache, syncHandler);
        var tagFixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "<add",
            2);
        var attributeFixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "managedRuntimeVersion",
            "managedRuntimeVersion".Length);

        var tagHover = hoverHandler.GetHoverResponse(tagFixture.Text, tagFixture.Line, tagFixture.Character);
        var attributeHover = hoverHandler.GetHoverResponse(attributeFixture.Text, attributeFixture.Line, attributeFixture.Character);

        tagHover.Should().NotBeNull();
        attributeHover.Should().NotBeNull();
        tagHover!.Contents.Value.Should().Contain("**Tag** `add`");
        tagHover.Contents.Value.Should().Contain("system.applicationHost/applicationPools/add");
        attributeHover!.Contents.Value.Should().Contain("**Attribute** `managedRuntimeVersion`");
        attributeHover.Contents.Value.Should().Contain("Owner: `system.applicationHost/applicationPools/add`");
    }
}

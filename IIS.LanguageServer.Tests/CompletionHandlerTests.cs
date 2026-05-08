using System;
using System.Linq;
using FluentAssertions;
using EmmyLua.LanguageServer.Framework.Protocol.Message.Completion;
using IIS.LanguageServer.Handlers;
using Xunit;

namespace IIS.LanguageServer.Tests;

public class CompletionHandlerTests
{
    [Fact]
    public void GetCompletionResponse_OnAttributeValue_ReturnsEnumValuesFromFixture()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var completionHandler = new IISCompletionHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "managedPipelineMode=\"Integrated\"");
        var documentText = fixture.Text.Replace(
            "managedPipelineMode=\"Integrated\"",
            "managedPipelineMode=\"Integr",
            StringComparison.Ordinal);
        var line = fixture.Line;
        var character = fixture.Character + "managedPipelineMode=\"Integr".Length;

        var result = completionHandler.GetCompletionResponse(
            documentText,
            line,
            character);

        result.Should().NotBeNull();
        result!.Items.Should().Contain(item => item.Label == "Integrated");
        result.Items.Should().Contain(item => item.Label == "Classic");
        var integrated = result.Items.Single(item => item.Label == "Integrated");
        integrated.TextEdit.Should().NotBeNull();
        integrated.TextEdit!.TextEdit.Should().NotBeNull();
        integrated.TextEdit.TextEdit!.NewText.Should().Be("Integrated");
    }

    [Fact]
    public void GetCompletionResponse_OnElementAttributes_ReturnsKnownAttributesFromFixture()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var completionHandler = new IISCompletionHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "<security requireClientCertificate=\"false\" />");
        var documentText = fixture.Text.Replace(
            "<security requireClientCertificate=\"false\" />",
            "<security ",
            StringComparison.Ordinal);
        var line = fixture.Line;
        var character = fixture.Character + "<security ".Length;

        var result = completionHandler.GetCompletionResponse(
            documentText,
            line,
            character);

        result.Should().NotBeNull();
        result!.Items.Should().Contain(item => item.Label == "requireClientCertificate");
    }

    [Fact]
    public void GetCompletionResponse_OnNestedElement_ReturnsChildElementsFromFixture()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var completionHandler = new IISCompletionHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "<system.webServer>");
        var line = fixture.Line + 1;
        var character = 4;

        var result = completionHandler.GetCompletionResponse(
            fixture.Text,
            line,
            character);

        result.Should().NotBeNull();
        result!.Items.Should().Contain(item => item.Label == "security");
        result.Items.Should().Contain(item => item.Label == "defaultDocument");
    }

    [Fact]
    public void GetCompletionResponse_ForClosedDocument_ReturnsNull()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var completionHandler = new IISCompletionHandler(schemaCache, syncHandler);

        CompletionResponse? result = completionHandler.GetCompletionResponse(null, 0, 0);

        result.Should().BeNull();
    }

    [Fact]
    public void GetCompletionResponse_InsideExistingEnumValue_ReplacesWholeToken()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var completionHandler = new IISCompletionHandler(schemaCache, syncHandler);
        var fixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "managedPipelineMode=\"Integrated\"");
        var documentText = fixture.Text.Replace(
            "managedPipelineMode=\"Integrated\"",
            "managedPipelineMode=\"Integra\"",
            StringComparison.Ordinal);
        var character = fixture.Character + "managedPipelineMode=\"Integ".Length;

        var result = completionHandler.GetCompletionResponse(documentText, fixture.Line, character);

        result.Should().NotBeNull();
        var integrated = result!.Items.Should().ContainSingle(item => item.Label == "Integrated").Subject;
        integrated.TextEdit.Should().NotBeNull();
        integrated.TextEdit!.TextEdit.Should().NotBeNull();
        integrated.TextEdit.TextEdit!.NewText.Should().Be("Integrated");
    }
}

using System;
using FluentAssertions;
using IIS.LanguageServer.Handlers;
using Xunit;

namespace IIS.LanguageServer.Tests;

public class LanguageServerDocumentIntegrationTests
{
    [Fact]
    public void CompletionAndHover_UseTheSameRealDocumentAndSchemaFixtures()
    {
        var schemaCache = SchemaCacheFixture.Create();
        var syncHandler = new TextDocumentSyncHandler(schemaCache);
        var completionHandler = new IISCompletionHandler(schemaCache, syncHandler);
        var hoverHandler = new IISHoverHandler(schemaCache, syncHandler);
        var completionFixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "managedPipelineMode=\"Integrated\"");
        var completionText = completionFixture.Text.Replace(
            "managedPipelineMode=\"Integrated\"",
            "managedPipelineMode=\"Integr",
            StringComparison.Ordinal);
        var completionCharacter = completionFixture.Character + "managedPipelineMode=\"Integr".Length;
        var hoverFixture = TestFixtureDocument.LoadWithCursor(
            "Fixtures/applicationhost.config",
            "requireClientCertificate",
            "requireClientCertificate".Length);
        const string uri = "file:///fixtures/applicationhost.config";

        syncHandler.OpenDocumentForTesting(uri, completionText);
        var completion = completionHandler.GetCompletionResponse(
            syncHandler.GetDocumentContent(uri),
            completionFixture.Line,
            completionCharacter);

        syncHandler.OpenDocumentForTesting(uri, hoverFixture.Text);
        var hover = hoverHandler.GetHoverResponse(
            syncHandler.GetDocumentContent(uri),
            hoverFixture.Line,
            hoverFixture.Character);

        completion.Should().NotBeNull();
        completion!.Items.Should().Contain(item => item.Label == "Integrated");
        hover.Should().NotBeNull();
        hover!.Contents.Value.Should().Contain("requireClientCertificate");
    }
}

using System.IO;
using System.Linq;
using FluentAssertions;
using EmmyLua.LanguageServer.Framework.Protocol.Model.Diagnostic;
using IIS.LanguageServer.Handlers;
using Xunit;

namespace IIS.LanguageServer.Tests;

public class DiagnosticsHandlerTests
{
    private static DiagnosticsHandler CreateHandler() =>
        new(SchemaCacheFixture.Create());

    [Fact]
    public void CollectDiagnostics_EmptyDocument_ReturnsNoDiagnostics()
    {
        var handler = CreateHandler();
        var result = handler.CollectDiagnostics(string.Empty);
        result.Should().BeEmpty();
    }

    [Fact]
    public void CollectDiagnostics_MalformedXml_ReportsXmlError()
    {
        var handler = CreateHandler();
        var result = handler.CollectDiagnostics("<configuration><unclosed>");

        result.Should().HaveCount(1);
        result[0].Severity.Should().Be(DiagnosticSeverity.Error);
        result[0].Source.Should().Be("xml");
        result[0].Message.StringValue.Should().Contain("XML parse error");
    }

    [Fact]
    public void CollectDiagnostics_NonConfigurationRoot_ReturnsNoDiagnostics()
    {
        var handler = CreateHandler();
        var result = handler.CollectDiagnostics("<root><child /></root>");
        result.Should().BeEmpty();
    }

    [Fact]
    public void CollectDiagnostics_ValidDocument_ReturnsNoErrors()
    {
        var handler = CreateHandler();
        var text = File.ReadAllText("Fixtures/applicationhost.config");
        var result = handler.CollectDiagnostics(text);

        result.Where(d => d.Severity == DiagnosticSeverity.Error).Should().BeEmpty();
    }

    [Fact]
    public void CollectDiagnostics_AllowUnrecognizedAttributes_NoWarningsForExtraAttributes()
    {
        // configProtectedData/providers/add has allowUnrecognizedAttributes="true"
        // The fixture applicationhost.config has a 'description' attribute on that element
        var handler = CreateHandler();
        var text = File.ReadAllText("Fixtures/applicationhost.config");
        var result = handler.CollectDiagnostics(text);

        var unknownOnProvidersAdd = result.Where(d =>
            d.Severity == DiagnosticSeverity.Warning &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains("description") &&
            d.Message.StringValue.Contains("'add'")).ToList();

        unknownOnProvidersAdd.Should().BeEmpty("configProtectedData/providers/add allows unrecognized attributes");
    }

    [Fact]
    public void CollectDiagnostics_UnknownAttribute_ReportsWarning()
    {
        var handler = CreateHandler();
        var xml = """
            <configuration>
              <system.applicationHost>
                <applicationPools>
                  <add name="TestPool" unknownAttr="bad" />
                </applicationPools>
              </system.applicationHost>
            </configuration>
            """;

        var result = handler.CollectDiagnostics(xml);

        result.Should().Contain(d =>
            d.Severity == DiagnosticSeverity.Warning &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains("unknownAttr"));
    }

    [Fact]
    public void CollectDiagnostics_InvalidEnumValue_ReportsError()
    {
        var handler = CreateHandler();
        var xml = """
            <configuration>
              <system.applicationHost>
                <applicationPools>
                  <add name="TestPool" managedPipelineMode="BadValue" />
                </applicationPools>
              </system.applicationHost>
            </configuration>
            """;

        var result = handler.CollectDiagnostics(xml);

        result.Should().Contain(d =>
            d.Severity == DiagnosticSeverity.Error &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains("BadValue") &&
            d.Message.StringValue.Contains("managedPipelineMode"));
    }

    [Theory]
    [InlineData("lockAttributes")]
    [InlineData("lockAllAttributesExcept")]
    [InlineData("lockElements")]
    [InlineData("lockAllElementsExcept")]
    [InlineData("lockItem")]
    public void CollectDiagnostics_LockAttributes_NoWarning(string lockAttr)
    {
        var handler = CreateHandler();
        var xml = $"""
            <configuration>
              <system.applicationHost>
                <applicationPools>
                  <add name="TestPool" {lockAttr}="autoStart" />
                </applicationPools>
              </system.applicationHost>
            </configuration>
            """;

        var result = handler.CollectDiagnostics(xml);

        result.Should().NotContain(d =>
            d.Severity == DiagnosticSeverity.Warning &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains(lockAttr));
    }

    [Fact]
    public void CollectDiagnostics_ValidEnumValue_NoError()
    {
        var handler = CreateHandler();
        var xml = """
            <configuration>
              <system.applicationHost>
                <applicationPools>
                  <add name="TestPool" managedPipelineMode="Integrated" />
                </applicationPools>
              </system.applicationHost>
            </configuration>
            """;

        var result = handler.CollectDiagnostics(xml);

        result.Should().NotContain(d =>
            d.Severity == DiagnosticSeverity.Error &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains("managedPipelineMode"));
    }

    [Fact]
    public void CollectDiagnostics_EmptyBoolAttribute_ReportsError()
    {
        var handler = CreateHandler();
        var xml = """
            <configuration>
              <system.applicationHost>
                <applicationPools>
                  <add name="TestPool" autoStart="" />
                </applicationPools>
              </system.applicationHost>
            </configuration>
            """;

        var result = handler.CollectDiagnostics(xml);

        result.Should().Contain(d =>
            d.Severity == DiagnosticSeverity.Error &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains("autoStart"));
    }

    [Fact]
    public void CollectDiagnostics_InvalidBoolAttribute_ReportsError()
    {
        var handler = CreateHandler();
        var xml = """
            <configuration>
              <system.applicationHost>
                <applicationPools>
                  <add name="TestPool" autoStart="yes" />
                </applicationPools>
              </system.applicationHost>
            </configuration>
            """;

        var result = handler.CollectDiagnostics(xml);

        result.Should().Contain(d =>
            d.Severity == DiagnosticSeverity.Error &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains("autoStart"));
    }

    [Fact]
    public void CollectDiagnostics_ValidBoolAttribute_NoError()
    {
        var handler = CreateHandler();
        var xml = """
            <configuration>
              <system.applicationHost>
                <applicationPools>
                  <add name="TestPool" autoStart="true" />
                </applicationPools>
              </system.applicationHost>
            </configuration>
            """;

        var result = handler.CollectDiagnostics(xml);

        result.Should().NotContain(d =>
            d.Severity == DiagnosticSeverity.Error &&
            d.Message.StringValue != null &&
            d.Message.StringValue.Contains("autoStart"));
    }
}

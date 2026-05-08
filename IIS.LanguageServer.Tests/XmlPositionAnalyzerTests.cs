using FluentAssertions;
using IIS.LanguageServer.Language;
using Xunit;

namespace IIS.LanguageServer.Tests;

public class XmlPositionAnalyzerTests
{
    [Fact]
    public void GetContext_OnElementTag_ReturnsCorrectElementPath()
    {
        var text = "<system.webServer>";
        var offset = text.IndexOf('>');

        var context = XmlPositionAnalyzer.GetContext(text, offset);

        context.ElementPath.Should().Be("system.webServer");
        context.CurrentElementName.Should().Be("system.webServer");
        context.Type.Should().Be(ContextType.ElementTag);
    }

    [Fact]
    public void GetContext_OnOpeningTag_ReturnsElementPath()
    {
        var text = "<configuration><system.webServer>";
        var offset = text.Length;

        var context = XmlPositionAnalyzer.GetContext(text, offset);

        context.ElementPath.Should().Contain("system.webServer");
    }

    [Fact]
    public void GetContext_OnAttributeName_ReturnsAttributeName()
    {
        var text = "<binding protocol=\"http\" bindingInformation";
        var offset = text.Length;

        var context = XmlPositionAnalyzer.GetContext(text, offset);

        context.CurrentAttributeName.Should().Be("bindingInformation");
    }

    [Fact]
    public void GetContext_OnAttributeValue_ReturnsAttributeValue()
    {
        var text = "<binding protocol=\"http\"";
        var offset = text.Length;

        var context = XmlPositionAnalyzer.GetContext(text, offset);

        context.Type.Should().Be(ContextType.AttributeValue);
        context.CurrentAttributeValue.Should().Be("http");
    }

    [Fact]
    public void GetContext_InsideAttributeValue_PartialValue()
    {
        var text = "<add managedPipelineMode=\"Integr";
        var offset = text.Length;

        var context = XmlPositionAnalyzer.GetContext(text, offset);

        context.Type.Should().Be(ContextType.AttributeValue);
        context.CurrentAttributeName.Should().Be("managedPipelineMode");
        context.CurrentAttributeValue.Should().Be("Integr");
    }

    [Fact]
    public void GetContext_ClosingTag_UpdatesElementPath()
    {
        var text = "<configuration><system.webServer></system.webServer>";
        var offset = text.Length;

        var context = XmlPositionAnalyzer.GetContext(text, offset);

        // After closing tag, element path should not include closed element
        context.ElementPath.Should().NotContain("system.webServer");
    }

    [Fact]
    public void GetContext_NestedElements_BuildsCorrectPath()
    {
        var text = "<system.applicationHost><applicationPools><add name=\"test\"";
        var offset = text.Length;

        var context = XmlPositionAnalyzer.GetContext(text, offset);

        context.ElementPath.Should().Contain("system.applicationHost");
        context.ElementPath.Should().Contain("applicationPools");
        context.ElementPath.Should().Contain("add");
    }
}

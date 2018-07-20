// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.TraceFailedRequests
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.TraceFailedRequests;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class TraceFailedRequestssFeatureSiteTestFixture
    {
        private TraceFailedRequestsFeature _feature;

        private ServerManager _server;

        private const string Current = @"applicationHost.config";

        private void SetUp()
        {
            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";
            if (Helper.IsRunningOnMono())
            {
                File.Copy("Website1/original.config", "Website1/web.config", true);
                File.Copy(OriginalMono, Current, true);
            }
            else
            {
                File.Copy("Website1\\original.config", "Website1\\web.config", true);
                File.Copy(Original, Current, true);
            }

            Environment.SetEnvironmentVariable(
                "JEXUS_TEST_HOME",
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            _server = new IisExpressServerManager(Current);

            var serviceContainer = new ServiceContainer();
            serviceContainer.RemoveService(typeof(IConfigurationService));
            serviceContainer.RemoveService(typeof(IControlPanel));
            var scope = ManagementScope.Site;
            serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            serviceContainer.AddService(
                typeof(IConfigurationService),
                new ConfigurationService(
                    null,
                    _server.Sites[0].GetWebConfiguration(),
                    scope,
                    null,
                    _server.Sites[0],
                    null,
                    null,
                    null, _server.Sites[0].Name));

            serviceContainer.RemoveService(typeof(IManagementUIService));
            var mock = new Mock<IManagementUIService>();
            mock.Setup(
                action =>
                action.ShowMessage(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<MessageBoxButtons>(),
                    It.IsAny<MessageBoxIcon>(),
                    It.IsAny<MessageBoxDefaultButton>())).Returns(DialogResult.Yes);
            serviceContainer.AddService(typeof(IManagementUIService), mock.Object);

            var module = new TraceFailedRequestsModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new TraceFailedRequestsFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(2, _feature.Items.Count);
            Assert.Equal("*.asp", _feature.Items[0].Path);
        }

        [Fact]
        public void TestRemoveInherited()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var server = document.Root?.XPathSelectElement("/configuration/system.webServer");
            server?.Add(
                new XElement("tracing",
                    new XElement("traceFailedRequests",
                        new XElement("remove",
                            new XAttribute("path", "*.asp")))));
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("*.asp", _feature.SelectedItem.Path);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Single(_feature.Items);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            document.Save(expected);

            var item = new TraceFailedRequestsItem(null);
            item.Path = "*.php";
            item.Codes = "200-999";
            _feature.AddItem(item);

            Assert.Equal("*.php", _feature.SelectedItem.Path);
            Assert.Equal(3, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(2, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestEditInherited()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var server = document.Root?.XPathSelectElement("/configuration/system.webServer");
            server?.Add(
                new XElement("tracing",
                    new XElement("traceFailedRequests",
                        new XElement("remove",
                            new XAttribute("path", "*.asp")),
                        new XElement("add",
                            new XAttribute("path", "*.asp"),
                            new XElement("traceAreas",
                                new XElement("add",
                                    new XAttribute("provider", "ASP"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ASPNET"),
                                    new XAttribute("areas", "Infrastructure,Module,Page,AppServices"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ISAPI Extension"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "WWW Server"),
                                    new XAttribute("areas", "Authentication,Security,Filter,StaticFile,CGI,Compression,Cache,RequestNotifications,Module,Rewrite,WebSocket"),
                                    new XAttribute("verbosity", "Verbose"))),
                            new XElement("failureDefinitions",
                                new XAttribute("statusCodes", "100-999"))
                            ))));
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("*.asp", _feature.SelectedItem.Path);
            var item = _feature.SelectedItem;
            item.Codes = "100-999";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("100-999", _feature.SelectedItem.Codes);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var server = document.Root?.XPathSelectElement("/configuration/system.webServer");
            server?.Add(
                new XElement("tracing",
                    new XElement("traceFailedRequests",
                        new XElement("add",
                            new XAttribute("path", "*.php"),
                            new XElement("failureDefinitions",
                                new XAttribute("statusCodes", "100-999")),
                            new XElement("traceAreas",
                                new XElement("add",
                                    new XAttribute("provider", "ASP"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ASPNET"),
                                    new XAttribute("areas", "Infrastructure,Module,Page,AppServices"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ISAPI Extension"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "WWW Server"),
                                    new XAttribute("areas", "Authentication,Security,Filter,StaticFile,CGI,Compression,Cache,RequestNotifications,Module,Rewrite,WebSocket"),
                                    new XAttribute("verbosity", "Verbose")))
                            ))));
            document.Save(expected);

            var item = new TraceFailedRequestsItem(null);
            item.Path = "*.php";
            item.Codes = "200-999";
            _feature.AddItem(item);

            Assert.Equal("*.php", _feature.SelectedItem.Path);
            Assert.Equal(3, _feature.Items.Count);
            item.Codes = "100-999";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("100-999", _feature.SelectedItem.Codes);
            Assert.Equal(3, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var server = document.Root?.XPathSelectElement("/configuration/system.webServer");
            server?.Add(
                new XElement("tracing",
                    new XElement("traceFailedRequests",
                        new XElement("add",
                            new XAttribute("path", "*.php"),
                            new XElement("traceAreas",
                                new XElement("add",
                                    new XAttribute("provider", "ASP"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ASPNET"),
                                    new XAttribute("areas", "Infrastructure,Module,Page,AppServices"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ISAPI Extension"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "WWW Server"),
                                    new XAttribute("areas", "Authentication,Security,Filter,StaticFile,CGI,Compression,Cache,RequestNotifications,Module,Rewrite,WebSocket"),
                                    new XAttribute("verbosity", "Verbose"))),
                            new XElement("failureDefinitions",
                                new XAttribute("statusCodes", "200-999"))
                            ))));
            document.Save(expected);

            var item = new TraceFailedRequestsItem(null);
            item.Path = "*.php";
            item.Codes = "200-999";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("200-999", _feature.SelectedItem.Codes);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestRevert()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var server = document.Root?.XPathSelectElement("/configuration/system.webServer");
            server?.Add(new XElement("tracing"));
            document.Save(expected);

            var item = new TraceFailedRequestsItem(null);
            item.Path = "*.php";
            item.Codes = "200-999";
            _feature.AddItem(item);
            Assert.Equal(3, _feature.Items.Count);

            _feature.Revert();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(2, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestMoveUp()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var server = document.Root?.XPathSelectElement("/configuration/system.webServer");
            server?.Add(
                new XElement("tracing",
                    new XElement("traceFailedRequests",
                        new XElement("clear"),
                        new XElement("add",
                            new XAttribute("path", "*.aspx"),
                            new XElement("traceAreas",
                                new XElement("add",
                                    new XAttribute("provider", "ASP"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ASPNET"),
                                    new XAttribute("areas", "Infrastructure,Module,Page,AppServices"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ISAPI Extension"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "WWW Server"),
                                    new XAttribute("areas", "Authentication,Security,Filter,StaticFile,CGI,Compression,Cache,RequestNotifications,Module,Rewrite,WebSocket"),
                                    new XAttribute("verbosity", "Verbose"))),
                            new XElement("failureDefinitions",
                                new XAttribute("statusCodes", "200-999"))),
                        new XElement("add",
                            new XAttribute("path", "*.asp"),
                            new XElement("traceAreas",
                                new XElement("add",
                                    new XAttribute("provider", "ASP"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ASPNET"),
                                    new XAttribute("areas", "Infrastructure,Module,Page,AppServices"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ISAPI Extension"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "WWW Server"),
                                    new XAttribute("areas", "Authentication,Security,Filter,StaticFile,CGI,Compression,Cache,RequestNotifications,Module,Rewrite,WebSocket"),
                                    new XAttribute("verbosity", "Verbose"))),
                            new XElement("failureDefinitions",
                                new XAttribute("statusCodes", "200-999")))
                    )));
            document.Save(expected);

            var last = 1;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[last];
            var expectedValue = "*.aspx";
            Assert.Equal(expectedValue, _feature.Items[last].Path);
            var original = "*.asp";
            Assert.Equal(original, _feature.Items[previous].Path);
            _feature.MoveUp();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(expectedValue, _feature.SelectedItem.Path);
            Assert.Equal(expectedValue, _feature.Items[previous].Path);
            Assert.Equal(original, _feature.Items[last].Path);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestMoveDown()
        {
            SetUp();

            const string expected = @"expected_add.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var server = document.Root?.XPathSelectElement("/configuration/system.webServer");
            server?.Add(
                new XElement("tracing",
                    new XElement("traceFailedRequests",
                        new XElement("remove",
                            new XAttribute("path", "*.asp")),
                        new XElement("add",
                            new XAttribute("path", "*.asp"),
                            new XElement("traceAreas",
                                new XElement("add",
                                    new XAttribute("provider", "ASP"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ASPNET"),
                                    new XAttribute("areas", "Infrastructure,Module,Page,AppServices"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "ISAPI Extension"),
                                    new XAttribute("verbosity", "Verbose")),
                                new XElement("add",
                                    new XAttribute("provider", "WWW Server"),
                                    new XAttribute("areas", "Authentication,Security,Filter,StaticFile,CGI,Compression,Cache,RequestNotifications,Module,Rewrite,WebSocket"),
                                    new XAttribute("verbosity", "Verbose"))),
                            new XElement("failureDefinitions",
                                new XAttribute("statusCodes", "200-999"))
                    ))));
            document.Save(expected);

            var last = 1;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[previous];
            var expectedValue = "*.aspx";
            Assert.Equal(expectedValue, _feature.Items[last].Path);
            var original = "*.asp";
            Assert.Equal(original, _feature.Items[previous].Path);
            _feature.MoveDown();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(original, _feature.SelectedItem.Path);
            Assert.Equal(expectedValue, _feature.Items[previous].Path);
            Assert.Equal(original, _feature.Items[last].Path);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }
    }
}

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

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using NSubstitute;

    public class TraceFailedRequestsFeatureServerTestFixture
    {
        private TraceFailedRequestsFeature _feature;

        private ServerManager _server;

        private ServiceContainer _serviceContainer;

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

            _serviceContainer = new ServiceContainer();
            _serviceContainer.RemoveService(typeof(IConfigurationService));
            _serviceContainer.RemoveService(typeof(IControlPanel));
            var scope = ManagementScope.Server;
            _serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            _serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(null, _server.GetApplicationHostConfiguration(), scope, _server, null, null, null, null, null));

            _serviceContainer.RemoveService(typeof(IManagementUIService));
            var substitute = Substitute.For<IManagementUIService>();
            substitute.ShowMessage(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<MessageBoxButtons>(),
                Arg.Any<MessageBoxIcon>(),
                Arg.Any<MessageBoxDefaultButton>()).Returns(DialogResult.Yes);

            _serviceContainer.AddService(typeof(IManagementUIService), substitute);

            var module = new TraceFailedRequestsModule();
            module.TestInitialize(_serviceContainer, null);

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
        public void TestRemove()
        {
            SetUp();
            const string Expected = @"expected_remove.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests");
            node?.FirstNode?.Remove();
            document.Save(Expected);

            Assert.Equal("*.asp", _feature.Items[0].Path);
            _feature.SelectedItem = _feature.Items[0];
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Single(_feature.Items);

            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();
            const string Expected = @"expected_edit.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests");
            var element = node?.FirstNode as XElement;
            XElement definition = element?.FirstNode?.NextNode as XElement;
            definition?.SetAttributeValue("statusCodes", "100-999");
            definition.Remove();
            element.AddFirst(definition);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            var item = _feature.SelectedItem;
            item.Codes = "100-999";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("100-999", _feature.SelectedItem.Codes);
            Assert.Equal(2, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();
            const string Expected = @"expected_add.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests");
            node?.Add(
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
                               ));
            document.Save(Expected);

            var item = new TraceFailedRequestsItem(null);
            item.Path = "*.php";
            item.Codes = "200-999";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("*.php", _feature.SelectedItem.Path);
            Assert.Equal(3, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestRevert()
        {
            SetUp();
            var exception = Assert.Throws<InvalidOperationException>(() => _feature.Revert());
            Assert.Equal("Revert operation cannot be done at server level", exception.Message);
        }

        [Fact]
        public void TestMoveUp()
        {
            SetUp();
            const string Expected = @"expected_up.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests");
            var node1 = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests/add[@path='*.aspx']");
            var node2 = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests/add[@path='*.asp']");
            node1?.Remove();
            node2?.Remove();
            node?.AddFirst(node2);
            node?.AddFirst(node1);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[1];
            var selected = "*.aspx";
            var other = "*.asp";
            Assert.Equal(selected, _feature.Items[1].Path);
            Assert.Equal(other, _feature.Items[0].Path);
            _feature.MoveUp();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(selected, _feature.SelectedItem.Path);
            Assert.Equal(selected, _feature.Items[0].Path);
            Assert.Equal(other, _feature.Items[1].Path);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestMoveDown()
        {
            SetUp();
            const string Expected = @"expected_up.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests");
            var node1 = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests/add[@path='*.aspx']");
            var node2 = document.Root?.XPathSelectElement("/configuration/system.webServer/tracing/traceFailedRequests/add[@path='*.asp']");
            node1?.Remove();
            node2?.Remove();
            node?.AddFirst(node2);
            node?.AddFirst(node1);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            var other = "*.aspx";
            Assert.Equal(other, _feature.Items[1].Path);
            var selected = "*.asp";
            Assert.Equal(selected, _feature.Items[0].Path);
            _feature.MoveDown();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(selected, _feature.SelectedItem.Path);
            Assert.Equal(other, _feature.Items[0].Path);
            Assert.Equal(selected, _feature.Items[1].Path);
            XmlAssert.Equal(Expected, Current);
        }
    }
}

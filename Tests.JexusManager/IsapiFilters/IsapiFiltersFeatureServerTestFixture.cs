// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.IsapiFilters
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.IsapiFilters;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class IsapiFiltersFeatureServerTestFixture
    {
        private IsapiFiltersFeature _feature;

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
            var mock = new Mock<IManagementUIService>();
            mock.Setup(
                action =>
                action.ShowMessage(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<MessageBoxButtons>(),
                    It.IsAny<MessageBoxIcon>(),
                    It.IsAny<MessageBoxDefaultButton>())).Returns(DialogResult.Yes);
            _serviceContainer.AddService(typeof(IManagementUIService), mock.Object);

            var module = new IsapiFiltersModule();
            module.TestInitialize(_serviceContainer, null);

            _feature = new IsapiFiltersFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(5, _feature.Items.Count);
            Assert.Equal("ASP.Net_2.0.50727-64", _feature.Items[0].Name);
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();
            const string Expected = @"expected_remove.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters");
            node?.FirstNode?.Remove();
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(4, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();
            const string Expected = @"expected_edit.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters");
            var element = node?.FirstNode as XElement;
            element?.SetAttributeValue("path", "c:\\test.dll");
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            var item = _feature.SelectedItem;
            var expected = "c:\\test.dll";
            item.Path = expected;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(expected, _feature.SelectedItem.Path);
            Assert.Equal(5, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();
            const string Expected = @"expected_add.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters");
            var element = new XElement("filter");
            element.SetAttributeValue("name", "my cgi");
            element.SetAttributeValue("path", "c:\\test.dll");
            node?.Add(element);
            document.Save(Expected);

            var item = new IsapiFiltersItem(null);
            item.Name = "my cgi";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("my cgi", _feature.SelectedItem.Name);
            Assert.Equal("c:\\test.dll", _feature.SelectedItem.Path);
            Assert.Equal(6, _feature.Items.Count);
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
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters");
            var node1 = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters/filter[@name='ASP.Net_2.0.50727.0']");
            var node2 = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters/filter[@name='ASP.Net_2.0.50727.0-64']");
            node1?.Remove();
            node2?.Remove();
            node?.AddFirst(node1);
            node?.AddFirst(node2);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[1];
            var selected = "ASP.Net_2.0.50727.0";
            var other = "ASP.Net_2.0.50727-64";
            Assert.Equal(selected, _feature.Items[1].Name);
            Assert.Equal(other, _feature.Items[0].Name);
            _feature.MoveUp();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(selected, _feature.SelectedItem.Name);
            Assert.Equal(selected, _feature.Items[0].Name);
            Assert.Equal(other, _feature.Items[1].Name);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestMoveDown()
        {
            SetUp();
            const string Expected = @"expected_up.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters");
            var node1 = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters/filter[@name='ASP.Net_2.0.50727.0']");
            var node2 = document.Root.XPathSelectElement("/configuration/system.webServer/isapiFilters/filter[@name='ASP.Net_2.0.50727.0-64']");
            node1?.Remove();
            node2?.Remove();
            node?.AddFirst(node1);
            node?.AddFirst(node2);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            var other = "ASP.Net_2.0.50727.0";
            Assert.Equal(other, _feature.Items[1].Name);
            var selected = "ASP.Net_2.0.50727-64";
            Assert.Equal(selected, _feature.Items[0].Name);
            _feature.MoveDown();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(selected, _feature.SelectedItem.Name);
            Assert.Equal(other, _feature.Items[0].Name);
            Assert.Equal(selected, _feature.Items[1].Name);
            XmlAssert.Equal(Expected, Current);
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;
using System.Xml.XPath;

namespace Tests.Modules
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.Modules;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;

    public class ModulesFeatureServerTestFixture
    {
        private ModulesFeature _feature;

        private ServerManager _server;

        private ServiceContainer _serviceContainer;

        private const string Current = @"applicationHost.config";

        public void SetUp()
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

            var module = new ModulesModule();
            module.TestInitialize(_serviceContainer, null);

            _feature = new ModulesFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(44, _feature.Items.Count);
            Assert.Equal("DynamicCompressionModule", _feature.Items[0].Name);
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();
            const string Expected = @"expected_remove.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules");
            node?.FirstNode?.Remove(); // remove comment
            node?.FirstNode?.Remove();
            document.Save(Expected);

            Assert.Equal("DynamicCompressionModule", _feature.Items[0].Name);
            _feature.SelectedItem = _feature.Items[0];
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(43, _feature.Items.Count);
            Assert.Equal("StaticCompressionModule", _feature.Items[0].Name);

            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();
            const string Expected = @"expected_remove.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules");
            node?.FirstNode?.Remove(); // remove comment
            var element = node?.LastNode as XElement;
            element?.SetAttributeValue("type", "test");
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[43];
            var item = _feature.SelectedItem;
            item.Type = "test";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Type);
            Assert.Equal(44, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();
            const string Expected = @"expected_add.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules");
            node?.FirstNode?.Remove(); // remove comment
            var element = new XElement("add");
            element.SetAttributeValue("name", "test");
            element.SetAttributeValue("type", "test");
            node?.Add(element);
            document.Save(Expected);

            var item = new ModulesItem(null);
            item.Name = "test";
            item.Type = "test";
            item.IsManaged = true;
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(45, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestAddGlobal()
        {
            SetUp();
            const string Expected = @"expected_addglobal.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/globalModules");
            var element = new XElement("add");
            element.SetAttributeValue("name", "test");
            element.SetAttributeValue("image", "test");
            node?.Add(element);
            document.Save(Expected);

            Assert.Equal(37, _feature.GlobalModules.Count);

            var item = new GlobalModule(null);
            item.Name = "test";
            item.Image = "test";
            _feature.AddGlobal(item);
            Assert.Equal(38, _feature.GlobalModules.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestRemoveGlobal()
        {
            SetUp();
            const string Expected = @"expected_removeglobal.config";

            var document = XDocument.Load(Current);
            document.Save(Expected);

            Assert.Equal(37, _feature.GlobalModules.Count);

            var item = new GlobalModule(null);
            item.Name = "test";
            item.Image = "test";
            _feature.AddGlobal(item);
            Assert.Equal(38, _feature.GlobalModules.Count);

            _feature.RemoveGlobal(item);
            Assert.Equal(37, _feature.GlobalModules.Count);
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
            var node = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules");
            node?.FirstNode?.Remove(); // remove comment
            var node1 = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules/add[@name='StaticCompressionModule']");
            var node2 = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules/add[@name='DynamicCompressionModule']");
            node1?.Remove();
            node2?.Remove();
            node?.AddFirst(node2);
            node?.AddFirst(node1);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[1];
            var selected = "StaticCompressionModule";
            var other = "DynamicCompressionModule";
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
            var node = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules");
            node?.FirstNode?.Remove(); // remove comment
            var node1 = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules/add[@name='StaticCompressionModule']");
            var node2 = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules/add[@name='DynamicCompressionModule']");
            node1?.Remove();
            node2?.Remove();
            node?.AddFirst(node2);
            node?.AddFirst(node1);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            var other = "StaticCompressionModule";
            Assert.Equal(other, _feature.Items[1].Name);
            var selected = "DynamicCompressionModule";
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

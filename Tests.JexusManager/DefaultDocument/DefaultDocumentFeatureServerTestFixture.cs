// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.DefaultDocument
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.DefaultDocument;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class DefaultDocumentFeatureServerTestFixture
    {
        private DefaultDocumentFeature _feature;

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

            var module = new DefaultDocumentModule();
            module.TestInitialize(_serviceContainer, null);

            _feature = new DefaultDocumentFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(6, _feature.Items.Count);
        }

        [Fact]
        public void TestEnable()
        {
            SetUp();
            const string Expected = @"expected_disabled.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument");
            node?.SetAttributeValue("enabled", "false");
            document.Save(Expected);

            Assert.True(_feature.IsEnabled);
            _feature.Disable();
            Assert.False(_feature.IsEnabled);
            XmlAssert.Equal(Expected, Current);

            _feature.Enable();
            Assert.True(_feature.IsEnabled);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(
                Helper.IsRunningOnMono()
                    ? OriginalMono
                    : Original,
                Current);
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();
            const string Expected = @"expected_remove.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add[@value='Default.asp']");
            node?.Remove();
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[1];
            Assert.Equal("Default.asp", _feature.Items[1].Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal("index.htm", _feature.Items[1].Name);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();
            const string Expected = @"expected_add.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            var add = new XElement("add");
            add.SetAttributeValue("value", "default.my");
            node?.AddFirst(add);
            document.Save(Expected);

            var item = new DocumentItem(null);
            item.Name = "default.my";
            _feature.InsertItem(_feature.Items.FindIndex(i => i.Flag == "Local"), item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("default.my", _feature.SelectedItem.Name);
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
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            var asp = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add[@value='Default.asp']");
            asp?.Remove();
            var htm = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add[@value='Default.htm']");
            htm?.Remove();
            node?.AddFirst(htm);
            node?.AddFirst(asp);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[1];
            Assert.Equal("Default.asp", _feature.Items[1].Name);
            Assert.Equal("Default.htm", _feature.Items[0].Name);
            _feature.MoveUp();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("Default.asp", _feature.SelectedItem.Name);
            Assert.Equal("Default.asp", _feature.Items[0].Name);
            Assert.Equal("Default.htm", _feature.Items[1].Name);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestMoveDown()
        {
            SetUp();
            const string Expected = @"expected_up.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            var asp = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add[@value='Default.asp']");
            asp?.Remove();
            var htm = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add[@value='Default.htm']");
            htm?.Remove();
            node?.AddFirst(htm);
            node?.AddFirst(asp);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("Default.asp", _feature.Items[1].Name);
            Assert.Equal("Default.htm", _feature.Items[0].Name);
            _feature.MoveDown();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("Default.htm", _feature.SelectedItem.Name);
            Assert.Equal("Default.asp", _feature.Items[0].Name);
            Assert.Equal("Default.htm", _feature.Items[1].Name);
            XmlAssert.Equal(Expected, Current);
        }
    }
}

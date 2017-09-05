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

    public class DefaultDocumentFeatureSiteTestFixture
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
            var scope = ManagementScope.Site;
            _serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            _serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(null, _server.Sites[0].GetWebConfiguration(), scope, null, _server.Sites[0], null, null, null, _server.Sites[0].Name));

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
            Assert.Equal(7, _feature.Items.Count);
        }

        [Fact]
        public void TestEnable()
        {
            SetUp();
            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_disabled.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument");
            node?.SetAttributeValue("enabled", "false");
            document.Save(expected);

            Assert.True(_feature.IsEnabled);
            _feature.Disable();
            Assert.False(_feature.IsEnabled);

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);

            _feature.Enable();
            Assert.True(_feature.IsEnabled);

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestRemoveInherited()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_remove.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            var remove = new XElement("remove");
            remove.SetAttributeValue("value", "Default.asp");
            node?.AddFirst(remove);
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[2];
            Assert.Equal("Default.asp", _feature.Items[2].Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal("index.htm", _feature.Items[2].Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();
            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_remove1.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            node?.Remove();
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("home1.html", _feature.Items[0].Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal("Default.htm", _feature.Items[0].Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_add.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            var add = new XElement("add");
            add.SetAttributeValue("value", "default.my");
            node?.AddFirst(add);
            document.Save(expected);

            var item = new DocumentItem(null);
            item.Name = "default.my";
            _feature.InsertItem(_feature.Items.FindIndex(i => i.Flag == "Local"), item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("default.my", _feature.SelectedItem.Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestRevert()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_revert.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            node?.Remove();
            document.Save(expected);

            _feature.Revert();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(6, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestMoveUp()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_up.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            var add = new XElement("add");
            add.SetAttributeValue("value", "Default.htm");
            node?.AddFirst(add);
            var remove = new XElement("remove");
            remove.SetAttributeValue("value", "Default.htm");
            node?.AddFirst(remove);
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[1];
            Assert.Equal("Default.htm", _feature.Items[1].Name);
            Assert.Equal("home1.html", _feature.Items[0].Name);
            _feature.MoveUp();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("Default.htm", _feature.SelectedItem.Name);
            Assert.Equal("Default.htm", _feature.Items[0].Name);
            Assert.Equal("home1.html", _feature.Items[1].Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";
            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestMoveDown()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_up.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files");
            var htm = new XElement("add");
            htm.SetAttributeValue("value", "Default.htm");
            node?.AddFirst(htm);
            node?.AddFirst(new XElement("clear"));
            var asp = new XElement("add");
            asp.SetAttributeValue("value", "Default.asp");
            node?.Add(asp);
            var index = new XElement("add");
            index.SetAttributeValue("value", "index.htm");
            node?.Add(index);
            var index1 = new XElement("add");
            index1.SetAttributeValue("value", "index.html");
            node?.Add(index1);
            var iis = new XElement("add");
            iis.SetAttributeValue("value", "iisstart.htm");
            node?.Add(iis);
            var aspx = new XElement("add");
            aspx.SetAttributeValue("value", "default.aspx");
            node?.Add(aspx);
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("Default.htm", _feature.Items[1].Name);
            Assert.Equal("home1.html", _feature.Items[0].Name);
            _feature.MoveDown();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("home1.html", _feature.SelectedItem.Name);
            Assert.Equal("Default.htm", _feature.Items[0].Name);
            Assert.Equal("home1.html", _feature.Items[1].Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";
            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }
    }
}

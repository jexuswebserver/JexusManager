// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.Handlers
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.Handlers;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class HandlersFeatureSiteTestFixture
    {
        private HandlersFeature _feature;

        private ServerManager _server;

        private const string Current = @"applicationHost.config";

        public async Task SetUp()
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

            var module = new HandlersModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new HandlersFeature(module);
            _feature.Load();
        }

        [Fact]
        public async void TestBasic()
        {
            await this.SetUp();
            Assert.Equal(82, _feature.Items.Count);
            Assert.Equal("AXD-ISAPI-4.0_64bit", _feature.Items[0].Name);
        }

        [Fact]
        public async void TestRemoveInherited()
        {
            await this.SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("handlers");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "AXD-ISAPI-4.0_64bit");
            content.Add(remove);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("AXD-ISAPI-4.0_64bit", _feature.SelectedItem.Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(81, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRemove()
        {
            await this.SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            document.Save(Expected);

            var item = new HandlersItem(null);
            item.Name = "test";
            item.Path = "*";
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(83, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(82, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestEditInherited()
        {
            await this.SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("handlers");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "AXD-ISAPI-4.0_64bit");
            content.Add(remove);
            var add = new XElement("add");
            add.SetAttributeValue("allowPathInfo", "true");
            add.SetAttributeValue("modules", "IsapiModule");
            add.SetAttributeValue("name", "AXD-ISAPI-4.0_64bit");
            add.SetAttributeValue("path", "*.axd");
            add.SetAttributeValue("preCondition", "classicMode,runtimeVersionv4.0,bitness64");
            add.SetAttributeValue("responseBufferLimit", "0");
            add.SetAttributeValue("scriptProcessor", @"%windir%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll");
            add.SetAttributeValue("verb", "GET,HEAD,POST,DEBUG");
            content.Add(add);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("AXD-ISAPI-4.0_64bit", _feature.SelectedItem.Name);
            var item = _feature.SelectedItem;
            item.AllowPathInfo = true;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(true, _feature.SelectedItem.AllowPathInfo);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestEdit()
        {
            await this.SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("handlers");
            web.Add(content);
            var add = new XElement("add");
            add.SetAttributeValue("resourceType", "File");
            add.SetAttributeValue("allowPathInfo", "true");
            add.SetAttributeValue("modules", "");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "*");
            add.SetAttributeValue("verb", "*");
            content.Add(add);
            document.Save(Expected);

            var item = new HandlersItem(null);
            item.Name = "test";
            item.Path = "*";

            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(83, _feature.Items.Count);
            item.AllowPathInfo = true;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(true, _feature.SelectedItem.AllowPathInfo);
            Assert.Equal(83, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestAdd()
        {
            await this.SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("handlers");
            web.Add(content);
            var add = new XElement("add");
            add.SetAttributeValue("resourceType", "File");
            add.SetAttributeValue("modules", "");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "*");
            add.SetAttributeValue("verb", "*");
            content.Add(add);
            document.Save(Expected);

            var item = new HandlersItem(null);
            item.Name = "test";
            item.Path = "*";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Name);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRevert()
        {
            await SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            document.Save(Expected);

            var item = new HandlersItem(null);
            item.Name = "test";
            item.Path = "*";
            _feature.AddItem(item);

            _feature.Revert();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(82, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestMoveUp()
        {
            await SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("handlers");
            web.Add(content);
            var clear = new XElement("clear");
            content.Add(clear);
            var all = document.Root.XPathSelectElement("/configuration/location[@path='']/system.webServer/handlers");
            foreach (var element in all.Elements())
            {
                content.Add(element);
            }

            content.LastNode.Remove();

            var add = new XElement("add");
            add.SetAttributeValue("resourceType", "File");
            add.SetAttributeValue("modules", "");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "*");
            add.SetAttributeValue("verb", "*");
            content.Add(add);
            var one = new XElement("add");
            one.SetAttributeValue("modules", "StaticFileModule,DefaultDocumentModule,DirectoryListingModule");
            one.SetAttributeValue("name", "StaticFile");
            one.SetAttributeValue("path", "*");
            one.SetAttributeValue("requireAccess", "Read");
            one.SetAttributeValue("resourceType", "Either");
            one.SetAttributeValue("verb", "*");
            content.Add(one);
            document.Save(Expected);

            var item = new HandlersItem(null);
            item.Name = "test";
            item.Path = "*";
            _feature.AddItem(item);

            var last = 82;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[last];
            var expected = "test";
            Assert.Equal(expected, _feature.Items[last].Name);
            var original = "StaticFile";
            Assert.Equal(original, _feature.Items[previous].Name);
            _feature.MoveUp();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(expected, _feature.SelectedItem.Name);
            Assert.Equal(expected, _feature.Items[previous].Name);
            Assert.Equal(original, _feature.Items[last].Name);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestMoveDown()
        {
            await SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("handlers");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "StaticFile");
            content.Add(remove);
            var add = new XElement("add");
            add.SetAttributeValue("resourceType", "File");
            add.SetAttributeValue("modules", "");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "*");
            add.SetAttributeValue("verb", "*");
            content.Add(add);
            var one = new XElement("add");
            one.SetAttributeValue("modules", "StaticFileModule,DefaultDocumentModule,DirectoryListingModule");
            one.SetAttributeValue("name", "StaticFile");
            one.SetAttributeValue("path", "*");
            one.SetAttributeValue("requireAccess", "Read");
            one.SetAttributeValue("resourceType", "Either");
            one.SetAttributeValue("verb", "*");
            content.Add(one);
            document.Save(Expected);

            var item = new HandlersItem(null);
            item.Name = "test";
            item.Path = "*";
            _feature.AddItem(item);

            var last = 82;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[previous];
            var expected = "test";
            Assert.Equal(expected, _feature.Items[last].Name);
            var original = "StaticFile";
            Assert.Equal(original, _feature.Items[previous].Name);
            _feature.MoveDown();
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(original, _feature.SelectedItem.Name);
            Assert.Equal(expected, _feature.Items[previous].Name);
            Assert.Equal(original, _feature.Items[last].Name);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class ModulesFeatureSiteTestFixture
    {
        private ModulesFeature _feature;

        private ServerManager _server;

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

            var module = new ModulesModule();
            module.TestInitialize(serviceContainer, null);

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
        public void TestRemoveInherited()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root?.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("modules");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "DynamicCompressionModule");
            content.Add(remove);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("DynamicCompressionModule", _feature.SelectedItem.Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(43, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root?.Add(node);
            document.Save(Expected);

            var item = new ModulesItem(null);
            item.Name = "test";
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(45, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(44, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestEditInherited()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root?.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("modules");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "ScriptModule-4.0");
            content.Add(remove);
            var add = new XElement("add");
            add.SetAttributeValue("preCondition", "managedHandler,runtimeVersionv4.0");
            add.SetAttributeValue("name", "ScriptModule-4.0");
            add.SetAttributeValue("type", "test");
            content.Add(add);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[43];
            Assert.Equal("System.Web.Handlers.ScriptModule, System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35", _feature.SelectedItem.Type);
            var item = _feature.SelectedItem;
            item.Type = "test";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Type);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root?.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("modules");
            web.Add(content);
            var add = new XElement("add");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("type", "test");
            content.Add(add);
            document.Save(Expected);

            var item = new ModulesItem(null);
            item.Name = "test";
            item.Type = "test2";
            item.IsManaged = true;
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(45, _feature.Items.Count);
            item.Type = "test";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Type);
            Assert.Equal(45, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root?.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("modules");
            web.Add(content);
            var add = new XElement("add");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("type", "test1");
            content.Add(add);
            document.Save(Expected);

            var item = new ModulesItem(null);
            item.Name = "test";
            item.Type = "test1";
            item.IsManaged = true;
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Name);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestRevert()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root?.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            document.Save(Expected);

            var item = new ModulesItem(null);
            item.Name = "test";
            item.Type = "test1";
            item.IsManaged = true;
            _feature.AddItem(item);

            _feature.Revert();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(44, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestMoveUp()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("modules");
            web.Add(content);
            var clear = new XElement("clear");
            content.Add(clear);
            var all = document.Root.XPathSelectElement("/configuration/location[@path='']/system.webServer/modules");
            foreach (var element in all.Elements())
            {
                content.Add(element);
            }

            content.LastNode.Remove();

            var add = new XElement("add");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("type", "test1");
            content.Add(add);
            var one = new XElement("add");
            one.SetAttributeValue("preCondition", "managedHandler,runtimeVersionv4.0");
            one.SetAttributeValue("name", "ScriptModule-4.0");
            one.SetAttributeValue("type", "System.Web.Handlers.ScriptModule, System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            content.Add(one);
            document.Save(Expected);

            var item = new ModulesItem(null);
            item.Name = "test";
            item.Type = "test1";
            item.IsManaged = true;
            _feature.AddItem(item);

            var last = 44;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[last];
            var expected = "test";
            Assert.Equal(expected, _feature.Items[last].Name);
            var original = "ScriptModule-4.0";
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
        public void TestMoveDown()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("modules");
            web.Add(content);
            var clear = new XElement("remove");
            clear.SetAttributeValue("name", "ScriptModule-4.0");
            content.Add(clear);
            var add = new XElement("add");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("type", "test1");
            content.Add(add);
            var one = new XElement("add");
            one.SetAttributeValue("preCondition", "managedHandler,runtimeVersionv4.0");
            one.SetAttributeValue("name", "ScriptModule-4.0");
            one.SetAttributeValue("type", "System.Web.Handlers.ScriptModule, System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            content.Add(one);
            document.Save(Expected);

            var item = new ModulesItem(null);
            item.Name = "test";
            item.Type = "test1";
            item.IsManaged = true;
            _feature.AddItem(item);

            var last = 44;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[previous];
            var expected = "test";
            Assert.Equal(expected, _feature.Items[last].Name);
            var original = "ScriptModule-4.0";
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

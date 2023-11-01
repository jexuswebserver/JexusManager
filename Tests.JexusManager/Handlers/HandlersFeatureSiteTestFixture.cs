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

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using NSubstitute;

    public class HandlersFeatureSiteTestFixture
    {
        private HandlersFeature _feature;

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
            var substitute = Substitute.For<IManagementUIService>();
            substitute.ShowMessage(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<MessageBoxButtons>(),
                Arg.Any<MessageBoxIcon>(),
                Arg.Any<MessageBoxDefaultButton>()).Returns(DialogResult.Yes);

            serviceContainer.AddService(typeof(IManagementUIService), substitute);

            var module = new HandlersModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new HandlersFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(82, _feature.Items.Count);
            Assert.Equal("AXD-ISAPI-4.0_64bit", _feature.Items[0].Name);
        }

        [Fact]
        public void TestRemoveInherited()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("handlers",
                            new XElement("remove",
                                new XAttribute("name", "AXD-ISAPI-4.0_64bit"))))));
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
        public void TestRemove()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1")));
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
        public void TestEditInherited()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("handlers",
                            new XElement("remove",
                                new XAttribute("name", "AXD-ISAPI-4.0_64bit")),
                            new XElement("add",
                                new XAttribute("allowPathInfo", "true"),
                                new XAttribute("modules", "IsapiModule"),
                                new XAttribute("name", "AXD-ISAPI-4.0_64bit"),
                                new XAttribute("path", "*.axd"),
                                new XAttribute("preCondition", "classicMode,runtimeVersionv4.0,bitness64"),
                                new XAttribute("responseBufferLimit", "0"),
                                new XAttribute("scriptProcessor", @"%windir%\Microsoft.NET\Framework64\v4.0.30319\aspnet_isapi.dll"),
                                new XAttribute("verb", "GET,HEAD,POST,DEBUG"))))));
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("AXD-ISAPI-4.0_64bit", _feature.SelectedItem.Name);
            var item = _feature.SelectedItem;
            item.AllowPathInfo = true;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.True(_feature.SelectedItem.AllowPathInfo);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("handlers",
                            new XElement("add",
                                new XAttribute("resourceType", "File"),
                                new XAttribute("allowPathInfo", "true"),
                                new XAttribute("modules", ""),
                                new XAttribute("name", "test"),
                                new XAttribute("path", "*"),
                                new XAttribute("verb", "*"))))));
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
            Assert.True(_feature.SelectedItem.AllowPathInfo);
            Assert.Equal(83, _feature.Items.Count);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("handlers",
                            new XElement("add",
                                new XAttribute("resourceType", "File"),
                                new XAttribute("modules", ""),
                                new XAttribute("name", "test"),
                                new XAttribute("path", "*"),
                                new XAttribute("verb", "*"))))));
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
        public void TestRevert()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer")));
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
        public void TestMoveUp()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location",
                new XAttribute("path", "WebSite1"));
            document.Root?.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            var content = new XElement("handlers",
                new XElement("clear"));
            web.Add(content);
            var all = document.Root?.XPathSelectElement("/configuration/location[@path='']/system.webServer/handlers");
            if (all != null)
            {
                foreach (var element in all.Elements())
                {
                    content.Add(element);
                }
            }

            content.LastNode.Remove();

            var add = new XElement("add",
                new XAttribute("resourceType", "File"),
                new XAttribute("modules", ""),
                new XAttribute("name", "test"),
                new XAttribute("path", "*"),
                new XAttribute("verb", "*"));
            content.Add(add);
            var one = new XElement("add",
                new XAttribute("modules", "StaticFileModule,DefaultDocumentModule,DirectoryListingModule"),
                new XAttribute("name", "StaticFile"),
                new XAttribute("path", "*"),
                new XAttribute("requireAccess", "Read"),
                new XAttribute("resourceType", "Either"),
                new XAttribute("verb", "*"));
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
        public void TestMoveDown()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("handlers",
                            new XElement("remove",
                                new XAttribute("name", "StaticFile")),
                            new XElement("add",
                                new XAttribute("resourceType", "File"),
                                new XAttribute("modules", ""),
                                new XAttribute("name", "test"),
                                new XAttribute("path", "*"),
                                new XAttribute("verb", "*")),
                            new XElement("add",
                                new XAttribute("modules", "StaticFileModule,DefaultDocumentModule,DirectoryListingModule"),
                                new XAttribute("name", "StaticFile"),
                                new XAttribute("path", "*"),
                                new XAttribute("requireAccess", "Read"),
                                new XAttribute("resourceType", "Either"),
                                new XAttribute("verb", "*"))))));
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

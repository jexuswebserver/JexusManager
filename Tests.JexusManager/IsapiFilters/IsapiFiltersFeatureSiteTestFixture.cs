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

    public class IsapiFiltersFeatureSiteTestFixture
    {
        private IsapiFiltersFeature _feature;

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

            var module = new IsapiFiltersModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new IsapiFiltersFeature(module);
            _feature.Load();
        }

        [Fact]
        public async void TestBasic()
        {
            await this.SetUp();
            Assert.Equal(5, _feature.Items.Count);
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
            var content = new XElement("isapiFilters");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "ASP.Net_2.0.50727-64");
            content.Add(remove);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("ASP.Net_2.0.50727-64", _feature.SelectedItem.Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(4, _feature.Items.Count);

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

            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(6, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(5, _feature.Items.Count);

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
            var content = new XElement("isapiFilters");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "ASP.Net_2.0.50727-64");
            content.Add(remove);
            var add = new XElement("filter");
            add.SetAttributeValue("enableCache", "true");
            add.SetAttributeValue("preCondition", "bitness64,runtimeVersionv2.0");
            add.SetAttributeValue("name", "ASP.Net_2.0.50727-64");
            add.SetAttributeValue("path", "c:\\test.dll");
            content.Add(add);
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("ASP.Net_2.0.50727-64", _feature.SelectedItem.Name);
            var item = _feature.SelectedItem;
            item.Path = "c:\\test.dll";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("c:\\test.dll", _feature.SelectedItem.Path);

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
            var content = new XElement("isapiFilters");
            web.Add(content);
            var add = new XElement("filter");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "c:\\test.exe");
            content.Add(add);
            document.Save(Expected);

            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(6, _feature.Items.Count);
            item.Path = "c:\\test.exe";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("c:\\test.exe", _feature.SelectedItem.Path);
            Assert.Equal(6, _feature.Items.Count);

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
            var content = new XElement("isapiFilters");
            web.Add(content);
            var add = new XElement("filter");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "c:\\test.dll");
            content.Add(add);
            document.Save(Expected);

            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
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

            const string Expected = @"expected_revert.site.config";
            var document = XDocument.Load(Current);
            var node = new XElement("location");
            node.SetAttributeValue("path", "WebSite1");
            document.Root.Add(node);
            var web = new XElement("system.webServer");
            node.Add(web);
            document.Save(Expected);

            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Name);

            _feature.Revert();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(5, _feature.Items.Count);

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
            var content = new XElement("isapiFilters");
            web.Add(content);
            var clear = new XElement("clear");
            content.Add(clear);
            var one = new XElement("filter");
            one.SetAttributeValue("enableCache", "true");
            one.SetAttributeValue("name", "ASP.Net_2.0.50727-64");
            one.SetAttributeValue("path", @"%windir%\Microsoft.NET\Framework64\v2.0.50727\aspnet_filter.dll");
            one.SetAttributeValue("preCondition", "bitness64,runtimeVersionv2.0");
            content.Add(one);

            var two = new XElement("filter");
            two.SetAttributeValue("enableCache", "true");
            two.SetAttributeValue("name", "ASP.Net_2.0.50727.0");
            two.SetAttributeValue("path", @"%windir%\Microsoft.NET\Framework\v2.0.50727\aspnet_filter.dll");
            two.SetAttributeValue("preCondition", "bitness32,runtimeVersionv2.0");
            content.Add(two);

            var three = new XElement("filter");
            three.SetAttributeValue("enableCache", "true");
            three.SetAttributeValue("name", "ASP.Net_2.0_for_v1.1");
            three.SetAttributeValue("path", @"%windir%\Microsoft.NET\Framework\v2.0.50727\aspnet_filter.dll");
            three.SetAttributeValue("preCondition", "runtimeVersionv1.1");
            content.Add(three);

            var four = new XElement("filter");
            four.SetAttributeValue("enableCache", "true");
            four.SetAttributeValue("name", "ASP.Net_4.0_32bit");
            four.SetAttributeValue("path", @"%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_filter.dll");
            four.SetAttributeValue("preCondition", "bitness32,runtimeVersionv4.0");
            content.Add(four);

            var add = new XElement("filter");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "c:\\test.dll");
            content.Add(add);

            var six = new XElement("filter");
            six.SetAttributeValue("enableCache", "true");
            six.SetAttributeValue("name", "ASP.Net_4.0_64bit");
            six.SetAttributeValue("path", @"%windir%\Microsoft.NET\Framework64\v4.0.30319\aspnet_filter.dll");
            six.SetAttributeValue("preCondition", "bitness64,runtimeVersionv4.0");
            content.Add(six);

            document.Save(Expected);

            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);

            var last = 5;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[last];
            var expected = "test";
            Assert.Equal(expected, _feature.Items[last].Name);
            var original = "ASP.Net_4.0_64bit";
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
            var content = new XElement("isapiFilters");
            web.Add(content);
            var remove = new XElement("remove");
            remove.SetAttributeValue("name", "ASP.Net_4.0_64bit");
            content.Add(remove);

            var add = new XElement("filter");
            add.SetAttributeValue("name", "test");
            add.SetAttributeValue("path", "c:\\test.dll");
            content.Add(add);

            var six = new XElement("filter");
            six.SetAttributeValue("enableCache", "true");
            six.SetAttributeValue("name", "ASP.Net_4.0_64bit");
            six.SetAttributeValue("path", @"%windir%\Microsoft.NET\Framework64\v4.0.30319\aspnet_filter.dll");
            six.SetAttributeValue("preCondition", "bitness64,runtimeVersionv4.0");
            content.Add(six);

            document.Save(Expected);

            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);

            var last = 5;
            var previous = last - 1;
            _feature.SelectedItem = _feature.Items[previous];
            var expected = "test";
            Assert.Equal(expected, _feature.Items[last].Name);
            var original = "ASP.Net_4.0_64bit";
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

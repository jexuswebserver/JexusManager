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

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using NSubstitute;

    public class IsapiFiltersFeatureSiteTestFixture
    {
        private IsapiFiltersFeature _feature;

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

            var module = new IsapiFiltersModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new IsapiFiltersFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(5, _feature.Items.Count);
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
                        new XElement("isapiFilters",
                            new XElement("remove",
                                new XAttribute("name", "ASP.Net_2.0.50727-64"))))));
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
        public void TestRemove()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1")));
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
        public void TestEditInherited()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("isapiFilters",
                            new XElement("remove",
                                new XAttribute("name", "ASP.Net_2.0.50727-64")),
                            new XElement("filter",
                                new XAttribute("enableCache", "true"),
                                new XAttribute("preCondition", "bitness64,runtimeVersionv2.0"),
                                new XAttribute("name", "ASP.Net_2.0.50727-64"),
                                new XAttribute("path", "c:\\test.dll"))))));
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
        public void TestEdit()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("isapiFilters",
                            new XElement("filter",
                                new XAttribute("name", "test"),
                                new XAttribute("path", "c:\\test.exe"))))));
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
        public void TestAdd()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("isapiFilters",
                            new XElement("filter",
                                new XAttribute("name", "test"),
                                new XAttribute("path", "c:\\test.dll"))))));
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
        public void TestRevert()
        {
            SetUp();

            const string Expected = @"expected_revert.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer")));
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
        public void TestMoveUp()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("isapiFilters",
                            new XElement("clear"),
                            new XElement("filter",
                                new XAttribute("enableCache", "true"),
                                new XAttribute("name", "ASP.Net_2.0.50727-64"),
                                new XAttribute("path", @"%windir%\Microsoft.NET\Framework64\v2.0.50727\aspnet_filter.dll"),
                                new XAttribute("preCondition", "bitness64,runtimeVersionv2.0")),
                            new XElement("filter",
                                new XAttribute("enableCache", "true"),
                                new XAttribute("name", "ASP.Net_2.0.50727.0"),
                                new XAttribute("path", @"%windir%\Microsoft.NET\Framework\v2.0.50727\aspnet_filter.dll"),
                                new XAttribute("preCondition", "bitness32,runtimeVersionv2.0")),
                            new XElement("filter",
                                new XAttribute("enableCache", "true"),
                                new XAttribute("name", "ASP.Net_2.0_for_v1.1"),
                                new XAttribute("path", @"%windir%\Microsoft.NET\Framework\v2.0.50727\aspnet_filter.dll"),
                                new XAttribute("preCondition", "runtimeVersionv1.1")),
                            new XElement("filter",
                                new XAttribute("enableCache", "true"),
                                new XAttribute("name", "ASP.Net_4.0_32bit"),
                                new XAttribute("path", @"%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_filter.dll"),
                                new XAttribute("preCondition", "bitness32,runtimeVersionv4.0")),
                            new XElement("filter",
                                new XAttribute("name", "test"),
                                new XAttribute("path", "c:\\test.dll")),
                            new XElement("filter",
                                new XAttribute("enableCache", "true"),
                                new XAttribute("name", "ASP.Net_4.0_64bit"),
                                new XAttribute("path", @"%windir%\Microsoft.NET\Framework64\v4.0.30319\aspnet_filter.dll"),
                                new XAttribute("preCondition", "bitness64,runtimeVersionv4.0"))))));
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
        public void TestMoveDown()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                 new XAttribute("path", "WebSite1"),
                 new XElement("system.webServer",
                    new XElement("isapiFilters",
                        new XElement("remove",
                            new XAttribute("name", "ASP.Net_4.0_64bit")),
                        new XElement("filter",
                            new XAttribute("name", "test"),
                            new XAttribute("path", "c:\\test.dll")),
                        new XElement("filter",
                            new XAttribute("enableCache", "true"),
                            new XAttribute("name", "ASP.Net_4.0_64bit"),
                            new XAttribute("path", @"%windir%\Microsoft.NET\Framework64\v4.0.30319\aspnet_filter.dll"),
                            new XAttribute("preCondition", "bitness64,runtimeVersionv4.0"))))));
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

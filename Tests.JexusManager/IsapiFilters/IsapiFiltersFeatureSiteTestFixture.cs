﻿// Copyright (c) Lex Li. All rights reserved.
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

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("ASP.Net_2.0.50727-64", _feature.SelectedItem.Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(4, _feature.Items.Count);

            const string Expected = @"expected_remove.site.config";
            const string ExpectedMono = @"expected_remove.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRemove()
        {
            await this.SetUp();

            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Name);
            Assert.Equal(6, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(5, _feature.Items.Count);

            const string Expected = @"expected_remove1.site.config";
            const string ExpectedMono = @"expected_remove1.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestEditInherited()
        {
            await this.SetUp();

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("ASP.Net_2.0.50727-64", _feature.SelectedItem.Name);
            var item = _feature.SelectedItem;
            item.Path = "c:\\test.dll";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("c:\\test.dll", _feature.SelectedItem.Path);

            const string Expected = @"expected_edit.site.config";
            const string ExpectedMono = @"expected_edit.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestEdit()
        {
            await this.SetUp();

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

            const string Expected = @"expected_edit1.site.config";
            const string ExpectedMono = @"expected_edit1.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestAdd()
        {
            await this.SetUp();
            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Name);

            const string Expected = @"expected_add.site.config";
            const string ExpectedMono = @"expected_add.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRevert()
        {
            await SetUp();
            var item = new IsapiFiltersItem(null);
            item.Name = "test";
            item.Path = "c:\\test.dll";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Name);

            _feature.Revert();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(5, _feature.Items.Count);

            const string Expected = @"expected_revert.site.config";
            const string ExpectedMono = @"expected_revert.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestMoveUp()
        {
            await SetUp();

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

            const string Expected = @"expected_up.site.config";
            const string ExpectedMono = @"expected_up.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestMoveDown()
        {
            await SetUp();

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

            const string Expected = @"expected_up1.site.config";
            const string ExpectedMono = @"expected_up.site.mono.config";

            XmlAssert.Equal(Path.Combine("IsapiFilters", Helper.IsRunningOnMono() ? ExpectedMono : Expected), Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }
    }
}

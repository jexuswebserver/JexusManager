// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.MimeMap
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.MimeMap;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;

    public class MimeMapFeatureSiteTestFixture
    {
        private MimeMapFeature _feature;

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

            _server = new ServerManager(Current) { Mode = WorkingMode.IisExpress };

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

            var module = new MimeMapModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new MimeMapFeature(module);
            _feature.Load();
        }

        [Fact]
        public async void TestBasic()
        {
            await this.SetUp();
            Assert.Equal(374, _feature.Items.Count);
        }

        [Fact]
        public async void TestRemoveInherited()
        {
            await this.SetUp();

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal(".323", _feature.SelectedItem.FileExtension);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(373, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("MimeMap", "expected_remove.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRemove()
        {
            await this.SetUp();

            var item = new MimeMapItem(null);
            item.FileExtension = ".xl1";
            item.MimeType = "text/test";
            _feature.AddItem(item);

            Assert.Equal(".xl1", _feature.SelectedItem.FileExtension);
            Assert.Equal(375, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(374, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("MimeMap", "expected_remove1.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestEditInherited()
        {
            await this.SetUp();

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal(".323", _feature.SelectedItem.FileExtension);
            var item = _feature.SelectedItem;
            item.MimeType = "text/test";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("text/test", _feature.SelectedItem.MimeType);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("MimeMap", "expected_edit.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestEdit()
        {
            await this.SetUp();

            var item = new MimeMapItem(null);
            item.FileExtension = ".xl1";
            item.MimeType = "text/test";
            _feature.AddItem(item);

            Assert.Equal("text/test", _feature.SelectedItem.MimeType);
            Assert.Equal(375, _feature.Items.Count);
            item.MimeType = "text/test2";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("text/test2", _feature.SelectedItem.MimeType);
            Assert.Equal(375, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("MimeMap", "expected_edit1.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestAdd()
        {
            await this.SetUp();
            var item = new MimeMapItem(null);
            item.FileExtension = ".pp1";
            item.MimeType = "text/test";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(".pp1", _feature.SelectedItem.FileExtension);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("MimeMap", "expected_add.site.config"), Path.Combine("Website1", "web.config"));
        }
    }
}

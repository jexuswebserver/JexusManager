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

    public class DefaultDocumentFeatureSiteTestFixture
    {
        private DefaultDocumentFeature _feature;

        private ServerManager _server;

        private ServiceContainer _serviceContainer;

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
        public async void TestBasic()
        {
            await SetUp();
            Assert.Equal(7, _feature.Items.Count);
        }

        [Fact]
        public async void TestEnable()
        {
            await this.SetUp();
            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            Assert.True(_feature.IsEnabled);
            _feature.Disable();
            Assert.False(_feature.IsEnabled);

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("DefaultDocument", "expected_disabled.site.config"), Path.Combine("Website1", "web.config"));

            _feature.Enable();
            Assert.True(_feature.IsEnabled);

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRemoveInherited()
        {
            await this.SetUp();

            _feature.SelectedItem = _feature.Items[2];
            Assert.Equal("Default.asp", _feature.Items[2].Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal("index.htm", _feature.Items[2].Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("DefaultDocument", "expected_remove.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRemove()
        {
            await SetUp();

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("home1.html", _feature.Items[0].Name);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal("Default.htm", _feature.Items[0].Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("DefaultDocument", "expected_remove1.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestAdd()
        {
            await SetUp();
            var item = new DocumentItem(null);
            item.Name = "default.my";
            _feature.InsertItem(_feature.Items.FindIndex(i => i.Flag == "Local"), item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("default.my", _feature.SelectedItem.Name);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("DefaultDocument", "expected_add.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestRevert()
        {
            await SetUp();
            _feature.Revert();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(6, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("DefaultDocument", "expected_revert.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestMoveUp()
        {
            await SetUp();

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
            XmlAssert.Equal(Path.Combine("DefaultDocument", "expected_up1.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public async void TestMoveDown()
        {
            await SetUp();

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
            XmlAssert.Equal(Path.Combine("DefaultDocument", "expected_up.site.config"), Path.Combine("Website1", "web.config"));
        }
    }
}

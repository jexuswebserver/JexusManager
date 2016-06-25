// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.Caching
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.Caching;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;

    public class CachingFeatureSiteTestFixture
    {
        private CachingFeature _feature;

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
                    null,
                    _server.Sites[0].Name));

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

            var module = new CachingModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new CachingFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(1, _feature.Items.Count);
        }

        [Fact]
        public void TestRemoveInherited()
        {
            SetUp();

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal(".cs", _feature.SelectedItem.Extension);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(0, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("Caching", "expected_remove.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();

            var item = new CachingItem(null);
            item.Extension = ".xls";
            _feature.AddItem(item);

            Assert.Equal(".xls", _feature.SelectedItem.Extension);
            Assert.Equal(2, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(1, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("Caching", "expected_remove1.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestEditInherited()
        {
            SetUp();

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal(".cs", _feature.SelectedItem.Extension);
            var item = _feature.SelectedItem;
            item.Extension = ".vb";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(".vb", _feature.SelectedItem.Extension);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("Caching", "expected_edit.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();

            var item = new CachingItem(null);
            item.Extension = ".xls";
            _feature.AddItem(item);

            Assert.Equal(".xls", _feature.SelectedItem.Extension);
            Assert.Equal(2, _feature.Items.Count);
            item.Extension = ".xslt";
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(".xslt", _feature.SelectedItem.Extension);
            Assert.Equal(2, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("Caching", "expected_edit1.site.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();
            var item = new CachingItem(null);
            item.Extension = ".ppt";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(".ppt", _feature.SelectedItem.Extension);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(Path.Combine("Caching", "expected_add.site.config"), Path.Combine("Website1", "web.config"));
        }
    }
}

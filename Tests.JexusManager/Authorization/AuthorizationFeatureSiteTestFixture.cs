// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.Authorization
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.Authorization;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class AuthorizationFeatureSiteTestFixture
    {
        private AuthorizationFeature _feature;

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

            var module = new AuthorizationModule();
            module.TestInitialize(_serviceContainer, null);

            _feature = new AuthorizationFeature(module);
            _feature.Load();
        }

        [Fact]
        public async void TestBasic()
        {
            await this.SetUp();
            Assert.Equal(1, _feature.Items.Count);
        }

        [Fact]
        public async void TestRemoveInherited()
        {
            await this.SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = Path.Combine("Authorization", "expected_remove.site.config");
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            var security = new XElement("security");
            var authorization = new XElement("authorization");
            var remove = new XElement("remove");
            remove.SetAttributeValue("users", "*");
            node?.Add(security);
            security.Add(authorization);
            authorization.Add(remove);
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("*", _feature.SelectedItem.Users);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(0, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public async void TestRemove()
        {
            await this.SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = Path.Combine("Authorization", "expected_remove1.site.config");
            var document = XDocument.Load(site);
            document.Save(expected);

            var item = new AuthorizationRule(null);
            item.Roles = "test";
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Roles);
            Assert.Equal(2, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(1, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }


        [Fact]
        public async void TestEditInherited()
        {
            await this.SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = Path.Combine("Authorization", "expected_edit.site.config");
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            var security = new XElement("security");
            var authorization = new XElement("authorization");
            var remove = new XElement("remove");
            remove.SetAttributeValue("users", "*");
            var add = new XElement("add");
            add.SetAttributeValue("accessType", "Allow");
            add.SetAttributeValue("roles", "testers");
            add.SetAttributeValue("users", "*");
            node?.Add(security);
            security.Add(authorization);
            authorization.Add(remove);
            authorization.Add(add);
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("", _feature.SelectedItem.Roles);
            var item = _feature.SelectedItem;
            var expectedValue = "testers";
            item.Roles = expectedValue;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(expectedValue, _feature.SelectedItem.Roles);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public async void TestEdit()
        {
            await this.SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = Path.Combine("Authorization", "expected_edit1.site.config");
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            var security = new XElement("security");
            var authorization = new XElement("authorization");
            var add = new XElement("add");
            add.SetAttributeValue("accessType", "Allow");
            add.SetAttributeValue("roles", "defenders");
            node?.Add(security);
            security.Add(authorization);
            authorization.Add(add);
            document.Save(expected);

            var item = new AuthorizationRule(null);
            var original = "testers";
            item.Roles = original;
            _feature.AddItem(item);

            Assert.Equal(original, _feature.SelectedItem.Roles);
            Assert.Equal(2, _feature.Items.Count);
            var expectedValue = "defenders";
            item.Roles = expectedValue;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(expectedValue, _feature.SelectedItem.Roles);
            Assert.Equal(2, _feature.Items.Count);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }


        [Fact]
        public async void TestAdd()
        {
            await this.SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = Path.Combine("Authorization", "expected_add.site.config");
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            var security = new XElement("security");
            var authorization = new XElement("authorization");
            var add = new XElement("add");
            add.SetAttributeValue("roles", "test");
            add.SetAttributeValue("accessType", "Allow");
            node?.Add(security);
            security.Add(authorization);
            authorization.Add(add);
            document.Save(expected);

            var item = new AuthorizationRule(null);
            item.Roles = "test";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test", _feature.SelectedItem.Roles);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }
    }
}

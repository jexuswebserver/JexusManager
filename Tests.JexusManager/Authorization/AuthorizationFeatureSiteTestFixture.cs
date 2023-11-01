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

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using NSubstitute;

    public class AuthorizationFeatureSiteTestFixture
    {
        private AuthorizationFeature _feature;

        private ServerManager _server;

        private ServiceContainer _serviceContainer;

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

            _serviceContainer = new ServiceContainer();
            _serviceContainer.RemoveService(typeof(IConfigurationService));
            _serviceContainer.RemoveService(typeof(IControlPanel));
            var scope = ManagementScope.Site;
            _serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            _serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(null, _server.Sites[0].GetWebConfiguration(), scope, null, _server.Sites[0], null, null, null, _server.Sites[0].Name));

            _serviceContainer.RemoveService(typeof(IManagementUIService));
            var substitute = Substitute.For<IManagementUIService>();
            substitute.ShowMessage(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<MessageBoxButtons>(),
                Arg.Any<MessageBoxIcon>(),
                Arg.Any<MessageBoxDefaultButton>()).Returns(DialogResult.Yes);

            _serviceContainer.AddService(typeof(IManagementUIService), substitute);

            var module = new AuthorizationModule();
            module.TestInitialize(_serviceContainer, null);

            _feature = new AuthorizationFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Single(_feature.Items);
        }

        [Fact]
        public void TestRemoveInherited()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_remove.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("security",
                    new XElement("authorization",
                        new XElement("remove",
                            new XAttribute("users", "*")))));
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("*", _feature.SelectedItem.Users);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Empty(_feature.Items);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_remove1.site.config";
            var document = XDocument.Load(site);
            document.Save(expected);

            var item = new AuthorizationRule(null);
            item.Roles = "test";
            _feature.AddItem(item);

            Assert.Equal("test", _feature.SelectedItem.Roles);
            Assert.Equal(2, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Single(_feature.Items);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }


        [Fact]
        public void TestEditInherited()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_edit.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("security",
                    new XElement("authorization",
                        new XElement("remove",
                            new XAttribute("users", "*")),
                        new XElement("add",
                            new XAttribute("accessType", "Allow"),
                            new XAttribute("roles", "testers"),
                            new XAttribute("users", "*")))));
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
        public void TestEdit()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_edit1.site.config";
            var document = XDocument.Load(site);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("security",
                    new XElement("authorization",
                        new XElement("add",
                            new XAttribute("accessType", "Allow"),
                            new XAttribute("roles", "defenders")))));
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
        public void TestAdd()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_add.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("security",
                    new XElement("authorization",
                        new XElement("add",
                            new XAttribute("roles", "test"),
                            new XAttribute("accessType", "Allow")))));
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

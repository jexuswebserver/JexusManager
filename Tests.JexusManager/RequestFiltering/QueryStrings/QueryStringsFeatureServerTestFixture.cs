// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;
using System.Xml.XPath;

namespace Tests.RequestFiltering.QueryStrings
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.RequestFiltering;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;

    public class QueryStringsFeatureServerTestFixture
    {
        private QueryStringsFeature _feature;

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
            var scope = ManagementScope.Server;
            _serviceContainer.AddService(typeof(IControlPanel), new ControlPanel());
            _serviceContainer.AddService(typeof(IConfigurationService),
                new ConfigurationService(null, _server.GetApplicationHostConfiguration(), scope, _server, null, null, null, null, null));

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

            var module = new RequestFilteringModule();
            module.TestInitialize(_serviceContainer, null);

            _feature = new QueryStringsFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(2, _feature.Items.Count);
            Assert.Equal("test", _feature.Items[0].QueryString);
            Assert.True(_feature.Items[0].Allowed);
            Assert.Equal("test", _feature.Items[1].QueryString);
            Assert.False(_feature.Items[1].Allowed);
        }

        [Fact]
        public void TestRemove()
        {
            SetUp();
            const string Expected = @"expected_remove.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/security/requestFiltering/alwaysAllowedQueryStrings");
            node?.Remove();
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[0];
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Single(_feature.Items);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestRemoveDeny()
        {
            SetUp();
            const string Expected = @"expected_remove_deny.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/security/requestFiltering/denyQueryStringSequences");
            node?.Remove();
            document.Save(Expected);

            _feature.SelectedItem = _feature.Items[1];
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Single(_feature.Items);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestAdd()
        {
            SetUp();
            const string Expected = @"expected_add.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/security/requestFiltering/alwaysAllowedQueryStrings");
            node?.Add(
                new XElement("add",
                    new XAttribute("queryString", "test1")));
            document.Save(Expected);

            var item = new QueryStringsItem(null, true);
            item.QueryString = "test1";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test1", _feature.SelectedItem.QueryString);
            Assert.Equal(3, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void TestAddDeny()
        {
            SetUp();
            const string Expected = @"expected_add_deny.config";
            var document = XDocument.Load(Current);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer/security/requestFiltering/denyQueryStringSequences");
            node?.Add(
                new XElement("add",
                    new XAttribute("sequence", "test1")));
            document.Save(Expected);

            var item = new QueryStringsItem(null, false);
            item.QueryString = "test1";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal("test1", _feature.SelectedItem.QueryString);
            Assert.Equal(3, _feature.Items.Count);
            XmlAssert.Equal(Expected, Current);
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.Authentication
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.Authentication;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;
    using System.Xml.Linq;

    public class DigestAuthenticationFeatureSiteTestFixture
    {
        private DigestAuthenticationFeature _feature;

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

            var module = new AuthenticationModule();
            module.TestInitialize(_serviceContainer, null);

            _feature = new DigestAuthenticationFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.False(_feature.IsEnabled);
        }

        [Fact]
        public void TestDisable()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1")));
            document.Save(Expected);

            _feature.Disable();
            Assert.False(_feature.IsEnabled);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }

        [Fact]
        public void TestEnable()
        {
            SetUp();

            const string Expected = @"expected_add.site.config";
            var document = XDocument.Load(Current);
            document.Root?.Add(
                new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("security",
                            new XElement("authentication", 
                                new XElement("digestAuthentication",
                                    new XAttribute("enabled", true)))))));
            document.Save(Expected);

            _feature.Enable();
            Assert.True(_feature.IsEnabled);

            XmlAssert.Equal(Expected, Current);
            XmlAssert.Equal(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"));
        }
    }
}

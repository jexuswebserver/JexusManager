// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.DirectoryBrowse
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.DirectoryBrowse;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class DirectoryBrowseFeatureSiteTestFixture
    {
        private DirectoryBrowseFeature _feature;

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

            var module = new DirectoryBrowseModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new DirectoryBrowseFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.False(_feature.IsEnabled);
        }

        [Fact]
        public void TestEdit()
        {
            SetUp();

            var site = Path.Combine("Website1", "web.config");
            var expected = "expected_edit1.site.config";
            var document = XDocument.Load(site);
            var node = document.Root.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("directoryBrowse",
                    new XAttribute("enabled", "true"),
                    new XAttribute("showFlags", "LongDate")));
            document.Save(expected);

            _feature.IsEnabled = true;
            _feature.DateEnabled = _feature.ExtensionEnabled = _feature.SizeEnabled = _feature.TimeEnabled = false;
            _feature.LongDateEnabled = true;
            _feature.ApplyChanges();

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }
    }
}

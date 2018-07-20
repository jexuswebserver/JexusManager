// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Xml.Linq;
using System.Xml.XPath;

namespace Tests.HttpErrors
{
    using System;
    using System.ComponentModel.Design;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using global::JexusManager.Features.HttpErrors;
    using global::JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;
    using Microsoft.Web.Management.Server;

    using Moq;

    using Xunit;

    public class HttpErrorsFeatureSiteTestFixture
    {
        private HttpErrorsFeature _feature;

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

            var module = new HttpErrorsModule();
            module.TestInitialize(serviceContainer, null);

            _feature = new HttpErrorsFeature(module);
            _feature.Load();
        }

        [Fact]
        public void TestBasic()
        {
            SetUp();
            Assert.Equal(9, _feature.Items.Count);
        }

        [Fact]
        public void TestRemoveInherited()
        {
            SetUp();

            var expected = "expected_remove.site.config";
            var site = Path.Combine("Website1", "web.config");
            var document = XDocument.Load(site);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("httpErrors",
                    new XElement("remove",
                        new XAttribute("statusCode", "401"))));
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal(401U, _feature.SelectedItem.Status);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(8, _feature.Items.Count);

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

            var item = new HttpErrorsItem(null);
            item.Status = 455;
            item.Path = "c:\\test.htm";
            _feature.AddItem(item);

            Assert.Equal(455U, _feature.SelectedItem.Status);
            Assert.Equal(10, _feature.Items.Count);
            _feature.Remove();
            Assert.Null(_feature.SelectedItem);
            Assert.Equal(9, _feature.Items.Count);

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
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("httpErrors",
                    new XElement("remove",
                        new XAttribute("statusCode", "401")),
                    new XElement("error",
                        new XAttribute("prefixLanguageFilePath", "%IIS_BIN%\\custerr"),
                        new XAttribute("statusCode", "401"),
                        new XAttribute("path", "c:\\test.htm"))));
            document.Save(expected);

            _feature.SelectedItem = _feature.Items[0];
            Assert.Equal("401.htm", _feature.SelectedItem.Path);
            var item = _feature.SelectedItem;
            var expectedValue = "c:\\test.htm";
            item.Path = expectedValue;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(expectedValue, _feature.SelectedItem.Path);

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
            var expected = "expected_edit.site.config";
            var document = XDocument.Load(site);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("httpErrors",
                    new XElement("error",
                        new XAttribute("statusCode", "455"),
                        new XAttribute("path", "c:\\test1.htm"))));
            document.Save(expected);

            var item = new HttpErrorsItem(null);
            item.Status = 455;
            var original = "c:\\test.htm";
            item.Path = original;
            _feature.AddItem(item);

            Assert.Equal(original, _feature.SelectedItem.Path);
            Assert.Equal(10, _feature.Items.Count);
            var expectedValue = "c:\\test1.htm";
            item.Path = expectedValue;
            _feature.EditItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(expectedValue, _feature.SelectedItem.Path);
            Assert.Equal(10, _feature.Items.Count);

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
            var expected = "expected_edit.site.config";
            var document = XDocument.Load(site);
            var node = document.Root?.XPathSelectElement("/configuration/system.webServer");
            node?.Add(
                new XElement("httpErrors",
                    new XElement("error",
                        new XAttribute("statusCode", "455"),
                        new XAttribute("path", "c:\\test.htm"))));
            document.Save(expected);

            var item = new HttpErrorsItem(null);
            item.Status = 455;
            item.Path = "c:\\test.htm";
            _feature.AddItem(item);
            Assert.NotNull(_feature.SelectedItem);
            Assert.Equal(455U, _feature.SelectedItem.Status);

            const string Original = @"original.config";
            const string OriginalMono = @"original.mono.config";

            XmlAssert.Equal(Helper.IsRunningOnMono() ? OriginalMono : Original, Current);
            XmlAssert.Equal(expected, site);
        }
    }
}

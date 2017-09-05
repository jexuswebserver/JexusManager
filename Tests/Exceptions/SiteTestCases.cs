// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Tests.Exceptions
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using Microsoft.Web.Administration;
    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class SiteTestCases
    {
        [Fact]
        public void TestIisExpressInvalidSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.webServer");
                var security = new XElement("security");
                pool.Add(security);
                var authentication = new XElement("authentication");
                security.Add(authentication);
                var windows = new XElement("windowsAuthentication");
                authentication.Add(windows);
                windows.SetAttributeValue("enabled", true);
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<FileLoadException>(
                () =>
                    {
                        // enable Windows authentication
                        var windowsSection =
                            config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                    });
            Assert.Equal(
                string.Format(
                    "Filename: \\\\?\\{0}\r\nLine number: 11\r\nError: This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n\r\n",
                    siteConfig),
                exception.Message);
        }

        [Fact]
        public void TestIisExpressUnlockSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            
            {
                // add the section.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var windows = root.XPathSelectElement("/configuration/configSections/sectionGroup[@name='system.webServer']/sectionGroup[@name='security']/sectionGroup[@name='authentication']/section[@name='windowsAuthentication']");
                windows.SetAttributeValue("overrideModeDefault", "Allow");
                file.Save(Current);
            }

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.webServer");
                var security = new XElement("security");
                pool.Add(security);
                var authentication = new XElement("authentication");
                security.Add(authentication);
                var windows = new XElement("windowsAuthentication");
                authentication.Add(windows);
                windows.SetAttributeValue("enabled", true);
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            
            // enable Windows authentication
            var windowsSection =
                config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.True((bool)windowsSection["enabled"]);
        }

        [Fact]
        public void TestIisExpressUnlockSectionWithRuntimeTag()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the section.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var windows = root.XPathSelectElement(
                    "/configuration/configSections/sectionGroup[@name='system.webServer']/sectionGroup[@name='security']/sectionGroup[@name='authentication']/section[@name='windowsAuthentication']");
                windows.SetAttributeValue("overrideModeDefault", "Allow");
                file.Save(Current);
            }

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.webServer");
                var security = new XElement("security");
                pool.Add(security);
                var authentication = new XElement("authentication");
                security.Add(authentication);
                var windows = new XElement("windowsAuthentication");
                authentication.Add(windows);
                windows.SetAttributeValue("enabled", true);

                var tag = "<runtime>" +
                          "<asm:assemblyBinding xmlns:asm=\"urn:schemas-microsoft-com:asm.v1\">" +
                          "<asm:dependentAssembly>" +
                          "<asm:assemblyIdentity name=\"Newtonsoft.Json\" publicKeyToken=\"30ad4fe6b2a6aeed\" culture=\"neutral\" />" +
                          "<asm:bindingRedirect oldVersion=\"4.5.0.0-9.0.0.0\" newVersion=\"9.0.0.0\" />" +
                          "</asm:dependentAssembly>" +
                          "</asm:assemblyBinding>" +
                          "</runtime>";
                var runtime = XElement.Parse(tag);
                file.Root?.Add(runtime);
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();

            // enable Windows authentication
            var windowsSection =
                config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.True((bool) windowsSection["enabled"]);
#if IIS
            windowsSection.SetAttributeValue("enabled", false);
            var exception = Assert.Throws<COMException>(() => server.CommitChanges());
            Assert.Equal(0xc00cef03, (uint) exception.HResult);
            Assert.Equal("Exception from HRESULT: 0xC00CEF03", exception.Message);
#else
            server.CommitChanges();
#endif
        }


        [Fact]
        public void TestIisExpressUnknownSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.webServer");
                var unknown = new XElement("unknown");
                pool.Add(unknown);
                var test = new XElement("test");
                unknown.Add(test);
                test.SetAttributeValue("test", "test");
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<COMException>(
                () =>
                    {
                        // enable Windows authentication
                        var windowsSection =
                            config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                    });
#if IIS
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nError: \r\n", siteConfig), exception.Message);
#else
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nLine number: 10\r\nError: Unrecognized element 'test'\r\n\r\n", siteConfig), exception.Message);
#endif
        }

        [Fact]
        public void TestIisExpressDuplicateSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // modify the path
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var location = root.XPathSelectElement("/configuration/location[@path='WebSite2']");
                var newLocation = new XElement("location");
                location.AddAfterSelf(newLocation);
                newLocation.SetAttributeValue("path", "WebSite1");
                var webServer = new XElement("system.webServer");
                newLocation.Add(webServer);
                var document = new XElement("defaultDocument");
                document.SetAttributeValue("enabled", false);
                webServer.Add(document);
                var files = new XElement("files");
                document.Add(files);
                var add = new XElement("add");
                add.SetAttributeValue("value", "home1.html");
                files.Add(add);

                file.Save(Current);
            }

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var doc = root.XPathSelectElement("/configuration/system.webServer/defaultDocument");
                doc.SetAttributeValue("enabled", true);
                var add = root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add");
                add.SetAttributeValue("value", "home2.html");
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section =
                config.GetSection("system.webServer/defaultDocument");
            Assert.Equal(true, section["enabled"]);
            {
                var files = section.GetCollection("files");
                Assert.Equal(8, files.Count);
                Assert.Equal("home2.html", files[0]["value"]);
                Assert.True(files[0].IsLocallyStored);
                Assert.Equal("home1.html", files[1]["value"]);
                Assert.False(files[1].IsLocallyStored);

                files.RemoveAt(1);
            }

            server.CommitChanges();

            const string Expected = @"expected3.config";
            TestHelper.FixPhysicalPathMono(Expected);
            XmlAssert.Equal(Expected, Current);
            TestHelper.AssertSiteConfig(directoryName, Expected);
        }
    }
}

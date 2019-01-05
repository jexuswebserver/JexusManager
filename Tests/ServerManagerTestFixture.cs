// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Web.Administration;
using Xunit;

namespace Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    /// IIS version.
    /// </summary>
    public class ServerManagerTestFixture
    {
        [Fact]
        public void TestIisExpressMissingWebsiteConfig()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            string Expected = Path.Combine(directoryName, @"expected.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            File.Delete(TestHelper.GetSiteConfig(directoryName));

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            TestCases.TestIisExpressMissingWebsiteConfig(server);

            {
                // reorder entities to match IIS result.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.applicationHost/applicationPools/add[@name='Clr4IntegratedAppPool']");
                pool.SetAttributeValue("managedPipelineMode", "Integrated");
#if !IIS
                var windows = root.XPathSelectElement("/configuration/location[@path='WebSite2']/system.webServer/security/authentication/windowsAuthentication");
                windows?.SetAttributeValue("enabled", true);

                var security = root.XPathSelectElement("/configuration/location[@path='WebSite1']/system.webServer/security");
                security.Remove();
                var httpLogging = root.XPathSelectElement("/configuration/location[@path='WebSite1']/system.webServer/httpLogging");
                httpLogging.AddAfterSelf(security);

                var extended = windows.Element("extendedProtection");
                extended?.SetAttributeValue("flags", "None");
#endif
                file.Save(Current);
            }

            {
                // reorder entities to match IIS result.
                var file = XDocument.Load(TestHelper.GetSiteConfig(directoryName));
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var top = root.XPathSelectElement("/configuration/system.webServer/defaultDocument");
                var staticContent = root.XPathSelectElement("/configuration/system.webServer/staticContent");
                staticContent?.Remove();
                top.AddBeforeSelf(staticContent);
#if !IIS
                var rewrite = root.XPathSelectElement("/configuration/system.webServer/rewrite");
                var urlCompression = root.XPathSelectElement("/configuration/system.webServer/urlCompression");
                urlCompression.Remove();
                var httpErrors = root.XPathSelectElement("/configuration/system.webServer/httpErrors");
                httpErrors?.Remove();
                var security = root.XPathSelectElement("/configuration/system.webServer/security");
                security.Remove();
                rewrite.AddAfterSelf(httpErrors, urlCompression, security);
                httpErrors?.Element("error")?.SetAttributeValue("prefixLanguageFilePath", string.Empty);
                httpErrors?.Element("error")?.SetAttributeValue("responseMode", "File");

                // IMPORTANT: workaround an IIS issue.
                var document = root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add[@value='index.htm']");
                var item = new XElement("add",
                    new XAttribute("value", "index.html"));
                document?.AddAfterSelf(item);

                var clear = root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/clear");
                var remove = new XElement("remove",
                    new XAttribute("value", "index.html"));
                clear?.AddAfterSelf(remove);
#endif
                file.Save(TestHelper.GetSiteConfig(directoryName));
            }

            TestHelper.FixPhysicalPathMono(Expected);
            XmlAssert.Equal(Expected, Current);
            TestHelper.AssertSiteConfig(directoryName, "expected1.config");
        }

        [Fact]
        public void TestIisExpressLoad()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            string Expected = Path.Combine(directoryName, @"expected.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            TestHelper.CopySiteConfig(directoryName, "original.config");

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            TestCases.TestIisExpress(server, Current);

            {
                // reorder entities to match IIS result.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.applicationHost/applicationPools/add[@name='Clr4IntegratedAppPool']");
                pool.SetAttributeValue("managedPipelineMode", "Integrated");

                var windows = root.XPathSelectElement("/configuration/location[@path='WebSite2']/system.webServer/security/authentication/windowsAuthentication");
                windows.SetAttributeValue("enabled", true);

                var security = root.XPathSelectElement("/configuration/location[@path='WebSite1']/system.webServer/security");
                security.Remove();
                var httpLogging = root.XPathSelectElement("/configuration/location[@path='WebSite1']/system.webServer/httpLogging");
                httpLogging.AddAfterSelf(security);

                var extended = windows.Element("extendedProtection");
                extended?.SetAttributeValue("flags", "None");
#if IIS
                var asp = root.XPathSelectElement("/configuration/system.webServer/asp/comPlus");
                asp.Remove();
#endif
                file.Save(Current);
            }

            {
                // reorder entities to match IIS result.
                var file = XDocument.Load(TestHelper.GetSiteConfig(directoryName));
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var rewrite = root.XPathSelectElement("/configuration/system.webServer/rewrite");
                var urlCompression = root.XPathSelectElement("/configuration/system.webServer/urlCompression");
                urlCompression.Remove();
                var httpErrors = root.XPathSelectElement("/configuration/system.webServer/httpErrors");
                httpErrors?.Remove();
                var security = root.XPathSelectElement("/configuration/system.webServer/security");
                security.Remove();
                rewrite.AddAfterSelf(httpErrors, urlCompression, security);
                httpErrors?.Element("error")?.SetAttributeValue("prefixLanguageFilePath", string.Empty);
                httpErrors?.Element("error")?.SetAttributeValue("responseMode", "File");
#if !IIS
                // IMPORTANT: workaround an IIS issue.
                var document = root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add[@value='Default.asp']");
                var item = new XElement("add",
                    new XAttribute("value", "index.htm"));
                document?.AddAfterSelf(item);

                var clear = root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/clear");
                var remove = new XElement("remove",
                    new XAttribute("value", "index.htm"));
                clear?.AddAfterSelf(remove);
#endif
                file.Save(TestHelper.GetSiteConfig(directoryName));
            }

            TestHelper.FixPhysicalPathMono(Expected);
            XmlAssert.Equal(Expected, Current);
            TestHelper.AssertSiteConfig(directoryName, "expected.config");
        }

        [Fact]
        public void TestIisExpressLocking()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            string Expected = Path.Combine(directoryName, @"expected.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            TestHelper.CopySiteConfig(directoryName, "original.config");

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            TestCases.TestIisExpressHandlers(server);
        }

        [Fact]
        public void TestIisExpressLocation()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            string Expected = Path.Combine(directoryName, @"expected.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            TestHelper.CopySiteConfig(directoryName, "original.config");

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            TestCases.TestIisExpressLocation(server);
        }

        [Fact]
        public void TestIisExpressLocation2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            string Expected = Path.Combine(directoryName, @"expected.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            TestHelper.CopySiteConfig(directoryName, "original.config");

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            TestCases.TestIisExpressLocation2(server);
        }

        [Fact]
        public void TestIisExpressInheritance()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            string Expected = Path.Combine(directoryName, @"expected.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            TestHelper.CopySiteConfig(directoryName, "original.config");

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            TestCases.TestIisExpressInheritance(server);
        }

        [Fact]
        public void TestIisExpressSiteDefaults()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            string Expected = Path.Combine(directoryName, @"expected.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
            TestHelper.CopySiteConfig(directoryName, "original.config");

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            TestCases.TestIisSiteDefaults(server);

            // TODO: assert generated XML.
        }
    }
}

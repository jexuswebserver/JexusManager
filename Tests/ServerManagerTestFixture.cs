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
        public void MissingWebsiteConfig()
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
            TestCases.IisExpressMissingWebsiteConfig(server);

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
        public void Load()
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
            TestCases.IisExpress(server, Current);

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
        public void Locking()
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
            TestCases.IisExpressHandlers(server);
        }

        [Fact]
        public void Location()
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
            TestCases.IisExpressLocation(server);
        }

        [Fact]
        public void Location2()
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
            TestCases.IisExpressLocation2(server);
        }

        [Fact]
        public void Inheritance()
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
            TestCases.IisExpressInheritance(server);
        }

        [Fact]
        public void SiteDefaults()
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
            TestCases.IisSiteDefaults(server);

            // TODO: assert generated XML.
        }

        [Fact]
        public void PreloadEnabledInApplicationDefaults()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // set the flag to true
                var file = XDocument.Load(current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var site = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@name='GuessMeWeb']");
                site.Add(new XElement("applicationDefaults",
                                       new XAttribute("preloadEnabled", "true")));
                file.Save(current);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.GetApplicationHostConfiguration();
            Assert.Equal(true, server.Sites["GuessMeWeb"].ApplicationDefaults.GetAttributeValue("preloadEnabled"));
        }
    }
}

﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Text;

namespace Tests.Exceptions
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using Microsoft.Web.Administration;
    using Xunit;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.Collections.Generic;

    public class SiteTestCases
    {
        [Fact]
        public void InvalidSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var pool = root?.XPathSelectElement("/configuration/system.webServer");
                pool?.Add(
                    new XElement("security",
                        new XElement("authentication",
                            new XElement("windowsAuthentication",
                                new XAttribute("enabled", true)))));

                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<FileLoadException>(
                () =>
                    {
                        // enable Windows authentication
                        var unused =
                                config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                    });
            Assert.Equal(
                $"Filename: \\\\?\\{siteConfig}\r\nLine number: 11\r\nError: This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n\r\n",
                exception.Message);
        }

        [Fact]
        public void EmptyFile()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                File.WriteAllText(siteConfig, string.Empty);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            // enable Windows authentication
            var exception = Assert.Throws<COMException>(() => config.GetSection("system.webServer/security/authentication/windowsAuthentication"));
            Assert.Equal(
                $"Filename: \\\\?\\{siteConfig}\r\nLine number: 1\r\nError: Configuration file is not well-formed XML\r\n\r\n",
                exception.Message);
        }

        [Fact]
        public void EmptyTag()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var pool = root?.XPathSelectElement("/configuration/system.webServer");
                pool?.RemoveAll();
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            // enable Windows authentication
            var unused =
                config.GetSection("system.webServer/security/authentication/windowsAuthentication");
        }

        [Fact]
        public void EmptyTag2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var pool = root?.XPathSelectElement("/configuration/system.webServer");
                pool?.RemoveAll();
                pool?.Add(new XElement("security"));
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            // enable Windows authentication
            var unused =
                config.GetSection("system.webServer/security/authentication/windowsAuthentication");
        }

        [Fact]
        public void UnlockSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // add the section.
                var file = XDocument.Load(current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var windows = root.XPathSelectElement("/configuration/configSections/sectionGroup[@name='system.webServer']/sectionGroup[@name='security']/sectionGroup[@name='authentication']/section[@name='windowsAuthentication']");
                windows?.SetAttributeValue("overrideModeDefault", "Allow");
                file.Save(current);
            }

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var pool = root?.XPathSelectElement("/configuration/system.webServer");
                pool?.Add(
                    new XElement("security",
                        new XElement("authentication",
                            new XElement("windowsAuthentication",
                                new XAttribute("enabled", true)))));
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();

            // enable Windows authentication
            var windowsSection =
                config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.True((bool)windowsSection["enabled"]);
        }

        [Fact]
        public void UnlockSectionWithRuntimeTag()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // add the section.
                var file = XDocument.Load(current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var windows = root.XPathSelectElement(
                    "/configuration/configSections/sectionGroup[@name='system.webServer']/sectionGroup[@name='security']/sectionGroup[@name='authentication']/section[@name='windowsAuthentication']");
                windows?.SetAttributeValue("overrideModeDefault", "Allow");
                file.Save(current);
            }

            {
                // add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var pool = root?.XPathSelectElement("/configuration/system.webServer");
                pool?.Add(
                    new XElement("security",
                        new XElement("authentication",
                            new XElement("windowsAuthentication",
                                new XAttribute("enabled", true)))));

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
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();

            // enable Windows authentication
            var windowsSection =
                config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.True((bool)windowsSection["enabled"]);
#if IIS
            windowsSection.SetAttributeValue("enabled", false);
            var exception = Assert.Throws<COMException>(() => server.CommitChanges());
            Assert.Equal(0xc00cef03, (uint)exception.HResult);
            Assert.Equal("Exception from HRESULT: 0xC00CEF03", exception.Message);
#else
            server.CommitChanges();
#endif
        }


        [Fact]
        public void UnknownSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var pool = root?.XPathSelectElement("/configuration/system.webServer");
                var unknown = new XElement("unknown",
                    new XElement("test",
                        new XAttribute("test", "test")));
                pool?.Add(unknown);
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<COMException>(
                () =>
                    {
                        // enable Windows authentication
                        var unused =
                            config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                    });
#if IIS
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nError: \r\n", siteConfig), exception.Message);
#else
            Assert.Null(exception.Data["oob"]);
            Assert.Equal(
                $"Filename: \\\\?\\{siteConfig}\r\nLine number: 10\r\nError: Unrecognized element 'test'\r\n\r\n", exception.Message);
#endif
        }

        [Fact]
        public void ArrSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var pool = root?.XPathSelectElement("/configuration/system.webServer");
                var unknown = new XElement("webFarms",
                    new XElement("test",
                        new XAttribute("test", "test")));
                pool?.Add(unknown);
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<COMException>(
                () =>
                {
                    // enable Windows authentication
                    var unused =
                        config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                });
#if IIS
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nError: \r\n", siteConfig), exception.Message);
#else
            Assert.Equal("Application Request Routing Module (system.webServer/webFarms/)", exception.Data["oob"]);
            Assert.Equal("https://docs.microsoft.com/iis/extensions/configuring-application-request-routing-arr/define-and-configure-an-application-request-routing-server-farm#prerequisites", exception.Data["link"]);
            Assert.Equal(
                $"Filename: \\\\?\\{siteConfig}\r\nLine number: 10\r\nError: Unrecognized element 'test'\r\n\r\n", exception.Message);
#endif
        }

        [Fact]
        public void DuplicateSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // modify the path
                var file = XDocument.Load(current);
                var root = file.Root;
                var location = root?.XPathSelectElement("/configuration/location[@path='WebSite2']");
                var newLocation = new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("defaultDocument",
                            new XAttribute("enabled", false),
                            new XElement("files",
                                new XElement("add",
                                    new XAttribute("value", "home1.html"))))));
                location?.AddAfterSelf(newLocation);
                file.Save(current);
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
                doc?.SetAttributeValue("enabled", true);
                var add = root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add");
                add?.SetAttributeValue("value", "home2.html");
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
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

            const string expected = @"expected3.config";
            TestHelper.FixPhysicalPathMono(expected);
            XmlAssert.Equal(expected, current);
            TestHelper.AssertSiteConfig(directoryName, expected);
        }

        [Fact]
        public void DuplicateSectionDefinition()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // modify the path
                var file = XDocument.Load(current);
                var root = file.Root;
                var webSocket = root?.XPathSelectElement("/configuration/configSections/sectionGroup[@name='system.webServer']/section[@name='webSocket']");
                webSocket?.AddAfterSelf(
                    new XElement("section",
                        new XAttribute("name", "webSocket"),
                        new XAttribute("overrideModeDefault", "Allow")));
                file.Save(current);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var exception = Assert.Throws<COMException>(() =>
            {
                var config = server.Sites[0].Applications[0].GetWebConfiguration();
            });
            Assert.Equal($"Filename: \\\\?\\{current}\r\nLine number: 117\r\nError: There is a duplicate 'system.webServer/webSocket' section defined\r\n\r\n", exception.Message);
        }

        [Fact]
        public void DuplicateSectionDefinition2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root?.Add(
                    new XElement("configSections",
                        new XElement("sectionGroup",
                            new XAttribute("name", "system.webServer"),
                            new XElement("section",
                                new XAttribute("name", "webSocket"),
                                new XAttribute("overrideModeDefault", "Allow")))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<COMException>(() =>
            {
                var section = config.GetSection("system.webServer/defaultDocument");
            });
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nLine number: 10\r\nError: Only one <configSections> element allowed.  It must be the first child element of the root <configuration> element   \r\n\r\n", exception.Message);
        }

        [Fact]
        public void DuplicateSectionDefinition3()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root?.AddFirst(
                    new XElement("configSections",
                        new XElement("sectionGroup",
                            new XAttribute("name", "system.webServer"),
                            new XElement("section",
                                new XAttribute("name", "test"),
                                new XAttribute("overrideModeDefault", "Allow")))));
                root?.AddFirst(
                    new XElement("configSections",
                        new XElement("sectionGroup",
                            new XAttribute("name", "system.webServer"),
                            new XElement("section",
                                new XAttribute("name", "test2"),
                                new XAttribute("overrideModeDefault", "Allow")))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<COMException>(() =>
            {
                var section = config.GetSection("system.webServer/defaultDocument");
            });
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nLine number: 8\r\nError: Only one <configSections> element allowed.  It must be the first child element of the root <configuration> element   \r\n\r\n", exception.Message);
        }

        [Fact]
        public void DuplicateSectionDefinition4()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root?.AddFirst(
                    new XElement("configSections",
                        new XElement("sectionGroup",
                            new XAttribute("name", "system.webServer"),
                            new XElement("section",
                                new XAttribute("name", "webSocket"),
                                new XAttribute("overrideModeDefault", "Allow")))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<COMException>(() =>
            {
                var section = config.GetSection("system.webServer/defaultDocument");
            });
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nLine number: 5\r\nError: There is a duplicate 'system.webServer/webSocket' section defined\r\n\r\n", exception.Message);
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

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // modify the path
                var file = XDocument.Load(current);
                var root = file.Root;
                var location = root?.XPathSelectElement("/configuration/location[@path='WebSite2']");
                var newLocation = new XElement("location",
                    new XAttribute("path", "WebSite1"),
                    new XElement("system.webServer",
                        new XElement("defaultDocument",
                            new XAttribute("enabled", false),
                            new XElement("files",
                                new XElement("add",
                                    new XAttribute("value", "home1.html"))))));
                location?.AddAfterSelf(newLocation);
                file.Save(current);
            }

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                Debug.Assert(root != null, nameof(root) + " != null");
                var doc = root.XPathSelectElement("/configuration/system.webServer/defaultDocument");
                doc?.SetAttributeValue("enabled", true);
                var add = root.XPathSelectElement("/configuration/system.webServer/defaultDocument/files/add");
                add?.SetAttributeValue("value", "home2.html");

                root?.Add(
                    new XElement("location",
                        new XAttribute("path", "WebSite1/test"),
                        new XElement("system.webServer",
                            new XElement("defaultDocument",
                                new XElement("files",
                                    new XElement("add",
                                        new XAttribute("value", "home3.html")))))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();

            {
                var rootGroup = config.RootSectionGroup;
                Assert.Empty(rootGroup.Sections);
                Assert.Empty(rootGroup.SectionGroups);

                var effective = config.GetEffectiveSectionGroup();
                var buffer = new StringBuilder();
                foreach (var item in effective.Sections)
                {
                    buffer.AppendLine(item.Name);
                }

                Assert.True(21 <= effective.Sections.Count);
                Assert.Equal(12, effective.SectionGroups.Count);

                var list = new List<SectionDefinition>();
                effective.GetAllDefinitions(list);
                Assert.True(155 <= list.Count);
            }

            var serverConfig = server.GetApplicationHostConfiguration();
            {
                var rootGroup = serverConfig.RootSectionGroup;
                Assert.Empty(rootGroup.Sections);
                Assert.Equal(2, rootGroup.SectionGroups.Count);

                var list = new List<SectionDefinition>();
                rootGroup.GetAllDefinitions(list);
                Assert.Equal(55, list.Count);

                var effective = serverConfig.GetEffectiveSectionGroup();
                Assert.True(21 <= effective.Sections.Count);
                Assert.Equal(12, effective.SectionGroups.Count);
            }

            var section =
                config.GetSection("system.webServer/defaultDocument");
            Assert.Equal("system.webServer/defaultDocument", section.SectionPath);

            var childSection = config.GetSection("system.webServer/defaultDocument", "WebSite1/test");
            Assert.Equal("system.webServer/defaultDocument", childSection.SectionPath);
#if !IIS
            Assert.Equal("WebSite1", section.Location);
            Assert.EndsWith("web.config", section.FileContext.FileName);

            Assert.Equal("WebSite1/test", childSection.Location);
            Assert.EndsWith("web.config", childSection.FileContext.FileName);

            var handlersInWebConfig = childSection.GetParentElement().Section;
            Assert.Equal("system.webServer/defaultDocument", handlersInWebConfig.SectionPath);
            Assert.Equal("WebSite1", handlersInWebConfig.Location);
            Assert.EndsWith("web.config", handlersInWebConfig.FileContext.FileName);

            var handlersInWebSite = section.GetParentElement().Section;
            Assert.Equal("system.webServer/defaultDocument", handlersInWebSite.SectionPath);
            Assert.Equal("WebSite1", handlersInWebSite.Location);
            Assert.EndsWith("applicationHost.config", handlersInWebSite.FileContext.FileName);

            var handlersInEmpty = handlersInWebSite.GetParentElement().Section;
            Assert.Equal("system.webServer/defaultDocument", handlersInEmpty.SectionPath);
            Assert.Equal("", handlersInEmpty.Location);
            Assert.EndsWith("applicationHost.config", handlersInEmpty.FileContext.FileName);

            var handlersInNull = handlersInEmpty.GetParentElement().Section;
            Assert.Equal("system.webServer/defaultDocument", handlersInNull.SectionPath);
            Assert.Null(handlersInNull.Location);
            Assert.EndsWith("applicationHost.config", handlersInNull.FileContext.FileName);

            Assert.Null(handlersInNull.GetParentElement());

            var handlers2 = handlersInNull.GetCollection("files");
            Assert.Equal(6, handlers2.Count);
            var handlers1 = handlersInEmpty.GetCollection("files");
            Assert.Equal(6, handlers1.Count);
            var handlers3 = handlersInWebSite.GetCollection("files");
            Assert.Equal(7, handlers3.Count);

            var appHost = section.FileContext.Parent;
            Assert.EndsWith("applicationHost.config", appHost.FileName);

            var webRoot = appHost.Parent;
            Assert.EndsWith("web.config", webRoot.FileName);
            {
                var rootGroup = webRoot.RootSectionGroup;
                Assert.Empty(rootGroup.Sections);
                Assert.Empty(rootGroup.SectionGroups);
            }

            var machine = webRoot.Parent;
            Assert.EndsWith("machine.config", machine.FileName);
            {
                var rootGroup = machine.RootSectionGroup;
                Assert.True(21 <= rootGroup.Sections.Count);
                Assert.Equal(10, rootGroup.SectionGroups.Count);

                var list = new List<SectionDefinition>();
                rootGroup.GetAllDefinitions(list);
                Assert.True(100 <= list.Count);
            }
#endif
            {
                var handlers = section.GetCollection("files");
                Assert.Equal(8, handlers.Count);

                var childHandlers = childSection.GetCollection("files");
                Assert.Equal(9, childHandlers.Count);
            }

            var serverSection = serverConfig.GetSection("system.webServer/defaultDocument", "WebSite1");
            var serverChildSection = serverConfig.GetSection("system.webServer/defaultDocument", "WebSite1/test");

            {
                var handlers = serverSection.GetCollection("files");
                Assert.Equal(7, handlers.Count);

                var childHandlers = serverChildSection.GetCollection("files");
                Assert.Equal(7, childHandlers.Count);
            }
        }

        [Fact]
        public void UnlockedSection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var webServer = root?.XPathSelectElement("/configuration/system.webServer");
                webServer?.Add(
                    new XElement("handlers",
                        new XElement("add",
                            new XAttribute("name", "Python FastCGI"),
                            new XAttribute("path", "*"),
                            new XAttribute("verb", "*"),
                            new XAttribute("modules", "FastCgiModule"),
                            new XAttribute("scriptProcessor",
                                @"C:\Python36\python.exe|C:\Python36\Lib\site-packages\wfastcgi.py"),
                            new XAttribute("resourceType", "Unspecified"),
                            new XAttribute("requireAccess", "Script"))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section =
                config.GetSection("system.webServer/defaultDocument");
            Assert.Equal(true, section["enabled"]);
            {
                var files = section.GetCollection("files");
                Assert.Equal(7, files.Count);
            }
        }

        [Fact]
        public void ConfigSource()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // <directoryBrowse configSource="directorybrowse.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var webServer = root?.XPathSelectElement("/configuration/system.webServer");
                webServer?.Add(
                    new XElement("directoryBrowse",
                        new XAttribute("configSource", "directorybrowse.config")));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.webServer/directoryBrowse");
            Assert.Equal(true, section["enabled"]);
        }

        [Fact]
        public void ConfigSource2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // <authorization configSource="authorization.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var webServer = root?.XPathSelectElement("/configuration/system.web");
                webServer?.Add(
                    new XElement("authorization",
                        new XAttribute("configSource", "authorization.config")));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.web/authorization");
            var collection = section.GetCollection();
            Assert.Single(collection);

            var newItem = collection.CreateElement();
            Assert.Equal("allow", newItem.ElementTagName);
            newItem["users"] = "test";
            collection.Add(newItem);
            server.CommitChanges();
            /*
             *     <system.web>
        <authorization>
            <allow users="test" />
        </authorization>
    </system.web>
             */
            var content = File.ReadAllText(Path.Combine(Path.GetDirectoryName(siteConfig), "authorization.config"));
            {
                // <authorization configSource="authorization.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var webServer = root?.XPathSelectElement("/configuration/system.web");
                Assert.NotNull(webServer);
                var authorization = webServer.Element("authorization");
                Assert.NotNull(authorization);
                Assert.Single(authorization.Elements());
                var allow = authorization.Element("allow");
                Assert.NotNull(allow);
                Assert.Equal("test", allow.Attribute("users").Value);
            }

            Assert.Equal("<authorization>\r\n  <allow users=\"*\" />\r\n  <deny users=\"?\" />\r\n</authorization>\r\n", content);
        }

        [Fact]
        public void ConfigSource3()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // <directoryBrowse configSource="directorybrowse.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root.Add(
                    new XElement("system.web",
                        new XElement("authentication",
                            new XAttribute("configSource", "authentication.config"))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<COMException>(() => config.GetSection("system.web/authentication"));
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nLine number: 11\r\nError: Specified configSource cannot be parsed\r\n\r\n",
                exception.Message);
        }

        [Fact]
        public void Log4Net()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // <directoryBrowse configSource="directorybrowse.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root.AddFirst(
                    new XElement("configSections",
                        new XElement("section",
                            new XAttribute("name", "log4net"),
                            new XAttribute("type", "log4net.Config.Log4NetConfigurationSectionHandler, log4net"))));
                root.Add(
                    new XElement("log4net",
                        new XElement("root",
                            new XElement("level",
                                new XAttribute("value", "INFO")))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<FileNotFoundException>(() => config.GetSection("log4net"));
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nError: The configuration section 'log4net' cannot be read because it is missing schema\r\n\r\n",
                exception.Message);
            {
                var section = config.GetSection("system.webServer/defaultDocument");
                var files = section.GetCollection("files");
                Assert.Equal(7, files.Count);
            }
        }

        [Fact]
        public void Log4Net2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // <directoryBrowse configSource="directorybrowse.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root.AddFirst(
                    new XElement("configSections",
                        new XElement("section",
                            new XAttribute("name", "log4net"),
                            new XAttribute("type", "log4net.Config.Log4NetConfigurationSectionHandler, log4net"))));
                root.Add(
                    new XElement("log4net",
                        new XElement("root",
                            new XElement("level",
                                new XAttribute("value", "INFO")))));
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            {
                var section = config.GetSection("system.webServer/defaultDocument");
                var files = section.GetCollection("files");
                Assert.Equal(7, files.Count);
            }

            var exception = Assert.Throws<FileNotFoundException>(() => config.GetSection("log4net"));
#if IIS
            Assert.Equal($"Filename: \r\nError: The configuration section 'log4net' cannot be read because it is missing schema\r\n\r\n",
                exception.Message);
#else
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nError: The configuration section 'log4net' cannot be read because it is missing schema\r\n\r\n",
                exception.Message);
#endif
            {
                var section = config.GetSection("system.webServer/defaultDocument");
                var files = section.GetCollection("files");
                Assert.Equal(7, files.Count);
            }
        }

        [Fact]
        public void MissingSectionDefinition()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // modify the path
                var file = XDocument.Load(current);
                var root = file.Root;
                var defaultDocument = root?.XPathSelectElement("/configuration/configSections/sectionGroup[@name='system.webServer']/section[@name='defaultDocument']");
                defaultDocument?.Remove();
                file.Save(current);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var exception = Assert.Throws<COMException>(() =>
            {
                var config = server.Sites[0].Applications[0].GetWebConfiguration();
            });
#if IIS
            Assert.Equal($"Filename: \\\\?\\{current}\r\nError: \r\n", exception.Message);
#else
            Assert.Equal($"Filename: \\\\?\\{current}\r\nLine number: 286\r\nError: The configuration section 'system.webServer/defaultDocument' cannot be read because it is missing section definition\r\n\r\n", exception.Message);
#endif
        }

        [Fact]
        public void AspNetCore()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // <directoryBrowse configSource="directorybrowse.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root.Add(
                    new XElement("location",
                        new XAttribute("path", "."),
                        new XAttribute("inheritInChildApplications", "false"),
                        new XElement("system.webServer",
                            new XElement("handlers",
                                new XElement("add",
                                    new XAttribute("name", "aspNetCore"),
                                    new XAttribute("path", "*"),
                                    new XAttribute("verb", "*"),
                                    new XAttribute("modules", "AspNetCoreModule"),
                                    new XAttribute("resourceType", "Unspecified"))),
                            new XElement("aspNetCore",
                                new XAttribute("processPath", "dotnet"),
                                new XAttribute("arguments", ".\\testmvccore.dll"),
                                new XAttribute("stdoutLogEnabled", "false"),
                                new XAttribute("stdoutLogFile", ".\\logs\\stdout")))));
                file.Save(siteConfig);
            }

            {
                // modify the path
                var file = XDocument.Load(current);
                var root = file.Root;
                var webServer = root?.XPathSelectElement("/configuration/configSections/sectionGroup[@name='system.webServer']");
                webServer?.Add(
                    new XElement("section",
                        new XAttribute("name", "aspNetCore"),
                        new XAttribute("overrideModeDefault", "Allow")));
                file.Save(current);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            {
                var section = config.GetSection("system.webServer/defaultDocument");
                var files = section.GetCollection("files");
                Assert.Equal(7, files.Count);
            }
        }

        [Fact]
        public void LockAttributeModified()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // <directoryBrowse configSource="directorybrowse.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root.Add(
                    new XElement("location",
                        new XAttribute("path", "."),
                        new XAttribute("inheritInChildApplications", "false"),
                        new XElement("system.webServer",
                            new XElement("httpErrors",
                                new XAttribute("defaultPath", "unknown")
                                ))));
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<FileLoadException>(() => config.GetSection("system.webServer/httpErrors"));
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nLine number: 12\r\nError: Lock violation\r\n\r\n", exception.Message);
        }

        [Fact]
        public void LockAttributeModifiedWildcard()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // add the tags
                var file = XDocument.Load(current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var httpErrors = root.XPathSelectElement("/configuration/system.webServer/httpErrors");
                httpErrors?.SetAttributeValue("lockAttributes", "*");
                file.Save(current);
            }

            {
                // <directoryBrowse configSource="directorybrowse.config" />
                // Add the section.
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                root.Add(
                    new XElement("location",
                        new XAttribute("path", "."),
                        new XAttribute("inheritInChildApplications", "false"),
                        new XElement("system.webServer",
                            new XElement("httpErrors",
                                new XAttribute("defaultPath", "unknown")
                                ))));
                file.Save(siteConfig);
            }

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<FileLoadException>(() => config.GetSection("system.webServer/httpErrors"));
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nLine number: 12\r\nError: Lock violation\r\n\r\n", exception.Message);
        }

        [Fact]
        public void LockItemRemovedFromCollection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.webServer/modules");
            var collection = section.GetCollection();
            {
                Assert.Equal("DynamicCompressionModule", collection[0]["name"]);
                var exception = Assert.Throws<FileLoadException>(() => collection.RemoveAt(0)); // lockItem=true
                Assert.Equal($"Filename: \r\nError: Lock violation\r\n\r\n", exception.Message);
            }
        }

        [Fact]
        public void LockItemRemovedFromCollection2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.webServer/modules");
            var collection = section.GetCollection();
            var item = collection[collection.Count - 1];
            {
                Assert.Equal("ScriptModule-4.0", item["name"]);
                Assert.Equal(44, collection.Count);
                collection.Remove(item);
                Assert.Equal(43, collection.Count);
            }
        }

        [Fact]
        public void LockItemAddToCollection()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.webServer/modules");
            var collection = section.GetCollection();
            var newModule = collection.CreateElement();
            newModule["name"] = "RequestMonitorModule";
            collection.Add(newModule);
            collection.Remove(newModule);
        }

        [Fact]
        public void LockItemAddToCollection2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.webServer/modules");
            var collection = section.GetCollection();
            var newModule = collection.CreateElement();
            newModule["name"] = "RequestMonitorModule";
            // TODO: investigate how to behave the same.
#if IIS
            var exception = Assert.Throws<FileLoadException>(() => collection.AddAt(0, newModule));
            Assert.Equal("Filename: \r\nError: This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n\r\n", exception.Message);
#else            
            collection.AddAt(0, newModule);
            collection.Remove(newModule);
#endif
        }

        [Fact]
        public void LockItemRemovedInLocationTag()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // modify the path
                var file = XDocument.Load(current);
                var root = file.Root;
                root.Add(
                    new XElement("location",
                        new XAttribute("path", "WebSite1"),
                        new XElement("system.webServer",
                            new XElement("modules",
                                new XElement("remove",
                                    new XAttribute("name", "DynamicCompressionModule")))))
                    );
                file.Save(current);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.webServer/modules");
            var collection = section.GetCollection();
            var item = collection[0];
            Assert.Equal("StaticCompressionModule", item["name"]);
        }

        [Fact]
        public void LockItemRemovedInWebConfig()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // modify the path
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var webServer = root.Element("system.webServer");
                webServer.Add(
                    new XElement("modules",
                        new XElement("remove",
                            new XAttribute("name", "DynamicCompressionModule")))
                    );
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var exception = Assert.Throws<FileLoadException>(() => config.GetSection("system.webServer/modules"));
            Assert.Equal($"Filename: \\\\?\\{siteConfig}\r\nLine number: 10\r\nError: Lock violation\r\n\r\n", exception.Message);
        }

        [Fact]
        public void LockItemRemovedInWebConfig2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string current = Path.Combine(directoryName, @"applicationHost.config");
            string original = Path.Combine(directoryName, @"original2.config");
            var siteConfig = TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(original, current, true);
            TestHelper.FixPhysicalPathMono(current);

            {
                // modify the path
                var file = XDocument.Load(siteConfig);
                var root = file.Root;
                var webServer = root.Element("system.webServer");
                webServer.Add(
                    new XElement("modules",
                        new XElement("remove",
                            new XAttribute("name", "ScriptModule-4.0")))
                    );
                file.Save(siteConfig);
            }
#if IIS
            var server = new ServerManager(current);
#else
            var server = new IisExpressServerManager(current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section = config.GetSection("system.webServer/modules");
            Assert.Equal(43, section.GetCollection().Count);
        }
    }
}

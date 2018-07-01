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
    using System.Linq;
    using System.Net;

    public class ServerTestCases
    {
        [Fact]
        public void TestProviders()
        {
            const string Current = @"applicationHost.config";
            const string Original = @"original2.config";
            const string OriginalMono = @"original.mono.config";
            File.Copy(Helper.IsRunningOnMono() ? OriginalMono : Original, Current, true);

            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }
#if IIS
            var server = new ServerManager(Path.Combine(directoryName, Current));
#else
            var server = new IisExpressServerManager(Path.Combine(directoryName, Current));
#endif
            var config = server.GetApplicationHostConfiguration();
            var section = config.GetSection("configProtectedData");
            Assert.Equal("RsaProtectedConfigurationProvider", section["defaultProvider"]);
            var collection = section.GetCollection("providers");
            Assert.Equal(5, collection.Count);
        }

        [Fact]
        public void TestIisExpressMissingFile()
        {
            const string Original = @"applicationHost.config";
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            var file = Path.Combine(directoryName, Original);
            File.Delete(file);

#if IIS
            var server = new ServerManager(file);
#else
            var server = new IisExpressServerManager(file);
#endif
            var exception = Assert.Throws<FileNotFoundException>(
                () =>
                    {
                        TestCases.TestIisExpress(server);
                    });
            Assert.Equal(
                string.Format("Filename: \\\\?\\{0}\r\nError: Cannot read configuration file\r\n\r\n", file),
                exception.Message);
        }

        [Fact]
        public void TestIisExpressMissingClosingTag()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(@"original2_missing_closing.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            //TestHelper.FixPhysicalPathMono(Current);

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var exception = Assert.Throws<COMException>(
                () =>
                    {
                        TestCases.TestIisExpress(server);
                    });
            Assert.Equal(
                string.Format(
                    "Filename: \\\\?\\{0}\r\nLine number: 1135\r\nError: Configuration file is not well-formed XML\r\n\r\n",
                    Current),
                exception.Message);
        }

        [Fact]
        public void TestIisExpressMissingRequiredAttribute()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // Remove the attribute.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.applicationHost/applicationPools/add[@name='UnmanagedClassicAppPool']");
                pool?.SetAttributeValue("name", null);
                file.Save(Current);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var exception = Assert.Throws<COMException>(
                () =>
                    {
                        TestCases.TestIisExpress(server);
                    });
            Assert.Equal(
                string.Format(
                    "Filename: \\\\?\\{0}\r\nLine number: 142\r\nError: Missing required attribute 'name'\r\n\r\n",
                    Current),
                exception.Message);
        }

        [Fact]
        public void TestIisExpressValidatorFails()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // Remove the attribute.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.applicationHost/applicationPools/add[@name='UnmanagedClassicAppPool']");
                pool?.SetAttributeValue("name", string.Empty);
                file.Save(Current);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var exception = Assert.Throws<COMException>(
                () =>
                    {
                        TestCases.TestIisExpress(server);
                    });
            Assert.Equal(
                string.Format(
                    "Filename: \\\\?\\{0}\r\nLine number: 142\r\nError: The 'name' attribute is invalid.  Invalid application pool name\r\n\r\n\r\n",
                    Current),
                exception.Message);
        }

        [Fact]
        public void TestIisExpressInvalidAttribute()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // Remove the attribute.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pool = root.XPathSelectElement("/configuration/system.applicationHost/applicationPools/add[@name='UnmanagedClassicAppPool']");
                pool?.SetAttributeValue("testAuto", true);
                file.Save(Current);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var exception = Assert.Throws<COMException>(
                () =>
                    {
                        TestCases.TestIisExpress(server);
                    });
            Assert.Equal(
                string.Format(
                    "Filename: \\\\?\\{0}\r\nLine number: 142\r\nError: Unrecognized attribute 'testAuto'\r\n\r\n",
                    Current),
                exception.Message);
        }

        [Fact]
        public void TestIisExpressReadOnly()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            var message =
                "The configuration object is read only, because it has been committed by a call to ServerManager.CommitChanges(). If write access is required, use ServerManager to get a new reference.";
#if IIS
            var server = new ServerManager(true, Current);
#else
            var server = new IisExpressServerManager(true, Current);
#endif
            var exception1 = Assert.Throws<InvalidOperationException>(
                () =>
                    {
                        TestCases.TestIisExpress(server);
                    });
            Assert.Equal(message, exception1.Message);

            // site config "Website1"
            var config = server.Sites[0].Applications[0].GetWebConfiguration();

            // enable Windows authentication
            var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
            Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
            Assert.True(windowsSection.IsLocked);
            Assert.False(windowsSection.IsLocallyStored);

            var windowsEnabled = (bool)windowsSection["enabled"];
            Assert.False(windowsEnabled);

            var compression = config.GetSection("system.webServer/urlCompression");
            Assert.Equal(OverrideMode.Inherit, compression.OverrideMode);
            Assert.Equal(OverrideMode.Allow, compression.OverrideModeEffective);
            Assert.False(compression.IsLocked);
            Assert.False(compression.IsLocallyStored);

            Assert.Equal(true, compression["doDynamicCompression"]);

            var compress = Assert.Throws<InvalidOperationException>(() => compression["doDynamicCompression"] = false);
            Assert.Equal(message, compress.Message);

            {
                // disable default document. Saved to web.config as this section can be overridden anywhere.
                ConfigurationSection defaultDocumentSection = config.GetSection("system.webServer/defaultDocument");
                Assert.Equal(true, defaultDocumentSection["enabled"]);

                ConfigurationElementCollection filesCollection = defaultDocumentSection.GetCollection("files");
                Assert.Equal(7, filesCollection.Count);

                {
                    var first = filesCollection[0];
                    Assert.Equal("home1.html", first["value"]);
                    Assert.True(first.IsLocallyStored);
                }

                var second = filesCollection[1];
                Assert.Equal("Default.htm", second["value"]);
                Assert.False(second.IsLocallyStored);

                var third = filesCollection[2];
                Assert.Equal("Default.asp", third["value"]);
                Assert.False(third.IsLocallyStored);

                var remove = Assert.Throws<FileLoadException>(() => filesCollection.RemoveAt(4));
                Assert.Equal(
                    "Filename: \r\nError: This configuration section cannot be modified because it has been opened for read only access\r\n\r\n",
                    remove.Message);

                ConfigurationElement addElement = filesCollection.CreateElement();
                var add = Assert.Throws<InvalidOperationException>(() => filesCollection.AddAt(0, addElement));
                Assert.Equal(message, add.Message);

                Assert.Equal(7, filesCollection.Count);

                {
                    var first = filesCollection[0];
                    Assert.Equal("home1.html", first["value"]);
                    // TODO: why?
                    // Assert.IsFalse(first.IsLocallyStored);
                }

                Assert.Equal(7, filesCollection.Count);

                var clear = Assert.Throws<InvalidOperationException>(() => filesCollection.Clear());
                Assert.Equal(message, clear.Message);

                var delete = Assert.Throws<InvalidOperationException>(() => filesCollection.Delete());
                Assert.Equal(message, delete.Message);
            }
        }
        
        [Fact]
        public void TestIisExpressNoBinding()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // change the path.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var app = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='1']/bindings");
                app.Remove();
                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var site = server.Sites[0];
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var binding = site.Bindings[0];
                });
        }


        [Fact]
        public void TestIisExpressNoRootApplication()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // change the path.
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var app = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='1']/application");
                app?.SetAttributeValue("path", "/xxx");
                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var site = server.Sites[0];
            var config = site.GetWebConfiguration();
            var exception = Assert.Throws<FileNotFoundException>(
                () =>
                {
                    var root = config.RootSectionGroup;
                });
#if IIS
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/WebSite1'\r\n\r\n", Current), exception.Message);
#else
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nLine number: 155\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/WebSite1'\r\n\r\n", Current), exception.Message);
#endif
        }

        [Fact]
        public void TestIisExpressRootApplicationOutOfOrder()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the tags
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var app = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='1']/application");
                var newApp = new XElement("application",
                    new XAttribute("path", "/xxx"));
                app.AddBeforeSelf(newApp);
                var vDir = new XElement("virtualDirectory",
                    new XAttribute("path", "/"),
                    new XAttribute("physicalPath", @"%JEXUS_TEST_HOME%\WebSite1"));
                newApp.Add(vDir);
                file.Save(Current);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var site = server.Sites[0];
            var config = site.GetWebConfiguration();
            {
                var root = config.RootSectionGroup;
                Assert.NotNull(root);
            }

            // enable Windows authentication
            var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
            Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
            Assert.True(windowsSection.IsLocked);
            Assert.False(windowsSection.IsLocallyStored);
        }

        [Fact]
        public void TestIisExpressNoRootVirtualDirectory()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
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

                var vDir = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='1']/application/virtualDirectory");
                vDir?.SetAttributeValue("path", "/xxx");
                file.Save(Current);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var site = server.Sites[0];
            var config = site.GetWebConfiguration();
#if IIS
            var exception = Assert.Throws<NullReferenceException>(
                () =>
                {
                    var root = config.RootSectionGroup;
                });
#else
            var exception = Assert.Throws<FileNotFoundException>(
                () =>
                {
                    var root = config.RootSectionGroup;
                });
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nLine number: 156\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/WebSite1'\r\n\r\n", Current), exception.Message);
#endif
        }

        [Fact]
        public void TestIisExpressRootVirtualDirectoryOutOfOrder()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
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

                var vDir = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='1']/application/virtualDirectory");
                var newDir = new XElement("virtualDirectory",
                    new XAttribute("path", "/xxx"),
                    new XAttribute("physicalPath", @"%JEXUS_TEST_HOME%\WebSite1"));
                vDir.AddBeforeSelf(newDir);
                file.Save(Current);
            }

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var site = server.Sites[0];
            var config = site.GetWebConfiguration();
            {
                var root = config.RootSectionGroup;
                Assert.NotNull(root);
            }

            // enable Windows authentication
            var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
            Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
            Assert.True(windowsSection.IsLocked);
            Assert.False(windowsSection.IsLocallyStored);
        }

        [Fact]
        public void TestIisExpressLogFileInheritance()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the tags
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var site1 = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='2']");
                var log = new XElement("logFile",
                    new XAttribute("logFormat", "IIS"));
                site1.Add(log);

                var site2 = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='3']");
                var log2 = new XElement("logFile",
                    new XAttribute("directory", @"%IIS_USER_HOME%\Logs\1"));
                site2.Add(log2);

                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            {
                var site = server.Sites[0];
                Assert.Equal(@"%IIS_USER_HOME%\Logs", site.LogFile.Directory);
                Assert.Equal(LogFormat.W3c, site.LogFile.LogFormat);
            }

            {
                var site = server.Sites[1];
                Assert.Equal(@"%IIS_USER_HOME%\Logs", site.LogFile.Directory);
                Assert.Equal(LogFormat.Iis, site.LogFile.LogFormat);
            }

            {
                var site = server.Sites[2];
                Assert.Equal(@"%IIS_USER_HOME%\Logs\1", site.LogFile.Directory);
                Assert.Equal(LogFormat.W3c, site.LogFile.LogFormat);
            }
        }

        [Fact]
        public void TestIisExpressDuplicateBindings()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the tags
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var site1 = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='2']/bindings");
                var binding = new XElement("binding",
                    new XAttribute("protocol", "http"),
                    new XAttribute("bindingInformation", "*:61902:localhost"));
                site1.Add(binding);
                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            {
                var exception = Assert.Throws<COMException>(() => server.Sites[1]);
                Assert.Equal($"Filename: \\\\?\\{Current}\r\nLine number: 183\r\nError: Cannot add duplicate collection entry of type 'binding' with combined key attributes 'protocol, bindingInformation' respectively set to 'http, *:61902:localhost'\r\n\r\n", exception.Message);
            }
        }

        [Fact]
        public void TestIisExpressBindingInvalidPort()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the tags
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var site1 = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='2']/bindings");
                var binding = new XElement("binding",
                    new XAttribute("protocol", "http"),
                    new XAttribute("bindingInformation", "*:161902:localhost"));
                site1.Add(binding);
                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            {
                Assert.Null(server.Sites[1].Bindings[2].EndPoint);
                Assert.Equal("*:161902:localhost", server.Sites[1].Bindings[2].BindingInformation);
            }
        }

        [Fact]
        public void TestIisExpressBindingInvalidAddress()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the tags
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var site1 = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='2']/bindings");
                var binding = new XElement("binding",
                    new XAttribute("protocol", "http"),
                    new XAttribute("bindingInformation", "1.1.1.1.1:61902:localhost"));
                site1.Add(binding);
                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            {
                Assert.Null(server.Sites[1].Bindings[2].EndPoint);
                Assert.Equal("1.1.1.1.1:61902:localhost", server.Sites[1].Bindings[2].BindingInformation);
            }
        }

        [Fact]
        public void TestIisExpressBindingInvalidAddress2()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the tags
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var site1 = root.XPathSelectElement("/configuration/system.applicationHost/sites/site[@id='2']/bindings");
                var binding = new XElement("binding",
                    new XAttribute("protocol", "http"),
                    new XAttribute("bindingInformation", "1.1.1:61902:localhost"));
                site1.Add(binding);
                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            {
                Assert.Equal(IPAddress.Parse("1.1.0.1"), server.Sites[1].Bindings[2].EndPoint.Address);
                Assert.Equal("1.1.1:61902:localhost", server.Sites[1].Bindings[2].BindingInformation);
            }
        }

        [Fact]
        public void TestIisExpressDuplicateApplicationPools()
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.SetEnvironmentVariable("JEXUS_TEST_HOME", directoryName);

            if (directoryName == null)
            {
                return;
            }

            string Current = Path.Combine(directoryName, @"applicationHost.config");
            string Original = Path.Combine(directoryName, @"original2.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

            {
                // add the tags
                var file = XDocument.Load(Current);
                var root = file.Root;
                if (root == null)
                {
                    return;
                }

                var pools = root.XPathSelectElement("/configuration/system.applicationHost/applicationPools");
                var pool = new XElement("add",
                    new XAttribute("name", "Clr4IntegratedAppPool"));
                pools.Add(pool);
                file.Save(Current);
            }
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            {
                var exception = Assert.Throws<COMException>(() => server.ApplicationPools);
                Assert.Equal($"Filename: \\\\?\\{Current}\r\nLine number: 144\r\nError: Cannot add duplicate collection entry of type 'add' with unique key attribute 'name' set to 'Clr4IntegratedAppPool'\r\n\r\n", exception.Message);
            }
        }
    }
}

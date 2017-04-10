// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Tests
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    using Microsoft.Web.Administration;
    using Xunit;

    public class ExceptionTestCases
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
            string Original = Path.Combine(directoryName, @"original2_missing_required.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
                    "Filename: \\\\?\\{0}\r\nLine number: 149\r\nError: Missing required attribute 'name'\r\n\r\n",
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
            string Original = Path.Combine(directoryName, @"original2_validator_fails.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
                    "Filename: \\\\?\\{0}\r\nLine number: 149\r\nError: The 'name' attribute is invalid.  Invalid application pool name\r\n\r\n\r\n",
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
            string Original = Path.Combine(directoryName, @"original2_invalid_attribute.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
                    "Filename: \\\\?\\{0}\r\nLine number: 143\r\nError: Unrecognized attribute 'testAuto'\r\n\r\n",
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
            Assert.Equal(true, windowsSection.IsLocked);
            Assert.Equal(false, windowsSection.IsLocallyStored);

            var windowsEnabled = (bool)windowsSection["enabled"];
            Assert.False(windowsEnabled);

            var compression = config.GetSection("system.webServer/urlCompression");
            Assert.Equal(OverrideMode.Inherit, compression.OverrideMode);
            Assert.Equal(OverrideMode.Allow, compression.OverrideModeEffective);
            Assert.Equal(false, compression.IsLocked);
            Assert.Equal(false, compression.IsLocallyStored);

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
            TestHelper.CopySiteConfig(directoryName, "original_invalid_section.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
            var siteConfig = Path.Combine(directoryName, "WebSite1", "web.config");
            Assert.Equal(
                string.Format(
                    "Filename: \\\\?\\{0}\r\nLine number: 11\r\nError: This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n\r\n",
                    siteConfig),
                exception.Message);
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
            TestHelper.CopySiteConfig(directoryName, "original_unknown_section.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
            var siteConfig = Path.Combine(directoryName, "WebSite1", "web.config");
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
            string Original = Path.Combine(directoryName, @"original3.config");
            TestHelper.CopySiteConfig(directoryName, "original_duplicate_section.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);
#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var config = server.Sites[0].Applications[0].GetWebConfiguration();
            var section =
                config.GetSection("system.webServer/defaultDocument");
            Assert.Equal(true, section["enabled"]);
            var files = section.GetCollection("files");
            Assert.Equal(8, files.Count);
            Assert.Equal("home2.html", files[0]["value"]);
            Assert.True(files[0].IsLocallyStored);
            Assert.Equal("home1.html", files[1]["value"]);
            Assert.False(files[1].IsLocallyStored);

            files.RemoveAt(1);
            server.CommitChanges();

            const string Expected = @"expected3.config";
            TestHelper.FixPhysicalPathMono(Expected);
            XmlAssert.Equal(Expected, Current);
            TestHelper.AssertSiteConfig(directoryName, Expected);
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
            string Original = Path.Combine(directoryName, @"original2_no_root_app.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nLine number: 164\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/WebSite1'\r\n\r\n", Current), exception.Message);
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
            string Original = Path.Combine(directoryName, @"original2_root_app_out.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var site = server.Sites[0];
            var config = site.GetWebConfiguration();
            var root = config.RootSectionGroup;
            Assert.NotNull(root);

            // enable Windows authentication
            var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
            Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
            Assert.Equal(true, windowsSection.IsLocked);
            Assert.Equal(false, windowsSection.IsLocallyStored);
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
            string Original = Path.Combine(directoryName, @"original2_no_root_vdir.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
            Assert.Equal(string.Format("Filename: \\\\?\\{0}\r\nLine number: 165\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/WebSite1'\r\n\r\n", Current), exception.Message);
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
            string Original = Path.Combine(directoryName, @"original2_root_vdir_out.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

#if IIS
            var server = new ServerManager(Current);
#else
            var server = new IisExpressServerManager(Current);
#endif
            var site = server.Sites[0];
            var config = site.GetWebConfiguration();
            var root = config.RootSectionGroup;
            Assert.NotNull(root);

            // enable Windows authentication
            var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
            Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
            Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
            Assert.Equal(true, windowsSection.IsLocked);
            Assert.Equal(false, windowsSection.IsLocallyStored);
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
            string Original = Path.Combine(directoryName, @"original2_logfile.config");
            TestHelper.CopySiteConfig(directoryName, "original.config");
            File.Copy(Original, Current, true);
            TestHelper.FixPhysicalPathMono(Current);

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
    }
}

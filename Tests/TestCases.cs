// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

using Microsoft.Web.Administration;
using Xunit;

namespace Tests
{
    public static class TestCases
    {
        public static void TestIisExpress(ServerManager server)
        {
            Assert.Equal(5, server.ApplicationPools.Count);
            Assert.True(server.ApplicationPools.AllowsAdd);
            Assert.False(server.ApplicationPools.AllowsClear);
            Assert.False(server.ApplicationPools.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '/', '"', '|', '<', '>', ':', '*', '?', ']', '[', '+', '=', ';', ',', '@', '&' },
                ApplicationPoolCollection.InvalidApplicationPoolNameCharacters());

            Assert.Equal(12, server.Sites.Count);
            Assert.True(server.Sites.AllowsAdd);
            Assert.False(server.Sites.AllowsClear);
            Assert.False(server.Sites.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '/', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>' },
                SiteCollection.InvalidSiteNameCharacters());

            var siteDefaults = server.SiteDefaults;
            //Assert.Equal("[http] :8080:localhost", server.Sites[0].Bindings[0].ToString());
            Assert.Equal("localhost on *:8080 (http)", server.Sites[0].Bindings[0].ToShortString());
            Assert.Equal("%IIS_USER_HOME%\\Logs", siteDefaults.LogFile.Directory);
            Assert.Equal(LogFormat.W3c, siteDefaults.LogFile.LogFormat);
            Assert.Equal("%IIS_USER_HOME%\\TraceLogFiles", siteDefaults.TraceFailedRequestsLogging.Directory);
            Assert.True(siteDefaults.TraceFailedRequestsLogging.Enabled);
            Assert.True(siteDefaults.LogFile.Enabled);
            Assert.Equal("Clr4IntegratedAppPool", server.ApplicationDefaults.ApplicationPoolName);

            var pool = server.ApplicationPools[0];

            var model = pool.GetChildElement("processModel");
            var item = model["maxProcesses"];
            Assert.Equal("Clr4IntegratedAppPool", pool.Name);
            Assert.Equal("v4.0", pool.ManagedRuntimeVersion);
            Assert.Equal(1, pool.ProcessModel.MaxProcesses);
            Assert.Equal(string.Empty, pool.ProcessModel.UserName);
#if IIS
            Assert.Equal(1L, item);
#else
            Assert.Equal(1U, item);
#endif

            // TODO: why it should be int.
#if IIS
            Assert.Equal(0, pool.GetAttributeValue("managedPipelineMode"));
            pool.SetAttributeValue("managedPipelineMode", 1);
            Assert.Equal(1, pool.GetAttributeValue("managedPipelineMode"));
            pool.SetAttributeValue("managedPipelineMode", "Integrated");
            Assert.Equal(0, pool.GetAttributeValue("managedPipelineMode"));
#else
            Assert.Equal(0L, pool.GetAttributeValue("managedPipelineMode"));
            pool.SetAttributeValue("managedPipelineMode", 1);
            Assert.Equal(1L, pool.GetAttributeValue("managedPipelineMode"));
            pool.SetAttributeValue("managedPipelineMode", "Integrated");
            Assert.Equal(0L, pool.GetAttributeValue("managedPipelineMode"));

            Assert.Equal(14, pool.ApplicationCount);
#endif
            var name = Assert.Throws<COMException>(() => pool.SetAttributeValue("name", ""));
            Assert.Equal("Invalid application pool name\r\n", name.Message);

            var time = Assert.Throws<COMException>(() => pool.ProcessModel.IdleTimeout = TimeSpan.MaxValue);
            Assert.Equal("Timespan value must be between 00:00:00 and 30.00:00:00 seconds inclusive, with a granularity of 60 seconds\r\n", time.Message);

            var site = server.Sites[0];
            Assert.Equal("WebSite1", site.Name);
            Assert.Equal(14, site.Bindings.Count);
            var binding = site.Bindings[0];
            Assert.Equal(IPAddress.Any, binding.EndPoint.Address);
            Assert.Equal(8080, binding.EndPoint.Port);
            Assert.Equal("localhost", binding.Host);
            Assert.Equal("*:8080:localhost", binding.ToString());
            Assert.Equal(":8080:localhost", binding.BindingInformation);
            Assert.True(site.Bindings.AllowsAdd);
            Assert.True(site.Bindings.AllowsClear);
            Assert.False(site.Bindings.AllowsRemove);
            Assert.Equal("%IIS_USER_HOME%\\Logs", site.LogFile.Directory);

            var sslSite = server.Sites[1];
            var sslBinding = sslSite.Bindings[1];
            var certificateHash = sslBinding.CertificateHash;
            var certificateStoreName = sslBinding.CertificateStoreName;
            Assert.Equal(SslFlags.Sni, sslBinding.SslFlags);

            sslSite.Bindings.RemoveAt(1);
            sslSite.Bindings.Add(":443:localhost", "https");
            sslBinding = sslSite.Bindings[1];
            Assert.Equal(SslFlags.None, sslBinding.SslFlags);

            sslSite.Bindings.RemoveAt(1);
            sslSite.Bindings.Add(":443:localhost", certificateHash, certificateStoreName, SslFlags.Sni);
            sslBinding = sslSite.Bindings[1];
            Assert.Equal(SslFlags.Sni, sslBinding.SslFlags);

            sslSite.Bindings.RemoveAt(1);
            sslSite.Bindings.Add(":443:localhost", certificateHash, certificateStoreName);
            sslBinding = sslSite.Bindings[1];
            Assert.Equal(SslFlags.None, sslBinding.SslFlags);

            {
                sslBinding = sslSite.Bindings.CreateElement();
                var exception = Assert.Throws<FileNotFoundException>(() => sslSite.Bindings.Add(sslBinding));
                Assert.Equal("Filename: \r\nError: Element is missing required attributes protocol,bindingInformation\r\n\r\n", exception.Message);
            }

            var app = site.Applications[0];
            Assert.True(site.Applications.AllowsAdd);
            Assert.False(site.Applications.AllowsClear);
            Assert.False(site.Applications.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>', '*' },
                ApplicationCollection.InvalidApplicationPathCharacters());
            Assert.Equal("Clr4IntegratedAppPool", app.ApplicationPoolName);

            Assert.Equal("/", app.Path);
            var vDir = app.VirtualDirectories[0];
            Assert.True(app.VirtualDirectories.AllowsAdd);
            Assert.False(app.VirtualDirectories.AllowsClear);
            Assert.False(app.VirtualDirectories.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>', '*' },
                VirtualDirectoryCollection.InvalidVirtualDirectoryPathCharacters());

            Assert.Equal(Helper.IsRunningOnMono() ? "%JEXUS_TEST_HOME%/WebSite1" : "%JEXUS_TEST_HOME%\\WebSite1", vDir.PhysicalPath);

            {
                var config = server.GetApplicationHostConfiguration();
                var section = config.GetSection("system.applicationHost/log");
                var mode = section.Attributes["centralLogFileMode"];
#if IIS
                Assert.Equal(0, mode.Value);
#else
                Assert.Equal(0L, mode.Value);
#endif
                var encoding = section.Attributes["logInUTF8"];
                Assert.Equal(true, encoding.Value);
                ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging");
                Assert.Equal(false, httpLoggingSection["dontLog"]);

                ConfigurationSection defaultDocumentSection = config.GetSection("system.webServer/defaultDocument");
                Assert.Equal(true, defaultDocumentSection["enabled"]);
                ConfigurationElementCollection filesCollection = defaultDocumentSection.GetCollection("files");
                Assert.Equal(6, filesCollection.Count);

                var errorsSection = config.GetSection("system.webServer/httpErrors");
                var errorsCollection = errorsSection.GetCollection();
                Assert.Equal(9, errorsCollection.Count);

                var anonymousSection = config.GetSection("system.webServer/security/authentication/anonymousAuthentication");
                var anonymousEnabled = (bool)anonymousSection["enabled"];
                Assert.True(anonymousEnabled);
                var value = anonymousSection["password"];
                Assert.Equal(string.Empty, value);

                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.False(windowsEnabled);

                windowsSection["enabled"] = true;
                server.CommitChanges();
            }

            {
                var config = server.GetApplicationHostConfiguration();
                var asp = config.GetSection("system.webServer/asp");
                var comPlus = asp.ChildElements["comPlus"];
                var sxsName = comPlus.GetAttribute("sxsName");
                sxsName.Delete();
                Assert.Equal("", (string)sxsName.Value);

                comPlus["sxsName"] = "test";
                server.CommitChanges();
            }

            {
                var config = server.GetApplicationHostConfiguration();
                var asp = config.GetSection("system.webServer/asp");
                var comPlus = asp.ChildElements["comPlus"];
                var sxsName = (string)comPlus["sxsName"];
                Assert.Equal("test", sxsName);

                var exception = Assert.Throws<COMException>(() => comPlus["sxsName"] = string.Empty);
                Assert.Equal("String must not be empty\r\n", exception.Message);
                server.CommitChanges();
            }

            {
                var config = server.GetApplicationHostConfiguration();
                var asp = config.GetSection("system.webServer/asp");
                var comPlus = asp.ChildElements["comPlus"];
                var sxsName = (string)comPlus["sxsName"];
                Assert.Equal("test", sxsName);

                comPlus.GetAttribute("sxsName").Delete();
                server.CommitChanges();

                sxsName = (string)comPlus.GetAttribute("sxsName").Value;
                Assert.Equal("", sxsName);
            }

            {
                var config = server.GetApplicationHostConfiguration();
                var locations = config.GetLocationPaths();
                Assert.Equal(3, locations.Length);

                var exception =
                    Assert.Throws<FileNotFoundException>(
                        () =>
                        config.GetSection(
                            "system.webServer/security/authentication/anonymousAuthentication",
                            "Default Web Site"));
                Assert.Equal(
                    "Filename: \r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/Default Web Site'\r\n\r\n",
                    exception.Message);
                Assert.Equal(null, exception.FileName);

                var anonymousSection = config.GetSection(
                    "system.webServer/security/authentication/anonymousAuthentication",
                    "WebSite2");
                var anonymousEnabled = (bool)anonymousSection["enabled"];
                Assert.True(anonymousEnabled);
                Assert.Equal("test", anonymousSection["userName"]);

                // Assert.Equal("123456", anonymousSection["password"]);
            }

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
                Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
                Assert.Equal(false, windowsSection.IsLocked);
                Assert.Equal(true, windowsSection.IsLocallyStored);

                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.True(windowsEnabled);
                windowsSection["enabled"] = false;
                Assert.Equal(false, windowsSection["enabled"]);

                {
                    // disable logging. Saved in applicationHost.config, as it cannot be overridden in web.config.
                    ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging", "WebSite1");
                    Assert.Equal(false, httpLoggingSection["dontLog"]);
                    httpLoggingSection["dontLog"] = true;
                }

                {
                    ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging", "WebSite1/test");
                    // TODO:
                    // Assert.Equal(true, httpLoggingSection["dontLog"]);
                    httpLoggingSection["dontLog"] = false;
                }
            }

            var siteName = Assert.Throws<COMException>(() => server.Sites[0].Name = "");
            Assert.Equal("Invalid site name\r\n", siteName.Message);

            var limit = Assert.Throws<COMException>(() => server.Sites[0].Limits.MaxBandwidth = 12);
            Assert.Equal("Integer value must not be between 0 and 1023 inclusive\r\n", limit.Message);

            var appPath = Assert.Throws<COMException>(() => server.Sites[0].Applications[0].Path = "");
            Assert.Equal("Invalid application path\r\n", appPath.Message);

            var vDirPath =
                Assert.Throws<COMException>(() => server.Sites[0].Applications[0].VirtualDirectories[0].Path = "");
            Assert.Equal("Invalid virtual directory path\r\n", vDirPath.Message);

            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
                Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
                Assert.Equal(true, windowsSection.IsLocked);
                Assert.Equal(false, windowsSection.IsLocallyStored);

                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.True(windowsEnabled);
                var exception = Assert.Throws<FileLoadException>(() => windowsSection["enabled"] = false);
                Assert.Equal(
                    "This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n",
                    exception.Message);
                Assert.Equal(null, exception.FileName);

                var compression = config.GetSection("system.webServer/urlCompression");
                Assert.Equal(OverrideMode.Inherit, compression.OverrideMode);
                Assert.Equal(OverrideMode.Allow, compression.OverrideModeEffective);
                Assert.Equal(false, compression.IsLocked);
                Assert.Equal(false, compression.IsLocallyStored);

                Assert.Equal(true, compression["doDynamicCompression"]);

                compression["doDynamicCompression"] = false;

                {
                    // disable default document. Saved to web.config as this section can be overridden anywhere.
                    ConfigurationSection defaultDocumentSection = config.GetSection("system.webServer/defaultDocument");
                    Assert.Equal(true, defaultDocumentSection["enabled"]);
                    defaultDocumentSection["enabled"] = false;

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

                    ConfigurationElement addElement = filesCollection.CreateElement();
                    addElement["value"] = @"home.html";
                    filesCollection.AddAt(0, addElement);

                    Assert.Equal(8, filesCollection.Count);

                    {
                        var first = filesCollection[0];
                        Assert.Equal("home.html", first["value"]);
                        // TODO: why?
                        // Assert.False(first.IsLocallyStored);
                    }

                    filesCollection.RemoveAt(4);
                    Assert.Equal(7, filesCollection.Count);

                    ConfigurationElement lastElement = filesCollection.CreateElement();
                    lastElement["value"] = @"home1.html";
                    lastElement["value"] = @"home2.html";
                    filesCollection.Add(lastElement);
                    Assert.Equal(8, filesCollection.Count);
                }

                //{
                //    var last = filesCollection[8];
                //    Assert.Equal("home2.html", last["value"]);
                //    // TODO: why?
                //    Assert.False(last.IsLocallyStored);
                //}

                {
                    // disable logging. Saved in applicationHost.config, as it cannot be overridden in web.config.
                    ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging");
                    Assert.Equal(false, httpLoggingSection["dontLog"]);
                    Assert.Throws<FileLoadException>(() => httpLoggingSection["dontLog"] = true);
                }
                {
                    ConfigurationSection httpLoggingSection = config.GetSection(
                        "system.webServer/httpLogging",
                        "WebSite1/test");
                    // TODO:
                    //Assert.Equal(true, httpLoggingSection["dontLog"]);
                    Assert.Throws<FileLoadException>(() => httpLoggingSection["dontLog"] = false);
                }

                var errorsSection = config.GetSection("system.webServer/httpErrors");
                Assert.Equal(OverrideMode.Inherit, errorsSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, errorsSection.OverrideModeEffective);
                Assert.Equal(false, errorsSection.IsLocked);
                Assert.Equal(false, errorsSection.IsLocallyStored);

                var errorsCollection = errorsSection.GetCollection();
                Assert.Equal(9, errorsCollection.Count);

                var error = errorsCollection.CreateElement();
                var cast = Assert.Throws<InvalidCastException>(() => error.SetAttributeValue("statusCode", "500"));
                Assert.Equal(Helper.IsRunningOnMono() ? "Cannot cast from source type to destination type." : "Specified cast is not valid.", cast.Message);

                var ex2 = Assert.Throws<COMException>(() => error["statusCode"] = 90000);
                Assert.Equal("Integer value must be between 400 and 999 inclusive\r\n", ex2.Message);

                error["statusCode"] = 500;
                error["subStatusCode"] = 55;
                error["prefixLanguageFilePath"] = string.Empty;
                error["responseMode"] = "File";
                var ex1 = Assert.Throws<FileNotFoundException>(() => errorsCollection.Add(error));
                Assert.Equal("Filename: \r\nError: Element is missing required attributes path\r\n\r\n", ex1.Message);
                Assert.Equal(null, ex1.FileName);

                var ex = Assert.Throws<COMException>(() => error["path"] = "");
                Assert.Equal("String must not be empty\r\n", ex.Message);

                error["path"] = "test.htm";
                errorsCollection.Add(error);

                var staticContent = config.GetSection("system.webServer/staticContent").GetChildElement("clientCache");
                staticContent["cacheControlMode"] = "DisableCache";

                ConfigurationSection requestFilteringSection =
                    config.GetSection("system.webServer/security/requestFiltering");
                ConfigurationElement hiddenSegmentsElement = requestFilteringSection.GetChildElement("hiddenSegments");
                ConfigurationElementCollection hiddenSegmentsCollection = hiddenSegmentsElement.GetCollection();
                Assert.Equal(8, hiddenSegmentsCollection.Count);

                var test = hiddenSegmentsCollection.CreateElement();
                test["segment"] = "test";
                hiddenSegmentsCollection.Add(test);

                var old = hiddenSegmentsCollection[0];
                hiddenSegmentsCollection.Remove(old);

                var section = config.GetSection("system.webServer/rewrite/rules");
                ConfigurationElementCollection collection = section.GetCollection();
                //collection.Clear();
                ////collection.Delete();

                //collection = section.GetCollection();
                //Assert.Equal(0, collection.Count);

                var newElement = collection.CreateElement();
                newElement["name"] = "test";
                collection.Add(newElement);
                Assert.Equal(2, collection.Count);

                collection.Clear();
                Assert.Equal(0, collection.Count);

                newElement = collection.CreateElement();
                newElement["name"] = "test";
                collection.Add(newElement);
                Assert.Equal(1, collection.Count);
            }

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
                Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
                Assert.Equal(false, windowsSection.IsLocked);
                Assert.Equal(true, windowsSection.IsLocallyStored);

                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.False(windowsEnabled);
            }

            {
                Configuration config = server.GetApplicationHostConfiguration();

                var anonymousSection = config.GetSection(
                    "system.webServer/security/authentication/anonymousAuthentication",
                    "WebSite2");

                // anonymousSection["userName"] = "test1";
                // anonymousSection["password"] = "654321";
                ConfigurationSection windowsAuthenticationSection =
                    config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite2");
                windowsAuthenticationSection["enabled"] = true;

                ConfigurationElement extendedProtectionElement =
                    windowsAuthenticationSection.GetChildElement("extendedProtection");
                extendedProtectionElement["tokenChecking"] = @"Allow";
                extendedProtectionElement["flags"] = @"None";

                var exception = Assert.Throws<COMException>(() => extendedProtectionElement["tokenChecking"] = @"NotExist");
                Assert.Equal("Enum must be one of None, Allow, Require\r\n", exception.Message);

                exception = Assert.Throws<COMException>(() => extendedProtectionElement["flags"] = @"NotExist");
                Assert.Equal("Flags must be some combination of None, Proxy, NoServiceNameCheck, AllowDotlessSpn, ProxyCohosting\r\n", exception.Message);

                ConfigurationElementCollection extendedProtectionCollection = extendedProtectionElement.GetCollection();

                ConfigurationElement spnElement = extendedProtectionCollection.CreateElement("spn");
                spnElement["name"] = @"HTTP/www.contoso.com";
                extendedProtectionCollection.Add(spnElement);

                ConfigurationElement spnElement1 = extendedProtectionCollection.CreateElement("spn");
                spnElement1["name"] = @"HTTP/contoso.com";
                extendedProtectionCollection.Add(spnElement1);

                server.CommitChanges();

                ConfigurationSection rulesSection = config.GetSection("system.webServer/rewrite/rules");
                ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();
                Assert.Equal(1, rulesCollection.Count);

                ConfigurationElement ruleElement = rulesCollection[0];
                Assert.Equal("lextudio2", ruleElement["name"]);
#if IIS
                Assert.Equal(0, ruleElement["patternSyntax"]);
#else
                Assert.Equal(0L, ruleElement["patternSyntax"]);
#endif
                Assert.Equal(true, ruleElement["stopProcessing"]);

                ConfigurationElement matchElement = ruleElement.GetChildElement("match");
                Assert.Equal(@"(.*)", matchElement["url"]);

                ConfigurationElement actionElement = ruleElement.GetChildElement("action");
#if IIS
                Assert.Equal(1, actionElement["type"]);
#else
                Assert.Equal(1L, actionElement["type"]);
#endif
                Assert.Equal("/www/{R:0}", actionElement["url"]);
            }

            // remove application pool
            Assert.False(server.ApplicationPools.AllowsRemove);
            Assert.Equal(5, server.ApplicationPools.Count);
            server.ApplicationPools.RemoveAt(4);
            Assert.Equal(4, server.ApplicationPools.Count);

            // remove binding
            server.Sites[1].Bindings.RemoveAt(1);

            // remove site
            server.Sites.RemoveAt(4);

            // remove application
            var site1 = server.Sites[9];
            site1.Applications.RemoveAt(1);

            // remove virtual directory
            var application = server.Sites[2].Applications[0];
            application.VirtualDirectories.RemoveAt(1);

            server.CommitChanges();
        }

        public static void TestIisExpressMissingWebsiteConfig(ServerManager server)
        {
            Assert.Equal(5, server.ApplicationPools.Count);
            Assert.True(server.ApplicationPools.AllowsAdd);
            Assert.False(server.ApplicationPools.AllowsClear);
            Assert.False(server.ApplicationPools.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '/', '"', '|', '<', '>', ':', '*', '?', ']', '[', '+', '=', ';', ',', '@', '&' },
                ApplicationPoolCollection.InvalidApplicationPoolNameCharacters());

            Assert.Equal(12, server.Sites.Count);
            Assert.True(server.Sites.AllowsAdd);
            Assert.False(server.Sites.AllowsClear);
            Assert.False(server.Sites.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '/', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>' },
                SiteCollection.InvalidSiteNameCharacters());

            var siteDefaults = server.SiteDefaults;
            //Assert.Equal("localhost on *:8080 (http)", server.Sites[0].Bindings[0].ToShortString());
            Assert.Equal("%IIS_USER_HOME%\\Logs", siteDefaults.LogFile.Directory);
            Assert.Equal(LogFormat.W3c, siteDefaults.LogFile.LogFormat);
            Assert.Equal("%IIS_USER_HOME%\\TraceLogFiles", siteDefaults.TraceFailedRequestsLogging.Directory);
            Assert.True(siteDefaults.TraceFailedRequestsLogging.Enabled);
            Assert.True(siteDefaults.LogFile.Enabled);
            Assert.Equal("Clr4IntegratedAppPool", server.ApplicationDefaults.ApplicationPoolName);

            var pool = server.ApplicationPools[0];

            var model = pool.GetChildElement("processModel");
            var item = model["maxProcesses"];
            Assert.Equal("Clr4IntegratedAppPool", pool.Name);
            Assert.Equal("v4.0", pool.ManagedRuntimeVersion);
            Assert.Equal(1, pool.ProcessModel.MaxProcesses);
            Assert.Equal(string.Empty, pool.ProcessModel.UserName);
#if IIS
            Assert.Equal(1L, item);
#else
            Assert.Equal(1U, item);
#endif

            var name = Assert.Throws<COMException>(() => pool.SetAttributeValue("name", ""));
            Assert.Equal("Invalid application pool name\r\n", name.Message);

            var time = Assert.Throws<COMException>(() => pool.ProcessModel.IdleTimeout = TimeSpan.MaxValue);
            Assert.Equal("Timespan value must be between 00:00:00 and 30.00:00:00 seconds inclusive, with a granularity of 60 seconds\r\n", time.Message);

            var site = server.Sites[0];
            Assert.Equal("WebSite1", site.Name);
            Assert.Equal(14, site.Bindings.Count);
            Assert.Equal("localhost on *:8080 (http)", site.Bindings[0].ToShortString());
            Assert.Equal("0.0.0.0:8080", site.Bindings[0].EndPoint.ToString());
            Assert.Equal("localhost", site.Bindings[0].Host);

            Assert.Null(site.Bindings[1].EndPoint);
            Assert.Equal(string.Empty, site.Bindings[1].Host);
            Assert.False(site.Bindings[1].IsIPPortHostBinding);
            Assert.False(site.Bindings[1].UseDsMapper);
            Assert.Null(site.Bindings[1].CertificateHash);
            Assert.Null(site.Bindings[1].CertificateStoreName);
            Assert.Equal("808:* (net.tcp)", site.Bindings[1].ToShortString());

            Assert.Null(site.Bindings[2].EndPoint);
            Assert.Equal(string.Empty, site.Bindings[2].Host);
            Assert.False(site.Bindings[2].IsIPPortHostBinding);
            Assert.False(site.Bindings[2].UseDsMapper);
            Assert.Null(site.Bindings[2].CertificateHash);
            Assert.Null(site.Bindings[2].CertificateStoreName);
            Assert.Equal("localhost (net.msmq)", site.Bindings[2].ToShortString());

            Assert.Null(site.Bindings[3].EndPoint);
            Assert.Equal(string.Empty, site.Bindings[3].Host);
            Assert.False(site.Bindings[3].IsIPPortHostBinding);
            Assert.False(site.Bindings[3].UseDsMapper);
            Assert.Null(site.Bindings[3].CertificateHash);
            Assert.Null(site.Bindings[3].CertificateStoreName);
            Assert.Equal("localhost (msmq.formatname)", site.Bindings[3].ToShortString());

            Assert.Null(site.Bindings[4].EndPoint);
            Assert.Equal(string.Empty, site.Bindings[4].Host);
            Assert.False(site.Bindings[4].IsIPPortHostBinding);
            Assert.False(site.Bindings[4].UseDsMapper);
            Assert.Null(site.Bindings[4].CertificateHash);
            Assert.Null(site.Bindings[4].CertificateStoreName);
            Assert.Equal("* (net.pipe)", site.Bindings[4].ToShortString());

            Assert.Null(site.Bindings[5].EndPoint);
            Assert.Equal(string.Empty, site.Bindings[5].Host);
            Assert.False(site.Bindings[5].IsIPPortHostBinding);
            Assert.False(site.Bindings[5].UseDsMapper);
            Assert.Null(site.Bindings[5].CertificateHash);
            Assert.Null(site.Bindings[5].CertificateStoreName);
            Assert.Equal("*:21: (ftp)", site.Bindings[5].ToShortString());

            Assert.Equal("* on *:8080 (http)", site.Bindings[6].ToShortString());
            Assert.Equal("0.0.0.0:8080", site.Bindings[6].EndPoint.ToString());
            Assert.Equal("*", site.Bindings[6].Host);

            Assert.Equal("localhost on *:443 (https)", site.Bindings[7].ToShortString());
            Assert.Equal("0.0.0.0:443", site.Bindings[7].EndPoint.ToString());
            Assert.Equal("localhost", site.Bindings[7].Host);

            Assert.Equal("* on *:44300 (https)", site.Bindings[8].ToShortString());
            Assert.Equal("0.0.0.0:44300", site.Bindings[8].EndPoint.ToString());
            Assert.Equal("*", site.Bindings[8].Host);

            Assert.Null(site.Bindings[9].EndPoint);
            Assert.Equal(string.Empty, site.Bindings[9].Host);
            Assert.False(site.Bindings[9].IsIPPortHostBinding);
            Assert.False(site.Bindings[9].UseDsMapper);
            Assert.Null(site.Bindings[9].CertificateHash);
            Assert.Null(site.Bindings[9].CertificateStoreName);
            Assert.Equal("*:210:localhost (ftp)", site.Bindings[9].ToShortString());

            {
                var binding = site.Bindings[10];
                Assert.Null(binding.EndPoint);
                Assert.Equal(string.Empty, binding.Host);
                Assert.False(binding.IsIPPortHostBinding);
                Assert.False(binding.UseDsMapper);
                Assert.Null(binding.CertificateHash);
                Assert.Null(binding.CertificateStoreName);
                Assert.Equal("* (net.tcp)", binding.ToShortString());
            }
            {
                var binding = site.Bindings[11];
                Assert.Null(binding.EndPoint);
                Assert.Equal(string.Empty, binding.Host);
                Assert.False(binding.IsIPPortHostBinding);
                Assert.False(binding.UseDsMapper);
                Assert.Null(binding.CertificateHash);
                Assert.Null(binding.CertificateStoreName);
                Assert.Equal("* (net.msmq)", binding.ToShortString());
            }
            {
                var siteBinding = site.Bindings[12];
                Assert.Null(siteBinding.EndPoint);
                Assert.Equal(string.Empty, siteBinding.Host);
                Assert.False(siteBinding.IsIPPortHostBinding);
                Assert.False(siteBinding.UseDsMapper);
                Assert.Null(siteBinding.CertificateHash);
                Assert.Null(siteBinding.CertificateStoreName);
                Assert.Equal("* (msmq.formatname)", siteBinding.ToShortString());
            }
            {
                var binding = site.Bindings[13];
                Assert.Null(binding.EndPoint);
                Assert.Equal(string.Empty, binding.Host);
                Assert.False(binding.IsIPPortHostBinding);
                Assert.False(binding.UseDsMapper);
                Assert.Null(binding.CertificateHash);
                Assert.Null(binding.CertificateStoreName);
                Assert.Equal("localhost (net.pipe)", binding.ToShortString());
            }

            Assert.True(site.Bindings.AllowsAdd);
            Assert.True(site.Bindings.AllowsClear);
            Assert.False(site.Bindings.AllowsRemove);

            var app = site.Applications[0];
            Assert.True(site.Applications.AllowsAdd);
            Assert.False(site.Applications.AllowsClear);
            Assert.False(site.Applications.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>', '*' },
                ApplicationCollection.InvalidApplicationPathCharacters());

            Assert.Equal("/", app.Path);
            var vDir = app.VirtualDirectories[0];
            Assert.True(app.VirtualDirectories.AllowsAdd);
            Assert.False(app.VirtualDirectories.AllowsClear);
            Assert.False(app.VirtualDirectories.AllowsRemove);
            Assert.Equal(
                new[] { '\\', '?', ';', ':', '@', '&', '=', '+', '$', ',', '|', '"', '<', '>', '*' },
                VirtualDirectoryCollection.InvalidVirtualDirectoryPathCharacters());

            Assert.Equal(Helper.IsRunningOnMono() ? "%JEXUS_TEST_HOME%/WebSite1" : "%JEXUS_TEST_HOME%\\WebSite1", vDir.PhysicalPath);

            {
                var config = server.GetApplicationHostConfiguration();
                var section = config.GetSection("system.applicationHost/log");
                var encoding = section.Attributes["logInUTF8"];
                Assert.Equal(true, encoding.Value);
                ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging");
                Assert.Equal(false, httpLoggingSection["dontLog"]);

                ConfigurationSection defaultDocumentSection = config.GetSection("system.webServer/defaultDocument");
                Assert.Equal(true, defaultDocumentSection["enabled"]);
                ConfigurationElementCollection filesCollection = defaultDocumentSection.GetCollection("files");
                Assert.Equal(6, filesCollection.Count);

                var errorsSection = config.GetSection("system.webServer/httpErrors");
                var errorsCollection = errorsSection.GetCollection();
                Assert.Equal(9, errorsCollection.Count);

                var anonymousSection = config.GetSection("system.webServer/security/authentication/anonymousAuthentication");
                var anonymousEnabled = (bool)anonymousSection["enabled"];
                Assert.True(anonymousEnabled);
                var value = anonymousSection["password"];
                Assert.Equal(string.Empty, value);

                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.False(windowsEnabled);

                windowsSection["enabled"] = true;
                server.CommitChanges();
            }

            {
                var config = server.GetApplicationHostConfiguration();
                var locations = config.GetLocationPaths();
                Assert.Equal(3, locations.Length);

                var exception =
                    Assert.Throws<FileNotFoundException>(
                        () =>
                        config.GetSection(
                            "system.webServer/security/authentication/anonymousAuthentication",
                            "Default Web Site"));
                Assert.Equal(
                    "Filename: \r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/Default Web Site'\r\n\r\n",
                    exception.Message);
                Assert.Equal(null, exception.FileName);

                var anonymousSection = config.GetSection(
                    "system.webServer/security/authentication/anonymousAuthentication",
                    "WebSite2");
                var anonymousEnabled = (bool)anonymousSection["enabled"];
                Assert.True(anonymousEnabled);
                Assert.Equal("test", anonymousSection["userName"]);

                // Assert.Equal("123456", anonymousSection["password"]);
            }

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
                Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
                Assert.Equal(false, windowsSection.IsLocked);
                Assert.Equal(true, windowsSection.IsLocallyStored);

                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.True(windowsEnabled);
                windowsSection["enabled"] = false;
                Assert.Equal(false, windowsSection["enabled"]);

                {
                    // disable logging. Saved in applicationHost.config, as it cannot be overridden in web.config.
                    ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging", "WebSite1");
                    Assert.Equal(false, httpLoggingSection["dontLog"]);
                    httpLoggingSection["dontLog"] = true;
                }

                {
                    ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging", "WebSite1/test");
                    // TODO:
                    // Assert.Equal(true, httpLoggingSection["dontLog"]);
                    httpLoggingSection["dontLog"] = false;
                }
            }

            var siteName = Assert.Throws<COMException>(() => server.Sites[0].Name = "");
            Assert.Equal("Invalid site name\r\n", siteName.Message);

            var limit = Assert.Throws<COMException>(() => server.Sites[0].Limits.MaxBandwidth = 12);
            Assert.Equal("Integer value must not be between 0 and 1023 inclusive\r\n", limit.Message);

            var appPath = Assert.Throws<COMException>(() => server.Sites[0].Applications[0].Path = "");
            Assert.Equal("Invalid application path\r\n", appPath.Message);

            var vDirPath =
                Assert.Throws<COMException>(() => server.Sites[0].Applications[0].VirtualDirectories[0].Path = "");
            Assert.Equal("Invalid virtual directory path\r\n", vDirPath.Message);

            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
                Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
                Assert.Equal(true, windowsSection.IsLocked);
                Assert.Equal(false, windowsSection.IsLocallyStored);

                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.True(windowsEnabled);
                var exception = Assert.Throws<FileLoadException>(() => windowsSection["enabled"] = false);
                Assert.Equal(
                    "This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n",
                    exception.Message);
                Assert.Equal(null, exception.FileName);

                var compression = config.GetSection("system.webServer/urlCompression");
                Assert.Equal(OverrideMode.Inherit, compression.OverrideMode);
                Assert.Equal(OverrideMode.Allow, compression.OverrideModeEffective);
                Assert.Equal(false, compression.IsLocked);
                Assert.Equal(false, compression.IsLocallyStored);

                Assert.Equal(true, compression["doDynamicCompression"]);

                compression["doDynamicCompression"] = false;

                {
                    // disable default document. Saved to web.config as this section can be overridden anywhere.
                    ConfigurationSection defaultDocumentSection = config.GetSection("system.webServer/defaultDocument");
                    Assert.Equal(true, defaultDocumentSection["enabled"]);
                    defaultDocumentSection["enabled"] = false;

                    ConfigurationElementCollection filesCollection = defaultDocumentSection.GetCollection("files");
                    Assert.Equal(6, filesCollection.Count);

                    {
                        var first = filesCollection[0];
                        Assert.Equal("Default.htm", first["value"]);
                        Assert.False(first.IsLocallyStored);
                    }

                    var second = filesCollection[1];
                    Assert.Equal("Default.asp", second["value"]);
                    Assert.False(second.IsLocallyStored);

                    var third = filesCollection[2];
                    Assert.Equal("index.htm", third["value"]);
                    Assert.False(third.IsLocallyStored);

                    ConfigurationElement addElement = filesCollection.CreateElement();
                    addElement["value"] = @"home.html";
                    filesCollection.AddAt(0, addElement);

                    Assert.Equal(7, filesCollection.Count);

                    {
                        var first = filesCollection[0];
                        Assert.Equal("home.html", first["value"]);
                        // TODO: why?
                        // Assert.False(first.IsLocallyStored);
                    }

                    filesCollection.RemoveAt(4);
                    Assert.Equal(6, filesCollection.Count);

                    ConfigurationElement lastElement = filesCollection.CreateElement();
                    lastElement["value"] = @"home.html";
                    var dup = Assert.Throws<COMException>(() => filesCollection.Add(lastElement));
#if IIS
                    Assert.Equal(
                        "Filename: \r\nError: Cannot add duplicate collection entry of type 'add' with unique key attribute 'value' set to 'home.html'\r\n\r\n",
                        dup.Message);
#else
                    Assert.Equal(
$"Filename: \\\\?\\{config.FileContext.FileName}\r\nLine number: 0\r\nError: Cannot add duplicate collection entry of type 'add' with unique key attribute 'value' set to 'home.html'\r\n\r\n",
                        dup.Message);
#endif
                    lastElement["value"] = @"home2.html";
                    filesCollection.Add(lastElement);
                    Assert.Equal(7, filesCollection.Count);
                }

                //{
                //    var last = filesCollection[8];
                //    Assert.Equal("home2.html", last["value"]);
                //    // TODO: why?
                //    Assert.False(last.IsLocallyStored);
                //}

                {
                    // disable logging. Saved in applicationHost.config, as it cannot be overridden in web.config.
                    ConfigurationSection httpLoggingSection = config.GetSection("system.webServer/httpLogging");
                    Assert.Equal(false, httpLoggingSection["dontLog"]);
                    Assert.Throws<FileLoadException>(() => httpLoggingSection["dontLog"] = true);
                }
                {
                    ConfigurationSection httpLoggingSection = config.GetSection(
                        "system.webServer/httpLogging",
                        "WebSite1/test");
                    // TODO:
                    //Assert.Equal(true, httpLoggingSection["dontLog"]);
                    Assert.Throws<FileLoadException>(() => httpLoggingSection["dontLog"] = false);
                }

                var errorsSection = config.GetSection("system.webServer/httpErrors");
                Assert.Equal(OverrideMode.Inherit, errorsSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, errorsSection.OverrideModeEffective);
                Assert.Equal(false, errorsSection.IsLocked);
                Assert.Equal(false, errorsSection.IsLocallyStored);

                var errorsCollection = errorsSection.GetCollection();
                Assert.Equal(9, errorsCollection.Count);

                var error = errorsCollection.CreateElement();
                var cast = Assert.Throws<InvalidCastException>(() => error.SetAttributeValue("statusCode", "500"));
                Assert.Equal(Helper.IsRunningOnMono() ? "Cannot cast from source type to destination type." : "Specified cast is not valid.", cast.Message);

                var ex2 = Assert.Throws<COMException>(() => error["statusCode"] = 90000);
                Assert.Equal("Integer value must be between 400 and 999 inclusive\r\n", ex2.Message);

                error["statusCode"] = 500;
                error["subStatusCode"] = 55;
                error["prefixLanguageFilePath"] = string.Empty;

                error["responseMode"] = 0;
                error["responseMode"] = ResponseMode.Test;
#if IIS
                Assert.Equal(1, error["responseMode"]);
#else
                Assert.Equal(1L, error["responseMode"]);
#endif

                error["responseMode"] = "File";
                var ex1 = Assert.Throws<FileNotFoundException>(() => errorsCollection.Add(error));
                Assert.Equal("Filename: \r\nError: Element is missing required attributes path\r\n\r\n", ex1.Message);
                Assert.Equal(null, ex1.FileName);

                var ex = Assert.Throws<COMException>(() => error["path"] = "");
                Assert.Equal("String must not be empty\r\n", ex.Message);

                error["path"] = "test.htm";
                errorsCollection.Add(error);

                var staticContent = config.GetSection("system.webServer/staticContent").GetChildElement("clientCache");
                staticContent["cacheControlMode"] = "DisableCache";

                ConfigurationSection requestFilteringSection =
                    config.GetSection("system.webServer/security/requestFiltering");
                ConfigurationElement hiddenSegmentsElement = requestFilteringSection.GetChildElement("hiddenSegments");
                ConfigurationElementCollection hiddenSegmentsCollection = hiddenSegmentsElement.GetCollection();
                Assert.Equal(8, hiddenSegmentsCollection.Count);

                var test = hiddenSegmentsCollection.CreateElement();
                test["segment"] = "test";
                hiddenSegmentsCollection.Add(test);

                var old = hiddenSegmentsCollection[0];
                hiddenSegmentsCollection.Remove(old);

                var section = config.GetSection("system.webServer/rewrite/rules");
                ConfigurationElementCollection collection = section.GetCollection();
                //collection.Clear();
                ////collection.Delete();

                //collection = section.GetCollection();
                //Assert.Equal(0, collection.Count);

                var newElement = collection.CreateElement();
                newElement["name"] = "test";
                collection.Add(newElement);
                Assert.Equal(2, collection.Count);

                collection.Clear();
                Assert.Equal(0, collection.Count);

                newElement = collection.CreateElement();
                newElement["name"] = "test";
                collection.Add(newElement);
                Assert.Equal(1, collection.Count);
            }

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
                Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
                Assert.Equal(false, windowsSection.IsLocked);
                Assert.Equal(true, windowsSection.IsLocallyStored);

                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.False(windowsEnabled);
            }

            {
                Configuration config = server.GetApplicationHostConfiguration();

                var anonymousSection = config.GetSection(
                    "system.webServer/security/authentication/anonymousAuthentication",
                    "WebSite2");

                // anonymousSection["userName"] = "test1";
                // anonymousSection["password"] = "654321";
                ConfigurationSection windowsAuthenticationSection =
                    config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite2");
                windowsAuthenticationSection["enabled"] = true;

                ConfigurationElement extendedProtectionElement =
                    windowsAuthenticationSection.GetChildElement("extendedProtection");
                extendedProtectionElement["tokenChecking"] = @"Allow";
                extendedProtectionElement["flags"] = @"None";

                var exception = Assert.Throws<COMException>(() => extendedProtectionElement["tokenChecking"] = @"NotExist");
                Assert.Equal("Enum must be one of None, Allow, Require\r\n", exception.Message);

                exception = Assert.Throws<COMException>(() => extendedProtectionElement["flags"] = @"NotExist");
                Assert.Equal("Flags must be some combination of None, Proxy, NoServiceNameCheck, AllowDotlessSpn, ProxyCohosting\r\n", exception.Message);

                ConfigurationElementCollection extendedProtectionCollection = extendedProtectionElement.GetCollection();

                ConfigurationElement spnElement = extendedProtectionCollection.CreateElement("spn");
                spnElement["name"] = @"HTTP/www.contoso.com";
                extendedProtectionCollection.Add(spnElement);

                ConfigurationElement spnElement1 = extendedProtectionCollection.CreateElement("spn");
                spnElement1["name"] = @"HTTP/contoso.com";
                extendedProtectionCollection.Add(spnElement1);

                server.CommitChanges();

                ConfigurationSection rulesSection = config.GetSection("system.webServer/rewrite/rules");
                ConfigurationElementCollection rulesCollection = rulesSection.GetCollection();
                Assert.Equal(1, rulesCollection.Count);

                ConfigurationElement ruleElement = rulesCollection[0];
                Assert.Equal("lextudio2", ruleElement["name"]);
#if IIS
                Assert.Equal(0, ruleElement["patternSyntax"]);
#else
                Assert.Equal(0L, ruleElement["patternSyntax"]);
#endif
                Assert.Equal(true, ruleElement["stopProcessing"]);

                ConfigurationElement matchElement = ruleElement.GetChildElement("match");
                Assert.Equal(@"(.*)", matchElement["url"]);

                ConfigurationElement actionElement = ruleElement.GetChildElement("action");
#if IIS
                Assert.Equal(1, actionElement["type"]);
#else
                Assert.Equal(1L, actionElement["type"]);
#endif
                Assert.Equal("/www/{R:0}", actionElement["url"]);
            }

            // remove application pool
            Assert.False(server.ApplicationPools.AllowsRemove);
            Assert.Equal(5, server.ApplicationPools.Count);
            server.ApplicationPools.RemoveAt(4);
            Assert.Equal(4, server.ApplicationPools.Count);

            // remove binding
            server.Sites[1].Bindings.RemoveAt(1);

            // remove site
            server.Sites.RemoveAt(4);

            // remove application
            var site1 = server.Sites[9];
            site1.Applications.RemoveAt(1);

            // remove virtual directory
            var application = server.Sites[2].Applications[0];
            application.VirtualDirectories.RemoveAt(1);

            server.CommitChanges();
        }

        private enum ResponseMode
        {
            File = 0,
            Test = 1
        }
    }
}

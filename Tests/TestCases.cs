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
        public static void IisExpress(ServerManager server, string fileName)
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
            {
                var binding = site.Bindings[0];
                Assert.Equal(IPAddress.Any, binding.EndPoint.Address);
                Assert.Equal(8080, binding.EndPoint.Port);
                Assert.Equal("localhost", binding.Host);
                Assert.Equal("*:8080:localhost", binding.ToString());
                Assert.Equal(":8080:localhost", binding.BindingInformation);
            }
            {
                var siteBinding = site.Bindings[1];
                Assert.Null(siteBinding.EndPoint);
                Assert.Equal(string.Empty, siteBinding.Host);
                Assert.False(siteBinding.IsIPPortHostBinding);
                Assert.False(siteBinding.UseDsMapper);
                Assert.Null(siteBinding.CertificateHash);
                Assert.Null(siteBinding.CertificateStoreName);
                Assert.Equal("808:* (net.tcp)", siteBinding.ToShortString());
            }
            {
                var siteBinding = site.Bindings[2];
                Assert.Null(siteBinding.EndPoint);
                Assert.Equal(string.Empty, siteBinding.Host);
                Assert.False(siteBinding.IsIPPortHostBinding);
                Assert.False(siteBinding.UseDsMapper);
                Assert.Null(siteBinding.CertificateHash);
                Assert.Null(siteBinding.CertificateStoreName);
                Assert.Equal("localhost (net.msmq)", siteBinding.ToShortString());
            }
            {
                var siteBinding = site.Bindings[3];
                Assert.Null(siteBinding.EndPoint);
                Assert.Equal(string.Empty, siteBinding.Host);
                Assert.False(siteBinding.IsIPPortHostBinding);
                Assert.False(siteBinding.UseDsMapper);
                Assert.Null(siteBinding.CertificateHash);
                Assert.Null(siteBinding.CertificateStoreName);
                Assert.Equal("localhost (msmq.formatname)", siteBinding.ToShortString());
            }
            {
                var siteBinding = site.Bindings[4];
                Assert.Null(siteBinding.EndPoint);
                Assert.Equal(string.Empty, siteBinding.Host);
                Assert.False(siteBinding.IsIPPortHostBinding);
                Assert.False(siteBinding.UseDsMapper);
                Assert.Null(siteBinding.CertificateHash);
                Assert.Null(siteBinding.CertificateStoreName);
                Assert.Equal("* (net.pipe)", siteBinding.ToShortString());
            }
            {
                var siteBinding = site.Bindings[5];
                Assert.Null(siteBinding.EndPoint);
                Assert.Equal(string.Empty, siteBinding.Host);
                Assert.False(siteBinding.IsIPPortHostBinding);
                Assert.False(siteBinding.UseDsMapper);
                Assert.Null(siteBinding.CertificateHash);
                Assert.Null(siteBinding.CertificateStoreName);
                Assert.Equal("*:21: (ftp)", siteBinding.ToShortString());
            }
            {
                var siteBinding = site.Bindings[6];
                Assert.Equal("* on *:8080 (http)", siteBinding.ToShortString());
                Assert.Equal("0.0.0.0:8080", siteBinding.EndPoint.ToString());
                Assert.Equal("*", siteBinding.Host);
            }
            {
                var siteBinding = site.Bindings[7];
                Assert.Equal("localhost on *:443 (https)", siteBinding.ToShortString());
                Assert.Equal("0.0.0.0:443", siteBinding.EndPoint.ToString());
                Assert.Equal("localhost", siteBinding.Host);
            }
            {
                var siteBinding = site.Bindings[8];
                Assert.Equal("* on *:44300 (https)", siteBinding.ToShortString());
                Assert.Equal("0.0.0.0:44300", siteBinding.EndPoint.ToString());
                Assert.Equal("*", siteBinding.Host);
            }
            {
                var siteBinding = site.Bindings[9];
                Assert.Null(siteBinding.EndPoint);
                Assert.Equal(string.Empty, siteBinding.Host);
                Assert.False(siteBinding.IsIPPortHostBinding);
                Assert.False(siteBinding.UseDsMapper);
                Assert.Null(siteBinding.CertificateHash);
                Assert.Null(siteBinding.CertificateStoreName);
                Assert.Equal("*:210:localhost (ftp)", siteBinding.ToShortString());
            }
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
#if IIS
                Assert.Equal(
                    "Filename: \r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/Default Web Site'\r\n\r\n",
                    exception.Message);
                Assert.Null(exception.FileName);
#else
                Assert.Equal(
                    $"Filename: \\\\?\\{fileName}\r\nError: Unrecognized configuration path 'MACHINE/WEBROOT/APPHOST/Default Web Site'\r\n\r\n",
                    exception.Message);
                Assert.Equal(fileName, exception.FileName);
#endif
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
                Assert.False(windowsSection.IsLocked);
                Assert.True(windowsSection.IsLocallyStored);

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
#if IIS
            Assert.Equal("Invalid application path\r\n", appPath.Message);
#else
            Assert.Equal("Invalid application path \r\n", appPath.Message);
#endif
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
                Assert.True(windowsSection.IsLocked);
                Assert.False(windowsSection.IsLocallyStored);

                var windowsEnabled = (bool)windowsSection["enabled"];
                Assert.True(windowsEnabled);
                var exception = Assert.Throws<FileLoadException>(() => windowsSection["enabled"] = false);
                Assert.Equal(
                    "This configuration section cannot be used at this path. This happens when the section is locked at a parent level. Locking is either by default (overrideModeDefault=\"Deny\"), or set explicitly by a location tag with overrideMode=\"Deny\" or the legacy allowOverride=\"false\".\r\n",
                    exception.Message);
                Assert.Null(exception.FileName);

                var compression = config.GetSection("system.webServer/urlCompression");
                Assert.Equal(OverrideMode.Inherit, compression.OverrideMode);
                Assert.Equal(OverrideMode.Allow, compression.OverrideModeEffective);
                Assert.False(compression.IsLocked);
                Assert.False(compression.IsLocallyStored);

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
                Assert.False(errorsSection.IsLocked);
                Assert.False(errorsSection.IsLocallyStored);

                var errorsCollection = errorsSection.GetCollection();
                Assert.Equal(9, errorsCollection.Count);

                var error = errorsCollection.CreateElement();
                var cast = Assert.Throws<InvalidCastException>(() => error.SetAttributeValue("statusCode", "500"));
#if IIS
                Assert.Equal(Helper.IsRunningOnMono() ? "Cannot cast from source type to destination type." : "Specified cast is not valid.", cast.Message);
#else
                Assert.Equal(Helper.IsRunningOnMono() ? "Cannot cast from source type to destination type." : "Cannot convert 500 of System.String to uint.", cast.Message);
#endif

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
                Assert.Null(ex1.FileName);

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
                Assert.Empty(collection);

                newElement = collection.CreateElement();
                newElement["name"] = "test";
                collection.Add(newElement);
                Assert.Single(collection);
            }

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, windowsSection.OverrideMode);
                Assert.Equal(OverrideMode.Deny, windowsSection.OverrideModeEffective);
                Assert.False(windowsSection.IsLocked);
                Assert.True(windowsSection.IsLocallyStored);

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
                Assert.Single(rulesCollection);

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

        public static void IisExpressMissingWebsiteConfig(ServerManager server)
        {
            {
                var config = server.GetApplicationHostConfiguration();

                var windowsSection =
                    config.GetSection("system.webServer/security/authentication/windowsAuthentication");
                windowsSection["enabled"] = true;
                server.CommitChanges();
            }
            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication",
                    "WebSite1");
                windowsSection["enabled"] = false;
                {
                    // disable logging. Saved in applicationHost.config, as it cannot be overridden in web.config.
                    ConfigurationSection httpLoggingSection =
                        config.GetSection("system.webServer/httpLogging", "WebSite1");
                    httpLoggingSection["dontLog"] = true;
                }

                {
                    ConfigurationSection httpLoggingSection =
                        config.GetSection("system.webServer/httpLogging", "WebSite1/test");
                    httpLoggingSection["dontLog"] = false;
                }
            }
            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();
                var compression = config.GetSection("system.webServer/urlCompression");
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

                var errorsSection = config.GetSection("system.webServer/httpErrors");
                var errorsCollection = errorsSection.GetCollection();
                var error = errorsCollection.CreateElement();
                error["statusCode"] = 500;
                error["subStatusCode"] = 55;
                error["prefixLanguageFilePath"] = string.Empty;
                error["responseMode"] = "File";
                error["path"] = "test.htm";
                errorsCollection.Add(error);

                var staticContent = config.GetSection("system.webServer/staticContent").GetChildElement("clientCache");
                staticContent["cacheControlMode"] = "DisableCache";

                ConfigurationSection requestFilteringSection =
                    config.GetSection("system.webServer/security/requestFiltering");
                ConfigurationElement hiddenSegmentsElement = requestFilteringSection.GetChildElement("hiddenSegments");
                ConfigurationElementCollection hiddenSegmentsCollection = hiddenSegmentsElement.GetCollection();
                var test = hiddenSegmentsCollection.CreateElement();
                test["segment"] = "test";
                hiddenSegmentsCollection.Add(test);
                var old = hiddenSegmentsCollection[0];
                hiddenSegmentsCollection.Remove(old);

                var section = config.GetSection("system.webServer/rewrite/rules");
                ConfigurationElementCollection collection = section.GetCollection();
                collection.Clear();
                var newElement = collection.CreateElement();
                newElement["name"] = "test";
                collection.Add(newElement);
            }

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();
                // enable Windows authentication
                var windowsSection = config.GetSection("system.webServer/security/authentication/windowsAuthentication",
                    "WebSite1");
                var windowsEnabled = (bool)windowsSection["enabled"];
            }
            {
                Configuration config = server.GetApplicationHostConfiguration();

                var anonymousSection = config.GetSection(
                    "system.webServer/security/authentication/anonymousAuthentication",
                    "WebSite2");
                ConfigurationSection windowsAuthenticationSection =
                    config.GetSection("system.webServer/security/authentication/windowsAuthentication", "WebSite2");
                windowsAuthenticationSection["enabled"] = true;

                ConfigurationElement extendedProtectionElement =
                    windowsAuthenticationSection.GetChildElement("extendedProtection");
                extendedProtectionElement["tokenChecking"] = @"Allow";
                extendedProtectionElement["flags"] = @"None";
                ConfigurationElementCollection extendedProtectionCollection =
                    extendedProtectionElement.GetCollection();

                ConfigurationElement spnElement = extendedProtectionCollection.CreateElement("spn");
                spnElement["name"] = @"HTTP/www.contoso.com";
                extendedProtectionCollection.Add(spnElement);

                ConfigurationElement spnElement1 = extendedProtectionCollection.CreateElement("spn");
                spnElement1["name"] = @"HTTP/contoso.com";
                extendedProtectionCollection.Add(spnElement1);

                server.CommitChanges();
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

        public static void IisExpressHandlers(ServerManager server)
        {
            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.True(handlersSection.IsLocallyStored);
            }

            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers");
                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.False(handlersSection.IsLocallyStored);
            }
        }

        public static void IisExpressLocation(ServerManager server)
        {
            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers", "WebSite1");

                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.True(handlersSection.IsLocallyStored);

                var handlers = handlersSection.GetCollection();
                Assert.Equal(82, handlers.Count);
                var newHandler = handlers.CreateElement();
                newHandler["name"] = "WebDAV";
                newHandler["path"] = "*";
                newHandler["verb"] = "PROPFIND,PROPPATCH,MKCOL,PUT,COPY,DELETE,MOVE,LOCK,UNLOCK";
                newHandler["modules"] = "WedDAVModule";
                newHandler["resourceType"] = "Unspecified";
                handlers.Add(newHandler);

                Assert.Equal(83, handlers.Count);
            }

            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers");
                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.False(handlersSection.IsLocallyStored);

                var handlers = handlersSection.GetCollection();
                Assert.Equal(82, handlers.Count);
            }

            // IMPORTANT: changes go to <location path="WebSites1"> in applicationHost.config.
            server.CommitChanges();

            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers");
                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.True(handlersSection.IsLocallyStored);

                var handlers = handlersSection.GetCollection();
                Assert.Equal(83, handlers.Count);
            }
        }

        public static void IisExpressInheritance(ServerManager server)
        {
            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers", "WebSite1");
                Assert.Equal("system.webServer/handlers", handlersSection.SectionPath);
#if !IIS
                Assert.Equal("WebSite1", handlersSection.Location);
                Assert.EndsWith("applicationHost.config", handlersSection.FileContext.FileName);

                var handlersInEmpty = handlersSection.GetParentElement().Section;
                Assert.Equal("system.webServer/handlers", handlersInEmpty.SectionPath);
                Assert.Equal("", handlersInEmpty.Location);
                Assert.EndsWith("applicationHost.config", handlersInEmpty.FileContext.FileName);

                var handlersInNull = handlersInEmpty.GetParentElement().Section;
                Assert.Equal("system.webServer/handlers", handlersInNull.SectionPath);
                Assert.Null(handlersInNull.Location);
                Assert.EndsWith("applicationHost.config", handlersInNull.FileContext.FileName);

                Assert.Null(handlersInNull.GetParentElement());

                var handlers2 = handlersInNull.GetCollection();
                Assert.Empty(handlers2);
                var handlers1 = handlersInEmpty.GetCollection();
                Assert.Equal(82, handlers1.Count);
#endif
                var handlers = handlersSection.GetCollection();
                Assert.Equal(82, handlers.Count);

                var newHandler = handlers.CreateElement();
                newHandler["name"] = "WebDAV";
                newHandler["path"] = "*";
                newHandler["verb"] = "PROPFIND,PROPPATCH,MKCOL,PUT,COPY,DELETE,MOVE,LOCK,UNLOCK";
                newHandler["modules"] = "WedDAVModule";
                newHandler["resourceType"] = "Unspecified";
                handlers.Add(newHandler);

                Assert.Equal(83, handlers.Count);
            }

            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers");
                Assert.Equal("system.webServer/handlers", handlersSection.SectionPath);
#if !IIS
                Assert.Equal("WebSite1", handlersSection.Location);
                Assert.EndsWith("web.config", handlersSection.FileContext.FileName);

                var handlersInWebSite = handlersSection.GetParentElement().Section;
                Assert.Equal("system.webServer/handlers", handlersInWebSite.SectionPath);
                Assert.Equal("WebSite1", handlersInWebSite.Location);
                Assert.EndsWith("applicationHost.config", handlersInWebSite.FileContext.FileName);

                var handlersInEmpty = handlersInWebSite.GetParentElement().Section;
                Assert.Equal("system.webServer/handlers", handlersInEmpty.SectionPath);
                Assert.Equal("", handlersInEmpty.Location);
                Assert.EndsWith("applicationHost.config", handlersInEmpty.FileContext.FileName);

                var handlersInNull = handlersInEmpty.GetParentElement().Section;
                Assert.Equal("system.webServer/handlers", handlersInNull.SectionPath);
                Assert.Null(handlersInNull.Location);
                Assert.EndsWith("applicationHost.config", handlersInNull.FileContext.FileName);

                Assert.Null(handlersInNull.GetParentElement());

                var handlers2 = handlersInNull.GetCollection();
                Assert.Empty(handlers2);
                var handlers1 = handlersInEmpty.GetCollection();
                Assert.Equal(82, handlers1.Count);
                var handlers3 = handlersInWebSite.GetCollection();
                Assert.Equal(82, handlers3.Count);
#endif
                var handlers = handlersSection.GetCollection();
                Assert.Equal(82, handlers.Count);
            }

            // IMPORTANT: changes go to <location path="WebSites1"> in applicationHost.config.
            server.CommitChanges();

            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers");
                Assert.Equal("system.webServer/handlers", handlersSection.SectionPath);
#if !IIS
                Assert.Equal("WebSite1", handlersSection.Location);
                Assert.EndsWith("applicationHost.config", handlersSection.FileContext.FileName);

                var handlersInEmpty = handlersSection.GetParentElement().Section;
                Assert.Equal("system.webServer/handlers", handlersInEmpty.SectionPath);
                Assert.Equal("", handlersInEmpty.Location);
                Assert.EndsWith("applicationHost.config", handlersInEmpty.FileContext.FileName);

                var handlersInNull = handlersInEmpty.GetParentElement().Section;
                Assert.Equal("system.webServer/handlers", handlersInNull.SectionPath);
                Assert.Null(handlersInNull.Location);
                Assert.EndsWith("applicationHost.config", handlersInNull.FileContext.FileName);

                Assert.Null(handlersInNull.GetParentElement());

                var handlers2 = handlersInNull.GetCollection();
                Assert.Empty(handlers2);
                var handlers1 = handlersInEmpty.GetCollection();
                Assert.Equal(82, handlers1.Count);
#endif
                var handlers = handlersSection.GetCollection();
                Assert.Equal(83, handlers.Count);
            }
        }

        public static void IisExpressLocation2(ServerManager server)
        {
            {
                // site config "Website1"
                var config = server.Sites[0].Applications[0].GetWebConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers");
                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.False(handlersSection.IsLocallyStored);

                var handlers = handlersSection.GetCollection();
                Assert.Equal(82, handlers.Count);
                var newHandler = handlers.CreateElement();
                newHandler["name"] = "WebDAV";
                newHandler["path"] = "*";
                newHandler["verb"] = "PROPFIND,PROPPATCH,MKCOL,PUT,COPY,DELETE,MOVE,LOCK,UNLOCK";
                newHandler["modules"] = "WedDAVModule";
                newHandler["resourceType"] = "Unspecified";
                handlers.Add(newHandler);

                Assert.Equal(83, handlers.Count);
            }

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                // enable Windows authentication
                var handlersSection = config.GetSection("system.webServer/handlers", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.True(handlersSection.IsLocallyStored);

                var handlers = handlersSection.GetCollection();
                Assert.Equal(82, handlers.Count);
            }

            // IMPORTANT: changes go to web.config.
            server.CommitChanges();

            {
                // server config "Website1"
                var config = server.GetApplicationHostConfiguration();

                var handlersSection = config.GetSection("system.webServer/handlers", "WebSite1");
                Assert.Equal(OverrideMode.Inherit, handlersSection.OverrideMode);
                Assert.Equal(OverrideMode.Allow, handlersSection.OverrideModeEffective);
                Assert.False(handlersSection.IsLocked);
                Assert.True(handlersSection.IsLocallyStored);

                var handlers = handlersSection.GetCollection();
                Assert.Equal(82, handlers.Count);
            }
        }

        public static void IisSiteDefaults(ServerManager server)
        {
            var siteDefaults = server.SiteDefaults.TraceFailedRequestsLogging;
            Assert.True(siteDefaults.Enabled);

            var site = server.Sites[0].TraceFailedRequestsLogging;
            Assert.True(site.Enabled);
            var attribute = site.GetAttribute("enabled");
            Assert.Equal(false, attribute.Schema.DefaultValue);
            Assert.False(attribute.IsInheritedFromDefaultValue);

            attribute.Value = false;
            Assert.Equal(false, attribute.Value);
        }

        private enum ResponseMode
        {
            Test = 1
        }
    }
}

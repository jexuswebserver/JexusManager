// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Tests.Main
{
    using System;
    using System.IO;
    using System.Reflection;

    using Microsoft.Web.Administration;

    using Xunit;

    public class SiteTestFixture
    {
        [Fact]
        public async void AddSite()
        {
            const string Current = @"applicationHost.config";
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

            var serverManager = new ServerManager(Current) { Mode = WorkingMode.IisExpress };

            Configuration config = serverManager.GetApplicationHostConfiguration();
            ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");
            ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

            ConfigurationElement siteElement = sitesCollection.CreateElement("site");
            siteElement["name"] = @"Contoso";
            siteElement["id"] = 2;
            siteElement["serverAutoStart"] = true;

            ConfigurationElementCollection bindingsCollection = siteElement.GetCollection("bindings");
            ConfigurationElement bindingElement = bindingsCollection.CreateElement("binding");
            bindingElement["protocol"] = @"http";
            bindingElement["bindingInformation"] = @"*:80:www.contoso.com";
            bindingsCollection.Add(bindingElement);

            ConfigurationElementCollection siteCollection = siteElement.GetCollection();
            ConfigurationElement applicationElement = siteCollection.CreateElement("application");
            applicationElement["path"] = @"/";
            ConfigurationElementCollection applicationCollection = applicationElement.GetCollection();
            ConfigurationElement virtualDirectoryElement = applicationCollection.CreateElement("virtualDirectory");
            virtualDirectoryElement["path"] = @"/";
            virtualDirectoryElement["physicalPath"] = @"C:\Inetpub\www.contoso.com\wwwroot";
            applicationCollection.Add(virtualDirectoryElement);
            siteCollection.Add(applicationElement);
            sitesCollection.Add(siteElement);

            serverManager.CommitChanges();

            const string Expected = @"expected_site_add.config";
            const string ExpectedMono = @"expected_site_add.mono.config";

            XmlAssert.Equal(
                Helper.IsRunningOnMono()
                    ? Path.Combine("Main", ExpectedMono)
                    : Path.Combine("Main", Expected),
                Current);
        }

        [Fact]
        public async void AddSiteManaged()
        {
            const string Current = @"applicationHost.config";
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

            var serverManager = new ServerManager(Current) { Mode = WorkingMode.IisExpress };

            var sites = serverManager.Sites;
            var site = new Site(sites);
            site.Name = @"Contoso";
            site.Id = 2L;
            site.ServerAutoStart = true;

            var binding = new Binding(@"http", @"*:80:www.contoso.com", null, null, SslFlags.None, site.Bindings);
            site.Bindings.Add(binding);

            var application = new Application(site.Applications);
            application.Path = @"/";

            site.Applications.Add(application);

            var directory = new VirtualDirectory(null, application.VirtualDirectories);
            directory.Path = @"/";
            directory.PhysicalPath = @"C:\Inetpub\www.contoso.com\wwwroot";

            application.VirtualDirectories.Add(directory);

            sites.Add(site);

            serverManager.CommitChanges();

            const string Expected = @"expected_site_add.config";
            const string ExpectedMono = @"expected_site_add.mono.config";

            XmlAssert.Equal(
                Helper.IsRunningOnMono()
                    ? Path.Combine("Main", ExpectedMono)
                    : Path.Combine("Main", Expected),
                Current);
        }
    }
}

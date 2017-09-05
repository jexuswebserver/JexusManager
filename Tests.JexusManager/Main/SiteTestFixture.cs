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
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class SiteTestFixture
    {
        [Fact]
        public void AddSite()
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

            var serverManager = new IisExpressServerManager(Current);

            const string Expected = @"expected_site_add.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.applicationHost/sites");
            var siteNode = new XElement("site");
            siteNode.SetAttributeValue("name", "Contoso");
            siteNode.SetAttributeValue("id", "2");
            var bindingsNode = new XElement("bindings");
            siteNode.Add(bindingsNode);
            node?.Add(siteNode);

            var bindingNode = new XElement("binding");
            bindingNode.SetAttributeValue("protocol", "http");
            bindingNode.SetAttributeValue("bindingInformation", @"*:80:www.contoso.com");
            bindingsNode.Add(bindingNode);

            var appNode = new XElement("application");
            appNode.SetAttributeValue("path", "/");
            siteNode.Add(appNode);
            var vDirNode = new XElement("virtualDirectory");
            vDirNode.SetAttributeValue("path", "/");
            vDirNode.SetAttributeValue("physicalPath", @"C:\Inetpub\www.contoso.com\wwwroot");
            appNode.Add(vDirNode);

            document.Save(Expected);

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

            XmlAssert.Equal(Expected, Current);
        }

        [Fact]
        public void AddSiteManaged()
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

            var serverManager = new IisExpressServerManager(Current);

            const string Expected = @"expected_site_add.config";
            var document = XDocument.Load(Current);
            var node = document.Root.XPathSelectElement("/configuration/system.applicationHost/sites");
            var siteNode = new XElement("site");
            siteNode.SetAttributeValue("name", "Contoso");
            siteNode.SetAttributeValue("id", "2");
            var bindingsNode = new XElement("bindings");
            siteNode.Add(bindingsNode);
            node?.Add(siteNode);

            var bindingNode = new XElement("binding");
            bindingNode.SetAttributeValue("protocol", "http");
            bindingNode.SetAttributeValue("bindingInformation", @"*:80:www.contoso.com");
            bindingsNode.Add(bindingNode);

            var appNode = new XElement("application");
            appNode.SetAttributeValue("path", "/");
            siteNode.Add(appNode);
            var vDirNode = new XElement("virtualDirectory");
            vDirNode.SetAttributeValue("path", "/");
            vDirNode.SetAttributeValue("physicalPath", @"C:\Inetpub\www.contoso.com\wwwroot");
            appNode.Add(vDirNode);

            document.Save(Expected);

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

            XmlAssert.Equal(Expected, Current);
        }
    }
}

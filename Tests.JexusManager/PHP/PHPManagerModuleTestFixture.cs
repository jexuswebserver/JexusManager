// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using JexusManager.Services;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Server;
using Web.Management.PHP;
using Xunit;
using Module = Microsoft.Web.Management.Client.Module;

namespace Tests.PHP
{
    public sealed class PHPManagerModuleTestFixture
    {
        private sealed class TestModule : Module
        {
        }

        [Fact]
        public void PHPProviderSupportsServerAndSiteScopes()
        {
            var provider = new PHPProvider();
            Assert.True(provider.SupportsScope(ManagementScope.Server));
            Assert.True(provider.SupportsScope(ManagementScope.Site));
        }

        [Fact]
        public void PHPProviderResolvesClientModule()
        {
            var provider = new PHPProvider();
            var definition = provider.GetModuleDefinition(null);
            Assert.Equal(typeof(PHPService), provider.ServiceType);
            var type = Type.GetType(definition.ClientModuleTypeName);
            Assert.NotNull(type);
            Assert.Equal(typeof(PHPModule).FullName, type.FullName);
        }

        [Fact]
        public void PHPModuleCanCallServiceOverInProcessChannel()
        {
            var (module, connection) = Create(ManagementScope.Server);
            var proxy = (PHPModuleProxy)connection.CreateProxy(module, typeof(PHPModuleProxy));

            var sites = proxy.GetSites();
            Assert.NotNull(sites);
            Assert.Contains("WebSite1", sites.Cast<string>());
        }

        [Fact]
        public void PHPModuleRegistersOneControlPanelPage()
        {
            var (module, connection) = Create(ManagementScope.Server);
            var controlPanel = (IControlPanel)((IServiceProvider)module).GetService(typeof(IControlPanel));
            var pages = controlPanel.GetPages(module);
            Assert.Equal(7, pages.Count);
            Assert.Single(controlPanel.Pages);
            Assert.Equal(typeof(PHPPage), controlPanel.Pages[0].PageType);
        }

        private static (Module Module, Connection Connection) Create(ManagementScope scope)
        {
            File.Copy("original.config", "applicationHost.config", true);
            File.Copy(Path.Combine("Website1", "original.config"), Path.Combine("Website1", "web.config"), true);
            Environment.SetEnvironmentVariable(
                "JEXUS_TEST_HOME",
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            var server = new IisExpressServerManager("applicationHost.config");
            var services = new ServiceContainer();
            IConfigurationService configurationService;
            if (scope == ManagementScope.Server)
            {
                configurationService = new ConfigurationService(
                    null,
                    server.GetApplicationHostConfiguration(),
                    scope,
                    server,
                    null,
                    null,
                    null,
                    null,
                    null);
            }
            else
            {
                var site = server.Sites[0];
                configurationService = new ConfigurationService(
                    null,
                    site.GetWebConfiguration(),
                    scope,
                    null,
                    site,
                    site.Applications[0],
                    null,
                    null,
                    site.Name);
            }

            services.AddService(typeof(IConfigurationService), configurationService);
            services.AddService(typeof(IControlPanel), new ControlPanel());
            var provider = new PHPProvider();
            var connection = InProcessConnectionFactory.Configure(services, configurationService, new[] { provider });
            var definition = provider.GetModuleDefinition(null);
            var type = Type.GetType(definition.ClientModuleTypeName);
            var module = (Module)Activator.CreateInstance(type);
            module.TestInitialize(services, (ModuleInfo)connection.Modules[definition.Name]);
            return (module, connection);
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using JexusManager.Features.HttpErrors;
using JexusManager.Services;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Server;
using Xunit;
using Module = Microsoft.Web.Management.Client.Module;

namespace Tests.HttpErrors
{
    public sealed class HttpErrorsServicePipelineTestFixture
    {
        private sealed class TestModule : Module
        {
        }

        [Fact]
        public void HttpErrorsServiceCanRoundTripSettingsThroughProxy()
        {
            var (module, connection) = Create(ManagementScope.Server);
            var proxy = (HttpErrorsModuleProxy)connection.CreateProxy(module, typeof(HttpErrorsModuleProxy));

            var settings = new HttpErrorsSettings
            {
                ErrorMode = 1L,
                DefaultResponseMode = 2L,
                DefaultPath = "C:/inetpub/wwwroot/error.htm"
            };

            proxy.ApplySettings(settings);
            var result = proxy.GetSettings();

            Assert.Equal(settings.ErrorMode, result.ErrorMode);
            Assert.Equal(settings.DefaultResponseMode, result.DefaultResponseMode);
            Assert.Equal(settings.DefaultPath, result.DefaultPath);
        }

        private static (TestModule Module, Connection Connection) Create(ManagementScope scope)
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
            var provider = new HttpErrorsModuleProvider();
            var connection = InProcessConnectionFactory.Configure(services, configurationService, new[] { provider });
            var definition = provider.GetModuleDefinition(null);
            var module = new TestModule();
            module.TestInitialize(services, new ModuleInfo(definition.Name, definition.ClientModuleTypeName));
            return (module, connection);
        }
    }
}

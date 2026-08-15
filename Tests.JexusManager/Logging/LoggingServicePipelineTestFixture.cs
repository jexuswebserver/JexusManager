// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using JexusManager.Features.Logging;
using JexusManager.Services;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Server;
using Xunit;
using Module = Microsoft.Web.Management.Client.Module;

namespace Tests.Logging
{
    public sealed class LoggingServicePipelineTestFixture
    {
        private sealed class TestModule : Module
        {
        }

        [Fact]
        public void LoggingServiceCanRoundTripSettingsThroughProxy()
        {
            var (module, connection) = Create(ManagementScope.Server);
            var proxy = (LoggingModuleProxy)connection.CreateProxy(module, typeof(LoggingModuleProxy));

            var settings = new LoggingSettings
            {
                Enabled = true,
                Mode = 1,
                Encoding = 1,
                LogFormat = 2,
                Directory = "C:/logs",
                LogTargetW3C = 3,
                LocalTimeRollover = true,
                TruncateSizeString = "1048576",
                Period = 4
            };

            proxy.Apply(settings);
            var result = proxy.GetSettings();

            Assert.Equal(settings.Enabled, result.Enabled);
            Assert.Equal(settings.Mode, result.Mode);
            Assert.Equal(settings.Encoding, result.Encoding);
            Assert.Equal(settings.LogFormat, result.LogFormat);
            Assert.Equal(settings.Directory, result.Directory);
            Assert.Equal(settings.LogTargetW3C, result.LogTargetW3C);
            Assert.Equal(settings.LocalTimeRollover, result.LocalTimeRollover);
            Assert.Equal(settings.TruncateSizeString, result.TruncateSizeString);
            Assert.Equal(settings.Period, result.Period);
        }

        [Fact]
        public void LoggingServiceCanRoundTripSettingsThroughProxyForSiteScope()
        {
            var (module, connection) = Create(ManagementScope.Site);
            var proxy = (LoggingModuleProxy)connection.CreateProxy(module, typeof(LoggingModuleProxy));

            var settings = new LoggingSettings
            {
                Enabled = true,
                Mode = 1,
                Encoding = 1,
                LogFormat = 2,
                Directory = "C:/logs",
                LogTargetW3C = 3,
                LocalTimeRollover = true,
                TruncateSizeString = "1048576",
                Period = 4
            };

            proxy.Apply(settings);
            var result = proxy.GetSettings();

            Assert.Equal(settings.Enabled, result.Enabled);
            Assert.Equal(0, result.Mode);
            Assert.Equal(0, result.Encoding);
            Assert.Equal(0, result.LogFormat);
            Assert.Equal(string.Empty, result.Directory);
            Assert.Equal(-1, result.LogTargetW3C);
            Assert.False(result.LocalTimeRollover);
            Assert.Equal(string.Empty, result.TruncateSizeString);
            Assert.Equal(0, result.Period);
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
            var provider = new LoggingModuleProvider();
            var connection = InProcessConnectionFactory.Configure(services, configurationService, new[] { provider });
            var definition = provider.GetModuleDefinition(null);
            var module = new TestModule();
            module.TestInitialize(services, new ModuleInfo(definition.Name, definition.ClientModuleTypeName));
            return (module, connection);
        }
    }
}

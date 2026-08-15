// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel.Design;
using System.IO;
using System.Reflection;
using JexusManager.Services;
using Microsoft.Web.Administration;
using Microsoft.Web.Management.Client;
using Microsoft.Web.Management.Server;
using Xunit;
using Module = Microsoft.Web.Management.Client.Module;

namespace Tests.Management
{
    public sealed class ModuleServicePipelineTestFixture
    {
        private sealed class TestModule : Module
        {
        }

        private sealed class TestProxy : ModuleServiceProxy
        {
            public string Echo(string value)
            {
                return (string)Invoke(nameof(Echo), value);
            }

            public object Hidden()
            {
                return Invoke(nameof(Hidden));
            }
        }

        private sealed class TestService : ModuleService
        {
            [ModuleServiceMethod]
            public string Echo(string value)
            {
                return $"{ManagementUnit.Scope}:{value}";
            }

            public string Hidden()
            {
                return "not callable";
            }
        }

        private sealed class TestProvider : ModuleProvider
        {
            public override ModuleDefinition GetModuleDefinition(IManagementContext context)
            {
                return new ModuleDefinition(Name, typeof(TestModule).AssemblyQualifiedName);
            }

            public override bool SupportsScope(ManagementScope scope)
            {
                return scope == ManagementScope.Server;
            }

            public override Type ServiceType => typeof(TestService);
        }

        [Fact]
        public void MarkedMethodIsDispatchedThroughProxy()
        {
            var (module, connection) = Create(ManagementScope.Server);
            var proxy = (TestProxy)connection.CreateProxy(module, typeof(TestProxy));

            Assert.Equal("Server:hello", proxy.Echo("hello"));
        }

        [Fact]
        public void UnmarkedMethodIsRejected()
        {
            var (module, connection) = Create(ManagementScope.Server);
            var proxy = (TestProxy)connection.CreateProxy(module, typeof(TestProxy));

            var exception = Assert.Throws<ModuleServiceException>(() => proxy.Hidden());
            Assert.Equal("MethodNotAvailable", exception.ErrorCode);
        }

        [Fact]
        public void ProviderScopeIsEnforcedByServerDispatch()
        {
            var (module, connection) = Create(ManagementScope.Site);
            var proxy = (TestProxy)connection.CreateProxy(module, typeof(TestProxy));

            var exception = Assert.Throws<ModuleServiceException>(() => proxy.Echo("hello"));
            Assert.Equal("ModuleNotAvailable", exception.ErrorCode);
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
            var provider = new TestProvider();
            var connection = InProcessConnectionFactory.Configure(services, configurationService, new[] { provider });
            var definition = provider.GetModuleDefinition(null);
            var module = new TestModule();
            module.TestInitialize(services, new ModuleInfo(definition.Name, definition.ClientModuleTypeName));
            return (module, connection);
        }
    }
}

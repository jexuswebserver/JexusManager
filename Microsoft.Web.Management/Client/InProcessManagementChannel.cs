// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;
using Microsoft.Web.Management.Server;

namespace Microsoft.Web.Management.Client
{
    public sealed class InProcessManagementChannel : ManagementChannel
    {
        private readonly ManagementUnit _managementUnit;

        public InProcessManagementChannel(ConnectionInfo connectionInfo, ManagementUnit managementUnit)
            : base(null, connectionInfo)
        {
            _managementUnit = managementUnit ?? throw new ArgumentNullException(nameof(managementUnit));
        }

        protected override void DownloadAssembly(AssemblyDownloadInfo info, string fileName)
        {
            throw new NotSupportedException("In-process connections do not download client assemblies.");
        }

        protected override object Invoke(string serviceName, string methodName, params object[] parameters)
        {
            var service = _managementUnit.GetModuleService(serviceName);
            var candidates = service.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => string.Equals(method.Name, methodName, StringComparison.Ordinal)
                    && method.GetCustomAttribute<ModuleServiceMethodAttribute>(inherit: true) != null
                    && ParametersMatch(method.GetParameters(), parameters))
                .ToArray();

            if (candidates.Length != 1)
            {
                throw new ModuleServiceException(
                    candidates.Length == 0
                        ? $"Operation '{serviceName}.{methodName}' is not available."
                        : $"Operation '{serviceName}.{methodName}' is ambiguous.",
                    candidates.Length == 0 ? "MethodNotAvailable" : "AmbiguousMethod");
            }

            try
            {
                return candidates[0].Invoke(service, parameters);
            }
            catch (TargetInvocationException exception) when (exception.InnerException != null)
            {
                throw exception.InnerException;
            }
        }

        private static bool ParametersMatch(ParameterInfo[] parameterInfo, object[] parameters)
        {
            if (parameterInfo.Length != parameters.Length)
            {
                return false;
            }

            for (var index = 0; index < parameterInfo.Length; index++)
            {
                var value = parameters[index];
                var parameterType = parameterInfo[index].ParameterType;
                if (value == null)
                {
                    if (parameterType.IsValueType && Nullable.GetUnderlyingType(parameterType) == null)
                    {
                        return false;
                    }

                    continue;
                }

                if (!parameterType.IsInstanceOfType(value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}

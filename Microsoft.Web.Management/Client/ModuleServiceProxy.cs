// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Resources;

namespace Microsoft.Web.Management.Client
{
    public abstract class ModuleServiceProxy
    {
        private Connection _connection;
        private string _serviceName;

        internal void Initialize(Connection connection, string serviceName)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _serviceName = string.IsNullOrWhiteSpace(serviceName)
                ? throw new ArgumentException("A module service name is required.", nameof(serviceName))
                : serviceName;
        }

        public static string GetErrorInformation(
            Exception ex,
            ResourceManager resourceManager,
            out string errorText,
            out string errorMessage
            )
        {
            errorText = string.Empty;
            errorMessage = string.Empty;
            return null;
        }

        protected Object Invoke(
            string methodName,
            params Object[] parameters
            )
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("The proxy has not been bound to a connection.");
            }

            return _connection.Invoke(_serviceName, methodName, parameters);
        }
    }
}

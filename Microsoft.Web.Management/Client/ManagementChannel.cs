// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public abstract class ManagementChannel
    {
        protected ManagementChannel(
            Connection connection,
            ConnectionInfo connectionInfo
            )
        {
            Connection = connection;
            ConnectionInfo = connectionInfo;
        }

        protected abstract void DownloadAssembly(
            AssemblyDownloadInfo info,
            string fileName
            );

        protected abstract Object Invoke(
            string serviceName,
            string methodName,
            params Object[] parameters
            );
        protected void SetLastRequestConfigurationInfo(
            bool configurationUsed,
            string configurationPath,
            string locationSubPath
            )
        { }

        protected Connection Connection { get; }
        protected ConnectionInfo ConnectionInfo { get; }
    }
}
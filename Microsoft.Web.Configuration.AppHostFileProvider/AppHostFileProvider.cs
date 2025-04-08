// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ClassInterface(ClassInterfaceType.None), ComVisible(true), Guid("0019ca4f-5c41-4fe4-bda7-8adfe5115e11"), ProgId("Microsoft.ApplicationHost.AppHostFileProvider")]
    public sealed class AppHostFileProvider : IAppHostProvider,
        IDisposable
    {
        public void AddChangeHandler(
            IAppHostProviderChangeHandler changeHandler
            )
        { }

        public void CommitChanges(
            string fileName,
            string sectionName,
            string configPath
            )
        { }

        public void Dispose()
        { }

        public IApplicationPool GetApplicationPool(
            string appPoolName
            )
        { throw new NotImplementedException(); }

        public IEnumApplicationPool GetApplicationPoolEnumerator()
        {
            throw new NotImplementedException();
        }

        public string GetConfigFile(
            string sectionName,
            string configPath
            )
        { throw new NotImplementedException(); }

        public IEnumListenerAdapter GetListenerAdapterEnumerator()
        {
            return null;
        }

        public IEnumLocation GetLocationEnumerator()
        {
            return null;
        }

        public IAppHostSite GetSite(
            string siteName
            )
        {
            return null;
        }

        public IAppHostSite GetSiteById(
            uint id
            )
        {
            return null;
        }

        public IEnumSite GetSiteEnumerator()
        {
            return null;
        }

        public void Initialize()
        { }

        public bool IsDynamicRegistrationEnabled()
        {
            return false;
        }

        public void Terminate()
        { }

        public string ConfigPath { get; set; }
        public bool ExpandEnvironmentVariables { get; set; }
        public LoadOptions FileLoadOptions { get; set; }
        public ILogSettings LogSettings { get; }
        public bool ServiceModel { get; set; }
        public string TempFilePath { get; set; }
        public IWebLimits WebLimits { get; }
    }
}

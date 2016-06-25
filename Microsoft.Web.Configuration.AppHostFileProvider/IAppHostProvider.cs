// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("81d9b25e-5ba5-4166-a69f-3f09a0fc3c65")]
    public interface IAppHostProvider
    {
        [DispId(11)]
        ILogSettings LogSettings
        {
            get;
        }
        [DispId(12)]
        IWebLimits WebLimits
        {
            get;
        }
        [DispId(13)]
        string ConfigPath
        {
            get;
            set;
        }
        [DispId(16)]
        bool ExpandEnvironmentVariables
        {
            get;
            set;
        }
        [DispId(17)]
        LoadOptions FileLoadOptions
        {
            get;
            set;
        }
        [DispId(18)]
        string TempFilePath
        {
            get;
            set;
        }
        [DispId(1)]
        void Initialize();
        [DispId(2)]
        IEnumSite GetSiteEnumerator();
        [DispId(3)]
        IEnumApplicationPool GetApplicationPoolEnumerator();
        [DispId(4)]
        IAppHostSite GetSite(string siteName);
        [DispId(5)]
        IAppHostSite GetSiteById(uint id);
        [DispId(6)]
        IApplicationPool GetApplicationPool(string appPoolName);
        [DispId(7)]
        void AddChangeHandler(IAppHostProviderChangeHandler changeHandler);
        [DispId(8)]
        string GetConfigFile(string sectionName, string configPath);
        [DispId(9)]
        void CommitChanges(string fileName, string sectionName, string configPath);
        [DispId(10)]
        IEnumListenerAdapter GetListenerAdapterEnumerator();
        [DispId(14)]
        IEnumLocation GetLocationEnumerator();
        [DispId(15)]
        bool IsDynamicRegistrationEnabled();
        [DispId(19)]
        void Terminate();
    }
}
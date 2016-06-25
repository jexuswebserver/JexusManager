// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("db759e1a-256b-4fc5-b240-8268ad2eddee")]
    public interface IAppHostSite
    {
        [DispId(1)]
        string Name
        {
            get;
        }
        [DispId(2)]
        uint Id
        {
            get;
        }
        [DispId(3)]
        bool ServerAutoStart
        {
            get;
        }
        [DispId(5)]
        ISiteLimits Limits
        {
            get;
        }
        [DispId(6)]
        ISiteLogFile LogFile
        {
            get;
        }
        [DispId(7)]
        ISiteTraceRequestsLogging TraceRequestsLogging
        {
            get;
        }
        [DispId(9)]
        IApplication ApplicationDefaults
        {
            get;
        }
        [DispId(10)]
        IVirtualDirectory VirtualDirectoryDefaults
        {
            get;
        }
        [DispId(11)]
        uint State
        {
            get;
        }
        [DispId(4)]
        IEnumBindings GetBindingsEnumerator();
        [DispId(8)]
        IEnumApplications GetApplicationsEnumerator();
    }
}

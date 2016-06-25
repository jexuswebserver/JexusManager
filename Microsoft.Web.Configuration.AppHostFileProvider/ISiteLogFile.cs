// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("3c686440-aad6-4624-815b-85ab57e02d92")]
    public interface ISiteLogFile
    {
        [DispId(1)]
        uint LogExtFileFlags
        {
            get;
        }
        [DispId(2)]
        string CustomLogPluginClsid
        {
            get;
        }
        [DispId(3)]
        uint LogFormat
        {
            get;
        }
        [DispId(4)]
        string Directory
        {
            get;
        }
        [DispId(5)]
        uint Period
        {
            get;
        }
        [DispId(6)]
        long TruncateSize
        {
            get;
        }
        [DispId(7)]
        bool LocalTimeRollover
        {
            get;
        }
        [DispId(8)]
        bool Enabled
        {
            get;
        }
        [DispId(9)]
        bool LogSiteID
        {
            get;
        }
        [DispId(10)]
        uint LogTargetW3C
        {
            get;
        }
        [DispId(11)]
        uint FlushByEntryCountW3CLog
        {
            get;
        }
        [DispId(12)]
        uint MaxLogLineLength
        {
            get;
        }
        [DispId(13)]
        ICustomFields CustomFields
        {
            get;
        }
    }
}
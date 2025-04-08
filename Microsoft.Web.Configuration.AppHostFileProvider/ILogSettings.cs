// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("b88f4753-99ce-40e9-bf83-fcb4c29efa2f")]
    public interface ILogSettings
    {
        [DispId(1)]
        bool LogInUtf8
        {
            get;
        }
        [DispId(2)]
        uint LogFileMode
        {
            get;
        }
        [DispId(3)]
        bool BinaryLogFileEnabled
        {
            get;
        }
        [DispId(4)]
        string BinaryLogFileDirectory
        {
            get;
        }
        [DispId(5)]
        uint BinaryLogFilePeriod
        {
            get;
        }
        [DispId(6)]
        long BinaryLogFileTruncateSize
        {
            get;
        }
        [DispId(7)]
        bool BinaryLogFileLocalTimeRollover
        {
            get;
        }
        [DispId(8)]
        bool W3CLogFileEnabled
        {
            get;
        }
        [DispId(9)]
        string W3CLogFileDirectory
        {
            get;
        }
        [DispId(10)]
        uint W3CLogFilePeriod
        {
            get;
        }
        [DispId(11)]
        long W3CLogFileTruncateSize
        {
            get;
        }
        [DispId(12)]
        bool W3CLogFileLocalTimeRollover
        {
            get;
        }
        [DispId(13)]
        uint W3CLogExtFileFlags
        {
            get;
        }
    }
}

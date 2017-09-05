// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;

namespace Microsoft.Web.Administration
{
    [Flags]
    public enum LogExtFileFlags
    {
        [Description("Date ( date )")]
        Date = 1,

        [Description("Time ( time )")]
        Time = 2,

        [Description("Client IP Address ( c-ip )")]
        ClientIP = 4,

        [Description("User Name ( cs-username )")]
        UserName = 8,

        [Description("Service Name ( s-sitename )")]
        SiteName = 16,

        [Description("Server Name ( s-computername )")]
        ComputerName = 32,

        [Description("Server IP Address ( s-ip )")]
        ServerIP = 64,

        [Description("Method ( cs-method )")]
        Method = 128,

        [Description("URI Stem ( cs-uri-stem )")]
        UriStem = 256,

        [Description("URI Query ( cs-uri-query )")]
        UriQuery = 512,

        [Description("Protocol ( sc-status )")]
        HttpStatus = 1024,

        [Description("Win32 Status ( sc-win32-status )")]
        Win32Status = 2048,

        [Description("Bytes Sent ( sc-bytes )")]
        BytesSent = 4096,

        [Description("Bytes Received ( cs-bytes )")]
        BytesRecv = 8192,

        [Description("Time Taken ( time-taken )")]
        TimeTaken = 16384,

        [Description("Server Port ( s-port )")]
        ServerPort = 32768,

        [Description("User Agent ( cs(User-Agent) )")]
        UserAgent = 65536,

        [Description("Cookie ( cs(Cookie) )")]
        Cookie = 131072,

        [Description("Referer ( cs(Referer) )")]
        Referer = 262144,

        [Description("Protocol Version ( cs-version )")]
        ProtocolVersion = 524288,

        [Description("Host ( cs-host )")]
        Host = 1048576,

        [Description("Protocol Substatus ( sc-substatus )")]
        HttpSubStatus = 2097152
    }
}

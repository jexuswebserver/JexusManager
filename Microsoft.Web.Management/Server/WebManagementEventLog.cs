// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

namespace Microsoft.Web.Management.Server
{
    public static class WebManagementEventLog
    {
        public static void Write(
            int eventId,
            string message
            )
        { }

        public static void Write(
            string message,
            EventLogEntryType entryType,
            int eventId
            )
        { }

        public static void Write(
            int eventId,
            string message,
            string connectionName,
            string connectionUser,
            Exception exception
            )
        { }

        public static void Write(
            int eventId,
            string message,
            string connectionName,
            string connectionUser,
            Exception exception,
            EventLogEntryType entryType
            )
        { }

        public static bool TraceOnly { get; set; }
    }
}

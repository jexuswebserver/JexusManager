// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Management;

namespace Microsoft.Web.Administration
{
    internal static class ProcessExtensions
    {
        public static string GetCommandLine(this Process process)
        {
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            {
                foreach (var @object in searcher.Get())
                {
                    return @object["CommandLine"] as string;
                }
            }

            return string.Empty;
        }
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Serilog;

namespace Microsoft.Web.Administration
{
    internal static class ProcessExtensions
    {
        /// <summary>
        /// Gets the command line of the specified process
        /// </summary>
        /// <param name="process">The process to get the command line from</param>
        /// <returns>The command line string if successful, null otherwise</returns>
        public static unsafe string GetCommandLine(this Process process)
        {
            if (process == null)
            {
                return null;
            }

            try
            {
                // Get the process handle with appropriate access rights
                using var handle = Windows.Win32.PInvoke.OpenProcess_SafeHandle(
                    Windows.Win32.System.Threading.PROCESS_ACCESS_RIGHTS.PROCESS_QUERY_INFORMATION | 
                    Windows.Win32.System.Threading.PROCESS_ACCESS_RIGHTS.PROCESS_VM_READ, 
                    false, 
                    (uint)process.Id);
                
                if (handle.IsInvalid)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error, $"Failed to open process {process.Id}");
                }

                // First try to get the command line using NtQueryInformationProcess
                string commandLine = TryGetCommandLineFromNtQuery(handle);
                if (!string.IsNullOrEmpty(commandLine))
                {
                    return commandLine;
                }
                
                // Fall back to getting the executable path if NtQueryInformationProcess fails
                return TryGetExecutablePath(handle);
            }
            catch (Exception ex)
            {
                // Log but don't crash - return null as original method did
                Log.Logger?.Debug(ex, "Error getting command line");
                return null;
            }
        }

        /// <summary>
        /// Attempts to get the command line using NtQueryInformationProcess
        /// </summary>
        private static unsafe string TryGetCommandLineFromNtQuery(Win32.SafeHandles.SafeFileHandle handle)
        {
            // Allocate unmanaged memory for the UNICODE_STRING structure
            IntPtr unicodeStringPtr = IntPtr.Zero;
            uint returnLength = 0;

            try
            {
                // Allocate memory for the UNICODE_STRING structure
                unicodeStringPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Windows.Win32.Foundation.UNICODE_STRING>());
                
                // Call NtQueryInformationProcess with ProcessCommandLineInformation
                var result = Windows.Wdk.PInvoke.NtQueryInformationProcess(
                    (Windows.Win32.Foundation.HANDLE)handle.DangerousGetHandle(),
                    Windows.Wdk.System.Threading.PROCESSINFOCLASS.ProcessCommandLineInformation,
                    (void*)unicodeStringPtr,
                    (uint)Marshal.SizeOf<Windows.Win32.Foundation.UNICODE_STRING>(),
                    ref returnLength);

                if (result == 0) // STATUS_SUCCESS
                {
                    // Marshal the result into a UNICODE_STRING structure
                    var unicodeString = Marshal.PtrToStructure<Windows.Win32.Foundation.UNICODE_STRING>(unicodeStringPtr);
                    
                    // Convert the buffer pointer to a managed string if it's valid
                    if (unicodeString.Buffer.Value != null && unicodeString.Length > 0)
                    {
                        // Length is in bytes, divide by 2 to get character count for UTF-16
                        return Marshal.PtrToStringUni((nint)unicodeString.Buffer.Value, unicodeString.Length / 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger?.Debug(ex, "Error in TryGetCommandLineFromNtQuery: {0}", ex.Message);
            }
            finally
            {
                // Free allocated memory to prevent leaks
                if (unicodeStringPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(unicodeStringPtr);
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to get the executable path of the process as a fallback
        /// </summary>
        private static string TryGetExecutablePath(Win32.SafeHandles.SafeFileHandle handle)
        {
            try
            {
                uint size = 1024;
                // Create a character buffer
                char[] buffer = new char[size];
                
                // Pin the buffer in memory so the GC won't move it during the API call
                GCHandle bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                
                try
                {
                    // Get the full path using QueryFullProcessImageName
                    if (Windows.Win32.PInvoke.QueryFullProcessImageName(handle, 0, buffer, ref size))
                    {
                        // Convert the result to a managed string
                        return new string(buffer, 0, (int)size);
                    }
                }
                finally
                {
                    // Always free the pinned buffer
                    bufferHandle.Free();
                }
            }
            catch (Exception ex)
            {
                Log.Logger?.Debug(ex, "Error in TryGetExecutablePath: {0}", ex.Message);
            }

            return null;
        }
    }
}

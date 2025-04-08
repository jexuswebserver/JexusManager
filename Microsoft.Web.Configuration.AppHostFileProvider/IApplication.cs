// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("40f6b041-8070-490e-bf92-f001ff7bb843")]
    public interface IApplication
    {
        [DispId(1)]
        string Path
        {
            get;
        }
        [DispId(2)]
        string ApplicationPool
        {
            get;
        }
        [DispId(3)]
        string EnabledProtocols
        {
            get;
        }
        [DispId(4)]
        bool ServiceAutoStartEnabled
        {
            get;
        }
        [DispId(5)]
        string ServiceAutoStartProvider
        {
            get;
        }
        [DispId(6)]
        bool PreloadEnabled
        {
            get;
        }
        [DispId(7)]
        IVirtualDirectory VirtualDirectoryDefaults
        {
            get;
        }
        [DispId(8)]
        IEnumVirtualDirectory GetVirtualDirectoryEnumerator();
    }
}

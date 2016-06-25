// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("e2c36157-96fe-46ef-9066-3b02cd29fe3c")]
    public interface IVirtualDirectory
    {
        [DispId(1)]
        string Path
        {
            get;
        }
        [DispId(2)]
        string PhysicalPath
        {
            get;
        }
        [DispId(3)]
        string UserName
        {
            get;
        }
        [DispId(4)]
        string Password
        {
            get;
        }
        [DispId(5)]
        uint LogonMethod
        {
            get;
        }
        [DispId(6)]
        bool AllowSubdirectoryConfig
        {
            get;
        }
    }
}
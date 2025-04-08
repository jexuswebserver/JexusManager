// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("ec2f2398-ce56-4e01-bc86-8aed057013a5")]
    public interface ILocation
    {
        [DispId(1)]
        string Path
        {
            get;
        }
    }
}

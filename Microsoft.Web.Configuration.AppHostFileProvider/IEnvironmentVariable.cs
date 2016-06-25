// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("f1cf4779-1bf8-41f5-89ef-0de04dd641c2")]
    public interface IEnvironmentVariable
    {
        [DispId(1)]
        string Name
        {
            get;
        }
        [DispId(2)]
        string Value
        {
            get;
        }
    }
}
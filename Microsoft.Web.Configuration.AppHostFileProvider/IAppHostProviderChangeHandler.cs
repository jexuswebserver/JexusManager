// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("d1696cca-ad31-4e3d-a033-f6aadfef9b93")]
    public interface IAppHostProviderChangeHandler
    {
        void OnSectionChanges(string sectionName, string configPath);
    }
}

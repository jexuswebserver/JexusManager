// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Microsoft.ApplicationHost
{
    [ComVisible(true), Guid("142e5e0e-33d0-4902-934c-6a4d49a84506")]
    public interface ICustomFields
    {
        [DispId(1)]
        uint MaxCustomFieldLength
        {
            get;
        }
        [DispId(2)]
        IEnumCustomFields GetCustomFieldsEnumerator();
    }
}

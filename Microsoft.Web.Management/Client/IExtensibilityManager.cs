// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;

namespace Microsoft.Web.Management.Client
{
    public interface IExtensibilityManager
    {
        ICollection GetExtensions(
            Type extensionType
            );

        void RegisterExtension(
            Type extensionType,
            Object extension
            );
    }
}
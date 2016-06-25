// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Management.Client
{
    public interface IModuleChildPage
    {
        IModulePage ParentPage { get; set; }
    }
}
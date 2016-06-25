// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Client
{
    public interface IPropertyEditingService
    {
        bool EditProperties(
            IServiceProvider serviceProvider,
            IPropertyEditingUser user,
            Object targetObject,
            string titleText,
            string helpText
            );

        bool EditProperties(
            IServiceProvider serviceProvider,
            IPropertyEditingUser user,
            Object targetObject,
            string titleText,
            string helpText,
            EventHandler showHelp
            );
    }
}
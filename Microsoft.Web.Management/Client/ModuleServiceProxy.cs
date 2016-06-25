// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Resources;

namespace Microsoft.Web.Management.Client
{
    public abstract class ModuleServiceProxy
    {
        public static string GetErrorInformation(
            Exception ex,
            ResourceManager resourceManager,
            out string errorText,
            out string errorMessage
            )
        {
            errorText = string.Empty;
            errorMessage = string.Empty;
            return null;
        }

        protected Object Invoke(
            string methodName,
            params Object[] parameters
            )
        {
            return null;
        }
    }
}
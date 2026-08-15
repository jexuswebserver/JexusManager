// Copyright (c) Lex Li. All rights reserved.
//
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Server
{
    public sealed class ModuleServiceException : Exception
    {
        public ModuleServiceException(string message, string errorCode = null, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; }
    }
}

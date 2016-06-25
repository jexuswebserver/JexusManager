// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Web.Management.Server
{
    [Serializable]
    public sealed class WebManagementServiceException : Exception
    {
        public WebManagementServiceException() { }

        public WebManagementServiceException(
            string errorMessage
            )
        { }

        public WebManagementServiceException(
            string errorMessage,
            Exception exception
            )
        {
        }

        public WebManagementServiceException(
            string resourceName,
            string errorMessage
            )
        {
        }

        public WebManagementServiceException(
            string resourceName,
            string errorMessage,
            Exception innerException
            )
        { }

        public override string ToString()
        {
            return null;
        }

        public string ResourceName { get; }
    }
}
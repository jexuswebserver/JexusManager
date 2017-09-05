// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Microsoft.Web.Administration
{
    [Serializable]
    public sealed class ServerManagerException : Exception,
        ISerializable
    {
        public ServerManagerException()
        { }

        public ServerManagerException(
    string errorMessage
) : base(errorMessage)
        { }

        public ServerManagerException(
    string errorMessage,
    Exception exception
) : base(errorMessage, exception)
        { }
        public ServerManagerException(
    string errorMessage,
    int errorCode
) : base(errorMessage)
        {
            ErrorCode = errorCode;
        }

        public ServerManagerException(
    string errorMessage,
    Exception exception,
    int errorCode
) : base(errorMessage, exception)
        {
            ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return null;
        }

        public int ErrorCode { get; private set; }
    }
}

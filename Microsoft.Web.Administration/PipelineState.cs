// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public enum PipelineState
    {
        Unknown = 0,
        BeginRequest = 1,
        AuthenticateRequest = 2,
        AuthorizeRequest = 4,
        ResolveRequestCache = 8,
        MapRequestHandler = 16,
        AcquireRequestState = 32,
        PreExecuteRequestHandler = 64,
        ExecuteRequestHAndler = 128,
        ReleaseRequestState = 256,
        UpdateRequestCache = 512,
        LogRequest = 1024,
        EndRequest = 2048,
        SendResponse = 536870912
    }
}

// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class Request : ConfigurationElement
    {
        internal Request()
            : base(null, null, null, null, null, null)
        { }

        public string ClientIPAddr { get; private set; }
        public string ConnectionId { get; private set; }
        public string CurrentModule { get; private set; }
        public string HostName { get; private set; }
        public string LocalIPAddress { get; private set; }
        public int LocalPort { get; private set; }
        public PipelineState PipelineState { get; private set; }
        public int ProcessId { get; private set; }
        public string RequestId { get; private set; }
        public int SiteId { get; private set; }
        public int TimeElapsed { get; private set; }
        public int TimeInModule { get; private set; }
        public int TimeInState { get; private set; }
        public string Url { get; private set; }
        public string Verb { get; private set; }
    }
}

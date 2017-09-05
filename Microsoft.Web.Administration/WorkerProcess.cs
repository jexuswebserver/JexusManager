// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class WorkerProcess : ConfigurationElement
    {
        internal WorkerProcess(ConfigurationElement parent)
            : base(null, "workerProcess", null, parent, null, null)
        { }

        public RequestCollection GetRequests(int timeElapsedFilter)
        {
            return null;
        }

        public ApplicationDomainCollection ApplicationDomains { get; private set; }

        public string AppPoolName { get; private set; }

        public string ProcessGuid { get; private set; }

        public int ProcessId { get; private set; }

        public WorkerProcessState State { get; private set; }
    }
}

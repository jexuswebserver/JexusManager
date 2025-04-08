// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;

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

        public string AppPoolName
        {
            get { return (string)this["appPoolName"]; }
            set { this["appPoolName"] = value; }
        }

        public string ProcessGuid
        {
            get { return (string)this["guid"]; }
            set { this["guid"] = value; }
        }

        public int ProcessId
        {
            get { return Convert.ToInt32(this["processId"]); }
            set { this["processId"] = Convert.ToUInt32(value); }
        }

        public WorkerProcessState State
        {
            get
            {
                var found = Process.GetProcessById(ProcessId);
                if (found == null)
                {
                    ParentElement.GetCollection().Remove(this);
                    return WorkerProcessState.Stopping;
                }

                return WorkerProcessState.Running;
            }
        }
    }
}

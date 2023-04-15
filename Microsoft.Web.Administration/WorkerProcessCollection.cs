// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class WorkerProcessCollection : ConfigurationElementCollectionBase<WorkerProcess>
    {
        internal WorkerProcessCollection(ConfigurationElement collection)
            : base(null, "workProcesses", collection.Schema, null, null, null)
        { }

        internal WorkerProcessCollection(ConfigurationElement collection, ServerManager server)
            : base(collection, "workProcesses", collection?.Schema, null, collection?.InnerEntity, null)
        {
            Section = collection?.Section;
            if (collection == null)
            {
                return;
            }
        }

        protected override WorkerProcess CreateNewElement(string elementTagName)
        {
            var result = new WorkerProcess(this);
            Add(result);
            return result;
        }

        public WorkerProcess GetWorkerProcess(int processId)
        {
            foreach (WorkerProcess item in ChildElements)
            {
                if (item.ProcessId == processId)
                {
                    return item;
                }
            }

            return null;
        }
    }
}

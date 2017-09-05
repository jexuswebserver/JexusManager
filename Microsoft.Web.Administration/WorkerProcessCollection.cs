// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Web.Administration
{
    public sealed class WorkerProcessCollection : ConfigurationElementCollectionBase<WorkerProcess>
    {
        internal WorkerProcessCollection()
        { }

        protected override WorkerProcess CreateNewElement(string elementTagName)
        {
            return null;
        }

        public WorkerProcess GetWorkerProcess(int processId)
        {
            return null;
        }
    }
}
